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
            BufferPool bufferPool,
            IScheduler readerScheduler = null,
            IScheduler writerScheduler = null,
            long maximumSizeHigh = 0,
            long maximumSizeLow = 0)
        {
            BufferPool = bufferPool;
            ReaderScheduler = readerScheduler;
            WriterScheduler = writerScheduler;
            MaximumSizeHigh = maximumSizeHigh;
            MaximumSizeLow = maximumSizeLow;
        }

        public long MaximumSizeHigh { get; }

        public long MaximumSizeLow { get; }

        public IScheduler WriterScheduler { get; }

        public IScheduler ReaderScheduler { get; }

        public BufferPool BufferPool { get; }
    }
}
