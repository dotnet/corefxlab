// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
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
            Assert.True(span.TryReadBigEndian(out byte byteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<byte>(0x11, span.ReadLittleEndian<byte>());
            Assert.True(span.TryReadLittleEndian(out byteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<sbyte>(0x11, span.ReadBigEndian<sbyte>());
            Assert.True(span.TryReadBigEndian(out byte sbyteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<sbyte>(0x11, span.ReadLittleEndian<sbyte>());
            Assert.True(span.TryReadLittleEndian(out byteValue));
            Assert.Equal(0x11, sbyteValue);

            Assert.Equal<ushort>(0x1122, span.ReadBigEndian<ushort>());
            Assert.True(span.TryReadBigEndian(out ushort ushortValue));
            Assert.Equal(0x1122, ushortValue);

            Assert.Equal<ushort>(0x2211, span.ReadLittleEndian<ushort>());
            Assert.True(span.TryReadLittleEndian(out ushortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<short>(0x1122, span.ReadBigEndian<short>());
            Assert.True(span.TryReadBigEndian(out short shortValue));
            Assert.Equal(0x1122, shortValue);

            Assert.Equal<short>(0x2211, span.ReadLittleEndian<short>());
            Assert.True(span.TryReadLittleEndian(out shortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<uint>(0x11223344, span.ReadBigEndian<uint>());
            Assert.True(span.TryReadBigEndian(out uint uintValue));
            Assert.Equal<uint>(0x11223344, uintValue);

            Assert.Equal<uint>(0x44332211, span.ReadLittleEndian<uint>());
            Assert.True(span.TryReadLittleEndian(out uintValue));
            Assert.Equal<uint>(0x44332211, uintValue);

            Assert.Equal<int>(0x11223344, span.ReadBigEndian<int>());
            Assert.True(span.TryReadBigEndian(out int intValue));
            Assert.Equal<int>(0x11223344, intValue);

            Assert.Equal<int>(0x44332211, span.ReadLittleEndian<int>());
            Assert.True(span.TryReadLittleEndian(out intValue));
            Assert.Equal<int>(0x44332211, intValue);

            Assert.Equal<ulong>(0x1122334455667788, span.ReadBigEndian<ulong>());
            Assert.True(span.TryReadBigEndian(out ulong ulongValue));
            Assert.Equal<ulong>(0x1122334455667788, ulongValue);

            Assert.Equal<ulong>(0x8877665544332211, span.ReadLittleEndian<ulong>());
            Assert.True(span.TryReadLittleEndian(out ulongValue));
            Assert.Equal<ulong>(0x8877665544332211, ulongValue);

            Assert.Equal<long>(0x1122334455667788, span.ReadBigEndian<long>());
            Assert.True(span.TryReadBigEndian(out long longValue));
            Assert.Equal<long>(0x1122334455667788, longValue);

            Assert.Equal<long>(unchecked((long)0x8877665544332211), span.ReadLittleEndian<long>());
            Assert.True(span.TryReadLittleEndian(out longValue));
            Assert.Equal<long>(unchecked((long)0x8877665544332211), longValue);
        }
    }
}
