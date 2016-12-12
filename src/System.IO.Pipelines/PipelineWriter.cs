// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public abstract class PipelineWriter : IPipelineWriter
    {
        private readonly Pipe _pipe;

        public PipelineWriter(IBufferPool pool)
        {
            _pipe = new Pipe(pool);

            Consume(_pipe);
        }

        protected abstract Task WriteAsync(ReadableBuffer buffer);

        public Task Writing => _pipe.Writing;

        public WritableBuffer Alloc(int minimumSize = 0) => _pipe.Alloc(minimumSize);

        public void Complete(Exception exception = null) => _pipe.CompleteWriter(exception);

        private async void Consume(IPipelineReader input)
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
    }
}
