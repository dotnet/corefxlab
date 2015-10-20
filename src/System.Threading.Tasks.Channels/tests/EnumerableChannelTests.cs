// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class EnumerableChannelTests : TestBase
    {
        [Fact]
        public async Task Completion_NotEmpty_Idempotent()
        {
            IReadableChannel<int> c = Channel.CreateFromEnumerable(Enumerable.Range(0, 10));
            Assert.Same(c.Completion, c.Completion);
            while (await c.WaitToReadAsync())
            {
                int result;
                Assert.True(c.TryRead(out result));
            }
            await c.Completion;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Range_AllDataReadable_Success(int mode)
        {
            const int Start = 42, Count = 99;

            IReadableChannel<int> c = Channel.CreateFromEnumerable<int>(Enumerable.Range(Start, Count));
            Assert.False(c.Completion.IsCompleted);

            int result;
            for (int i = Start; i < Start + Count; i++)
            {
                switch (mode)
                {
                    case 0: // TryRead
                        Assert.True(c.TryRead(out result));
                        Assert.Equal(i, result);
                        break;

                    case 1: // WaitToReadAsync then TryRead
                        Assert.True(await c.WaitToReadAsync());
                        Assert.True(c.TryRead(out result));
                        Assert.Equal(i, result);
                        break;

                    case 2: // ReadAsync
                        Assert.Equal(i, await c.ReadAsync());
                        break;

                    case 3: // WaitToReadAsync then ReadAsync
                        Assert.True(await c.WaitToReadAsync());
                        Assert.Equal(i, await c.ReadAsync());
                        break;

                    case 4: // Multiple WaitToReadAsync then ReadAsync
                        Assert.True(await c.WaitToReadAsync());
                        Assert.True(await c.WaitToReadAsync());
                        Assert.True(await c.WaitToReadAsync());
                        Assert.True(await c.WaitToReadAsync());
                        Assert.Equal(i, await c.ReadAsync());
                        break;
                }
            }

            Assert.False(await c.WaitToReadAsync());
            Assert.False(await c.WaitToReadAsync());

            Assert.False(c.TryRead(out result));
            Assert.False(c.TryRead(out result));

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());

            await c.Completion;
        }

        [Fact]
        public async Task ReadAsync_NoMoreDataAvailable_ThrowsException()
        {
            IReadableChannel<int> c = Channel.CreateFromEnumerable(Enumerable.Empty<int>());
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => c.ReadAsync().AsTask());
        }

        [Fact]
        public void Precancellation_Reading_ReturnsCanceledImmediately()
        {
            IReadableChannel<int> c = Channel.CreateFromEnumerable(Enumerable.Empty<int>());
            var cts = new CancellationTokenSource();
            cts.Cancel();

            AssertSynchronouslyCanceled(c.ReadAsync(cts.Token).AsTask(), cts.Token);
            AssertSynchronouslyCanceled(c.WaitToReadAsync(cts.Token), cts.Token);
        }

        [Fact]
        public void Enumerator_DisposedWhenCompleted()
        {
            bool disposeCalled = false;
            IReadableChannel<int> c = Channel.CreateFromEnumerable(new DelegateEnumerable<int>
            {
                GetEnumeratorDelegate = () => new DelegateEnumerator<int>
                {
                    DisposeDelegate = () => disposeCalled = true
                }
            });

            int i;
            while (c.TryRead(out i))
            {
                Assert.False(disposeCalled);
            }
            Assert.True(disposeCalled);
        }
    }
}
