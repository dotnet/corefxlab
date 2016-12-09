using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    using System.Net;

    unsafe public class UdpClient : UVHandle
    {
        public event Action<string,uint,Memory<byte>> ReceiveCompleted;

        #region static helpers
        static bool IsUnix
        {
            get
            {
                return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }
        static sockaddr_in CreateSockaddr(string ip, uint port)
        {
            sockaddr_in address;
            int result = UVInterop.uv_ip4_addr(ip, (int)port, out address);

            UVException.ThrowIfError(result);

            return address;
        }
        static sockaddr_in6 CreateSockaddrIp6(string ip, uint port)
        {
            sockaddr_in6 address;
            int result = UVInterop.uv_ip6_addr(ip,(int) port, out address);
            UVException.ThrowIfError(result);
            return address;
        }

        unsafe static string GetAddress(IntPtr handle, IntPtr pointer, bool map)
        {
            byte[] buffer = new byte[256];
            sockaddr_storage* sa = (sockaddr_storage*)pointer;
            if (sa->sin_family == 2)
            {
                sockaddr_in* sockaddr = (sockaddr_in*)pointer;
                UVException.ThrowIfError(
                        UVInterop.uv_ip4_name(
                                (IntPtr)sockaddr, buffer, (IntPtr)buffer.Length
                            )
                    );
            }
            else
            {
                sockaddr_in6* sockaddr = (sockaddr_in6*)pointer;
                UVException.ThrowIfError(
                        UVInterop.uv_ip4_name(
                                (IntPtr)sockaddr, buffer, (IntPtr)buffer.Length
                            )
                    );
            }
            var ipAddress = Text.Encoding.UTF8.GetString(buffer, 0, strlen(buffer));

            return ipAddress;
        }

        unsafe static uint GetPort(IntPtr handle, IntPtr pointer, bool map)
        {
            sockaddr_storage* sa = (sockaddr_storage*)pointer;
            return sa->sin_port;
        }

        static void Bind(UdpClient client,string ipAddress, int port, bool ip6 = false)
        {
            if (ip6)
            {
                sockaddr_in6 address = CreateSockaddrIp6(ipAddress.ToString(),(uint) port);
                var result = UVInterop.uv_udp_bind(
                  client.Handle, ref address, 4
                );
                UVException.ThrowIfError(result);
            }
            else
            {
                sockaddr_in address = CreateSockaddr(ipAddress.ToString(),(uint) port);
                var result = UVInterop.uv_udp_bind(
                    client.Handle, ref address, 4
                );
                UVException.ThrowIfError(result);
            }
        }

        static int strlen(byte[] bytes)
        {
            int i = 0;
            while(i < bytes.Length && bytes[i] != 0)
                i++;
            return i;
        }
        #endregion

        uv_udp_t* _udp;
        
        public UdpClient(string ipAddress, ushort port, UVLoop loop)
            : base(loop, HandleType.UV_UDP)
        {
            _udp = (uv_udp_t*)(Handle.ToInt64() + GetSize(HandleType.UV_HANDLE));

            UVException.ThrowIfError(
                UVInterop.uv_udp_init(
                    Loop.Handle, Handle
                )
            );

            Bind(this, ipAddress, port);
        }

        public void Listen()
        {
            EnsureNotDisposed();
            if (IsUnix)
            {
                UVException.ThrowIfError(
                    UVInterop.uv_udp_recv_start(
                        Handle, UVBuffer.AllocateUnixBuffer, ReceiveUnix
                    )
                );
            }
            else
            {
                UVException.ThrowIfError(
                    UVInterop.uv_udp_recv_start(
                        Handle, UVBuffer.AllocWindowsBuffer, ReceiveWindows
                    )
                );
            }
        }

        #region OnReceive & ReceiveCompleted
        void OnReceiveWindows(string ipAddress, uint ipPort, UVBuffer.Windows buffer, IntPtr bytesAvaliable)
        {
            long bytesRead = bytesAvaliable.ToInt64();
            if (bytesRead == 0)
            {
                buffer.Dispose();
                return;
            }
            else if (bytesRead < 0)
            {
                var error = UVException.ErrorCodeToError((int)bytesRead);
                if (error == UVError.EOF)
                {
                    Dispose();
                    buffer.Dispose();
                }
                else if (error == UVError.ECONNRESET)
                {
                    Debug.Assert(buffer.Buffer == IntPtr.Zero && buffer.Length == 0);
                }
                else
                {
                    Dispose();
                    buffer.Dispose();
                    throw new UVException((int)bytesRead);
                }
            }
            else
            {
                using (var owned = new OwnedNativeMemory((int)bytesRead, buffer.Buffer))
                {
                    OnReceiveCompleted(ipAddress,ipPort,owned.Memory);
                }
            }
        }

        void OnReceiveUnix(string ipAddress,uint ipPort,UVBuffer.Unix buffer, IntPtr bytesAvaliable)
        {
            long bytesRead = bytesAvaliable.ToInt64();
            if (bytesRead == 0)
            {
                return;
            }
            else if (bytesRead < 0)
            {
                var error = UVException.ErrorCodeToError((int)bytesRead);
                if (error == UVError.EOF)
                {
                    Dispose();
                }
                else
                {
                    Dispose();

                    throw new UVException((int)bytesRead);
                }
            }
            else
            {
                using (var owned = new OwnedNativeMemory((int)bytesRead, buffer.Buffer))
                {
                    OnReceiveCompleted(ipAddress, ipPort, owned.Memory);
                }
            }
        }

        void OnReceiveCompleted(string address, uint port, Memory<byte> bytesRead)
        {
            if (ReceiveCompleted != null)
            {
                ReceiveCompleted(address,port,bytesRead);
            }
        }

        #region static interop callback helpers
        static UVInterop.start_receive_callback_unix ReceiveUnix = OnReceiveUnix;
        static void OnReceiveUnix(IntPtr handle, IntPtr size, ref UVBuffer.Unix buffer, IntPtr sockaddr, int flags)
        {
            var _client = As<UdpClient>(handle);

            var ipAddress = GetAddress(handle, sockaddr, false);
            var ipPort = GetPort(handle, sockaddr, false);

            _client.OnReceiveUnix(ipAddress, ipPort, buffer, size);
        }

        static UVInterop.start_receive_callback_win ReceiveWindows = OnReceiveWindows;
        static void OnReceiveWindows(IntPtr handle, IntPtr size, ref UVBuffer.Windows buffer,IntPtr sockaddr, int flags)
        {
            var _client = As<UdpClient>(handle);

            var ipAddress = GetAddress(handle, sockaddr, false);
            var ipPort = GetPort(handle, sockaddr, false);

            _client.OnReceiveWindows(ipAddress,ipPort,buffer, size);
        }
        #endregion
        #endregion


    }
}
