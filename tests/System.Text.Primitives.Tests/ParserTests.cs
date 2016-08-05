using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class ParserTests
    {
        private byte[] UtfEncode(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
        
        #region ulong
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("The biggest ulong is 9223372036854775808.", true, 21, 9223372036854775808, 19)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("The biggest ulong is 9223372036854775808.", true, 21, 9223372036854775808, 19)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region uint
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region ushort
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh37511", true, 9, 37511, 5)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("65535", true, 0, 65535, 5)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh37511", true, 9, 37511, 5)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("65535", true, 0, 65535, 5)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region byte
        [Theory]
        [InlineData("172", true, 0, 172, 3)]
        [InlineData("blahblahh37", true, 9, 37, 2)]
        [InlineData("187abhced", true, 0, 187, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("255", true, 0, 255, 3)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteArrayToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("172", true, 0, 172, 3)]
        [InlineData("blahblahh37", true, 9, 37, 2)]
        [InlineData("187abhced", true, 0, 187, 3)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("255", true, 0, 255, 3)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region long
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("The smallest long is -9223372036854775808.", true, 21, -9223372036854775808, 20)]
        [InlineData("Letthem-32984eatcake", true, 7, -32984, 6)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 19)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 20)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("The smallest long is -9223372036854775808.", true, 21, -9223372036854775808, 20)]
        [InlineData("Letthem-32984eatcake", true, 7, -32984, 6)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 19)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 20)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region int
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("-2147483648", true, 0, -2147483648, 11)]
        [InlineData("Letthem-32984eatcake", true, 7, -32984, 6)]
        [InlineData("2147483647", true, 0, 2147483647, 10)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh1751110", true, 9, 1751110, 7)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("-2147483648", true, 0, -2147483648, 11)]
        [InlineData("Letthem-32984eatcake", true, 7, -32984, 6)]
        [InlineData("2147483647", true, 0, 2147483647, 10)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region short
        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh17511", true, 9, 17511, 5)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("-32768", true, 0, -32768, 6)]
        [InlineData("Letthem-32684eatcake", true, 7, -32684, 6)]
        [InlineData("32767", true, 0, 32767, 5)] // max
        [InlineData("-32768", true, 0, -32768, 6)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("1728", true, 0, 1728, 4)]
        [InlineData("blahblahh17511", true, 9, 17511, 5)]
        [InlineData("987abcdefg", true, 0, 987, 3)]
        [InlineData("-32768", true, 0, -32768, 6)]
        [InlineData("32767", true, 0, 32767, 5)] // max
        [InlineData("-32768", true, 0, -32768, 6)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region sbyte
        [Theory]
        [InlineData("blahblahh125", true, 9, 125, 3)]
        [InlineData("127acndasjfh", true, 0, 127, 3)]
        [InlineData("Letthem-126eatcake", true, 7, -126, 4)]
        [InlineData("127", true, 0, 127, 3)] // max
        [InlineData("-128", true, 0, -128, 4)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("blahblahh125", true, 9, 125, 3)]
        [InlineData("127acndasjfh", true, 0, 127, 3)]
        [InlineData("Letthem-126eatcake", true, 7, -126, 4)]
        [InlineData("127", true, 0, 127, 3)] // max
        [InlineData("-128", true, 0, -128, 4)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }
        #endregion

        #region float

        [Theory]
        [InlineData(".1728", true, 0, 0.1728f, 5)]
        [InlineData("blahblahh175.1110", true, 9, 175.1110f, 8)]
        [InlineData("+98.7abcdefg", true, 0, 98.7, 5)]
        [InlineData("A small float is -0.10000000001", true, 17, -0.10000000001f, 14)]
        [InlineData("-3400000000000000000000000", true, 0, -3400000000000000000000000f, 26)] // min value
        [InlineData("3400000000000000000000000", true, 0, 3400000000000000000000000f, 25)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("1844674407370955161600000000000000000000000000000000000000", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToFloat(string text, bool expectSuccess, int index, float expectedValue, int expectedBytesConsumed)
        {
            float parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(UtfEncode(text), index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        #endregion
    }
}
