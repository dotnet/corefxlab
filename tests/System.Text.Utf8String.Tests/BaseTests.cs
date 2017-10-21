// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Utf16;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8Utf16ConversionTests
    {
        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf16StringToUtf8StringToUtf16StringRoundTrip(string utf16String)
        {
            Utf8String utf8String = new Utf8String(utf16String);
            Assert.Equal(utf16String, utf8String.ToString());
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
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf8StringToUtf16StringToUtf8StringRoundTrip(string str)
        {
            Utf8String utf8String = new Utf8String(str);
            string utf16String = utf8String.ToString();
            TestHelper.Validate(utf8String, new Utf8String(utf16String));
        }

        public static object[][] EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases = {
            new object[] { "", ""},
            new object[] { "1258", "1258"},
            new object[] { "\uABCD", "\uABCD"},
            new object[] { "a\uABEE", "a\uABEE"},
            new object[] { "a\uABEEa", "a\uABEEa"},
            new object[] { "a\uABEE\uABCDa", "a\uABEE\uABCDa"}
        };

        [Theory, MemberData("EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases")]
        public void EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSame(string utf16String, string str)
        {
            var utf8String = new Utf8String(str);
            var utf16StringCodePoints = new Utf16LittleEndianCodePointEnumerable(utf16String);

            var utf16CodePointEnumerator = utf16StringCodePoints.GetEnumerator();
            var utf8CodePointEnumerator = utf8String.CodePoints.GetEnumerator();

            bool moveNext;
            while (true)
            {
                moveNext = utf16CodePointEnumerator.MoveNext();
                Assert.Equal(moveNext, utf8CodePointEnumerator.MoveNext());
                if (!moveNext)
                {
                    break;
                }
                Assert.Equal(utf16CodePointEnumerator.Current, utf8CodePointEnumerator.Current);
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
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf16StringToUtf8SpanToUtf16StringRoundTrip(string utf16String)
        {
            Utf8Span Utf8Span = new Utf8Span(utf16String);
            Assert.Equal(utf16String, Utf8Span.ToString());
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
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf8SpanToUtf16StringToUtf8SpanRoundTrip(string str)
        {
            Utf8Span Utf8Span = new Utf8Span(str);
            string utf16String = Utf8Span.ToString();
            TestHelper.Validate(Utf8Span, new Utf8Span(utf16String));
        }
        

        [Theory, MemberData("EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases")]
        public void EnumerateAndEnsureCodePointsOfTheSameUtf8SpanAndUtf16StringsAreTheSame(string utf16String, string str)
        {
            var Utf8Span = new Utf8Span(str);
            var utf16StringCodePoints = new Utf16LittleEndianCodePointEnumerable(utf16String);

            var utf16CodePointEnumerator = utf16StringCodePoints.GetEnumerator();
            var utf8CodePointEnumerator = Utf8Span.CodePoints.GetEnumerator();

            bool moveNext;
            while (true)
            {
                moveNext = utf16CodePointEnumerator.MoveNext();
                Assert.Equal(moveNext, utf8CodePointEnumerator.MoveNext());
                if (!moveNext)
                {
                    break;
                }
                Assert.Equal(utf16CodePointEnumerator.Current, utf8CodePointEnumerator.Current);
            }
        }
    }
}
