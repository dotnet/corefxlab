// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers;
using Xunit;

namespace System.Slices.Tests
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

        [Theory]
        [InlineData(8, 0)]
        [InlineData(8, 1)]
        [InlineData(8, 4)]
        [InlineData(8, 7)]
        [InlineData(32, 0)]
        [InlineData(32, 1)]
        [InlineData(32, 17)]
        [InlineData(32, 31)]
        [InlineData(128, 0)]
        [InlineData(128, 1)]
        [InlineData(128, 26)]
        [InlineData(128, 127)]
        [InlineData(256, 0)]
        [InlineData(256, 1)]
        [InlineData(256, 173)]
        [InlineData(256, 255)]
        public void SpanIndicesOf(int gap, byte target)
        {
            byte[] dataBytes = new byte[8 * 1024];
            for (var i = 0; i < dataBytes.Length; i++)
                dataBytes[i] = (byte)(i % gap);
            Span<byte> data = dataBytes;

            Span<int> indices = new int[(dataBytes.Length / gap) + 1];

            // Validate correctness
            int count;
            Assert.True(data.TryIndicesOf(target, indices, out count));
            Assert.Equal(indices.Length - 1, count);

            var idx = 0;
            var cur = 0;
            Span<byte> sp = data;

            while (true)
            {
                int pos = sp.IndexOf(target);
                if (pos == -1) break;
                Assert.True(indices.Length > idx);
                Assert.Equal(cur + pos, indices[idx++]);
                sp = sp.Slice(pos + 1);
                cur += pos + 1;
            }

            Assert.Equal(idx, count);
        }

        [Fact]
        public void NotFoundIndicesOf()
        {
            byte[] dataBytes = new byte[20];
            for (var i = 0; i < dataBytes.Length; i++)
                dataBytes[i] = (byte)1;
            Span<byte> data = dataBytes;

            Span<int> indices = new int[1];

            int count;
            Assert.True(data.TryIndicesOf(2, indices, out count));
            Assert.Equal(0, count);
        }

        [Fact]
        public void PartialIndicesOf()
        {
            int gap = 8;
            byte[] dataBytes = new byte[200];
            for (var i = 0; i < dataBytes.Length; i++)
                dataBytes[i] = (byte)(i % gap);
            Span<byte> data = dataBytes;

            Span<int> indices = new int[(dataBytes.Length / gap) - 5];

            int count;
            Assert.False(data.TryIndicesOf(3, indices, out count));
            Assert.Equal(indices.Length, count);

            Span<byte> left = data.Slice(indices[count - 1]);
            Assert.True(left.TryIndicesOf(1, indices, out count));
            Assert.Equal(5, count);
        }

        [Fact]
        public void ExactIndicesOf()
        {
            int gap = 8;
            byte[] dataBytes = new byte[200];
            for (var i = 0; i < dataBytes.Length; i++)
                dataBytes[i] = (byte)(i % gap);
            Span<byte> data = dataBytes;

            Span<int> indices = new int[dataBytes.Length / gap];

            int count;
            Assert.False(data.TryIndicesOf(3, indices, out count));
            Assert.Equal(indices.Length, count);

            Span<byte> left = data.Slice(indices[count - 1]);
            Assert.True(left.TryIndicesOf(1, indices, out count));
            Assert.Equal(0, count);
        }

        [Fact]
        public void NegativeIndicesOf()
        {
            byte[] buffer = new byte[0];
            Span<byte> span = buffer;

            Span<int> indices = new int[1];
            int count;
            Assert.False(span.TryIndicesOf(4, indices, out count));

            indices = new int[0];
            Assert.False(span.TryIndicesOf(3, indices, out count));
        }
    }
}