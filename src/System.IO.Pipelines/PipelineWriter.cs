// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public abstract class PipelineWriter : IPipelineWriter
    {
        private readonly PipelineReaderWriter _output;

        public PipelineWriter(IBufferPool pool)
        {
            _output = new PipelineReaderWriter(pool);

            Consume(_output);
        }

        protected abstract Task WriteAsync(ReadableBuffer buffer);

        public Task Writing => _output.Writing;

        public WritableBuffer Alloc(int minimumSize = 0) => _output.Alloc(minimumSize);

        public void Complete(Exception exception = null) => _output.CompleteWriter(exception);

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
