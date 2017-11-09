// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8ComparisonTests
    {
        [Theory]
        [InlineData("", "", 0)]
        [InlineData("a", "a", 0)]
        [InlineData("abc", "abc", 0)]     
        [InlineData("a\uABEE\uABCDa", "a\uABEE\uABCDa", 0)]
        [InlineData("a", "aa", -1)]
        [InlineData("aa", "a", 1)]
        [InlineData("a", "b", -1)]
        [InlineData("a\uABEE\uABCDa", "a", 1)]
        [InlineData("a", "a\uABEE\uABCDa", -1)]
        public void CopmareTo(string left, string right, int expected)
        {
            Utf8Span leftSpan = new Utf8Span(left);
            Utf8Span rightSpan = new Utf8Span(right);
            Utf8String leftString = new Utf8String(left);
            Utf8String rightString = new Utf8String(right);

            Assert.Equal(expected, leftSpan.CompareTo(rightSpan));
            Assert.Equal(expected, leftSpan.CompareTo(rightString));
            Assert.Equal(expected, leftSpan.CompareTo(right));

            Assert.Equal(expected, leftString.CompareTo(rightSpan));
            Assert.Equal(expected, leftString.CompareTo(rightString));
            Assert.Equal(expected, leftString.CompareTo(right));

            Assert.Equal(-expected, rightSpan.CompareTo(leftSpan));
            Assert.Equal(-expected, rightSpan.CompareTo(leftString));
            Assert.Equal(-expected, rightSpan.CompareTo(left));

            Assert.Equal(-expected, rightString.CompareTo(leftSpan));
            Assert.Equal(-expected, rightString.CompareTo(leftString));
            Assert.Equal(-expected, rightString.CompareTo(left));

            Assert.Equal(0, rightString.CompareTo(rightString));
            Assert.Equal(0, rightString.CompareTo(rightSpan));
            Assert.Equal(0, rightString.CompareTo(right));

            Assert.Equal(0, rightSpan.CompareTo(rightString));
            Assert.Equal(0, rightSpan.CompareTo(rightSpan));
            Assert.Equal(0, rightSpan.CompareTo(right));

            Assert.Equal(0, leftString.CompareTo(leftString));
            Assert.Equal(0, leftString.CompareTo(leftSpan));
            Assert.Equal(0, leftString.CompareTo(left));

            Assert.Equal(0, leftSpan.CompareTo(leftString));
            Assert.Equal(0, leftSpan.CompareTo(leftSpan));
            Assert.Equal(0, leftSpan.CompareTo(left));        
        }
    }
}
