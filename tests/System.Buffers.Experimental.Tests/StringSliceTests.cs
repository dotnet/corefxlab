// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            ReadOnlySpan<char> span = s.AsSpan().Slice(2);
            char[] expected = s.Substring(2).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntPastEnd()
        {
            string s = "Hello";
            ReadOnlySpan<char> span = s.AsSpan().Slice(s.Length);
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void StringSliceIntInt()
        {
            string s = "Goodbye";
            ReadOnlySpan<char> span = s.AsSpan().Slice(2, 4);
            char[] expected = s.Substring(2, 4).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntIntUpToEnd()
        {
            string s = "Goodbye";
            ReadOnlySpan<char> span = s.AsSpan().Slice(2, s.Length - 2);
            char[] expected = s.Substring(2).ToCharArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringSliceIntIntPastEnd()
        {
            string s = "Hello";
            ReadOnlySpan<char> span = s.AsSpan().Slice(s.Length, 0);
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void StringSliceNullChecked()
        {
            string s = null;
            Assert.Throws<ArgumentNullException>(() => s.AsSpan().DontBox());
            Assert.Throws<ArgumentNullException>(() => s.AsSpan().Slice(0).DontBox());
            Assert.Throws<ArgumentNullException>(() => s.AsSpan().Slice(0, 0).DontBox());
        }

        [Fact]
        public static void StringSliceIntRangeChecked()
        {
            string s = "Hello";
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(-1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(s.Length + 1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(-1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(0, s.Length + 1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(2, s.Length + 1 - 2).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(s.Length + 1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => s.AsSpan().Slice(s.Length, 1).DontBox());
        }
    }
}
