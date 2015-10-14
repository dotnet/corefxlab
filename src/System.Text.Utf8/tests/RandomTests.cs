using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Utf8.Tests
{
    public class Utf8StringTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Length(string s)
        {
            Assert.Equal(s.Length, (new Utf8String(Encoding.UTF8.GetBytes(s))).CodePoints.Count());
        }

        [Theory]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void ToStringTest(string s)
        {
            var utf8string = new Utf8String(Encoding.UTF8.GetBytes(s));
            Assert.Equal(s, utf8string.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void CodePointValidation(string s)
        {
            var utf8string = new Utf8String(Encoding.UTF8.GetBytes(s));
            IEnumerator<UnicodeCodePoint> codePoints = utf8string.CodePoints.GetEnumerator();
            for (int i = 0; i < s.Length; i++)
            {
                Assert.True(codePoints.MoveNext());
                Assert.Equal((uint)s[i], (uint)codePoints.Current);
            }

            Assert.False(codePoints.MoveNext());
        }

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

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        public void AsciiStringEnumerators(string s)
        {
            Utf8String u8s = new Utf8String(Encoding.UTF8.GetBytes(s));
            Utf8String.Enumerator e = u8s.GetEnumerator();
            Utf8String.CodePointEnumerator cpe = u8s.CodePoints.GetEnumerator();

            Assert.Equal(s.Length, u8s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                Assert.True(e.MoveNext());
                Assert.True(cpe.MoveNext());
                Assert.Equal((byte)s[i], (byte)u8s[i]);
                Assert.Equal(u8s[i], e.Current);
                Assert.Equal((byte)s[i], (byte)(uint)cpe.Current);
            }
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void RoundTrip(string s)
        {
            Utf8String u8s = new Utf8String(s);
            Assert.Equal(s, u8s.ToString());
        }
		
        [Theory]
        [InlineData(true, "test123", "test123")]
        [InlineData(false, "test123", "test124")]
        public unsafe void StringEquals(bool equal, string s1, string s2)
        {
            byte[] b1 = Encoding.UTF8.GetBytes(s1);
            byte[] b2 = Encoding.UTF8.GetBytes(s2);
            Utf8String s1FromBytes = new Utf8String(b1);
            Utf8String s2FromBytes = new Utf8String(b2);
            fixed (byte* b1pinned = b1)
            fixed (byte* b2pinned = b2)
            {
                ByteSpan sp1 = new ByteSpan(b1pinned, b1.Length);
                ByteSpan sp2 = new ByteSpan(b2pinned, b2.Length);
                Utf8String s1FromSpan = new Utf8String(sp1);
                Utf8String s2FromSpan = new Utf8String(sp2);

                Assert.Equal(equal, s1FromBytes == s2FromBytes);
                Assert.Equal(equal, s1FromBytes == s2FromSpan);
                Assert.Equal(equal, s1FromBytes == s2);

                Assert.Equal(equal, s1FromSpan == s2FromBytes);
                Assert.Equal(equal, s1FromSpan == s2FromSpan);
                Assert.Equal(equal, s1FromSpan == s2);

                Assert.Equal(equal, s1 == s2FromBytes);
                Assert.Equal(equal, s1 == s2FromSpan);


                Assert.Equal(equal, s1FromBytes.Equals(s2FromBytes));
                Assert.Equal(equal, s1FromBytes.Equals(s2FromSpan));
                Assert.Equal(equal, s1FromBytes.Equals(s2));

                Assert.Equal(equal, s1FromSpan.Equals(s2FromBytes));
                Assert.Equal(equal, s1FromSpan.Equals(s2FromSpan));
                Assert.Equal(equal, s1FromSpan.Equals(s2));

                Assert.Equal(equal, s1.EqualsUtf8String(s2FromBytes));
                Assert.Equal(equal, s1.EqualsUtf8String(s2FromSpan));
            }
        }
    }
}
