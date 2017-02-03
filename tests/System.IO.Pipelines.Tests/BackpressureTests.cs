using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class BackpressureTests : IDisposable
    {
        private PipelineFactory _pipelineFactory;

        private Pipe _pipe;

        public BackpressureTests()
        {
            _pipelineFactory = new PipelineFactory();
            _pipe = _pipelineFactory.Create(new PipeOptions
            {
                MaximumSizeLow = 32,
                MaximumSizeHigh = 64
            });
        }

        public void Dispose()
        {
            _pipe.CompleteWriter();
            _pipe.CompleteReader();
            _pipelineFactory?.Dispose();
        }

        [Fact]
        public void FlushAsyncReturnsCompletedTaskWhenSizeLessThenLimit()
        {
            var writableBuffer = _pipe.Alloc(32);
            writableBuffer.Advance(32);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.True(flushAsync.IsCompleted);
            Assert.True(flushAsync.GetResult());
        }

        [Fact]
        public void FlushAsyncReturnsNonCompletedSizeWhenCommitOverTheLimit()
        {
            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.False(flushAsync.IsCompleted);
        }

        [Fact]
        public void FlushAsyncAwaitableCompletesWhenReaderAdvancesUnderLow()
        {
            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 33);
            _pipe.AdvanceReader(consumed, consumed);

            Assert.True(flushAsync.IsCompleted);
            Assert.True(flushAsync.GetResult());
        }

        [Fact]
        public void FlushAsyncAwaitableDoesNotCompletesWhenReaderAdvancesUnderHight()
        {
            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 32);
            _pipe.AdvanceReader(consumed, consumed);

            Assert.False(flushAsync.IsCompleted);
        }

        [Fact]
        public async Task FlushAsyncThrowsIfReaderCompletedWithException()
        {
            _pipe.CompleteReader(new InvalidOperationException("Reader failed"));

            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await writableBuffer.FlushAsync());
            Assert.Equal("Reader failed", invalidOperationException.Message);
            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await writableBuffer.FlushAsync());
            Assert.Equal("Reader failed", invalidOperationException.Message);
        }

        [Fact]
        public void FlushAsyncReturnsFalseIfReaderCompletes()
        {
            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            _pipe.CompleteReader();

            Assert.True(flushAsync.IsCompleted);
            Assert.False(flushAsync.GetResult());
        }

        [Fact]
        public void FlushAsyncAwaitableResetsOnCommit()
        {
            var writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            var result = _pipe.ReadAsync().GetAwaiter().GetResult();
            var consumed = result.Buffer.Move(result.Buffer.Start, 33);
            _pipe.AdvanceReader(consumed, consumed);

            Assert.True(flushAsync.IsCompleted);
            Assert.True(flushAsync.GetResult());

            writableBuffer = _pipe.Alloc(64);
            writableBuffer.Advance(64);
            flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);
        }
    }
}
