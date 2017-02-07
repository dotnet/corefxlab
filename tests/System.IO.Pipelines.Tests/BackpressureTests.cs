﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class BackpressureTests : IDisposable
    {
        private PipeFactory _pipeFactory;

        private IPipe _pipe;

        public BackpressureTests()
        {
            _pipeFactory = new PipeFactory();
            _pipe = _pipeFactory.Create(new PipeOptions
            {
                MaximumSizeLow = 32,
                MaximumSizeHigh = 64
            });
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory?.Dispose();
        }

        [Fact(Skip = "Trying to find a hang")]
        public void FlushAsyncReturnsCompletedTaskWhenSizeLessThenLimit()
        {
            var writableBuffer = _pipe.Writer.Alloc(32);
            writableBuffer.Advance(32);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.True(flushAsync.IsCompleted);
            Assert.True(flushAsync.GetResult());
        }

        [Fact(Skip = "Trying to find a hang")]
        public void FlushAsyncReturnsNonCompletedSizeWhenCommitOverTheLimit()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();
            Assert.False(flushAsync.IsCompleted);
        }

        [Fact(Skip = "Trying to find a hang")]
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
            Assert.True(flushAsync.GetResult());
        }

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
        public void FlushAsyncReturnsFalseIfReaderCompletes()
        {
            var writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            var flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);

            _pipe.Reader.Complete();

            Assert.True(flushAsync.IsCompleted);
            Assert.False(flushAsync.GetResult());
        }

        [Fact(Skip = "Trying to find a hang")]
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
            Assert.True(flushAsync.GetResult());

            writableBuffer = _pipe.Writer.Alloc(64);
            writableBuffer.Advance(64);
            flushAsync = writableBuffer.FlushAsync();

            Assert.False(flushAsync.IsCompleted);
        }
    }
}
