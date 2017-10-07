// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelineReaderWriterFacts : IDisposable
    {
        private IPipe _pipe;
        private MemoryPool _pool;

        public PipelineReaderWriterFacts()
        {
            _pool = new MemoryPool();
            _pipe = new Pipe(new PipeOptions(_pool));
        }
        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytesWhenOverflowingSegments()
        {
            // Fill the block with stuff leaving 5 bytes at the end
            var buffer = _pipe.Writer.Alloc(1);

            var len = buffer.Buffer.Length;
            // Fill the buffer with garbage
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', len - 5).ToArray();
            buffer.Write(paddingBytes);
            await buffer.FlushAsync();

            // Write 10 and flush
            buffer = _pipe.Writer.Alloc();
            buffer.Write(new byte[] { 0, 0, 0, 10});

            // Write 9
            buffer.Write(new byte[] { 0, 0, 0, 9 });

            // Write 8
            buffer.Write(new byte[] { 0, 0, 0, 8 });

            // Make sure we don't see it yet
            var result = await _pipe.Reader.ReadAsync();
            var reader = result.Buffer;

            Assert.Equal(len - 5, reader.Length);

            // Don't move
            _pipe.Reader.Advance(reader.End);

            // Now flush
            await buffer.FlushAsync();

            reader = (await _pipe.Reader.ReadAsync()).Buffer;

            Assert.Equal(12, reader.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 10 }, reader.Slice(0, 4).ToArray());
            Assert.Equal(new byte[] { 0, 0, 0, 9 }, reader.Slice(4, 4).ToArray());
            Assert.Equal(new byte[] { 0, 0, 0, 8 }, reader.Slice(8, 4).ToArray());

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact]
        public void WhenTryReadReturnsFalseDontNeedToCallAdvance()
        {
            var gotData = _pipe.Reader.TryRead(out var result);
            Assert.False(gotData);
            _pipe.Reader.Advance(default);
        }

        [Fact]
        public void TryReadAfterReaderCompleteThrows()
        {
            _pipe.Reader.Complete();

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.TryRead(out var result));
        }

        [Fact]
        public void TryReadAfterCloseWriterWithExceptionThrows()
        {
            _pipe.Writer.Complete(new Exception("wow"));

            var ex = Assert.Throws<Exception>(() => _pipe.Reader.TryRead(out var result));
            Assert.Equal("wow", ex.Message);
        }

        [Fact]
        public void TryReadAfterCancelPendingReadReturnsTrue()
        {
            _pipe.Reader.CancelPendingRead();

            var gotData = _pipe.Reader.TryRead(out var result);

            Assert.True(result.IsCancelled);

            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
        public void TryReadAfterWriterCompleteReturnsTrue()
        {
            _pipe.Writer.Complete();

            var gotData = _pipe.Reader.TryRead(out var result);

            Assert.True(result.IsCompleted);

            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
        public async Task SyncReadThenAsyncRead()
        {
            var buffer = _pipe.Writer.Alloc();
            buffer.Write(Encoding.ASCII.GetBytes("Hello World"));
            await buffer.FlushAsync();

            var gotData = _pipe.Reader.TryRead(out var result);
            Assert.True(gotData);

            Assert.Equal("Hello World", Encoding.ASCII.GetString(result.Buffer.ToArray()));

            _pipe.Reader.Advance(result.Buffer.Move(result.Buffer.Start, 6));

            result = await _pipe.Reader.ReadAsync();

            Assert.Equal("World", Encoding.ASCII.GetString(result.Buffer.ToArray()));

            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytes()
        {
            // Write 10 and flush
            var buffer = _pipe.Writer.Alloc();
            buffer.Write(new byte[] { 0, 0, 0, 10 });
            await buffer.FlushAsync();

            // Write 9
            buffer = _pipe.Writer.Alloc();
            buffer.Write(new byte[] { 0, 0, 0, 9 });

            // Write 8
            buffer.Write(new byte[] { 0, 0, 0, 8 });

            // Make sure we don't see it yet
            var result = await _pipe.Reader.ReadAsync();
            var reader = result.Buffer;

            Assert.Equal(4, reader.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 10 }, reader.ToArray());

            // Don't move
            _pipe.Reader.Advance(reader.Start);

            // Now flush
            await buffer.FlushAsync();

            reader = (await _pipe.Reader.ReadAsync()).Buffer;

            Assert.Equal(12, reader.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 10 }, reader.Slice(0, 4).ToArray());
            Assert.Equal(new byte[] { 0, 0, 0, 9 }, reader.Slice(4, 4).ToArray());
            Assert.Equal(new byte[] { 0, 0, 0, 8 }, reader.Slice(8, 4).ToArray());

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytesWithAppend()
        {
            // Write 10 and flush
            var buffer = _pipe.Writer.Alloc();
            buffer.Write(new byte[] { 0, 0, 0, 10 });
            await buffer.FlushAsync();

            // Write Hello to another pipeline and get the buffer
            var bytes = Encoding.ASCII.GetBytes("Hello");

            var c2 = new Pipe(new PipeOptions(_pool));
            await c2.Writer.WriteAsync(bytes);
            var result = await c2.Reader.ReadAsync();
            var c2Buffer = result.Buffer;

            Assert.Equal(bytes.Length, c2Buffer.Length);

            // Write 9 to the buffer
            buffer = _pipe.Writer.Alloc();
            buffer.Write(new byte[] { 0, 0, 0, 9 });

            // Append the data from the other pipeline
            buffer.Append(c2Buffer);

            // Mark it as consumed
            c2.Reader.Advance(c2Buffer.End);

            // Now read and make sure we only see the comitted data
            result = await _pipe.Reader.ReadAsync();
            var reader = result.Buffer;

            Assert.Equal(4, reader.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 10 }, reader.Slice(0, 4).ToArray());

            // Consume nothing
            _pipe.Reader.Advance(reader.Start);

            // Flush the second set of writes
            await buffer.FlushAsync();

            reader = (await _pipe.Reader.ReadAsync()).Buffer;

            // int, int, "Hello"
            Assert.Equal(13, reader.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 10 }, reader.Slice(0, 4).ToArray());
            Assert.Equal(new byte[] { 0, 0, 0, 9 }, reader.Slice(4, 4).ToArray());
            Assert.Equal("Hello", Encoding.ASCII.GetString(reader.Slice(8).ToArray()));

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact]
        public async Task WritingDataMakesDataReadableViaPipeline()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");

            await _pipe.Writer.WriteAsync(bytes);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task AdvanceEmptyBufferAfterWritingResetsAwaitable()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");

            await _pipe.Writer.WriteAsync(bytes);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            _pipe.Reader.Advance(buffer.End);

            // Now write 0 and advance 0
            await _pipe.Writer.WriteAsync(new byte [] {});
            result = await _pipe.Reader.ReadAsync();
            _pipe.Reader.Advance(result.Buffer.End);

            var awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }

        [Fact]
        public async Task AdvanceShouldResetStateIfReadCancelled()
        {
            _pipe.Reader.CancelPendingRead();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            _pipe.Reader.Advance(buffer.End);

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.True(buffer.IsEmpty);

            var awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }

        [Fact]
        public async Task ReadingCanBeCancelled()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() =>
            {
                _pipe.Writer.Complete(new OperationCanceledException(cts.Token));
            });

            var ignore = Task.Run(async () =>
            {
                await Task.Delay(1000);
                cts.Cancel();
            });

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                var result = await _pipe.Reader.ReadAsync();
                var buffer = result.Buffer;
            });
        }

        [Fact]
        public async Task HelloWorldAcrossTwoBlocks()
        {
            const int blockSize = 4032;
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var writeBuffer = _pipe.Writer.Alloc();
            writeBuffer.Write(paddingBytes);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Assert.False(buffer.IsSingleSpan);
            var helloBuffer = buffer.Slice(blockSize - 5);
            Assert.False(helloBuffer.IsSingleSpan);
            var memory = new List<Memory<byte>>();
            foreach (var m in helloBuffer)
            {
                memory.Add(m);
            }
            var spans = memory;
            _pipe.Reader.Advance(buffer.Start, buffer.Start);

            Assert.Equal(2, memory.Count);
            var helloBytes = new byte[spans[0].Length];
            spans[0].Span.CopyTo(helloBytes);
            var worldBytes = new byte[spans[1].Length];
            spans[1].Span.CopyTo(worldBytes);
            Assert.Equal("Hello", Encoding.ASCII.GetString(helloBytes));
            Assert.Equal(" World", Encoding.ASCII.GetString(worldBytes));
        }

        [Fact]
        public void AllocMoreThanPoolBlockSizeThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _pipe.Writer.Alloc(8192));
        }

        [Fact]
        public void ThrowsOnReadAfterCompleteReader()
        {
            _pipe.Reader.Complete();

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.ReadAsync());
        }

        [Fact]
        public void ThrowsOnAllocAfterCompleteWriter()
        {
            _pipe.Writer.Complete();

            Assert.Throws<InvalidOperationException>(() => _pipe.Writer.Alloc());
        }

        [Fact]
        public async Task CompleteReaderThrowsIfReadInProgress()
        {
            await _pipe.Writer.WriteAsync(new byte[1]);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.Complete());

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public void CompleteWriterThrowsIfWriteInProgress()
        {
            var buffer = _pipe.Writer.Alloc();

            Assert.Throws<InvalidOperationException>(() => _pipe.Writer.Complete());

            buffer.Commit();
        }

        [Fact]
        public async Task ReadAsync_ThrowsIfWriterCompletedWithException()
        {
            void ThrowTestException()
            {
                try
                {
                    throw new InvalidOperationException("Writer exception");
                }
                catch (Exception e)
                {
                    _pipe.Writer.Complete(e);
                }
            }

            ThrowTestException();

            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());

            Assert.Equal("Writer exception", invalidOperationException.Message);
            Assert.Contains("ThrowTestException", invalidOperationException.StackTrace);

            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());
            Assert.Equal("Writer exception", invalidOperationException.Message);
            Assert.Contains("ThrowTestException", invalidOperationException.StackTrace);
        }

        [Fact]
        public async Task FlushAsync_ThrowsIfWriterReaderWithException()
        {
            void ThrowTestException()
            {
                try
                {
                    throw new InvalidOperationException("Reader exception");
                }
                catch (Exception e)
                {
                    _pipe.Reader.Complete(e);
                }
            }

            ThrowTestException();

            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Writer.Alloc().FlushAsync());

            Assert.Equal("Reader exception", invalidOperationException.Message);
            Assert.Contains("ThrowTestException", invalidOperationException.StackTrace);

            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Writer.Alloc().FlushAsync());
            Assert.Equal("Reader exception", invalidOperationException.Message);
            Assert.Contains("ThrowTestException", invalidOperationException.StackTrace);
        }

        [Fact]
        public void FlushAsync_ReturnsCompletedTaskWhenMaxSizeIfZero()
        {
            var writableBuffer = _pipe.Writer.Alloc(1);
            writableBuffer.Advance(1);
            var flushTask = writableBuffer.FlushAsync();
            Assert.True(flushTask.IsCompleted);

            writableBuffer = _pipe.Writer.Alloc(1);
            writableBuffer.Advance(1);
            flushTask = writableBuffer.FlushAsync();
            Assert.True(flushTask.IsCompleted);
        }

        [Fact]
        public async Task AdvanceToInvalidCursorThrows()
        {
            await _pipe.Writer.WriteAsync(new byte[100]);

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            _pipe.Reader.Advance(buffer.End);

            _pipe.Reader.CancelPendingRead();
            result = await _pipe.Reader.ReadAsync();

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.Advance(buffer.End));
            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
        public async Task EmptyBufferStartCrossingSegmentBoundaryIsTreatedLikeAndEnd()
        {
            // Append one full segment to a pipe
            var buffer = _pipe.Writer.Alloc(1);
            buffer.Advance(buffer.Buffer.Length);
            buffer.Commit();
            await buffer.FlushAsync();

            // Consume entire segment
            var result = await _pipe.Reader.ReadAsync();
            _pipe.Reader.Advance(result.Buffer.End);

            // Append empty segment
            buffer = _pipe.Writer.Alloc(1);
            buffer.Commit();
            await buffer.FlushAsync();

            result = await _pipe.Reader.ReadAsync();

            Assert.True(result.Buffer.IsEmpty);
            Assert.Equal(result.Buffer.Start, result.Buffer.End);

            buffer = _pipe.Writer.Alloc();
            _pipe.Reader.Advance(result.Buffer.Start);
            var awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
            buffer.Commit();
        }

        [Fact]
        public async Task AdvanceResetsCommitHeadIndex()
        {
            var buffer = _pipe.Writer.Alloc(1);
            buffer.Advance(100);
            await buffer.FlushAsync();

            // Advance to the end
            var readResult = await _pipe.Reader.ReadAsync();
            _pipe.Reader.Advance(readResult.Buffer.End);

            // Try reading, it should block
            var awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);

            // Unblock without writing anything
            buffer = _pipe.Writer.Alloc();
            await buffer.FlushAsync();

            Assert.True(awaitable.IsCompleted);

            // Advance to the end should reset awaitable
            readResult = await awaitable;
            _pipe.Reader.Advance(readResult.Buffer.End);

            // Try reading, it should block
            awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }
    }
}
