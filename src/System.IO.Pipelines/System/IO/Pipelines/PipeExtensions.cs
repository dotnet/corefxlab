// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineExtensions
    {
        private static readonly Task _completedTask = Task.FromResult(0);

        public static Task WriteAsync(this IPipeWriter output, ReadOnlyMemory<byte> source)
        {
            var writeBuffer = output.Alloc();
            writeBuffer.Write(source.Span);

            var awaitable = writeBuffer.FlushAsync();
            if (awaitable.IsCompleted)
            {
                awaitable.GetResult();
                return _completedTask;
            }

            return FlushAsyncAwaited(awaitable);
        }

        private static async Task FlushAsyncAwaited(ValueAwaiter<FlushResult> awaitable)
        {
            await awaitable;
        }
    }
}
