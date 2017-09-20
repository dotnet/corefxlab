// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferWriterTests
    {   
        [Theory]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(0xFF000000)]
        [InlineData(0x00FF0000)]
        [InlineData(0x0000FF00)]
        [InlineData(0x000000FF)]
        public void WriteUInt32(uint value)
        {
            var span = new Span<byte>(new byte[4]);
            span.WriteBigEndianUInt32(value);
            uint read = span.ReadBigEndianUInt32();
            Assert.Equal(value, read);

            span.Clear();
            Assert.True(span.TryWriteBigEndianUInt32(value));
            read = span.ReadBigEndianUInt32();
            Assert.Equal(value,read);

            span.Clear();
            span.WriteLittleEndianUInt32(value);
            read = span.ReadLittleEndianUInt32();
            Assert.Equal(value, read);

            span.Clear();
            Assert.True(span.TryWriteLittleEndianUInt32(value));
            read = span.ReadLittleEndianUInt32();
            Assert.Equal(value, read);
        }
    }
}
