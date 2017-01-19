using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8EncoderTests
    {
        public static object[][] TryEncodeFromUTF16ToUTF8TestData = {
            // empty
            new object[] { TextEncoder.Utf8, new byte[] { }, new ReadOnlySpan<char>(new char[]{ (char)0x0050 } ), false },
            // multiple bytes
            new object[] { TextEncoder.Utf8, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },
                new ReadOnlySpan<char>(new char[]{ (char)0x0050, (char)0x03E8, (char)0xAFC8, (char)0xD852, (char)0xDDF0 } ), true },
        };

        [Theory, MemberData("TryEncodeFromUTF16ToUTF8TestData")]
        public void UTF16ToUTF8EncodingTestForReadOnlySpanOfChar(TextEncoder encoder, byte[] expectedBytes, ReadOnlySpan<char> characters, bool expectedReturnVal)
        {
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
            new object[] { TextEncoder.Utf8, new byte[] { }, new ReadOnlySpan<uint>(new uint[] { 0x50 }), false },
            new object[] { TextEncoder.Utf16, new byte[] { }, new ReadOnlySpan<uint>(new uint[] { 0x50 }), false },
            // multiple bytes
            new object[] { TextEncoder.Utf8, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 }, new ReadOnlySpan<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD }, new ReadOnlySpan<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), true },
            // multiple bytes - buffer too small
            new object[] { TextEncoder.Utf8, new byte[] { 0x50 }, new ReadOnlySpan<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), false },
            new object[] { TextEncoder.Utf16, new byte[] { 0x50, 0x00 }, new ReadOnlySpan<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), false },
        };

        [Theory, MemberData("TryEncodeFromUnicodeMultipleCodePointsTestData")]
        public void TryEncodeFromUnicodeMultipleCodePoints(TextEncoder encoder, byte[] expectedBytes, ReadOnlySpan<uint> codePoints, bool expectedReturnVal)
        {
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
            new object[] { TextEncoder.Utf8, new Span<uint>(new uint[] {}), new Span<byte> (new byte[] {}), true },
            new object[] { TextEncoder.Utf16, new Span<uint>(new uint[] {}), new Span<byte> (new byte[] {}), true },
            // multiple bytes
            new object[] { TextEncoder.Utf8, new Span<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), new Span<byte> (new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 }), true },
            new object[] { TextEncoder.Utf16, new Span<uint>(
                new uint[] {
                    0x50,
                    0x3E8,
                    0xAFC8,
                    0x249F0 } ), new Span<byte> (new byte[] {  0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD }), true },
        };

        [Theory, MemberData("TryDecodeToUnicodeMultipleCodePointsTestData")]
        public void TryDecodeToUnicodeMultipleCodePoints(TextEncoder encoder, Span<uint> expectedCodePoints, Span<byte> inputBytes, bool expectedReturnVal)
        {
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
        
        public static object[][] Encoders = { new object[] { TextEncoder.Utf8 }, new object[] { TextEncoder.Utf16 } };

        [Theory, MemberData("Encoders")]
        public void BruteTestingRoundtripEncodeDecodeAllUnicodeCodePoints(TextEncoder encoder)
        {
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

        public static object[][] UsingBothEncoders = { new object[] { TextEncoder.Utf8, Encoding.UTF8 }, new object[] { TextEncoder.Utf16, Encoding.Unicode } };

        [Theory, MemberData("UsingBothEncoders")]
        public void BruteTestingEncodeAllUnicodeCodePoints(TextEncoder encoder, Encoding systemTextEncoder)
        {
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
            ReadOnlySpan<char> characters = unicodeString.Slice();
            int byteCount = systemTextEncoder.GetByteCount(unicodeString);
            byte[] buff = new byte[byteCount];
            Span<byte> expectedBuffer;
            char[] charArray = characters.ToArray();

            systemTextEncoder.GetBytes(charArray, 0, characters.Length, buff, 0);
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

        public static object[][] EnsureCodeUnitsOfStringTestCases = {
            // empty
            new object[] { new byte[0], default(Utf8String) },
            new object[] { new byte[0], Utf8String.Empty },
            new object[] { new byte[0], new Utf8String("")},
            // ascii
            new object[] { new byte[] { 0x61 }, new Utf8String("a")},
            new object[] { new byte[] { 0x61, 0x62, 0x63 }, new Utf8String("abc")},
            new object[] { new byte[] { 0x41, 0x42, 0x43, 0x44 }, new Utf8String("ABCD")},
            new object[] { new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 }, new Utf8String("01234")},
            new object[] { new byte[] { 0x20, 0x2c, 0x2e, 0x0d, 0x0a, 0x5b, 0x5d, 0x3c, 0x3e, 0x28, 0x29 },  new Utf8String(" ,.\r\n[]<>()")},
            // edge cases for multibyte characters
            new object[] { new byte[] { 0x7f }, new Utf8String("\u007f")},
            new object[] { new byte[] { 0xc2, 0x80 }, new Utf8String("\u0080")},
            new object[] { new byte[] { 0xdf, 0xbf }, new Utf8String("\u07ff")},
            new object[] { new byte[] { 0xe0, 0xa0, 0x80 }, new Utf8String("\u0800")},
            new object[] { new byte[] { 0xef, 0xbf, 0xbf }, new Utf8String("\uffff")},
            // ascii mixed with multibyte characters
            // 1 code unit + 2 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80 }, new Utf8String("a\u0080")},
            // 2 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0x61 }, new Utf8String("\u0080a")},
            // 1 code unit + 2 code units + 1 code unit
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61 }, new Utf8String("a\u0080a")},
            // 3 code units + 2 code units
            new object[] { new byte[] { 0xe0, 0xa0, 0x80, 0xc2, 0x80 }, new Utf8String("\u0800\u0080")},
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0xe0, 0xa0, 0x80 }, new Utf8String("\u0080\u0800")},
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf }, new Utf8String("\u0080a\uffff")},
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0xef, 0xbf, 0xbf }, new Utf8String("a\u0080\uffff")},
            // 2 code units + 3 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0xef, 0xbf, 0xbf, 0x61 }, new Utf8String("\u0080\uffffa")},
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf, 0x61 }, new Utf8String("a\u0080a\uffffa")}
            // TODO: Add case with 4 byte character - it is impossible to do using string literals, need to create it using code point
        };
        [Theory, MemberData("EnsureCodeUnitsOfStringTestCases")]
        public void EnsureCodeUnitsOfStringByEnumeratingBytes(byte[] expectedBytes, Utf8String utf8String)
        {
            Assert.Equal(expectedBytes.Length, utf8String.Length);
            Utf8String.Enumerator e = utf8String.GetEnumerator();

            int i = 0;
            while (e.MoveNext())
            {
                Assert.True(i < expectedBytes.Length);
                Assert.Equal(expectedBytes[i], (byte)e.Current);
                i++;
            }
            Assert.Equal(expectedBytes.Length, i);
        }

        [Theory, MemberData("EnsureCodeUnitsOfStringTestCases")]
        public void EnsureCodeUnitsOfStringByIndexingBytes(byte[] expectedBytes, Utf8String utf8String)
        {
            Assert.Equal(expectedBytes.Length, utf8String.Length);

            for (int i = 0; i < utf8String.Length; i++)
            {
                Assert.Equal(expectedBytes[i], (byte)utf8String[i]);
            }
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
            int written;
            int consumed;

            var input = new Span<byte>(inputBytes);
            var output = new Span<byte>(new byte[outputSize]);

            Assert.False(TextEncoder.Utf16.TryEncode(input, output, out consumed, out written));
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected1, output.Slice(0, written).ToArray()));

            input = input.Slice(consumed);
            output = new Span<byte>(new byte[expected2.Length]);
            Assert.True(TextEncoder.Utf16.TryEncode(input, output, out consumed, out written));
            Assert.Equal(expected2.Length, written);
            Assert.Equal(inputBytes.Length - expectedConsumed, consumed);
            Assert.True(AreByteArraysEqual(expected2, output.ToArray()));
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
