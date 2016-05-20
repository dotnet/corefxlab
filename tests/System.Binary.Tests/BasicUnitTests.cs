// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Binary.BigEndian;
 
namespace System.Binary.Tests
{
    public class BigEndianTests
    {
        [Theory]
        [InlineData(new byte[] { 0, 0,  }, 0)]
        [InlineData(new byte[] { 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 255 }, 255)]
        [InlineData(new byte[] { 1, 0 }, 256)]
        [InlineData(new byte[] { 255, 255 }, ushort.MaxValue)]
        public void ReadUInt16(byte[] bytes, ushort expected)
        {
            var span = new Span<byte>(bytes);
            ushort value = span.ReadUInt16();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadUInt16();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(new byte[] { 0, 0 }, 0)]
        [InlineData(new byte[] { 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 255 }, 255)]
        [InlineData(new byte[] { 1, 0 }, 256)]
        [InlineData(new byte[] { 255, 255 }, -1)]
        public void ReadInt16(byte[] bytes, short expected)
        {
            var span = new Span<byte>(bytes);
            short value = span.ReadInt16();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadInt16();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0 }, 0)]
        [InlineData(new byte[] { 0, 0, 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 0, 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 0, 0, 255 }, 255)]
        [InlineData(new byte[] { 0, 0, 1, 0 }, 256)]
        [InlineData(new byte[] { 1, 0, 0, 0 }, 16777216)]
        [InlineData(new byte[] { 255, 255, 255, 255 }, uint.MaxValue)]
        public void ReadUInt32(byte[] bytes, uint expected)
        {
            var span = new Span<byte>(bytes);
            uint value = span.ReadUInt32();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadUInt32();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0 }, 0)]
        [InlineData(new byte[] { 0, 0, 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 0, 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 0, 0, 255 }, 255)]
        [InlineData(new byte[] { 0, 0, 1, 0 }, 256)]
        [InlineData(new byte[] { 1, 0, 0, 0 }, 16777216)]
        [InlineData(new byte[] { 255, 255, 255, 255 }, -1)]
        public void ReadInt32(byte[] bytes, int expected)
        {
            var span = new Span<byte>(bytes);
            int value = span.ReadInt32();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadInt32();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0 , 0, 0, 0, 0}, 0)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 255 }, 255)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 1, 0 }, 256)]
        [InlineData(new byte[] { 1, 0, 0, 0 , 0, 0, 0, 0 }, 72057594037927936)]
        [InlineData(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, ulong.MaxValue)]
        public void ReadUInt64(byte[] bytes, ulong expected)
        {
            var span = new Span<byte>(bytes);
            ulong value = span.ReadUInt64();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadUInt64();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, 1)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 2 }, 2)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 255 }, 255)]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 1, 0 }, 256)]
        [InlineData(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }, 72057594037927936)]
        [InlineData(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, -1)]
        public void ReadInt64(byte[] bytes, long expected)
        {
            var span = new Span<byte>(bytes);
            long value = span.ReadInt64();
            Assert.Equal(expected, value);

            var roSpan = new ReadOnlySpan<byte>(bytes);
            value = span.ReadInt64();
            Assert.Equal(expected, value);
        }
        
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
            span.WriteUInt32(value);
            uint read = span.ReadUInt32();
            Assert.Equal(value,read);         
        }
    }
}