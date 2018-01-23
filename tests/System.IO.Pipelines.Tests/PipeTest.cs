// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines.Tests
{
    public abstract class PipeTest : IDisposable
    {
        protected const int MaximumSizeHigh = 65;

        protected Pipe Pipe;
        private readonly MemoryPool _pool;

        protected PipeTest()
        {
            _pool = new MemoryPool();
            Pipe = new ResetablePipe(new PipeOptions(_pool,
                maximumSizeHigh: 65,
                maximumSizeLow: 6
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
