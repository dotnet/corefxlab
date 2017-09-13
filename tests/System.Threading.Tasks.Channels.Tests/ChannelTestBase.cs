// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Channels.Tests
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
                c.Writer.WriteAsync(i);
            }
            DebuggerAttributes.ValidateDebuggerDisplayReferences(c);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(c);
        }

        [Fact]
        public void Cast_MatchesInOut()
        {
            Channel<int> c = CreateChannel();
            ChannelReader<int> rc = c;
            ChannelWriter<int> wc = c;
            Assert.Same(rc, c.Reader);
            Assert.Same(wc, c.Writer);
        }

        [Fact]
        public void Completion_Idempotent()
        {
            Channel<int> c = CreateChannel();

            Task completion = c.Reader.Completion;
            Assert.Equal(TaskStatus.WaitingForActivation, completion.Status);

            Assert.Same(completion, c.Reader.Completion);
            c.Writer.Complete();
            Assert.Same(completion, c.Reader.Completion);

            Assert.Equal(TaskStatus.RanToCompletion, completion.Status);
        }

        [Fact]
        public void CompletionCancellationToken_Idempotent()
        {
            Channel<int> c = CreateChannel();

            CancellationToken ct1 = c.Reader.CompletionCancellationToken;
            CancellationToken ct2 = c.Reader.CompletionCancellationToken;

            Assert.Equal(ct1, ct2);
            Assert.False(ct1.IsCancellationRequested);
            Assert.True(ct1.CanBeCanceled);

            c.Writer.Complete();

            var mres = new ManualResetEventSlim();
            ct1.Register(mres.Set);
            mres.Wait();
        }

        [Fact]
        public void CompletionCancellationToken_ParallelAccess_Idempotent()
        {
            Channel<int> c = CreateChannel();
            CancellationToken[] tokens = new CancellationToken[2];
            using (var b = new Barrier(2, _ => { Assert.Equal(tokens[0], tokens[1]); c = CreateChannel(); }))
            {
                Task.WaitAll((from p in Enumerable.Range(0, b.ParticipantCount)
                              select Task.Run(() =>
                              {
                                  for (int i = 0; i < 1000; i++)
                                  {
                                      tokens[p] = c.Reader.CompletionCancellationToken;
                                      b.SignalAndWait();
                                  }
                              })).ToArray());
            }
        }

        [Fact]
        public void CompletionCancellationToken_AccessAfterCompletion_Canceled()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Assert.True(c.Reader.CompletionCancellationToken.IsCancellationRequested);
        }

        [Fact]
        public async Task Complete_AfterEmpty_NoWaiters_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            await c.Reader.Completion;
        }

        [Fact]
        public async Task Complete_AfterEmpty_WaitingReader_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            Task<int> r = c.Reader.ReadAsync().AsTask();
            c.Writer.Complete();
            await c.Reader.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => r);
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingReaders_TriggersCompletion()
        {
            Channel<int> c = CreateChannel();
            Task<int> read = c.Reader.ReadAsync().AsTask();
            c.Writer.Complete();
            await c.Reader.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => read);
        }

        [Fact]
        public void Complete_Twice_ThrowsInvalidOperationException()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Assert.ThrowsAny<InvalidOperationException>(() => c.Writer.Complete());
        }

        [Fact]
        public void TryComplete_Twice_ReturnsTrueThenFalse()
        {
            Channel<int> c = CreateChannel();
            Assert.True(c.Writer.TryComplete());
            Assert.False(c.Writer.TryComplete());
            Assert.False(c.Writer.TryComplete());
        }

        [Fact]
        public async Task TryComplete_ErrorsPropage()
        {
            Channel<int> c;

            // Success
            c = CreateChannel();
            Assert.True(c.Writer.TryComplete());
            await c.Reader.Completion;

            // Error
            c = CreateChannel();
            Assert.True(c.Writer.TryComplete(new FormatException()));
            await Assert.ThrowsAsync<FormatException>(() => c.Reader.Completion);

            // Canceled
            c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();
            Assert.True(c.Writer.TryComplete(new OperationCanceledException(cts.Token)));
            await AssertCanceled(c.Reader.Completion, cts.Token);
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
                        await c.Writer.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c.Reader.ReadAsync());
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
                        await c.Writer.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.Equal(i, await c.Reader);
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
                        Assert.Equal(i, await c1.Reader);
                        await c2.Writer.WriteAsync(i);
                    }
                }),
                Task.Run(async () =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        await c1.Writer.WriteAsync(i);
                        Assert.Equal(i, await c2.Reader);
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
            if (RequiresSingleReader && numReaders > 1)
            {
                return;
            }

            if (RequiresSingleWriter && numWriters > 1)
            {
                return;
            }

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
                        while (true)
                        {
                            Interlocked.Add(ref readTotal, await c.Reader.ReadAsync());
                        }
                    }
                    catch (ChannelClosedException) { }
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
                        await c.Writer.WriteAsync(value + 1);
                    }
                    if (Interlocked.Decrement(ref remainingWriters) == 0)
                    {
                        c.Writer.Complete();
                    }
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
            if (RequiresSingleReader && numReaders > 1)
            {
                return;
            }

            if (RequiresSingleWriter && numWriters > 1)
            {
                return;
            }

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
                        while (true)
                        {
                            Interlocked.Add(ref readTotal, await c.Reader);
                        }
                    }
                    catch (ChannelClosedException) { }
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
                        await c.Writer.WriteAsync(value + 1);
                    }
                    if (Interlocked.Decrement(ref remainingWriters) == 0)
                    {
                        c.Writer.Complete();
                    }
                });
            }

            Task.WaitAll(tasks);
            Assert.Equal((NumItems * (NumItems + 1L)) / 2, readTotal);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableBefore_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Writer.WriteAsync(42);
            Task<bool> read = c.Reader.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
        }

        [Fact]
        public void WaitToReadAsync_DataAvailableAfter_CompletesAsynchronously()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.Reader.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            Task write = c.Writer.WriteAsync(42);
            Assert.True(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_AfterComplete_SynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Task<bool> read = c.Reader.WaitToReadAsync();
            Assert.Equal(TaskStatus.RanToCompletion, read.Status);
            Assert.False(read.Result);
        }

        [Fact]
        public void WaitToReadAsync_BeforeComplete_AsynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.Reader.WaitToReadAsync();
            Assert.False(read.IsCompleted);
            c.Writer.Complete();
            Assert.False(read.Result);
        }

        [Fact]
        public async Task WaitToWriteAsync_AfterComplete_SynchronouslyCompletes()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Task<bool> write = c.Writer.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
            Assert.False(write.Result);
            Assert.False(await c.Writer);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableBefore_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            ValueTask<int> read = c.Reader.ReadAsync();
            Task<bool> write = c.Writer.WaitToWriteAsync();
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public void WaitToWriteAsync_SpaceAvailableAfter_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            Task<bool> write = c.Writer.WaitToWriteAsync();
            ValueTask<int> read = c.Reader.ReadAsync();
            Assert.True(write.Result);
        }

        [Fact]
        public void TryRead_DataAvailable_Success()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Writer.WriteAsync(42);
            Assert.True(c.Reader.TryRead(out int result));
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task ReadAsync_DataAvailable_Success()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Writer.WriteAsync(42);
            Assert.Equal(42, await c.Reader.ReadAsync());
        }

        [Fact]
        public void TryRead_AfterComplete_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Assert.False(c.Reader.TryRead(out int result));
        }

        [Fact]
        public void TryWrite_AfterComplete_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Assert.False(c.Writer.TryWrite(42));
        }

        [Fact]
        public async Task WriteAsync_AfterComplete_ThrowsException()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Writer.WriteAsync(42));
        }

        [Fact]
        public async Task ReadAsync_AfterComplete_ThrowsException()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.Reader.ReadAsync().AsTask());
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToCompletion()
        {
            Channel<int> c = CreateChannel();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Assert.Same(exc, await Assert.ThrowsAsync<FormatException>(() => c.Reader.Completion));
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

            c.Writer.Complete(exc);
            await AssertCanceled(c.Reader.Completion, cts.Token);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingReader()
        {
            Channel<int> c = CreateChannel();
            Task<int> read = c.Reader.ReadAsync().AsTask();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(() => read)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWriter()
        {
            Channel<int> c = CreateFullChannel();
            if (c != null)
            {
                Task write = c.Writer.WriteAsync(42);
                var exc = new FormatException();
                c.Writer.Complete(exc);
                Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(() => write)).InnerException);
            }
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewReader()
        {
            Channel<int> c = CreateChannel();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Task<int> read = c.Reader.ReadAsync().AsTask();
            Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(() => read)).InnerException);
            Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(async () => await c.Reader)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWriter()
        {
            Channel<int> c = CreateChannel();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Task write = c.Writer.WriteAsync(42);
            Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(() => write)).InnerException);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWaitingReader()
        {
            Channel<int> c = CreateChannel();
            Task<bool> read = c.Reader.WaitToReadAsync();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            await Assert.ThrowsAsync<FormatException>(() => read);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToExistingWaitingWriter()
        {
            Channel<int> c = CreateFullChannel();
            if (c != null)
            {
                Task<bool> write = c.Writer.WaitToWriteAsync();
                var exc = new FormatException();
                c.Writer.Complete(exc);
                await Assert.ThrowsAsync<FormatException>(() => write);
            }
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWaitingReader()
        {
            Channel<int> c = CreateChannel();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Task<bool> read = c.Reader.WaitToReadAsync();
            await Assert.ThrowsAsync<FormatException>(() => read);
        }

        [Fact]
        public async Task Complete_WithException_PropagatesToNewWaitingWriter()
        {
            Channel<int> c = CreateChannel();
            var exc = new FormatException();
            c.Writer.Complete(exc);
            Task<bool> write = c.Writer.WaitToWriteAsync();
            await Assert.ThrowsAsync<FormatException>(() => write);
            await Assert.ThrowsAsync<FormatException>(async () => await c.Writer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ManyWriteAsync_ThenManyReadAsync_Success(int readMode)
        {
            if (RequiresSingleReader || RequiresSingleWriter)
            {
                return;
            }

            Channel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i] = c.Writer.WriteAsync(i);
            }

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                switch (readMode)
                {
                    case 0:
                        readers[i] = c.Reader.ReadAsync().AsTask();
                        break;
                    case 1:
                        int result;
                        Assert.True(c.Reader.TryRead(out result));
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
            if (RequiresSingleReader || RequiresSingleReader)
            {
                return;
            }

            Channel<int> c = CreateChannel();

            const int NumItems = 2000;

            Task<int>[] readers = new Task<int>[NumItems];
            for (int i = 0; i < readers.Length; i++)
            {
                readers[i] = c.Reader.ReadAsync().AsTask();
            }

            Task[] writers = new Task[NumItems];
            for (int i = 0; i < writers.Length; i++)
            {
                switch (writeMode)
                {
                    case 0:
                        writers[i] = c.Writer.WriteAsync(i);
                        break;
                    case 1:
                        Assert.True(c.Writer.TryWrite(i));
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

            AssertSynchronouslyCanceled(c.Reader.ReadAsync(cts.Token).AsTask(), cts.Token);
            AssertSynchronouslyCanceled(c.Reader.WaitToReadAsync(cts.Token), cts.Token);
        }

        [Fact]
        public void Precancellation_Writing_ReturnsSuccessImmediately()
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Task writeTask = c.Writer.WriteAsync(42, cts.Token);
            Assert.True(writeTask.Status == TaskStatus.Canceled || writeTask.Status == TaskStatus.RanToCompletion, $"Status == {writeTask.Status}");

            Task<bool> waitTask = c.Writer.WaitToWriteAsync(cts.Token);
            Assert.True(writeTask.Status == TaskStatus.Canceled || writeTask.Status == TaskStatus.RanToCompletion, $"Status == {writeTask.Status}");
            if (waitTask.Status == TaskStatus.RanToCompletion)
            {
                Assert.True(waitTask.Result);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CancelPendingRead_Writing_DataTransferredToCorrectReader(int writeMode)
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();

            Task<int> read1 = c.Reader.ReadAsync(cts.Token).AsTask();
            cts.Cancel();
            Task<int> read2 = c.Reader.ReadAsync().AsTask();

            switch (writeMode)
            {
                case 0: await c.Writer.WriteAsync(42); break;
                case 1: Assert.True(c.Writer.TryWrite(42)); break;
            }

            Assert.Equal(42, await read2);
            await AssertCanceled(read1, cts.Token);
        }

        [Fact]
        public async Task CancelPendingRead_WrittenDataPendingOrStored()
        {
            Channel<int> c = CreateChannel();
            var cts = new CancellationTokenSource();

            Task<int> read1 = c.Reader.ReadAsync(cts.Token).AsTask();
            cts.Cancel();
            await AssertCanceled(read1, cts.Token);

            Task write = c.Writer.WriteAsync(42);

            Assert.Equal(42, await c.Reader.ReadAsync());
            await write;
        }

        [Fact]
        public async Task AwaitRead_CompletedChannel_Throws()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await c.Reader);
        }

        [Fact]
        public async Task AwaitRead_ChannelAfterExistingData_ReturnsData()
        {
            Channel<int> c = CreateChannel();
            Task write = c.Writer.WriteAsync(42);
            Assert.Equal(42, await c.Reader);
            Assert.Equal(TaskStatus.RanToCompletion, write.Status);
        }

        [Fact]
        public async Task AwaitWriteAvailable_CompletedChannel_ReturnsFalse()
        {
            Channel<int> c = CreateChannel();
            c.Writer.Complete();
            Assert.False(await c.Writer);
        }

        [Fact]
        public async Task AwaitWriteAvailable_ChannelWhenPendingRead_ReturnsTrue()
        {
            Channel<int> c = CreateChannel();
            ValueTask<int> pendingRead = c.Reader.ReadAsync();
            Assert.False(pendingRead.IsCompleted);
            Assert.True(await c.Writer);
        }

        

        [Fact]
        public async Task TryWrite_BlockedReader_Success()
        {
            Channel<int> c = CreateChannel();
            Task<int> read = c.Reader.ReadAsync().AsTask();
            Assert.False(read.IsCompleted);
            Assert.True(c.Writer.TryWrite(42));
            Assert.Equal(42, await read);
        }

        [Fact]
        public void Write_WaitToReadAsync_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            c.Writer.WriteAsync(42);
            AssertSynchronousTrue(c.Reader.WaitToReadAsync());
        }

        [Fact]
        public void Read_WaitToWriteAsync_CompletesSynchronously()
        {
            Channel<int> c = CreateChannel();
            c.Reader.ReadAsync();
            AssertSynchronousTrue(c.Writer.WaitToWriteAsync());
        }

        [Theory]
        [InlineData(TaskStatus.RanToCompletion)]
        [InlineData(TaskStatus.Faulted)]
        [InlineData(TaskStatus.Canceled)]
        public async Task AsObserver_DataWritten(TaskStatus endingMode)
        {
            Channel<int> c = CreateChannel();
            IObserver<int> o = c.Writer.AsObserver();

            var reader = Task.Run(async () =>
            {
                int received = 0;
                try
                {
                    while (true)
                    {
                        Assert.Equal(received++, await c.Reader.ReadAsync());
                    }
                }
                catch (ChannelClosedException e) when (e.InnerException == null) { }
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
                    Assert.Same(exc, (await Assert.ThrowsAsync<ChannelClosedException>(() => reader)).InnerException);
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
            if (RequiresSingleWriter)
            {
                return;
            }

            Channel<int> c = CreateChannel();

            int received = 0;
            var tcs = new TaskCompletionSource<Exception>();

            IObservable<int> o = c.Reader.AsObservable();
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
                writes[i] = c.Writer.WriteAsync(i + 1);
            }
            await Task.WhenAll(writes);

            c.Writer.Complete(
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
