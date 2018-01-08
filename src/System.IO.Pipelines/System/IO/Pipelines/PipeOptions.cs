// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading;

namespace System.IO.Pipelines
{
    public class PipeOptions
    {
        public PipeOptions(
            MemoryPool<byte> pool,
            Scheduler readerScheduler = null,
            Scheduler writerScheduler = null,
            long maximumSizeHigh = 0,
            long maximumSizeLow = 0,
            int minimumSegmentSize = 2048)
        {
            Pool = pool;
            ReaderScheduler = readerScheduler;
            WriterScheduler = writerScheduler;
            MaximumSizeHigh = maximumSizeHigh;
            MaximumSizeLow = maximumSizeLow;
            MinimumSegmentSize = minimumSegmentSize;
        }

        public long MaximumSizeHigh { get; }

        public long MaximumSizeLow { get; }

        public int MinimumSegmentSize { get; }

        public Scheduler WriterScheduler { get; }

        public Scheduler ReaderScheduler { get; }

        public MemoryPool<byte> Pool { get; }
    }
}
