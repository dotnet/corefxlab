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
        [InlineData("-128", 4, sbyte.MinValue, 4)]
        [InlineData("127", 3, sbyte.MaxValue, 3)]
        public unsafe void SBytePositiveTests(string text, int length, sbyte expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);
            
            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            sbyte actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseSByte(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseSByte(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.TryParseSByte(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.TryParseSByte(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.TryParseSByte(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseSByte(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16);
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseSByte(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.TryParseSByte(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.TryParseSByte(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.TryParseSByte(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
        
        [Theory (Skip = "SByte hex parsing not implemented yet")]
        public unsafe void SBytePositiveHexTests(string text, int length, sbyte expectedValue, int expectedConsumed)
        {
            byte[] byteBuffer = new Utf8String(text).CopyBytes();
            ReadOnlySpan<byte> byteSpan = new ReadOnlySpan<byte>(byteBuffer);

            char[] charBuffer = text.ToCharArray();
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charBuffer);

            bool result;
            sbyte actualValue;
            int actualConsumed;

            result = PrimitiveParser.TryParseSByte(byteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf8, 'X');

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            fixed (byte* bytePointer = byteBuffer)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(bytePointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(bytePointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(byteSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(byteSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);

            ReadOnlySpan<byte> utf16ByteSpan = charSpan.Cast<char, byte>();
            result = PrimitiveParser.TryParseSByte(utf16ByteSpan, out actualValue, out actualConsumed, EncodingData.InvariantUtf16, 'X');
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed / 2);

            fixed (char* charPointer = charBuffer)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(charPointer, length, out actualValue);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(charPointer, length, out actualValue, out actualConsumed);

                Assert.True(result);
                Assert.Equal(expectedValue, actualValue);
                Assert.Equal(expectedConsumed, actualConsumed);
            }

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(charSpan, out actualValue);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(charSpan, out actualValue, out actualConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedConsumed, actualConsumed);
        }
    }
}
