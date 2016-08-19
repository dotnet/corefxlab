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
        private byte[] UtfEncode(string s, bool utf16)
        {
            if (utf16)
                return Encoding.Unicode.GetBytes(s);
            else
                return UtfEncode(s);
        }

		#region byte

		[Theory]
        [InlineData("55", true, 0, 55, 2)]
        [InlineData("blahblahh68", true, 9, 68, 2)]
        [InlineData("68abhced", true, 0, 68, 2)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("255", true, 0, 255, 3)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteArrayToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("55", true, 0, 55, 2)]
        [InlineData("blahblahh68", true, 9, 68, 2)]
        [InlineData("68abhced", true, 0, 68, 2)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("255", true, 0, 255, 3)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, false);
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("55", true, 0, 55, 4)]
        [InlineData("blahblahh68", true, 18, 68, 4)]
        [InlineData("68abhced", true, 0, 68, 4)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("255", true, 0, 255, 6)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteArrayToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("55", true, 0, 55, 4)]
        [InlineData("blahblahh68", true, 18, 68, 4)]
        [InlineData("68abhced", true, 0, 68, 4)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("255", true, 0, 255, 6)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("256", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteStarToByte(string text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, true);
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region ushort

		[Theory]
        [InlineData("5535", true, 0, 5535, 4)]
        [InlineData("blahblahh6836", true, 9, 6836, 4)]
        [InlineData("6836abhced", true, 0, 6836, 4)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("65535", true, 0, 65535, 5)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteArrayToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("5535", true, 0, 5535, 4)]
        [InlineData("blahblahh6836", true, 9, 6836, 4)]
        [InlineData("6836abhced", true, 0, 6836, 4)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("65535", true, 0, 65535, 5)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, false);
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("5535", true, 0, 5535, 8)]
        [InlineData("blahblahh6836", true, 18, 6836, 8)]
        [InlineData("6836abhced", true, 0, 6836, 8)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("65535", true, 0, 65535, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteArrayToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("5535", true, 0, 5535, 8)]
        [InlineData("blahblahh6836", true, 18, 6836, 8)]
        [InlineData("6836abhced", true, 0, 6836, 8)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("65535", true, 0, 65535, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("65536", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteStarToUshort(string text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, true);
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region uint

		[Theory]
        [InlineData("294967295", true, 0, 294967295, 9)]
        [InlineData("blahblahh354864498", true, 9, 354864498, 9)]
        [InlineData("354864498abhced", true, 0, 354864498, 9)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteArrayToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("294967295", true, 0, 294967295, 9)]
        [InlineData("blahblahh354864498", true, 9, 354864498, 9)]
        [InlineData("354864498abhced", true, 0, 354864498, 9)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 10)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, false);
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("294967295", true, 0, 294967295, 18)]
        [InlineData("blahblahh354864498", true, 18, 354864498, 18)]
        [InlineData("354864498abhced", true, 0, 354864498, 18)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteArrayToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("294967295", true, 0, 294967295, 18)]
        [InlineData("blahblahh354864498", true, 18, 354864498, 18)]
        [InlineData("354864498abhced", true, 0, 354864498, 18)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("4294967295", true, 0, 4294967295, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("4294967296", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteStarToUint(string text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, true);
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region ulong

		[Theory]
        [InlineData("8446744073709551615", true, 0, 8446744073709551615, 19)]
        [InlineData("blahblahh6745766045317562215", true, 9, 6745766045317562215, 19)]
        [InlineData("6745766045317562215abhced", true, 0, 6745766045317562215, 19)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteArrayToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("8446744073709551615", true, 0, 8446744073709551615, 19)]
        [InlineData("blahblahh6745766045317562215", true, 9, 6745766045317562215, 19)]
        [InlineData("6745766045317562215abhced", true, 0, 6745766045317562215, 19)]
        [InlineData("0", true, 0, 0, 1)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 20)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, false);
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("8446744073709551615", true, 0, 8446744073709551615, 38)]
        [InlineData("blahblahh6745766045317562215", true, 18, 6745766045317562215, 38)]
        [InlineData("6745766045317562215abhced", true, 0, 6745766045317562215, 38)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 40)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteArrayToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("8446744073709551615", true, 0, 8446744073709551615, 38)]
        [InlineData("blahblahh6745766045317562215", true, 18, 6745766045317562215, 38)]
        [InlineData("6745766045317562215abhced", true, 0, 6745766045317562215, 38)]
        [InlineData("0", true, 0, 0, 2)] // min value
        [InlineData("18446744073709551615", true, 0, 18446744073709551615, 40)] // max value
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("18446744073709551616", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf16ByteStarToUlong(string text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;

            byte[] textBytes = UtfEncode(text, true);
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('G');
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf, out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region sbyte

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh27", true, 9, 27, 2)]
        [InlineData("53abcdefg", true, 0, 53, 2)]
        [InlineData("The smallest of this type is -128.", true, 29, -128, 4)]
        [InlineData("Letthem-28eatcake", true, 7, -28, 3)]
        [InlineData("127", true, 0, 127, 3)] // max
        [InlineData("-128", true, 0, -128, 4)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh27", true, 9, 27, 2)]
        [InlineData("53abcdefg", true, 0, 53, 2)]
        [InlineData("The smallest of this type is -128.", true, 29, -128, 4)]
        [InlineData("Letthem-28eatcake", true, 7, -28, 3)]
        [InlineData("127", true, 0, 127, 3)] // max
        [InlineData("-128", true, 0, -128, 4)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, false);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh27", true, 18, 27, 4)]
        [InlineData("53abcdefg", true, 0, 53, 4)]
        [InlineData("The smallest of this type is -128.", true, 58, -128, 8)]
        [InlineData("Letthem-28eatcake", true, 14, -28, 6)]
        [InlineData("127", true, 0, 127, 6)] // max
        [InlineData("-128", true, 0, -128, 8)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf16ByteArrayToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh27", true, 18, 27, 4)]
        [InlineData("53abcdefg", true, 0, 53, 4)]
        [InlineData("The smallest of this type is -128.", true, 58, -128, 8)]
        [InlineData("Letthem-28eatcake", true, 14, -28, 6)]
        [InlineData("127", true, 0, 127, 6)] // max
        [InlineData("-128", true, 0, -128, 8)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("128", false, 0, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf16ByteStarToSbyte(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedBytesConsumed)
        {
            sbyte parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, true);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region short

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh2767", true, 9, 2767, 4)]
        [InlineData("5333abcdefg", true, 0, 5333, 4)]
        [InlineData("The smallest of this type is -32768.", true, 29, -32768, 6)]
        [InlineData("Letthem-2768eatcake", true, 7, -2768, 5)]
        [InlineData("32767", true, 0, 32767, 5)] // max
        [InlineData("-32768", true, 0, -32768, 6)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh2767", true, 9, 2767, 4)]
        [InlineData("5333abcdefg", true, 0, 5333, 4)]
        [InlineData("The smallest of this type is -32768.", true, 29, -32768, 6)]
        [InlineData("Letthem-2768eatcake", true, 7, -2768, 5)]
        [InlineData("32767", true, 0, 32767, 5)] // max
        [InlineData("-32768", true, 0, -32768, 6)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, false);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh2767", true, 18, 2767, 8)]
        [InlineData("5333abcdefg", true, 0, 5333, 8)]
        [InlineData("The smallest of this type is -32768.", true, 58, -32768, 12)]
        [InlineData("Letthem-2768eatcake", true, 14, -2768, 10)]
        [InlineData("32767", true, 0, 32767, 10)] // max
        [InlineData("-32768", true, 0, -32768, 12)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf16ByteArrayToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh2767", true, 18, 2767, 8)]
        [InlineData("5333abcdefg", true, 0, 5333, 8)]
        [InlineData("The smallest of this type is -32768.", true, 58, -32768, 12)]
        [InlineData("Letthem-2768eatcake", true, 14, -2768, 10)]
        [InlineData("32767", true, 0, 32767, 10)] // max
        [InlineData("-32768", true, 0, -32768, 12)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf16ByteStarToShort(string text, bool expectSuccess, int index, short expectedValue, int expectedBytesConsumed)
        {
            short parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, true);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region int

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh147483647", true, 9, 147483647, 9)]
        [InlineData("474753647abcdefg", true, 0, 474753647, 9)]
        [InlineData("The smallest of this type is -2147483648.", true, 29, -2147483648, 11)]
        [InlineData("Letthem-147483648eatcake", true, 7, -147483648, 10)]
        [InlineData("2147483647", true, 0, 2147483647, 10)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh147483647", true, 9, 147483647, 9)]
        [InlineData("474753647abcdefg", true, 0, 474753647, 9)]
        [InlineData("The smallest of this type is -2147483648.", true, 29, -2147483648, 11)]
        [InlineData("Letthem-147483648eatcake", true, 7, -147483648, 10)]
        [InlineData("2147483647", true, 0, 2147483647, 10)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, false);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh147483647", true, 18, 147483647, 18)]
        [InlineData("474753647abcdefg", true, 0, 474753647, 18)]
        [InlineData("The smallest of this type is -2147483648.", true, 58, -2147483648, 22)]
        [InlineData("Letthem-147483648eatcake", true, 14, -147483648, 20)]
        [InlineData("2147483647", true, 0, 2147483647, 20)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 22)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf16ByteArrayToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh147483647", true, 18, 147483647, 18)]
        [InlineData("474753647abcdefg", true, 0, 474753647, 18)]
        [InlineData("The smallest of this type is -2147483648.", true, 58, -2147483648, 22)]
        [InlineData("Letthem-147483648eatcake", true, 14, -147483648, 20)]
        [InlineData("2147483647", true, 0, 2147483647, 20)] // max
        [InlineData("-2147483648", true, 0, -2147483648, 22)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("2147483648", false, 0, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf16ByteStarToInt(string text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, true);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region long

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh223372036854775807", true, 9, 223372036854775807, 18)]
        [InlineData("555642036585755426abcdefg", true, 0, 555642036585755426, 18)]
        [InlineData("The smallest of this type is -9223372036854775808.", true, 29, -9223372036854775808, 20)]
        [InlineData("Letthem-223372036854775808eatcake", true, 7, -223372036854775808, 19)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 19)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 20)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf8ByteArrayToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, false), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 3)]
        [InlineData("blahblahh223372036854775807", true, 9, 223372036854775807, 18)]
        [InlineData("555642036585755426abcdefg", true, 0, 555642036585755426, 18)]
        [InlineData("The smallest of this type is -9223372036854775808.", true, 29, -9223372036854775808, 20)]
        [InlineData("Letthem-223372036854775808eatcake", true, 7, -223372036854775808, 19)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 19)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 20)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf8ByteStarToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, false);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh223372036854775807", true, 18, 223372036854775807, 36)]
        [InlineData("555642036585755426abcdefg", true, 0, 555642036585755426, 36)]
        [InlineData("The smallest of this type is -9223372036854775808.", true, 58, -9223372036854775808, 40)]
        [InlineData("Letthem-223372036854775808eatcake", true, 14, -223372036854775808, 38)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 38)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 40)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public void ParseUtf16ByteArrayToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text, true), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData("111", true, 0, 111, 6)]
        [InlineData("blahblahh223372036854775807", true, 18, 223372036854775807, 36)]
        [InlineData("555642036585755426abcdefg", true, 0, 555642036585755426, 36)]
        [InlineData("The smallest of this type is -9223372036854775808.", true, 58, -9223372036854775808, 40)]
        [InlineData("Letthem-223372036854775808eatcake", true, 14, -223372036854775808, 38)]
        [InlineData("9223372036854775807", true, 0, 9223372036854775807, 38)] // max
        [InlineData("-9223372036854775808", true, 0, -9223372036854775808, 40)] // min
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("9223372036854775808", false, 0, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0, 0)] // negative overflow test
        public unsafe void ParseUtf16ByteStarToLong(string text, bool expectSuccess, int index, long expectedValue, int expectedBytesConsumed)
        {
            long parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf16;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text, true);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

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
        [InlineData("+98.7abcdefg", true, 0, 98.7f, 5)]
        [InlineData("A small float is -0.10000000001", true, 17, -0.10000000001f, 14)]
        [InlineData("1.45e12", true, 0, 1.45e12f, 7)]
        [InlineData("1E-8", true, 0, 1e-8f, 4)]
        [InlineData("-3.402822E+38", true, 0, -3.402822E+38f, 13)] // min value
        [InlineData("3.402822E+38", true, 0, 3.402822E+38f, 12)] // max value
        [InlineData("Infinity", true, 0, float.PositiveInfinity, 8)]
        [InlineData("-Infinity", true, 0, float.NegativeInfinity, 9)]
        [InlineData("NaN", true, 0, float.NaN, 3)]
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("1.6540654e100000", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToFloat(string text, bool expectSuccess, int index, float expectedValue, int expectedBytesConsumed)
        {
            float parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData(".1728", true, 0, 0.1728f, 5)]
        [InlineData("blahblahh175.1110", true, 9, 175.1110f, 8)]
        [InlineData("+98.7abcdefg", true, 0, 98.7f, 5)]
        [InlineData("A small float is -0.10000000001", true, 17, -0.10000000001f, 14)]
        [InlineData("1.45e12", true, 0, 1.45e12f, 7)]
        [InlineData("1E-8", true, 0, 1e-8f, 4)]
        [InlineData("-3.402822E+38", true, 0, -3.402822E+38f, 13)] // min value
        [InlineData("3.402822E+38", true, 0, 3.402822E+38f, 12)] // max value
        [InlineData("Infinity", true, 0, float.PositiveInfinity, 8)]
        [InlineData("-Infinity", true, 0, float.NegativeInfinity, 9)]
        [InlineData("NaN", true, 0, float.NaN, 3)]
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("1.6540654e100000", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToFloat(string text, bool expectSuccess, int index, float expectedValue, int expectedBytesConsumed)
        {
            float parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion

		#region double

		[Theory]
        [InlineData(".1728", true, 0, 0.1728, 5)]
        [InlineData("blahblahh175.1110", true, 9, 175.1110, 8)]
        [InlineData("+98.7abcdefg", true, 0, 98.7, 5)]
        [InlineData("A small float is -0.10000000001", true, 17, -0.10000000001, 14)]
        [InlineData("1.45e12", true, 0, 1.45e12, 7)]
        [InlineData("1E-8", true, 0, 1e-8, 4)]
        [InlineData("-1.79769313486231E+308", true, 0, -1.79769313486231E+308, 22)] // min value
        [InlineData("1.79769313486231E+308", true, 0, 1.79769313486231E+308, 21)] // max value
        [InlineData("Infinity", true, 0, double.PositiveInfinity, 8)]
        [InlineData("-Infinity", true, 0, double.NegativeInfinity, 9)]
        [InlineData("NaN", true, 0, double.NaN, 3)]
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("1.6540654e100000", false, 0, 0, 0)] // overflow test
        public void ParseUtf8ByteArrayToDouble(string text, bool expectSuccess, int index, double expectedValue, int expectedBytesConsumed)
        {
            double parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

		[Theory]
        [InlineData(".1728", true, 0, 0.1728, 5)]
        [InlineData("blahblahh175.1110", true, 9, 175.1110, 8)]
        [InlineData("+98.7abcdefg", true, 0, 98.7, 5)]
        [InlineData("A small float is -0.10000000001", true, 17, -0.10000000001, 14)]
        [InlineData("1.45e12", true, 0, 1.45e12, 7)]
        [InlineData("1E-8", true, 0, 1e-8, 4)]
        [InlineData("-1.79769313486231E+308", true, 0, -1.79769313486231E+308, 22)] // min value
        [InlineData("1.79769313486231E+308", true, 0, 1.79769313486231E+308, 21)] // max value
        [InlineData("Infinity", true, 0, double.PositiveInfinity, 8)]
        [InlineData("-Infinity", true, 0, double.NegativeInfinity, 9)]
        [InlineData("NaN", true, 0, double.NaN, 3)]
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData("1.6540654e100000", false, 0, 0, 0)] // overflow test
        public unsafe void ParseUtf8ByteStarToDouble(string text, bool expectSuccess, int index, double expectedValue, int expectedBytesConsumed)
        {
            double parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

		#endregion
        
		#region bool

        [Theory]
        [InlineData("blahblahhTrue", true, 9, true, 4)]
        [InlineData("trueacndasjfh", true, 0, true, 4)]
        [InlineData("LetthemFALSEeatcake", true, 7, false, 5)]
        [InlineData("false", true, 0, false, 5)]
        [InlineData("FaLsE", true, 0, false, 5)]
        [InlineData("0", true, 0, false, 1)]
        [InlineData("1", true, 0, true, 1)]
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        public void ParseUtf8ByteArrayToBool(string text, bool expectSuccess, int index, bool expectedValue, int expectedBytesConsumed)
        {
            bool parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');
            bool result = InvariantParser.TryParse(UtfEncode(text), index, fd, nf, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("blahblahhTrue", true, 9, true, 4)]
        [InlineData("trueacndasjfh", true, 0, true, 4)]
        [InlineData("LetthemFALSEeatcake", true, 7, false, 5)]
        [InlineData("false", true, 0, false, 5)]
        [InlineData("FaLsE", true, 0, false, 5)]
        [InlineData("0", true, 0, false, 1)]
        [InlineData("1", true, 0, true, 1)]
        [InlineData("-A", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        public unsafe void ParseUtf8ByteStarToBool(string text, bool expectSuccess, int index, bool expectedValue, int expectedBytesConsumed)
        {
            bool parsedValue;
            int bytesConsumed;
            FormattingData fd = FormattingData.InvariantUtf8;
            Format.Parsed nf = new Format.Parsed('N');

            byte[] textBytes = UtfEncode(text);
            fixed (byte* arrayPointer = textBytes)
            {
                bool result = InvariantParser.TryParse(arrayPointer, index, textBytes.Length, fd, nf,
                    out parsedValue, out bytesConsumed);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedBytesConsumed, bytesConsumed);
            }
        }

        #endregion
	}
}