// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public static class TextEncoderTestHelper
    {
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

            return GenerateValidString(TextEncoderConstants.DataLength, minCodePoint, maxCodePoint, ignoreSurrogates);
        }

        public static string GenerateValidString(int length, int minCodePoint, int maxCodePoint, bool ignoreSurrogates = false)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(minCodePoint, maxCodePoint + 1);

                if (ignoreSurrogates)
                {
                    while (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint + 1); // skip surrogate characters
                    }
                    plainText.Append((char)val);
                    continue;
                }

                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint + 1); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1); // low surrogate
                    }
                }
                else
                {
                    while (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1); // skip surrogate characters if they can't be paired
                    }
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static string GenerateStringAlternatingASCIIAndNonASCII(int charLength)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 5);
            var plainText = new StringBuilder();

            for (int j = 0; j < charLength / 3; j++)
            {
                var ascii = rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1);
                var highSurrogate = rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1);
                var lowSurrogate = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);

                plainText.Append((char)ascii);
                plainText.Append((char)highSurrogate);
                plainText.Append((char)lowSurrogate);
            }

            for (int j = 0; j < charLength % 3; j++)
            {
                plainText.Append((char)rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1));
            }

            return plainText.ToString();
        }

        public static string GenerateStringWithMostlyASCIIAndSomeNonASCII(int charLength)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 6);
            var plainText = new StringBuilder();

            int j = 0;
            while (j < charLength - 70)
            {
                for (int i = 0; i < rand.Next(20, 51); i++)
                {
                    var ascii = rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1);
                    plainText.Append((char)ascii);
                    j++;
                }
                for (int i = 0; i < rand.Next(5, 11); i++)
                {
                    var highSurrogate = rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1);
                    var lowSurrogate = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);
                    j += 2;
                    plainText.Append((char)highSurrogate);
                    plainText.Append((char)lowSurrogate);
                }
            }

            while (j < charLength)
            {
                plainText.Append((char)rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1));
                j++;
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
                else if (!IsSupportedCodePoint((uint)j))
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

        public static string GenerateOnlyInvalidString(int length, bool highSurrogateOnly = false)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 2);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = highSurrogateOnly ?
                    rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1) :
                    rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static byte[] GenerateOnlyInvalidUtf8Bytes(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 2);
            var utf8Byte = new byte[length];
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(0x60, 0xFF + 1); // Each byte starts with 11xx xxxx with no bytes that start with 10xx xxxx following it.
                utf8Byte[j] = (byte)val;
            }
            return utf8Byte;
        }

        public static uint[] GenerateOnlyInvalidUtf32CodePoints(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 2);

            var codepoints = new uint[length];
            for (int j = 0; j < length; j++)
                codepoints[j] = GenerateInvalidCodePoint(ref rand);

            return codepoints;
        }

        public static string GenerateInvalidStringEndsWithLow(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 3);
            var plainText = new StringBuilder();
            for (int j = 0; j < length - 1; j++)
            {
                var val = rand.Next(0, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1);
                plainText.Append((char)val);
            }
            // last char should be low surrogate (no high surrogate before, invalid)
            plainText.Append((char)rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1));
            return plainText.ToString();
        }

        public static string GenerateStringWithInvalidChars(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 3);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1);

                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1); // high surrogate may not be paired with a low surrogate (invalid)
                    }
                }
                else
                {
                    // last char should be high surrogate (no low surrogate after, invalid)
                    val = rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1);
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static byte[] GenerateUtf8BytesWithInvalidBytes(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 3);
            var utf8Byte = new byte[length];
            for (int j = 0; j < length; j++)
            {
                // High probability of invalid utf8 bytes since there is no gaurantee that bytes following a byte within 0x60 and 0xFF
                // will be of the required form 10xx xxxx (and that there will be the correct number of such bytes).
                var val = rand.Next(0, 0xFF + 1);
                if (j < length - 1)
                {
                    utf8Byte[j] = (byte)val;
                }
                else
                {
                    utf8Byte[j] = 0xFF; // Only possible for codepoints larger than 0x10FFFF, hence invalid
                }
            }
            return utf8Byte;
        }

        public static uint[] GenerateUtf32WithInvalidCodePoints(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 3);

            var codepoints = new uint[length];
            for (int j = 0; j < length; j++)
            {
                // Every 10 codepoints, we will inject a guaranteed invalid one.
                // However, there may be others as well based on small ranges inside the valid codepoint range.
                if (j % 10 == 0)
                    codepoints[j] = GenerateInvalidCodePoint(ref rand);
                else
                    codepoints[j] = (uint)rand.Next(0, 0x110000);
            }

            return codepoints;
        }

        public static string GenerateValidStringEndsWithHighStartsWithLow(int length, bool startsWithLow)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 4);
            var plainText = new StringBuilder();

            int val = rand.Next(0, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1);
            if (length > 0)
            {
                if (startsWithLow)
                {
                    // first character must be low surrogate
                    val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);
                }
                plainText.Append((char)val);
            }

            for (int j = 1; j < length; j++)
            {
                val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1);

                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint + 1); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);  // low surrogate
                    }
                }
                else
                {
                    // if first char is valid, last char should be high surrogate (no low surrogate after, invalid)
                    val = startsWithLow ? rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1) : rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1);
                }

                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        public static uint[] GenerateValidUtf32CodePoints(int length)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 3);

            var codepoints = new uint[length];
            for (int j = 0; j < length; j++)
            {
                while (true)
                {
                    codepoints[j] = (uint)rand.Next(0, 0x110000);
                    if (IsSupportedCodePoint(codepoints[j]))
                        break;
                }
            }

            return codepoints;
        }

        public static byte[] GenerateValidBytesUtf32EndsWithHighStartsWithLow(int length, bool startsWithLow)
        {
            Random rand = new Random(TextEncoderConstants.RandomSeed * 4);
            var utf32 = new byte[length*4];

            int val = rand.Next(0, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1);

            if (length > 0)
            {
                if (startsWithLow)
                {
                    val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);
                }
                WriteToArray(ref utf32, 0, val);
            }

            for (int j = 1; j < length - 1; j++)
            {
                val = rand.Next(0, (int)TextEncoderConstants.LastValidCodePoint + 1);

                if (j < length - 1)
                {
                    while (val >= TextEncoderConstants.Utf16LowSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, (int)TextEncoderConstants.LastValidCodePoint + 1); // skip surrogate characters if they can't be paired
                    }

                    if (val >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && val <= TextEncoderConstants.Utf16HighSurrogateLastCodePoint)
                    {
                        WriteToArray(ref utf32, j*4, val);  // high surrogate
                        j++;
                        val = rand.Next(TextEncoderConstants.Utf16LowSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint + 1);  // low surrogate
                    }
                }
                else
                {
                    // if first char is valid, last char should be high surrogate (no low surrogate after, invalid)
                    val = startsWithLow ? rand.Next(0, TextEncoderConstants.Utf8OneByteLastCodePoint + 1) : rand.Next(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16HighSurrogateLastCodePoint + 1);
                }

                WriteToArray(ref utf32, j * 4, val);
            }
            return utf32;
        }

        private static void WriteToArray(ref byte[] utf32, int index, int val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);
            Array.Copy(intBytes, 0, utf32, index, intBytes.Length);
        }

        public static int GetUtf8ByteCount(ReadOnlySpan<byte> utf8)
        {
            var inputLength = utf8.Length;

            int byteCount = 0;
            while (byteCount < inputLength)
            {
                byte byte1 = utf8[byteCount];
                if ((byte1 & 0x80) == 0)
                {
                    byteCount += 1;
                }
                else if ((byte1 & 0xE0) == 0xC0)
                {
                    if (byteCount + 1 >= inputLength) return byteCount; //not enough bytes follow
                    byte byte2 = utf8[byteCount + 1];
                    if ((byte2 & 0x60) != 0x40) return byteCount;       // invalid byte sequence. Expect 10xx xxxx
                    byteCount += 2;
                }
                else if ((byte1 & 0xF0) == 0xE0)
                {
                    if (byteCount + 2 >= inputLength) return byteCount; //not enough bytes follow
                    byte byte2 = utf8[byteCount + 1];
                    byte byte3 = utf8[byteCount + 2];
                    if ((byte2 & 0x60) != 0x40) return byteCount;       // invalid byte sequence. Expect 10xx xxxx
                    if ((byte3 & 0x60) != 0x40) return byteCount;
                    byteCount += 3;
                }
                else if ((byte1 & 0xF8) == 0xF0)
                {
                    if (byteCount + 3 >= inputLength) return byteCount; //not enough bytes follow
                    byte byte2 = utf8[byteCount + 1];
                    byte byte3 = utf8[byteCount + 2];
                    byte byte4 = utf8[byteCount + 3];
                    if ((byte2 & 0x60) != 0x40) return byteCount;       // invalid byte sequence. Expect 10xx xxxx
                    if ((byte3 & 0x60) != 0x40) return byteCount;
                    if ((byte4 & 0x60) != 0x40) return byteCount;
                    byteCount += 4;
                }
                else
                {
                    return byteCount;   // invalid first byte
                }
            }
            return byteCount;
        }

        public static int GetUtf8ByteCount(ReadOnlySpan<char> utf16)
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

        public static int GetUtf8ByteCount(string utf16String)
        {
            return GetUtf8ByteCount(utf16String.AsSpan());
        }

        public static int GetUtf8ByteCount(ReadOnlySpan<uint> utf32)
        {
            int byteCount = 0;
            for (int i = 0; i < utf32.Length; i++)
            {
                uint codePoint = utf32[i];
                if (!IsSupportedCodePoint(codePoint))
                    return byteCount;
                else if (codePoint > TextEncoderConstants.Utf8ThreeBytesLastCodePoint)
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
        public static bool IsSupportedCodePoint(uint codePoint)
        {
            if (codePoint > TextEncoderConstants.LastValidCodePoint)
                return false;
            if (codePoint >= TextEncoderConstants.Utf16HighSurrogateFirstCodePoint && codePoint <= TextEncoderConstants.Utf16LowSurrogateLastCodePoint)
                return false;

            return true;
        }

        public static uint GenerateInvalidCodePoint(ref Random rnd)
        {
            int range = rnd.Next(0, 2);
            if (range == 0)
                return (uint)rnd.Next(0xD800, 0xE000); // Generate a random surrogate
            else
                return (uint)rnd.Next(0x110000, 0xFFFFFF); // Generate something above 0x110000
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
