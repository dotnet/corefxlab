﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {
        private byte[] UtfEncode(string s, bool utf16)
        {
            if (utf16)
                return Text.Encoding.Unicode.GetBytes(s);
            else
                return Text.Encoding.UTF8.GetBytes(s);
        }

        static Tuple<byte, int>[] s_utf8ParsingTrie = new Tuple<byte, int>[] {
            Tuple.Create((byte)17, 0x3004390D),
            Tuple.Create((byte)43, 18),
            Tuple.Create((byte)45, 19),
            Tuple.Create((byte)46, 20),
            Tuple.Create((byte)48, 21),
            Tuple.Create((byte)49, 22),
            Tuple.Create((byte)50, 23),
            Tuple.Create((byte)51, 24),
            Tuple.Create((byte)52, 25),
            Tuple.Create((byte)53, 26),
            Tuple.Create((byte)54, 27),
            Tuple.Create((byte)55, 28),
            Tuple.Create((byte)56, 29),
            Tuple.Create((byte)57, 30),
            Tuple.Create((byte)69, 31),
            Tuple.Create((byte)73, 32),
            Tuple.Create((byte)78, 33),
            Tuple.Create((byte)101, 34),
            Tuple.Create((byte)0, 14),
            Tuple.Create((byte)0, 13),
            Tuple.Create((byte)0, 10),
            Tuple.Create((byte)0, 0),
            Tuple.Create((byte)0, 1),
            Tuple.Create((byte)0, 2),
            Tuple.Create((byte)0, 3),
            Tuple.Create((byte)0, 4),
            Tuple.Create((byte)0, 5),
            Tuple.Create((byte)0, 6),
            Tuple.Create((byte)0, 7),
            Tuple.Create((byte)0, 8),
            Tuple.Create((byte)0, 9),
            Tuple.Create((byte)0, 16),
            Tuple.Create((byte)0, 12),
            Tuple.Create((byte)0, 15),
            Tuple.Create((byte)0, 16),
        };

        static Tuple<byte, int>[] s_thais_utf8ParsingTrie = new Tuple<byte, int>[] {
            Tuple.Create((byte)4, 0),
            Tuple.Create((byte)43, 5),
            Tuple.Create((byte)69, 6),
            Tuple.Create((byte)101, 7),
            Tuple.Create((byte)0xE0, 8),
            Tuple.Create((byte)0, 14),
            Tuple.Create((byte)0, 16),
            Tuple.Create((byte)0, 16),
            Tuple.Create((byte)2, 0),
            Tuple.Create((byte)0xB8, 11),
            Tuple.Create((byte)0xB9, 15),
            Tuple.Create((byte)3, 0),
            Tuple.Create((byte)0x88, 27),
            Tuple.Create((byte)0xA5, 28),
            Tuple.Create((byte)0xAA, 29),
            Tuple.Create((byte)11, -1878877941 /* 0x9002990B */),
            Tuple.Create((byte)0x84, 30),
            Tuple.Create((byte)0x90, 31),
            Tuple.Create((byte)0x91, 32),
            Tuple.Create((byte)0x92, 33),
            Tuple.Create((byte)0x93, 34),
            Tuple.Create((byte)0x94, 35),
            Tuple.Create((byte)0x95, 36),
            Tuple.Create((byte)0x96, 37),
            Tuple.Create((byte)0x97, 38),
            Tuple.Create((byte)0x98, 39),
            Tuple.Create((byte)0x99, 40),
            Tuple.Create((byte)0, 10),
            Tuple.Create((byte)0, 13),
            Tuple.Create((byte)0, 12),
            Tuple.Create((byte)0, 15),
            Tuple.Create((byte)0, 0),
            Tuple.Create((byte)0, 1),
            Tuple.Create((byte)0, 2),
            Tuple.Create((byte)0, 3),
            Tuple.Create((byte)0, 4),
            Tuple.Create((byte)0, 5),
            Tuple.Create((byte)0, 6),
            Tuple.Create((byte)0, 7),
            Tuple.Create((byte)0, 8),
            Tuple.Create((byte)0, 9),
        };

        static byte[][] s_utf8digitsAndSymbols = new byte[][] {
            new byte[] { 48, },
            new byte[] { 49, },
            new byte[] { 50, },
            new byte[] { 51, },
            new byte[] { 52, },
            new byte[] { 53, },
            new byte[] { 54, },
            new byte[] { 55, },
            new byte[] { 56, },
            new byte[] { 57, }, // digit 9
            new byte[] { 46, }, // decimal separator
            new byte[] { 44, }, // group separator
            new byte[] { 73, 110, 102, 105, 110, 105, 116, 121, },
            new byte[] { 45, }, // minus sign
            new byte[] { 43, }, // plus sign
            new byte[] { 78, 97, 78, }, // NaN
            new byte[] { 69, }, // E
            new byte[] { 101, }, // e
        };

        static byte[][] s_thaiUtf8DigitsAndSymbols = new byte[][]
        {
            new byte[] { 0xe0, 0xb9, 0x90 }, new byte[] { 0xe0, 0xb9, 0x91 }, new byte[] { 0xe0, 0xb9, 0x92 },
            new byte[] { 0xe0, 0xb9, 0x93 }, new byte[] { 0xe0, 0xb9, 0x94 }, new byte[] { 0xe0, 0xb9, 0x95 }, new byte[] { 0xe0, 0xb9, 0x96 },
            new byte[] { 0xe0, 0xb9, 0x97 }, new byte[] { 0xe0, 0xb9, 0x98 }, new byte[] { 0xe0, 0xb9, 0x99 }, new byte[] { 0xE0, 0xB8, 0x88, 0xE0, 0xB8, 0x94 }, null,
            new byte[] { 0xE0, 0xB8, 0xAA, 0xE0, 0xB8, 0xB4, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x87, 0xE0, 0xB8, 0x97, 0xE0, 0xB8, 0xB5, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x83,
                0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0x8D, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x82, 0xE0, 0xB8, 0x95, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0xA5, 0xE0,
                0xB8, 0xB7, 0xE0, 0xB8, 0xAD, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0x81, 0xE0, 0xB8, 0xB4, 0xE0, 0xB8, 0x99 },
            new byte[] { 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x9A }, new byte[] { 43 }, new byte[] { 0xE0, 0xB9, 0x84, 0xE0, 0xB8, 0xA1, 0xE0, 0xB9, 0x88, 0xE0, 0xB9,
                0x83, 0xE0, 0xB8, 0x8A, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x95, 0xE0, 0xB8, 0xB1, 0xE0, 0xB8, 0xA7, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x82 },
            new byte[] { 69 }, new byte[] { 101 },
        };

        static TextEncoder s_thaiEncoder = TextEncoder.CreateUtf8Encoder(s_thaiUtf8DigitsAndSymbols);

        static TextEncoder s_utf8Encoder = TextEncoder.CreateUtf8Encoder(s_utf8digitsAndSymbols);


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
            byte parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseByte(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseByte(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseByte(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseByte(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseByte(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            byte parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseByte(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseByte(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            ushort parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt16(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseUInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt16(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt16(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            ushort parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt16(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt16(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt16(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            uint parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt32(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseUInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt32(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            uint parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt32(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt32(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            ulong parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt64(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseUInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt64(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseUInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            ulong parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseUInt64(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseUInt64(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseUInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
        }

        #endregion



        #region sbyte

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("49abcdefg", true, 49, 2)]
        [InlineData("127", true, 127, 3)] // max
        [InlineData("-128", true, -128, 4)] // min
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("128", false, 0, 0)] // positive overflow test
        [InlineData("-129", false, 0, 0)] // negative overflow test
        public unsafe void ParseSByteDec(string text, bool expectSuccess, sbyte expectedValue, int expectedConsumed)
        {
            sbyte parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseSByte(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseSByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseSByte(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseSByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseSByte(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseSByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseSByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            sbyte parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            bool result;

            result = PrimitiveParser.TryParseSByte(utf8Span.Slice(index), out parsedValue, out consumed, 'G', s_thaiEncoder);

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
            sbyte parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseSByte(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseSByte(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseSByte(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
        }

        #endregion


        #region short

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("4922abcdefg", true, 4922, 4)]
        [InlineData("32767", true, 32767, 5)] // max
        [InlineData("-32768", true, -32768, 6)] // min
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("32768", false, 0, 0)] // positive overflow test
        [InlineData("-32769", false, 0, 0)] // negative overflow test
        public unsafe void ParseInt16Dec(string text, bool expectSuccess, short expectedValue, int expectedConsumed)
        {
            short parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt16(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseInt16(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt16(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            short parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            bool result;

            result = PrimitiveParser.TryParseInt16(utf8Span.Slice(index), out parsedValue, out consumed, 'G', s_thaiEncoder);

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
            short parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt16(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt16(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt16(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
        }

        #endregion


        #region int

        [Theory]
        [InlineData("111", true, 111, 3)]
        [InlineData("492206507abcdefg", true, 492206507, 9)]
        [InlineData("2147483647", true, 2147483647, 10)] // max
        [InlineData("-2147483648", true, -2147483648, 11)] // min
        [InlineData("-A", false, 0, 0)] // invalid character after a sign
        [InlineData("I am 1", false, 0, 0)] // invalid character test
        [InlineData(" !", false, 0, 0)] // invalid character test w/ char < '0'
        [InlineData("2147483648", false, 0, 0)] // positive overflow test
        [InlineData("-2147483649", false, 0, 0)] // negative overflow test
        public unsafe void ParseInt32Dec(string text, bool expectSuccess, int expectedValue, int expectedConsumed)
        {
            int parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt32(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseInt32(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt32(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
        public unsafe void ParseInt32Thai(string text, bool expectSuccess, int index, int expectedValue, int expectedConsumed)
        {
            int parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            bool result;

            result = PrimitiveParser.TryParseInt32(utf8Span.Slice(index), out parsedValue, out consumed, 'G', s_thaiEncoder);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);
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
            int parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt32(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt32(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt32(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
            long parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt64(utf8Span, out parsedValue, out consumed, 'G', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.TryParseInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.TryParseInt64(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.TryParseInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt64(utf16ByteSpan, out parsedValue, out consumed, 'G', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.TryParseInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.TryParseInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
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
        public unsafe void ParseInt64Thai(string text, bool expectSuccess, int index, long expectedValue, int expectedConsumed)
        {
            long parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            bool result;

            result = PrimitiveParser.TryParseInt64(utf8Span.Slice(index), out parsedValue, out consumed, 'G', s_thaiEncoder);

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
            long parsedValue;
            int consumed;
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            ReadOnlySpan<byte> utf16ByteSpan = UtfEncode(text, true);
            ReadOnlySpan<char> utf16CharSpan = utf16ByteSpan.NonPortableCast<byte, char>();
            byte[] textBytes = utf8Span.ToArray();
            char[] textChars = utf16CharSpan.ToArray();
            bool result;

            result = PrimitiveParser.TryParseInt64(utf8Span, out parsedValue, out consumed, 'X', TextEncoder.Utf8);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt64(utf8Span, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt64(utf8Span, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (byte* arrayPointer = textBytes)
            {
                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue);

                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf8.Hex.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }

            result = PrimitiveParser.TryParseInt64(utf16ByteSpan, out parsedValue, out consumed, 'X', TextEncoder.Utf16);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed * sizeof(char), consumed);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt64(utf16CharSpan, out parsedValue);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);

            result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt64(utf16CharSpan, out parsedValue, out consumed);
            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedConsumed, consumed);

            fixed (char* arrayPointer = textChars)
            {
                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);

                result = PrimitiveParser.InvariantUtf16.Hex.TryParseInt64(arrayPointer, textBytes.Length, out parsedValue, out consumed);
                Assert.Equal(expectSuccess, result);
                Assert.Equal(expectedValue, parsedValue);
                Assert.Equal(expectedConsumed, consumed);
            }
        }

        #endregion


    }
}
