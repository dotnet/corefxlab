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
        [InlineData("-2147483648", 11, int.MinValue, 11)]
        [InlineData("2147483647", 10, int.MaxValue, 10)]
        public unsafe void Int32PositiveTests(string text, int length, int expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);
            
            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            int actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseInt32(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseInt32(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.TryParseInt32(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.TryParseInt32(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.TryParseInt32(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseInt32(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseInt32(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.TryParseInt32(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.TryParseInt32(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.TryParseInt32(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
        
        [Theory (Skip = "Int32 hex parsing not implemented yet")]
        public unsafe void Int32PositiveHexTests(string text, int length, int expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            int actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseInt32(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8, 'X');

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseInt32(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16, 'X');
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
