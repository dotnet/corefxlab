// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Linq;
using Xunit;

namespace System.Buffers.Tests
{
    public class VectorizedOperationsTests
    {
        [Fact]
        public void SpanIndexOf()
        {
            int len = 10000;
            byte[] buffer = new byte[len];
            buffer[0] = 1;
            buffer[len / 2] = 2;
            buffer[len - 1] = 3;

            Span<byte> span = buffer;
            Assert.Equal(0, span.IndexOfVectorized(1));
            Assert.Equal(len/2, span.IndexOfVectorized(2));
            Assert.Equal(len-1, span.IndexOfVectorized(3));
            Assert.Equal(-1, span.IndexOfVectorized(4));
        }

        [Fact]
        public void ReadOnlySpanIndexOf()
        {
            int len = 10000;
            byte[] buffer = new byte[len];
            buffer[0] = 1;
            buffer[len / 2] = 2;
            buffer[len - 1] = 3;

            ReadOnlySpan<byte> span = buffer;
            Assert.Equal(0, span.IndexOfVectorized(1));
            Assert.Equal(len/2, span.IndexOfVectorized(2));
            Assert.Equal(len-1, span.IndexOfVectorized(3));
            Assert.Equal(-1, span.IndexOfVectorized(4));
        }

        [Fact]
        public void EmptySpanIndexOf()
        {
            int len = 0;
            byte[] buffer = new byte[len];
            Span<byte> span = buffer;
            Assert.Equal(-1, span.IndexOfVectorized(4));
        }

        [Fact]
        public void EmptyReadOnlySpanIndexOf()
        {
            int len = 10000;
            byte[] buffer = new byte[len];
            buffer[0] = 1;
            buffer[len / 2] = 2;
            buffer[len - 1] = 3;

            ReadOnlySpan<byte> span = buffer;
            Assert.Equal(-1, span.IndexOfVectorized(4));
        }

        [Fact]
        public void MatchEnumeratorSeekMatches()
        {
            byte value = (byte)'a';
            int len = 10247;
            var buffer = Enumerable.Repeat(value, len).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(value))
            {
                Assert.Equal(i, matchIndex);
                i++;
            }

            Assert.Equal(buffer.Length, i);
        }

        [Fact]
        public void MatchEnumeratorSeekSkips()
        {
            byte value = (byte)'a';
            byte search = (byte)'b';
            int len = 10247;
            var buffer = Enumerable.Repeat(value, len).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(search))
            {
                i++;
            }

            Assert.Equal(0, i);
        }

        [Fact]
        public void MatchEnumeratorSeekMatchesAndSkips()
        {
            byte match = (byte)'a';
            byte skip = (byte)'b';
            int len = 10247;

            var buffer = Enumerable.Repeat(new[] { match, skip }, len).SelectMany(item => item).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(match))
            {
                Assert.Equal(i, matchIndex);
                i += 2;
            }

            Assert.Equal(buffer.Length, i);
        }

        [Fact]
        public void MatchEnumeratorScanMatches()
        {
            byte value = (byte)'a';
            int len = sizeof(ulong) - 1;
            var buffer = Enumerable.Repeat(value, len).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(value))
            {
                Assert.Equal(i, matchIndex);
                i++;
            }

            Assert.Equal(buffer.Length, i);
        }

        [Fact]
        public void MatchEnumeratorScanSkips()
        {
            byte value = (byte)'a';
            byte search = (byte)'b';
            int len = sizeof(ulong) - 1;
            var buffer = Enumerable.Repeat(value, len).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(search))
            {
                i++;
            }

            Assert.Equal(0, i);
        }

        [Fact]
        public void MatchEnumeratorScanMatchesAndSkips()
        {
            byte match = (byte)'a';
            byte skip = (byte)'b';
            int len = sizeof(ulong) / 2 - 1;

            var buffer = Enumerable.Repeat(new[] { match, skip }, len).SelectMany(item => item).ToArray();

            ReadOnlySpan<byte> span = buffer;

            var i = 0;
            foreach (var matchIndex in span.MatchIndicies(match))
            {
                Assert.Equal(i, matchIndex);
                i += 2;
            }

            Assert.Equal(buffer.Length, i);
        }
    }
}