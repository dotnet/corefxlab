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

        private const int CharLength = 999;

        [Fact]
        public void InputBufferEmpty()
        {
            ReadOnlySpan<char> utf16 = ReadOnlySpan<char>.Empty;
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            var encodedBytes = Span<byte>.Empty;    // Output buffer is size 0

            Assert.True(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(utf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));

            encodedBytes = new Span<byte>(new byte[100]); // Output buffer is not size 0

            Assert.True(utf8.TryEncode(utf16, encodedBytes, out charactersConsumed, out bytesWritten));
            Assert.Equal(utf16.Length, charactersConsumed);
            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes.Slice(0, bytesWritten)));
        }

        [Fact]
        public void OutputBufferEmpty()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            var encodedBytes = Span<byte>.Empty;

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(0, charactersConsumed);
            Assert.Equal(0, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(Span<byte>.Empty, encodedBytes));
        }

        [Fact]
        public void InputBufferLargerThanOutputBuffer()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes1, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(utf16.Length / 2, charactersConsumed);
            Assert.Equal(expectedBytesWritten / 2, bytesWritten);

            var encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten / 2]);

            Assert.True(utf8.TryEncode(utf16.Slice(charactersConsumed), encodedBytes2, out charactersConsumed, out bytesWritten));
            Assert.Equal(utf16.Length / 2, charactersConsumed);
            Assert.Equal(expectedBytesWritten / 2, bytesWritten);

            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
            encodedBytes1.CopyTo(encodedBytes);
            encodedBytes2.CopyTo(encodedBytes.Slice(encodedBytes1.Length));

            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }

        [Fact]
        public void OutputBufferLargerThanInputBuffer()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<char> firstUtf16 = inputString.AsSpan();
            byte[] expectedBytes1 = testEncoder.GetBytes(firstUtf16.ToArray());

            inputString = TextEncoderTestHelper.GenerateValidString(CharLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
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

        [Fact]
        public void InputBufferContainsOnlyInvalidData()
        {
            string inputString = TextEncoderTestHelper.GenerateOnlyInvalidString(CharLength);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(0, charactersConsumed);
            Assert.Equal(0, bytesWritten);
        }

        [Fact]
        public void InputBufferContainsSomeInvalidData()
        {
            string inputString = TextEncoderTestHelper.GenerateStringWithInvalidChars(CharLength);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.False(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.True(charactersConsumed < inputString.Length);
            Assert.Equal(expectedBytesWritten, bytesWritten);
        }

        [Fact]
        public void InputBufferContainsOnlyASCII()
        {
            Assert.True(TextEncoderTestHelper.Validate(utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ASCII));  // 1 byte
        }

        [Fact]
        public void InputBufferContainsNonASCII()
        {
            Assert.True(TextEncoderTestHelper.Validate(utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.TwoBytes));  // 2 bytes
            Assert.True(TextEncoderTestHelper.Validate(utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ThreeBytes));  // 3 bytes
            Assert.True(TextEncoderTestHelper.Validate(utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Surrogates));  // 4 bytes (high and low surrogates)
            Assert.True(TextEncoderTestHelper.Validate(utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Mixed));  // mixed
        }

        [Fact]
        public void InputBufferContainsAllCodePoints()
        {
            string inputString = TextEncoderTestHelper.GenerateAllCharacters();
            int charLengthOfAllCharacters = ((0xD7FF - 0) + 1) + ((TextEncoderConstants.Utf8ThreeBytesLastCodePoint - 0xE000) + 1); // single char
            charLengthOfAllCharacters += 2 * 
                ((TextEncoderConstants.Utf16HighSurrogateLastCodePoint - TextEncoderConstants.Utf16HighSurrogateFirstCodePoint) + 1) * 
                ((TextEncoderConstants.Utf16LowSurrogateLastCodePoint - TextEncoderConstants.Utf16LowSurrogateFirstCodePoint) + 1);  //double char

            Assert.Equal(inputString.Length, charLengthOfAllCharacters);  // should be equal to 2160640

            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            Assert.True(utf8.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten));
            Assert.Equal(utf16.Length, charactersConsumed);
            Assert.Equal(expectedBytesWritten, bytesWritten);
            Assert.True(TextEncoderTestHelper.BuffersAreEqual<byte>(expectedBytes, encodedBytes));
        }
    }
}
