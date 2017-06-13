// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Runtime.CompilerServices;

namespace System.Buffers.Tests
{
    public class SliceTests
    {
        [Fact]
        public static void SliceWithStart()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var buffer = new Buffer<int>(a).Slice(6);
            Assert.Equal(4, buffer.Length);
            Assert.True(Unsafe.AreSame(ref a[6], ref buffer.Span.DangerousGetPinnableReference()));
        }

        [Fact]
        public static void SliceWithStartPastEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var buffer = new Buffer<int>(a).Slice(a.Length);
            Assert.Equal(0, buffer.Length);
            Assert.True(Unsafe.AreSame(ref a[a.Length - 1], ref Unsafe.Subtract(ref buffer.Span.DangerousGetPinnableReference(), 1)));
        }

        [Fact]
        public static void SliceWithStartAndLength()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var buffer = new Buffer<int>(a).Slice(3, 5);
            Assert.Equal(5, buffer.Length);
            Assert.True(Unsafe.AreSame(ref a[3], ref buffer.Span.DangerousGetPinnableReference()));
        }

        [Fact]
        public static void SliceWithStartAndLengthUpToEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var buffer = new Buffer<int>(a).Slice(4, 6);
            Assert.Equal(6, buffer.Length);
            Assert.True(Unsafe.AreSame(ref a[4], ref buffer.Span.DangerousGetPinnableReference()));
        }

        [Fact]
        public static void SliceWithStartAndLengthPastEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var buffer = new Buffer<int>(a).Slice(a.Length, 0);
            Assert.Equal(0, buffer.Length);
            Assert.True(Unsafe.AreSame(ref a[a.Length - 1], ref Unsafe.Subtract(ref buffer.Span.DangerousGetPinnableReference(), 1)));
        }

        [Fact]
        public static void SliceRangeChecks()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(a.Length + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(0, a.Length + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(2, a.Length + 1 - 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(a.Length + 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a).Slice(a.Length, 1));
        }
    }
}
