// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.CaseFolding;
using System.Text.CaseFolding.Tests;
using Xunit;

namespace System.Text.CaseFolding.Tests
{
    public class FoldStringAndSpanTests
    {
        [Fact]
        public static void Fold_String_Span_Roundtrips()
        {
            string s = "Turkish I \u0131s TROUBL\u0130NG!";
            ReadOnlySpan<char> span = s.AsSpan();
            Span<char> span2 = stackalloc char[s.Length];
            s.AsSpan().CopyTo(span2);
            var foldedString = s.SimpleCaseFold();
            ReadOnlySpan<char> foldedSpan1 = span.SimpleCaseFold();

            Assert.Equal(foldedString, foldedSpan1.ToString());

            span2.SimpleCaseFold();
            Assert.Equal(foldedString, span2.ToString());
            Assert.Equal(0, foldedString.AsSpan().SequenceCompareTo(span2));
            Assert.Equal(foldedSpan1.ToString(), span2.ToString());
            Assert.Equal(0, foldedSpan1.SequenceCompareTo(span2));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("Hello", "hello")]
        [InlineData("Turkish I \u0131s TROUBL\u0130NG!", "turkish i \u0131s troubl\u0130ng!")]
        public static void Fold_String(string s, string target)
        {
            Assert.Equal(0, String.CompareOrdinal(s.SimpleCaseFold(), target));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Hello", "hello")]
        [InlineData("Turkish I \u0131s TROUBL\u0130NG!", "turkish i \u0131s troubl\u0130ng!")]
        public static void Fold_Span(string s, string target)
        {
            Assert.Equal(0, String.CompareOrdinal(s.AsSpan().SimpleCaseFold().ToString(), target));
            Assert.Equal(0, s.AsSpan().SimpleCaseFold().SequenceCompareTo(target.AsSpan()));
        }

        [Fact]
        public static void Fold_String_By_Char()
        {
            for (int i = 0; i <= 0xffff; i++)
            {
                var expected = i;
                if (CharUnicodeInfoTestData.CaseFoldingPairs.TryGetValue((char)i, out int foldedCharOut))
                {
                    expected = foldedCharOut;
                }

                var foldedChar = (int)SimpleCaseFolding.SimpleCaseFold((char)i);
                Assert.Equal(((char)expected).ToString(), ((char)foldedChar).ToString());
            }
        }
    }
}
