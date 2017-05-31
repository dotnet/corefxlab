// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Linq;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public abstract class ChannelTestBase : TestBase
    {
        protected abstract Channel<int> CreateChannel();
        protected abstract Channel<int> CreateFullChannel();

        protected virtual bool RequiresSingleReader => false;
        protected virtual bool RequiresSingleWriter => false;

        [Fact]
        public void ValidateDebuggerAttributes()
        {
            Channel<int> c = CreateChannel();
            for (int i = 1; i <= 10; i++)
            {
                c.Out.WriteAsync(i);
            }
            DebuggerAttributes.ValidateDebuggerDisplayReferences(c);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(c);
        }

        [Fact]
        public void Completion_Idempotent()
        {
            Channel<int> c = CreateChannel();

            Task completion = c.In.Completion;
            Assert.Equal(TaskStatus.WaitingForActivation, completion.Status);

            Assert.Same(completion, c.In.Completion);
            c.Out.Complete();
            Assert.Same(completion, c.In.Completion);

            Assert.Equal(TaskStatus.RanToCompletion, completion.Status);
        }

        [Fact]
        public async Task Complete_AfterEmpty_NoWaiters_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            await c.In.Completion;
        }

        [Fact]
        public async Task Complete_AfterEmpty_WaitingReader_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            Task<int> r = c.In.ReadAsync().AsTask();
            c.Out.Complete();
            await c.In.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => r);
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingReaders_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            Task<int> read = c.In.ReadAsync().AsTask();
            c.Out.Complete();
            await c.In.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => read);
        }

        [Fact]
        public void Complete_Twice_ThrowsInvalidOperationException()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            Assert.ThrowsAny<InvalidOperationException>(() => c.Out.Complete());
        }

        [Fact]
        public void TryComplete_Twice_ReturnsTrueThenFalse()
        {
            Channel<int> c = CreateChannel();
            Assert.True(c.Out.TryComplete());
            Assert.False(c.Out.TryComplete());
            Assert.False(c.Out.TryComplete());
        }

        [Fact]
        public async Task TryComplete_ErrorsPropage()
        {
            Channel<int> c;

            // Success
            c = CreateChannel();
            Assert.True(c.Out.TryComplete());
            await c.In.Completion;

            // Error
            c = CreateChannel();
            Assert.True(c.Out.TryComplete(new FormatException()));
            await Assert.ThrowsAsync<FormatException>(() => c.In.Completion);

            // Canceled
            c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();
            Assert.True(c.Out.TryComplete(new OperationCanceledException(cts.Token)));
            await AssertCanceled(c.In.Completion, cts.Token);
        }

        [Fact]
        public void SingleProducerConsumer_ConcurrentReadWrite_Success()
        {
            Channel<int> c = CreateChannel();

            const int NumItems = 100000;
            Task.WaitAll(
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        await c.Out.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c.In.ReadAsync());
                    }
                }));
        }

        [Fact]
        public void SingleProducerConsumer_ConcurrentAwaitWrite_Success()
        {
            Channel<int> c = CreateChannel();

            const int NumItems = 100000;
            Task.WaitAll(
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        await c.Out.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c.In);
                    }
                }));
        }

        [Fact]
        public void SingleProducerConsumer_PingPong_Success()
        {
            Channel<int> c1 = CreateChannel();
            Channel<int> c2 = CreateChannel();

            const int NumItems = 100000;
            Task.WaitAll(
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c1.In);
                        await c2.Out.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        await c1.Out.WriteAsync(i);
                        Assert.Equal(i, await c2.In);
                    }
                }));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(10, 1)]
        [InlineData(10, 10)]
        public void ManyProducerConsumer_ConcurrentReadWrite_Success(int numReaders, int numWriters)
        {
            if (RequiresSingleReader && numReaders > 1) return;
            if (RequiresSingleWriter && numWriters > 1) return;

            Channel<int> c = CreateChannel();

            const int NumItems = 10000;

            long readTotal = 0;
            int remainingWriters = numWriters;
            int remainingItems = NumItems;

            Task[] tasks = new Task[numWriters + numReaders];

            for (int i = 0; i < numReaders; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    IAsyncEnumerator<int> e = c.In.GetAsyncEnumerator();
                    while (await e.MoveNextAsync())
                    {
                        Interlocked.Add(ref readTotal, e.Current);
                    }
                });
            }

            for (int i = 0; i < numWriters; i++)
            {
                tasks[numReaders + i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        int value = Interlocked.Decrement(ref remainingItems);
                        if (value < 0)
                        {
                            break;
                        }
                        await c.Out.WriteAsync(value + 1);
                    }
                    if (Interlocked.Decrement(ref remainingWriters) == 0)
                        c.Out.Complete();
                });
            }

            Task.WaitAll(tasks);
            Assert.Equal((NumItems * (NumItems + 1L)) / 2, readTotal);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(10, 1)]
        [InlineData(10, 10)]
        public void ManyProducerConsumer_ConcurrentAwaitWrite_Success(int numReaders, int numWriters)
        {
            if (RequiresSingleReader && numReaders > 1) return;
            if (RequiresSingleWriter && numWriters > 1) return;

            Channel<int> c = CreateChannel();

            const int NumItems = 10000;

            long readTotal = 0;
            int remainingWriters = numWriters;
            int remainingItems = NumItems;

            Task[] tasks = new Task[numWriters + numReaders];

            for (int i = 0; i < numReaders; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        while (true) Interlocked.Add(ref readTotal, await c.In);
                    }
                    catch (ClosedChannelException) { }
                });
            }

            for (int i = 0; i < numWriters; i++)
            {
                tasks[numReaders + i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        int value = Interlocked.Decrement(ref remainingItems);
                        if (value < 0)
                        {
                            break;
                        }
                        await c.Out.WriteAsync(value + 1);
                    }
                    if (Interlocked.Decrement(ref remainingWriters) == 0)
                        c.Out.Complete();
                });
            }

            Task.WaitAll(tasks);
            Assert.Equal((NumItems * (NumItems + 1L)) / 2, readTotal);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableBefore_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Out.WriteAsync(42);
            Task<bool> read = c.In.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableAfter_CompletesAsynchronously()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.In.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            Task write = c.Out.WriteAsync(42);
            Assert.True(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_AfterComplete_SynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            Task<bool> read = c.In.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
            Assert.False(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_BeforeComplete_AsynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.In.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            c.Out.Complete();
            Assert.False(read.Result);
        }

        [Fact]
        public void WaitToWriteAsync_AfterComplete_SynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            Task<bool> write = c.Out.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
            Assert.False(write.Result);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableBefore_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            ValueTask<int> read = c.In.ReadAsync();
            Task<bool> write = c.Out.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableAfter_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            Task<bool> write = c.Out.WaitToWriteAsync();
            ValueTask<int> read = c.In.ReadAsync();
            Assert.True(write.Result);
        }

        [Fact]
        public void TryRead_DataAvailable_Success()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Out.WriteAsync(42);
            int result;
            Assert.True(c.In.TryRead(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task ReadAsync_DataAvailable_Success()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Out.WriteAsync(42);
            Assert.Equal(42, await c.In.ReadAsync());
        }

        [Fact]
        public void TryRead_AfterComplete_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            int result;
            Assert.False(c.In.TryRead(out result));
        }

        [Fact]
        public void TryWrite_AfterComplete_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            Assert.False(c.Out.TryWrite(42));
        }

        [Fact]
        public async Task WriteAsync_AfterComplete_ThrowsException()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Out.WriteAsync(42));
        }

        [Fact]
        public async Task ReadAsync_AfterComplete_ThrowsException()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.In.ReadAsync().AsTask());
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToCompletion()
        {
            Channel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Assert.Same(exc, await Assert.ThrowsAsync<FormatException>(() => c.In.Completion));
        }

        [Fact]
        public async Task Complete_WithCancellationException_PropagatesToCompletion()
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Exception exc = null;
            try { cts.Token.ThrowIfCancellationRequested(); }
            catch (Exception e) { exc = e; }

            c.Out.Complete(exc);
            await AssertCanceled(c.In.Completion, cts.Token);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingReader()
        {
            Channel<int> c = CreateChannel();
            Task<int> read = c.In.ReadAsync().AsTask();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Assert.Same(exc, (await Assert.ThrowsAsync<ClosedChannelException>(() => read)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWriter()
        {
            Channel<int> c = CreateFullChannel();
            if (c != null)
            {
                Task write = c.Out.WriteAsync(42);
                FormatException exc = new FormatException();
                c.Out.Complete(exc);
                Assert.Same(exc, (await Assert.ThrowsAsync<ClosedChannelException>(() => write)).InnerException);
            }
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewReader()
        {
            Channel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Task<int> read = c.In.ReadAsync().AsTask();
            Assert.Same(exc, (await Assert.ThrowsAsync<ClosedChannelException>(() => read)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWriter()
        {
            Channel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Task write = c.Out.WriteAsync(42);
            Assert.Same(exc, (await Assert.ThrowsAsync<ClosedChannelException>(() => write)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWaitingReader()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.In.WaitToReadAsync();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            await Assert.ThrowsAsync<FormatException>(() => read);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWaitingWriter()
        {
            Channel<int> c = CreateFullChannel();
            if (c != null)
            {
                Task<bool> write = c.Out.WaitToWriteAsync();
                FormatException exc = new FormatException();
                c.Out.Complete(exc);
                await Assert.ThrowsAsync<FormatException>(() => write);
            }
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWaitingReader()
        {
            Channel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Task<bool> read = c.In.WaitToReadAsync();
            await Assert.ThrowsAsync<FormatException>(() => read);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWaitingWriter()
        {
            Channel<int> c = CreateChannel();
            FormatException exc = new FormatException();
            c.Out.Complete(exc);
            Task<bool> write = c.Out.WaitToWriteAsync();
            await Assert.ThrowsAsync<FormatException>(() => write);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ManyWriteAsync_ThenManyReadAsync_Success(int readMode)
        {
            if (RequiresSingleReader || RequiresSingleWriter) return;

            Channel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i] = c.Out.WriteAsync(i);
            }

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                switch (readMode)
                {
                    case 0:
                        readers[i] = c.In.ReadAsync().AsTask();
                        break;
                    case 1:
                        int result;
                        Assert.True(c.In.TryRead(out result));
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
        public void ManyReadAsync_ThenManyWriteAsync_Success(int writeMode)
        {
            if (RequiresSingleReader || RequiresSingleReader) return;

            Channel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                readers[i] = c.In.ReadAsync().AsTask();
            }

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                switch (writeMode)
                {
                    case 0:
                        writers[i] = c.Out.WriteAsync(i);
                        break;
                    case 1:
                        Assert.True(c.Out.TryWrite(i));
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
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.In.ReadAsync(cts.Token).AsTask(), cts.Token);
            AssertSynchronouslyCanceled(c.In.WaitToReadAsync(cts.Token), cts.Token);
        }

        [Fact]
        public void Precancellation_Writing_ReturnsCanceledImmediately()
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.Out.WriteAsync(42, cts.Token), cts.Token);
            AssertSynchronouslyCanceled(c.Out.WaitToWriteAsync(cts.Token), cts.Token);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CancelPendingRead_Writing_DataTransferredToCorrectReader(int writeMode)
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();

            Task<int> read1 = c.In.ReadAsync(cts.Token).AsTask();
            cts.Cancel();
            Task<int> read2 = c.In.ReadAsync().AsTask();

            switch (writeMode)
            {
                case 0: await c.Out.WriteAsync(42); break;
                case 1: Assert.True(c.Out.TryWrite(42)); break;
            }

            Assert.Equal(42, await read2);
            await AssertCanceled(read1, cts.Token);
        }

        [Fact]
        public async Task CancelPendingRead_WrittenDataPendingOrStored()
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();

            Task<int> read1 = c.In.ReadAsync(cts.Token).AsTask();
            cts.Cancel();
            await AssertCanceled(read1, cts.Token);

            Task write = c.Out.WriteAsync(42);

            Assert.Equal(42, await c.In.ReadAsync());
            await write;
        }

        [Fact]
        public async Task AwaitRead_CompletedChannel_Throws()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await c.In);
        }

        [Fact]
        public async Task AwaitRead_ChannelAfterExistingData_ReturnsData()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Out.WriteAsync(42);
            Assert.Equal(42, await c.In);
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public async Task AwaitWriteAvailable_CompletedChannel_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Out.Complete();
            Assert.False(await c.Out);
        }

        [Fact]
        public async Task AwaitWriteAvailable_ChannelWhenPendingRead_ReturnsTrue()
        {
            Channel<int> c = CreateChannel();
            ValueTask<int> pendingRead = c.In.ReadAsync();
            Assert.False(pendingRead.IsCompleted);
            Assert.True(await c.Out);
        }

        [Fact]
        public async Task SelectAsync_CaseReadBeforeAvailable_Success()
        {
            if (RequiresSingleReader || RequiresSingleWriter) return;

            Channel<int> c1 = CreateChannel();
            Channel<int> c2 = CreateChannel();
            Channel<int> c3 = CreateChannel();

            int total1 = 0, total2 = 0, total3 = 0;
            int expectedTotal1 = 0, expectedTotal2 = 0, expectedTotal3 = 0;

            var selects = new Task<bool>[12];
            for (int i = 0; i < selects.Length; i++)
            {
                selects[i] = Channel
                    .CaseRead<int>(c1, item => Interlocked.Add(ref total1, item))
                    .CaseRead<int>(c2, item => { Interlocked.Add(ref total2, item); return Task.CompletedTask; })
                    .CaseRead<int>(c3, async item => { await Task.Yield(); Interlocked.Add(ref total3, item); })
                    .SelectAsync();
            }

            var writes = new Task[selects.Length];
            for (int i = 0; i < selects.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        writes[i] = c1.Out.WriteAsync(i);
                        expectedTotal1 += i;
                        break;
                    case 1:
                        writes[i] = c2.Out.WriteAsync(i);
                        expectedTotal2 += i;
                        break;
                    case 2:
                        writes[i] = c3.Out.WriteAsync(i);
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
            Channel<int> c = CreateChannel();
            Task<int> read = c.In.ReadAsync().AsTask();
            Assert.False(read.IsCompleted);
            Assert.True(c.Out.TryWrite(42));
            Assert.Equal(42, await read);
        }

        [Fact]
        public void Write_WaitToReadAsync_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            c.Out.WriteAsync(42);
            AssertSynchronousTrue(c.In.WaitToReadAsync());
        }

        [Fact]
        public void Read_WaitToWriteAsync_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            c.In.ReadAsync();
            AssertSynchronousTrue(c.Out.WaitToWriteAsync());
        }

        [Theory]
        [InlineData(TaskStatus.RanToCompletion)]
        [InlineData(TaskStatus.Faulted)]
        [InlineData(TaskStatus.Canceled)]
        public async Task AsObserver_DataWritten(TaskStatus endingMode)
        {
            Channel<int> c = CreateChannel();
            IObserver<int> o = c.Out.AsObserver();

            Task reader = Task.Run(async () =>
            {
                int received = 0;
                IAsyncEnumerator<int> e = c.In.GetAsyncEnumerator();
                while (await e.MoveNextAsync())
                {
                    Assert.Equal(received++, e.Current);
                }
            });

            for (int i = 0; i < 10; i++)
            {
                o.OnNext(i);
            }

            switch (endingMode)
            {
                case TaskStatus.RanToCompletion:
                    o.OnCompleted();
                    await reader;
                    break;
                case TaskStatus.Faulted:
                    var exc = new FormatException();
                    o.OnError(exc);
                    Assert.Same(exc, (await Assert.ThrowsAsync<ClosedChannelException>(() => reader)).InnerException);
                    break;
                case TaskStatus.Canceled:
                    o.OnError(new OperationCanceledException());
                    await Assert.ThrowsAnyAsync<OperationCanceledException>(() => reader);
                    break;
            }
            Assert.Equal(endingMode, reader.Status);
        }

        [Theory]
        [InlineData(1, TaskStatus.RanToCompletion)]
        [InlineData(1, TaskStatus.Faulted)]
        [InlineData(1, TaskStatus.Canceled)]
        [InlineData(5, TaskStatus.RanToCompletion)]
        [InlineData(5, TaskStatus.Faulted)]
        [InlineData(5, TaskStatus.Canceled)]
        public async Task AsObserver_AllDataPushed(int numSubscribers, TaskStatus endingMode)
        {
            if (RequiresSingleWriter) return;

            Channel<int> c = CreateChannel();

            int received = 0;
            var tcs = new TaskCompletionSource<Exception>();

            IObservable<int> o = c.In.AsObservable();
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
                writes[i] = c.Out.WriteAsync(i + 1);
            }
            await Task.WhenAll(writes);

            c.Out.Complete(
                endingMode == TaskStatus.RanToCompletion ? null :
                endingMode == TaskStatus.Faulted ? new FormatException() :
                (Exception)new OperationCanceledException());

            Exception result = await tcs.Task;
            switch (endingMode)
            {
                case TaskStatus.RanToCompletion:
                    Assert.Null(result);
                    break;
                case TaskStatus.Faulted:
                    Assert.IsType<FormatException>(result);
                    break;
                case TaskStatus.Canceled:
                    Assert.IsAssignableFrom<OperationCanceledException>(result);
                    break;
            }

            Assert.Equal(numSubscribers * (writes.Length * (writes.Length + 1)) / 2, received);
        }

    }
}
