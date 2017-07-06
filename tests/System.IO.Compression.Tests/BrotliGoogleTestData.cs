// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression.Resources;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliGoogleTestData
    {
        private int bufferSize = 8192;
        private const string TestDataFilesFolder = "GoogleTestData";

        public static IEnumerable<object[]> TestData()
        {
            foreach (CompressionLevel compressionLevel in Enum.GetValues(typeof(CompressionLevel)))
            {
                yield return new object[] { "10x10y", compressionLevel };
                yield return new object[] { "64x", compressionLevel };
                yield return new object[] { "backward65536", compressionLevel };
                yield return new object[] { "compressed_file", compressionLevel };
                yield return new object[] { "compressed_repeated", compressionLevel };
                yield return new object[] { "empty", compressionLevel };
                yield return new object[] { "mapsdatazrh", compressionLevel };
                yield return new object[] { "monkey", compressionLevel };
                yield return new object[] { "plrabn12.txt", compressionLevel };
                yield return new object[] { "quickfox", compressionLevel };
                yield return new object[] { "quickfox_repeated", compressionLevel };
                yield return new object[] { "random_org_10k.bin", compressionLevel };
                yield return new object[] { "x", compressionLevel };
                yield return new object[] { "ukkonooa", compressionLevel };
                yield return new object[] { "xyzzy", compressionLevel };
                yield return new object[] { "zeros", compressionLevel };
            }
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void DecompressFile(string fileName, CompressionLevel compressionLevel)
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("BrotliTestData", TestDataFilesFolder, fileName + ".compressed"));
            byte[] expected = File.ReadAllBytes(Path.Combine("BrotliTestData", TestDataFilesFolder, fileName));
            
            ValidateCompressedData(bytes, expected);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void RoundtripCompressDecompressFile(string fileName, CompressionLevel compressionLevel)
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("BrotliTestData", TestDataFilesFolder, fileName));
            MemoryStream memoryStream = new MemoryStream();
            using (BrotliStream brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true, bufferSize, compressionLevel))
            {
                brotliStream.Write(bytes, 0, bytes.Length);
                brotliStream.Dispose();
            }
            memoryStream.Position = 0;
            ValidateCompressedData(memoryStream.ToArray(), bytes);
            memoryStream.Dispose();
        }

        private void ValidateCompressedData(byte[] compressedData, byte[] expected)
        {
            MemoryStream compressed = new MemoryStream(compressedData);
            using (MemoryStream decompressed = new MemoryStream())
            using (var decompressor = new BrotliStream(compressed, CompressionMode.Decompress, true))
            {
                decompressor.CopyTo(decompressed);
                Assert.Equal(expected.Length, decompressed.ToArray().Length);
                Assert.Equal<byte>(expected, decompressed.ToArray());
            }
        }
    }
    
}
