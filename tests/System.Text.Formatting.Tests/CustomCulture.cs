// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CustomCultureTests
    {
        internal static FormattingData Culture1;
        internal static FormattingData Culture5;
        internal static ArrayPool<byte> pool = ArrayPool<byte>.Shared;

        // Sets up cultures with digits represented by 1 or 5 'A's (0) through 1 or 5 'J's (9) and the minus sigh represented by an underscore followed by a question mark
        static CustomCultureTests()
        {
            byte[][] utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 5);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.MinusSign] = GetBytesUtf16("_?");
            Culture5 = new FormattingData(utf16digitsAndSymbols, FormattingData.Encoding.Utf16);

            utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.MinusSign] = GetBytesUtf16("_?");
            Culture1 = new FormattingData(utf16digitsAndSymbols, FormattingData.Encoding.Utf16);
        }

        private static byte[] GetBytesUtf16(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }

        [Fact]
        public void CustomCulture()
        {
            var sb = new StringFormatter(pool);
            sb.FormattingData = Culture5;

            sb.Append(-1234567890);
            Assert.Equal("_?BBBBBCCCCCDDDDDEEEEEFFFFFGGGGGHHHHHIIIIIJJJJJAAAAA", sb.ToString());
        }
    }
}
