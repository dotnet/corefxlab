// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public static class TextEncoderTestHelper
    {
        private const int CharLength = 999;

        public static bool BuffersAreEqual<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual)
        {
            if (expected.Length != actual.Length) return false;
            for (int i = 0; i < expected.Length; i++)
            {
                if (!expected[i].Equals(actual[i])) return false;
            }
            return true;
        }

        public static bool Validate(TextEncoder encoder, Text.Encoding testEncoder, CodePointSubset subset)
        {
            int minCodePoint = 0;
            int maxCodePoint = 0;
            bool ignoreSurrogates = false;

            switch(subset)
            {
                case CodePointSubset.ASCII:
                    minCodePoint = 0;
                    maxCodePoint = TextEncoderConstants.Utf8OneByteLastCodePoint;
                    break;
                case CodePointSubset.TwoBytes:
                    minCodePoint = TextEncoderConstants.Utf8OneByteLastCodePoint + 1;
                    maxCodePoint = TextEncoderConstants.Utf8TwoBytesLastCodePoint;
                    break;
                case CodePointSubset.ThreeBytes:
                    minCodePoint = TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1;
                    maxCodePoint = TextEncoderConstants.Utf8ThreeBytesLastCodePoint;
                    ignoreSurrogates = true;
                    break;
                case CodePointSubset.Surrogates:
                    minCodePoint = TextEncoderConstants.Utf16HighSurrogateFirstCodePoint;
                    maxCodePoint = TextEncoderConstants.Utf16LowSurrogateLastCodePoint;
                    break;
                case CodePointSubset.Mixed:
                    minCodePoint = 0;
                    maxCodePoint = TextEncoderConstants.Utf8ThreeBytesLastCodePoint;
                    break;
                default:
                    break;
            }

            string inputString = GenerateValidString(CharLength, minCodePoint, maxCodePoint, ignoreSurrogates);
            ReadOnlySpan<char> utf16 = inputString.AsSpan();
            byte[] expectedBytes = testEncoder.GetBytes(utf16.ToArray());
            int expectedBytesWritten = expectedBytes.Length;
            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);

            bool retVal = true;
            retVal &= encoder.TryEncode(utf16, encodedBytes, out int charactersConsumed, out int bytesWritten);
            retVal &= utf16.Length == charactersConsumed;
            retVal &= expectedBytesWritten == bytesWritten;
            retVal &= BuffersAreEqual<byte>(expectedBytes, encodedBytes);

            return retVal;
        }

        public static string GenerateValidString(int length, int minCodePoint, int maxCodePoint, bool ignoreSurrogates = false)
        {
            Random rand = new Random(42);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(minCodePoint, maxCodePoint);

                if (ignoreSurrogates)
                {
                    while (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters
                    }
                    plainText.Append((char)val);
                    continue;
                }
                
                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint); // low surrogate
                    }
                }
                else
                {
                    while (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters if they can't be paired
                    }
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static string GenerateAllCharacters()
        {
            var plainText = new StringBuilder();
            for (int j = 0; j <= TextEncoderConstants.Utf8ThreeBytesLastCodePoint; j++)
            {
                if (j >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && j <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                {
                    // high surrogate, pair it with all low surrogates possible
                    for (int i = TextEncoderConstants.Utf16LowSurrogateFirstCodePoint; i <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint; i++)
                    {
                        plainText.Append((char)j);
                        plainText.Append((char)i);
                    }
                }
                else if (j >= 0xDC00 && j <= 0xDFFF)
                {
                    continue;   // don't want unpaird low surrogates
                }
                else
                {
                    // characters not reserved for surrogate pairs
                    plainText.Append((char)j);
                }
            }
            return plainText.ToString();    // Length should be 0x10FFFF characters (1114112 in decimal)
        }

        public static string GenerateOnlyInvalidString(int length)
        {
            Random rand = new Random(42);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint);
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static string GenerateStringWithInvalidChars(int length)
        {
            Random rand = new Random(42);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);

                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint); // high surrogate may not be paired with a low surrogate (invalid)
                    }
                }
                else
                {
                    // last char should be high surrogate (no low surrogate after, invalid)
                    val = rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint);
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public enum CodePointSubset
        {
            ASCII, TwoBytes, ThreeBytes, Surrogates, Mixed
        }
    }
}
