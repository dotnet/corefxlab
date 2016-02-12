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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(255)]
        public void CtorByte(byte value)
        {
            Assert.Equal(value, (new Utf8CodeUnit(value)).Value);
        }

        public static object[][] EqualsUtf8CodeUnitTestCases = new object[][] {
            new object[] { true, new Utf8CodeUnit(0), new Utf8CodeUnit(0) },
            new object[] { true, new Utf8CodeUnit(1), new Utf8CodeUnit(1) },
            new object[] { true, new Utf8CodeUnit(127), new Utf8CodeUnit(127) },
            new object[] { true, new Utf8CodeUnit(128), new Utf8CodeUnit(128) },
            new object[] { true, new Utf8CodeUnit(255), new Utf8CodeUnit(255) },

            new object[] { false, new Utf8CodeUnit(0), new Utf8CodeUnit(1) },
            new object[] { false, new Utf8CodeUnit(127), new Utf8CodeUnit(128) },
            new object[] { false, new Utf8CodeUnit(255), new Utf8CodeUnit(254) },
            new object[] { false, new Utf8CodeUnit(255), new Utf8CodeUnit(0) }
        };

        [Theory, MemberData("EqualsUtf8CodeUnitTestCases")]
        public void EqualsUtf8CodeUnit(bool expected, Utf8CodeUnit a, Utf8CodeUnit b)
        {
            Assert.Equal(expected, a.Equals(b));
            Assert.Equal(expected, b.Equals(a));
        }

        [Theory, MemberData("EqualsUtf8CodeUnitTestCases")]
        public void EqualsBoxedUtf8CodeUnit(bool expected, Utf8CodeUnit a, Utf8CodeUnit b)
        {
            Assert.Equal(expected, a.Equals((object)b));
            Assert.Equal(expected, b.Equals((object)a));
        }

        public static object[][] EqualsBoxedCharTestCases = new object[][] {
            new object[] { true, new Utf8CodeUnit(48), (char)48 },
            new object[] { false, new Utf8CodeUnit(48), (char)49 },
            new object[] { true, new Utf8CodeUnit(127), (char)127 },
            new object[] { false, new Utf8CodeUnit(128), (char)128 },
            new object[] { false, new Utf8CodeUnit(128), '\uABCD' }
        };

        [Theory, MemberData("EqualsBoxedCharTestCases")]
        public void EqualsBoxedChar(bool expected, Utf8CodeUnit codeUnit, char c)
        {
            Assert.Equal(expected, codeUnit.Equals((object)c));
        }

        public static object[][] EqualsAnyOtherObjectTestCases = new object[][] {
            new object[] { false, new Utf8CodeUnit(128), new object() },
            new object[] { false, new Utf8CodeUnit(48), 48 },
            new object[] { false, new Utf8CodeUnit(48), 49 },
            new object[] { false, new Utf8CodeUnit(48), "48" }
        };

        [Theory, MemberData("EqualsAnyOtherObjectTestCases")]
        public void EqualsAnyOtherObject(bool expected, Utf8CodeUnit codeUnit, object obj)
        {
            Assert.Equal(expected, codeUnit.Equals(obj));
        }

        [Fact]
        public void GetHashCodeIsDifferentForEveryPossibleCodeUnit()
        {
            HashSet<int> hashes = new HashSet<int>();
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                Utf8CodeUnit codeUnit = new Utf8CodeUnit((byte)i);
                Assert.True(hashes.Add(codeUnit.GetHashCode()));
            }

            // sanity
            Assert.Equal(256, hashes.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(255)]
        public void ConvertFromByte(byte value)
        {
            Assert.Equal(value, ((Utf8CodeUnit)value).Value);
        }

        public static object[][] ConvertToByteTestCases = new object[][] {
            new object[] { new Utf8CodeUnit(128) },
            new object[] { new Utf8CodeUnit(48) },
            new object[] { new Utf8CodeUnit(48) },
            new object[] { new Utf8CodeUnit(48) }
        };

        [Theory, MemberData("ConvertToByteTestCases")]
        public void ConvertToByte(Utf8CodeUnit codeUnit)
        {
            Assert.Equal(codeUnit.Value, (byte)codeUnit);
        }

        [Theory, MemberData("EqualsUtf8CodeUnitTestCases")]
        public void EqualsOperator(bool expected, Utf8CodeUnit a, Utf8CodeUnit b)
        {
            Assert.Equal(expected, a == b);
            Assert.Equal(expected, b == a);
        }

        [Theory, MemberData("EqualsUtf8CodeUnitTestCases")]
        public void NotEqualsOperator(bool expectedNegated, Utf8CodeUnit a, Utf8CodeUnit b)
        {
            Assert.Equal(!expectedNegated, a != b);
            Assert.Equal(!expectedNegated, b != a);
        }

        public static object[][] IsFirstCodeUnitInEncodedCodePointTestCases = new object[][] {
            // the only forbidden range is binary 1000 0000 - 1011 1111
            new object[] { true, new Utf8CodeUnit(b0000_0000) },
            new object[] { true, new Utf8CodeUnit(b0011_0000) },
            new object[] { true, new Utf8CodeUnit(b0111_1111) },

            new object[] { false, new Utf8CodeUnit(b1000_0000) },
            new object[] { false, new Utf8CodeUnit(b1001_1001) },
            new object[] { false, new Utf8CodeUnit(b1011_1111) },

            new object[] { true, new Utf8CodeUnit(b1100_0000) },
            new object[] { true, new Utf8CodeUnit(b1101_0000) },
            new object[] { true, new Utf8CodeUnit(b1111_1111) }
        };

        [Theory, MemberData("IsFirstCodeUnitInEncodedCodePointTestCases")]
        public void IsFirstCodeUnitInEncodedCodePoint(bool expected, Utf8CodeUnit codeUnit)
        {
            Assert.Equal(expected, Utf8CodeUnit.IsFirstCodeUnitInEncodedCodePoint(codeUnit));
        }

        public static object[][] IsAsciiTestCases = new object[][] {
            new object[] { true, new Utf8CodeUnit(0) },
            new object[] { true, new Utf8CodeUnit(35) },
            new object[] { true, new Utf8CodeUnit(127) },

            new object[] { false, new Utf8CodeUnit(128) },
            new object[] { false, new Utf8CodeUnit(150) },
            new object[] { false, new Utf8CodeUnit(255) }
        };

        [Theory, MemberData("IsAsciiTestCases")]
        public void IsAscii(bool expected, Utf8CodeUnit codeUnit)
        {
            Assert.Equal(expected, Utf8CodeUnit.IsAscii(codeUnit));
        }

        #region Char conversion APIs
        public static object[][] CtorCharTestCases = new object[][] {
            new object[] { false, (char)0 },
            new object[] { false, 'a' },
            new object[] { false, (char)127 },
            new object[] { true, (char)128 },
            new object[] { true, '\uABCD' }
        };

        [Theory, MemberData("CtorCharTestCases")]
        public void CtorChar(bool shouldThrow, char value)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var codeUnit = new Utf8CodeUnit(value); });
            }
            else
            {
                Assert.Equal((byte)value, (new Utf8CodeUnit(value)).Value);
            }
        }

        [Theory, MemberData("EqualsBoxedCharTestCases")]
        public void EqualsChar(bool expected, Utf8CodeUnit codeUnit, char c)
        {
            Assert.Equal(expected, codeUnit.Equals(c));
        }

        [Theory, MemberData("CtorCharTestCases")]
        public void ConvertFromChar(bool shouldThrow, char value)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var codeUnit = (Utf8CodeUnit)value; });
            }
            else
            {
                Assert.Equal((byte)value, ((Utf8CodeUnit)value).Value);
            }
        }

        public static object[][] ConvertToCharTestCases = new object[][] {
            new object[] { false, new Utf8CodeUnit(b0000_0000) },
            new object[] { false, new Utf8CodeUnit(b0011_0000) },
            new object[] { false, new Utf8CodeUnit(b0111_1111) },

            new object[] { true, new Utf8CodeUnit(b1000_0000) },
            new object[] { true, new Utf8CodeUnit(b1001_1001) },
            new object[] { true, new Utf8CodeUnit(b1011_1111) },

            new object[] { true, new Utf8CodeUnit(b1100_0000) },
            new object[] { true, new Utf8CodeUnit(b1101_0000) },
            new object[] { true, new Utf8CodeUnit(b1111_1111) }
        };

        [Theory, MemberData("ConvertToCharTestCases")]
        public void ConvertToChar(bool shouldThrow, Utf8CodeUnit codeUnit)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var c = (char)codeUnit; });
            }
            else
            {
                Assert.Equal((int)codeUnit.Value, (int)((char)codeUnit));
            }
        }

        [Theory, MemberData("CtorCharTestCases")]
        public void TryCreateFrom(bool expectedNegated, char value)
        {
            bool expected = !expectedNegated;

            Utf8CodeUnit codeUnit;
            Assert.Equal(expected, Utf8CodeUnit.TryCreateFrom(value, out codeUnit));
            if (expected)
            {
                Assert.Equal((int)value, (int)codeUnit.Value);
            }
        }
        #endregion
    }
}
