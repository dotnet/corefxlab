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

            Assert.Equal<ushort>(0x1122, span.ReadUInt16BigEndian());
            Assert.True(span.TryReadUInt16BigEndian(out ushort ushortValue));
            Assert.Equal(0x1122, ushortValue);

            Assert.Equal<ushort>(0x2211, span.ReadUInt16LittleEndian());
            Assert.True(span.TryReadUInt16LittleEndian(out ushortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<short>(0x1122, span.ReadInt16BigEndian());
            Assert.True(span.TryReadInt16BigEndian(out short shortValue));
            Assert.Equal(0x1122, shortValue);

            Assert.Equal<short>(0x2211, span.ReadInt16LittleEndian());
            Assert.True(span.TryReadInt16LittleEndian(out shortValue));
            Assert.Equal(0x2211, ushortValue);

            Assert.Equal<uint>(0x11223344, span.ReadUInt32BigEndian());
            Assert.True(span.TryReadUInt32BigEndian(out uint uintValue));
            Assert.Equal<uint>(0x11223344, uintValue);

            Assert.Equal<uint>(0x44332211, span.ReadUInt32LittleEndian());
            Assert.True(span.TryReadUInt32LittleEndian(out uintValue));
            Assert.Equal<uint>(0x44332211, uintValue);

            Assert.Equal<int>(0x11223344, span.ReadInt32BigEndian());
            Assert.True(span.TryReadInt32BigEndian(out int intValue));
            Assert.Equal<int>(0x11223344, intValue);

            Assert.Equal<int>(0x44332211, span.ReadInt32LittleEndian());
            Assert.True(span.TryReadInt32LittleEndian(out intValue));
            Assert.Equal<int>(0x44332211, intValue);

            Assert.Equal<ulong>(0x1122334455667788, span.ReadUInt64BigEndian());
            Assert.True(span.TryReadUInt64BigEndian(out ulong ulongValue));
            Assert.Equal<ulong>(0x1122334455667788, ulongValue);

            Assert.Equal<ulong>(0x8877665544332211, span.ReadUInt64LittleEndian());
            Assert.True(span.TryReadUInt64LittleEndian(out ulongValue));
            Assert.Equal<ulong>(0x8877665544332211, ulongValue);

            Assert.Equal<long>(0x1122334455667788, span.ReadInt64BigEndian());
            Assert.True(span.TryReadInt64BigEndian(out long longValue));
            Assert.Equal<long>(0x1122334455667788, longValue);

            Assert.Equal<long>(unchecked((long)0x8877665544332211), span.ReadInt64LittleEndian());
            Assert.True(span.TryReadInt64LittleEndian(out longValue));
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

            spanBE.WriteInt16BigEndian(myStruct.S0);
            spanBE.Slice(2).WriteInt32BigEndian(myStruct.I0);
            spanBE.Slice(6).WriteInt64BigEndian(myStruct.L0);
            spanBE.Slice(14).WriteUInt16BigEndian(myStruct.US0);
            spanBE.Slice(16).WriteUInt32BigEndian(myStruct.UI0);
            spanBE.Slice(20).WriteUInt64BigEndian(myStruct.UL0);
            spanBE.Slice(28).WriteInt16BigEndian(myStruct.S1);
            spanBE.Slice(30).WriteInt32BigEndian(myStruct.I1);
            spanBE.Slice(34).WriteInt64BigEndian(myStruct.L1);
            spanBE.Slice(42).WriteUInt16BigEndian(myStruct.US1);
            spanBE.Slice(44).WriteUInt32BigEndian(myStruct.UI1);
            spanBE.Slice(48).WriteUInt64BigEndian(myStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanBE.ReadInt16BigEndian(),
                I0 = spanBE.Slice(2).ReadInt32BigEndian(),
                L0 = spanBE.Slice(6).ReadInt64BigEndian(),
                US0 = spanBE.Slice(14).ReadUInt16BigEndian(),
                UI0 = spanBE.Slice(16).ReadUInt32BigEndian(),
                UL0 = spanBE.Slice(20).ReadUInt64BigEndian(),
                S1 = spanBE.Slice(28).ReadInt16BigEndian(),
                I1 = spanBE.Slice(30).ReadInt32BigEndian(),
                L1 = spanBE.Slice(34).ReadInt64BigEndian(),
                US1 = spanBE.Slice(42).ReadUInt16BigEndian(),
                UI1 = spanBE.Slice(44).ReadUInt32BigEndian(),
                UL1 = spanBE.Slice(48).ReadUInt64BigEndian()
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

            spanLE.WriteInt16LittleEndian(myStruct.S0);
            spanLE.Slice(2).WriteInt32LittleEndian(myStruct.I0);
            spanLE.Slice(6).WriteInt64LittleEndian(myStruct.L0);
            spanLE.Slice(14).WriteUInt16LittleEndian(myStruct.US0);
            spanLE.Slice(16).WriteUInt32LittleEndian(myStruct.UI0);
            spanLE.Slice(20).WriteUInt64LittleEndian(myStruct.UL0);
            spanLE.Slice(28).WriteInt16LittleEndian(myStruct.S1);
            spanLE.Slice(30).WriteInt32LittleEndian(myStruct.I1);
            spanLE.Slice(34).WriteInt64LittleEndian(myStruct.L1);
            spanLE.Slice(42).WriteUInt16LittleEndian(myStruct.US1);
            spanLE.Slice(44).WriteUInt32LittleEndian(myStruct.UI1);
            spanLE.Slice(48).WriteUInt64LittleEndian(myStruct.UL1);

            var readStruct = new TestStruct
            {
                S0 = spanLE.ReadInt16LittleEndian(),
                I0 = spanLE.Slice(2).ReadInt32LittleEndian(),
                L0 = spanLE.Slice(6).ReadInt64LittleEndian(),
                US0 = spanLE.Slice(14).ReadUInt16LittleEndian(),
                UI0 = spanLE.Slice(16).ReadUInt32LittleEndian(),
                UL0 = spanLE.Slice(20).ReadUInt64LittleEndian(),
                S1 = spanLE.Slice(28).ReadInt16LittleEndian(),
                I1 = spanLE.Slice(30).ReadInt32LittleEndian(),
                L1 = spanLE.Slice(34).ReadInt64LittleEndian(),
                US1 = spanLE.Slice(42).ReadUInt16LittleEndian(),
                UI1 = spanLE.Slice(44).ReadUInt32LittleEndian(),
                UL1 = spanLE.Slice(48).ReadUInt64LittleEndian()
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
