// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class FlushAsyncCompletionTests: PipeTest
    {
        [Fact]
        public void AwaitingFlushAsyncAwaitableTwiceCompletesReaderWithException()
        {
            async Task Await(ValueAwaiter<FlushResult> a) => await a;

            var writeBuffer = Pipe.Writer;
            writeBuffer.Write(new byte[MaximumSizeHigh]);
            var awaitable = writeBuffer.FlushAsync();

            var task1 = Await(awaitable);
            var task2 = Await(awaitable);

            Assert.Equal(true, task1.IsCompleted);
            Assert.Equal(true, task1.IsFaulted);
            Assert.Equal("Concurrent reads or writes are not supported.", task1.Exception.InnerExceptions[0].Message);

            Assert.Equal(true, task2.IsCompleted);
            Assert.Equal(true, task2.IsFaulted);
            Assert.Equal("Concurrent reads or writes are not supported.", task2.Exception.InnerExceptions[0].Message);
        }
    }
}
