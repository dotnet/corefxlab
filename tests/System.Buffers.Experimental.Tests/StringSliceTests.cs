// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Buffers.Tests
{
    public partial class StringSliceTests
    {
        [Fact]
        public static void StringSliceNullary()
        {
            string s = "Hello";
            ReadOnlySpan<char> span = s.AsSpan();
            char[] expected = s.ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceInt()
        {
            string s = "Goodbye";
            ReadOnlySpan<char> span = s.AsSpan(2);
            char[] expected = s.Substring(2).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntPastEnd()
        {
            string s = "Hello";
            ReadOnlySpan<char> span = s.AsSpan(s.Length);
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void StringSliceIntInt()
        {
            string s = "Goodbye";
            ReadOnlySpan<char> span = s.AsSpan(2, 4);
            char[] expected = s.Substring(2, 4).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntIntUpToEnd()
        {
            string s = "Goodbye";
            ReadOnlySpan<char> span = s.AsSpan(2, s.Length - 2);
            char[] expected = s.Substring(2).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntIntPastEnd()
        {
            string s = "Hello";
            ReadOnlySpan<char> span = s.AsSpan(s.Length, 0);
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void StringSliceIntRangeChecked()
        {
            string s = "Hello";
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(-1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(s.Length + 1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(-1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(0, s.Length + 1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(2, s.Length + 1 - 2).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(s.Length + 1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan(s.Length, 1).DontBox());
        }
    }
}
