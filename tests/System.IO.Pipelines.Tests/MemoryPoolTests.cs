// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class MemoryPoolTests
    {
        [Fact]
        public void ReleasedBlockWorks()
        {
            using (var pool = new MemoryPool())
            {
                var block1 = pool.Rent(1);
                block1.AddReference();
                block1.Release();

            OwnedBuffer<byte> block2 = null;

            // Lease-return until we get same block
                while (block1 != block2)
            {
                block2 = pool.Rent(1);
                    block2.AddReference();
                    block2.Release();
                }

            Assert.True(block2.AsSpan().Length > 0);
        }
    }
}
}
