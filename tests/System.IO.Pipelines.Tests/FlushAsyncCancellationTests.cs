// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class FlushAsyncCancellationTests : PipeTest
    {
        [Fact]
        public void GetResultThrowsIfFlushAsyncCancelledAfterOnCompleted()
        {
            var onCompletedCalled = false;
            var cancellationTokenSource = new CancellationTokenSource();
            var buffer = Pipe.Writer.Alloc(PipeTest.MaximumSizeHigh);
            buffer.Advance(PipeTest.MaximumSizeHigh);

            var awaiter = buffer.FlushAsync(cancellationTokenSource.Token);

            awaiter.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.Throws<OperationCanceledException>(() => awaiter.GetResult());
            });

            var awaiterIsCompleted = awaiter.IsCompleted;

            cancellationTokenSource.Cancel();

            Assert.False(awaiterIsCompleted);
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void GetResultThrowsIfFlushAsyncCancelledBeforeOnCompleted()
        {
            var onCompletedCalled = false;
            var cancellationTokenSource = new CancellationTokenSource();
            var buffer = Pipe.Writer.Alloc(PipeTest.MaximumSizeHigh);
            buffer.Advance(MaximumSizeHigh);

            var awaiter = buffer.FlushAsync(cancellationTokenSource.Token);
            var awaiterIsCompleted = awaiter.IsCompleted;

            cancellationTokenSource.Cancel();

            awaiter.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.Throws<OperationCanceledException>(() => awaiter.GetResult());
            });

            Assert.False(awaiterIsCompleted);
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void GetResultThrowsIfFlushAsyncTokenFiredAfterCancelPending()
        {
            var onCompletedCalled = false;
            var cancellationTokenSource = new CancellationTokenSource();
            var buffer = Pipe.Writer.Alloc(PipeTest.MaximumSizeHigh);
            buffer.Advance(MaximumSizeHigh);

            var awaiter = buffer.FlushAsync(cancellationTokenSource.Token);
            var awaiterIsCompleted = awaiter.IsCompleted;

            Pipe.Writer.CancelPendingFlush();
            cancellationTokenSource.Cancel();

            awaiter.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.Throws<OperationCanceledException>(() => awaiter.GetResult());
            });

            Assert.False(awaiterIsCompleted);
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void FlushAsyncThrowsIfPassedCancelledCancellationToken()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var buffer = Pipe.Writer.Alloc();

            Assert.Throws<OperationCanceledException>(() => buffer.FlushAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task FlushAsyncWithNewCancellationTokenNotAffectedByPrevious()
        {
            var cancellationTokenSource1 = new CancellationTokenSource();
            var buffer = Pipe.Writer.Alloc(10);
            buffer.Advance(10);
            await buffer.FlushAsync(cancellationTokenSource1.Token);

            cancellationTokenSource1.Cancel();

            var cancellationTokenSource2 = new CancellationTokenSource();
            buffer = Pipe.Writer.Alloc(10);
            buffer.Advance(10);
            // Verifying that ReadAsync does not throw
            await buffer.FlushAsync(cancellationTokenSource2.Token);
        }

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
        public void FlushAsyncNotCompletedAfterCancellationTokenCancelled()
        {
            bool onCompletedCalled = false;
            var cts = new CancellationTokenSource();
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);

            var awaitable = writableBuffer.FlushAsync(cts.Token);

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.True(awaitable.IsCompleted);

                Assert.Throws<OperationCanceledException>(() => awaitable.GetResult());

                awaitable = writableBuffer.FlushAsync();
                Assert.False(awaitable.IsCompleted);
            });

            cts.Cancel();
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

        [Fact]
        public void FlushAsyncReturnsIsCancelOnCancelPendingFlushBeforeGetResult()
        {
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);
            var awaitable = writableBuffer.FlushAsync();

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() => { });

            Pipe.Reader.Advance(Pipe.Reader.ReadAsync().GetResult().Buffer.End);
            Pipe.Writer.CancelPendingFlush();

            Assert.True(awaitable.IsCompleted);

            var result = awaitable.GetResult();
            Assert.True(result.IsCancelled);
        }

        [Fact]
        public void FlushAsyncReturnsIsCancelOnCancelPendingFlushAfterGetResult()
        {
            var writableBuffer = Pipe.Writer.Alloc(MaximumSizeHigh);
            writableBuffer.Advance(MaximumSizeHigh);
            var awaitable = writableBuffer.FlushAsync();

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() => { });

            Pipe.Writer.CancelPendingFlush();
            Pipe.Reader.Advance(Pipe.Reader.ReadAsync().GetResult().Buffer.End);

            Assert.True(awaitable.IsCompleted);

            var result = awaitable.GetResult();
            Assert.True(result.IsCancelled);
        }

        [Fact]
        public async Task FlushAsyncCancellationE2E()
        {
            var cts = new CancellationTokenSource();
            var cancelled = false;

            Func<Task> taskFunc = async () =>
            {
                WritableBuffer buffer;
                try
                {
                    buffer = Pipe.Writer.Alloc(MaximumSizeHigh);
                    buffer.Advance(MaximumSizeHigh);
                    await buffer.FlushAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    cancelled = true;
                    await buffer.FlushAsync();
                }
            };

            var task = taskFunc();

            cts.Cancel();

            var result = await Pipe.Reader.ReadAsync();
            Assert.Equal(new byte[MaximumSizeHigh], result.Buffer.ToArray());
            Pipe.Reader.Advance(result.Buffer.End);
            await task;
            Assert.True(cancelled);
        }

        [Fact]
        public void FlushAsyncCancellationDeadlock()
        {
            var cts = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();

            var buffer = Pipe.Writer.Alloc(MaximumSizeHigh);

            buffer.Advance(MaximumSizeHigh);

            var e = new ManualResetEventSlim();

            var awaiter = buffer.FlushAsync(cts.Token);
            awaiter.OnCompleted(() =>
            {
                // We are on cancellation thread and need to wait untill another FlushAsync call
                // takes pipe state lock
                e.Wait();

                // Make sure we had enough time to reach _cancellationTokenRegistration.Dispose
                Thread.Sleep(100);

                // Try to take pipe state lock
                buffer.FlushAsync();
            });

            // Start a thread that would run cancellation calbacks
            var cancellationTask = Task.Run(() => cts.Cancel());
            // Start a thread that would call FlushAsync with different token
            // and block on _cancellationTokenRegistration.Dispose
            var blockingTask = Task.Run(() =>
            {
                e.Set();
                buffer.FlushAsync(cts2.Token);
            });

            var completed = Task.WhenAll(cancellationTask, blockingTask).Wait(TimeSpan.FromSeconds(10));
            Assert.True(completed);
        }
    }
}