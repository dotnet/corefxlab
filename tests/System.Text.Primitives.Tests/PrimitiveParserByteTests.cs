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
        [InlineData("0", 1, byte.MinValue, 1)]
        [InlineData("255", 3, byte.MaxValue, 3)]
        public unsafe void BytePositiveTests(string text, int length, byte expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);
            
            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            byte actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseByte(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseByte(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.TryParseByte(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.TryParseByte(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.TryParseByte(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseByte(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseByte(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.TryParseByte(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.TryParseByte(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.TryParseByte(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
        
        [Theory (Skip = "Byte hex parsing not implemented yet")]
        public unsafe void BytePositiveHexTests(string text, int length, byte expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            byte actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseByte(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8, 'X');

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseByte(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16, 'X');
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
