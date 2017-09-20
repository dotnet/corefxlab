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
                UL0 = ulong.MaxValue,
                S1 = short.MinValue,
                I1 = int.MinValue,
                L1 = long.MinValue,
                US1 = ushort.MinValue,
                UI1 = uint.MinValue,
                UL1 = ulong.MinValue
            };

            Span<byte> spanBE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanBE.WriteBigEndianInt16(myStruct.S0);
            spanBE.Slice(2).WriteBigEndianInt32(myStruct.I0);
            spanBE.Slice(6).WriteBigEndianInt64(myStruct.L0);
            spanBE.Slice(14).WriteBigEndianUInt16(myStruct.US0);
            spanBE.Slice(16).WriteBigEndianUInt32(myStruct.UI0);
            spanBE.Slice(20).WriteBigEndianUInt64(myStruct.UL0);
            spanBE.Slice(28).WriteBigEndianInt16(myStruct.S1);
            spanBE.Slice(30).WriteBigEndianInt32(myStruct.I1);
            spanBE.Slice(34).WriteBigEndianInt64(myStruct.L1);
            spanBE.Slice(42).WriteBigEndianUInt16(myStruct.US1);
            spanBE.Slice(44).WriteBigEndianUInt32(myStruct.UI1);
            spanBE.Slice(48).WriteBigEndianUInt64(myStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanBE.ReadBigEndianInt16(),
                I0 = spanBE.Slice(2).ReadBigEndianInt32(),
                L0 = spanBE.Slice(6).ReadBigEndianInt64(),
                US0 = spanBE.Slice(14).ReadBigEndianUInt16(),
                UI0 = spanBE.Slice(16).ReadBigEndianUInt32(),
                UL0 = spanBE.Slice(20).ReadBigEndianUInt64(),
                S1 = spanBE.Slice(28).ReadBigEndianInt16(),
                I1 = spanBE.Slice(30).ReadBigEndianInt32(),
                L1 = spanBE.Slice(34).ReadBigEndianInt64(),
                US1 = spanBE.Slice(42).ReadBigEndianUInt16(),
                UI1 = spanBE.Slice(44).ReadBigEndianUInt32(),
                UL1 = spanBE.Slice(48).ReadBigEndianUInt64()
            };

            Assert.Equal(myStruct.S0, readStruct.S0);
            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L0, readStruct.L0);
            Assert.Equal(myStruct.US0, readStruct.US0);
            Assert.Equal(myStruct.UI0, readStruct.UI0);
            Assert.Equal(myStruct.UL0, readStruct.UL0);
            Assert.Equal(myStruct.S1, readStruct.S1);
            Assert.Equal(myStruct.I1, readStruct.I1);
            Assert.Equal(myStruct.L1, readStruct.L1);
            Assert.Equal(myStruct.US1, readStruct.US1);
            Assert.Equal(myStruct.UI1, readStruct.UI1);
            Assert.Equal(myStruct.UL1, readStruct.UL1);
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
                UL0 = ulong.MaxValue,
                S1 = short.MinValue,
                I1 = int.MinValue,
                L1 = long.MinValue,
                US1 = ushort.MinValue,
                UI1 = uint.MinValue,
                UL1 = ulong.MinValue
            };

            Span<byte> spanLE = new byte[Unsafe.SizeOf<TestStruct>()];

            spanLE.WriteLittleEndianInt16(myStruct.S0);
            spanLE.Slice(2).WriteLittleEndianInt32(myStruct.I0);
            spanLE.Slice(6).WriteLittleEndianInt64(myStruct.L0);
            spanLE.Slice(14).WriteLittleEndianUInt16(myStruct.US0);
            spanLE.Slice(16).WriteLittleEndianUInt32(myStruct.UI0);
            spanLE.Slice(20).WriteLittleEndianUInt64(myStruct.UL0);
            spanLE.Slice(28).WriteLittleEndianInt16(myStruct.S1);
            spanLE.Slice(30).WriteLittleEndianInt32(myStruct.I1);
            spanLE.Slice(34).WriteLittleEndianInt64(myStruct.L1);
            spanLE.Slice(42).WriteLittleEndianUInt16(myStruct.US1);
            spanLE.Slice(44).WriteLittleEndianUInt32(myStruct.UI1);
            spanLE.Slice(48).WriteLittleEndianUInt64(myStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanLE.ReadLittleEndianInt16(),
                I0 = spanLE.Slice(2).ReadLittleEndianInt32(),
                L0 = spanLE.Slice(6).ReadLittleEndianInt64(),
                US0 = spanLE.Slice(14).ReadLittleEndianUInt16(),
                UI0 = spanLE.Slice(16).ReadLittleEndianUInt32(),
                UL0 = spanLE.Slice(20).ReadLittleEndianUInt64(),
                S1 = spanLE.Slice(28).ReadLittleEndianInt16(),
                I1 = spanLE.Slice(30).ReadLittleEndianInt32(),
                L1 = spanLE.Slice(34).ReadLittleEndianInt64(),
                US1 = spanLE.Slice(42).ReadLittleEndianUInt16(),
                UI1 = spanLE.Slice(44).ReadLittleEndianUInt32(),
                UL1 = spanLE.Slice(48).ReadLittleEndianUInt64()
            };

            Assert.Equal(myStruct.S0, readStruct.S0);
            Assert.Equal(myStruct.I0, readStruct.I0);
            Assert.Equal(myStruct.L0, readStruct.L0);
            Assert.Equal(myStruct.US0, readStruct.US0);
            Assert.Equal(myStruct.UI0, readStruct.UI0);
            Assert.Equal(myStruct.UL0, readStruct.UL0);
            Assert.Equal(myStruct.S1, readStruct.S1);
            Assert.Equal(myStruct.I1, readStruct.I1);
            Assert.Equal(myStruct.L1, readStruct.L1);
            Assert.Equal(myStruct.US1, readStruct.US1);
            Assert.Equal(myStruct.UI1, readStruct.UI1);
            Assert.Equal(myStruct.UL1, readStruct.UL1);
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
            public short S1;
            public int I1;
            public long L1;
            public ushort US1;
            public uint UI1;
            public ulong UL1;
        }
    }
}
