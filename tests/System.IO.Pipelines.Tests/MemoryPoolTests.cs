// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class MemoryPoolTests
    {
        [Fact]
        public void ReleasedBlockWorks()
        {
            var pool = new MemoryPool();
            var block = pool.Rent(1);
            block.Dispose();

            OwnedBuffer<byte> block2 = null;

            // Lease-return until we get same block
            do
            {
                block2?.Dispose();
                block2 = pool.Rent(1);
            } while (block != block2);

            Assert.True(block2.Span.Length > 0);
        }
    }
}
