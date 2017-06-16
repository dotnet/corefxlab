// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.Xunit.Performance;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliPerfomanceTests
    {
        private const int Iter = 1000;

        [Benchmark(InnerIterationCount = Iter)]
        [InlineData(Util.CompressionType.CryptoRandom)]
        [InlineData(Util.CompressionType.RepeatedSegments)]
        [InlineData(Util.CompressionType.NormalData)]
        public void DecompressUsingStream(Util.CompressionType type)
        {
            string testFilePath = Util.CreateCompressedFile(type);
            int bufferSize = 1024;
            var bytes = new byte[bufferSize];
            using (MemoryStream brStream = new MemoryStream(File.ReadAllBytes(testFilePath)))
                foreach (var iteration in Benchmark.Iterations)
                    using (iteration.StartMeasurement())
                        for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        {
                            int retCount = -1;
                            using (BrotliStream brotliDecompressStream = new BrotliStream(brStream, CompressionMode.Decompress, true))
                            {
                                while (retCount != 0)
                                {
                                    retCount = brotliDecompressStream.Read(bytes, 0, bufferSize);
                                }
                            }
                            brStream.Seek(0, SeekOrigin.Begin);
                        }
            File.Delete(testFilePath);
        }

        [Benchmark]
        [InlineData(Util.CompressionType.CryptoRandom)]
        [InlineData(Util.CompressionType.RepeatedSegments)]
        [InlineData(Util.CompressionType.VeryRepetitive)]
        [InlineData(Util.CompressionType.NormalData)]
        public void CompressUsingStream(Util.CompressionType type)
        {
            byte[] bytes = Util.CreateBytesToCompress(type);
            foreach (var iteration in Benchmark.Iterations)
            {
                string filePath = Util.GetTestFilePath();
                FileStream output = File.Create(filePath);
                using (BrotliStream brotliCompressStream = new BrotliStream(output, CompressionMode.Compress))
                {
                    using (iteration.StartMeasurement())
                    {
                        brotliCompressStream.Write(bytes, 0, bytes.Length);
                    }
                }
                File.Delete(filePath);
            }
        }

        [Benchmark(InnerIterationCount = Iter)]
        [InlineData(Util.CompressionType.CryptoRandom)]
        [InlineData(Util.CompressionType.RepeatedSegments)]
        [InlineData(Util.CompressionType.NormalData)]
        public void Decompress(Util.CompressionType type)
        {
            string testFilePath = Util.CreateCompressedFile(type);
            int bufferSize = 1000000;
            byte[] data = File.ReadAllBytes(testFilePath);
            var bytes = new byte[bufferSize];
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Brotli.Decompress(data, bytes, out int consumed, out int written);
            File.Delete(testFilePath);
        }

        [Benchmark]
        [InlineData(Util.CompressionType.CryptoRandom)]
        [InlineData(Util.CompressionType.RepeatedSegments)]
        [InlineData(Util.CompressionType.VeryRepetitive)]
        [InlineData(Util.CompressionType.NormalData)]
        public void Compress(Util.CompressionType type)
        {
            byte[] bytes = Util.CreateBytesToCompress(type);
            foreach (var iteration in Benchmark.Iterations)
            {
                byte[] compressed = new byte[bytes.Length];
                using (iteration.StartMeasurement())
                {
                    Brotli.Compress(bytes, compressed, out int consumed, out int writen);
                }
            }
        }
    }
}
