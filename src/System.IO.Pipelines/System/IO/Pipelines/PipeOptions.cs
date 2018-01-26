// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading;

namespace System.IO.Pipelines
{
    public class PipeOptions
    {
        public static PipeOptions Default { get; } = new PipeOptions();

        public PipeOptions(
            MemoryPool<byte> pool = null,
            PipeScheduler readerScheduler = null,
            PipeScheduler writerScheduler = null,
            long pauseWriterThreshold = 0,
            long resumeWriterThreshold = 0,
            int minimumSegmentSize = 2048)
        {
            Pool = pool ?? MemoryPool.Default;
            ReaderScheduler = readerScheduler;
            WriterScheduler = writerScheduler;
            PauseWriterThreshold = pauseWriterThreshold;
            ResumeWriterThreshold = resumeWriterThreshold;
            MinimumSegmentSize = minimumSegmentSize;
        }

        public long PauseWriterThreshold { get; }

        public long ResumeWriterThreshold { get; }

        public int MinimumSegmentSize { get; }

        public PipeScheduler WriterScheduler { get; }

        public PipeScheduler ReaderScheduler { get; }

        public MemoryPool<byte> Pool { get; }
    }
}
