// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class BoundedChannelTests : ChannelTestBase
    {
        protected override Channel<int> CreateChannel()
        {
            return Channel.CreateBounded<int>(1);
        }

        [Fact]
        public async Task Complete_BeforeEmpty_NoWaiters_TriggersCompletion()
        {
            Channel<int> c = Channel.CreateBounded<int>(1);
            Assert.True(c.Out.TryWrite(42));
            c.Out.Complete();
            Assert.False(c.In.Completion.IsCompleted);
            Assert.Equal(42, await c.In.ReadAsync());
            await c.In.Completion;
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingWriters_TriggersCompletion()
        {
            Channel<int> c = Channel.CreateBounded<int>(1);
            Assert.True(c.Out.TryWrite(42));
            Task write2 = c.Out.WriteAsync(43);
            c.Out.Complete();
            Assert.Equal(42, await c.In.ReadAsync());
            await c.In.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => write2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_Many_Wait(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity);

            for (int i = 0; i < bufferedCapacity; i++)
            {
                Assert.True(c.Out.TryWrite(i));
            }
            Assert.False(c.Out.TryWrite(bufferedCapacity));

            int result;
            for (int i = 0; i < bufferedCapacity; i++)
            {
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }

            Assert.False(c.In.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_Many_DropOldest(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity, BoundedChannelFullMode.DropOldest);

            for (int i = 0; i < bufferedCapacity * 2; i++)
            {
                Assert.True(c.Out.TryWrite(i));
            }

            int result;
            for (int i = bufferedCapacity; i < bufferedCapacity * 2; i++)
            {
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }

            Assert.False(c.In.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void WriteAsync_TryRead_Many_DropOldest(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity, BoundedChannelFullMode.DropOldest);

            for (int i = 0; i < bufferedCapacity * 2; i++)
            {
                Assert.Equal(TaskStatus.RanToCompletion, c.Out.WriteAsync(i).Status);
            }

            int result;
            for (int i = bufferedCapacity; i < bufferedCapacity * 2; i++)
            {
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }

            Assert.False(c.In.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_Many_DropNewest(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity, BoundedChannelFullMode.DropNewest);

            for (int i = 0; i < bufferedCapacity * 2; i++)
            {
                Assert.True(c.Out.TryWrite(i));
            }

            int result;
            for (int i = 0; i < bufferedCapacity - 1; i++)
            {
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }
            Assert.True(c.In.TryRead(out result));
            Assert.Equal(bufferedCapacity * 2 - 1, result);

            Assert.False(c.In.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void WriteAsync_TryRead_Many_DropNewest(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity, BoundedChannelFullMode.DropNewest);

            for (int i = 0; i < bufferedCapacity * 2; i++)
            {
                Assert.Equal(TaskStatus.RanToCompletion, c.Out.WriteAsync(i).Status);
            }

            int result;
            for (int i = 0; i < bufferedCapacity - 1; i++)
            {
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }
            Assert.True(c.In.TryRead(out result));
            Assert.Equal(bufferedCapacity * 2 - 1, result);

            Assert.False(c.In.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task TryWrite_DropNewest_WrappedAroundInternalQueue()
        {
            Channel<int> c = Channel.CreateBounded<int>(3, BoundedChannelFullMode.DropNewest);

            // Move head of dequeue beyond the beginning
            Assert.True(c.Out.TryWrite(1));
            Assert.Equal(1, await c.In.ReadAsync());

            // Add items to fill the capacity and put the tail at 0
            Assert.True(c.Out.TryWrite(2));
            Assert.True(c.Out.TryWrite(3));
            Assert.True(c.Out.TryWrite(4));

            // Add an item to overwrite the newest
            Assert.True(c.Out.TryWrite(5));

            // Verify current contents
            Assert.Equal(2, await c.In.ReadAsync());
            Assert.Equal(3, await c.In.ReadAsync());
            Assert.Equal(5, await c.In.ReadAsync());
        }

        [Fact]
        public async Task CancelPendingWrite_Reading_DataTransferredFromCorrectWriter()
        {
            Channel<int> c = Channel.CreateBounded<int>(1);
            Assert.Equal(TaskStatus.RanToCompletion, c.Out.WriteAsync(42).Status);

            var cts = new CancellationTokenSource();

            Task write1 = c.Out.WriteAsync(43, cts.Token);
            Assert.Equal(TaskStatus.WaitingForActivation, write1.Status);

            cts.Cancel();

            Task write2 = c.Out.WriteAsync(44);

            Assert.Equal(42, await c.In);
            Assert.Equal(44, await c.In);

            await AssertCanceled(write1, cts.Token);
            await write2;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_OneAtATime(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity);

            const int NumItems = 100000;
            for (int i = 0; i < NumItems; i++)
            {
                Assert.True(c.Out.TryWrite(i));
                int result;
                Assert.True(c.In.TryRead(out result));
                Assert.Equal(i, result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void SingleProducerConsumer_ConcurrentReadWrite_Success(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity);

            const int NumItems = 10000;
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

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void ManyProducerConsumer_ConcurrentReadWrite_Success(int bufferedCapacity)
        {
            Channel<int> c = Channel.CreateBounded<int>(bufferedCapacity);

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
                    IAsyncEnumerator<int> e = c.In.GetAsyncEnumerator();
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
        public async Task WaitToWriteAsync_AfterFullThenRead_ReturnsTrue()
        {
            Channel<int> c = Channel.CreateBounded<int>(1);
            Assert.True(c.Out.TryWrite(1));

            Task<bool> write1 = c.Out.WaitToWriteAsync();
            Assert.False(write1.IsCompleted);

            Task<bool> write2 = c.Out.WaitToWriteAsync();
            Assert.False(write2.IsCompleted);

            Assert.Equal(1, await c.In.ReadAsync());

            Assert.True(await write1);
            Assert.True(await write2);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AllowSynchronousContinuations_ReadAsync_ContinuationsInvokedAccordingToSetting(bool allowSynchronousContinuations)
        {
            Channel<int> c = Channel.CreateBounded<int>(1, optimizations: new ChannelOptimizations { AllowSynchronousContinuations = allowSynchronousContinuations });

            int expectedId = Environment.CurrentManagedThreadId;
            Task r = c.In.ReadAsync().AsTask().ContinueWith(_ =>
            {
                Assert.Equal(allowSynchronousContinuations, expectedId == Environment.CurrentManagedThreadId);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            Assert.Equal(TaskStatus.RanToCompletion, c.Out.WriteAsync(42).Status);
            ((IAsyncResult)r).AsyncWaitHandle.WaitOne(); // avoid inlining the continuation
            r.GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AllowSynchronousContinuations_CompletionTask_ContinuationsInvokedAccordingToSetting(bool allowSynchronousContinuations)
        {
            Channel<int> c = Channel.CreateBounded<int>(1, optimizations: new ChannelOptimizations { AllowSynchronousContinuations = allowSynchronousContinuations });

            int expectedId = Environment.CurrentManagedThreadId;
            Task r = c.In.Completion.ContinueWith(_ =>
            {
                Assert.Equal(allowSynchronousContinuations, expectedId == Environment.CurrentManagedThreadId);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            Assert.True(c.Out.TryComplete());
            ((IAsyncResult)r).AsyncWaitHandle.WaitOne(); // avoid inlining the continuation
            r.GetAwaiter().GetResult();
        }
    }
}
