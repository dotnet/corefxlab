using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {
        [Theory]
        [InlineData("123", 3, 123, 3)]
        [InlineData("12a", 3, 12, 2)]
        [InlineData("-32768", 6, short.MinValue, 6)]
        [InlineData("32767", 5, short.MaxValue, 5)]
        public unsafe void Int16PositiveTests(string text, int length, short expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            short actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseInt16(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseInt16(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.TryParseInt16(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.TryParseInt16(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.TryParseInt16(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseInt16(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseInt16(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.TryParseInt16(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.TryParseInt16(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.TryParseInt16(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }

        [Theory(Skip = "Int16 hex parsing not implemented yet")]
        public unsafe void Int16PositiveHexTests(string text, int length, short expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            short actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseInt16(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8, 'X');

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseInt16(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16, 'X');
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
