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

            using (var factory = new PipeFactory(pool))
            {
                var readerWriter = factory.Create();
                await readerWriter.Writer.WriteAsync(new byte[] {1});

                readerWriter.Writer.Complete();
                readerWriter.Reader.Complete();
                Assert.Equal(1, pool.ReturnedBlocks);

                readerWriter.Writer.Complete();
                readerWriter.Reader.Complete();
                Assert.Equal(1, pool.ReturnedBlocks);
            }
        }

        [Fact]
        public async Task AdvanceToEndReturnsAllBlocks()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            using (var factory = new PipeFactory(pool))
            {
                var pipe = factory.Create();
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
        }

        [Fact]
        public async Task WriteDuringReadIsNotReturned()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            using (var factory = new PipeFactory(pool))
            {
                var pipe = factory.Create();
                await pipe.Writer.WriteAsync(new byte[writeSize]);

                var buffer = pipe.Writer.Alloc(writeSize);
                var readResult = await pipe.Reader.ReadAsync();
                pipe.Reader.Advance(readResult.Buffer.End);
                buffer.Write(new byte[writeSize]);
                buffer.Commit();

                Assert.Equal(1, pool.CurrentlyRentedBlocks);
            }
        }

        [Fact]
        public async Task CanWriteAfterReturningMultipleBlocks()
        {
            var pool = new DisposeTrackingBufferPool();

            var writeSize = 512;

            using (var factory = new PipeFactory(pool))
            {
                var pipe = factory.Create();

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
        }

        private class DisposeTrackingBufferPool : BufferPool
        {
            public override OwnedBuffer<byte> Rent(int size)
            {
                return new DisposeTrackingOwnedMemory(new byte[2048], this);
            }

            public int ReturnedBlocks { get; set; }
            public int CurrentlyRentedBlocks { get; set; }

            protected override void Dispose(bool disposing)
            {

            }

            private class DisposeTrackingOwnedMemory : OwnedBuffer<byte>
            {
                private readonly DisposeTrackingBufferPool _bufferPool;

                public DisposeTrackingOwnedMemory(byte[] array, DisposeTrackingBufferPool bufferPool)
                {
                    _array = array;
                    _bufferPool = bufferPool;
                    _bufferPool.CurrentlyRentedBlocks++;
                }

                public override int Length => _array.Length;
                public override Span<byte> AsSpan(int index, int length)
                {
                    if (IsDisposed)
                        PipelinesThrowHelper.ThrowObjectDisposedException(nameof(DisposeTrackingBufferPool));
                    return _array;
                }

                public override BufferHandle Pin()
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

                public override void Release()
                {
                    _bufferPool.ReturnedBlocks++;
                    _bufferPool.CurrentlyRentedBlocks--;
                }

                public override bool IsRetained => true;
                public override void Retain()
                {
                }

                byte[] _array;
            }
        }
    }
}
