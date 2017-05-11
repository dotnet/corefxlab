// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class ReadAsyncCompletionTests: PipeTest
    {
        public async Task AwaitingReadAsyncAwaitableTwiceThrows()
        {
            async Task Await(ReadableBufferAwaitable a) => await a;

            var awaitable = Pipe.Reader.ReadAsync();
            var task1 = Await(awaitable);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Await(awaitable);
            });
            await Pipe.Writer.WriteAsync(new byte[] { });

            Assert.Equal("Concurrent reads or writes are not supported.", exception.Message);
            Assert.Equal(true, task1.IsCompleted);
            Assert.Equal(false, task1.IsFaulted);
        }
    }
}
