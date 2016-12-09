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
            pool.Return(buffer);
            buffer = pool.Rent(10); 
            pool.Return(buffer);
        }
    }
}
