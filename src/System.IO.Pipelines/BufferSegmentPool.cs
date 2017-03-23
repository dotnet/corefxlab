// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    internal class BufferSegmentPool
    {
        [ThreadStatic]
        private static BufferSegment _threadCached;

        // Concurrent queue as contention with returners
        [ThreadStatic]
        private static ConcurrentQueue<BufferSegment> _pool;

        private ConcurrentQueue<BufferSegment> _localPool;

        private ConcurrentQueue<BufferSegment> Pool => _pool ?? CreateThreadStaticPool();

        // Rent from thread static pool
        private BufferSegment Rent()
        {
            var segment = _threadCached;
            if (segment != null)
            {
                _threadCached = null;
            }
            else
            {
                Pool.TryDequeue(out segment);
                if (segment == null)
                {
                    segment = new BufferSegment();
                }
            }

            segment.SourcePool = this;
            return segment;
        }

        internal BufferSegment Rent(OwnedBuffer<byte> buffer)
            => Rent().Initalize(buffer);

        internal BufferSegment Rent(OwnedBuffer<byte> buffer, int start, int end)
            => Rent().Initalize(buffer, start, end);

        // Return to local pool (thread static of Renting thread)
        public void Return(BufferSegment segment)
        {
            // Added returned segment to thread static keeps it hotter
            // and means it won't get reset and used by other threads if there is 
            // about to be a use after free by the current thread
            var currentSegment = _threadCached;
            segment.SourcePool = null;
            _threadCached = segment;

            if (currentSegment != null)
            {
                // Was a current segment in thread cache
                // Add this less recently used segment to queue if we have one
                _localPool?.Enqueue(currentSegment);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ConcurrentQueue<BufferSegment> CreateThreadStaticPool()
        {
            var pool = new ConcurrentQueue<BufferSegment>();
            _localPool = _pool =  pool;
            return pool;
        }
    }
}
