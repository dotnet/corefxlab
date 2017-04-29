// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public static class TextEncoderTestHelper
    {
        public static bool BuffersAreEqual<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual)
        {
            if (expected.Length != actual.Length) return false;
            for (int i = 0; i < expected.Length; i++)
            {
                if (!expected[i].Equals(actual[i]))
                {
                    Console.WriteLine(expected[i] + " : " + actual[i]);
                    return false;
                }
            }
            return true;
        }

        public static bool Validate(SupportedEncoding from, TextEncoder utf8, Text.Encoding testEncoder, CodePointSubset subset)
        {
            string inputString = GenerateValidString(TextEncoderConstants.CharLength, subset);
            Text.Encoding testEncoderUtf8 = Text.Encoding.UTF8;
            Text.Encoding testEncoderUnicode = Text.Encoding.Unicode;
            Text.Encoding testEncoderUtf32 = Text.Encoding.UTF32;

            byte[] expectedBytes;
            Span<byte> encodedBytes;

            int bytesWritten;
            bool retVal = true;

            switch (from)
            {
                case SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoderUtf8.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf8, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<byte> inputUtf8 = temp;
                    retVal &= utf8.TryEncode(inputUtf8, encodedBytes, out int charactersConsumed, out bytesWritten);
                    retVal &= inputUtf8.Length == charactersConsumed;
                    break;

                case SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<char> inputUtf16 = temp.AsSpan().NonPortableCast<byte, char>();
                    retVal &= utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten);
                    retVal &= inputUtf16.Length == charactersConsumed;
                    break;

                case SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    string inputStr = inputString;
                    retVal &= utf8.TryEncode(inputStr, encodedBytes, /*out charactersConsumed,*/ out bytesWritten);
                    //retVal &= inputString.Length == charactersConsumed;
                    break;

                case SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<uint> input = temp.AsSpan().NonPortableCast<byte, uint>();
                    retVal &= utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten);
                    retVal &= input.Length == charactersConsumed;
                    break;
            }

            retVal &= expectedBytes.Length == bytesWritten;
            retVal &= BuffersAreEqual<byte>(expectedBytes, encodedBytes);

            return retVal;
        }

        public static string GenerateValidString(int length, CodePointSubset subset)
        {
            int minCodePoint = 0;
            int maxCodePoint = 0;
            bool ignoreSurrogates = false;

            switch (subset)
            {
                case CodePointSubset.ASCII:
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
                    maxCodePoint = TextEncoderConstants.Utf8ThreeBytesLastCodePoint;
                    break;
                default:
                    break;
            }

            return GenerateValidString(TextEncoderConstants.CharLength, minCodePoint, maxCodePoint, ignoreSurrogates);
        }

        public static string GenerateValidString(int length, int minCodePoint, int maxCodePoint, bool ignoreSurrogates = false)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed);
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
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint); // skip surrogate characters if they can't be paired
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
                else if (j >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && j <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
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
            Random rand = new Random(TextEncoderConstants.RandomSeed);
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
            Random rand = new Random(TextEncoderConstants.RandomSeed);
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

        public static string GenerateValidStringEndsWithHighStartsWithLow(int charLength, bool startsWithLow)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed);
            var plainText = new StringBuilder();

            int val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            if (charLength > 0)
            {
                if (startsWithLow)
                {
                    // first character must be low surrogate
                    val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint);
                }
                plainText.Append((char)val);
            }

            for (int j = 1; j < charLength; j++)
            {
                val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);

                if (j < charLength - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint);  // low surrogate
                    }
                }
                else
                {
                    // if first char is valid, last char should be high surrogate (no low surrogate after, invalid)
                    val = startsWithLow ? rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint) : rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint);
                }

                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public enum CodePointSubset
        {
            ASCII, TwoBytes, ThreeBytes, Surrogates, Mixed
        }

        public enum SupportedEncoding
        {
            FromUtf8, FromUtf16, FromString, FromUtf32
        }
    }
}
