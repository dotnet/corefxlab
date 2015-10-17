// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class TaskChannelTests : TestBase
    {
        [Fact]
        public void Completion_Idempotent()
        {
            var t = new TaskCompletionSource<int>().Task;
            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Assert.NotNull(c.Completion);
            Assert.NotSame(t, c.Completion);
            Assert.Same(c.Completion, c.Completion);
        }

        [Fact]
        public void Precancellation()
        {
            IReadableChannel<int> c = Channel.CreateFromTask(Task.FromResult(42));

            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.WaitToReadAsync(cts.Token), cts.Token);
            AssertSynchronouslyCanceled(c.ReadAsync(cts.Token).AsTask(), cts.Token);
        }

        [Fact]
        public void SuccessTask_BeforeTryRead_Success()
        {
            IReadableChannel<int> c = Channel.CreateFromTask(Task.FromResult(42));

            AssertSynchronousTrue(c.WaitToReadAsync());
            Assert.False(c.Completion.IsCompleted);

            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
            Assert.True(c.Completion.IsCompleted);

            AssertSynchronousFalse(c.WaitToReadAsync());

            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task SuccessTask_BeforeReadAsync_Success()
        {
            IReadableChannel<int> c = Channel.CreateFromTask(Task.FromResult(42));

            AssertSynchronousTrue(c.WaitToReadAsync());

            Task<int> read = c.ReadAsync().AsTask();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
            Assert.Equal(42, read.Result);

            AssertSynchronousFalse(c.WaitToReadAsync());

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());
        }

        [Fact]
        public async Task SuccessTask_AfterReadAsync_Success()
        {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            IReadableChannel<int> c = Channel.CreateFromTask(tcs.Task);

            int result;
            Assert.False(c.TryRead(out result));

            Task<int> read = c.ReadAsync().AsTask();
            Assert.False(read.IsCompleted);

            tcs.SetResult(42);
            Assert.Equal(42, await read);

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());
        }

        [Fact]
        public async Task SuccessTask_AfterWaitAsync_Success()
        {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            IReadableChannel<int> c = Channel.CreateFromTask(tcs.Task);

            Task<bool> read = c.WaitToReadAsync();
            Assert.False(read.IsCompleted);

            tcs.SetResult(42);
            Assert.True(await read);

            AssertSynchronousTrue(c.WaitToReadAsync());
        }

        [Fact]
        public async Task FaultedTask_BeforeCreation()
        {
            Task<int> t = Task.FromException<int>(new FormatException());

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Assert.Equal(TaskStatus.Faulted, c.Completion.Status);
            Assert.Same(t.Exception.InnerException, c.Completion.Exception.InnerException);

            AssertSynchronousFalse(c.WaitToReadAsync());
            await Assert.ThrowsAsync<FormatException>(() => c.ReadAsync().AsTask());

            int result;
            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CanceledTask_BeforeCreation()
        {
            Task<int> t = Task.FromCanceled<int>(new CancellationToken(true));

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Assert.Equal(TaskStatus.Canceled, c.Completion.Status);

            AssertSynchronousFalse(c.WaitToReadAsync());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());

            int result;
            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task FaultedTask_AfterCreation()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            tcs.SetException(new FormatException());

            Assert.Equal(t.Exception.InnerException, await Assert.ThrowsAsync<FormatException>(() => c.Completion));

            AssertSynchronousFalse(c.WaitToReadAsync());
            await Assert.ThrowsAsync<FormatException>(() => c.ReadAsync().AsTask());

            int result;
            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CanceledTask_AfterCreation()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            tcs.SetCanceled();

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Completion);

            AssertSynchronousFalse(c.WaitToReadAsync());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());

            int result;
            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task FaultedTask_AfterReadAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Task<int> read = c.ReadAsync().AsTask();

            tcs.SetException(new FormatException());
            Assert.Equal(t.Exception.InnerException, await Assert.ThrowsAsync<FormatException>(() => c.Completion));
            Assert.Equal(t.Exception.InnerException, await Assert.ThrowsAsync<FormatException>(() => read));
        }

        [Fact]
        public async Task CanceledTask_AfterReadAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Task<int> read = c.ReadAsync().AsTask();

            tcs.SetCanceled();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Completion);
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => read);
        }

        [Fact]
        public async Task FaultedTask_AfterWaitAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Task<bool> read = c.WaitToReadAsync();

            tcs.SetException(new FormatException());
            Assert.Equal(t.Exception.InnerException, await Assert.ThrowsAsync<FormatException>(() => c.Completion));
            Assert.False(await read);
        }

        [Fact]
        public async Task CanceledTask_AfterWaitAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task<int> t = tcs.Task;

            IReadableChannel<int> c = Channel.CreateFromTask(t);
            Task<bool> read = c.WaitToReadAsync();

            tcs.SetCanceled();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Completion);
            Assert.False(await read);
        }

    }
}
