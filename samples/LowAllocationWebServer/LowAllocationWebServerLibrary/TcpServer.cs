// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Net.Interop;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Net.Sockets
{
    public struct TcpServer
    {
        IntPtr _handle;
        public TcpServer(ushort port, byte address1, byte address2, byte address3, byte address4)
        {
            var version = new TcpServer.Version(2, 2);
            int result = SocketImports.WSAStartup((short)version.Raw, out WSDATA data);
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
        public TcpConnection(IntPtr handle)
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

        public unsafe int Send(ReadOnlySpan<byte> buffer)
        {
            // TODO: This can work with Span<byte> because it's synchronous but we need async pinning support
            fixed (byte* bytes = &MemoryMarshal.GetReference(buffer))
            {
                IntPtr pointer = new IntPtr(bytes);
                return SendPinned(pointer, buffer.Length);
            }
        }

        public int Send(ReadOnlyMemory<byte> buffer)
        {
            return Send(buffer.Span);
        }

        public int Send(ArraySegment<byte> buffer)
        {
            return Send(buffer.Array, buffer.Offset, buffer.Count);
        }

        public int Send(byte[] bytes, int offset, int count)
        {
            unsafe {
                fixed (byte* buffer = bytes) {
                    IntPtr pointer = new IntPtr(buffer + offset);
                    return SendPinned(pointer, count);
                }
            }
        }

        public unsafe int Receive(Span<byte> buffer)
        {
            // TODO: This can work with Span<byte> because it's synchronous but we need async pinning support
            fixed (byte* bytes = &MemoryMarshal.GetReference(buffer))
            {
                IntPtr pointer = new IntPtr(bytes);
                return ReceivePinned(pointer, buffer.Length);
            }
        }

        public int Receive(Memory<byte> buffer)
        {
            return Receive(buffer.Span);
        }

        public int Receive(ArraySegment<byte> buffer)
        {
            return Receive(buffer.Array, buffer.Offset, buffer.Count);
        }

        public int Receive(byte[] array, int offset, int count)
        {
            unsafe {
                fixed (byte* buffer = array) {
                    IntPtr pointer = new IntPtr(buffer + offset);
                    return ReceivePinned(pointer, count);
                }
            }
        }

        private unsafe int SendPinned(IntPtr buffer, int length)
        {
            int bytesSent = SocketImports.send(_handle, buffer, length, 0);
            if (bytesSent == SocketImports.SOCKET_ERROR) {
                var error = SocketImports.WSAGetLastError();
                throw new Exception(String.Format("send failed with {0}", error));
            }
            return bytesSent;
        }

        private int ReceivePinned(IntPtr ptr, int length)
        {
            int bytesReceived = SocketImports.recv(Handle, ptr, length, 0);
            if (bytesReceived < 0) {
                var error = SocketImports.WSAGetLastError();
                throw new Exception(String.Format("receive failed with {0}", error));
            }
            return bytesReceived;
        }
    }
}
