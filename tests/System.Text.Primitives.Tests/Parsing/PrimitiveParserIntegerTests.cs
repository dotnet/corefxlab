// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {

        #region byte

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("49abhced", true, 49, 2)]
        [InlineData("0", true, 0, 1)] // min value
        [InlineData("255", true, 255, 3)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("256", false, 0, 0)] // overflow test
        public unsafe void ParseByteDec(string text, bool expectSuccess, byte expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseByte(utf8Span, out byte parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseByte(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("af", true, 0xaf, 2)]
        [InlineData("7ghijzl", true, 0x7, 1)]
        [InlineData("0", true, 0x0, 1)] // min value
        [InlineData("FF", true, 0xFF, 2)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("100", false, 0, 0)] // overflow test
        public unsafe void ParseByteHex(string text, bool expectSuccess, Byte expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseByte(utf8Span, out byte parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseByte(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region ushort

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("4922abhced", true, 4922, 4)]
        [InlineData("0", true, 0, 1)] // min value
        [InlineData("65535", true, 65535, 5)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("65536", false, 0, 0)] // overflow test
        public unsafe void ParseUInt16Dec(string text, bool expectSuccess, ushort expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt16(utf8Span, out ushort parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt16(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseUInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseUInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("af", true, 0xaf, 2)]
        [InlineData("7F3ghijzl", true, 0x7F3, 3)]
        [InlineData("0", true, 0x0, 1)] // min value
        [InlineData("FFFF", true, 0xFFFF, 4)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("10000", false, 0, 0)] // overflow test
        public unsafe void ParseUInt16Hex(string text, bool expectSuccess, UInt16 expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt16(utf8Span, out ushort parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt16(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseUInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseUInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region uint

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("492206507abhced", true, 492206507, 9)]
        [InlineData("0", true, 0, 1)] // min value
        [InlineData("4294967295", true, 4294967295, 10)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("4294967296", false, 0, 0)] // overflow test
        public unsafe void ParseUInt32Dec(string text, bool expectSuccess, uint expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt32(utf8Span, out uint parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt32(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseUInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseUInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("af", true, 0xaf, 2)]
        [InlineData("7F34098ghijzl", true, 0x7F34098, 7)]
        [InlineData("0", true, 0x0, 1)] // min value
        [InlineData("FFFFFFFF", true, 0xFFFFFFFF, 8)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("100000000", false, 0, 0)] // overflow test
        public unsafe void ParseUInt32Hex(string text, bool expectSuccess, UInt32 expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt32(utf8Span, out uint parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt32(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseUInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseUInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region ulong

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("4922065075844043901abhced", true, 4922065075844043901, 19)]
        [InlineData("0", true, 0, 1)] // min value
        [InlineData("18446744073709551615", true, 18446744073709551615, 20)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("18446744073709551616", false, 0, 0)] // overflow test
        public unsafe void ParseUInt64Dec(string text, bool expectSuccess, ulong expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt64(utf8Span, out ulong parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt64(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseUInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseUInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("af", true, 0xaf, 2)]
        [InlineData("7F340980C8C6717ghijzl", true, 0x7F340980C8C6717, 15)]
        [InlineData("0", true, 0x0, 1)] // min value
        [InlineData("FFFFFFFFFFFFFFFF", true, 0xFFFFFFFFFFFFFFFF, 16)] // max value
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("10000000000000000", false, 0, 0)] // overflow test
        public unsafe void ParseUInt64Hex(string text, bool expectSuccess, UInt64 expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseUInt64(utf8Span, out ulong parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseUInt64(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseUInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseUInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region sbyte

        [Theory]
        [InlineData("a1", false, 0, 0)]
        [InlineData("1", true, 1, 1)]
        [InlineData("-1", true, -1, 2)]
        [InlineData("11", true, 11, 2)]
        [InlineData("-11", true, -11, 3)]
        [InlineData("00a0", true, 0, 2)]
        [InlineData("00a", true, 0, 2)]
        [InlineData("111", true, 111, 3)]
        [InlineData("123abcdefg", true, 123, 3)]
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData("123!", true, 123, 3)] // invalid character test w/ char < '0' // TODO: Fix test case elsewhere
        [InlineData("128", false, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0)] // negative overflow test
        [InlineData("0", true, 0, 1)]
        [InlineData("+1", true, 1, 2)]
        [InlineData("+127", true, 127, 4)]
        [InlineData("as3gf31t`2c", false, 0, 0)]
        [InlineData("agbagbagb5", false, 0, 0)]
        [InlineData("1faag", true, 1, 1)]
        [InlineData("-1sdg", true, -1, 2)]
        [InlineData("-afsagsag4", false, 0, 0)]
        [InlineData("+a", false, 0, 0)]
        [InlineData("-0000123abcdefg1", true, -123, 8)]
        [InlineData("+0000123abcdefg1", true, 123, 8)]
        [InlineData("0000123abcdefg1", true, 123, 7)]
        [InlineData("00000012abcdefg1", true, 12, 8)]
        [InlineData("00000000000000abcdefghijklmnop", true, 0, 14)]
        [InlineData("000000a", true, 0, 6)]
        [InlineData("00000000000000!", true, 0, 14)]
        [InlineData("00000000000000", true, 0, 14)]
        [InlineData("114", true, 114, 3)]
        [InlineData("-115", true, -115, 4)]
        [InlineData("123!6", true, 123, 3)]
        [InlineData("123!abc", true, 123, 3)]
        [InlineData("!!", false, 0, 0)]
        [InlineData("+", false, 0, 0)]
        [InlineData("-", false, 0, 0)]
        [InlineData("", false, 0, 0)]
        [InlineData("5", true, 5, 1)]
        [InlineData("^", false, 0, 0)]
        [InlineData("41474836482145", false, 0, 0)]
        [InlineData("0127", true, 127, 4)] // max
        [InlineData("-0128", true, -128, 5)] // min
        [InlineData("0128", false, 0, 0)] // positive overflow test
        [InlineData("-0129", false, 0, 0)] // negative overflow test
        [InlineData("49abcdefg", true, 49, 2)]
        [InlineData("127", true, 127, 3)] // max
        [InlineData("-128", true, -128, 4)] // min
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        public unsafe void ParseSByteDec(string text, bool expectSuccess, sbyte expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseSByte(utf8Span, out sbyte parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseSByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseSByte(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseSByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseSByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("๑๑๑", true, 0, 111, 9)]
        [InlineData("เรื่องเหลวไหล๒๗", true, 39, 27, 6)]
        [InlineData("๕๖กขฃคฅฆง", true, 0, 56, 6)]
        [InlineData("ที่เล็กที่สุดของประเภทนี้คือลบ๑๒๘.", true, 84, -128, 15)]
        [InlineData("ปล่อยให้พวกเขา ลบ๒๘ กินเค้ก", true, 43, -28, 12)]
        [InlineData("๑๒๗", true, 0, 127, 9)] // max
        [InlineData("ลบ๑๒๘", true, 0, -128, 15)] // min
        [InlineData("ลบA", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am ๑", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("ลป๑", false, 0, 0, 0)] //
        public unsafe void ParseSByteThai(string text, bool expectSuccess, int index, sbyte expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result;

            result = CustomParser.TryParseSByte(utf8Span.Slice(index), out sbyte parsedValue, out int consumed, 'G', TestHelper.ThaiTable);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("1f", true, 0x1f, 2)]
        [InlineData("7ghijzl", true, 0x7, 1)]
        [InlineData("7F", true, 0x7F, 2)] // positive max
        [InlineData("80", true, -0x80, 2)] // negative min
        [InlineData("-G", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("100", false, 0, 0)] // overflow test
        public unsafe void ParseSByteHex(string text, bool expectSuccess, sbyte expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseSByte(utf8Span, out sbyte parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseSByte(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseSByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseSByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region short

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("4922abcdefg", true, 4922, 4)]
        [InlineData("a1", false, 0, 0)]
        [InlineData("1", true, 1, 1)]
        [InlineData("-1", true, -1, 2)]
        [InlineData("11", true, 11, 2)]
        [InlineData("-11", true, -11, 3)]
        [InlineData("00a0", true, 0, 2)]
        [InlineData("00a", true, 0, 2)]
        [InlineData("29220abcdefg", true, 29220, 5)]
        [InlineData("0", true, 0, 1)]
        [InlineData("+1", true, 1, 2)]
        [InlineData("+32767", true, 32767, 6)]
        [InlineData("as3gf31t`2c", false, 0, 0)]
        [InlineData("agbagbagb5", false, 0, 0)]
        [InlineData("1faag", true, 1, 1)]
        [InlineData("-1sdg", true, -1, 2)]
        [InlineData("-afsagsag4", false, 0, 0)]
        [InlineData("+a", false, 0, 0)]
        [InlineData("+", false, 0, 0)]
        [InlineData("-", false, 0, 0)]
        [InlineData("-000012345abcdefg1", true, -12345, 10)]
        [InlineData("+000012345abcdefg1", true, 12345, 10)]
        [InlineData("000012345abcdefg1", true, 12345, 9)]
        [InlineData("00000000000000abcdefghijklmnop", true, 0, 14)]
        [InlineData("000000a", true, 0, 6)]
        [InlineData("00000000000000!", true, 0, 14)]
        [InlineData("00000000000000", true, 0, 14)]
        [InlineData("11423", true, 11423, 5)]
        [InlineData("-11423", true, -11423, 6)]
        [InlineData("12345!6", true, 12345, 5)]
        [InlineData("12345!abc", true, 12345, 5)]
        [InlineData("!!", false, 0, 0)]
        [InlineData("", false, 0, 0)]
        [InlineData("5", true, 5, 1)]
        [InlineData("^", false, 0, 0)]
        [InlineData("32767", true, 32767, 5)] // max
        [InlineData("-32768", true, -32768, 6)] // min
        [InlineData("032767", true, 32767, 6)] // max with 0
        [InlineData("-032768", true, -32768, 7)] // min with 0
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0)] // negative overflow test
        [InlineData("032768", false, 0, 0)] // positive overflow test with 0
        [InlineData("-032769", false, 0, 0)] // negative overflow test with 0
        public unsafe void ParseInt16Dec(string text, bool expectSuccess, short expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt16(utf8Span, out short parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt16(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("๑๑๑", true, 0, 111, 9)]
        [InlineData("เรื่องเหลวไหล๒๗", true, 39, 27, 6)]
        [InlineData("๕๖กขฃคฅฆง", true, 0, 56, 6)]
        [InlineData("ที่เล็กที่สุดของประเภทนี้คือลบ๑๒๘.", true, 84, -128, 15)]
        [InlineData("ปล่อยให้พวกเขา ลบ๒๘ กินเค้ก", true, 43, -28, 12)]
        [InlineData("๑๒๗", true, 0, 127, 9)] // max
        [InlineData("ลบ๑๒๘", true, 0, -128, 15)] // min
        [InlineData("ลบA", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am ๑", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("ลป๑", false, 0, 0, 0)] //
        public unsafe void ParseInt16Thai(string text, bool expectSuccess, int index, short expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result;

            result = CustomParser.TryParseInt16(utf8Span.Slice(index), out short parsedValue, out int consumed, 'G', TestHelper.ThaiTable);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("1f", true, 0x1f, 2)]
        [InlineData("7F3ghijzl", true, 0x7F3, 3)]
        [InlineData("7FFF", true, 0x7FFF, 4)] // positive max
        [InlineData("8000", true, -0x8000, 4)] // negative min
        [InlineData("-G", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("10000", false, 0, 0)] // overflow test
        public unsafe void ParseInt16Hex(string text, bool expectSuccess, short expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt16(utf8Span, out short parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt16(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region int

        [Theory]
        [InlineData("a1", false, 0, 0)]
        [InlineData("1", true, 1, 1)]
        [InlineData("-1", true, -1, 2)]
        [InlineData("11", true, 11, 2)]
        [InlineData("-11", true, -11, 3)]
        [InlineData("00a0", true, 0, 2)]
        [InlineData("00a", true, 0, 2)]
        [InlineData("111", true, 111, 3)]
        [InlineData("492206507abcdefg", true, 492206507, 9)]
        [InlineData("2147483647", true, 2147483647, 10)] // max
        [InlineData("-2147483648", true, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData("123!", true, 123, 3)] // invalid character test w/ char < '0' // TODO: Fix test case elsewhere
        [InlineData("2147483648", false, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0)] // negative overflow test
        [InlineData("0", true, 0, 1)]
        [InlineData("+1", true, 1, 2)]
        [InlineData("+2147483647", true, 2147483647, 11)]
        [InlineData("as3gf31t`2c", false, 0, 0)]
        [InlineData("agbagbagb5", false, 0, 0)]
        [InlineData("1faag", true, 1, 1)]
        [InlineData("-1sdg", true, -1, 2)]
        [InlineData("-afsagsag4", false, 0, 0)]
        [InlineData("+a", false, 0, 0)]
        [InlineData("-000012345abcdefg1", true, -12345, 10)]
        [InlineData("+000012345abcdefg1", true, 12345, 10)]
        [InlineData("000012345abcdefg1", true, 12345, 9)]
        [InlineData("0000001234145abcdefg1", true, 1234145, 13)]
        [InlineData("00000000000000abcdefghijklmnop", true, 0, 14)]
        [InlineData("000000a", true, 0, 6)]
        [InlineData("00000000000000!", true, 0, 14)]
        [InlineData("00000000000000", true, 0, 14)]
        [InlineData("1147483648", true, 1147483648, 10)]
        [InlineData("-1147483649", true, -1147483649, 11)]
        [InlineData("12345!6", true, 12345, 5)]
        [InlineData("12345!abc", true, 12345, 5)]
        [InlineData("!!", false, 0, 0)]
        [InlineData("+", false, 0, 0)]
        [InlineData("-", false, 0, 0)]
        [InlineData("", false, 0, 0)]
        [InlineData("5", true, 5, 1)]
        [InlineData("^", false, 0, 0)]
        [InlineData("41474836482145", false, 0, 0)]
        [InlineData("02147483647", true, 2147483647, 11)] // max
        [InlineData("-02147483648", true, -2147483648, 12)] // min
        [InlineData("02147483648", false, 0, 0)] // positive overflow test
        [InlineData("-02147483649", false, 0, 0)] // negative overflow test
        public unsafe void ParseInt32Dec(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt32(utf8Span, out int parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt32(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("2", true, 2, 1)]
        [InlineData("21", true, 21, 2)]
        [InlineData("214", true, 214, 3)]
        [InlineData("2147", true, 2147, 4)]
        [InlineData("21474", true, 21474, 5)]
        [InlineData("214748", true, 214748, 6)]
        [InlineData("2147483", true, 2147483, 7)]
        [InlineData("21474836", true, 21474836, 8)]
        [InlineData("214748364", true, 214748364, 9)]
        [InlineData("2147483647", true, 2147483647, 10)]
        [InlineData("+2", true, 2, 2)]
        [InlineData("+21", true, 21, 3)]
        [InlineData("+214", true, 214, 4)]
        [InlineData("+2147", true, 2147, 5)]
        [InlineData("+21474", true, 21474, 6)]
        [InlineData("+214748", true, 214748, 7)]
        [InlineData("+2147483", true, 2147483, 8)]
        [InlineData("+21474836", true, 21474836, 9)]
        [InlineData("+214748364", true, 214748364, 10)]
        [InlineData("+2147483647", true, 2147483647, 11)]
        [InlineData("-2", true, -2, 2)]
        [InlineData("-21", true, -21, 3)]
        [InlineData("-214", true, -214, 4)]
        [InlineData("-2147", true, -2147, 5)]
        [InlineData("-21474", true, -21474, 6)]
        [InlineData("-214748", true, -214748, 7)]
        [InlineData("-2147483", true, -2147483, 8)]
        [InlineData("-21474836", true, -21474836, 9)]
        [InlineData("-214748364", true, -214748364, 10)]
        [InlineData("-2147483647", true, -2147483647, 11)]
        private void ParseInt32VariableLength(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result = Utf8Parser.TryParse(utf8Span, out int parsedValue, out int consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("3147483647")]
        [InlineData("4147483647")]
        [InlineData("5147483647")]
        [InlineData("6147483647")]
        [InlineData("7147483647")]
        [InlineData("8147483647")]
        [InlineData("9147483647")]
        [InlineData("2147483648")]
        [InlineData("3147483648")]
        [InlineData("4147483648")]
        [InlineData("5147483648")]
        [InlineData("6147483648")]
        [InlineData("7147483648")]
        [InlineData("8147483648")]
        [InlineData("9147483648")]
        [InlineData("11474836471")]
        [InlineData("21474836471")]
        [InlineData("31474836471")]
        [InlineData("41474836471")]
        [InlineData("51474836471")]
        [InlineData("61474836471")]
        [InlineData("71474836471")]
        [InlineData("81474836471")]
        [InlineData("91474836471")]
        [InlineData("11474836481")]
        [InlineData("21474836481")]
        [InlineData("31474836481")]
        [InlineData("41474836481")]
        [InlineData("51474836481")]
        [InlineData("61474836481")]
        [InlineData("71474836481")]
        [InlineData("81474836481")]
        [InlineData("91474836481")]
        private void ParseInt32VariableOverflowTests(string text)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result = Utf8Parser.TryParse(utf8Span, out int parsedValue, out int consumed);
            Assert.Equal(false, result);
            Assert.Equal(0, parsedValue);
            Assert.Equal(0, consumed);
        }

        //[Theory] - This test is far too slow (turns a 0.5-second test run into a 2.33 minute test run!) and tests a very unlikely scenario. Save for outerloop.
        [InlineData("0", true, 0, int.MaxValue)]
        [InlineData("2", true, 2, int.MaxValue)]
        [InlineData("21", true, 21, int.MaxValue)]
        [InlineData("+2", true, 2, int.MaxValue)]
        [InlineData("-2", true, -2, int.MaxValue)]
        [InlineData("2147483647", true, 2147483647, int.MaxValue)] // max
        [InlineData("-2147483648", true, -2147483648, int.MaxValue)] // min
        [InlineData("2147483648", false, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0)] // negative overflow test
        [InlineData("12345abcdefg1", true, 12345, int.MaxValue - 8)]
        [InlineData("1234145abcdefg1", true, 1234145, int.MaxValue - 8)]
        [InlineData("abcdefghijklmnop1", true, 0, int.MaxValue - 17)]
        [InlineData("1147483648", true, 1147483648, int.MaxValue)]
        [InlineData("-1147483649", true, -1147483649, int.MaxValue)]
        public unsafe void ParseInt32OverflowCheck(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);

            const int TwoGiB = int.MaxValue;

            unsafe
            {
                if (!TestHelper.TryAllocNative((IntPtr)TwoGiB, out IntPtr memBlock))
                    return; // It's not implausible to believe that a 2gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.

                try
                {
                    ref byte memory = ref Unsafe.AsRef<byte>(memBlock.ToPointer());
                    var span = new Span<byte>(memBlock.ToPointer(), TwoGiB);
                    span.Fill(48);

                    byte sign = utf8Span[0];
                    if (sign == '-' || sign == '+')
                    {
                        span[0] = sign;
                        utf8Span = utf8Span.Slice(1);
                    }
                    utf8Span.CopyTo(span.Slice(TwoGiB - utf8Span.Length));

                    bool result = Utf8Parser.TryParse(span, out int parsedValue, out int consumed);
                    Assert.Equal(expectSuccess, result);
                    Assert.Equal(expectedValue, parsedValue);
                    Assert.Equal(expectedConsumed, consumed);
                }
                finally
                {
                    TestHelper.ReleaseNative(ref memBlock);
                }
            }
        }

        [Theory]
        [InlineData("๑๑๑", true, 111, 9)]
        [InlineData("๕๖กขฃคฅฆง", true, 56, 6)]
        [InlineData("๑๒๗", true, 127, 9)] // max
        [InlineData("ลบ๑๒๘", true, -128, 15)] // min
        [InlineData("ลบA", false, 0, 0)] // invalid character after a sign
        [InlineData("I am ๑", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("ลป๑", false, 0, 0)]
        [InlineData("๑๐๗๓๗๔๑๘๒", true, 107374182, 9 * 3)]
        [InlineData("๒๑๔๗๔๘๓๖๔๗", true, 2147483647, 10 * 3)]
        [InlineData("๐๒๑๔๗๔๘๓๖๔๗", true, 2147483647, 11 * 3)]
        [InlineData("๐", true, 0, 1 * 3)]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๘", true, -2147483648, 10 * 3 + 6)]
        [InlineData("ลบ๐๒๑๔๗๔๘๓๖๔๘", true, -2147483648, 11 * 3 + 6)]
        [InlineData("๒๑๔๗๔๘๓๖๔", true, 214748364, 9 * 3)]
        [InlineData("๒", true, 2, 1 * 3)]
        [InlineData("๒๑๔๗๔๘๓๖", true, 21474836, 8 * 3)]
        [InlineData("ลบ๒๑๔๗๔", true, -21474, 5 * 3 + 6)]
        [InlineData("๒๑๔๗๔", true, 21474, 5 * 3)]
        [InlineData("ลบ๒๑", true, -21, 2 * 3 + 6)]
        [InlineData("ลบ๒", true, -2, 1 * 3 + 6)]
        [InlineData("๒๑๔", true, 214, 3 * 3)]
        [InlineData("ลบ๒๑๔๗๔๘๓๖", true, -21474836, 8 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔", true, -214748364, 9 * 3 + 6)]
        [InlineData("๒๑๔๗", true, 2147, 4 * 3)]
        [InlineData("ลบ๒๑๔๗", true, -2147, 4 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘", true, -214748, 6 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘๓", true, -2147483, 7 * 3 + 6)]
        [InlineData("๒๑๔๗๔๘", true, 214748, 6 * 3)]
        [InlineData("๒๑", true, 21, 2 * 3)]
        [InlineData("๒๑๔๗๔๘๓", true, 2147483, 7 * 3)]
        [InlineData("ลบ๒๑๔", true, -214, 3 * 3 + 6)]
        [InlineData("+๒๑๔๗๔", true, 21474, 5 * 3 + 1)]
        [InlineData("+๒๑", true, 21, 2 * 3 + 1)]
        [InlineData("+๒", true, 2, 1 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖", true, 21474836, 8 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖๔", true, 214748364, 9 * 3 + 1)]
        [InlineData("+๒๑๔๗", true, 2147, 4 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘", true, 214748, 6 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓", true, 2147483, 7 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗", true, 2147483647, 10 * 3 + 1)]
        [InlineData("+๒๑๔", true, 214, 3 * 3 + 1)]
        [InlineData("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg", true, 1235, 24 * 3)]
        [InlineData("๒๑๔๗๔๘๓๖๔abcdefghijklmnop", true, 214748364, 9 * 3)]
        [InlineData("๒abcdefghijklmnop", true, 2, 1 * 3)]
        [InlineData("๒๑๔๗๔๘๓๖abcdefghijklmnop", true, 21474836, 8 * 3)]
        [InlineData("ลบ๒๑๔๗๔abcdefghijklmnop", true, -21474, 5 * 3 + 6)]
        [InlineData("๒๑๔๗๔abcdefghijklmnop", true, 21474, 5 * 3)]
        [InlineData("ลบ๒๑abcdefghijklmnop", true, -21, 2 * 3 + 6)]
        [InlineData("ลบ๒abcdefghijklmnop", true, -2, 1 * 3 + 6)]
        [InlineData("๒๑๔abcdefghijklmnop", true, 214, 3 * 3)]
        [InlineData("ลบ๒๑๔๗๔๘๓๖abcdefghijklmnop", true, -21474836, 8 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔abcdefghijklmnop", true, -214748364, 9 * 3 + 6)]
        [InlineData("๒๑๔๗abcdefghijklmnop", true, 2147, 4 * 3)]
        [InlineData("ลบ๒๑๔๗abcdefghijklmnop", true, -2147, 4 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘abcdefghijklmnop", true, -214748, 6 * 3 + 6)]
        [InlineData("ลบ๒๑๔๗๔๘๓abcdefghijklmnop", true, -2147483, 7 * 3 + 6)]
        [InlineData("๒๑๔๗๔๘abcdefghijklmnop", true, 214748, 6 * 3)]
        [InlineData("๒๑abcdefghijklmnop", true, 21, 2 * 3)]
        [InlineData("๒๑๔๗๔๘๓abcdefghijklmnop", true, 2147483, 7 * 3)]
        [InlineData("ลบ๒๑๔abcdefghijklmnop", true, -214, 3 * 3 + 6)]
        [InlineData("+๒๑๔๗๔abcdefghijklmnop", true, 21474, 5 * 3 + 1)]
        [InlineData("+๒๑abcdefghijklmnop", true, 21, 2 * 3 + 1)]
        [InlineData("+๒abcdefghijklmnop", true, 2, 1 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖abcdefghijklmnop", true, 21474836, 8 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖๔abcdefghijklmnop", true, 214748364, 9 * 3 + 1)]
        [InlineData("+๒๑๔๗abcdefghijklmnop", true, 2147, 4 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘abcdefghijklmnop", true, 214748, 6 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓abcdefghijklmnop", true, 2147483, 7 * 3 + 1)]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗abcdefghijklmnop", true, 2147483647, 10 * 3 + 1)]
        [InlineData("+๒๑๔abcdefghijklmnop", true, 214, 3 * 3 + 1)]
        [InlineData("๐๐a๐", true, 0, 6)]
        [InlineData("๐๐a", true, 0, 6)]
        [InlineData("", false, 0, 0)]
        [InlineData("+", false, 0, 0)]
        [InlineData("ลบ", false, 0, 0)]
        public unsafe void ParseInt32Thai(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result = CustomParser.TryParseInt32(utf8Span, out int parsedValue, out int consumed, 'G', TestHelper.ThaiTable);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        //[Theory] // TODO: Test is too slow, only enable for "outerloop"
        [InlineData("๐", true, 0, int.MaxValue)]
        [InlineData("๒", true, 2, int.MaxValue)]
        [InlineData("๒๑", true, 21, int.MaxValue)]
        [InlineData("+๒", true, 2, int.MaxValue)]
        [InlineData("ลบ๒", true, -2, int.MaxValue)]
        [InlineData("๒๑๔๗๔๘๓๖๔๗", true, 2147483647, int.MaxValue)] // max
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๘", true, -2147483648, int.MaxValue)] // min
        [InlineData("๒๑๔๗๔๘๓๖๔๘", false, 0, 0)] // positive overflow test
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๙", false, 0, 0)] // negative overflow test
        [InlineData("๑๒๓๔๕abcdefg๑", true, 12345, int.MaxValue - 8)]
        [InlineData("๑๒๓๔๑๔๕abcdefg๑", true, 1234145, int.MaxValue - 8)]
        [InlineData("abcdefghijklmnop๑", true, 0, int.MaxValue - 17)]
        public unsafe void ParseInt32ThaiOverflowCheck(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);

            const int TwoGiB = int.MaxValue;

            unsafe
            {
                if (!TestHelper.TryAllocNative((IntPtr)TwoGiB, out IntPtr memBlock))
                    return; // It's not implausible to believe that a 2gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.

                try
                {
                    ref byte memory = ref Unsafe.AsRef<byte>(memBlock.ToPointer());
                    var span = new Span<byte>(memBlock.ToPointer(), TwoGiB);
                    for (int i = 0; i < TwoGiB / 3; i++)
                    {
                        span[i * 3] = 0xe0;
                        span[i * 3 + 1] = 0xb9;
                        span[i * 3 + 2] = 0x90;
                    }

                    byte sign = utf8Span[0];
                    Span<byte> minusSpan = new byte[] { sign, 0xb8, 0xa5, 0xe0, 0xb8, 0x9a };
                    if (sign == '+')
                    {
                        span[0] = sign;
                        utf8Span = utf8Span.Slice(1);
                    }
                    else if (span.StartsWith(minusSpan))
                    {
                        utf8Span = utf8Span.Slice(6);
                    }

                    utf8Span.CopyTo(span.Slice(TwoGiB - utf8Span.Length));

                    bool result = CustomParser.TryParseInt32(span, out int parsedValue, out int consumed, 'G', TestHelper.ThaiTable);
                    Assert.Equal(expectSuccess, result);
                    Assert.Equal(expectedValue, parsedValue);
                    Assert.Equal(expectedConsumed, consumed);
                }
                finally
                {
                    TestHelper.ReleaseNative(ref memBlock);
                }
            }
        }

        [Theory]
        [InlineData("1f", true, 0x1f, 2)]
        [InlineData("7F34098ghijzl", true, 0x7F34098, 7)]
        [InlineData("7FFFFFFF", true, 0x7FFFFFFF, 8)] // positive max
        [InlineData("80000000", true, -0x80000000, 8)] // negative min
        [InlineData("-G", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("100000000", false, 0, 0)] // overflow test
        public unsafe void ParseInt32Hex(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt32(utf8Span, out int parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt32(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


        #region long

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("492206507584404390abcdefg", true, 492206507584404390, 18)]
        [InlineData("9223372036854775807", true, 9223372036854775807, 19)] // max
        [InlineData("-9223372036854775808", true, -9223372036854775808, 20)] // min
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("9223372036854775808", false, 0, 0)] // positive overflow test
        [InlineData("-9223372036854775809", false, 0, 0)] // negative overflow test
        public unsafe void ParseInt64Dec(string text, bool expectSuccess, long expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt64(utf8Span, out long parsedValue, out int consumed, 'G', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt64(utf16ByteSpan, out parsedValue, out consumed, 'G', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.TryParseInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.TryParseInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("๑๑๑", true, 0, 111, 9)]
        [InlineData("เรื่องเหลวไหล๒๗", true, 39, 27, 6)]
        [InlineData("๕๖กขฃคฅฆง", true, 0, 56, 6)]
        [InlineData("ที่เล็กที่สุดของประเภทนี้คือลบ๑๒๘.", true, 84, -128, 15)]
        [InlineData("ปล่อยให้พวกเขา ลบ๒๘ กินเค้ก", true, 43, -28, 12)]
        [InlineData("๑๒๗", true, 0, 127, 9)] // max
        [InlineData("ลบ๑๒๘", true, 0, -128, 15)] // min
        [InlineData("ลบA", false, 0, 0, 0)] // invalid character after a sign
        [InlineData("I am ๑", false, 0, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("ลป๑", false, 0, 0, 0)]
        public unsafe void ParseInt64Thai(string text, bool expectSuccess, int index, long expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            bool result;

            result = CustomParser.TryParseInt64(utf8Span.Slice(index), out long parsedValue, out int consumed, 'G', TestHelper.ThaiTable);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        [Theory]
        [InlineData("1f", true, 0x1f, 2)]
        [InlineData("7F340980C8C6717ghijzl", true, 0x7F340980C8C6717, 15)]
        [InlineData("7FFFFFFFFFFFFFFF", true, 0x7FFFFFFFFFFFFFFF, 16)] // positive max
        [InlineData("8000000000000000", true, -0x8000000000000000, 16)] // negative min
        [InlineData("-G", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("10000000000000000", false, 0, 0)] // overflow test
        public unsafe void ParseInt64Hex(string text, bool expectSuccess, long expectedValue, int expectedConsumed)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = TestHelper.UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = MemoryMarshal.Cast<byte, char>(utf16ByteSpan);
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = CustomParser.TryParseInt64(utf8Span, out long parsedValue, out int consumed, 'X', SymbolTable.InvariantUtf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = Utf8Parser.TryParse(utf8Span, out parsedValue, out consumed, 'X');
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = CustomParser.TryParseInt64(utf16ByteSpan, out parsedValue, out consumed, 'X', SymbolTable.InvariantUtf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = Utf16Parser.Hex.TryParseInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = Utf16Parser.Hex.TryParseInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
        }

        #endregion


    }
}
