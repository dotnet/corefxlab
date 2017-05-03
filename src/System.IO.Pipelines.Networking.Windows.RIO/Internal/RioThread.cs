// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines.Networking.Windows.RIO.Internal.Winsock;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO.Pipelines.Networking.Windows.RIO.Internal
{
    internal unsafe class RioThread
    {
        const int maxResults = 1024;
        const string Kernel_32 = "Kernel32";
        const long INVALID_HANDLE_VALUE = -1;

        private readonly RegisteredIO _rio;
        private readonly int _id;
        private readonly IntPtr _completionPort;
        private readonly IntPtr _completionQueue;
        private readonly Thread _completionThread;
        private readonly object _notify;
        private readonly Thread _notifyThread;
        private readonly CancellationToken _token;
        private readonly Queue<NotifyBatch> _notifyBatches;
        private readonly Queue<NotifyBatch> _processedBatches;

        private PipeFactory _factory;
        private Dictionary<long, RioTcpConnection> _connections;
        private List<BufferMapping> _bufferIdMappings;

        public IntPtr ReceiveCompletionQueue => _completionQueue;

        public IntPtr SendCompletionQueue => _completionQueue;

        public IntPtr CompletionPort => _completionPort;

        public PipeFactory PipeFactory => _factory;

        public RioThread(int id, CancellationToken token, IntPtr completionPort, IntPtr completionQueue, RegisteredIO rio)
        {
            _id = id;
            _rio = rio;
            _token = token;

            if (CpuInfo.LogicalProcessorCount > CpuInfo.PhysicalCoreCount)
            {
                _completionThread = new Thread(RunLogicalCompletions)
                {
                    Name = $"RIO Completion Thread {id:00}",
                    IsBackground = true
                };

                _notifyBatches = new Queue<NotifyBatch>(16);
                _notify = new object();

                _notifyThread = new Thread(RunNotifies)
                {
                    Name = $"RIO Notify Thread {id:00}",
                    IsBackground = true
                };
                _processedBatches = new Queue<NotifyBatch>(16);
            }
            else
            {
                _completionThread = new Thread(RunPhysicalCompletions)
                {
                    Name = $"RIO Completion Thread {id:00}",
                    IsBackground = true
                };
            }

            _completionPort = completionPort;
            _completionQueue = completionQueue;
        }

        public void AddConnection(long key, RioTcpConnection value)
        {
            lock (_connections)
            {
                _connections.Add(key, value);
            }
        }

        public void RemoveConnection(long key)
        {
            lock (_connections)
            {
                _connections.Remove(key);
            }
        }

        private IntPtr GetBufferId(IntPtr address, out long startAddress)
        {
            var id = IntPtr.Zero;
            startAddress = 0;

            lock (_bufferIdMappings)
            {
                var addressLong = address.ToInt64();

                // Can binary search if it's too slow
                foreach (var mapping in _bufferIdMappings)
                {
                    if (addressLong >= mapping.Start && addressLong <= mapping.End)
                    {
                        id = mapping.Id;
                        startAddress = mapping.Start;
                        break;
                    }
                }
            }

            return id;
        }

        public unsafe RioBufferSegment GetSegmentFromMemory(Buffer<byte> memory)
        {
            // It's ok to unpin the handle here because the memory is from the pool
            // we created, which is already pinned.
            var pin = memory.Pin();
            var spanPtr = (IntPtr)pin.PinnedPointer;
            pin.Dispose();

            long startAddress;
            long spanAddress = spanPtr.ToInt64();
            var bufferId = GetBufferId(spanPtr, out startAddress);
            
            checked
            {
                var offset = (uint)(spanAddress - startAddress);
                return new RioBufferSegment(bufferId, offset, (uint)memory.Length);
            }
        }

        private void OnSlabAllocated(MemoryPoolSlab slab)
        {
            lock (_bufferIdMappings)
            {
                var memoryPtr = slab.NativePointer;
                var bufferId = _rio.RioRegisterBuffer(memoryPtr, (uint)slab.Length);
                var addressLong = memoryPtr.ToInt64();

                _bufferIdMappings.Add(new BufferMapping
                {
                    Id = bufferId,
                    Start = addressLong,
                    End = addressLong + slab.Length
                });
            }
        }

        private void OnSlabDeallocated(MemoryPoolSlab slab)
        {
            var memoryPtr = slab.NativePointer;
            var addressLong = memoryPtr.ToInt64();

            lock (_bufferIdMappings)
            {
                for (int i = _bufferIdMappings.Count - 1; i >= 0; i--)
                {
                    if (addressLong == _bufferIdMappings[i].Start)
                    {
                        _bufferIdMappings.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Start()
        {
            _completionThread.Start(this);
            _notifyThread?.Start(this);
        }

        private static void RunLogicalCompletions(object state)
        {

            var thread = ((RioThread)state);
#if NET451
            Thread.BeginThreadAffinity();
#endif
            var nativeThread = GetCurrentThread();
            var affinity = GetAffinity(thread._id);
            nativeThread.ProcessorAffinity = new IntPtr((long)affinity);

            thread._connections = new Dictionary<long, RioTcpConnection>();
            thread._bufferIdMappings = new List<BufferMapping>();

            var memoryPool = new MemoryPool();
            memoryPool.RegisterSlabAllocationCallback((slab) => thread.OnSlabAllocated(slab));
            memoryPool.RegisterSlabDeallocationCallback((slab) => thread.OnSlabDeallocated(slab));
            thread._factory = new PipeFactory(memoryPool);

            thread.ProcessLogicalCompletions();

#if NET451
            Thread.EndThreadAffinity();
#endif
        }

        private static void RunPhysicalCompletions(object state)
        {

            var thread = ((RioThread)state);
#if NET451
            Thread.BeginThreadAffinity();
#endif
            var nativeThread = GetCurrentThread();
            var affinity = GetAffinity(thread._id);
            nativeThread.ProcessorAffinity = new IntPtr((long)affinity);

            thread._connections = new Dictionary<long, RioTcpConnection>();
            thread._bufferIdMappings = new List<BufferMapping>();

            var memoryPool = new MemoryPool();
            memoryPool.RegisterSlabAllocationCallback((slab) => thread.OnSlabAllocated(slab));
            memoryPool.RegisterSlabDeallocationCallback((slab) => thread.OnSlabDeallocated(slab));
            thread._factory = new PipeFactory(memoryPool);

            thread.ProcessPhysicalCompletions();

#if NET451
            Thread.EndThreadAffinity();
#endif
        }

        private static void RunNotifies(object state)
        {

            var thread = ((RioThread)state);
#if NET451
            Thread.BeginThreadAffinity();
#endif
            var nativeThread = GetCurrentThread();
            var affinity = GetPairedAffinity(thread._id);
            nativeThread.ProcessorAffinity = new IntPtr((long)affinity);

            thread.ProcessNotifies();
#if NET451
            Thread.EndThreadAffinity();
#endif
        }

        private struct NotifyBatch
        {
            public RioTcpConnection[] ConnectionsToSignal;
            public uint Count;
        }

        private void ProcessNotifies()
        {
            while (!_token.IsCancellationRequested)
            {
                NotifyBatch batch;
                lock (_notify)
                {
                    if (_notifyBatches.Count == 0)
                    {
                        Monitor.Wait(_notify);
                    }

                    batch = _notifyBatches.Dequeue();
                }

                var count = batch.Count;
                var connectionsToSignal = batch.ConnectionsToSignal;

                Notify(connectionsToSignal, count);

                lock (_processedBatches)
                {
                    _processedBatches.Enqueue(batch);
                }
            }
        }

        private static void Notify(RioTcpConnection[] connectionsToSignal, uint count)
        {
            for (var i = 0; i < connectionsToSignal.Length; i++)
            {
                if (i >= count)
                {
                    break;
                }

                var connection = connectionsToSignal[i];

                if (connection != null)
                {
                    connection.ReceiveEndComplete();
                    connectionsToSignal[i] = null;
                }
            }
        }


        private void ProcessLogicalCompletions()
        {
            RioRequestResult* results = stackalloc RioRequestResult[maxResults];

            _rio.Notify(ReceiveCompletionQueue);
            while (!_token.IsCancellationRequested)
            {
                NativeOverlapped* overlapped;
                uint bytes, key;
                var success = GetQueuedCompletionStatus(CompletionPort, out bytes, out key, out overlapped, -1);
                if (success)
                {
                    var activatedNotify = false;
                    while (true)
                    {
                        var count = _rio.DequeueCompletion(ReceiveCompletionQueue, (IntPtr)results, maxResults);
                        if (count == 0)
                        {
                            if (!activatedNotify)
                            {
                                activatedNotify = true;
                                _rio.Notify(ReceiveCompletionQueue);
                                continue;
                            }

                            break;
                        }

                        var gotBatch = false;
                        var batch = default(NotifyBatch);
                        lock (_processedBatches)
                        {
                            if (_processedBatches.Count > 0)
                            {
                                batch = _processedBatches.Dequeue();
                                batch.Count = count;
                                gotBatch = true;
                            }
                        }

                        if (!gotBatch)
                        {
                            batch = new NotifyBatch()
                            {
                                ConnectionsToSignal = new RioTcpConnection[maxResults],
                                Count = count
                            };
                        }

                        var connectionsToSignal = batch.ConnectionsToSignal;

                        Complete(results, count, connectionsToSignal);

                        lock (_notify)
                        {
                            _notifyBatches.Enqueue(batch);

                            Monitor.Pulse(_notify);
                        }

                        if (!activatedNotify)
                        {
                            activatedNotify = true;
                            _rio.Notify(ReceiveCompletionQueue);

                        }
                    }
                }
                else
                {
                    var error = GetLastError();
                    if (error != 258)
                    {
                        throw new Exception($"ERROR: GetQueuedCompletionStatusEx returned {error}");
                    }
                }
            }
        }

        private unsafe void Complete(RioRequestResult* results, uint count, RioTcpConnection[] connectionsToSignal)
        {
            for (var i = 0; i < count; i++)
            {
                var result = results[i];

                RioTcpConnection connection;
                bool found;
                lock (_connections)
                {
                    found = _connections.TryGetValue(result.ConnectionCorrelation, out connection);
                }

                if (found)
                {
                    if (result.RequestCorrelation >= 0)
                    {
                        connection.ReceiveBeginComplete(result.BytesTransferred);
                        connectionsToSignal[i] = connection;
                    }
                    else
                    {
                        connection.SendComplete(result.RequestCorrelation);
                        connectionsToSignal[i] = null;
                    }
                }
                else
                {
                    connectionsToSignal[i] = null;
                }
            }
        }

        private void ProcessPhysicalCompletions()
        {
            RioRequestResult* results = stackalloc RioRequestResult[maxResults];
            var connectionsToSignal = new RioTcpConnection[maxResults];

            _rio.Notify(ReceiveCompletionQueue);
            while (!_token.IsCancellationRequested)
            {
                NativeOverlapped* overlapped;
                uint bytes, key;
                var success = GetQueuedCompletionStatus(CompletionPort, out bytes, out key, out overlapped, -1);
                if (success)
                {
                    var activatedNotify = false;
                    while (true)
                    {
                        var count = _rio.DequeueCompletion(ReceiveCompletionQueue, (IntPtr)results, maxResults);
                        if (count == 0)
                        {
                            if (!activatedNotify)
                            {
                                activatedNotify = true;
                                _rio.Notify(ReceiveCompletionQueue);
                                continue;
                            }

                            break;
                        }

                        Complete(results, count, connectionsToSignal);

                        Notify(connectionsToSignal, count);

                        if (!activatedNotify)
                        {
                            activatedNotify = true;
                            _rio.Notify(ReceiveCompletionQueue);

                        }
                    }
                }
                else
                {
                    var error = GetLastError();
                    if (error != 258)
                    {
                        throw new Exception($"ERROR: GetQueuedCompletionStatusEx returned {error}");
                    }
                }
            }
        }

        [DllImport(Kernel_32, SetLastError = true)]
        private static extern bool GetQueuedCompletionStatus(IntPtr CompletionPort, out uint lpNumberOfBytes, out uint lpCompletionKey, out NativeOverlapped* lpOverlapped, int dwMilliseconds);

        [DllImport(Kernel_32, SetLastError = true)]
        private static extern long GetLastError();

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        private static ProcessThread GetCurrentThread()
        {
            var id = GetCurrentThreadId();
            var processThreads = Process.GetCurrentProcess().Threads;

            for (var i = 0; i < processThreads.Count; i++)
            {
                var thread = processThreads[i];
                if (thread.Id == id)
                {
                    return thread;
                }
            }

            return null;
        }

        private static ulong GetAffinity(int threadId)
        {
            const int lshift = sizeof(ulong) * 8 - 1;

            var bitMask = CpuInfo.PhysicalCoreMask;
            var coreId = 0;

            for (var i = 0; i <= lshift; i++)
            {
                var bitTest = 1UL << i;

                if ((bitMask & bitTest) == bitTest)
                {
                    if (coreId == threadId)
                    {
                        return bitTest;
                    }
                    coreId++;
                }
            }

            unchecked
            {
                return (ulong)-1;
            }
        }

        private static ulong GetPairedAffinity(int threadId)
        {
            const int lshift = sizeof(ulong) * 8 - 1;

            var bitMask = CpuInfo.SecondaryCoreMask;
            var coreId = 0;

            for (var i = 0; i <= lshift; i++)
            {
                var bitTest = 1UL << i;

                if ((bitMask & bitTest) == bitTest)
                {
                    if (coreId == threadId)
                    {
                        return bitTest;
                    }
                    coreId++;
                }
            }

            unchecked
            {
                return (ulong)-1;
            }
        }

        private struct BufferMapping
        {
            public IntPtr Id;
            public long Start;
            public long End;

            public override string ToString()
            {
                return $"{Id} ({Start}) - ({End})";
            }
        }
    }
}
