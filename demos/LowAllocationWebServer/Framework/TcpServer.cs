using Microsoft.Net.Interop;
using System;

namespace Microsoft.Net.Sockets
{
    public struct TcpServer
    {
        IntPtr _handle;
        public TcpServer(ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            var version = new TcpServer.Version(2, 2);
            WSDATA data;
            int result = SocketImports.WSAStartup((short)version.Raw, out data);
            if (result != 0)
            {
                var error = SocketImports.WSAGetLastError();
                throw new Exception(String.Format("ERROR: WSAStartup returned {0}", error));
            }

            _handle = SocketImports.socket(ADDRESS_FAMILIES.AF_INET, SOCKET_TYPE.SOCK_STREAM, PROTOCOL.IPPROTO_TCP);
            if (_handle == IntPtr.Zero)
            {
                var error = SocketImports.WSAGetLastError();
                SocketImports.WSACleanup();
                throw new Exception(String.Format("ERROR: socket returned {0}", error));
            }

            Start(port, address1, address2, address3, address4);
        }

        private void Start(ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            // BIND
            in_addr inAddress = new in_addr();
            inAddress.s_b1 = address1;
            inAddress.s_b2 = address2;
            inAddress.s_b3 = address3;
            inAddress.s_b4 = address4;

            sockaddr_in sa = new sockaddr_in();
            sa.sin_family = ADDRESS_FAMILIES.AF_INET;
            sa.sin_port = SocketImports.htons(port);
            sa.sin_addr = inAddress;

            int result;
            unsafe
            {
                var size = sizeof(sockaddr_in);
                result = SocketImports.bind(_handle, ref sa, size);
            }
            if (result == SocketImports.SOCKET_ERROR)
            {
                SocketImports.WSACleanup();
                throw new Exception("bind failed");
            }

            // LISTEN
            result = SocketImports.listen(_handle, 10);
            if (result == SocketImports.SOCKET_ERROR)
            {
                SocketImports.WSACleanup();
                throw new Exception("listen failed");
            }
        }

        public TcpConnection Accept()
        {
            IntPtr accepted = SocketImports.accept(_handle, IntPtr.Zero, 0);
            if (accepted == new IntPtr(-1))
            {
                var error = SocketImports.WSAGetLastError();
                SocketImports.WSACleanup();
                throw new Exception(String.Format("listen failed with {0}", error));
            }
            return new TcpConnection(accepted);
        }

        public void Stop()
        {
            SocketImports.WSACleanup();
        }

        public struct Version
        {
            public ushort Raw;

            public Version(byte major, byte minor)
            {
                Raw = major;
                Raw <<= 8;
                Raw += minor;
            }

            public byte Major
            {
                get
                {
                    UInt16 result = Raw;
                    result >>= 8;
                    return (byte)result;
                }
            }

            public byte Minor
            {
                get
                {
                    UInt16 result = Raw;
                    result &= 0x00FF;
                    return (byte)result;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}.{1}", Major, Minor);
            }
        }
    }

    public struct TcpConnection
    {
        IntPtr _handle;
        internal TcpConnection(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public int Receive(byte[] bytes)
        {
            unsafe
            {
                fixed (byte* buffer = bytes)
                {
                    IntPtr ptr = new IntPtr(buffer);
                    int bytesReceived = SocketImports.recv(Handle, ptr, bytes.Length, 0);
                    if (bytesReceived < 0)
                    {
                        var error = SocketImports.WSAGetLastError();
                        throw new Exception(String.Format("receive failed with {0}", error));
                    }
                    return bytesReceived;
                }
            }
        }

        public void Close()
        {
            SocketImports.closesocket(_handle);
        }

        public int Send(Span<byte> bytes)
        {
            unsafe
            {
                IntPtr ptr = new IntPtr(bytes.UnsafePointer);
                int bytesSent = SocketImports.send(_handle, ptr, bytes.Length, 0);
                if (bytesSent == SocketImports.SOCKET_ERROR)
                {
                    var error = SocketImports.WSAGetLastError();
                    throw new Exception(String.Format("send failed with {0}", error));
                }
                return bytesSent;
            }
        }

        public int Send(byte[] bytes, int count)
        {
            unsafe
            {
                fixed (byte* buffer = bytes)
                {
                    IntPtr ptr = new IntPtr(buffer);
                    int bytesSent = SocketImports.send(_handle, ptr, count, 0);
                    if (bytesSent == SocketImports.SOCKET_ERROR)
                    {
                        var error = SocketImports.WSAGetLastError();
                        throw new Exception(String.Format("send failed with {0}", error));
                    }
                    return bytesSent;
                }
            }
        }

        internal int Receive(Span<byte> buffer)
        {
            unsafe
            {
                IntPtr ptr = new IntPtr(buffer.UnsafePointer);
                int bytesReceived = SocketImports.recv(Handle, ptr, buffer.Length, 0);
                if (bytesReceived < 0) {
                    var error = SocketImports.WSAGetLastError();
                    throw new Exception(String.Format("receive failed with {0}", error));
                }
                return bytesReceived;
            }
        }
    }
}
