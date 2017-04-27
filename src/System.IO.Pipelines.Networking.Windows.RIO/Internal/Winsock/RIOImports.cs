// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock
{
    public static class RioImports
    {
        const string Ws232 = "WS2_32.dll";

        static readonly IntPtr RioInvalidBufferid = (IntPtr)0xFFFFFFFF;


        const uint IocOut = 0x40000000;
        const uint IocIn = 0x80000000;
        const uint IOC_INOUT = IocIn | IocOut;
        const uint IocWs2 = 0x08000000;
        const uint IocVendor = 0x18000000;
        const uint SioGetMultipleExtensionFunctionPointer = IOC_INOUT | IocWs2 | 36;
          
        const int SioLoopbackFastPath =  -1744830448;// IOC_IN | IOC_WS2 | 16;
    
        const int TcpNodelay = 0x0001;
        const int IPPROTO_TCP = 6;

        public unsafe static RegisteredIO Initalize(IntPtr socket)
        {

            UInt32 dwBytes = 0;
            RioExtensionFunctionTable rio = new RioExtensionFunctionTable();
            Guid rioFunctionsTableId = new Guid("8509e081-96dd-4005-b165-9e2ee8c79e3f");


            int True = -1;

            int result = setsockopt(socket, IPPROTO_TCP, TcpNodelay, (char*)&True, 4);
            if (result != 0)
            {
                var error = WSAGetLastError();
                WSACleanup();
                throw new Exception($"ERROR: setsockopt TCP_NODELAY returned {error}");
            }

            result = WSAIoctlGeneral(socket, SioLoopbackFastPath, 
                                &True, 4, null, 0,
                                out dwBytes, IntPtr.Zero, IntPtr.Zero);

            if (result != 0)
            {
                var error = WSAGetLastError();
                WSACleanup();
                throw new Exception($"ERROR: WSAIoctl SIO_LOOPBACK_FAST_PATH returned {error}");
            }

            result = WSAIoctl(socket, SioGetMultipleExtensionFunctionPointer,
               ref rioFunctionsTableId, 16, ref rio,
               sizeof(RioExtensionFunctionTable),
               out dwBytes, IntPtr.Zero, IntPtr.Zero);
            
            if (result != 0)
            {
                var error = WSAGetLastError();
                WSACleanup();
                throw new Exception($"ERROR: RIOInitalize returned {error}");
            }

            var rioFunctions = new RegisteredIO();

            rioFunctions.RioRegisterBuffer = Marshal.GetDelegateForFunctionPointer<RioRegisterBuffer>(rio.RIORegisterBuffer);

            rioFunctions.RioCreateCompletionQueue = Marshal.GetDelegateForFunctionPointer<RioCreateCompletionQueue>(rio.RIOCreateCompletionQueue);

            rioFunctions.RioCreateRequestQueue = Marshal.GetDelegateForFunctionPointer<RioCreateRequestQueue>(rio.RIOCreateRequestQueue);
                
            rioFunctions.Notify = Marshal.GetDelegateForFunctionPointer<RioNotify>(rio.RIONotify);
            rioFunctions.DequeueCompletion = Marshal.GetDelegateForFunctionPointer<RioDequeueCompletion>(rio.RIODequeueCompletion);

            rioFunctions.RioReceive = Marshal.GetDelegateForFunctionPointer<RioReceive>(rio.RIOReceive);
            rioFunctions.Send = Marshal.GetDelegateForFunctionPointer<RioSend>(rio.RIOSend);

            rioFunctions.CloseCompletionQueue = Marshal.GetDelegateForFunctionPointer<RioCloseCompletionQueue>(rio.RIOCloseCompletionQueue);
            rioFunctions.DeregisterBuffer = Marshal.GetDelegateForFunctionPointer<RioDeregisterBuffer>(rio.RIODeregisterBuffer);
            rioFunctions.ResizeCompletionQueue = Marshal.GetDelegateForFunctionPointer<RioResizeCompletionQueue>(rio.RIOResizeCompletionQueue);
            rioFunctions.ResizeRequestQueue = Marshal.GetDelegateForFunctionPointer<RioResizeRequestQueue>(rio.RIOResizeRequestQueue);

            return rioFunctions;
        }
        
        [DllImport(Ws232, SetLastError = true)]
        private static extern int WSAIoctl(
          [In] IntPtr socket,
          [In] uint dwIoControlCode,
          [In] ref Guid lpvInBuffer,
          [In] uint cbInBuffer,
          [In, Out] ref RioExtensionFunctionTable lpvOutBuffer,
          [In] int cbOutBuffer,
          [Out] out uint lpcbBytesReturned,
          [In] IntPtr lpOverlapped,
          [In] IntPtr lpCompletionRoutine
        );

        [DllImport(Ws232, SetLastError = true, EntryPoint = "WSAIoctl")]
        private unsafe static extern int WSAIoctlGeneral(
          [In] IntPtr socket,
          [In] int dwIoControlCode,
          [In] int* lpvInBuffer,
          [In] uint cbInBuffer,
          [In] int* lpvOutBuffer,
          [In] int cbOutBuffer,
          [Out] out uint lpcbBytesReturned,
          [In] IntPtr lpOverlapped,
          [In] IntPtr lpCompletionRoutine
        );

        [DllImport(Ws232, SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = true, ThrowOnUnmappableChar = true)]
        internal static extern SocketError WSAStartup([In] short wVersionRequested, [Out] out WindowsSocketsData lpWindowsSocketsData );

        [DllImport(Ws232, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr WSASocket([In] AddressFamilies af, [In] SocketType type, [In] Protocol protocol, [In] IntPtr lpProtocolInfo, [In] Int32 group, [In] SocketFlags dwFlags );

        [DllImport(Ws232, SetLastError = true)]
        public static extern ushort htons([In] ushort hostshort);

        [DllImport(Ws232, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int bind(IntPtr s, ref SocketAddress name, int namelen);

        [DllImport(Ws232, SetLastError = true)]
        public static extern int listen(IntPtr s, int backlog);

        [DllImport(Ws232, SetLastError = true)]
        public unsafe static extern int setsockopt(IntPtr s, int level, int optname, char* optval, int optlen);

        [DllImport(Ws232, SetLastError = true)]
        public static extern IntPtr accept(IntPtr s, IntPtr addr, int addrlen);
        
        [DllImport(Ws232)]
        public static extern Int32 WSAGetLastError();

        [DllImport(Ws232, SetLastError = true)]
        public static extern Int32 WSACleanup();

        [DllImport(Ws232, SetLastError = true)]
        public static extern int closesocket(IntPtr s);

        public const int SocketError = -1;
        public const int InvalidSocket = -1;
    }
}
