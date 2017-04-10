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

    }
}