// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Linq;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public abstract class ChannelTestBase : TestBase
    {
        protected abstract IChannel<int> CreateChannel();

        protected virtual bool RequiresSingleReaderWriter { get { return false; } }

        [Fact]
        public void ValidateDebuggerAttributes()
        {
            IChannel<int> c = CreateChannel();
            for (int i = 1; i <= 10; i++)
            {
                c.WriteAsync(i);
            }
            DebuggerAttributes.ValidateDebuggerDisplayReferences(c);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(c);
        }

        [Fact]
        public void Completion_Idempotent()
        {
            IChannel<int> c = CreateChannel();

            Task completion = c.Completion;
            Assert.Equal(TaskStatus.WaitingForActivation, completion.Status);

            Assert.Same(completion, c.Completion);
            c.Complete();
            Assert.Same(completion, c.Completion);

            Assert.Equal(TaskStatus.RanToCompletion, completion.Status);
        }

        [Fact]
        public async Task Complete_AfterEmpty_NoWaiters_TriggersCompletion()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            await c.Completion;
        }

        [Fact]
        public async Task Complete_AfterEmpty_WaitingReader_TriggersCompletion()
        {
            IChannel<int> c = CreateChannel();
            Task<int> r = c.ReadAsync().AsTask();
            c.Complete();
            await c.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => r);
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingReaders_TriggersCompletion()
        {
            IChannel<int> c = Channel.Create<int>(1);
            Task<int> read = c.ReadAsync().AsTask();
            c.Complete();
            await c.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => read);
        }

        [Fact]
        public void Complete_Twice_ThrowsInvalidOperationException()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            Assert.ThrowsAny<InvalidOperationException>(() => c.Complete());
        }

        [Fact]
        public void TryComplete_Twice_ReturnsTrueThenFalse()
        {
            IChannel<int> c = CreateChannel();
            Assert.True(c.TryComplete());
            Assert.False(c.TryComplete());
            Assert.False(c.TryComplete());
        }

        [Fact]
        public void SingleProducerConsumer_ConcurrentReadWrite_Success()
        {
            IChannel<int> c = Channel.Create<int>();

            const int NumItems = 100000;
            Task.WaitAll(
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        await c.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c.ReadAsync());
                    }
                }));
        }

        [Fact]
        public void ManyProducerConsumer_ConcurrentReadWrite_Success()
        {
            if (RequiresSingleReaderWriter)
                return;

            IChannel<int> c = CreateChannel();

            const int NumWriters = 10;
            const int NumReaders = 10;
            const int NumItems = 10000;

            long readTotal = 0;
            int remainingWriters = NumWriters;
            int remainingItems = NumItems;

            Task[] tasks = new Task[NumWriters + NumReaders];

            for (int i = 0; i < NumReaders; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    IAsyncEnumerator<int> e = c.GetAsyncEnumerator();
                    while (await e.MoveNextAsync())
                    {
                        Interlocked.Add(ref readTotal, e.Current);
                    }
                });
            }

            for (int i = 0; i < NumWriters; i++)
            {
                tasks[NumReaders + i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        int value = Interlocked.Decrement(ref remainingItems);
                        if (value < 0)
                        {
                            break;
                        }
                        await c.WriteAsync(value + 1);
                    }
                    if (Interlocked.Decrement(ref remainingWriters) == 0)
                        c.Complete();
                });
            }

            Task.WaitAll(tasks);
            Assert.Equal((NumItems * (NumItems + 1L)) / 2, readTotal);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableBefore_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            Task write = c.WriteAsync(42);
            Task<bool> read = c.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableAfter_CompletesAsynchronously()
        {
            IChannel<int> c = CreateChannel();
            Task<bool> read = c.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            Task write = c.WriteAsync(42);
            Assert.True(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_AfterComplete_SynchronouslyCompletes()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            Task<bool> read = c.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
            Assert.False(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_BeforeComplete_AsynchronouslyCompletes()
        {
            IChannel<int> c = CreateChannel();
            Task<bool> read = c.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            c.Complete();
            Assert.False(read.Result);
        }

        [Fact]
        public void WaitToWriteAsync_AfterComplete_SynchronouslyCompletes()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            Task<bool> write = c.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
            Assert.False(write.Result);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableBefore_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            ValueTask<int> read = c.ReadAsync();
            Task<bool> write = c.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableAfter_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            Task<bool> write = c.WaitToWriteAsync();
            ValueTask<int> read = c.ReadAsync();
            Assert.True(write.Result);
        }

        [Fact]
        public void TryRead_DataAvailable_Success()
        {
            IChannel<int> c = CreateChannel();
            Task write = c.WriteAsync(42);
            int result;
            Assert.True(c.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task ReadAsync_DataAvailable_Success()
        {
            IChannel<int> c = CreateChannel();
            Task write = c.WriteAsync(42);
            Assert.Equal(42, await c.ReadAsync());
        }

        [Fact]
        public void TryRead_AfterComplete_ReturnsFalse()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            int result;
            Assert.False(c.TryRead(out result));
        }

        [Fact]
        public void TryWrite_AfterComplete_ReturnsFalse()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            Assert.False(c.TryWrite(42));
        }

        [Fact]
        public async Task WriteAsync_AfterComplete_ThrowsException()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.WriteAsync(42));
        }

        [Fact]
        public async Task ReadAsync_AfterComplete_ThrowsException()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToCompletion()
        {
            IChannel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Complete(exc);
            Assert.Same(exc, await Assert.ThrowsAsync<FormatException>(() => c.Completion));
        }

        [Fact]
        public async Task Complete_WithCancellationException_PropagatesToCompletion()
        {
            IChannel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Exception exc = null;
            try { cts.Token.ThrowIfCancellationRequested(); }
            catch (Exception e) { exc = e; }

            c.Complete(exc);
            await AssertCanceled(c.Completion, cts.Token);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingReader()
        {
            IChannel<int> c = CreateChannel();
            Task<int> read = c.ReadAsync().AsTask();
            FormatException exc = new FormatException();
            c.Complete(exc);
            Assert.Same(exc, await Assert.ThrowsAsync<FormatException>(() => read));
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewReader()
        {
            IChannel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Complete(exc);
            Task<int> read = c.ReadAsync().AsTask();
            Assert.Same(exc, await Assert.ThrowsAsync<FormatException>(() => read));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ManyWriteAsync_ThenManyReadAsync_Success(int mode)
        {
            if (RequiresSingleReaderWriter)
                return;

            IChannel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i] = c.WriteAsync(i);
            }

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                switch (mode)
                {
                    case 0:
                        readers[i] = c.ReadAsync().AsTask();
                        break;
                    case 1:
                        int result;
                        Assert.True(c.TryRead(out result));
                        readers[i] = Task.FromResult(result);
                        break;
                }
            }

            Task.WaitAll(readers);
            Task.WaitAll(writers);

            for (int i = 0; i < readers.Length; i++)
            {
                Assert.Equal(i, readers[i].Result);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ManyReadAsync_ThenManyWriteAsync_Success(int mode)
        {
            if (RequiresSingleReaderWriter)
                return;

            IChannel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                readers[i] = c.ReadAsync().AsTask();
            }

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                switch (mode)
                {
                    case 0:
                        writers[i] = c.WriteAsync(i);
                        break;
                    case 1:
                        Assert.True(c.TryWrite(i));
                        writers[i] = Task.CompletedTask;
                        break;
                }
            }

            Task.WaitAll(readers);
            Task.WaitAll(writers);

            for (int i = 0; i < readers.Length; i++)
            {
                Assert.Equal(i, readers[i].Result);
            }
        }

        [Fact]
        public void Precancellation_Reading_ReturnsCanceledImmediately()
        {
            IChannel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.ReadAsync(cts.Token).AsTask(), cts.Token);
            AssertSynchronouslyCanceled(c.WaitToReadAsync(cts.Token), cts.Token);
        }

        [Fact]
        public void Precancellation_Writing_ReturnsCanceledImmediately()
        {
            IChannel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.WriteAsync(42, cts.Token), cts.Token);
            AssertSynchronouslyCanceled(c.WaitToWriteAsync(cts.Token), cts.Token);
        }

        [Fact]
        public async Task Await_CompletedChannel_Throws()
        {
            IChannel<int> c = CreateChannel();
            c.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await c);
        }

        [Fact]
        public async Task Await_ChannelAfterExistingData_ReturnsData()
        {
            IChannel<int> c = CreateChannel();
            Task write = c.WriteAsync(42);
            Assert.Equal(42, await c);
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public async Task SelectAsync_CaseReadBeforeAvailable_Success()
        {
            if (RequiresSingleReaderWriter)
                return;

            IChannel<int> c1 = CreateChannel();
            IChannel<int> c2 = CreateChannel();
            IChannel<int> c3 = CreateChannel();

            int total1 = 0, total2 = 0, total3 = 0;
            int expectedTotal1 = 0, expectedTotal2 = 0, expectedTotal3 = 0;

            var selects = new Task<bool>[12];
            for (int i = 0; i < selects.Length; i++)
            {
                selects[i] = Channel
                    .CaseRead(c1, item => Interlocked.Add(ref total1, item))
                    .CaseRead(c2, item => { Interlocked.Add(ref total2, item); return Task.CompletedTask; })
                    .CaseRead(c3, async item => { await Task.Yield(); Interlocked.Add(ref total3, item); })
                    .SelectAsync();
            }

            var writes = new Task[selects.Length];
            for (int i = 0; i < selects.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        writes[i] = c1.WriteAsync(i);
                        expectedTotal1 += i;
                        break;
                    case 1:
                        writes[i] = c2.WriteAsync(i);
                        expectedTotal2 += i;
                        break;
                    case 2:
                        writes[i] = c3.WriteAsync(i);
                        expectedTotal3 += i;
                        break;
                }
            }

            await Task.WhenAll(selects);
            Assert.All(writes, write => Assert.Equal(TaskStatus.RanToCompletion, write.Status));
            Assert.All(selects, select => Assert.Equal(TaskStatus.RanToCompletion, select.Status));
            Assert.Equal(selects.Length, selects.Count(s => s.Result));

            Assert.Equal(expectedTotal1, total1);
            Assert.Equal(expectedTotal2, total2);
            Assert.Equal(expectedTotal3, total3);
        }

        [Fact]
        public async Task TryWrite_BlockedReader_Success()
        {
            IChannel<int> c = CreateChannel();
            Task<int> read = c.ReadAsync().AsTask();
            Assert.False(read.IsCompleted);
            Assert.True(c.TryWrite(42));
            Assert.Equal(42, await read);
        }

        [Fact]
        public void Write_WaitToReadAsync_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            c.WriteAsync(42);
            AssertSynchronousTrue(c.WaitToReadAsync());
        }

        [Fact]
        public void Read_WaitToWriteAsync_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            c.ReadAsync();
            AssertSynchronousTrue(c.WaitToWriteAsync());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task AsObservable_DataWritten(bool endWithError)
        {
            IChannel<int> c = CreateChannel();
            IObserver<int> o = c.AsObserver();

            Task reader = Task.Run(async () =>
            {
                int received = 0;
                IAsyncEnumerator<int> e = c.GetAsyncEnumerator();
                while (await e.MoveNextAsync())
                {
                    Assert.Equal(received++, e.Current);
                }
            });

            for (int i = 0; i < 10; i++)
            {
                o.OnNext(i);
            }

            if (endWithError)
            {
                o.OnError(new FormatException());
                await Assert.ThrowsAsync<FormatException>(() => reader);
            }
            else
            {
                o.OnCompleted();
                await reader;
            }
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(1, true)]
        [InlineData(5, false)]
        [InlineData(5, true)]
        public async Task AsObserver_AllDataPushed(int numSubscribers, bool completeWithError)
        {
            if (RequiresSingleReaderWriter)
                return;

            IChannel<int> c = CreateChannel();

            int received = 0;
            var tcs = new TaskCompletionSource<Exception>();

            IObservable<int> o = c.AsObservable();
            for (int s = 0; s < numSubscribers; s++)
            {
                o.Subscribe(new DelegateObserver<int>
                {
                    OnNextDelegate = i => received += i,
                    OnCompletedDelegate = () => tcs.TrySetResult(null),
                    OnErrorDelegate = e => tcs.TrySetResult(e),
                });
            }

            Task[] writes = new Task[10];
            for (int i = 0; i < writes.Length; i++)
            {
                writes[i] = c.WriteAsync(i + 1);
            }
            await Task.WhenAll(writes);

            c.Complete(completeWithError ? new FormatException() : null);

            Exception result = await tcs.Task;
            if (completeWithError)
                Assert.IsType<FormatException>(result);
            else
                Assert.Null(result);

            Assert.Equal(numSubscribers * (writes.Length * (writes.Length + 1)) / 2, received);
        }

    }
}
