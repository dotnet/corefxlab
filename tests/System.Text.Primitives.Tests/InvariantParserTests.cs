using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Formatting.Tests
{
    // TODO: add negative tests (not a number, overflow, etc.)
    public class InvariantParserTests
    {
        [Theory]
        [InlineData("0", 0, 1, 0, 1)]
        [InlineData("00", 0, 2, 0, 1)] // TODO: is this what we want?
        [InlineData("01", 0, 2, 0, 1)] // TODO: is this what we want?
        [InlineData("10", 0, 2, 10, 2)]
        [InlineData("1234567890", 0, 10, 1234567890, 10)]
        [InlineData("1234567890_", 0, 11, 1234567890, 10)]
        [InlineData("1234567890", 1, 9, 234567890, 9)]
        [InlineData("1234567890_", 1, 10, 234567890, 9)]
        public unsafe void ParseSubstringToUInt32(string text, int index, int count, uint expectedValue, int expectedConsumed)
        {
            uint parsedValue;
            int charsConsumed;
            bool result = InvariantParser.TryParse(text, index, count, out parsedValue, out charsConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, charsConsumed);
        }

        [Theory]
        [InlineData("0", 0, 1)]
        [InlineData("00", 0, 1)] // TODO: is this what we want?
        [InlineData("01", 0, 1)] // TODO: is this what we want?
        [InlineData("10", 10, 2)]
        [InlineData("1234567890",1234567890, 10)]
        [InlineData("1234567890_", 1234567890, 10)]
        public unsafe void ParseSpanOfCharToUInt32(string text, uint expectedValue, int expectedConsumed)
        {
            var span = new ReadOnlySpan<char>(text.ToCharArray());
            uint parsedValue;
            int charsConsumed;
            bool result = InvariantParser.TryParse(span, out parsedValue, out charsConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, charsConsumed);
        }

        [Theory]
        [InlineData("0", 0, 1)]
        [InlineData("00", 0, 2)] // TODO: is this what we want?
        [InlineData("01", 1, 2)] // TODO: is this what we want?
        [InlineData("10", 10, 2)]
        [InlineData("1234567890", 1234567890, 10)]
        [InlineData("1234567890_", 1234567890, 10)]
        public unsafe void ParseUtf8StringToUInt32(string text, uint expectedValue, int expectedConsumed)
        {
            var utf8 = new Utf8String(text);

            uint parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(utf8, out parsedValue, out bytesConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData("0", 0, 1)]
        [InlineData("00", 0, 1)]
        [InlineData("01", 0, 1)]
        [InlineData("10", 10, 2)]
        [InlineData("1234567890", 1234567890, 10)]
        [InlineData("1234567890_", 1234567890, 10)]
        public unsafe void ParseUtf8SpanOfBytesToUInt32(string text, uint expectedValue, int expectedConsumed)
        {
            byte[] textBuffer = Encoding.UTF8.GetBytes(text);
            var span = new ReadOnlySpan<byte>(textBuffer);

            uint parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(span, FormattingData.Encoding.Utf8, out parsedValue, out bytesConsumed);

            Assert.True(result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, bytesConsumed); 
        }
    }
}
