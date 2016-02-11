// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class SingleReaderWriterUnboundedChannelTests : UnboundedChannelTests
    {
        protected override IChannel<int> CreateChannel()
        {
            return Channel.Create<int>(singleReaderWriter: true);
        }

        protected override bool RequiresSingleReaderWriter { get { return true; } }

        [Fact]
        public void ValidateInternalDebuggerAttributes()
        {
            IChannel<int> c = CreateChannel();
            Assert.True(c.TryWrite(1));
            Assert.True(c.TryWrite(2));

            var queue = DebuggerAttributes.GetFieldValue(c, "_items");
            DebuggerAttributes.ValidateDebuggerDisplayReferences(queue);
            DebuggerAttributes.ValidateDebuggerTypeProxyProperties(queue);
        }

        [Fact]
        public async Task MultipleWaiters_ThrowsInvalid()
        {
            IChannel<int> c = CreateChannel();
            Task t1 = c.WaitToReadAsync();
            Task t2 = c.WaitToReadAsync();
            await Assert.ThrowsAsync<InvalidOperationException>(() => t1);
        }

        [Fact]
        public async Task MultipleReaders_ThrowsInvalid()
        {
            IChannel<int> c = CreateChannel();
            ValueTask<int> t1 = c.ReadAsync();
            ValueTask<int> t2 = c.ReadAsync();
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await t2);
        }

        [Fact]
        public void Stress_TryWrite_TryRead()
        {
            const int NumItems = 3000000;
            IChannel<int> c = CreateChannel();

            Task.WaitAll(
                Task.Run(async () =>
                {
                    int received = 0;
                    while (await c.WaitToReadAsync())
                    {
                        int i;
                        while (c.TryRead(out i))
                        {
                            Assert.Equal(received, i);
                            received++;
                        }
                    }
                }),
                Task.Run(() =>
                {
                    for (int i = 0; i < NumItems; i++)
                    {
                        Assert.True(c.TryWrite(i));
                    }
                    c.Complete();
                }));
        }
    }

    public class MultiReaderWriterUnboundedChannelTests : UnboundedChannelTests
    {
        protected override IChannel<int> CreateChannel()
        {
            return Channel.Create<int>(singleReaderWriter: false);
        }
    }

    public abstract class UnboundedChannelTests : ChannelTestBase
    {
        [Fact]
        public async Task Complete_BeforeEmpty_NoWaiters_TriggersCompletion()
        {
            IChannel<int> c = CreateChannel();
            Assert.True(c.TryWrite(42));
            c.Complete();
            Assert.False(c.Completion.IsCompleted);
            Assert.Equal(42, await c.ReadAsync());
            await c.Completion;
        }

        [Fact]
        public void TryWrite_TryRead_Many()
        {
            IChannel<int> c = CreateChannel();

            const int NumItems = 100000;
            for (int i = 0; i < NumItems; i++)
            {
                Assert.True(c.TryWrite(i));
            }
            for (int i = 0; i < NumItems; i++)
            {
                int result;
                Assert.True(c.TryRead(out result));
                Assert.Equal(i, result);
            }
        }

        [Fact]
        public void TryWrite_TryRead_OneAtATime()
        {
            IChannel<int> c = CreateChannel();

            for (int i = 0; i < 10; i++)
            {
                Assert.True(c.TryWrite(i));
                int result;
                Assert.True(c.TryRead(out result));
                Assert.Equal(i, result);
            }
        }

        [Fact]
        public void WaitForReadAsync_DataAvailable_CompletesSynchronously()
        {
            IChannel<int> c = CreateChannel();
            Assert.True(c.TryWrite(42));
            AssertSynchronousTrue(c.WaitToReadAsync());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task WriteMany_ThenComplete_SuccessfullyReadAll(int readMode)
        {
            IChannel<int> c = CreateChannel();
            for (int i = 0; i < 10; i++)
            {
                Assert.True(c.TryWrite(i));
            }

            c.Complete();
            Assert.False(c.Completion.IsCompleted);

            for (int i = 0; i < 10; i++)
            {
                Assert.False(c.Completion.IsCompleted);
                switch (readMode)
                {
                    case 0:
                        int result;
                        Assert.True(c.TryRead(out result));
                        Assert.Equal(i, result);
                        break;
                    case 1:
                        Assert.Equal(i, await c.ReadAsync());
                        break;
                }
            }

            await c.Completion;
        }

    }
}
