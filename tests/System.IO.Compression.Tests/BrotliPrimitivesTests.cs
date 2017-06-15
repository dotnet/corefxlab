// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Buffers;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        [Theory]
        [InlineData(25, 1)]
        [InlineData(-1, 1)]
        [InlineData(24, 0)]
        [InlineData(24, 12)]
        public void TestMethodCompressEx(CompressionLevel quality, int lgWinSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Brotli.Compress(new byte[1], new byte[1], out int consumed, out int written, quality, lgWinSize));
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(1023)]
        [InlineData(1024 * 1024)]
        public void RoundtripCompressDecompress(int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            Span<byte> compressed = new byte[Brotli.GetMaximumCompressedSize(totalSize)];
            TransformationStatus result = Brotli.Compress(data, compressed, out int consumed, out int written);
            Assert.Equal(TransformationStatus.Done, result);
            Assert.Equal(totalSize, consumed);
            compressed = compressed.Slice(0, written);
            ValidateCompressedData(compressed, data);
        }

        private void ValidateCompressedData(Span<byte> data, byte[] expected)
        {
            byte[] decompressed = new byte[expected.Length];
            TransformationStatus result = Brotli.Decompress(data, decompressed, out int consumed, out int written);
            Assert.Equal<TransformationStatus>(TransformationStatus.Done, result);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
