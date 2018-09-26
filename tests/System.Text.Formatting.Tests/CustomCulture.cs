// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CustomCultureTests
    {
        internal static SymbolTable Culture1;
        internal static SymbolTable Culture5;
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
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.MinusSign] = GetBytesUtf16("_?");
            Culture5 = new CustomUtf16SymbolTable(utf16digitsAndSymbols);

            utf16digitsAndSymbols = new byte[17][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)SymbolTable.Symbol.MinusSign] = GetBytesUtf16("_?");
            Culture1 = new CustomUtf16SymbolTable(utf16digitsAndSymbols);
        }

        private static byte[] GetBytesUtf16(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }

        [Fact]
        public void CustomCulture()
        {
            var sb = new StringFormatter();
            sb.SymbolTable = Culture5;

            sb.Append(-1234567890);
            Assert.Equal("_?BBBBBCCCCCDDDDDEEEEEFFFFFGGGGGHHHHHIIIIIJJJJJAAAAA", sb.ToString());
        }
    }
}
