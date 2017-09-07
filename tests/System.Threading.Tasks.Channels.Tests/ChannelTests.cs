// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace System.Threading.Tasks.Channels.Tests
{
    public class ChannelTests
    {
        [Fact]
        public void ChannelOptimizations_Properties_Roundtrip()
        {
            var co = new ChannelOptimizations();

            Assert.False(co.SingleReader);
            Assert.False(co.SingleWriter);

            co.SingleReader = true;
            Assert.True(co.SingleReader);
            Assert.False(co.SingleWriter);
            co.SingleReader = false;
            Assert.False(co.SingleReader);

            co.SingleWriter = true;
            Assert.False(co.SingleReader);
            Assert.True(co.SingleWriter);
            co.SingleWriter = false;
            Assert.False(co.SingleWriter);

            co.SingleReader = true;
            co.SingleWriter = true;
            Assert.True(co.SingleReader);
            Assert.True(co.SingleWriter);

            Assert.False(co.AllowSynchronousContinuations);
            co.AllowSynchronousContinuations = true;
            Assert.True(co.AllowSynchronousContinuations);
            co.AllowSynchronousContinuations = false;
            Assert.False(co.AllowSynchronousContinuations);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void CreateBounded_InvalidBufferSizes_ThrowArgumentExceptions(int bufferedCapacity) =>
            Assert.Throws<ArgumentOutOfRangeException>("bufferedCapacity", () => Channel.CreateBounded<int>(bufferedCapacity));

        [Theory]
        [InlineData((BoundedChannelFullMode)(-1))]
        [InlineData((BoundedChannelFullMode)(4))]
        public void CreateBounded_InvalidModes_ThrowArgumentExceptions(BoundedChannelFullMode mode) =>
            Assert.Throws<ArgumentOutOfRangeException>("mode", () => Channel.CreateBounded<int>(1, mode));

        [Theory]
        [InlineData(1)]
        public void CreateBounded_ValidBufferSizes_Success(int bufferedCapacity) =>
            Assert.NotNull(Channel.CreateBounded<int>(bufferedCapacity));

        [Fact]
        public void AsObservable_SameSource_Idempotent()
        {
            var c = Channel.CreateUnbounded<int>();
            Assert.Same(c.In.AsObservable(), c.In.AsObservable());
        }

        [Fact]
        public async Task DefaultReadAsync_UsesWaitToReadAsyncAndTryRead()
        {
            var c = new TestReadableChannel<int>(Enumerable.Range(10, 10));
            Assert.NotNull(c.Completion);
            Assert.False(c.Completion.IsCompleted);
            Assert.Equal(TaskStatus.Canceled, c.ReadAsync(new CancellationToken(true)).AsTask().Status);

            for (int i = 10; i < 20; i++)
            {
                Assert.Equal(i, await c.ReadAsync());
            }
            await Assert.ThrowsAsync<ClosedChannelException>(async () => await c.ReadAsync());
        }

        [Fact]
        public async Task DefaultWriteAsync_UsesWaitToWriteAsyncAndTryWrite()
        {
            var c = new TestWritableChannel<int>(10);
            Assert.False(c.TryComplete());
            Assert.Equal(TaskStatus.Canceled, c.WriteAsync(42, new CancellationToken(true)).Status);

            int count = 0;
            try
            {
                while (true)
                {
                    await c.WriteAsync(count++);
                }
            }
            catch (ClosedChannelException) { }
            Assert.Equal(11, count);
        }

        private sealed class TestWritableChannel<T> : WritableChannel<T>
        {
            private readonly Random _rand = new Random(42);
            private readonly int _max;
            private int _count;

            public TestWritableChannel(int max) => _max = max;

            public override bool TryWrite(T item) => _rand.Next(0, 2) == 0 && _count++ < _max; // succeed if we're under our limit, and add random failures

            public override Task<bool> WaitToWriteAsync(CancellationToken cancellationToken) =>
                _count >= _max ? Task.FromResult(false) :
                _rand.Next(0, 2) == 0 ? Task.Delay(1).ContinueWith(_ => true) : // randomly introduce delays
                Task.FromResult(true);
        }

        private sealed class TestReadableChannel<T> : ReadableChannel<T>
        {
            private Random _rand = new Random(42);
            private IEnumerator<T> _enumerator;
            private int _count;
            private bool _closed;

            public TestReadableChannel(IEnumerable<T> enumerable) => _enumerator = enumerable.GetEnumerator();

            public override bool TryRead(out T item)
            {
                // Randomly fail to read
                if (_rand.Next(0, 2) == 0)
                {
                    item = default(T);
                    return false;
                }

                // If the enumerable is closed, fail the read.
                if (!_enumerator.MoveNext())
                {
                    _enumerator.Dispose();
                    _closed = true;
                    item = default(T);
                    return false;
                }

                // Otherwise return the next item.
                _count++;
                item = _enumerator.Current;
                return true;
            }

            public override Task<bool> WaitToReadAsync(CancellationToken cancellationToken) =>
                _closed ? Task.FromResult(false) :
                _rand.Next(0, 2) == 0 ? Task.Delay(1).ContinueWith(_ => true) : // randomly introduce delays
                Task.FromResult(true);
        }

        private sealed class CanReadFalseStream : MemoryStream
        {
            public override bool CanRead => false;
        }
    }
}
