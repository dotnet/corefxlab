// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Runtime.InteropServices;

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
        #endregion

        public static object[][] LengthTestCases = {
            new object[] { 0, ""},
            new object[] { 4, "1258"},
            new object[] { 3, "\uABCD"},
            new object[] { 4, "a\uABEE"},
            new object[] { 5, "a\uABEEa"},
            new object[] { 8, "a\uABEE\uABCDa"}
        };

        [Theory, MemberData("LengthTestCases")]
        public void Length(int expectedLength, string str)
        {
            Utf8String s = new Utf8String(str);
            Assert.Equal(expectedLength, s.Bytes.Length);
        }

        public static object[][] LengthInCodePointsTestCases = {
            new object[] { 0, ""},
            new object[] { 4, "1258"},
            new object[] { 1, "\uABCD"},
            new object[] { 2, "a\uABEE"},
            new object[] { 3, "a\uABEEa"},
            new object[] { 4, "a\uABEE\uABCDa"}
        };

        //[Theory(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program."), MemberData("LengthInCodePointsTestCases")]
        public void LengthInCodePoints(int expectedLength, string str)
        {
            Utf8String s = new Utf8String(str);
            Assert.Equal(expectedLength, s.GetEnumerator().Count());
        }

        public static object[][] ToStringTestCases = {
            new object[] { "", ""},
            new object[] { "1258", "1258"},
            new object[] { "\uABCD", "\uABCD"},
            new object[] { "a\uABEE", "a\uABEE"},
            new object[] { "a\uABEEa", "a\uABEEa"},
            new object[] { "a\uABEE\uABCDa", "a\uABEE\uABCDa"}
        };

        [Theory, MemberData("ToStringTestCases")]
        public void ToString(string expected, string str)
        {
            Utf8String s = new Utf8String(str);
            Assert.Equal(expected, s.ToString());
        }

        public static object[][] StringEqualsTestCases_EmptyStrings = new object[][] {
            new object[] { true, "", ""},

            new object[] { false, "", " "},
            new object[] { false, "", "a"},
            new object[] { false, "", "\uABCD"},
            new object[] { false, "", "abc"},
            new object[] { false, "", "a\uABCD"},
            new object[] { false, "", "\uABCDa"}
        };

        public static object[][] StringEqualsTestCases_SimpleStrings = new object[][] {
            new object[] { true, "a", "a"},
            new object[] { true, "\uABCD", "\uABCD"},
            new object[] { true, "abc", "abc"},
            new object[] { true, "a\uABCDbc", "a\uABCDbc"},

            new object[] { false, "a", "b"},
            new object[] { false, "aaaaa", "aaaab"},
            new object[] { false, "aaaaa", "baaaa"},
            new object[] { false, "ababab", "bababa"},
            new object[] { false, "abbbba", "abbba"},
            new object[] { false, "aabcaa", "aacbaa"},
            new object[] { false, "\uABCD", "\uABCE"},
            new object[] { false, "abc", "abcd"},
            new object[] { false, "abc", "dabc"},
            new object[] { false, "ab\uABCDc", "ab\uABCEc"}
        };

        // TODO: add cases for different lengths
        [Theory]
        [MemberData("StringEqualsTestCases_EmptyStrings")]
        [MemberData("StringEqualsTestCases_SimpleStrings")]
        public unsafe void StringEqualsWithThreeArgs(bool expected, string str1, string str2)
        {   // TODO: investigate why the tests fail if we have two methods with the same name StringEquals
            Utf8String s1 = new Utf8String(str1);
            Utf8String s2 = new Utf8String(str2);
            Assert.Equal(expected, s1.Equals(s2));
            Assert.Equal(expected, s2.Equals(s1));
        }

        [Theory]
        [MemberData("StringEqualsTestCases_EmptyStrings")]
        [MemberData("StringEqualsTestCases_SimpleStrings")]
        public unsafe void StringEqualsConvertedToUtf16String(bool expected, string str1, string str2)
        {
            Utf8String s1 = new Utf8String(str1);
            Utf8String s2 = new Utf8String(str2);
            Assert.Equal(expected, s1.Equals(s2.ToString()));
            Assert.Equal(expected, s2.Equals(s1.ToString()));
        }

        public static object[][] StartsWithCodeUnitTestCases = new object[][] {
            new object[] { false, "", (byte)'a' },
            new object[] { false, "a", (byte)'a' },
            new object[] { false, "abc", (byte)'a' },
            new object[] { false, "b", (byte)'a' },
            new object[] { false, "ba", (byte)'a' }
        };

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

        private byte[] GetUtf8BytesFromCodePoints(List<uint> codePoints)
        {
            ReadOnlySpan<byte> utf32 = MemoryMarshal.AsBytes(codePoints.ToArray().AsSpan());
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf32.ToUtf8Length(utf32, out int needed));

            byte[] utf8 = new byte[needed];
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf32.ToUtf8(utf32, utf8, out int consumed, out int written));
            Assert.Equal(needed, written);

            return utf8;
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

        [Theory]
        [InlineData(" ", "")]
        [InlineData("", "a")]
        [InlineData("", "")]
        [InlineData("a","")]
        [InlineData("ab", "b")]
        [InlineData("aba", "ab")]
        [InlineData("abcdefghijklmnopqrstuvwxyz   ", "abcd")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABD")]
        [InlineData("0123456789", "234")]
        [InlineData("1258", "1258")]
        [InlineData("1258Hello", "Hello")]
        [InlineData("\uABCD", "\uABCD")]
        [InlineData("    1258He        llo", "1258Hello")]
        [InlineData(" a a a ", " a")]
        [InlineData("         a a a           ", " ")]
        public void TrimStartCharacterTest(string s, string trimCharacters)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8String u8TrimCharacters = new Utf8String(trimCharacters);

            string expected = s.TrimStart(trimCharacters.ToCharArray());
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimStart(u8TrimCharacters);
            TestHelper.Validate(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        public void TrimStartTest(string s)
        {
            Utf8String u8s = new Utf8String(s);
            string expected = s.TrimStart();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimStart();

            TestHelper.Validate(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        public void TrimEndTest(string s)
        {
            Utf8String u8s = new Utf8String(s);

            string expected = s.TrimEnd();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimEnd();
            TestHelper.Validate(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        [Theory]
        [InlineData(" ", "")]
        [InlineData("", "a")]
        [InlineData("", "")]
        [InlineData("a", "")]
        [InlineData("ab", "b")]
        [InlineData("ab", "a")]
        [InlineData("abdad", "ad")]
        [InlineData("abcdefghijklmnopqrstuvwxyz   ", " ")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "WYZ")]
        [InlineData("0123456789", "234")]
        [InlineData("1258", "1258")]
        [InlineData("1258Hello", "Hello")]
        [InlineData("\uABCD", "\uABCD")]
        [InlineData("    1258He        llo", "1258Hello")]
        [InlineData(" a a a ", " a")]
        [InlineData("         a a a           ", " ")]
        public void TrimEndCharacterTest(string s, string trimCharacters)
        {
            Utf8String u8s = new Utf8String(s);
            Utf8String u8TrimCharacters = new Utf8String(trimCharacters);

            string expected = s.TrimEnd(trimCharacters.ToCharArray());
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.TrimEnd(u8TrimCharacters);
            TestHelper.Validate(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);

        }

        public void TrimTest(string s)
        {
            Utf8String u8s = new Utf8String(s);

            string expected = s.Trim();
            Utf8String u8expected = new Utf8String(expected);

            Utf8String u8trimmed = u8s.Trim();
            TestHelper.Validate(u8expected, u8trimmed);

            string trimmed = u8trimmed.ToString();
            Assert.Equal(expected, trimmed);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test124")]
        public unsafe void StringEquals(string text)
        {
            byte[] textArray = Text.Encoding.UTF8.GetBytes(text);
            byte[] buffer = new byte[textArray.Length];

            fixed (byte* p = textArray)
            fixed (byte* pBuffer = buffer)
            {
                Span<byte> byteSpan = new Span<byte>(pBuffer, buffer.Length);
                Utf8String strFromArray = new Utf8String(textArray);
                Utf8String strFromPointer = new Utf8String(new Span<byte>(p, textArray.Length));
                TestHelper.Validate(strFromArray, strFromPointer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromArray.Bytes.CopyTo(byteSpan);
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromPointer.Bytes.CopyTo(byteSpan);
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromArray.Bytes.CopyTo(buffer);
                Assert.Equal(textArray, buffer);

                Array.Clear(buffer, 0, buffer.Length);
                strFromPointer.Bytes.CopyTo(buffer);
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
            Assert.Equal(a.Bytes.Length, b.Bytes.Length);
            TestHelper.Validate(a, b);

            for (int i = 0; i < a.Bytes.Length; i++)
            {
                Utf8String prefixOfA = a.Substring(i, a.Bytes.Length - i);
                Utf8String prefixOfB = b.Substring(i, b.Bytes.Length - i);
                // sanity
                TestHelper.Validate(prefixOfA, prefixOfB);
                Assert.Equal(prefixOfA.GetHashCode(), prefixOfB.GetHashCode());

                // for all suffixes
                Utf8String suffixOfA = a.Substring(a.Bytes.Length - i, i);
                Utf8String suffixOfB = b.Substring(b.Bytes.Length - i, i);
                TestHelper.Validate(suffixOfA, suffixOfB);
            }
        }

        [Theory]
        [InlineData(0, "", "")]
        [InlineData(0, "abc", "")]
        [InlineData(-1, "", "a")]
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
        [InlineData(7, "baabaaabaaaa", "baaaa")]
        public void IndexOfTests(int expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Assert.Equal(expected, utf8s.IndexOf(utf8substring));
        }

        [Theory]
        [InlineData(0, "", "")]
        [InlineData(2, "abc", "")]
        [InlineData(-1, "", "a")]
        [InlineData(-1, "", "abc")]
        [InlineData(0, "a", "a")]
        [InlineData(-1, "a", "b")]
        [InlineData(0, "abc", "a")]
        [InlineData(0, "abc", "ab")]
        [InlineData(0, "abc", "abc")]
        [InlineData(1, "abc", "b")]
        [InlineData(1, "abc", "bc")]
        [InlineData(3, "abcc", "c")]
        [InlineData(3, "abcbc", "bc")]
        [InlineData(38, "aaaaaaabaaaaaaaabaaaaaaaabaaaaaaaabaaaaaaaaaaab", "aaaaaaaab")]
        [InlineData(16, "abbbbbbbabbbbbbbabbbbbbbbbbbbbbbbbbbbbbbbbbbb", "abbbbbbbb")]
        [InlineData(-1, "aaabaaa", "aabaaaa")]
        [InlineData(-1, "aaabaaa", "aadaaaa")]
        [InlineData(3, "ababababa", "bababa")]
        [InlineData(-1, "abababab", "bbababa")]
        [InlineData(-1, "abababab", "bababaa")]
        [InlineData(7, "aaabaacaaac", "aaac")]
        [InlineData(9, "baaaabaaabaaaa", "baaaa")]
        [InlineData(3, "\uABC0\uABC1\uABC2", "\uABC1\uABC2")]
        public void LastIndexOfTests(int expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Assert.Equal(expected, utf8s.LastIndexOf(utf8substring));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("abc", "abc", "")]
        [InlineData(null, "", "a")]
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
        [InlineData("baaaa", "baabaaabaaaa", "baaaa")]
        public void SubstringFromTests(string expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Assert.Equal(expected != null, utf8s.TrySubstringFrom(utf8substring, out Utf8String result));

            if (expected != null)
            {
                Utf8String utf8expected = new Utf8String(expected);
                Assert.Equal(expected, result.ToString());
                TestHelper.Validate(utf8expected, result);
            }
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("", "abc", "")]
        [InlineData(null, "", "a")]
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
        [InlineData("baabaaa", "baabaaabaaaa", "baaaa")]
        public void SubstringToTests(string expected, string s, string substring)
        {
            Utf8String utf8s = new Utf8String(s);
            Utf8String utf8substring = new Utf8String(substring);
            Assert.Equal(expected != null, utf8s.TrySubstringTo(utf8substring, out Utf8String result));

            if (expected != null)
            {
                Utf8String utf8expected = new Utf8String(expected);
                Assert.Equal(expected, result.ToString());
                TestHelper.Validate(utf8expected, result);
            }
        }

        [Theory]
        [InlineData("abc", 'a')]
        [InlineData("", 'a')]
        [InlineData("abc", 'c')]
        [InlineData("a", 'c')]
        [InlineData("c", 'c')]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", 'b')]
        [InlineData("abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", 'a')]
        [InlineData("aaabaaa", 'b')]
        [InlineData("aaabaaa", 'a')]
        [InlineData("abababab", 'a')]
        [InlineData("abababab", 'b')]
        public void IndexOfUtf8CharacterTest(string s, char character)
        {
            int expected = s.IndexOf(character);

            Utf8String u8s = new Utf8String(s);
            byte u8codeUnit = (byte)(character);

            Assert.Equal(expected, u8s.IndexOf(u8codeUnit));
        }

        [Theory]
        [InlineData("abc", 'a')]
        [InlineData("", 'a')]
        [InlineData("abc", 'c')]
        [InlineData("a", 'c')]
        [InlineData("c", 'c')]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", 'b')]
        [InlineData("abbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", 'a')]
        [InlineData("aaabaaa", 'b')]
        [InlineData("aaabaaa", 'a')]
        [InlineData("abababab", 'a')]
        [InlineData("abababab", 'b')]
        public void LastIndexOfUtf8CharacterTest(string s, char character)
        {
            int expected = s.LastIndexOf(character);

            Utf8String u8s = new Utf8String(s);
            byte u8codeUnit = (byte)(character);

            Assert.Equal(expected, u8s.LastIndexOf(u8codeUnit));
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
            Assert.Equal(expected, u8s.IndexOf(codePointValue));
        }

        [Theory]
        [InlineData(-1, "", 0)]
        [InlineData(0, "a", (uint)'a')]
        [InlineData(-1, "a", (uint)'b')]
        [InlineData(0, "\uABCD", 0xABCD)]
        [InlineData(-1, "\uABCD", 0xABCE)]
        [InlineData(0, "abc", (uint)'a')]
        [InlineData(1, "abc", (uint)'b')]
        [InlineData(3, "abcc", (uint)'c')]
        [InlineData(-1, "abc", (uint)'d')]
        [InlineData(0, "\uABC0\uABC1\uABC2", 0xABC0)]
        [InlineData(3, "\uABC0\uABC1\uABC2", 0xABC1)]
        [InlineData(6, "\uABC0\uABC1\uABC2", 0xABC2)]
        [InlineData(9, "\uABC0\uABC1\uABC2\uABC0", 0xABC0)]
        [InlineData(-1, "\uABC0\uABC1\uABC2", 0xABC3)]
        [InlineData(0, "\uABC0bc", 0xABC0)]
        [InlineData(1, "a\uABC1c", 0xABC1)]
        [InlineData(2, "ab\uABC2", 0xABC2)]
        [InlineData(-1, "\uABC0\uABC1\uABC2", (uint)'d')]
        public void LastIndexOfUnicodeCodePoint(int expected, string s, uint codePointValue)
        {
            Utf8String u8s = new Utf8String(s);
            Assert.Equal(expected, u8s.LastIndexOf(codePointValue));
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

        public static IEnumerable<object[]> TryComputeEncodedBytesShouldMatchEncoding_Strings()
        {
            string[] data =
            {
                "",
                "abc",
                "def",
                "\uABCD",
                "\uABC0bc",
                "a\uABC1c",
                "ab\uABC2",
                "\uABC0\uABC1\uABC2",
                Text.Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x92, 0xA9})
            };
            return data.Select(s => new object[] { s });
        }

        public static object[][] EnsureCodeUnitsOfStringTestCases = {
            // empty
            new object[] { new byte[0],""},
            // ascii
            new object[] { new byte[] { 0x61 }, "a"},
            new object[] { new byte[] { 0x61, 0x62, 0x63 }, "abc"},
            new object[] { new byte[] { 0x41, 0x42, 0x43, 0x44 }, "ABCD"},
            new object[] { new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 }, "01234"},
            new object[] { new byte[] { 0x20, 0x2c, 0x2e, 0x0d, 0x0a, 0x5b, 0x5d, 0x3c, 0x3e, 0x28, 0x29 },  " ,.\r\n[]<>()"},
            // edge cases for multibyte characters
            new object[] { new byte[] { 0x7f }, "\u007f"},
            new object[] { new byte[] { 0xc2, 0x80 }, "\u0080"},
            new object[] { new byte[] { 0xdf, 0xbf }, "\u07ff"},
            new object[] { new byte[] { 0xe0, 0xa0, 0x80 }, "\u0800"},
            new object[] { new byte[] { 0xef, 0xbf, 0xbf }, "\uffff"},
            // ascii mixed with multibyte characters
            // 1 code unit + 2 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80 }, "a\u0080"},
            // 2 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0x61 }, "\u0080a"},
            // 1 code unit + 2 code units + 1 code unit
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61 }, "a\u0080a"},
            // 3 code units + 2 code units
            new object[] { new byte[] { 0xe0, 0xa0, 0x80, 0xc2, 0x80 }, "\u0800\u0080"},
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0xe0, 0xa0, 0x80 }, "\u0080\u0800"},
            // 2 code units + 3 code units
            new object[] { new byte[] { 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf }, "\u0080a\uffff"},
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0xef, 0xbf, 0xbf }, "a\u0080\uffff"},
            // 2 code units + 3 code units + 1 code unit
            new object[] { new byte[] { 0xc2, 0x80, 0xef, 0xbf, 0xbf, 0x61 }, "\u0080\uffffa"},
            // 1 code unit + 2 code units + 3 code units
            new object[] { new byte[] { 0x61, 0xc2, 0x80, 0x61, 0xef, 0xbf, 0xbf, 0x61 }, "a\u0080a\uffffa"}
            // TODO: Add case with 4 byte character - it is impossible to do using string literals, need to create it using code point
        };

        [Theory, MemberData("EnsureCodeUnitsOfStringTestCases")]
        public void EnsureCodeUnitsOfStringByIndexingBytes(byte[] expectedBytes, string str)
        {
            var utf8String = new Utf8String(str);
            Assert.Equal(expectedBytes.Length, utf8String.Bytes.Length);

            for (int i = 0; i < utf8String.Bytes.Length; i++)
            {
                Assert.Equal(expectedBytes[i], utf8String.Bytes[i]);
            }
        }
    }
}
