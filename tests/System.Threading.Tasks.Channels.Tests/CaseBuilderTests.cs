// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class CaseBuilderTests : TestBase
    {
        [Fact]
        public void CaseRead_Sync_InvalidArguments_ThrowsArgumentException()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseRead<int>(null, (Action<int>)null));
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseRead<int>(null, i => { }));
            Assert.Throws<ArgumentNullException>("action", () => cb.CaseRead<int>(Channel.CreateUnbounded<int>(), (Action<int>)null));
        }

        [Fact]
        public void CaseRead_Async_InvalidArguments_ThrowsArgumentException()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseRead<int>(null, (Func<int, Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseRead<int>(null, i => Task.CompletedTask));
            Assert.Throws<ArgumentNullException>("func", () => cb.CaseRead<int>(Channel.CreateUnbounded<int>(), (Func<int, Task>)null));
        }

        [Fact]
        public void CaseWrite_Sync_InvalidArguments_ThrowsArgumentException()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseWrite<int>(null, 0, (Action)null));
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseWrite<int>(null, 0, (Action)delegate { }));
            Assert.Throws<ArgumentNullException>("action", () => cb.CaseWrite<int>(Channel.CreateUnbounded<int>(), 0, (Action)null));
        }

        [Fact]
        public void CaseWrite_Async_InvalidArguments_ThrowsArgumentException()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseWrite<int>(null, 0, (Func<Task>)null));
            Assert.Throws<ArgumentNullException>("channel", () => cb.CaseWrite<int>(null, 0, delegate { return Task.CompletedTask; }));
            Assert.Throws<ArgumentNullException>("func", () => cb.CaseWrite<int>(Channel.CreateUnbounded<int>(), 0, (Func<Task>)null));
        }

        [Fact]
        public void CaseDefault_Sync_InvalidAction_ThrowsException()
        {
            CaseBuilder builder1 = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>(() => builder1.CaseDefault((Action)null));
        }
        [Fact]
        public void CaseDefault_Async_InvalidAction_ThrowsException()
        {
            CaseBuilder builder1 = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => Task.CompletedTask);
            Assert.Throws<ArgumentNullException>(() => builder1.CaseDefault((Func<Task>)null));
        }

        [Fact]
        public void CaseReadWrite_Sync_CallMultipleTimes_IdempotentResult()
        {
            CaseBuilder builder1 = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Same(builder1, builder1.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { }));
            Assert.Same(builder1, builder1.CaseWrite(Channel.CreateUnbounded<string>(), "", () => { }));
            Assert.Same(builder1, builder1.CaseDefault(() => { }));

            CaseBuilder builder2 = Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => { });
            Assert.Same(builder2, builder2.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { }));
            Assert.Same(builder2, builder2.CaseWrite(Channel.CreateUnbounded<string>(), "", () => { }));
            Assert.Same(builder2, builder2.CaseDefault(() => { }));
        }

        [Fact]
        public void CaseReadWrite_Async_CallMultipleTimes_IdempotentResult()
        {
            CaseBuilder builder1 = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => Task.CompletedTask);
            Assert.Same(builder1, builder1.CaseRead<int>(Channel.CreateUnbounded<int>(), i => Task.CompletedTask));
            Assert.Same(builder1, builder1.CaseWrite(Channel.CreateUnbounded<string>(), "", () => Task.CompletedTask));
            Assert.Same(builder1, builder1.CaseDefault(() => Task.CompletedTask));

            CaseBuilder builder2 = Channel.CaseWrite(Channel.CreateUnbounded<int>(), 0, () => Task.CompletedTask);
            Assert.Same(builder2, builder2.CaseRead<int>(Channel.CreateUnbounded<int>(), i => Task.CompletedTask));
            Assert.Same(builder2, builder2.CaseWrite(Channel.CreateUnbounded<string>(), "", () => Task.CompletedTask));
            Assert.Same(builder2, builder2.CaseDefault(() => Task.CompletedTask));
        }

        [Fact]
        public void CaseDefault_AlreadyExists_ThrowsException()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { }).CaseDefault(() => { });
            Assert.Throws<InvalidOperationException>(() => cb.CaseDefault(() => { }));
            Assert.Throws<InvalidOperationException>(() => cb.CaseDefault(() => Task.CompletedTask));
        }

        [Fact]
        public void SelectAsync_Precanceled_ThrowsCancellationException()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            Assert.True(c.TryWrite(42));

            var cts = new CancellationTokenSource();
            cts.Cancel();

            Task<bool> select = Channel
                .CaseRead<int>(c, i => { throw new InvalidOperationException(); })
                .SelectAsync(cts.Token);
            AssertSynchronouslyCanceled(select, cts.Token);
        }

        [Fact]
        public async Task SelectAsync_CanceledAfterSelectBeforeData_ThrowsCancellationException()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            var cts = new CancellationTokenSource();

            Task<bool> select = Channel
                .CaseRead<int>(c, i => { throw new InvalidOperationException(); })
                .SelectAsync(cts.Token);

            cts.Cancel();

            await AssertCanceled(select, cts.Token);
        }

        [Fact]
        public void SelectAsync_NoChannelsAvailable_SyncDefault_CompletesSynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(() => tcs.SetResult(84))
                .SelectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, select.Status);
            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.True(select.Result);
            Assert.Equal(84, tcs.Task.Result);
        }

        [Fact]
        public void SelectAsync_NoChannelsAvailable_AsyncDefault_CompletesSynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(() => { tcs.SetResult(84); return Task.CompletedTask; })
                .SelectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, select.Status);
            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.True(select.Result);
            Assert.Equal(84, tcs.Task.Result);
        }

        [Fact]
        public async Task SelectAsync_NoChannelsAvailable_AsyncDefault_CompletesAsynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(async () => { await Task.Yield(); tcs.SetResult(84); })
                .SelectAsync();

            Assert.True(await select);
            Assert.Equal(84, tcs.Task.Result);
        }

        [Fact]
        public async Task SelectAsync_NoChannelsAvailable_SyncDefault_ThrowsSynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(new Action(() => { throw new FormatException(); }))
                .SelectAsync();

            Assert.True(select.IsCompleted);
            await Assert.ThrowsAsync<FormatException>(() => select);
        }

        [Fact]
        public async Task SelectAsync_NoChannelsAvailable_AsyncDefault_ThrowsSynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(new Func<Task>(() => { throw new FormatException(); }))
                .SelectAsync();

            Assert.True(select.IsCompleted);
            await Assert.ThrowsAsync<FormatException>(() => select);
        }

        [Fact]
        public async Task SelectAsync_NoChannelsAvailable_AsyncDefault_ThrowsAsynchronously()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .CaseDefault(async () => { await Task.Yield(); throw new FormatException(); })
                .SelectAsync();

            await Assert.ThrowsAsync<FormatException>(() => select);
        }

        [Fact]
        public async Task SelectAsync_AllChannelsCompletedBefore_ReturnsFalse()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();
            c1.Complete();
            c2.Complete();

            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .SelectAsync();
            Assert.False(await select);
        }

        [Fact]
        public async Task SelectAsync_AllChannelsCompletedAfter_ReturnsFalse()
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<int> c2 = Channel.CreateUnbuffered<int>();

            Task<bool> select = Channel
                .CaseRead<int>(c1, i => { throw new InvalidOperationException(); })
                .CaseWrite(c2, 42, () => { throw new InvalidOperationException(); })
                .SelectAsync();

            c1.Complete();
            c2.Complete();

            Assert.False(await select);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Sync_DataAlreadyAvailable()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            Assert.True(c.TryWrite(42));

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, i => tcs.SetResult(i)).SelectAsync();

            Assert.True(select.IsCompleted);
            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Async_DataAlreadyAvailable_CompletesSynchronously()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            Assert.True(c.TryWrite(42));

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, i => { tcs.SetResult(i); return Task.CompletedTask; }).SelectAsync();

            Assert.True(select.IsCompleted);
            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Async_DataAlreadyAvailable_CompletesAsynchronously()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();
            Assert.True(c.TryWrite(42));

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, async i => { await Task.Yield(); tcs.SetResult(i); }).SelectAsync();

            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Sync_DataNotAlreadyAvailable()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, i => tcs.SetResult(i)).SelectAsync();
            Assert.False(select.IsCompleted);

            Task write = c.WriteAsync(42);

            Assert.True(await select);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Async_DataNotAlreadyAvailable_CompletesSynchronously()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, i => { tcs.SetResult(i); return Task.CompletedTask; }).SelectAsync();
            Assert.False(select.IsCompleted);

            Task write = c.WriteAsync(42);

            Assert.True(await select);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseRead_Async_DataNotAlreadyAvailable_CompletesAsynchronously()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseRead<int>(c, async i => { await Task.Yield(); tcs.SetResult(i); }).SelectAsync();
            Assert.False(select.IsCompleted);

            Task write = c.WriteAsync(42);

            Assert.True(await select);
            Assert.Equal(42, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Sync_SpaceAlreadyAvailable_CompletesSynchronously()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, () => tcs.SetResult(1)).SelectAsync();

            Assert.True(select.IsCompleted);
            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(1, await tcs.Task);

            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Async_SpaceAlreadyAvailable_CompletesSynchronously()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, () => { tcs.SetResult(1); return Task.CompletedTask; }).SelectAsync();

            Assert.True(select.IsCompleted);
            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(1, await tcs.Task);

            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Async_SpaceAlreadyAvailable_CompletesAsynchronously()
        {
            Channel<int> c = Channel.CreateUnbounded<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, async () => { await Task.Yield(); tcs.SetResult(1); }).SelectAsync();

            Assert.True(await select);

            Assert.Equal(TaskStatus.RanToCompletion, tcs.Task.Status);
            Assert.Equal(1, await tcs.Task);

            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Sync_SpaceNotAlreadyAvailable()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, () => tcs.SetResult(1)).SelectAsync();

            Assert.False(select.IsCompleted);

            Task<int> read = c.ReadAsync().AsTask();

            Assert.True(await select);
            Assert.Equal(42, await read);
            Assert.Equal(1, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Async_SpaceNotAlreadyAvailable_CompletesSynchronously()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, () => { tcs.SetResult(1); return Task.CompletedTask; }).SelectAsync();

            Assert.False(select.IsCompleted);

            Task<int> read = c.ReadAsync().AsTask();

            Assert.True(await select);
            Assert.Equal(42, await read);
            Assert.Equal(1, await tcs.Task);
        }

        [Fact]
        public async Task SelectAsync_SingleCaseWrite_Async_SpaceNotAlreadyAvailable_CompletesAsynchronously()
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();

            var tcs = new TaskCompletionSource<int>();
            Task<bool> select = Channel.CaseWrite(c, 42, async () => { await Task.Yield(); tcs.SetResult(1); }).SelectAsync();

            Assert.False(select.IsCompleted);

            Task<int> read = c.ReadAsync().AsTask();

            Assert.True(await select);
            Assert.Equal(42, await read);
            Assert.Equal(1, await tcs.Task);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseRead_Sync_ThrowsSynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task write;
            if (before)
                write = c.WriteAsync(42);
            Task t = Channel.CaseRead<int>(c, new Action<int>(i => { throw new FormatException(); })).SelectAsync();
            if (!before)
                write = c.WriteAsync(42);
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseRead_Async_DataAlreadyAvailable_ThrowsSynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task write;
            if (before)
                write = c.WriteAsync(42);
            Task t = Channel.CaseRead<int>(c, new Func<int, Task>(i => { throw new FormatException(); })).SelectAsync();
            if (!before)
                write = c.WriteAsync(42);
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseRead_Async_DataAlreadyAvailable_ThrowsAsynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task write;
            if (before)
                write = c.WriteAsync(42);
            Task t = Channel.CaseRead<int>(c, async i => { await Task.Yield(); throw new FormatException(); }).SelectAsync();
            if (!before)
                write = c.WriteAsync(42);
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseWrite_Sync_ThrowsSynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task read;
            if (before)
                read = c.ReadAsync().AsTask();
            Task t = Channel.CaseWrite(c, 42, new Action(() => { throw new FormatException(); })).SelectAsync();
            if (!before)
                read = c.ReadAsync().AsTask();
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseWrite_Async_ThrowsSynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task read;
            if (before)
                read = c.ReadAsync().AsTask();
            Task t = Channel.CaseWrite(c, 42, new Func<Task>(() => { throw new FormatException(); })).SelectAsync();
            if (!before)
                read = c.ReadAsync().AsTask();
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task SelectAsync_CaseWrite_Async_ThrowsAsynchronously(bool before)
        {
            Channel<int> c = Channel.CreateUnbuffered<int>();
            Task read;
            if (before)
                read = c.ReadAsync().AsTask();
            Task t = Channel.CaseWrite(c, 42, async () => { await Task.Yield(); throw new FormatException(); }).SelectAsync();
            if (!before)
                read = c.ReadAsync().AsTask();
            await Assert.ThrowsAsync<FormatException>(() => t);
        }

        [Fact]
        public void SelectUntilAsync_InvalidArguments_ThrowsExceptions()
        {
            CaseBuilder cb = Channel.CaseRead<int>(Channel.CreateUnbounded<int>(), i => { });
            Assert.Throws<ArgumentNullException>(() => { cb.SelectUntilAsync(null); });
        }

        [Theory]
        [InlineData(false, 100, 150)]
        [InlineData(true, 100, 150)]
        [InlineData(false, 100, 100)]
        [InlineData(true, 100, 100)]
        [InlineData(false, 100, 99)]
        [InlineData(true, 100, 99)]
        [InlineData(false, 100, 1)]
        [InlineData(true, 100, 1)]
        [InlineData(false, 100, 0)]
        [InlineData(true, 100, 0)]
        public async Task SelectUntilAsync_ProcessUntilAllDataExhausted_Success(bool dataAvailableBefore, int numItems, int maxIterations)
        {
            Channel<int> c1 = Channel.CreateUnbounded<int>();
            Channel<string> c2 = Channel.CreateUnbounded<string>();
            Channel<double> c3 = Channel.CreateUnbounded<double>();

            int delegatesInvoked = 0;
            Task<int> select = null;

            if (!dataAvailableBefore)
            {
                select = Channel
                    .CaseRead<int>(c1, i => { Interlocked.Increment(ref delegatesInvoked); })
                    .CaseRead<string>(c2, s => { Interlocked.Increment(ref delegatesInvoked); })
                    .CaseRead<double>(c3, d => { Interlocked.Increment(ref delegatesInvoked); })
                    .SelectUntilAsync(i => i < maxIterations);
            }

            for (int i = 0; i < numItems; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        Assert.True(c1.TryWrite(i));
                        break;
                    case 1:
                        Assert.True(c2.TryWrite(i.ToString()));
                        break;
                    case 2:
                        Assert.True(c3.TryWrite(i));
                        break;
                }
            }

            c1.Complete();
            c2.Complete();
            c3.Complete();

            if (dataAvailableBefore)
            {
                select = Channel
                    .CaseRead<int>(c1, i => { Interlocked.Increment(ref delegatesInvoked); })
                    .CaseRead<string>(c2, s => { Interlocked.Increment(ref delegatesInvoked); })
                    .CaseRead<double>(c3, d => { Interlocked.Increment(ref delegatesInvoked); })
                    .SelectUntilAsync(i => i < maxIterations);
            }

            int expected = Math.Min(numItems, maxIterations);
            Assert.Equal(expected, await select);
            Assert.Equal(expected, delegatesInvoked);
        }

    }
}
