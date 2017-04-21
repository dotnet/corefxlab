// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal
{
    internal class RioThreadPool
    {
        const string Kernel_32 = "Kernel32";
        const long INVALID_HANDLE_VALUE = -1;

        private RegisteredIO _rio;
        private CancellationToken _token;
        private int _maxThreads;

        private IntPtr _socket;
        private RioThread[] _rioThreads;
        
        public unsafe RioThreadPool(RegisteredIO rio, IntPtr socket, CancellationToken token)
        {
            _socket = socket;
            _rio = rio;
            _token = token;

            // Count non-HT cores only
            var procCount = CpuInfo.PhysicalCoreCount;
            // RSS only supports up to 16 cores
            _maxThreads = procCount > 16 ? 16 : procCount;

            _rioThreads = new RioThread[_maxThreads];
            for (var i = 0; i < _rioThreads.Length; i++)
            {
                IntPtr completionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, IntPtr.Zero, 0, 0);

                if (completionPort == IntPtr.Zero)
                {
                    var error = GetLastError();
                    RioImports.WSACleanup();
                    throw new Exception($"ERROR: CreateIoCompletionPort returned {error}");
                }

                var completionMethod = new NotificationCompletion
                {
                    Type = NotificationCompletionType.IocpCompletion,
                    Iocp = new NotificationCompletionIocp
                    {
                        IocpHandle = completionPort,
                        QueueCorrelation = (ulong) i,
                        Overlapped = (NativeOverlapped*) (-1) // nativeOverlapped
                    }
                };

                IntPtr completionQueue = _rio.RioCreateCompletionQueue(RioTcpServer.MaxOutsandingCompletionsPerThread,
                    completionMethod);

                if (completionQueue == IntPtr.Zero)
                {
                    var error = RioImports.WSAGetLastError();
                    RioImports.WSACleanup();
                    throw new Exception($"ERROR: RioCreateCompletionQueue returned {error}");
                }

                var thread = new RioThread(i, _token, completionPort, completionQueue, rio);
                _rioThreads[i] = thread;
            }

            for (var i = 0; i < _rioThreads.Length; i++)
            {
                var thread = _rioThreads[i];
                thread.Start();
                
                thread.Ready.Wait();
            }
        }

        internal RioThread GetThread(long connetionId)
        {
            return _rioThreads[(connetionId % _maxThreads)];
        }

        [DllImport(Kernel_32, SetLastError = true)]
        private static extern IntPtr CreateIoCompletionPort(long handle, IntPtr hExistingCompletionPort,
            int puiCompletionKey, uint uiNumberOfConcurrentThreads);

        [DllImport(Kernel_32, SetLastError = true)]
        private static extern long GetLastError();
    }
}
