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
            span.WriteBigEndian(value);
            uint read = span.ReadBigEndian<uint>();
            Assert.Equal(value, read);

            span.Clear();
            Assert.True(span.TryWriteBigEndian(value));
            read = span.ReadBigEndian<uint>();
            Assert.Equal(value,read);

            span.Clear();
            span.WriteLittleEndian(value);
            read = span.ReadLittleEndian<uint>();
            Assert.Equal(value, read);

            span.Clear();
            Assert.True(span.TryWriteLittleEndian(value));
            read = span.ReadLittleEndian<uint>();
            Assert.Equal(value, read);
        }
    }
}
