// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Tests;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelinesOwnedBuffersReferenceTests
    {
        [Fact]
        public void MemoryPoolBlockReferenceTests()
        {
            var pool = new MemoryPool();

            BufferReferenceTests.TestOwnedBuffer(() => pool.Rent(1024), block => pool.Return((MemoryPoolBlock) block));
            pool.Dispose();
        }

        [Fact]
        public void UnownedBufferReferenceTests()
        {
            BufferReferenceTests.TestOwnedBuffer(() => new UnownedBuffer(new ArraySegment<byte>(new byte[1024])));
        }
    }
}
