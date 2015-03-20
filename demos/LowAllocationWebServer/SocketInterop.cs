using System;
using System.Runtime.InteropServices;

namespace Microsoft.Net.Http.Server.Socket.Interop
{
    public static class SocketImports
    {
        const string WS2_32 = "ws2_32.dll";

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        internal static extern int WSAStartup([In] short wVersionRequested, [Out] out WSDATA lpWSAData);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr socket([In] ADDRESS_FAMILIES af, [In] SOCKET_TYPE socket_type, [In] PROTOCOL protocol);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern ushort htons([In] ushort hostshort);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int bind(IntPtr s, ref sockaddr_in name, int namelen);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int listen(IntPtr s, int backlog);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr accept(IntPtr s, IntPtr addr, int addrlen);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int recv(IntPtr s, IntPtr buf, int len, int flags);

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int send(IntPtr s, IntPtr buf, int len, int flags);

        [DllImport(WS2_32)]
        public static extern Int32 WSAGetLastError();

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern Int32 WSACleanup();

        [DllImport(WS2_32, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int closesocket(IntPtr s);

        public const int SOCKET_ERROR = -1;
        public const int INVALID_SOCKET = -1;
    }

    public enum ADDRESS_FAMILIES : short
    {
        AF_INET = 2,
    }

    public enum SOCKET_TYPE : short
    {
        SOCK_STREAM = 1,    
    }

    public enum PROTOCOL : short
    {   
        IPPROTO_TCP = 6,     
    }

    public unsafe struct WSDATA
    {
        public UInt16 wVersion;
        public UInt16 wHighVersion;
        public byte* szDescription;
        public byte* szSystemStatus;
        public UInt16 iMaxSockets; 
        public UInt16 iMaxUdpDg;   
        public byte* lpVendorInfo; 
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct sockaddr_in
    {
        public ADDRESS_FAMILIES sin_family;
        public ushort sin_port;
        public in_addr sin_addr;
        public fixed byte sin_zero[8];
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct in_addr
    {
        [FieldOffset(0)]
        public byte s_b1;
        [FieldOffset(1)]
        public byte s_b2;
        [FieldOffset(2)]
        public byte s_b3;
        [FieldOffset(3)]
        public byte s_b4;
    }
}
