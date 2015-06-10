// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO.Buffers;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class BufferPoolTests
    {
        [Fact]
        public void NativePoolBasics()
        {
            using (var pool = new NativeBufferPool(256, 10)) {
                List<ByteSpan> buffers = new List<ByteSpan>();
                for (int i = 0; i < 10; i++) {
                    var buffer = pool.Rent();
                    buffers.Add(buffer);
                }
                foreach (var buffer in buffers) {
                    var toReturn = buffer;
                    pool.Return(ref toReturn);
                }
            }
        }
    }
}
