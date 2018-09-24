// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Buffers.Tests
{
    public class BufferExtensionTests
    {
        [Fact]
        public void ToSpanOnReadableBufferSingleSpan()
        {
            byte[] data = { 0, 1, 2, 3, 4, 5, 6, 7 };

            var readable = BufferUtilities.CreateBuffer(data);
            var span = readable.ToSpan();

            Assert.True(readable.IsSingleSegment);
            Assert.Equal(span.Length, data.Length);
        }

        [Fact]
        public void ToSpanOnReadableBufferMultiSpan()
        {
            byte[] data1 = { 0, 1, 2 };
            byte[] data2 = { 3, 4, 5 };
            byte[] data3 = { 6, 7, 8 };

            var readable = BufferUtilities.CreateBuffer(data1, data2, data3);
            var span = readable.ToSpan();

            Assert.False(readable.IsSingleSegment);
            Assert.Equal(span.Length, data1.Length + data2.Length + data3.Length);
        }
    }
}
