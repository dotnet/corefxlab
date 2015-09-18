// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Buffers;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class BufferPoolTests
    {
        [Fact]
        public void NativePoolBasics()
        {
            using (var pool = new NativeBufferPool(256, 10))
            {
                List<ByteSpan> buffers = new List<ByteSpan>();
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = pool.Rent();
                    buffers.Add(buffer);
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        buffer[bi] = i;
                    }
                }
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = buffers[i];
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        Assert.Equal(i, buffer[bi]);
                    }
                    pool.Return(ref buffer);
                }
            }
        }
    }
}
