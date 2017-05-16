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
                Assert.Equal(1, pool.Disposed);

                readerWriter.Writer.Complete();
                readerWriter.Reader.Complete();
                Assert.Equal(1, pool.Disposed);
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
                while (pool.RentedBlocks != 3)
                {
                    var writableBuffer = pipe.Writer.Alloc(writeSize);
                    writableBuffer.Advance(writeSize);
                    await writableBuffer.FlushAsync();
                }

                var readResult = await pipe.Reader.ReadAsync();
                pipe.Reader.Advance(readResult.Buffer.End);

                Assert.Equal(0, pool.RentedBlocks);
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

                Assert.Equal(1, pool.RentedBlocks);
            }
        }

        private class DisposeTrackingBufferPool : BufferPool
        {
            public override OwnedBuffer<byte> Rent(int size)
            {
                return new DisposeTrackingOwnedMemory(new byte[2048], this);
            }

            public int Disposed { get; set; }
            public int RentedBlocks { get; set; }

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
                    _bufferPool.RentedBlocks++;
                }

                protected override void Dispose(bool disposing)
                {
                    _bufferPool.Disposed++;
                    _bufferPool.RentedBlocks--;
                    base.Dispose(disposing);
                }

                protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
                {
                    if (IsDisposed)
                        PipelinesThrowHelper.ThrowObjectDisposedException(nameof(DisposeTrackingBufferPool));
                    buffer = new ArraySegment<byte>(_array);
                    return true;
                }

                protected override unsafe bool TryGetPointerInternal(out void* pointer)
                {
                    pointer = null;
                    return false;
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

                byte[] _array;
            }
        }
    }
}