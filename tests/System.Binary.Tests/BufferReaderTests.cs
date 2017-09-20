// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
            unsafe
            {
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

        [Fact]
        public void BufferReaderReadHomogenousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            var myStruct = new HomogenousTestStruct
            {
                I0 = 12345, // 00 00 48 57 => BE => 959447040
                I1 = 5316513, // 00 81 31 161 => BE => 2703184128 => (integer overflow) -1591783168
                I2 = 790 // 00 00 03 22 => BE => 369295360
            };

            Span<byte> spanBE = new byte[Unsafe.SizeOf<HomogenousTestStruct>()];

            spanBE.WriteBigEndian(myStruct.I0);
            spanBE.Slice(4).WriteBigEndian(myStruct.I1);
            spanBE.Slice(8).WriteBigEndian(myStruct.I2);

            HomogenousTestStruct readStruct = spanBE.Read<HomogenousTestStruct>();

            if (BitConverter.IsLittleEndian)
            {
                readStruct.I0 = readStruct.I0.ReverseEndianness();
                readStruct.I1 = readStruct.I1.ReverseEndianness();
                readStruct.I2 = readStruct.I2.ReverseEndianness();
            }

            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.I1, readStruct.I1);
            Assert.Equal(myStruct.I2, readStruct.I2);
        }

        [Fact]
        public void BufferReaderReadHeterogeneousStructWILLFAIL()
        {
            Assert.True(BitConverter.IsLittleEndian);

            var myStruct = new HeterogenousTestStruct
            {
                I0 = 12345, // 00 00 48 57 => BE => 959447040
                L1 = 4147483647,
                I2 = int.MaxValue,
                L3 = long.MinValue,
                I4 = int.MinValue,
                L5 = long.MaxValue
            };

            Span<byte> spanBE = new byte[Unsafe.SizeOf<HeterogenousTestStruct>()];

            spanBE.WriteBigEndian(myStruct.I0);
            spanBE.Slice(4).WriteBigEndian(myStruct.L1);
            spanBE.Slice(12).WriteBigEndian(myStruct.I2);
            spanBE.Slice(16).WriteBigEndian(myStruct.L3);
            spanBE.Slice(24).WriteBigEndian(myStruct.I4);
            spanBE.Slice(28).WriteBigEndian(myStruct.L5);

            HeterogenousTestStruct readStruct = spanBE.Read<HeterogenousTestStruct>();

            // This does not work!
            if (BitConverter.IsLittleEndian)
            {
                readStruct.I0 = readStruct.I0.ReverseEndianness();
                readStruct.L1 = readStruct.L1.ReverseEndianness();
                readStruct.I2 = readStruct.I2.ReverseEndianness();
                readStruct.L3 = readStruct.L3.ReverseEndianness();
                readStruct.I4 = readStruct.I4.ReverseEndianness();
                readStruct.L5 = readStruct.L5.ReverseEndianness();
            }

            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L1, readStruct.L1);
            Assert.Equal(myStruct.I2, readStruct.I2);
            Assert.Equal(myStruct.L3, readStruct.L3);
            Assert.Equal(myStruct.I4, readStruct.I4);
            Assert.Equal(myStruct.L5, readStruct.L5);
        }

        [Fact]
        public void BufferReaderReadHeterogeneousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            var myStruct = new HeterogenousTestStruct
            {
                I0 = 12345, // 00 00 48 57 => BE => 959447040
                L1 = 4147483647,
                I2 = int.MaxValue,
                L3 = long.MinValue,
                I4 = int.MinValue,
                L5 = long.MaxValue
            };

            Span<byte> spanBE = new byte[Unsafe.SizeOf<HeterogenousTestStruct>()];

            spanBE.WriteBigEndian(myStruct.I0);
            spanBE.Slice(4).WriteBigEndian(myStruct.L1);
            spanBE.Slice(12).WriteBigEndian(myStruct.I2);
            spanBE.Slice(16).WriteBigEndian(myStruct.L3);
            spanBE.Slice(24).WriteBigEndian(myStruct.I4);
            spanBE.Slice(28).WriteBigEndian(myStruct.L5);

            HeterogenousTestStruct readStruct = new HeterogenousTestStruct
            {
                I0 = spanBE.ReadBigEndian<int>(),
                L1 = spanBE.Slice(4).ReadBigEndian<long>(),
                I2 = spanBE.Slice(12).ReadBigEndian<int>(),
                L3 = spanBE.Slice(16).ReadBigEndian<long>(),
                I4 = spanBE.Slice(24).ReadBigEndian<int>(),
                L5 = spanBE.Slice(28).ReadBigEndian<long>()
            };

            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L1, readStruct.L1);
            Assert.Equal(myStruct.I2, readStruct.I2);
            Assert.Equal(myStruct.L3, readStruct.L3);
            Assert.Equal(myStruct.I4, readStruct.I4);
            Assert.Equal(myStruct.L5, readStruct.L5);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HeterogenousTestStruct
        {
            public int I0;
            public long L1;
            public int I2;
            public long L3;
            public int I4;
            public long L5;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HomogenousTestStruct
        {
            public int I0;
            public int I1;
            public int I2;
        }
    }
}
