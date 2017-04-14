// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class ReadAsyncCancellationTests : PipeTest
    {
        [Fact]
        public void GetResultThrowsIfReadAsyncCancelledAfterOnCompleted()
        {
            var onCompletedCalled = false;
            var cancellationTokenSource = new CancellationTokenSource();

            var awaiter = Pipe.Reader.ReadAsync(cancellationTokenSource.Token);
            var awaiterIsCompleted = awaiter.IsCompleted;
            awaiter.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.Throws<OperationCanceledException>(() => awaiter.GetResult());
            });

            cancellationTokenSource.Cancel();

            Assert.False(awaiterIsCompleted);
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void GetResultThrowsIfReadAsyncCancelledBeforeOnCompleted()
        {
            var onCompletedCalled = false;
            var cancellationTokenSource = new CancellationTokenSource();

            var awaiter = Pipe.Reader.ReadAsync(cancellationTokenSource.Token);
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

            var awaiter = Pipe.Reader.ReadAsync(cancellationTokenSource.Token);
            var awaiterIsCompleted = awaiter.IsCompleted;

            cancellationTokenSource.Cancel();
            Pipe.Reader.CancelPendingRead();

            awaiter.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.Throws<OperationCanceledException>(() => awaiter.GetResult());
            });


            Assert.False(awaiterIsCompleted);
            Assert.True(onCompletedCalled);
        }


        [Fact]
        public void ReadAsyncThrowsIfPassedCancelledCancellationToken()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => Pipe.Reader.ReadAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task ReadAsyncWithNewCancellationTokenNotAffectedByPrevious()
        {
            await Pipe.Writer.WriteAsync(new byte[] {0});

            var cancellationTokenSource1 = new CancellationTokenSource();
            var result = await Pipe.Reader.ReadAsync(cancellationTokenSource1.Token);
            Pipe.Reader.Advance(result.Buffer.Start);

            cancellationTokenSource1.Cancel();
            var cancellationTokenSource2 = new CancellationTokenSource();

            // Verifying that ReadAsync does not throw
            result = await Pipe.Reader.ReadAsync(cancellationTokenSource2.Token);
            Pipe.Reader.Advance(result.Buffer.Start);
        }

        [Fact]
        public async Task CancellingPendingAfterReadAsync()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = Pipe.Writer.Alloc();
            output.Write(bytes);

            Func<Task> taskFunc = async () =>
            {
                var result = await Pipe.Reader.ReadAsync();
                var buffer = result.Buffer;
                Pipe.Reader.Advance(buffer.End);

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.True(buffer.IsEmpty);

                await output.FlushAsync();

                result = await Pipe.Reader.ReadAsync();
                buffer = result.Buffer;

                Assert.Equal(11, buffer.Length);
                Assert.True(buffer.IsSingleSpan);
                Assert.False(result.IsCancelled);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
                Pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                Pipe.Reader.Complete();
            };

            var task = taskFunc();

            Pipe.Reader.CancelPendingRead();

            await task;

            Pipe.Writer.Complete();
        }

        [Fact]
        public async Task WriteAndCancellingPendingReadBeforeReadAsync()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = Pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            Pipe.Reader.CancelPendingRead();

            var result = await Pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.False(buffer.IsEmpty);
            Assert.Equal(11, buffer.Length);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
            Pipe.Reader.Advance(buffer.End, buffer.End);
        }
        [Fact]
        public async Task CancellingPendingReadBeforeReadAsync()
        {
            Pipe.Reader.CancelPendingRead();

            var result = await Pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Pipe.Reader.Advance(buffer.End);

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.True(buffer.IsEmpty);

            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = Pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            result = await Pipe.Reader.ReadAsync();
            buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            Pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task CancellingBeforeAdvance()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = Pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            var result = await Pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            Pipe.Reader.CancelPendingRead();

            Pipe.Reader.Advance(buffer.End);

            var awaitable = Pipe.Reader.ReadAsync();

            Assert.True(awaitable.IsCompleted);

            result = await awaitable;

            Assert.True(result.IsCancelled);

            Pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public void ReadAsyncNotCompletedAfterCancellation()
        {
            bool onCompletedCalled = false;
            var awaitable = Pipe.Reader.ReadAsync();

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.True(awaitable.IsCompleted);

                var readResult = awaitable.GetResult();
                Assert.True(readResult.IsCancelled);

                awaitable = Pipe.Reader.ReadAsync();
                Assert.False(awaitable.IsCompleted);
            });

            Pipe.Reader.CancelPendingRead();
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void ReadAsyncNotCompletedAfterCancellationTokenCancelled()
        {
            bool onCompletedCalled = false;
            var cts = new CancellationTokenSource();
            var awaitable = Pipe.Reader.ReadAsync(cts.Token);

            Assert.False(awaitable.IsCompleted);
            awaitable.OnCompleted(() =>
            {
                onCompletedCalled = true;
                Assert.True(awaitable.IsCompleted);

                Assert.Throws<OperationCanceledException>(() => awaitable.GetResult());

                awaitable = Pipe.Reader.ReadAsync();
                Assert.False(awaitable.IsCompleted);
            });

            cts.Cancel();
            Assert.True(onCompletedCalled);
        }

        [Fact]
        public void ReadAsyncCompletedAfterPreCancellation()
        {
            Pipe.Reader.CancelPendingRead();
            Pipe.Writer.WriteAsync(new byte[] {1, 2, 3}).GetAwaiter().GetResult();

            var awaitable = Pipe.Reader.ReadAsync();

            Assert.True(awaitable.IsCompleted);

            var result = awaitable.GetResult();

            Assert.True(result.IsCancelled);

            awaitable = Pipe.Reader.ReadAsync();

            Assert.True(awaitable.IsCompleted);

            Pipe.Reader.Advance(awaitable.GetResult().Buffer.End);
        }

        [Fact]
        public async Task ReadingCanBeCancelled()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() =>
            {
                Pipe.Writer.Complete(new OperationCanceledException(cts.Token));
            });

            var ignore = Task.Run(async () =>
            {
                await Task.Delay(1000);
                cts.Cancel();
            });

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                var result = await Pipe.Reader.ReadAsync();
                var buffer = result.Buffer;
            });
        }

        [Fact]
        public async Task AdvanceShouldResetStateIfReadCancelled()
        {
            Pipe.Reader.CancelPendingRead();

            var result = await Pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Pipe.Reader.Advance(buffer.End);

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.True(buffer.IsEmpty);

            var awaitable = Pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }

        [Fact]
        public async Task ReadAsyncCancellationE2E()
        {
            var cts = new CancellationTokenSource();
            var e = new AutoResetEvent(false);
            var cancelled = false;

            Func<Task> taskFunc = async () =>
            {
                try
                {
                    var result = await Pipe.Reader.ReadAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    cancelled = true;
                    var result = await Pipe.Reader.ReadAsync();
                    Assert.Equal(new byte[] { 1, 2, 3 }, result.Buffer.ToArray());
                    Pipe.Reader.Advance(result.Buffer.End);
                }
            };

            var task = taskFunc();

            cts.Cancel();

            await Pipe.Writer.WriteAsync(new byte[] { 1, 2, 3 });
            await task;
            Assert.True(cancelled);
        }

        [Fact]
        public void FlushAsyncCancellationDeadlock()
        {
            var cts = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            var e = new ManualResetEventSlim();

            var awaiter = Pipe.Reader.ReadAsync(cts.Token);
            awaiter.OnCompleted(() =>
            {
                // We are on cancellation thread and need to wait untill another ReadAsync call
                // takes pipe state lock
                e.Wait();
                // Make sure we had enough time to reach _cancellationTokenRegistration.Dispose
                Thread.Sleep(100);
                // Try to take pipe state lock
                Pipe.Reader.ReadAsync();
            });

            // Start a thread that would run cancellation calbacks
            var cancellationTask = Task.Run(() => cts.Cancel());
            // Start a thread that would call ReadAsync with different token
            // and block on _cancellationTokenRegistration.Dispose
            var blockingTask = Task.Run(() =>
            {
                e.Set();
                Pipe.Reader.ReadAsync(cts2.Token);
            });

            var completed = Task.WhenAll(cancellationTask, blockingTask).Wait(TimeSpan.FromSeconds(10));
            Assert.True(completed);
        }
    }
}
