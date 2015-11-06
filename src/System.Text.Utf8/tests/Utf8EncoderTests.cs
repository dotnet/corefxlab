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

        public static object[][] EnsureCodeUnitsOfStringTestCases = {
            // empty
            new object[] { new byte[0], default(Utf8String) },
            new object[] { new byte[0], Utf8String.Empty },
            new object[] { new byte[0], ""u8 },
            // ascii
            new object[] { new byte[] { 0x61 }, "a"u8 },
            new object[] { new byte[] { 0x61, 0x62, 0x63 }, "abc"u8 },
            new object[] { new byte[] { 0x41, 0x42, 0x43, 0x44 }, "ABCD"u8 },
            new object[] { new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 }, "01234"u8 },
            new object[] { new byte[] { 0x20, 0x2c, 0x2e, 0x0d, 0x0a, 0x5b, 0x5d, 0x3c, 0x3e, 0x28, 0x29 },  " ,.\r\n[]<>()"u8 },
            // edge cases for multibyte characters
            new object[] { new byte[] { 0x7f }, "\u007f"u8 },
            new object[] { new byte[] { 0xc2, 0x80 }, "\u0080"u8 },
            new object[] { new byte[] { 0xdf, 0xbf }, "\u07ff"u8 },
            new object[] { new byte[] { 0xe0, 0xa0, 0x80 }, "\u0800"u8 },
            new object[] { new byte[] { 0xef, 0xbf, 0xbf }, "\uffff"u8 },
            // ascii mixed with multibyte characters
            // 1 code unit + 2 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80 }, "a\u0080"u8 },
            // 2 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0x61 }, "\u0080a"u8 },
            // 1 code unit + 2 code units + 1 code unit
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61 }, "a\u0080a"u8 },
            // 3 code units + 2 code units
            new object[] { new byte[] { 0xe0, 0xa0, 0x80, 0xc2, 0x80 }, "\u0800\u0080"u8 },
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0xe0, 0xa0, 0x80 }, "\u0080\u0800"u8 },
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf }, "\u0080a\uffff"u8 },
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0xef, 0xbf, 0xbf }, "a\u0080\uffff"u8 },
            // 2 code units + 3 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0xef, 0xbf, 0xbf, 0x61 }, "\u0080\uffffa"u8 },
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf, 0x61 }, "a\u0080a\uffffa"u8 }
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
