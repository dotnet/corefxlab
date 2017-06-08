// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class Utf8EncoderTests
    {
        public static object[][] TryEncodeFromUTF16ToUTF8TestData = {
            // empty
            new object[] { true, new byte[] { }, new char[]{ (char)0x0050 }, false },
            // multiple bytes
            new object[] { true, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },
                new char[]{ (char)0x0050, (char)0x03E8, (char)0xAFC8, (char)0xD852, (char)0xDDF0 }, true },
        };

        [Theory, MemberData("TryEncodeFromUTF16ToUTF8TestData")]
        public void UTF16ToUTF8EncodingTestForReadOnlySpanOfChar(bool useUtf8Encoder, byte[] expectedBytes, char[] chars, bool expectedReturnVal)
        {
            TextEncoder encoder = useUtf8Encoder ? TextEncoder.Utf8 : TextEncoder.Utf16;
            ReadOnlySpan<char> characters = new ReadOnlySpan<char>(chars);
            Span<byte> buffer = new Span<byte>(new byte[expectedBytes.Length]);
            int bytesWritten;
            int consumed;

            Assert.Equal(expectedReturnVal, encoder.TryEncode(characters, buffer, out consumed, out bytesWritten));
            Assert.Equal(expectedReturnVal ? expectedBytes.Length : 0, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.Equal(characters.Length, consumed);
                Assert.True(AreByteArraysEqual(expectedBytes, buffer.ToArray()));
            }
        }

        public static object[][] TryEncodeFromUnicodeMultipleCodePointsTestData = {
             // empty
            new object[] { true, new byte[] { }, new uint[] { 0x50 }, false },
            new object[] { false, new byte[] { }, new uint[] { 0x50 }, false },
            // multiple bytes
            new object[] { true, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , true },
            new object[] { false, new byte[] { 0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , true },
            // multiple bytes - buffer too small
            new object[] { true, new byte[] { 0x50 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , false },
            new object[] { false, new byte[] { 0x50, 0x00 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , false },
        };

        [Theory, MemberData("TryEncodeFromUnicodeMultipleCodePointsTestData")]
        public void TryEncodeFromUnicodeMultipleCodePoints(bool useUtf8Encoder, byte[] expectedBytes, uint[] codePointsArray, bool expectedReturnVal)
        {
            TextEncoder encoder = useUtf8Encoder ? TextEncoder.Utf8 : TextEncoder.Utf16;
            ReadOnlySpan<uint> codePoints = new ReadOnlySpan<uint>(codePointsArray);
            Span<byte> buffer = new Span<byte>(new byte[expectedBytes.Length]);
            int bytesWritten;
            int consumed;

            Assert.Equal(expectedReturnVal, encoder.TryEncode(codePoints, buffer, out consumed, out bytesWritten));
            Assert.Equal(expectedBytes.Length, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.Equal(codePoints.Length, consumed);
                Assert.True(AreByteArraysEqual(expectedBytes, buffer.ToArray()));
            }
        }

        public static object[][] TryDecodeToUnicodeMultipleCodePointsTestData = {
            //empty
            new object[] { true, new uint[] {}, new byte[] {}, true },
            new object[] { false, new uint[] {}, new byte[] {}, true },
            // multiple bytes
            new object[] { true,
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 }, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 }, true },
            new object[] { false,
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 }, new byte[] {  0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD }, true },
        };

        [Theory, MemberData("TryDecodeToUnicodeMultipleCodePointsTestData")]
        public void TryDecodeToUnicodeMultipleCodePoints(bool useUtf8Encoder, uint[] expectedCodePointsArray, byte[] inputBytesArray, bool expectedReturnVal)
        {
            TextEncoder encoder = useUtf8Encoder ? TextEncoder.Utf8 : TextEncoder.Utf16;
            Span<uint> expectedCodePoints = new Span<uint>(expectedCodePointsArray);
            Span<byte> inputBytes = new Span<byte>(inputBytesArray);
            Span<uint> codePoints = new Span<uint>(new uint[expectedCodePoints.Length]);
            int bytesWritten;
            int consumed;

            Assert.Equal(expectedReturnVal, encoder.TryDecode(inputBytes, codePoints, out consumed, out bytesWritten));

            if (expectedReturnVal)
            {
                Assert.Equal(inputBytes.Length, consumed);
                Assert.True(AreCodePointArraysEqual(expectedCodePoints.ToArray(), codePoints.ToArray()));
            }

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BruteTestingRoundtripEncodeDecodeAllUnicodeCodePoints(bool useUtf8Encoder)
        {
            TextEncoder encoder = useUtf8Encoder ? TextEncoder.Utf8 : TextEncoder.Utf16;
            const uint maximumValidCodePoint = 0x10FFFF;
            uint[] expectedCodePoints = new uint[maximumValidCodePoint + 1];
            for (uint i = 0; i <= maximumValidCodePoint; i++)
            {
                if (i >= 0xD800 && i <= 0xDFFF)
                {
                    expectedCodePoints[i] = 0; // skip surrogate characters
                }
                else
                {
                    expectedCodePoints[i] = i;
                }
            }

            ReadOnlySpan<uint> expectedCodePointsSpan = new ReadOnlySpan<uint>(expectedCodePoints);
            uint maxBytes = 4 * (maximumValidCodePoint + 1);
            Span<byte> buffer = new Span<byte>(new byte[maxBytes]);
            int bytesEncoded;
            int consumed;
            Assert.True(encoder.TryEncode(expectedCodePointsSpan, buffer, out consumed, out bytesEncoded));

            buffer = buffer.Slice(0, bytesEncoded);
            Span<uint> codePoints = new Span<uint>(new uint[maximumValidCodePoint + 1]);
            int written;
            Assert.True(encoder.TryDecode(buffer, codePoints, out consumed, out written));

            Assert.Equal(bytesEncoded, consumed);
            Assert.Equal(maximumValidCodePoint + 1, (uint)written);

            for (int i = 0; i <= maximumValidCodePoint; i++)
            {
                Assert.Equal(expectedCodePointsSpan[i], codePoints[i]);
            }
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EncodeAllUnicodeCodePoints(bool useUtf8Encoder)
        {
            TextEncoder encoder = useUtf8Encoder ? TextEncoder.Utf8 : TextEncoder.Utf16;
            Text.Encoding systemEncoder = useUtf8Encoder ? Text.Encoding.UTF8 : Text.Encoding.Unicode;
            const uint maximumValidCodePoint = 0x10FFFF;
            uint[] codePoints = new uint[maximumValidCodePoint + 1];

            var plainText = new StringBuilder();
            for (int i = 0; i <= maximumValidCodePoint; i++)
            {
                if (i >= 0xD800 && i <= 0xDFFF)
                {
                    codePoints[i] = 0; // skip surrogate characters
                    plainText.Append((char)0); // skip surrogate characters
                }
                else
                {
                    codePoints[i] = (uint)i;

                    if (i > 0xFFFF)
                    {
                        plainText.Append(char.ConvertFromUtf32(i));
                    }
                    else
                    {
                        plainText.Append((char)i);
                    }
                }
            }

            ReadOnlySpan<uint> codePointsSpan = new ReadOnlySpan<uint>(codePoints);
            uint maxBytes = 4 * (maximumValidCodePoint + 1);
            Span<byte> buffer = new Span<byte>(new byte[maxBytes]);
            int bytesWritten;
            int consumed;
            Assert.True(encoder.TryEncode(codePointsSpan, buffer, out consumed, out bytesWritten));

            string unicodeString = plainText.ToString();
            ReadOnlySpan<char> characters = unicodeString.AsSpan();
            int byteCount = systemEncoder.GetByteCount(unicodeString);
            byte[] buff = new byte[byteCount];
            Span<byte> expectedBuffer;
            char[] charArray = characters.ToArray();

            systemEncoder.GetBytes(charArray, 0, characters.Length, buff, 0);
            expectedBuffer = new Span<byte>(buff);

            int minLength = Math.Min(expectedBuffer.Length, buffer.Length);

            for (int i = 0; i < minLength; i++)
            {
                Assert.Equal(expectedBuffer[i], buffer[i]);
            }
        }

        public bool AreByteArraysEqual(byte[] arrayOne, byte[] arrayTwo)
        {
            if (arrayOne.Length != arrayTwo.Length) return false;

            for (int i = 0; i < arrayOne.Length; i++)
            {
                if (arrayOne[i] != arrayTwo[i]) return false;
            }

            return true;
        }

        public bool AreCodePointArraysEqual(uint[] arrayOne, uint[] arrayTwo)
        {
            if (arrayOne.Length != arrayTwo.Length) return false;

            for (int i = 0; i < arrayOne.Length; i++)
            {
                if (!arrayOne[i].Equals(arrayTwo[i])) return false;
            }

            return true;
        }

        public static object[] PartialEncodeDecodeUtf8ToUtf16TestCases = new object[]
        {
            //new object[]
            //{
            //    /* output buffer size */, /* consumed on first pass */,
            //    new byte[] { /* UTF-8 encoded input data */ },
            //    new byte[] { /* expected output first pass */ },
            //    new byte[] { /* expected output second pass */ },
            //},
            new object[]
            {
                4, 2,
                new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F },
                new byte[] { 0x48, 0x00, 0x65, 0x00 },
                new byte[] { 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00 },
            },
            new object[]
            {
                5, 6,
                new byte[] { 0xE6, 0xA8, 0x99, 0xE6, 0xBA, 0x96, 0xE8, 0x90, 0xAC, 0xE5, 0x9C, 0x8B, 0xE7, 0xA2, 0xBC },
                new byte[] { 0x19, 0x6A, 0x96, 0x6E },
                new byte[] { 0x2C, 0x84, 0x0B, 0x57, 0xBC, 0x78 },
            },
        };

        [Theory, MemberData("PartialEncodeDecodeUtf8ToUtf16TestCases")]
        public void TryPartialUtf8ToUtf16EncodingTest(int outputSize, int expectedConsumed, byte[] inputBytes, byte[] expected1, byte[] expected2)
        {
            Span<byte> input = inputBytes;
            Span<byte> output = new byte[outputSize];

            var result = Encoders.Utf16.ConvertFromUtf8(input, output, out int consumed, out int written);
            Assert.Equal(TransformationStatus.DestinationTooSmall, result);
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(output.Slice(0, written).SequenceEqual(expected1));

            input = input.Slice(consumed);
            output = new byte[expected2.Length];
            result = Encoders.Utf16.ConvertFromUtf8(input, output, out consumed, out written);
            Assert.Equal(TransformationStatus.Done, result);
            Assert.Equal(expected2.Length, written);
            Assert.Equal(inputBytes.Length - expectedConsumed, consumed);
            Assert.True(output.SequenceEqual(expected2));
        }

        [Theory, MemberData("PartialEncodeDecodeUtf8ToUtf16TestCases")]
        public void TryPartialUtf8ToUtf16DecodingTest(int outputSize, int expectedConsumed, byte[] inputBytes, byte[] expected1, byte[] expected2)
        {
            int written;
            int consumed;

            var input = new Span<byte>(inputBytes);
            var output = new Span<byte>(new byte[outputSize]);

            Assert.False(TextEncoder.Utf8.TryDecode(input, output.NonPortableCast<byte, char>(), out consumed, out written));
            Assert.Equal(expected1.Length, written * sizeof(char));
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected1, output.Slice(0, written * sizeof(char)).ToArray()));

            input = input.Slice(consumed);
            output = new Span<byte>(new byte[expected2.Length]);
            Assert.True(TextEncoder.Utf8.TryDecode(input, output.NonPortableCast<byte, char>(), out consumed, out written));
            Assert.Equal(expected2.Length, written * sizeof(char));
            Assert.Equal(inputBytes.Length - expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected2, output.ToArray()));
        }

        public static object[] PartialEncodeDecodeUtf16ToUtf8TestCases = new object[]
        {
            //new object[]
            //{
            //    /* output buffer size */, /* consumed on first pass */,
            //    new char[] { /* UTF-16 encoded input data */ },
            //    new byte[] { /* expected output first pass */ },
            //    new byte[] { /* expected output second pass */ },
            //},
            new object[]
            {
                2, 2,
                new char[] { '\u0048', '\u0065', '\u006C', '\u006C', '\u006F' },
                new byte[] { 0x48, 0x65 },
                new byte[] { 0x6C, 0x6C, 0x6F },
            },
            new object[]
            {
                7, 2,
                new char[] { '\u6A19', '\u6E96', '\u842C', '\u570B', '\u78BC' },
                new byte[] { 0xE6, 0xA8, 0x99, 0xE6, 0xBA, 0x96 },
                new byte[] { 0xE8, 0x90, 0xAC, 0xE5, 0x9C, 0x8B, 0xE7, 0xA2, 0xBC },
            },
        };

        [Theory, MemberData("PartialEncodeDecodeUtf16ToUtf8TestCases")]
        public void TryPartialUtf16ToUtf8EncodingTest(int outputSize, int expectedConsumed, char[] inputBytes, byte[] expected1, byte[] expected2)
        {
            int written;
            int consumed;

            var input = new Span<char>(inputBytes);
            var output = new Span<byte>(new byte[outputSize]);

            Assert.False(TextEncoder.Utf8.TryEncode(input, output, out consumed, out written));
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected1, output.Slice(0, written).ToArray()));

            input = input.Slice(consumed);
            output = new Span<byte>(new byte[expected2.Length]);
            Assert.True(TextEncoder.Utf8.TryEncode(input, output, out consumed, out written));
            Assert.Equal(expected2.Length, written);
            Assert.Equal(inputBytes.Length - expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected2, output.ToArray()));
        }

        [Theory, MemberData("PartialEncodeDecodeUtf16ToUtf8TestCases")]
        public void TryPartialUtf16ToUtf8DecodingTest(int outputSize, int expectedConsumed, char[] inputBytes, byte[] expected1, byte[] expected2)
        {
            int written;
            int consumed;

            var input = new Span<char>(inputBytes).AsBytes();
            var output = new Span<byte>(new byte[outputSize]);

            Assert.False(TextEncoder.Utf16.TryDecode(input, output, out consumed, out written));
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected1, output.Slice(0, written).ToArray()));

            input = input.Slice(consumed * sizeof(char));
            output = new Span<byte>(new byte[expected2.Length]);
            Assert.True(TextEncoder.Utf16.TryDecode(input, output, out consumed, out written));
            Assert.Equal(expected2.Length, written);
            Assert.Equal(inputBytes.Length - expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected2, output.ToArray()));
        }
    }
}
