// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        [Fact]
        public void WhenTryReadReturnsFalseDontNeedToCallAdvance()
        {
            var gotData = _pipe.Reader.TryRead(out var result);
            Assert.False(gotData);
            _pipe.Reader.Advance(default(ReadCursor));
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

            Assert.Equal("Hello World", result.Buffer.GetAsciiString());

            _pipe.Reader.Advance(result.Buffer.Move(result.Buffer.Start, 6));

            result = await _pipe.Reader.ReadAsync();

            Assert.Equal("World", result.Buffer.GetAsciiString());

            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
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

        [Fact]
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
            var memory = new List<Buffer<byte>>();
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

        [Fact]
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

        [Fact]
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
        public async Task MultipleCompleteReaderWriterCauseDisposeOnlyOnce()
        {
            var pool = new DisposeTrackingBufferPool();

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
            _pipe.Writer.Complete(new InvalidOperationException("Writer exception"));

            var invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());
            Assert.Equal("Writer exception", invalidOperationException.Message);
            invalidOperationException = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _pipe.Reader.ReadAsync());
            Assert.Equal("Writer exception", invalidOperationException.Message);
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

        private class DisposeTrackingBufferPool : BufferPool
        {
            private DisposeTrackingOwnedMemory _memory = new DisposeTrackingOwnedMemory(new byte[1]);

            public override OwnedBuffer<byte> Rent(int size)
            {
                return _memory;
            }

            public int Disposed => _memory.Disposed;

            protected override void Dispose(bool disposing)
            {

            }

            private class DisposeTrackingOwnedMemory : OwnedBuffer<byte>
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

            }
        }
    }
}
