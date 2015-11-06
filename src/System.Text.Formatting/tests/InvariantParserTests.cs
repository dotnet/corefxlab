using System.Text.Parsing;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class InvariantParserTests
    {
        [Fact]
        public unsafe void ParseUtf8UInt32()
        {
            byte[] textBuffer = Encoding.UTF8.GetBytes("1258");
            fixed(byte * ptextBuffer = textBuffer) { 
                uint value;
                int bytesConsumed;
                var text = new Span<byte>(ptextBuffer, textBuffer.Length);
                Assert.True(InvariantParser.TryParse(text, FormattingData.Encoding.Utf8, out value, out bytesConsumed));
                Assert.Equal(1258U, value);
                Assert.Equal(textBuffer.Length, bytesConsumed); 
            }
        }
    }
}
