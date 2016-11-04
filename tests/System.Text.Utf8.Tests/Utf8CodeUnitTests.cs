using System.Collections.Generic;
using System.Linq;
using System.Text.Utf16;
using Xunit;
using Xunit.Abstractions;

namespace System.Text.Utf8.Tests
{
    public class Utf8CodeUnitTests
    {
        private const byte b0000_0000 = 0;
        private const byte b0011_0000 = 48;
        private const byte b0111_1111 = 127;
        private const byte b1000_0000 = 128;
        private const byte b1001_1001 = 153;
        private const byte b1011_1111 = 191;
        private const byte b1100_0000 = 192;
        private const byte b1101_0000 = 208;
        private const byte b1111_1111 = 255;

        public static object[][] IsFirstCodeUnitInEncodedCodePointTestCases = new object[][] {
            // the only forbidden range is binary 1000 0000 - 1011 1111
            new object[] { true, b0000_0000 },
            new object[] { true, b0011_0000 },
            new object[] { true, b0111_1111 },

            new object[] { false, b1000_0000 },
            new object[] { false, b1001_1001 },
            new object[] { false, b1011_1111 },

            new object[] { true, b1100_0000 },
            new object[] { true, b1101_0000 },
            new object[] { true, b1111_1111 }
        };

        [Theory, MemberData("IsFirstCodeUnitInEncodedCodePointTestCases")]
        public void IsFirstCodeUnitInEncodedCodePoint(bool expected, byte codeUnit)
        {
            Assert.Equal(expected, Utf8CodeUnit.IsFirstCodeUnitInEncodedCodePoint(codeUnit));
        }

        public static object[][] IsAsciiTestCases = new object[][] {
            new object[] { true, (byte)0 },
            new object[] { true, (byte)35 },
            new object[] { true, (byte)127 },

            new object[] { false, (byte)128 },
            new object[] { false, (byte)150 },
            new object[] { false, (byte)255 }
        };

        [Theory, MemberData("IsAsciiTestCases")]
        public void IsAscii(bool expected, byte codeUnit)
        {
            Assert.Equal(expected, Utf8CodeUnit.IsAscii(codeUnit));
        }


        public static object[][] CtorCharTestCases = new object[][] {
            new object[] { false, (char)0 },
            new object[] { false, 'a' },
            new object[] { false, (char)127 },
            new object[] { true, (char)128 },
            new object[] { true, '\uABCD' }
        };

        [Theory, MemberData("CtorCharTestCases")]
        public void TryCreateFrom(bool expectedNegated, char value)
        {
            bool expected = !expectedNegated;

            byte codeUnit;
            Assert.Equal(expected, Utf8CodeUnit.TryCreateFrom(value, out codeUnit));
            if (expected)
            {
                Assert.Equal((int)value, (int)codeUnit);
            }
        }
    }
}
