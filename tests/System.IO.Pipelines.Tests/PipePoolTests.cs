using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PoolUsageTests
    {

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


        private class DisposeTrackingBufferPool : BufferPool
        {
            public override OwnedBuffer<byte> Rent(int size)
            {
                return new DisposeTrackingOwnedMemory(new byte[size * 4], this);
            }

            public int RentedBlocks { get; set; }

            public int Disposed { get; set; }

            protected override void Dispose(bool disposing)
            {

            }

            private class DisposeTrackingOwnedMemory : OwnedBuffer<byte>
            {
                private readonly DisposeTrackingBufferPool _bufferPool;

                public DisposeTrackingOwnedMemory(byte[] array, DisposeTrackingBufferPool bufferPool) : base(array)
                {
                    _bufferPool = bufferPool;
                    _bufferPool.RentedBlocks++;
                }

                protected override void Dispose(bool disposing)
                {
                    _bufferPool.Disposed++;
                    _bufferPool.RentedBlocks--;
                    base.Dispose(disposing);
                }
            }
        }

    }
}