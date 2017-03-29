// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class FlushAsyncCancellationTests : PipeTest
    {
        [Fact]
        public void FlushAsyncReturnsCanceledIfFlushCancelled()
        {
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            Pipe.Writer.CancelPendingFlush();

            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.True(flushResult.IsCancelled);
        }

        [Fact]
        public void FlushAsyncReturnsCanceledIfCancelledBeforeFlush()
        {
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);

            Pipe.Writer.CancelPendingFlush();

            var flushAsync = writableBuffer.FlushAsync();

            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.True(flushResult.IsCancelled);
        }

        [Fact]
        public void FlushAsyncNotCompletedAfterCancellation()
        {
            bool onCompletedCalled = false;
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);

            var awaitable = writableBuffer.FlushAsync();

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.True(awaitable.IsCompleted);

                var flushResult = awaitable.GetResult();

                Assert.True(flushResult.IsCancelled);

                awaitable = writableBuffer.FlushAsync();
                Assert.False(awaitable.IsCompleted);
            });

            Pipe.Writer.CancelPendingFlush();
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void FlushAsyncCompletedAfterPreCancellation()
        {
            var writableBuffer = Pipe.Writer.Alloc(1);
            writableBuffer.Advance(1);

            Pipe.Writer.CancelPendingFlush();

            var flushAsync = writableBuffer.FlushAsync();

            Assert.True(flushAsync.IsCompleted);

            var flushResult = flushAsync.GetResult();

            Assert.True(flushResult.IsCancelled);

            flushAsync = writableBuffer.FlushAsync();

            Assert.True(flushAsync.IsCompleted);
        }
    }
}