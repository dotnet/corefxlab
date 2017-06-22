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

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(1023)]
        [InlineData(1024 * 1024)]
        public void RoundtripCompressDecompress(int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            byte[] compressed = new byte[Brotli.GetMaximumCompressedSize(totalSize)];
            Assert.NotEqual(compressed.Length, 0);
            Brotli.State state = new Brotli.State();
            state.InitializeEncoder();
            TransformationStatus result = Brotli.Compress(data, compressed, out int consumed, out int written, ref state);
            while (consumed != 0 || result != TransformationStatus.Done)
            {
                result = Brotli.Compress(data, compressed, out consumed, out written, ref state);
            }
            byte[] flush = new byte[0];
            result = Brotli.FlushEncoder(flush, compressed, out consumed, out written, ref state);
            Assert.Equal(TransformationStatus.Done, result);
            Assert.Equal(consumed, 0);
            byte[] resultCompressed = new byte[written];
            Array.Copy(compressed, resultCompressed, written);
            ValidateCompressedData(resultCompressed, data);
        }

        private void ValidateCompressedData(byte[] data, byte[] expected)
        {
            byte[] decompressed = new byte[expected.Length];
            Brotli.State state = new Brotli.State();
            state.InitializeDecoder();
            TransformationStatus result = Brotli.Decompress(data, decompressed, out int consumed, out int written, ref state);
            Assert.Equal<TransformationStatus>(TransformationStatus.Done, result);
            Assert.Equal<long>(expected.Length, written);
            Assert.Equal<long>(consumed, 0);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
