using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8EncoderTests
    {
        [Fact]
        public void Utf8EncodedCodePointFromChar0()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u0000');
            Assert.Equal(1, ecp.Length);
            Assert.Equal(0x00, ecp.Byte0);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar1()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u007F');
            Assert.Equal(1, ecp.Length);
            Assert.Equal(0x7F, ecp.Byte0);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar2()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u0080');
            Assert.Equal(2, ecp.Length);
            Assert.Equal(0xC2, ecp.Byte0);
            Assert.Equal(0x80, ecp.Byte1);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar3()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u01ED');
            Assert.Equal(2, ecp.Length);
            Assert.Equal(0xC7, ecp.Byte0);
            Assert.Equal(0xAD, ecp.Byte1);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar4()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u07FF');
            Assert.Equal(2, ecp.Length);
            Assert.Equal(0xDF, ecp.Byte0);
            Assert.Equal(0xBF, ecp.Byte1);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar5()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u0800');
            Assert.Equal(3, ecp.Length);
            Assert.Equal(0xE0, ecp.Byte0);
            Assert.Equal(0xA0, ecp.Byte1);
            Assert.Equal(0x80, ecp.Byte2);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar6()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\u1FA9');
            Assert.Equal(3, ecp.Length);
            Assert.Equal(0xE1, ecp.Byte0);
            Assert.Equal(0xBE, ecp.Byte1);
            Assert.Equal(0xA9, ecp.Byte2);
        }

        [Fact]
        public void Utf8EncodedCodePointFromChar7()
        {
            Utf8EncodedCodePoint ecp = new Utf8EncodedCodePoint('\uFFFF');
            Assert.Equal(3, ecp.Length);
            Assert.Equal(0xEF, ecp.Byte0);
            Assert.Equal(0xBF, ecp.Byte1);
            Assert.Equal(0xBF, ecp.Byte2);
        }

        public static object[][] TryEncodeFromUnicodeCodePointTestData = {
            // empty
            new object[] { TextEncoder.Utf8, new byte[] { }, new UnicodeCodePoint(0x50), false },
            new object[] { TextEncoder.Utf16, new byte[] { }, new UnicodeCodePoint(0x50), false },
            // 1 byte
            new object[] { TextEncoder.Utf8, new byte[] { 0x50 }, new UnicodeCodePoint(0x50), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0x50, 0x00 }, new UnicodeCodePoint(0x50), true },
            // 2 bytes
             new object[] { TextEncoder.Utf8, new byte[] { 0xCF, 0xA8 }, new UnicodeCodePoint(0x3E8), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0xE8, 0x03 }, new UnicodeCodePoint(0x3E8), true },
            // 3 bytes
            new object[] { TextEncoder.Utf8, new byte[] { 0xEA, 0xBF, 0x88 }, new UnicodeCodePoint(0xAFC8), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0xC8, 0xAF }, new UnicodeCodePoint(0xAFC8), true },
            // 4 bytes
            new object[] { TextEncoder.Utf8, new byte[] { 0xF0, 0xA4, 0xA7, 0xB0 }, new UnicodeCodePoint(0x249F0), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0x52, 0xD8, 0xF0, 0xDD }, new UnicodeCodePoint(0x249F0), true },
            // 4 bytes - buffer too small
            new object[] { TextEncoder.Utf8, new byte[] { 0xF0, 0xA4, 0xA7 }, new UnicodeCodePoint(0x249F0), false },
            new object[] { TextEncoder.Utf16, new byte[] { 0x52, 0xD8, 0xF0 }, new UnicodeCodePoint(0x249F0), false },
        };

        [Theory, MemberData("TryEncodeFromUnicodeCodePointTestData")]
        public void TryEncodeFromUnicodeCodePoint(TextEncoder encoder, byte[] expectedBytes, UnicodeCodePoint codepoint, bool expectedReturnVal)
        {
            Span<byte> buffer = new Span<byte>(new byte[expectedBytes.Length]);
            int bytesWritten;
            
            Assert.Equal(expectedReturnVal, encoder.TryEncodeFromUnicode(codepoint, buffer, out bytesWritten));
            Assert.Equal(expectedReturnVal ? expectedBytes.Length : 0, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.True(AreByteArraysEqual(expectedBytes, buffer.ToArray()));
            }
        }

        public static object[][] TryEncodeFromUnicodeMultipleCodePointsTestData = {
            // empty
            new object[] { TextEncoder.Utf8, new byte[] { }, new ReadOnlySpan<UnicodeCodePoint>(new UnicodeCodePoint[] { new UnicodeCodePoint(0x50) }), false },
            new object[] { TextEncoder.Utf16, new byte[] { }, new ReadOnlySpan<UnicodeCodePoint>(new UnicodeCodePoint[] { new UnicodeCodePoint(0x50) }), false },
            // multiple bytes
            new object[] { TextEncoder.Utf8, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 }, new ReadOnlySpan<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), true },
            new object[] { TextEncoder.Utf16, new byte[] { 0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD }, new ReadOnlySpan<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), true },
            // multiple bytes - buffer too small
            new object[] { TextEncoder.Utf8, new byte[] { 0x50 }, new ReadOnlySpan<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), false },
            new object[] { TextEncoder.Utf16, new byte[] { 0x50 }, new ReadOnlySpan<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), false }, 
        };

        [Theory, MemberData("TryEncodeFromUnicodeMultipleCodePointsTestData")]
        public void TryEncodeFromUnicodeMultipleCodePoints(TextEncoder encoder, byte[] expectedBytes, ReadOnlySpan<UnicodeCodePoint> codePoints, bool expectedReturnVal)
        {
            Span<byte> buffer = new Span<byte>(new byte[expectedBytes.Length]);
            int bytesWritten;

            Assert.Equal(expectedReturnVal, encoder.TryEncodeFromUnicode(codePoints, buffer, out bytesWritten));
            Assert.Equal(expectedReturnVal ? expectedBytes.Length : 0, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.True(AreByteArraysEqual(expectedBytes, buffer.ToArray()));
            }
        }

        public static object[][] TryDecodeToUnicodeCodePointTestData = {
            //empty
            new object[] { TextEncoder.Utf8, new UnicodeCodePoint(0x50), new Span<byte> (new byte[] {}),  false },
            new object[] { TextEncoder.Utf16, new UnicodeCodePoint(0x50), new Span<byte> (new byte[] {}),  false },
            // 1 byte
            new object[] { TextEncoder.Utf8, new UnicodeCodePoint(0x50), new Span<byte> (new byte[] { 0x50 }),  true },
            new object[] { TextEncoder.Utf16, new UnicodeCodePoint(0x50), new Span<byte> (new byte[] { 0x50, 0x00 }),  true },
            // 2 bytes
            new object[] { TextEncoder.Utf8, new UnicodeCodePoint(0x3E8), new Span<byte> (new byte[] { 0xCF, 0xA8 }),  true },
            new object[] { TextEncoder.Utf16, new UnicodeCodePoint(0x3E8), new Span<byte> (new byte[] { 0xE8, 0x03 }),  true },
            // 3 bytes
            new object[] { TextEncoder.Utf8, new UnicodeCodePoint(0xAFC8), new Span<byte> (new byte[] { 0xEA, 0xBF, 0x88 }),  true },
            new object[] { TextEncoder.Utf16, new UnicodeCodePoint(0xAFC8), new Span<byte> (new byte[] { 0xC8, 0xAF }),  true },
            // 4 bytes
            new object[] { TextEncoder.Utf8, new UnicodeCodePoint(0x249F0), new Span<byte> (new byte[] { 0xF0, 0xA4, 0xA7, 0xB0 }),  true },
            new object[] { TextEncoder.Utf16, new UnicodeCodePoint(0x249F0), new Span<byte> (new byte[] { 0x52, 0xD8, 0xF0, 0xDD }),  true },
        };

        [Theory, MemberData("TryDecodeToUnicodeCodePointTestData")]
        public void TryDecodeToUnicodeCodePoint(TextEncoder encoder, UnicodeCodePoint expectedCodePoint, Span<byte> inputBytes, bool expectedReturnVal)
        {
            UnicodeCodePoint codePoint = new UnicodeCodePoint();
            int bytesWritten;
            
            Assert.Equal(expectedReturnVal, encoder.TryDecodeToUnicode(inputBytes, out codePoint, out bytesWritten));
            Assert.Equal(expectedReturnVal ? inputBytes.Length : 0, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.True(expectedCodePoint.Equals(codePoint));
            }
        }

        public static object[][] TryDecodeToUnicodeMultipleCodePointsTestData = {
            //empty
            new object[] { TextEncoder.Utf8, new Span<UnicodeCodePoint>(new UnicodeCodePoint[] { new UnicodeCodePoint(0x50) }), new Span<byte> (new byte[] {}),  false },
            new object[] { TextEncoder.Utf16, new Span<UnicodeCodePoint>(new UnicodeCodePoint[] { new UnicodeCodePoint(0x50) }), new Span<byte> (new byte[] {}),  false },
            // multiple bytes
            new object[] { TextEncoder.Utf8, new Span<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), new Span<byte> (new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 }), true },
            new object[] { TextEncoder.Utf16, new Span<UnicodeCodePoint>(
                new UnicodeCodePoint[] {
                    new UnicodeCodePoint(0x50),
                    new UnicodeCodePoint(0x3E8),
                    new UnicodeCodePoint(0xAFC8),
                    new UnicodeCodePoint(0x249F0) } ), new Span<byte> (new byte[] {  0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD }), true },
        };

        [Theory, MemberData("TryDecodeToUnicodeMultipleCodePointsTestData")]
        public void TryDecodeToUnicodeMultipleCodePoints(TextEncoder encoder, Span<UnicodeCodePoint> expectedCodePoints, Span<byte> inputBytes, bool expectedReturnVal)
        {
            Span<UnicodeCodePoint> codePoints = new Span<UnicodeCodePoint>(new UnicodeCodePoint[expectedCodePoints.Length]);
            int bytesWritten;

            Assert.Equal(expectedReturnVal, encoder.TryDecodeToUnicode(inputBytes, codePoints, out bytesWritten));
            Assert.Equal(expectedReturnVal ? inputBytes.Length : 0, bytesWritten);

            if (expectedReturnVal)
            {
                Assert.True(AreCodePointArraysEqual(expectedCodePoints.ToArray(), codePoints.ToArray()));
            }

        }
        
        public static object[][] Encoders = { new object[] { TextEncoder.Utf8 }, new object[] { TextEncoder.Utf16 } };

        [Theory, MemberData("Encoders")]
        public void BruteTestingRoundtripEncodeDecodeAllUnicodeCodePoints(TextEncoder encoder)
        {
            for (int i = 0; i < 0x10FFFF; i++)
            {
                if (i >= 0xD800 && i <= 0xDFFF) continue; // skip surrogate characters

                Span<byte> buffer = new Span<byte>(new byte[4]);
                int bytesWritten;
                UnicodeCodePoint expectedCodePoint = new UnicodeCodePoint((uint)i);
                UnicodeCodePoint codePoint = new UnicodeCodePoint();
                Assert.True(encoder.TryEncodeFromUnicode(expectedCodePoint, buffer, out bytesWritten));

                if (encoder == TextEncoder.Utf16 && i <= 0xFFFF)
                {
                    Assert.True(encoder.TryDecodeToUnicode(buffer.Slice(0,2), out codePoint, out bytesWritten));
                }
                else
                {
                    Assert.True(encoder.TryDecodeToUnicode(buffer, out codePoint, out bytesWritten));
                }
                Assert.True(expectedCodePoint.Equals(codePoint));
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

        public bool AreCodePointArraysEqual(UnicodeCodePoint[] arrayOne, UnicodeCodePoint[] arrayTwo)
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
    }
}
