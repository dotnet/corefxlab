using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelineReaderWriterFacts : IDisposable
    {
        private IPipe _pipe;
        private PipeFactory _pipeFactory;

        public PipelineReaderWriterFacts()
        {
            _pipeFactory = new PipeFactory();
            _pipe = _pipeFactory.Create();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory?.Dispose();
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task ReaderShouldNotGetUnflushedBytesWhenOverflowingSegments()
        {
            // Fill the block with stuff leaving 5 bytes at the end
            var buffer = _pipe.Writer.Alloc(1);

            var len = buffer.Memory.Length;
            // Fill the buffer with garbage
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', len - 5).ToArray();
            buffer.Write(paddingBytes);
            await buffer.FlushAsync();

            // Write 10 and flush
            buffer = _pipe.Writer.Alloc();
            buffer.WriteLittleEndian(10);

            // Write 9
            buffer.WriteLittleEndian(9);

            // Write 8
            buffer.WriteLittleEndian(8);

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
            Assert.Equal(10, reader.ReadLittleEndian<int>());
            Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
            Assert.Equal(8, reader.Slice(8).ReadLittleEndian<int>());

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task ReaderShouldNotGetUnflushedBytes()
        {
            // Write 10 and flush
            var buffer = _pipe.Writer.Alloc();
            buffer.WriteLittleEndian(10);
            await buffer.FlushAsync();

            // Write 9
            buffer = _pipe.Writer.Alloc();
            buffer.WriteLittleEndian(9);

            // Write 8
            buffer.WriteLittleEndian(8);

            // Make sure we don't see it yet
            var result = await _pipe.Reader.ReadAsync();
            var reader = result.Buffer;

            Assert.Equal(4, reader.Length);
            Assert.Equal(10, reader.ReadLittleEndian<int>());

            // Don't move
            _pipe.Reader.Advance(reader.Start);

            // Now flush
            await buffer.FlushAsync();

            reader = (await _pipe.Reader.ReadAsync()).Buffer;

            Assert.Equal(12, reader.Length);
            Assert.Equal(10, reader.ReadLittleEndian<int>());
            Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
            Assert.Equal(8, reader.Slice(8).ReadLittleEndian<int>());

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task ReaderShouldNotGetUnflushedBytesWithAppend()
        {
            // Write 10 and flush
            var buffer = _pipe.Writer.Alloc();
            buffer.WriteLittleEndian(10);
            await buffer.FlushAsync();

            // Write Hello to another pipeline and get the buffer
            var bytes = Encoding.ASCII.GetBytes("Hello");

            var c2 = _pipeFactory.Create();
            await c2.Writer.WriteAsync(bytes);
            var result = await c2.Reader.ReadAsync();
            var c2Buffer = result.Buffer;

            Assert.Equal(bytes.Length, c2Buffer.Length);

            // Write 9 to the buffer
            buffer = _pipe.Writer.Alloc();
            buffer.WriteLittleEndian(9);

            // Append the data from the other pipeline
            buffer.Append(c2Buffer);

            // Mark it as consumed
            c2.Reader.Advance(c2Buffer.End);

            // Now read and make sure we only see the comitted data
            result = await _pipe.Reader.ReadAsync();
            var reader = result.Buffer;

            Assert.Equal(4, reader.Length);
            Assert.Equal(10, reader.ReadLittleEndian<int>());

            // Consume nothing
            _pipe.Reader.Advance(reader.Start);

            // Flush the second set of writes
            await buffer.FlushAsync();

            reader = (await _pipe.Reader.ReadAsync()).Buffer;

            // int, int, "Hello"
            Assert.Equal(13, reader.Length);
            Assert.Equal(10, reader.ReadLittleEndian<int>());
            Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
            Assert.Equal("Hello", reader.Slice(8).GetUtf8String());

            _pipe.Reader.Advance(reader.Start, reader.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
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
            await _pipe.Writer.WriteAsync(Span<byte>.Empty);
            result = await _pipe.Reader.ReadAsync();
            _pipe.Reader.Advance(result.Buffer.End);

            var awaitable = _pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
        public async Task CancellingPendingReadBeforeReadAsync()
        {
            _pipe.Reader.CancelPendingRead();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            _pipe.Reader.Advance(buffer.End);

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.True(buffer.IsEmpty);

            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = _pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            result = await _pipe.Reader.ReadAsync();
            buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task CancellingBeforeAdvance()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = _pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            _pipe.Reader.CancelPendingRead();

            _pipe.Reader.Advance(buffer.End);

            var awaitable = _pipe.Reader.ReadAsync();

            Assert.True(awaitable.IsCompleted);

            result = await awaitable;

            Assert.True(result.IsCancelled);

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task CancellingPendingAfterReadAsync()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = _pipe.Writer.Alloc();
            output.Write(bytes);

            Func<Task> taskFunc = async () =>
            {
                var result = await _pipe.Reader.ReadAsync();
                var buffer = result.Buffer;
                _pipe.Reader.Advance(buffer.End);

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.True(buffer.IsEmpty);

                await output.FlushAsync();

                result = await _pipe.Reader.ReadAsync();
                buffer = result.Buffer;

                Assert.Equal(11, buffer.Length);
                Assert.True(buffer.IsSingleSpan);
                Assert.False(result.IsCancelled);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
                _pipe.Reader.Advance(result.Buffer.End, result.Buffer.End);

                _pipe.Reader.Complete();
            };

            var task = taskFunc();

            _pipe.Reader.CancelPendingRead();

            await task;

            _pipe.Writer.Complete();
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task WriteAndCancellingPendingReadBeforeReadAsync()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var output = _pipe.Writer.Alloc();
            output.Write(bytes);
            await output.FlushAsync();

            _pipe.Reader.CancelPendingRead();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.False(buffer.IsEmpty);
            Assert.Equal(11, buffer.Length);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
            _pipe.Reader.Advance(buffer.End, buffer.End);
        }

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
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

        [Fact(Skip = "Trying to find a hang")]
        public async Task IndexOfNotFoundReturnsEnd()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");

            await _pipe.Writer.WriteAsync(bytes);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            ReadableBuffer slice;
            ReadCursor cursor;

            Assert.False(buffer.TrySliceTo(10, out slice, out cursor));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task FastPathIndexOfAcrossBlocks()
        {
            var vecUpperR = new Vector<byte>((byte)'R');

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
            ReadableBuffer slice;
            ReadCursor cursor;
            Assert.False(buffer.TrySliceTo((byte)'R', out slice, out cursor));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task SlowPathIndexOfAcrossBlocks()
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
            ReadableBuffer slice;
            ReadCursor cursor;
            Assert.False(buffer.IsSingleSpan);
            Assert.True(buffer.TrySliceTo((byte)' ', out slice, out cursor));

            slice = buffer.Slice(cursor).Slice(1);
            var array = slice.ToArray();

            Assert.Equal("World", Encoding.ASCII.GetString(array));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public void AllocMoreThanPoolBlockSizeThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _pipe.Writer.Alloc(8192));
        }

        [Fact(Skip = "Trying to find a hang")]
        public void ThrowsOnReadAfterCompleteReader()
        {
            _pipe.Reader.Complete();

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.ReadAsync());
        }

        [Fact(Skip = "Trying to find a hang")]
        public void ThrowsOnAllocAfterCompleteWriter()
        {
            _pipe.Writer.Complete();

            Assert.Throws<InvalidOperationException>(() => _pipe.Writer.Alloc());
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task MultipleCompleteReaderWriterCauseDisposeOnlyOnce()
        {
            var pool = new DisposeTrackingOwnedMemory(new byte[1]);

            using (var factory = new PipeFactory(pool))
            {
                var readerWriter = factory.Create();
                await readerWriter.Writer.WriteAsync(new byte[] { 1 });

                readerWriter.Writer.Complete();
                readerWriter.Reader.Complete();
                Assert.Equal(1, pool.Disposed);

                readerWriter.Writer.Complete();
                readerWriter.Reader.Complete();
                Assert.Equal(1, pool.Disposed);
            }
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task CompleteReaderThrowsIfReadInProgress()
        {
            await _pipe.Writer.WriteAsync(new byte[1]);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Throws<InvalidOperationException>(() => _pipe.Reader.Complete());

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact(Skip = "Trying to find a hang")]
        public void CompleteWriterThrowsIfWriteInProgress()
        {
            var buffer = _pipe.Writer.Alloc();

            Assert.Throws<InvalidOperationException>(() => _pipe.Writer.Complete());

            buffer.Commit();
        }

        [Fact(Skip = "Trying to find a hang")]
        public async Task ReadAsync_ThrowsIfWriterCompletedWithException()
        {
            _pipe.Writer.Complete(new InvalidOperationException("Writer exception"));

            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());
            Assert.Equal("Writer exception", invalidOperationException.Message);
            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());
            Assert.Equal("Writer exception", invalidOperationException.Message);
        }

        [Fact(Skip = "Trying to find a hang")]
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

        private class DisposeTrackingOwnedMemory : OwnedMemory<byte>, IBufferPool
        {
            public DisposeTrackingOwnedMemory(byte[] array) : base(array)
            {
            }

            protected override void Dispose(bool disposing)
            {
                Disposed++;
                base.Dispose(disposing);
            }

            public int Disposed { get; set; }

            public OwnedMemory<byte> Lease(int size)
            {
                return this;
            }
        }
    }
}
