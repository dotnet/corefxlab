// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
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
    }
}