// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class FlushAsyncCompletionTests: PipeTest
    {
        public async Task AwaitingFlushAsyncAwaitableTwiceThrows()
        {
            async Task Await(WritableBufferAwaitable a) => await a;

            var writeBuffer = Pipe.Writer.Alloc();
            writeBuffer.Write(new byte[MaximumSizeHigh]);
            var awaitable = writeBuffer.FlushAsync();

            var task1 = Await(awaitable);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Await(awaitable);
            });

            var result = await Pipe.Reader.ReadAsync();
            Pipe.Reader.Advance(result.Buffer.End);

            Assert.Equal("Concurrent reads or writes are not supported.", exception.Message);
            Assert.Equal(true, task1.IsCompleted);
            Assert.Equal(false, task1.IsFaulted);
        }
    }
}
