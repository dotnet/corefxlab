// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.CaseFolding;
using Xunit;

namespace System.Text.CaseFolding.Tests
{
    public class SimpleCaseFoldingStringComparerTests
    {
        // The tests come from CoreFX tests: src/System.Runtime.Extensions/tests/System/StringComparer.cs

        [Fact]
        public static void TestOrdinal_EmbeddedNull_ReturnsDifferentHashCodes()
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            Assert.NotEqual(sc.GetHashCode("\0AAAAAAAAA"), sc.GetHashCode("\0BBBBBBBBBBBB"));
        }

        [Theory]
        [InlineData("AAA", "aaa")]
        [InlineData("BaC", "bAc")]
        public static void TestGetHashCode_ReturnsHashCodes_Equal(string strA, string strB)
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            Assert.Equal(sc.GetHashCode(strA), sc.GetHashCode(strB));
            Assert.Equal(sc.GetHashCode((object)strA), sc.GetHashCode((object)strB));
        }

        [Theory]
        [InlineData("AAA", "AAB")]
        [InlineData("AAA", "AAb")]
        [InlineData("Ёлки-Палки", "ёлки-палкА")]
        public static void TestGetHashCode_ReturnsHashCodes_NotEqual(string strA, string strB)
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            Assert.NotEqual(sc.GetHashCode(strA), sc.GetHashCode(strB));
            Assert.NotEqual(sc.GetHashCode((object)strA), sc.GetHashCode((object)strB));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]

        [InlineData("Hello", "Hello")]
        [InlineData("\0AAAAAAAAA", "\0AAAAAAAAA")]
        [InlineData("Ёлки1#-Палки", "ёлки1#-палкИ")]

        [InlineData(SurrogateMinCodePoint, SurrogateMinCodePoint)]
        [InlineData(SurrogateMaxCodePoint, SurrogateMaxCodePoint)]
        [InlineData(SurrogateMinCodePoint + "A", SurrogateMinCodePoint + "A")]
        [InlineData("A" + SurrogateMinCodePoint, "A" + SurrogateMinCodePoint)]
        [InlineData("\U00010400AZ", "\U00010428AZ")]
        [InlineData("\U00010428AZ", "\U00010400AZ")]
        [InlineData("\U0001E921AZ", "\U0001E943AZ")]
        [InlineData("\U0001E943AZ", "\U0001E921AZ")]

        [InlineData("\u0600", "\u0600")]
        [InlineData("\u10A0", "\u10A0")]

        [InlineData("\u10A0", "\u2D00")]
        [InlineData("\u2D00", "\u10A0")]

        [InlineData("\uFF5A", "\uFF3A")]
        [InlineData("\uFF3A", "\uFF5A")]
        public static void VerifyStringComparer_Equal(string strA, string strB)
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            Assert.True(sc.Equals(strA, strB));
            Assert.True(sc.Equals((object)strA, (object)strB));
            Assert.True(sc.Equals((object)strA, strB));
            Assert.True(sc.Equals(strA, (object)strB));

            Assert.Equal(0, sc.Compare(strA, strB));
            Assert.Equal(0, ((IComparer)sc).Compare(strA, strB));
            Assert.True(((IEqualityComparer)sc).Equals(strA, strB));
        }

        [Fact]
        public static void VerifyStringComparer_Equal_By_Reference()
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            string strA = "test string";
            string strB = strA;

            Assert.True(sc.Equals(strA, strB));
            Assert.True(sc.Equals((object)strA, (object)strB));
            Assert.True(sc.Equals((object)strA, strB));
            Assert.True(sc.Equals(strA, (object)strB));

            Assert.Equal(0, sc.Compare(strA, strB));
            Assert.Equal(0, ((IComparer)sc).Compare(strA, strB));
            Assert.True(((IEqualityComparer)sc).Equals(strA, strB));
        }

        [Theory]
        [InlineData("", "Hello", -1)]
        [InlineData("Hello", "", 1)]

        [InlineData(null, "Hello", -1)]
        [InlineData("Hello", null, 1)]

        [InlineData("Hello1", "Hello2", -1)]
        [InlineData("Hello", "There", -1)]

        [InlineData("", "\0AAAAAAAAA", -1)]
        [InlineData("\0AAAAAAAAA", "", 1)]
        [InlineData("\0AAAAAAAAA", "\0BBBBBBBBBBBB", -1)]

        [InlineData("Ёлки-ПалкиЯ", "ёлки-палкИq", 1)]
        [InlineData("Палки-Ёлки", "Ёлки-Палки", -1)]

        [InlineData("Hello", SurrogateMinCodePoint, -1)]
        [InlineData(SurrogateMinCodePoint, "Hello", 1)]
        [InlineData("Ёлки-Палки", SurrogateMinCodePoint, -1)]
        [InlineData(SurrogateMinCodePoint, "Ёлки-Палки", 1)]

        [InlineData(SurrogateMinCodePoint + "A", SurrogateMinCodePoint, 1)]
        [InlineData(SurrogateMinCodePoint, SurrogateMinCodePoint + "A", -1)]
        [InlineData(SurrogateMaxCodePoint + "A", SurrogateMaxCodePoint, 1)]
        [InlineData(SurrogateMaxCodePoint, SurrogateMaxCodePoint + "A", -1)]
        [InlineData("A" + SurrogateMinCodePoint, "A" + SurrogateMaxCodePoint, -1)]
        [InlineData("A" + SurrogateMaxCodePoint, "A" + SurrogateMinCodePoint, 1)]

        [InlineData("\U00010400", "\u05ff", 1)]
        [InlineData("\u05ff", "\U00010400", -1)]
        [InlineData("\U00010400Z", "\u05ffZ", 1)]
        [InlineData("\u05ffZ", "\U00010400Z", -1)]
        [InlineData("Z\U00010400", "Z\u05ff", 1)]
        [InlineData("Z\u05ff", "Z\U00010400", -1)]

        [InlineData("\U00010400", LastCodePointInPlane0, 1)]
        [InlineData(LastCodePointInPlane0, "\U00010400", -1)]
        [InlineData("\U00010400Z", LastCodePointInPlane0 + "Z", 1)]
        [InlineData(LastCodePointInPlane0 + "Z", "\U00010400Z", -1)]
        [InlineData("Z\U00010400", "Z" + LastCodePointInPlane0, 1)]
        [InlineData("Z" + LastCodePointInPlane0, "Z\U00010400", -1)]

        [InlineData("\U0001E921", "\U00010400", 1)]
        [InlineData("\U00010400", "\U0001E921", -1)]

        [InlineData("\U0001E921", LastCodePointInPlane1, -1)]
        [InlineData(LastCodePointInPlane1, "\U0001E921", 1)]

        [InlineData("\U0001E921", SurrogateMaxCodePoint, -1)]
        [InlineData(SurrogateMaxCodePoint, "\U0001E921", 1)]

        [InlineData(BeforeSurrogates, SurrogateMinCodePoint, -1)]
        [InlineData(SurrogateMinCodePoint, BeforeSurrogates, 1)]
        [InlineData(SurrogateMaxCodePoint, AfterSurrogates, 1)]

        [InlineData(AfterSurrogates, SurrogateMaxCodePoint, -1)]
        [InlineData(SurrogateMinCodePoint, SurrogateMaxCodePoint, -1)]
        [InlineData(SurrogateMaxCodePoint, SurrogateMinCodePoint, 1)]

        [InlineData("\u05ff", "\u0600", -1)]
        [InlineData("\u0600", "\u05ff", 1)]
        public static void VerifyStringComparer_NotEqual(string strA, string strB, int result)
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            Assert.False(sc.Equals(strA, strB));
            Assert.False(sc.Equals((object)strA, (object)strB));
            Assert.False(sc.Equals((object)strA, strB));
            Assert.False(sc.Equals(strA, (object)strB));

            Assert.True(sc.Compare(strA, strB) * result > 0);
            Assert.True(((IComparer)sc).Compare(strA, strB) * result > 0);
            Assert.False(((IEqualityComparer)sc).Equals(strA, strB));
        }

        [Fact]
        public static void VerifyComparer()
        {
            SimpleCaseFoldingStringComparer sc = new SimpleCaseFoldingStringComparer();
            string s1 = "Hello";
            string s1a = "Hello";
            string s1b = "HELLO";
            string s2 = "ЯЯЯ2There";
            string aa = "\0AAAAAAAAA";
            string bb = "\0BBBBBBBBBBBB";

            Assert.True(sc.Equals(s1, s1a));
            Assert.True(sc.Equals((object)s1, (object)s1a));

            Assert.Equal(0, sc.Compare(s1, s1a));
            Assert.Equal(0, ((IComparer)sc).Compare(s1, s1a));

            Assert.True(sc.Equals(s1, s1));
            Assert.True(((IEqualityComparer)sc).Equals(s1, s1));
            Assert.Equal(0, sc.Compare(s1, s1));
            Assert.Equal(0, ((IComparer)sc).Compare(s1, s1));

            Assert.False(sc.Equals(s1, s2));
            Assert.False(((IEqualityComparer)sc).Equals(s1, s2));
            Assert.True(sc.Compare(s1, s2) < 0);
            Assert.True(((IComparer)sc).Compare(s1, s2) < 0);

            Assert.True(sc.Equals(s1, s1b));
            Assert.True(((IEqualityComparer)sc).Equals(s1, s1b));

            Assert.NotEqual(0, ((IComparer)sc).Compare(aa, bb));
            Assert.False(sc.Equals(aa, bb));
            Assert.False(((IEqualityComparer)sc).Equals(aa, bb));
            Assert.True(sc.Compare(aa, bb) < 0);
            Assert.True(((IComparer)sc).Compare(aa, bb) < 0);

            int result = sc.Compare(s1, s1b);
            Assert.Equal(0, result);

            result = ((IComparer)sc).Compare(s1, s1b);
            Assert.Equal(0, result);
        }

        const string SurrogateMinCodePoint = "\U00010000";
        const string SurrogateMaxCodePoint = "\U0010FFFF";
        const string BeforeSurrogates = "D7FF";
        const string AfterSurrogates = "\uE000";
        const string LastCodePointInPlane0 = "\uFFFF";
        const string LastCodePointInPlane1 = "\U0001FFFF";
    }
}
