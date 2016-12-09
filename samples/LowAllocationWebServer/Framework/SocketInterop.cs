using System;
using System.Runtime.InteropServices;

namespace Microsoft.Net.Interop
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

        [DllImport(WS2_32)]
        public static extern int setsockopt(IntPtr s, SocketOptionLevel level, SocketOptionName optname, ref int optval, int optlen);

        public const int SOCKET_ERROR = -1;
        public const int INVALID_SOCKET = -1;
    }

    public enum SocketOptionLevel
    {
        IP = 0,
        IPv6 = 0x29,
        Socket = 0xffff,
        Tcp = 6,
        Udp = 0x11
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

    public enum SocketOptionName
    {
        AcceptConnection = 2,
        AddMembership = 12,
        AddSourceMembership = 15,
        BlockSource = 0x11,
        Broadcast = 0x20,
        BsdUrgent = 2,
        ChecksumCoverage = 20,
        Debug = 1,
        DontFragment = 14,
        DontLinger = -129,
        DontRoute = 0x10,
        DropMembership = 13,
        DropSourceMembership = 0x10,
        Error = 0x1007,
        ExclusiveAddressUse = -5,
        Expedited = 2,
        HeaderIncluded = 2,
        HopLimit = 0x15,
        IPOptions = 1,
        IPProtectionLevel = 0x17,
        IpTimeToLive = 4,
        IPv6Only = 0x1b,
        KeepAlive = 8,
        Linger = 0x80,
        MaxConnections = 0x7fffffff,
        MulticastInterface = 9,
        MulticastLoopback = 11,
        MulticastTimeToLive = 10,
        NoChecksum = 1,
        NoDelay = 1,
        OutOfBandInline = 0x100,
        PacketInformation = 0x13,
        ReceiveBuffer = 0x1002,
        ReceiveLowWater = 0x1004,
        ReceiveTimeout = 0x1006,
        ReuseAddress = 4,
        SendBuffer = 0x1001,
        SendLowWater = 0x1003,
        SendTimeout = 0x1005,
        Type = 0x1008,
        TypeOfService = 3,
        UnblockSource = 0x12,
        UpdateAcceptContext = 0x700b,
        UpdateConnectContext = 0x7010,
        UseLoopback = 0x40
    }
}
