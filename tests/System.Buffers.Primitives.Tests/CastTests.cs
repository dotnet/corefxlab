// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Xunit;

using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Buffers.Tests
{
    public class CastTests
    {
        [Fact]
        public void TryReadSpan()
        {
            Span<byte> buffer;

            buffer = new Span<byte>(new byte[] { 1, 0, 0, 0 });
            Assert.True(TryRead(buffer, out uint value));
            Assert.Equal(1u, value);

            buffer = new Span<byte>(new byte[] { 1, 0, 0 });
            Assert.False(TryRead(buffer, out value));
        }

        [Fact]
        public void TryReadReadOnlySpan()
        {
            ReadOnlySpan<byte> buffer;

            buffer = new ReadOnlySpan<byte>(new byte[] { 1, 0, 0, 0 });
            Assert.True(TryRead(buffer, out uint value));
            Assert.Equal(1u, value);

            buffer = new ReadOnlySpan<byte>(new byte[] { 1, 0, 0 });
            Assert.False(TryRead(buffer, out value));
        }

        [Fact]
        public void TryWrite()
        {
            Span<byte> buffer = new byte[4];
            uint value = uint.MaxValue;
            Assert.True(MemoryMarshal.TryWrite(buffer, ref value));
            for (var i = 0; i < buffer.Length; i++)
            {
                Assert.Equal(255, buffer[i]);
            }

            buffer = buffer.Slice(1);
            Assert.False(MemoryMarshal.TryWrite(buffer, ref value));
        }

        [Fact]
        public void IntArraySpanCastedToByteArraySpanHasSameBytesAsOriginalArray()
        {
            var ints = new int[100000];
            Random r = new Random(42324232);
            for (int i = 0; i < ints.Length; i++) { ints[i] = r.Next(); }
            var bytes = MemoryMarshal.Cast<int, byte>(ints.AsSpan());
            Assert.Equal(bytes.Length, ints.Length * sizeof(int));
            for (int i = 0; i < ints.Length; i++)
            {
                Assert.Equal(bytes[i * 4], (ints[i] & 0xff));
                Assert.Equal(bytes[i * 4 + 1], (ints[i] >> 8 & 0xff));
                Assert.Equal(bytes[i * 4 + 2], (ints[i] >> 16 & 0xff));
                Assert.Equal(bytes[i * 4 + 3], (ints[i] >> 24 & 0xff));
            }
        }

        [Fact]
        public void ByteArraySpanCastedToIntArraySpanHasSameBytesAsOriginalArray()
        {
            var bytes = new byte[100000];
            Random r = new Random(541345);
            for (int i = 0; i < bytes.Length; i++) { bytes[i] = (byte)r.Next(256); }
            var ints = MemoryMarshal.Cast<byte, int>(bytes.AsSpan());
            Assert.Equal(ints.Length, bytes.Length / sizeof(int));
            for (int i = 0; i < ints.Length; i++)
            {
                Assert.Equal(BitConverter.ToInt32(bytes, i * 4), ints[i]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SourceTypeLargerThanTargetOneCorrectlyCalcsTargetsLength(int sourceLength)
        {
            var sourceSlice = new SevenBytesStruct[sourceLength].AsSpan();

            var targetSlice = MemoryMarshal.Cast<SevenBytesStruct, short>(sourceSlice);

            Assert.Equal((sourceLength * 7) / sizeof(short), targetSlice.Length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void WhenSourceDoesntFitIntoTargetLengthIsZero(int sourceLength)
        {
            var sourceSlice = new short[sourceLength].AsSpan();

            var targetSlice = MemoryMarshal.Cast<short, SevenBytesStruct>(sourceSlice);

            Assert.Equal(0, targetSlice.Length);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        public void WhenSourceFitsIntoTargetOnceLengthIsOne(int sourceLength)
        {
            var sourceSlice = new short[sourceLength].AsSpan();

            var targetSlice = MemoryMarshal.Cast<short, SevenBytesStruct>(sourceSlice);

            Assert.Equal(1, targetSlice.Length);
        }

        [Fact]
        public void WhenSourceTypeLargerThaTargetAndOverflowsInt32ThrowsException()
        {
            unsafe
            {
                byte dummy;
                int sourceLength = 620000000;
                var sourceSlice = new Span<SevenBytesStruct>(&dummy, sourceLength);

                try
                {
                    var targetSlice = MemoryMarshal.Cast<SevenBytesStruct, short>(sourceSlice);
                    Assert.True(false);
                }
                catch (Exception ex)
                {
                    Assert.True(ex is OverflowException);
                }
            }
        }
    }

    struct SevenBytesStruct
    {
#pragma warning disable CS0169
        byte b1, b2, b3, b4, b5, b6, b7;
#pragma warning restore CS0169
    }
}
