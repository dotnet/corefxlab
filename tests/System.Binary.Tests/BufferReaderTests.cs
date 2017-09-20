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
            unsafe {
                span = new Span<byte>(&value, 8);
            }

            Assert.Equal<byte>(0x11, span.Read<byte>());
            Assert.True(span.TryRead(out byte byteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<sbyte>(0x11, span.Read<sbyte>());
            Assert.True(span.TryRead(out byte sbyteValue));
            Assert.Equal(0x11, byteValue);

            Assert.Equal<ushort>(0x1122, span.ReadBigEndianUInt16());
            Assert.True(span.TryReadBigEndianUInt16(out ushort ushortValue));
            Assert.Equal(0x1122, ushortValue);

            Assert.Equal<ushort>(0x2211, span.ReadLittleEndianUInt16());
            Assert.True(span.TryReadLittleEndianUInt16(out ushortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<short>(0x1122, span.ReadBigEndianInt16());
            Assert.True(span.TryReadBigEndianInt16(out short shortValue));
            Assert.Equal(0x1122, shortValue);

            Assert.Equal<short>(0x2211, span.ReadLittleEndianInt16());
            Assert.True(span.TryReadLittleEndianInt16(out shortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<uint>(0x11223344, span.ReadBigEndianUInt32());
            Assert.True(span.TryReadBigEndianUInt32(out uint uintValue));
            Assert.Equal<uint>(0x11223344, uintValue);

            Assert.Equal<uint>(0x44332211, span.ReadLittleEndianUInt32());
            Assert.True(span.TryReadLittleEndianUInt32(out uintValue));
            Assert.Equal<uint>(0x44332211, uintValue);

            Assert.Equal<int>(0x11223344, span.ReadBigEndianInt32());
            Assert.True(span.TryReadBigEndianInt32(out int intValue));
            Assert.Equal<int>(0x11223344, intValue);

            Assert.Equal<int>(0x44332211, span.ReadLittleEndianInt32());
            Assert.True(span.TryReadLittleEndianInt32(out intValue));
            Assert.Equal<int>(0x44332211, intValue);

            Assert.Equal<ulong>(0x1122334455667788, span.ReadBigEndianUInt64());
            Assert.True(span.TryReadBigEndianUInt64(out ulong ulongValue));
            Assert.Equal<ulong>(0x1122334455667788, ulongValue);

            Assert.Equal<ulong>(0x8877665544332211, span.ReadLittleEndianUInt64());
            Assert.True(span.TryReadLittleEndianUInt64(out ulongValue));
            Assert.Equal<ulong>(0x8877665544332211, ulongValue);

            Assert.Equal<long>(0x1122334455667788, span.ReadBigEndianInt64());
            Assert.True(span.TryReadBigEndianInt64(out long longValue));
            Assert.Equal<long>(0x1122334455667788, longValue);

            Assert.Equal<long>(unchecked((long)0x8877665544332211), span.ReadLittleEndianInt64());
            Assert.True(span.TryReadLittleEndianInt64(out longValue));
            Assert.Equal<long>(unchecked((long)0x8877665544332211), longValue);
        }

        [Fact]
        public void BufferWriteReadBigEndianHeterogeneousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            var myStruct = new TestStruct
            {
                S0 = short.MaxValue,
                I0 = int.MaxValue,
                L0 = long.MaxValue,
                US0 = ushort.MaxValue,
                UI0 = uint.MaxValue,
                UL0 = ulong.MaxValue
            };

            Span<byte> spanBE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanBE.WriteBigEndianInt16(myStruct.S0);
            spanBE.Slice(2).WriteBigEndianInt32(myStruct.I0);
            spanBE.Slice(2 + 4).WriteBigEndianInt64(myStruct.L0);
            spanBE.Slice(2 + 4 + 8).WriteBigEndianUInt16(myStruct.US0);
            spanBE.Slice(2 + 4 + 8 + 2).WriteBigEndianUInt32(myStruct.UI0);
            spanBE.Slice(2 + 4 + 8 + 2 + 4).WriteBigEndianUInt64(myStruct.UL0);

            var readStruct = new TestStruct
            {
                S0 = spanBE.ReadBigEndianInt16(),
                I0 = spanBE.Slice(2).ReadBigEndianInt32(),
                L0 = spanBE.Slice(2 + 4).ReadBigEndianInt64(),
                US0 = spanBE.Slice(2 + 4 + 8).ReadBigEndianUInt16(),
                UI0 = spanBE.Slice(2 + 4 + 8 + 2).ReadBigEndianUInt32(),
                UL0 = spanBE.Slice(2 + 4 + 8 + 2 + 4).ReadBigEndianUInt64(),
            };

            Assert.Equal(myStruct.S0, readStruct.S0);
            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L0, readStruct.L0);
            Assert.Equal(myStruct.US0, readStruct.US0);
            Assert.Equal(myStruct.UI0, readStruct.UI0);
            Assert.Equal(myStruct.UL0, readStruct.UL0);
        }

        [Fact]
        public void BufferWriteReadLittleEndianHeterogeneousStruct()
        {
            Assert.True(BitConverter.IsLittleEndian);

            var myStruct = new TestStruct
            {
                S0 = short.MaxValue,
                I0 = int.MaxValue,
                L0 = long.MaxValue,
                US0 = ushort.MaxValue,
                UI0 = uint.MaxValue,
                UL0 = ulong.MaxValue
            };

            Span<byte> spanLE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanLE.WriteLittleEndianInt16(myStruct.S0);
            spanLE.Slice(2).WriteLittleEndianInt32(myStruct.I0);
            spanLE.Slice(2 + 4).WriteLittleEndianInt64(myStruct.L0);
            spanLE.Slice(2 + 4 + 8).WriteLittleEndianUInt16(myStruct.US0);
            spanLE.Slice(2 + 4 + 8 + 2).WriteLittleEndianUInt32(myStruct.UI0);
            spanLE.Slice(2 + 4 + 8 + 2 + 4).WriteLittleEndianUInt64(myStruct.UL0);

            var readStruct = new TestStruct
            {
                S0 = spanLE.ReadLittleEndianInt16(),
                I0 = spanLE.Slice(2).ReadLittleEndianInt32(),
                L0 = spanLE.Slice(2 + 4).ReadLittleEndianInt64(),
                US0 = spanLE.Slice(2 + 4 + 8).ReadLittleEndianUInt16(),
                UI0 = spanLE.Slice(2 + 4 + 8 + 2).ReadLittleEndianUInt32(),
                UL0 = spanLE.Slice(2 + 4 + 8 + 2 + 4).ReadLittleEndianUInt64(),
            };

            Assert.Equal(myStruct.S0, readStruct.S0);
            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L0, readStruct.L0);
            Assert.Equal(myStruct.US0, readStruct.US0);
            Assert.Equal(myStruct.UI0, readStruct.UI0);
            Assert.Equal(myStruct.UL0, readStruct.UL0);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TestStruct
        {
            public short S0;
            public int I0;
            public long L0;
            public ushort US0;
            public uint UI0;
            public ulong UL0;
        }
    }
}
