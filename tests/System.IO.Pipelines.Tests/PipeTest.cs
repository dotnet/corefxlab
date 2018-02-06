// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.IO.Pipelines.Tests
{
    public abstract class PipeTest : IDisposable
    {
        protected const int MaximumSizeHigh = 65;

        private readonly TestMemoryPool _pool;

        protected Pipe Pipe;

        protected PipeTest(int pauseWriterThreshold = 65, int resumeWriterThreshold = 6)
        {
            _pool = new TestMemoryPool();
            Pipe = new Pipe(
                new PipeOptions(
                    _pool,
                    pauseWriterThreshold: pauseWriterThreshold,
                    resumeWriterThreshold: resumeWriterThreshold
                ));
        }

        public void Dispose()
        {
            Pipe.Writer.Complete();
            Pipe.Reader.Complete();
            _pool.Dispose();
        }
    }
}
