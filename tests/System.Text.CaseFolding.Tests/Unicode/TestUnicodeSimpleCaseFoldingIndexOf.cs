// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.CaseFolding;
using Xunit;

namespace System.Text.CaseFolding.Tests
{
    public class IndexOfTests
    {
        [Theory]
        [InlineData("", '\u007f', -1)]
        [InlineData("Hello", 'o', 4)]
        [InlineData("Hello", 'O', 4)]
        [InlineData("Hello", 'h', 0)]
        [InlineData("Hello", 'H', 0)]
        [InlineData("Hello", 'g', -1)]
        [InlineData("Hello", 'G', -1)]
        [InlineData("HelLo", '\0', -1)]
        [InlineData("!@#$%", '%', 4)]
        [InlineData("!@#$", '!', 0)]
        [InlineData("!@#$", '@', 1)]
        [InlineData("_____________\u807f", '\u007f', -1)]
        [InlineData("_____________\u807f__", '\u007f', -1)]
        [InlineData("_____________\u807f\u007f_", '\u007f', 14)]
        [InlineData("__\u807f_______________", '\u007f', -1)]
        [InlineData("__\u807f___\u007f___________", '\u007f', 6)]
        public void Test_IndexOfFolded(string s, char target, int expected)
        {
            Assert.Equal(expected, s.IndexOfFolded(target));
        }

        // Follow tests comes from src\System.Runtime\tests\System\StringTests.netcoreapp.cs
        [Fact]
        public static void IndexOf_TurkishI_Char()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            string s = "Turkish I \u0131s TROUBL\u0130NG!";
            ReadOnlySpan<char> span = s.AsSpan();
            char value = '\u0130';
            Assert.Equal(19, s.IndexOfFolded(value));
            Assert.Equal(19, span.IndexOfFolded(value));

            value = '\u0131';
            Assert.Equal(10, s.IndexOfFolded(value));
            Assert.Equal(10, span.IndexOfFolded(value));
        }

        [Fact]
        public static void IndexOf_EquivalentDiacritics_Char()
        {
            string s = "Exhibit a\u0300\u00C0";
            ReadOnlySpan<char> span = s.AsSpan();
            char value = '\u00C0';
            Assert.Equal(10, s.IndexOfFolded(value));
            Assert.Equal(10, span.IndexOfFolded(value));

            value = '\u0300';
            Assert.Equal(9, s.IndexOfFolded(value));
            Assert.Equal(9, span.IndexOfFolded(value));
        }

        [Fact]
        public static void IndexOf_CyrillicE_Char()
        {
            string s = "Foo\u0400Bar";
            ReadOnlySpan<char> span = s.AsSpan();
            char value = '\u0400';

            Assert.Equal(3, s.IndexOfFolded(value));
            Assert.Equal(3, span.IndexOfFolded(value));
        }
    }
}
