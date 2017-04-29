// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class EncodeIntoUtf8Tests : ITextEncoderTest
    {
        private static TextEncoder utf8 = TextEncoder.Utf8;
        private static Text.Encoding testEncoder = Text.Encoding.UTF8;
        private static Text.Encoding testEncoderUnicode = Text.Encoding.Unicode;
        private static Text.Encoding testEncoderUtf32 = Text.Encoding.UTF32;

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(0, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan <char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            var encodedBytes = new Span<byte>(new byte[0]);    // Output buffer is size 0

            Assert.True(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(utf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));

            encodedBytes = new Span<byte>(new byte[1]); // Output buffer is not size 0

            Assert.True(utf8.TryEncode(utf16, encodedBytes, out charactersConsumed, out bytesWritten));
            Assert.Equal(utf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes.Slice(0, bytesWritten)));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void OutputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            var encodedBytes = new Span<byte>(new byte[0]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(0, charactersConsumed);
            Assert.Equal(0, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(Span<byte>.Empty, encodedBytes));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferLargerThanOutputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes1, out int charactersConsumed1, out int bytesWritten1));

            var encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten - bytesWritten1]);

            Assert.True(utf8.TryEncode(utf16.Slice(charactersConsumed1), encodedBytes2, out int charactersConsumed2, out int bytesWritten2));
            Assert.Equal(utf16.Length, charactersConsumed1 + charactersConsumed2);
            Assert.Equal(expectedBytesWritten, bytesWritten1 + bytesWritten2);

            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
            encodedBytes1.CopyTo(encodedBytes);
            encodedBytes2.CopyTo(encodedBytes.Slice(encodedBytes1.Length));

            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void OutputBufferLargerThanInputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> firstUtf16 = inputString.AsSpan();
            byte[] expectedBytes1 = testEncoder.GetBytes(firstUtf16.ToArray());

            inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> secondUtf16 = inputString.AsSpan();
            byte[] expectedBytes2 = testEncoder.GetBytes(secondUtf16.ToArray());

            int expectedBytesWritten = expectedBytes1.Length + expectedBytes2.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.True(utf8.TryEncode(firstUtf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(firstUtf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes1.Length, bytesWritten);

            Assert.True(utf8.TryEncode(secondUtf16, encodedBytes.Slice(bytesWritten), out charactersConsumed, out bytesWritten));
            Assert.Equal(secondUtf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes2.Length, bytesWritten);

            var expectedBytes = new byte[expectedBytesWritten];
            Array.Copy(expectedBytes1, expectedBytes, expectedBytes1.Length);
            Array.Copy(expectedBytes2, 0, expectedBytes, expectedBytes1.Length, expectedBytes2.Length);

            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferContainsOnlyInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateOnlyInvalidString(TextEncoderConstants.CharLength);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(0, charactersConsumed);
            Assert.Equal(0, bytesWritten);
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferContainsSomeInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateStringWithInvalidChars(TextEncoderConstants.CharLength);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = GetByteCount(utf16);
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.True(charactersConsumed < inputString.Length);
            Assert.Equal(expectedBytesWritten, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes.AsSpan().Slice(0, expectedBytesWritten), encodedBytes));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferEndsOnHighSurrogateAndRestart(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.CharLength, false);
            string inputString2 = inputString1 + TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.CharLength, true);

            ReadOnlySpan<char> firstUtf16 = inputString1.AsSpan();
            ReadOnlySpan<char> secondUtf16 = inputString2.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(secondUtf16.ToArray());
            var encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);

            Assert.False(utf8.TryEncode(firstUtf16, encodedBytes, out int charactersConsumed1, out int bytesWritten1));
            Assert.True(utf8.TryEncode(secondUtf16.Slice(charactersConsumed1), encodedBytes.Slice(bytesWritten1), out int charactersConsumed2, out int bytesWritten2));
            Assert.Equal(inputString2.Length, charactersConsumed1 + charactersConsumed2);
            Assert.Equal(expectedBytes.Length, bytesWritten1 + bytesWritten2);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferContainsOnlyASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ASCII));  // 1 byte
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferContainsNonASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.TwoBytes));  // 2 bytes
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ThreeBytes));  // 3 bytes
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Surrogates));  // 4 bytes (high and low surrogates)
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Mixed));  // mixed
        }

        [Theory]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferContainsAllCodePoints(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateAllCharacters();

            byte[] expectedBytes;
            Span<byte> encodedBytes;
            
            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoder.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<byte> inputUtf8 = temp;
                    Assert.True(utf8.TryEncode(inputUtf8, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.Equal(inputUtf8.Length, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<char> inputUtf16 = temp.AsSpan().NonPortableCast<byte, char>();
                    Assert.True(utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(inputUtf16.Length, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    string inputStr = inputString;
                    Assert.True(utf8.TryEncode(inputStr, encodedBytes, /*out charactersConsumed,*/ out bytesWritten));
                    //Assert.Equal(inputString.Length, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<uint> input = temp.AsSpan().NonPortableCast<byte, uint>();
                    Assert.True(utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(input.Length, charactersConsumed);
                    break;
            }

            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }

        private int GetByteCount(ReadOnlySpan<byte> utf8)
        {
            return utf8.Length;
        }

        private int GetByteCount(ReadOnlySpan<char> utf16)
        {
            var inputLength = utf16.Length;

            int byteCount = 0;
            for (int i = 0; i < inputLength; i++)
            {
                char codePoint = utf16[i];
                if (codePoint <= TextEncoderConstants.Utf8OneByteLastCodePoint)
                {
                    byteCount += 1;
                }
                else if (codePoint <= TextEncoderConstants.Utf8TwoBytesLastCodePoint)
                {
                    byteCount += 2;
                }
                else if (codePoint >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && codePoint <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                {
                    i++;
                    if (i >= inputLength)
                    {
                        return byteCount;
                    }
                    char lowSurrogate = utf16[i];

                    if (lowSurrogate < TextEncoderConstants.Utf16LowSurrogateFirstCodePoint || lowSurrogate > TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        return byteCount;    // invalid char
                    }
                    byteCount += 4;
                }
                else if (codePoint >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && codePoint <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                {
                    return byteCount;        // invalid char
                }
                else
                {
                    byteCount += 3;
                }
            }
            return byteCount;
        }

        private int GetByteCount(string utf16String)
        {
            return GetByteCount(utf16String.AsSpan());
        }

        private int GetByteCount(ReadOnlySpan<uint> utf32)
        {
            int byteCount = 0;
            for (int i = 0; i < utf32.Length; i++)
            {
                uint codePoint = utf32[i];
                if (codePoint > TextEncoderConstants.Utf8ThreeBytesLastCodePoint)
                    byteCount += 4;
                else if (codePoint > TextEncoderConstants.Utf8TwoBytesLastCodePoint)
                    byteCount += 3;
                else if (codePoint > TextEncoderConstants.Utf8OneByteLastCodePoint)
                    byteCount += 2;
                else
                    byteCount += 1;
            }
            return byteCount;
        }
    }
}
