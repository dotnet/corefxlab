using System.Buffers;
using System.Collections.Generic;
using Xunit;

namespace System.Buffers.Tests
{
    public class NativeBufferPoolTests
    {
        [Fact]
        public void BasicsWork() {
            var pool = System.Buffers.NativeBufferPool.Shared;
            var buffer = pool.Rent(10);
            buffer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => {
                var mem = buffer.Memory;
            });

            buffer = pool.Rent(10);
            buffer.Dispose();
        }
    }
}
