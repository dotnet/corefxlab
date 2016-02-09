using Xunit;

namespace System.Slices.Tests
{
    public class CastTests
    {
        [Fact]
        public void IntArraySpanCastedToByteArraySpanHasSameBytesAsOriginalArray()
        {
            var ints = new int[100000];
            Random r = new Random(42324232);
            for (int i = 0; i < ints.Length; i++) { ints[i] = r.Next(); }
            var bytes = ints.Slice().Cast<int, byte>();
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
            var ints = bytes.Slice().Cast<byte, int>();
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
            var sourceSlice = new SevenBytesStruct[sourceLength].Slice();

            var targetSlice = sourceSlice.Cast<SevenBytesStruct, short>();

            Assert.Equal((sourceLength * 7) / sizeof(short), targetSlice.Length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void WhenSourceDoesntFitIntoTargetLengthIsZero(int sourceLength)
        {
            var sourceSlice = new short[sourceLength].Slice();

            var targetSlice = sourceSlice.Cast<short, SevenBytesStruct>();

            Assert.Equal(0, targetSlice.Length);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        public void WhenSourceFitsIntoTargetOnceLengthIsOne(int sourceLength)
        {
            var sourceSlice = new short[sourceLength].Slice();

            var targetSlice = sourceSlice.Cast<short, SevenBytesStruct>();

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

                Assert.Throws<ArgumentException>(() =>
                {
                    var targetSlice = sourceSlice.Cast<SevenBytesStruct, short>();
                });
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
