using System.Collections.Generic;
using System.Linq;
using System.Text.Utf16;
using Xunit;
using Xunit.Abstractions;

namespace System.Text.Utf8.Tests
{
    public class Utf8StringTests
    {
        #region Helpers - useful only when debugging or generating test cases
        ITestOutputHelper output;

        public Utf8StringTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static string GetStringLiteral(string s)
        {
            if (s == null)
            {
                return "null";
            }

            Utf16LittleEndianCodePointEnumerable codePoints = new Utf16LittleEndianCodePointEnumerable(s);
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            foreach (var codePoint in codePoints)
            {
                if (codePoint.Value >= 32 && codePoint.Value < 127)
                {
                    sb.Append(char.ConvertFromUtf32(unchecked((int)codePoint.Value)));
                }
                else if (codePoint.Value == (uint)'\n')
                {
                    sb.Append("\\n");
                }
                else if (codePoint.Value == (uint)'\r')
                {
                    sb.Append("\\r");
                }
                else if (codePoint.Value == (uint)'\t')
                {
                    sb.Append("\\t");
                }
                else
                {
                    sb.Append(string.Format("\\u{0:X04}", codePoint.Value));
                }
            }
            sb.Append('"');
            return sb.ToString();
        }
        #endregion

        public static object[][] LengthTestCases = {
            new object[] { 0, default(Utf8String) },
            new object[] { 0, Utf8String.Empty },
            new object[] { 0, new Utf8String("")},
            new object[] { 4, new Utf8String("1258")},
            new object[] { 3, new Utf8String("\uABCD")},
            new object[] { 4, new Utf8String("a\uABEE")},
            new object[] { 5, new Utf8String("a\uABEEa")},
            new object[] { 8, new Utf8String("a\uABEE\uABCDa")}
        };

        [Theory, MemberData("LengthTestCases")]
        public void Length(int expectedLength, Utf8String s)
        {
            Assert.Equal(expectedLength, s.Length);
        }

        public static object[][] LengthInCodePointsTestCases = {
            new object[] { 0, default(Utf8String) },
            new object[] { 0, Utf8String.Empty },
            new object[] { 0, new Utf8String("")},
            new object[] { 4, new Utf8String("1258")},
            new object[] { 1, new Utf8String("\uABCD")},
            new object[] { 2, new Utf8String("a\uABEE")},
            new object[] { 3, new Utf8String("a\uABEEa")},
            new object[] { 4, new Utf8String("a\uABEE\uABCDa")}
        };

        [Theory, MemberData("LengthInCodePointsTestCases")]
        public void LengthInCodePoints(int expectedLength, Utf8String s)
        {
            Assert.Equal(expectedLength, s.CodePoints.Count());
        }

        public static object[][] ToStringTestCases = {
            new object[] { "", default(Utf8String) },
            new object[] { "", Utf8String.Empty },
            new object[] { "", new Utf8String("")},
            new object[] { "1258", new Utf8String("1258")},
            new object[] { "\uABCD", new Utf8String("\uABCD")},
            new object[] { "a\uABEE", new Utf8String("a\uABEE")},
            new object[] { "a\uABEEa", new Utf8String("a\uABEEa")},
            new object[] { "a\uABEE\uABCDa", new Utf8String("a\uABEE\uABCDa")}
        };

        [Theory, MemberData("ToStringTestCases")]
        public void ToString(string expected, Utf8String s)
        {
            Assert.Equal(expected, s.ToString());
        }

        public static object[][] StringEqualsTestCases_EmptyStrings = new object[][] {
            new object[] { true, new Utf8String(""), new Utf8String("")},
            new object[] { true, new Utf8String(""), default(Utf8String) },
            new object[] { true, new Utf8String(""), Utf8String.Empty },
            new object[] { true, new Utf8String(""), new Utf8String("") },
            new object[] { true, default(Utf8String), default(Utf8String) },
            new object[] { true, default(Utf8String), Utf8String.Empty },
            new object[] { true, default(Utf8String), new Utf8String("") },
            new object[] { true, Utf8String.Empty, Utf8String.Empty },
            new object[] { true, Utf8String.Empty, new Utf8String("") },
            new object[] { true, new Utf8String(""), new Utf8String("") },

            new object[] { false, new Utf8String(""), new Utf8String(" ")},
            new object[] { false, new Utf8String(""), new Utf8String("a")},
            new object[] { false, new Utf8String(""), new Utf8String("\uABCD")},
            new object[] { false, new Utf8String(""), new Utf8String("abc")},
            new object[] { false, new Utf8String(""), new Utf8String("a\uABCD")},
            new object[] { false, new Utf8String(""), new Utf8String("\uABCDa")}
        };

        public static object[][] StringEqualsTestCases_SimpleStrings = new object[][] {
            new object[] { true, new Utf8String("a"), new Utf8String("a")},
            new object[] { true, new Utf8String("\uABCD"), new Utf8String("\uABCD")},
            new object[] { true, new Utf8String("abc"), new Utf8String("abc")},
            new object[] { true, new Utf8String("a\uABCDbc"), new Utf8String("a\uABCDbc")},

            new object[] { false, new Utf8String("a"), new Utf8String("b")},
            new object[] { false, new Utf8String("aaaaa"), new Utf8String("aaaab")},
            new object[] { false, new Utf8String("aaaaa"), new Utf8String("baaaa")},
            new object[] { false, new Utf8String("ababab"), new Utf8String("bababa")},
            new object[] { false, new Utf8String("abbbba"), new Utf8String("abbba")},
            new object[] { false, new Utf8String("aabcaa"), new Utf8String("aacbaa")},
            new object[] { false, new Utf8String("\uABCD"), new Utf8String("\uABCE")},
            new object[] { false, new Utf8String("abc"), new Utf8String("abcd")},
            new object[] { false, new Utf8String("abc"), new Utf8String("dabc")},
            new object[] { false, new Utf8String("ab\uABCDc"), new Utf8String("ab\uABCEc")}
        };

        // TODO: add cases for different lengths
        [Theory]
        [MemberData("StringEqualsTestCases_EmptyStrings")]
        [MemberData("StringEqualsTestCases_SimpleStrings")]
        public unsafe void StringEquals(bool expected, Utf8String s1, Utf8String s2)
        {
            Assert.Equal(expected, s1.Equals(s2));
            Assert.Equal(expected, s2.Equals(s1));
        }

        [MemberData("StringEqualsTestCases_EmptyStrings")]
        [MemberData("StringEqualsTestCases_SimpleStrings")]
        public unsafe void StringEqualsConvertedToUtf16String(bool expected, Utf8String s1, Utf8String s2)
        {
            Assert.Equal(expected, s1.Equals(s2.ToString()));
            Assert.Equal(expected, s2.Equals(s1.ToString()));
        }

        public static object[][] StartsWithCodeUnitTestCases = new object[][] {
            new object[] { false, new Utf8String(""), (Utf8CodeUnit)'a' },
            new object[] { false, new Utf8String("a"), (Utf8CodeUnit)'a' },
            new object[] { false, new Utf8String("abc"), (Utf8CodeUnit)'a' },
            new object[] { false, new Utf8String("b"), (Utf8CodeUnit)'a' },
            new object[] { false, new Utf8String("ba"), (Utf8CodeUnit)'a' }
        };

        [Theory]
        [InlineData("", 'a')]
        [InlineData("abc", 'a')]
        [InlineData("v", 'v')]
        [InlineData("1", 'a')]
        [InlineData("1a", 'a')]
        public void StartsWithCodeUnit(string s, char c)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8CodeUnit codeUnit = (Utf8CodeUnit)(byte)c;
            Assert.Equal(s.StartsWith(c.ToString()), u8s.StartsWith(codeUnit));
        }

        [Theory]
        [InlineData("", 'a')]
        [InlineData("cba", 'a')]
        [InlineData("v", 'v')]
        [InlineData("1", 'a')]
        [InlineData("a1", 'a')]
        public void EndsWithCodeUnit(string s, char c)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8CodeUnit codeUnit = (Utf8CodeUnit)(byte)c;
            Assert.Equal(s.EndsWith(c.ToString()), u8s.EndsWith(codeUnit));
        }

        [Theory]
        [InlineData("", "a")]
        [InlineData("abc", "a")]
        [InlineData("v", "v")]
        [InlineData("1", "a")]
        [InlineData("1a", "a")]
        [InlineData("abc", "abc")]
        [InlineData("abcd", "abc")]
        [InlineData("abc", "abcd")]
        public void StartsWithUtf8String(string s, string pattern)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8String u8pattern = new Utf8String(pattern);

            Assert.Equal(s.StartsWith(pattern), u8s.StartsWith(u8pattern));
        }

        [Theory]
        [InlineData("", "a")]
        [InlineData("cba", "a")]
        [InlineData("v", "v")]
        [InlineData("1", "a")]
        [InlineData("a1", "a")]
        [InlineData("abc", "abc")]
        [InlineData("abcd", "bcd")]
        [InlineData("abc", "abcd")]
        public void EndsWithUtf8String(string s, string pattern)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8String u8pattern = new Utf8String(pattern);

            Assert.Equal(s.EndsWith(pattern), u8s.EndsWith(u8pattern));
        }

        [Theory]
        [InlineData("", 'a')]
        [InlineData("abc", 'a')]
        [InlineData("abc", 'b')]
        [InlineData("abc", 'c')]
        [InlineData("abc", 'd')]
        public void SubstringFromCodeUnit(string s, char c)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8CodeUnit codeUnit = (Utf8CodeUnit)(byte)(c);
            Utf8String u8result;

            int idx = s.IndexOf(c);
            bool expectedToFind = idx != -1;

            Assert.Equal(expectedToFind, u8s.TrySubstringFrom(codeUnit, out u8result));
            if (expectedToFind)
            {
                string expected = s.Substring(idx);
                Assert.Equal(new Utf8String(expected), u8result);
                Assert.Equal(expected, u8result.ToString());
            }
        }

        [Theory]
        [InlineData("", 'a')]
        [InlineData("abc", 'a')]
        [InlineData("abc", 'b')]
        [InlineData("abc", 'c')]
        [InlineData("abc", 'd')]
        public void SubstringToCodeUnit(string s, char c)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8CodeUnit codeUnit = (Utf8CodeUnit)(byte)(c);
            Utf8String u8result;

            int idx = s.IndexOf(c);
            bool expectedToFind = idx != -1;

            Assert.Equal(expectedToFind, u8s.TrySubstringTo(codeUnit, out u8result));
            if (expectedToFind)
            {
                string expected = s.Substring(0, idx);
                Assert.Equal(new Utf8String(expected), u8result);
                Assert.Equal(expected, u8result.ToString());
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void CodePointEnumeratorsTests(string s)
        {
            Utf8String u8s = new Utf8String(s);
            TestCodePointForwardEnumerator(s, u8s);
            TestCodePointReverseEnumerator(s, u8s);

            byte[] bytes = u8s.CopyBytes();
            unsafe
            {
                fixed (byte* pinnedBytes = bytes)
                {
                    Utf8String u8sFromBytePointer = new Utf8String(new Span<byte>(pinnedBytes, u8s.Length));
                    TestCodePointForwardEnumerator(s, u8sFromBytePointer);
                    TestCodePointReverseEnumerator(s, u8sFromBytePointer);
                }
            }
        }

        // Implementations are intentionally split to avoid boxing
        private void TestCodePointForwardEnumerator(string s, Utf8String u8s)
        {
            List<UnicodeCodePoint> codePoints = new List<UnicodeCodePoint>();
            Utf8String.CodePointEnumerator it = u8s.CodePoints.GetEnumerator();
            while (it.MoveNext())
            {
                codePoints.Add(it.Current);
            }

            Utf16LittleEndianCodePointEnumerable utf16CodePoints = new Utf16LittleEndianCodePointEnumerable(s);
            Assert.Equal(utf16CodePoints, codePoints);

            Utf8String u8s2 = new Utf8String(codePoints);
            Assert.Equal(u8s, u8s2);
            Assert.Equal(s, u8s2.ToString());
        }

        private void TestCodePointReverseEnumerator(string s, Utf8String u8s)
        {
            List<UnicodeCodePoint> codePoints = new List<UnicodeCodePoint>();
            Utf8String.CodePointReverseEnumerator it = u8s.CodePoints.GetReverseEnumerator();
            while (it.MoveNext())
            {
                codePoints.Add(it.Current);
            }

            codePoints.Reverse();

            Utf16LittleEndianCodePointEnumerable utf16CodePoints = new Utf16LittleEndianCodePointEnumerable(s);
            Assert.Equal(utf16CodePoints, codePoints);

            Utf8String u8s2 = new Utf8String(codePoints);
            Assert.Equal(u8s, u8s2);
            Assert.Equal(s, u8s2.ToString());
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("a ")]
        [InlineData("ab")]
        [InlineData("abcdefghijklmnopqrstuvwxyz   ")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(",.\r\n[]<>()")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("\uABEE ")]
        [InlineData("a\uABEE   ")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        [InlineData("  ")]
        [InlineData("  a")]
        [InlineData("   ab")]
        [InlineData("   abcdefghijklmnopqrstuvwxyz")]
        [InlineData(" ABCDEFGHIJKL     MNOPQRSTUVWXYZ     ")]
        [InlineData("   0123456789 ")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("   1258")]
        [InlineData("    1258He        llo")]
        [InlineData("   \uABCD")]
        [InlineData("  \uABEE")]
        [InlineData(" a\uABEE")]
        [InlineData("  a\uABEEa")]
        [InlineData("   a\uABEE\uABCDa")]
        [InlineData("   ")]
        [InlineData(" a  ")]
        [InlineData(" a ")]
        [InlineData(" a a a ")]
        [InlineData("         a a a           ")]
        public void TrimTests(string s)
        {
            TrimStartTest(s);
            TrimEndTest(s);
            TrimTest(s);

            char[] utf16codeUnits = s.ToCharArray();
            Array.Reverse(utf16codeUnits);
            s = new string(utf16codeUnits);

            TrimStartTest(s);
            TrimEndTest(s);
            TrimTest(s);
        }

        public void TrimStartTest(string s)
        {
            Utf8String u8s = new Utf8String(s);

            string expected = s.TrimStart();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimStart();
            Assert.Equal(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        public void TrimEndTest(string s)
        {
            Utf8String u8s = new Utf8String(s);

            string expected = s.TrimEnd();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimEnd();
            Assert.Equal(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        public void TrimTest(string s)
        {
            Utf8String u8s = new Utf8String(s);

            string expected = s.Trim();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.Trim();
            Assert.Equal(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }
		
        [Theory]
        [InlineData("")]
        [InlineData("test124")]
        public unsafe void StringEquals(string text)
        {
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            byte[] buffer = new byte[textArray.Length];

            fixed (byte* p = textArray)
            fixed (byte* pBuffer = buffer)
            {
                Span<byte> byteSpan = new Span<byte>(pBuffer, buffer.Length);
                Utf8String strFromArray = new Utf8String(textArray);
                Utf8String strFromPointer = new Utf8String(new Span<byte>(p, textArray.Length));
                Assert.Equal(strFromArray, strFromPointer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromArray.CopyTo(byteSpan);         
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromPointer.CopyTo(byteSpan);
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromArray.CopyTo(buffer);
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromPointer.CopyTo(buffer);
                Assert.Equal(textArray, buffer);
            }
        }

        [Fact]
        public void HashesSameForTheSameSubstrings()
        {
            const int len = 50;

            // two copies of the same string
            byte[] bytes = new byte[len * 2];
            for (int i = 0; i < len; i++)
            {
                // 0x20 is a spacebar, writing explicitly so
                // the value is more predictable
                bytes[i] = unchecked((byte)(0x20 + i));
                bytes[i + len] = bytes[i];
            }

            Utf8String sFromBytes = new Utf8String(bytes);
            Utf8String s1FromBytes = sFromBytes.Substring(0, len);
            Utf8String s2FromBytes = sFromBytes.Substring(len, len);

            unsafe
            {
                fixed (byte* pinnedBytes = bytes)
                {
                    Utf8String sFromSpan = new Utf8String(new Span<byte>(pinnedBytes, len * 2));
                    Utf8String s1FromSpan = sFromSpan.Substring(0, len);
                    Utf8String s2FromSpan = sFromSpan.Substring(len, len);
                    TestHashesSameForEquivalentString(s1FromBytes, s2FromBytes);
                    TestHashesSameForEquivalentString(s1FromSpan, s2FromSpan);
                    TestHashesSameForEquivalentString(s1FromSpan, s2FromBytes);
                }
            }
        }

        private void TestHashesSameForEquivalentString(Utf8String a, Utf8String b)
        {
            // for sanity
            Assert.Equal(a.Length, b.Length);
            Assert.Equal(a, b);

            for (int i = 0; i < a.Length; i++)
            {
                Utf8String prefixOfA = a.Substring(i, a.Length - i);
                Utf8String prefixOfB = b.Substring(i, b.Length - i);
                // sanity
                Assert.Equal(prefixOfA, prefixOfB);
                Assert.Equal(prefixOfA.GetHashCode(), prefixOfB.GetHashCode());

                // for all suffixes
                Utf8String suffixOfA = a.Substring(a.Length - i, i);
                Utf8String suffixOfB = b.Substring(b.Length - i, i);
                Assert.Equal(suffixOfA, suffixOfB);
            }
        }

        [Theory]
        [InlineData(0, "", "")]
        [InlineData(0, "abc", "")]
        [InlineData(-1, "", "a")]
        [InlineData(-1, "", "abc")]
        [InlineData(-1, "", "abc")]
        [InlineData(-1, "", "abc")]
        [InlineData(0, "a", "a")]
        [InlineData(-1, "a", "b")]
        [InlineData(0, "abc", "a")]
        [InlineData(0, "abc", "ab")]
        [InlineData(0, "abc", "abc")]
        [InlineData(1, "abc", "b")]
        [InlineData(1, "abc", "bc")]
        [InlineData(2, "abc", "c")]
        [InlineData(34, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", "aaaaaaaab")]
        [InlineData(0, "abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", "abbbbbbbb")]
        [InlineData(-1, "aaabaaa", "aabaaaa")]
        [InlineData(-1, "aaabaaa", "aadaaaa")]
        [InlineData(1, "abababab", "bababa")]
        [InlineData(-1, "abababab", "bbababa")]
        [InlineData(-1, "abababab", "bababaa")]
        [InlineData(7, "aaabaacaaac", "aaac")]
        [InlineData(7, "aaabaacaaac", "aaac")]
        [InlineData(7, "baabaaabaaaa", "baaaa")]
        public void IndexOfTests(int expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Assert.Equal(expected, utf8s.IndexOf(utf8substring));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("abc", "abc", "")]
        [InlineData(null, "", "a")]
        [InlineData(null, "", "abc")]
        [InlineData(null, "", "abc")]
        [InlineData(null, "", "abc")]
        [InlineData("a", "a", "a")]
        [InlineData(null, "a", "b")]
        [InlineData("abc", "abc", "a")]
        [InlineData("abc", "abc", "ab")]
        [InlineData("abc", "abc", "abc")]
        [InlineData("bc", "abc", "b")]
        [InlineData("bc", "abc", "bc")]
        [InlineData("c", "abc", "c")]
        [InlineData("aaaaaaaab", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", "aaaaaaaab")]
        [InlineData("abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", "abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", "abbbbbbbb")]
        [InlineData(null, "aaabaaa", "aabaaaa")]
        [InlineData(null, "aaabaaa", "aadaaaa")]
        [InlineData("bababab", "abababab", "bababa")]
        [InlineData(null, "abababab", "bbababa")]
        [InlineData(null, "abababab", "bababaa")]
        [InlineData("aaac", "aaabaacaaac", "aaac")]
        [InlineData("aaac", "aaabaacaaac", "aaac")]
        [InlineData("baaaa", "baabaaabaaaa", "baaaa")]
        public void SubstringFromTests(string expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Utf8String result;
            Assert.Equal(expected != null, utf8s.TrySubstringFrom(utf8substring, out result));

            if (expected != null)
            {
                Utf8String utf8expected = new Utf8String(expected);
                Assert.Equal(expected, result.ToString());
                Assert.Equal(utf8expected, result);
            }
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("", "abc", "")]
        [InlineData(null, "", "a")]
        [InlineData(null, "", "abc")]
        [InlineData(null, "", "abc")]
        [InlineData(null, "", "abc")]
        [InlineData("", "a", "a")]
        [InlineData(null, "a", "b")]
        [InlineData("", "abc", "a")]
        [InlineData("", "abc", "ab")]
        [InlineData("", "abc", "abc")]
        [InlineData("a", "abc", "b")]
        [InlineData("a", "abc", "bc")]
        [InlineData("ab", "abc", "c")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", "aaaaaaaab")]
        [InlineData("", "abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", "abbbbbbbb")]
        [InlineData(null, "aaabaaa", "aabaaaa")]
        [InlineData(null, "aaabaaa", "aadaaaa")]
        [InlineData("a", "abababab", "bababa")]
        [InlineData(null, "abababab", "bbababa")]
        [InlineData(null, "abababab", "bababaa")]
        [InlineData("aaabaac", "aaabaacaaac", "aaac")]
        [InlineData("aaabaac", "aaabaacaaac", "aaac")]
        [InlineData("baabaaa", "baabaaabaaaa", "baaaa")]
        public void SubstringToTests(string expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Utf8String result;
            Assert.Equal(expected != null, utf8s.TrySubstringTo(utf8substring, out result));

            if (expected != null)
            {
                Utf8String utf8expected = new Utf8String(expected);
                Assert.Equal(expected, result.ToString());
                Assert.Equal(utf8expected, result);
            }
        }

        [Theory]
        [InlineData("abc", 'a')]
        [InlineData("", 'a')]
        [InlineData("abc", 'a')]
        [InlineData("abc", 'c')]
        [InlineData("a", 'c')]
        [InlineData("c", 'c')]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", 'b')]
        [InlineData("abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", 'a')]
        [InlineData("aaabaaa", 'b')]
        [InlineData("aaabaaa", 'a')]
        [InlineData("abababab", 'a')]
        [InlineData("abababab", 'b')]
        public void IndexOfUtf8Character(string s, char character)
        {
            int expected = s.IndexOf(character);

            Utf8String u8s = new Utf8String(s);
            Utf8CodeUnit u8codeUnit = (Utf8CodeUnit)(byte)(character);

            Assert.Equal(expected, u8s.IndexOf(u8codeUnit));
        }

        [Theory]
        [InlineData(true, 0, "a", "a")]
        [InlineData(true, 0, "abc", "a")]
        [InlineData(true, 0, "abc", "ab")]
        [InlineData(true, 0, "abc", "abc")]
        [InlineData(true, 1, "abc", "b")]
        [InlineData(true, 1, "abc", "bc")]
        [InlineData(true, 2, "abc", "c")]
        [InlineData(false, -1, "a", "a")]
        [InlineData(false, 1, "a", "a")]
        [InlineData(false, 1, "abc", "a")]
        [InlineData(false, 1, "abc", "ab")]
        [InlineData(false, 1, "abc", "abc")]
        [InlineData(false, 0, "abc", "b")]
        [InlineData(false, 2, "abc", "b")]
        [InlineData(false, 0, "abc", "bc")]
        [InlineData(false, 0, "abc", "bc")]
        [InlineData(false, 1, "abc", "c")]
        public void IsSubstringAt(bool expected, int position, string s, string substring)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8String u8substring = new Utf8String(substring);
            Assert.Equal(expected, u8s.IsSubstringAt(position, u8substring));
        }

        [Theory]
        [InlineData(-1, "", 0)]
        [InlineData(0, "a", (uint)'a')]
        [InlineData(-1, "a", (uint)'b')]
        [InlineData(0, "\uABCD", 0xABCD)]
        [InlineData(-1, "\uABCD", 0xABCE)]
        [InlineData(0, "abc", (uint)'a')]
        [InlineData(1, "abc", (uint)'b')]
        [InlineData(2, "abc", (uint)'c')]
        [InlineData(-1, "abc", (uint)'d')]
        [InlineData(0, "\uABC0\uABC1\uABC2", 0xABC0)]
        [InlineData(3, "\uABC0\uABC1\uABC2", 0xABC1)]
        [InlineData(6, "\uABC0\uABC1\uABC2", 0xABC2)]
        [InlineData(-1, "\uABC0\uABC1\uABC2", 0xABC3)]
        [InlineData(0, "\uABC0bc", 0xABC0)]
        [InlineData(1, "a\uABC1c", 0xABC1)]
        [InlineData(2, "ab\uABC2", 0xABC2)]
        [InlineData(-1, "\uABC0\uABC1\uABC2", (uint)'d')]
        public void IndexOfUnicodeCodePoint(int expected, string s, uint codePointValue)
        {
            Utf8String u8s = new Utf8String(s);
            UnicodeCodePoint codePoint = (UnicodeCodePoint)codePointValue;
            Assert.Equal(expected, u8s.IndexOf(codePoint));
        }

        [Fact]
        public void ReferenceEquals()
        {
            Utf8String s1 = new Utf8String("asd");
            Assert.True(s1.ReferenceEquals(s1));

            Utf8String s2 = new Utf8String("asd");
            Assert.True(s2.ReferenceEquals(s2));

            Assert.False(s1.ReferenceEquals(s2));
            Assert.False(s2.ReferenceEquals(s1));
        }

        [Fact]
        public void ConstructingFromUtf16StringWithNullValueThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => { Utf8String utf8String = new Utf8String((string)null); });
        }

        [Fact]
        public void ConstructingFromUtf16StringWithEmptyStringDoesNotAllocate()
        {
            Assert.True((new Utf8String("")).ReferenceEquals(Utf8String.Empty));
            Assert.True((new Utf8String(string.Empty)).ReferenceEquals(Utf8String.Empty));
        }
    }
}
