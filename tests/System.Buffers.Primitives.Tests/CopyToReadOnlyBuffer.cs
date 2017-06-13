// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class CopyToReadOnlyBuffer
    {
        [Fact]
        public static void CopyToSpan()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101 };

            var srcBuffer = new ReadOnlyBuffer<int>(src);
            srcBuffer.CopyTo(dst.AsSpan());
            Assert.Equal<int>(src, dst);
        }

        [Fact]
        public static void CopyToBuffer()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101 };

            var srcBuffer = new ReadOnlyBuffer<int>(src);
            var dstBuffer = new Buffer<int>(dst);
            srcBuffer.CopyTo(dstBuffer);
            Assert.Equal<int>(src, dst);
        }

        [Fact]
        public static void TryCopyToSpan()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101 };

            var srcBuffer = new ReadOnlyBuffer<int>(src);
            bool success = srcBuffer.TryCopyTo(dst.AsSpan());
            Assert.True(success);
            Assert.Equal<int>(src, dst);
        }

        [Fact]
        public static void TryCopyToBuffer()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101 };

            var srcBuffer = new ReadOnlyBuffer<int>(src);
            var dstBuffer = new Buffer<int>(dst);
            bool success = srcBuffer.TryCopyTo(dstBuffer);
            Assert.True(success);
            Assert.Equal<int>(src, dst);
        }
    }
}
