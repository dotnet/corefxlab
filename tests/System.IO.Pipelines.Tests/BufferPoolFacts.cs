using System.IO.Pipelines.Networking.Sockets.Internal;
using System;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class BufferPoolFacts
    {
        [Fact]
        public void BufferPoolBasicUsage()
        {
            var pool = new MicroBufferPool(8, 4);

            ArraySegment<byte>[] segments = new ArraySegment<byte>[5];

            Assert.Equal(0, pool.InUse);
            Assert.Equal(4, pool.Available);
            Assert.True(pool.TryTake(out segments[0]));
            Assert.True(pool.TryTake(out segments[1]));
            Assert.Equal(2, pool.InUse);
            Assert.Equal(2, pool.Available);
            Assert.True(pool.TryTake(out segments[2]));
            Assert.True(pool.TryTake(out segments[3]));
            Assert.False(pool.TryTake(out segments[4]));
            Assert.Equal(4, pool.InUse);
            Assert.Equal(0, pool.Available);
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(i * 8, segments[i].Offset);
                Assert.Equal(8, segments[i].Count);
            }

            pool.Recycle(segments[3]);
            pool.Recycle(segments[1]);
            Assert.Equal(2, pool.InUse);
            Assert.Equal(2, pool.Available);
            Assert.True(pool.TryTake(out segments[1]));
            Assert.True(pool.TryTake(out segments[3]));
            Assert.False(pool.TryTake(out segments[4]));
            Assert.Equal(4, pool.InUse);
            Assert.Equal(0, pool.Available);

            Assert.Equal(24, segments[1].Offset);
            Assert.Equal(8, segments[3].Offset);
            for(int i = 0; i < 4; i++)
            {
                pool.Recycle(segments[i]);
            }
            Assert.Equal(0, pool.InUse);
            Assert.Equal(4, pool.Available);
        }
    }
}
