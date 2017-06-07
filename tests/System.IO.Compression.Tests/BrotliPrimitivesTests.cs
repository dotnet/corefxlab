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
    class BrotliPrimitivesTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        [Fact]
        public void TestMethodCompressEx()
        {
            int written, consumed;
            Assert.Throws<ArgumentOutOfRangeException>(() => BrotliPrimitives.Compress(new byte[1], new byte[1], out consumed, out written,25,1));
            Assert.Throws<ArgumentOutOfRangeException>(() => BrotliPrimitives.Compress(new byte[1], new byte[1], out consumed, out written, 0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => BrotliPrimitives.Compress(new byte[1], new byte[1], out consumed, out written, 24, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => BrotliPrimitives.Compress(new byte[1], new byte[1], out consumed, out written, 24, 12));
        }

        public static IEnumerable<object[]> RoundtripCompressDecompressOuterData
        {
            get
            {
                yield return new object[] { 1 };
                yield return new object[] { 5 }; // smallest possible writes
                yield return new object[] { 1023 }; // normal
                yield return new object[] { 1024 * 1024 }; // large single write
            }
        }

        [Theory]
        [MemberData(nameof(RoundtripCompressDecompressOuterData))]
        public void RoundtripCompressDecompress(int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            var compressed = new byte[totalSize];
            int consumed, written;
            BrotliPrimitives.Compress(data, compressed, out consumed,out written);
            Assert.Equal(consumed, 0);
            ValidateCompressedData(written, compressed, data);
        }

        private void ValidateCompressedData(int chunkSize, byte[]data, byte[] expected)
        {
            int consumed, written;
            byte[] decompressed = new byte[expected.Length];
            BrotliPrimitives.Decompress(data, decompressed, out consumed, out written);
            Assert.Equal(consumed, 0);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
