// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliPrimitivesTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        [Theory]
        [InlineData(25, 1)]
        [InlineData(0, 1)]
        [InlineData(24, 0)]
        [InlineData(24, 12)]
        public void TestMethodCompressEx(int quality, int lgwin)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BrotliPrimitives.Compress(new byte[1], new byte[1], out int consumed, out int written, quality, lgwin));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(1023)]
        [InlineData(1024 * 1024)]
        public void RoundtripCompressDecompress(int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            Span<byte> compressed = new byte[BrotliPrimitives.GetMaximumCompressedSize(totalSize)];
            bool res = BrotliPrimitives.Compress(data, compressed, out int consumed, out int written);
            Assert.Equal(true, res);
            Assert.Equal(totalSize, consumed);
            compressed = compressed.Slice(0, written);
            ValidateCompressedData(compressed, data);
        }

        private void ValidateCompressedData(Span<byte> data, byte[] expected)
        {
            byte[] decompressed = new byte[expected.Length];
            BrotliDecoderResult res = BrotliPrimitives.Decompress(data, decompressed, out int consumed, out int written);
            Assert.Equal<BrotliDecoderResult>(BrotliDecoderResult.Success, res);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
