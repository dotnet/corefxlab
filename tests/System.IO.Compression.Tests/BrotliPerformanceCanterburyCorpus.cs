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
                string folder = "Canterbury";
                foreach (int innerIterations in new int[] { 1, 10 })
                {
                    yield return new object[] { innerIterations, folder, "alice29.txt", compressionLevel };
                    yield return new object[] { innerIterations, folder, "asyoulik.txt", compressionLevel };
                    yield return new object[] { innerIterations, folder, "cp.html", compressionLevel };
                    yield return new object[] { innerIterations, folder, "fields.c", compressionLevel };
                    yield return new object[] { innerIterations, folder, "grammar.lsp", compressionLevel };
                    yield return new object[] { innerIterations, folder, "kennedy.xls", compressionLevel };
                    yield return new object[] { innerIterations, folder, "lcet10.txt", compressionLevel };
                    yield return new object[] { innerIterations, folder, "plrabn12.txt", compressionLevel };
                    yield return new object[] { innerIterations, folder, "ptt5", compressionLevel };
                    yield return new object[] { innerIterations, folder, "sum", compressionLevel };
                    yield return new object[] { innerIterations, folder, "xargs.1", compressionLevel };
                }
                folder = "WebFiles";
                foreach (int innerIterations in new int[] { 1, 10 })
                {
                    yield return new object[] { innerIterations, folder, "angular.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "angular.min.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "broker-config.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "config.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "jquery-3.2.1.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "jquery-3.2.1.min.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "meBoot.min.js", compressionLevel };
                    yield return new object[] { innerIterations, folder, "MWFMDL2.woff", compressionLevel };
                    yield return new object[] { innerIterations, folder, "mwf-west-european-default.min.css", compressionLevel };
                    yield return new object[] { innerIterations, folder, "style.css", compressionLevel };
                    yield return new object[] { innerIterations, folder, "uhf-west-european-default.min.css", compressionLevel };
                    yield return new object[] { innerIterations, folder, "www.reddit.com6.23.2017.har", compressionLevel };
                }
            }
        }

        public static void CreateCompressFile(string source, string destiny, CompressionLevel lvl)
        {
            byte[] bytes = File.ReadAllBytes(source);
            FileStream ms = File.Create(destiny);
            BrotliStream bs = new BrotliStream(ms, CompressionMode.Compress, true, (1 << 16) - 1, lvl);
            bs.Write(bytes, 0, bytes.Length);
            bs.Dispose();
            ms.Dispose();
        }
        /// <summary>
        /// Benchmark tests to measure the performance of individually compressing each file in the
        /// Canterbury Corpus
        /// </summary>
        [Benchmark]
        [MemberData(nameof(CanterburyCorpus))]
        public void Compress_Canterbury(int innerIterations, string folder, string fileName, CompressionLevel compressLevel)
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
        public void Decompress_Canterbury(int innerIterations, string folder, string fileName, CompressionLevel level)
        {
            string zipFilePath = Path.Combine("BrotliTestData", folder, "BrotliCompressed", fileName+level.ToString() + ".br");
            string sourceFilePath = Path.Combine("BrotliTestData", folder, fileName);
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
