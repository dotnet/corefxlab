// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class BackpressureTests : IDisposable
    {
        private MemoryPool _pool;
        private Pipe _pipe;

        public BackpressureTests()
        {
            _pool = new MemoryPool();
            _pipe = new Pipe(new PipeOptions(_pool) {
                MaximumSizeLow = 32,
                MaximumSizeHigh = 64
            });
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        [Fact]
        public void FlushAsyncReturnsCompletedTaskWhenSizeLessThenLimit()
        {
            var writableBuffer = _pipe.Writer.Alloc(32);
            writableBuffer.Advance(32);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.False(flushResult.IsCompleted);
        }

        [Fact]
        public void FlushAsyncReturnsNonCompletedSizeWhenCommitOverTheLimit()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.False(flushAsync.IsCompleted);
        }

        [Fact]
        public void FlushAsyncAwaitableCompletesWhenReaderAdvancesUnderLow()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 33);
            _pipe.Reader.Advance(consumed, consumed);

            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.False(flushResult.IsCompleted);
        }

        [Fact]
        public void FlushAsyncAwaitableDoesNotCompletesWhenReaderAdvancesUnderHight()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 32);
            _pipe.Reader.Advance(consumed, consumed);

            Assert.False(flushAsync.IsCompleted);
        }

        [Fact]
        public async Task FlushAsyncThrowsIfReaderCompletedWithException()
        {
            _pipe.Reader.Complete(new InvalidOperationException("Reader failed"));

            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await writableBuffer.FlushAsync());
            Assert.Equal("Reader failed", invalidOperationException.Message);
            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await writableBuffer.FlushAsync());
            Assert.Equal("Reader failed", invalidOperationException.Message);
        }

        [Fact]
        public void FlushAsyncReturnsCompletedIfReaderCompletes()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            _pipe.Reader.Complete();

            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.True(flushResult.IsCompleted);
        }

        [Fact]
        public void FlushAsyncAwaitableResetsOnCommit()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 33);
            _pipe.Reader.Advance(consumed, consumed);

            Assert.True(flushAsync.IsCompleted);
            var flushResult = flushAsync.GetResult();
            Assert.False(flushResult.IsCompleted);

            writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);
        }

        [Fact]
        public void AdvanceThrowsIfFlushActiveAndNotConsumedPastThreshold()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 31);
            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.Advance(consumed, result.Buffer.End));

            _pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);
        }
    }
}
