// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineWriterExtensions
    {
        public static Task WriteAsync(this IPipeWriter output, Span<byte> source)
        {
            var writeBuffer = output.Alloc();
            writeBuffer.Write(source);
            return FlushAsync(writeBuffer);
        }

        private static Task FlushAsync(WritableBuffer writeBuffer)
        {
            var awaitable = writeBuffer.FlushAsync();
            if (awaitable.IsCompletedSuccessfully)
            {
                return TaskUtilities.CompletedTask;
            }

            // Failed results we pass to the awaited form so it can return
            // a faulted task rather than throwing directly
            return FlushAsyncAwaited(awaitable);
        }

        private static async Task FlushAsyncAwaited(WritableBufferAwaitable awaitable)
        {
            await awaitable;
        }
    }
}