using System.Text.Parsing;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            0, 987, 3)]
        public void ParseUtf8ByteArrayToUlong(byte[] text, int index, uint expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.True(result);
            Assert.Equal(parsedValue, expectedValue);
            Assert.Equal(bytesConsumed, expectedBytesConsumed);
        }
    }
}
