// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipePoolTests
    {
        [Fact]
        public async Task MultipleCompleteReaderWriterCauseDisposeOnlyOnce()
        {
            var pool = new DisposeTrackingBufferPool();

            var readerWriter = new Pipe(new PipeOptions(pool));
            await readerWriter.Writer.WriteAsync(new byte[] {1});

            readerWriter.Writer.Complete();
            readerWriter.Reader.Complete();
            Assert.Equal(1, pool.ReturnedBlocks);

            readerWriter.Writer.Complete();
            readerWriter.Reader.Complete();
            Assert.Equal(1, pool.ReturnedBlocks);
        }

        [Fact]
        public async Task AdvanceToEndReturnsAllBlocks()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            var pipe = new Pipe(new PipeOptions(pool));
            while (pool.CurrentlyRentedBlocks != 3)
            {
                var writableBuffer = pipe.Writer.Alloc(writeSize);
                writableBuffer.Advance(writeSize);
                await writableBuffer.FlushAsync();
            }

            var readResult = await pipe.Reader.ReadAsync();
            pipe.Reader.Advance(readResult.Buffer.End);

            Assert.Equal(0, pool.CurrentlyRentedBlocks);
        }

        [Fact]
        public async Task WriteDuringReadIsNotReturned()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            var pipe = new Pipe(new PipeOptions(pool));
            await pipe.Writer.WriteAsync(new byte[writeSize]);

            var buffer = pipe.Writer.Alloc(writeSize);
            var readResult = await pipe.Reader.ReadAsync();
            pipe.Reader.Advance(readResult.Buffer.End);
            buffer.Write(new byte[writeSize]);
            buffer.Commit();

            Assert.Equal(1, pool.CurrentlyRentedBlocks);
        }

        [Fact]
        public async Task CanWriteAfterReturningMultipleBlocks()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            var pipe = new Pipe(new PipeOptions(pool));

            // Write two blocks
            var buffer = pipe.Writer.Alloc(writeSize);
            buffer.Advance(buffer.Buffer.Length);
            buffer.Ensure(buffer.Buffer.Length);
            buffer.Advance(writeSize);
            await buffer.FlushAsync();

            Assert.Equal(2, pool.CurrentlyRentedBlocks);

            // Read everything
            var readResult = await pipe.Reader.ReadAsync();
            pipe.Reader.Advance(readResult.Buffer.End);

            // Try writing more
            await pipe.Writer.WriteAsync(new byte[writeSize]);
        }

        private class DisposeTrackingBufferPool : MemoryPool
        {
            public override OwnedMemory<byte> Rent(int size)
            {
                return new DisposeTrackingOwnedMemory(new byte[2048], this);
            }

            public int ReturnedBlocks { get; set; }
            public int CurrentlyRentedBlocks { get; set; }

            protected override void Dispose(bool disposing)
            {

            }

            private class DisposeTrackingOwnedMemory : OwnedMemory<byte>
            {
                private readonly DisposeTrackingBufferPool _bufferPool;

                public DisposeTrackingOwnedMemory(byte[] array, DisposeTrackingBufferPool bufferPool)
                {
                    _array = array;
                    _bufferPool = bufferPool;
                    _bufferPool.CurrentlyRentedBlocks++;
                }

                public override int Length => _array.Length;
                public override Span<byte> Span
                {
                    get
                    {
                        if (IsDisposed)
                            PipelinesThrowHelper.ThrowObjectDisposedException(nameof(DisposeTrackingBufferPool));
                        return _array;
                    }
                }

                public override MemoryHandle Pin()
                {
                    throw new NotImplementedException();
                }

                protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
                {
                    if (IsDisposed)
                        PipelinesThrowHelper.ThrowObjectDisposedException(nameof(DisposeTrackingBufferPool));
                    arraySegment = new ArraySegment<byte>(_array);
                    return true;
                }

                public override bool IsDisposed { get; }
                protected override void Dispose(bool disposing)
                {
                    throw new NotImplementedException();
                }

                public override bool Release()
                {
                    _bufferPool.ReturnedBlocks++;
                    _bufferPool.CurrentlyRentedBlocks--;
                    return IsRetained;
                }

                protected override bool IsRetained => true;
                public override void Retain()
                {
                }

                byte[] _array;
            }
        }
    }
}
