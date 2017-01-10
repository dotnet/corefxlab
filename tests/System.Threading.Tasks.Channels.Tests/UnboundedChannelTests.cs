// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public abstract class UnboundedChannelTests : ChannelTestBase
    {
        protected abstract bool AllowSynchronousContinuations { get; }
        protected override IChannel<int> CreateChannel() => Channel.CreateUnbounded<int>(
            new ChannelOptimizations
            {
                SingleReader = this.RequiresSingleReader,
                AllowSynchronousContinuations = this.AllowSynchronousContinuations
            });

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

        [Fact]
        public void AllowSynchronousContinuations_ContinuationsInvokedAccordingToSetting()
        {
            IChannel<int> c = CreateChannel();

            int expectedId = Environment.CurrentManagedThreadId;
            Task r = c.ReadAsync().AsTask().ContinueWith(_ =>
            {
                Assert.Equal(AllowSynchronousContinuations, expectedId == Environment.CurrentManagedThreadId);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            Assert.Equal(TaskStatus.RanToCompletion, c.WriteAsync(42).Status);
            ((IAsyncResult)r).AsyncWaitHandle.WaitOne(); // avoid inlining the continuation
            r.GetAwaiter().GetResult();
        }
    }

    public abstract class SingleReaderUnboundedChannelTests : UnboundedChannelTests
    {
        protected override bool RequiresSingleReader => true;

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
        public async Task MultipleWaiters_CancelsPreviousWaiter()
        {
            IChannel<int> c = CreateChannel();
            Task<bool> t1 = c.WaitToReadAsync();
            Task<bool> t2 = c.WaitToReadAsync();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => t1);
            Assert.True(c.TryWrite(42));
            Assert.True(await t2);
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

    public sealed class SyncMultiReaderUnboundedChannelTests : UnboundedChannelTests
    {
        protected override bool AllowSynchronousContinuations => true;
    }

    public sealed class AsyncMultiReaderUnboundedChannelTests : UnboundedChannelTests
    {
        protected override bool AllowSynchronousContinuations => false;
    }

    public sealed class SyncSingleReaderUnboundedChannelTests : SingleReaderUnboundedChannelTests
    {
        protected override bool AllowSynchronousContinuations => true;
    }

    public sealed class AsyncSingleReaderUnboundedChannelTests : SingleReaderUnboundedChannelTests
    {
        protected override bool AllowSynchronousContinuations => false;
    }
}
