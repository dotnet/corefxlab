using System.Buffers.Pools;
using System.Collections.Generic;
using Xunit;

namespace System.Buffers.Tests
{
    public class NativeBufferPoolTests
    {
        [Fact]
        public void BasicsWork() {
            var pool = NativeBufferPool.Shared;
            var buffer = pool.Rent(10);
            pool.Return(buffer);
            buffer = pool.Rent(10); 
            pool.Return(buffer);
        }
    }
}
