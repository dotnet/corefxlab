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
            var mem = buffer.Memory;
            buffer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => {
                mem = buffer.Memory;
            });

            buffer = pool.Rent(10);
            mem = buffer.Memory;
            var span0 = buffer.Span;
            buffer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => {
                var span1 = buffer.Span;
            });
        }
    }
}
