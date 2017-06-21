// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.Xunit.Performance;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliPerformanceCanterburyCorpus
    {
        int bufferSize = 8192;

        public static IEnumerable<object[]> CanterburyCorpus()
        {
            foreach (CompressionLevel compressionLevel in Enum.GetValues(typeof(CompressionLevel)))
            {
                foreach (int innerIterations in new int[] { 1, 10 })
                {
                    yield return new object[] { innerIterations, "alice29.txt", compressionLevel };
                    yield return new object[] { innerIterations, "asyoulik.txt", compressionLevel };
                    yield return new object[] { innerIterations, "cp.html", compressionLevel };
                    yield return new object[] { innerIterations, "fields.c", compressionLevel };
                    yield return new object[] { innerIterations, "grammar.lsp", compressionLevel };
                    yield return new object[] { innerIterations, "kennedy.xls", compressionLevel };
                    yield return new object[] { innerIterations, "lcet10.txt", compressionLevel };
                    yield return new object[] { innerIterations, "plrabn12.txt", compressionLevel };
                    yield return new object[] { innerIterations, "ptt5", compressionLevel };
                    yield return new object[] { innerIterations, "sum", compressionLevel };
                    yield return new object[] { innerIterations, "xargs.1", compressionLevel };
                }
            }
        }

        /// <summary>
        /// Benchmark tests to measure the performance of individually compressing each file in the
        /// Canterbury Corpus
        /// </summary>
        [Benchmark]
        [MemberData(nameof(CanterburyCorpus))]
        public void Compress_Canterbury(int innerIterations, string fileName, CompressionLevel compressLevel)
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("BrotliTestData", "Canterbury", fileName));
            FileStream[] fileStreams = new FileStream[innerIterations];
            BrotliStream[] brotliStream = new BrotliStream[innerIterations];
            string[] paths = new string[innerIterations];
            foreach (var iteration in Benchmark.Iterations)
            {
                for (int i = 0; i < innerIterations; i++)
                {
                    paths[i] = Util.GetTestFilePath();
                    fileStreams[i] = File.Create(paths[i]);
                }
                using (iteration.StartMeasurement())
                    for (int i = 0; i < innerIterations; i++)
                    {
                        brotliStream[i] = new BrotliStream(fileStreams[i], CompressionMode.Compress, true, bufferSize, compressLevel);
                        brotliStream[i].Write(bytes, 0, bytes.Length);
                        brotliStream[i].Flush();
                        brotliStream[i].Dispose();
                        fileStreams[i].Dispose();
                    }
                for (int i = 0; i < innerIterations; i++)
                    File.Delete(paths[i]);
            }
        }

        /// <summary>
        /// Benchmark tests to measure the performance of individually decompressing each file in the
        /// Canterbury Corpus
        /// </summary>
        [Benchmark]
        [MemberData(nameof(CanterburyCorpus))]
        public void Decompress_Canterbury(int innerIterations, string fileName, CompressionLevel level)
        {
            string zipFilePath = Path.Combine("BrotliTestData", "Canterbury", "BrotliCompressed", fileName + level.ToString() + ".br");
            string sourceFilePath = Path.Combine("BrotliTestData", "Canterbury", fileName);
            byte[] outputRead = new byte[new FileInfo(sourceFilePath).Length];
            MemoryStream[] memories = new MemoryStream[innerIterations];
            foreach (var iteration in Benchmark.Iterations)
            {
                for (int i = 0; i < innerIterations; i++)
                    memories[i] = new MemoryStream(File.ReadAllBytes(zipFilePath));

                using (iteration.StartMeasurement())
                    for (int i = 0; i < innerIterations; i++)
                        using (BrotliStream decompressBrotli = new BrotliStream(memories[i], CompressionMode.Decompress))
                            decompressBrotli.Read(outputRead, 0, outputRead.Length);

                for (int i = 0; i < innerIterations; i++)
                    memories[i].Dispose();
            }
        }
    }
}
