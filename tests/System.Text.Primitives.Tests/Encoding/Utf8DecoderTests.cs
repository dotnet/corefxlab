// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text.Primitives.System.Text.Encoders;
using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class Utf8DecoderTests
    {
        private static readonly UTF8Encoding _strictEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        [Fact]
        public void IsWellFormedUtf8String_WithSequenceOfAllValidChars_ReturnsOk()
        {
            // Arrange - build up a buffer of all possible valid sequences

            MemoryStream buffer = new MemoryStream();
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                if (!IsSurrogateCodePoint(i))
                {
                    byte[] thisSequence = GetUtf8SequenceForCodePoint(i);
                    buffer.Write(thisSequence, 0, thisSequence.Length);
                }
            }

            // Act & assert

            Assert.True(Utf8Decoder.IsWellFormedUtf8String(buffer.ToArray()));
        }

        [Theory]
        [InlineData(new byte[] { }, -1)] // valid, empty string
        [InlineData(new byte[] { 0b0000_0000 }, -1)] // valid, ASCII character
        [InlineData(new byte[] { 0b1000_0000 }, 0)] // invalid, begins with trailing char
        [InlineData(new byte[] { 0b0000_0000, 0b1111_1111 }, 1)] // invalid, contains illegal char
        [InlineData(new byte[] { 0b1101_1111, 0b1011_1111, 0b0000_0000, 0b1110_1111, 0b1000_0000 }, 3)] // invalid, ends in middle of multi-byte sequence
        public void GetIndexOfFirstInvalidUtf8Byte_Tests(byte[] data, int expectedFirstIndex)
        {
            // Act & assert - 1

            Assert.Equal(expectedFirstIndex, Utf8Decoder.GetIndexOfFirstInvalidUtf8Byte(data));

            // Act & assert - 2

            Assert.Equal((expectedFirstIndex == -1), Utf8Decoder.IsWellFormedUtf8String(data));
        }

        [Theory]
        [InlineData((byte)0b1000_0000)]
        [InlineData((byte)0b1111_1000)]
        [InlineData((byte)0b1111_1111)]
        public void ReadUnicodeScalarValue_InputHasMalformedLeadingByte_ReturnsBadDataError(byte input)
        {
            // Act

            int retVal = Utf8Decoder.ReadUnicodeScalarValue(new byte[] { input }, out int numBytesConsumed);

            // Assert

            Assert.Equal(Utf8Decoder.ErrorStatus.BadCharacter, retVal);
            Assert.Equal(0, numBytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0b1100_0000, 0xFF }, 1)]
        [InlineData(new byte[] { 0b1110_0000, 0xFF, 0xFF }, 1)]
        [InlineData(new byte[] { 0b1110_0000, 0b1000_0000, 0xFF }, 2)]
        [InlineData(new byte[] { 0b1111_0000, 0xFF, 0xFF, 0xFF }, 1)]
        [InlineData(new byte[] { 0b1111_0000, 0b1000_0000, 0xFF, 0xFF }, 2)]
        [InlineData(new byte[] { 0b1111_0000, 0b1000_0000, 0b1000_0000, 0xFF }, 3)]
        public void ReadUnicodeScalarValue_InputHasMalformedTrailingByte_ReturnsBadDataError(byte[] data, int expectedNumBytesConsumed)
        {
            // Act

            int retVal = Utf8Decoder.ReadUnicodeScalarValue(data, out int actualNumBytesConsumed);

            // Assert

            Assert.Equal(Utf8Decoder.ErrorStatus.BadCharacter, retVal);
            Assert.Equal(expectedNumBytesConsumed, actualNumBytesConsumed);
        }

        [Fact]
        public void ReadUnicodeScalarValue_ReadsAllValidSequencesCorrectly()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                if (!IsSurrogateCodePoint(i))
                {
                    ReadUnicodeScalarValue_ReadsAllValidSequencesCorrectly_Core(i);
                }
            }
        }

        private static void ReadUnicodeScalarValue_ReadsAllValidSequencesCorrectly_Core(int codePoint)
        {
            // Arrange

            byte[] codePointAsUtf8 = GetUtf8SequenceForCodePoint(codePoint);

            // Act & assert 1 - buffer contains just this single code point

            int retVal1 = Utf8Decoder.ReadUnicodeScalarValue(codePointAsUtf8, out int bytesConsumed1);
            Assert.Equal(codePoint, retVal1);
            Assert.Equal(codePointAsUtf8.Length, bytesConsumed1);

            // Act & assert 2 - buffer contains extra unused chars

            byte[] codePointAsUtf8WithExtraChars = new byte[codePointAsUtf8.Length + 1];
            Buffer.BlockCopy(codePointAsUtf8, 0, codePointAsUtf8WithExtraChars, 0, codePointAsUtf8.Length);
            codePointAsUtf8WithExtraChars[codePointAsUtf8WithExtraChars.Length - 1] = 0x80; // invalid byte

            int retVal2 = Utf8Decoder.ReadUnicodeScalarValue(codePointAsUtf8WithExtraChars, out int bytesConsumed2);
            Assert.Equal(codePoint, retVal2);
            Assert.Equal(codePointAsUtf8.Length, bytesConsumed2);
        }

        [Theory]
        [InlineData(new byte[] { 0b1100_0001, 0b1011_1111 })] // U+007F should be encoded as [ 01111111 ]
        [InlineData(new byte[] { 0b1110_0000, 0b1001_1111, 0b1011_1111 })] // U+07FF should be encoded as [ 11011111 10111111 ]
        [InlineData(new byte[] { 0b1111_0000, 0b1000_1111, 0b1011_1111, 0b1011_1111 })] // U+FFFF should be encoded as [ 11101111 10111111 10111111 ]
        public void ReadUnicodeScalarValue_InputIsNotInShortestForm_ReturnsBadDataError(byte[] input) => RunDecoder_ExpectBadDataError(input);

        [Theory]
        [InlineData(new byte[] { 0b1110_1101, 0b1010_0000, 0b1000_0000 })] // U+D800, minimally encoded
        [InlineData(new byte[] { 0b1110_1101, 0b1011_1111, 0b1011_1111 })] // U+DFFF, minimally encoded
        public void ReadUnicodeScalarValue_InputIsSurrogate_ReturnsBadDataError(byte[] input) => RunDecoder_ExpectBadDataError(input);

        [Theory]
        [InlineData(new byte[] { })] // zero-length buffer
        [InlineData(new byte[] { 0b1100_0000 })] // marker begins a two-byte sequence
        [InlineData(new byte[] { 0b1110_0000, 0b1000_0000 })] // marker begins a three-byte sequence
        [InlineData(new byte[] { 0b1111_0000, 0b1000_0000, 0b1000_0000 })] // marker begins a four-byte sequence
        private static void ReadUnicodeScalarValue_WithTooShortBuffer_ReturnsInsufficientDataError(byte[] input)
        {
            // Act

            int retVal = Utf8Decoder.ReadUnicodeScalarValue(input, out int bytesConsumed);

            // Assert

            Assert.Equal(Utf8Decoder.ErrorStatus.InsufficientData, retVal);
            Assert.Equal(0, bytesConsumed);
        }

        private static bool IsSurrogateCodePoint(int i)
        {
            return (0xD800 <= i && i <= 0xDFFF);
        }

        private static void RunDecoder_ExpectBadDataError(byte[] input)
        {
            // Act

            int retVal = Utf8Decoder.ReadUnicodeScalarValue(input, out int bytesConsumed);

            // Assert

            Assert.Equal(Utf8Decoder.ErrorStatus.BadCharacter, retVal);
            Assert.Equal(input.Length, bytesConsumed);
        }

        private static byte[] GetUtf8SequenceForCodePoint(int codePoint)
        {
            return _strictEncoding.GetBytes(Char.ConvertFromUtf32(codePoint));
        }
    }
}
