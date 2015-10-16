// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class BoundedChannelTests : ChannelTestBase
    {
        protected override IChannel<int> CreateChannel()
        {
            return Channel.Create<int>(1);
        }

        [Fact]
        public async Task Complete_BeforeEmpty_NoWaiters_TriggersCompletion()
        {
            IChannel<int> c = Channel.Create<int>(1);
            Assert.True(c.TryWrite(42));
            c.Complete();
            Assert.False(c.Completion.IsCompleted);
            Assert.Equal(42, await c.ReadAsync());
            await c.Completion;
        }

        [Fact]
        public async Task Complete_BeforeEmpty_WaitingWriters_TriggersCompletion()
        {
            IChannel<int> c = Channel.Create<int>(1);
            Assert.True(c.TryWrite(42));
            Task write2 = c.WriteAsync(43);
            c.Complete();
            Assert.Equal(42, await c.ReadAsync());
            await c.Completion;
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => write2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_Many(int bufferedCapacity)
        {
            IChannel<int> c = Channel.Create<int>(bufferedCapacity);

            for (int i = 0; i < bufferedCapacity; i++)
            {
                Assert.True(c.TryWrite(i));
            }
            Assert.False(c.TryWrite(bufferedCapacity));

            int result;
            for (int i = 0; i < bufferedCapacity; i++)
            {
                Assert.True(c.TryRead(out result));
                Assert.Equal(i, result);
            }

            Assert.False(c.TryRead(out result));
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(10000)]
        public void TryWrite_TryRead_OneAtATime(int bufferedCapacity)
        {
            IChannel<int> c = Channel.Create<int>(bufferedCapacity);

            const int NumItems = 100000;
            for (int i = 0; i < NumItems; i++)
            {
                Assert.True(c.TryWrite(i));
                int result;
                Assert.True(c.TryRead(out result));
                Assert.Equal(i, result);
            }
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(10, false)]
        [InlineData(10000, false)]
        public void SingleProducerConsumer_ConcurrentReadWrite_Success(int bufferedCapacity, bool singleProducerConsumer)
        {
            IChannel<int> c = Channel.Create<int>(bufferedCapacity, singleProducerConsumer);

            const int NumItems = 10000;
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

        [Theory]
        [InlineData(1, false)]
        [InlineData(10, false)]
        [InlineData(10000, false)]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(10000, true)]
        public void ManyProducerConsumer_ConcurrentReadWrite_Success(int bufferedCapacity, bool singleProducerConsumer)
        {
            IChannel<int> c = Channel.Create<int>(bufferedCapacity, singleProducerConsumer);

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
        public async Task WaitToWriteAsync_AfterFullThenRead_ReturnsTrue()
        {
            IChannel<int> c = Channel.Create<int>(1);
            Assert.True(c.TryWrite(1));

            Task<bool> write1 = c.WaitToWriteAsync();
            Assert.False(write1.IsCompleted);

            Task<bool> write2 = c.WaitToWriteAsync();
            Assert.False(write2.IsCompleted);

            Assert.Equal(1, await c.ReadAsync());

            Assert.True(await write1);
            Assert.True(await write2);
        }

    }
}
