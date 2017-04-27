// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Binary.Tests
{
    public class BufferReaderTests
    {

        [Fact]
        public void BufferReaderRead()
        {
            Assert.True(BitConverter.IsLittleEndian);

            ulong value = 0x8877665544332211; // [11 22 33 44 55 66 77 88]
            Span<byte> span;
            unsafe {
                span = new Span<byte>(&value, 8);
            }

            Assert.Equal<byte>(0x11, span.ReadBigEndian<byte>());
            Assert.Equal<byte>(0x11, span.ReadLittleEndian<byte>());
            Assert.Equal<sbyte>(0x11, span.ReadBigEndian<sbyte>());
            Assert.Equal<sbyte>(0x11, span.ReadLittleEndian<sbyte>());

            Assert.Equal<ushort>(0x1122, span.ReadBigEndian<ushort>());
            Assert.Equal<ushort>(0x2211, span.ReadLittleEndian<ushort>());
            Assert.Equal<short>(0x1122, span.ReadBigEndian<short>());
            Assert.Equal<short>(0x2211, span.ReadLittleEndian<short>());

            Assert.Equal<uint>(0x11223344, span.ReadBigEndian<uint>());
            Assert.Equal<uint>(0x44332211, span.ReadLittleEndian<uint>());
            Assert.Equal<int>(0x11223344, span.ReadBigEndian<int>());
            Assert.Equal<int>(0x44332211, span.ReadLittleEndian<int>());

            Assert.Equal<ulong>(0x1122334455667788, span.ReadBigEndian<ulong>());
            Assert.Equal<ulong>(0x8877665544332211, span.ReadLittleEndian<ulong>());
            Assert.Equal<long>(0x1122334455667788, span.ReadBigEndian<long>());
            Assert.Equal<long>(unchecked((long)0x8877665544332211), span.ReadLittleEndian<long>());

        }
    }
}