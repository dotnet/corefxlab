// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public abstract class PipeWriter : IPipeWriter
    {
        private readonly Pipe _pipe;

        public PipeWriter(IBufferPool pool)
        {
            _pipe = new Pipe(pool);

            Consume(_pipe);
        }

        protected abstract Task WriteAsync(ReadableBuffer buffer);

        public WritableBuffer Alloc(int minimumSize = 0) => _pipe.Alloc(minimumSize);

        public void Complete(Exception exception = null) => _pipe.CompleteWriter(exception);

        private async void Consume(IPipeReader input)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var buffer = result.Buffer;

                try
                {
                    if (buffer.IsEmpty && result.IsCompleted)
                    {
                        break;
                    }

                    await WriteAsync(buffer);
                }
                finally
                {
                    input.Advance(buffer.End);
                }
            }

            input.Complete();
        }

        public Task ReadingStarted => _pipe.Writer.ReadingStarted;
    }
}
