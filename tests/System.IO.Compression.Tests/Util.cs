// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography;

namespace System.IO.Compression.Tests
{
    public static class Util
    {
        public enum CompressionType
        {
            CryptoRandom,
            RepeatedSegments,
            VeryRepetitive,
            NormalData
        }

        public static string GetTestFilePath(int? index = null, string memberName = null, int lineNumber = 0)
        {
            return Path.Combine(Path.GetTempPath(), string.Format(
                index.HasValue ? "{0}_{1}_{2}_{3}" : "{0}_{1}_{2}",
                memberName ?? "TestBase", lineNumber, Path.GetRandomFileName(),
                index.GetValueOrDefault()));
        }

        internal static string CreateCompressedFile(CompressionType type)
        {
            const int fileSize = 1000000;
            string filePath = Util.GetTestFilePath() + ".br";
            switch (type)
            {
                case CompressionType.CryptoRandom:
                    using (RandomNumberGenerator rand = RandomNumberGenerator.Create())
                    {
                        byte[] bytes = new byte[fileSize];
                        rand.GetBytes(bytes);
                        using (FileStream output = File.Create(filePath))
                        using (BrotliStream zip = new BrotliStream(output, CompressionMode.Compress))
                            zip.Write(bytes, 0, bytes.Length);
                    }
                    break;
                case CompressionType.RepeatedSegments:
                    {
                        byte[] bytes = new byte[fileSize / 1000];
                        new Random(128453).NextBytes(bytes);
                        using (FileStream output = File.Create(filePath))
                        using (BrotliStream zip = new BrotliStream(output, CompressionMode.Compress))
                            for (int i = 0; i < 1000; i++)
                                zip.Write(bytes, 0, bytes.Length);
                    }
                    break;
                case CompressionType.NormalData:
                    {
                        byte[] bytes = new byte[fileSize];
                        new Random(128453).NextBytes(bytes);
                        using (FileStream output = File.Create(filePath))
                        using (BrotliStream zip = new BrotliStream(output, CompressionMode.Compress))
                            zip.Write(bytes, 0, bytes.Length);
                    }
                    break;
            }
            return filePath;
        }

        internal static byte[] CreateBytesToCompress(CompressionType type)
        {
            const int fileSize = 500000;
            byte[] bytes = new byte[fileSize];
            switch (type)
            {
                case CompressionType.CryptoRandom:
                    using (RandomNumberGenerator rand = RandomNumberGenerator.Create())
                        rand.GetBytes(bytes);
                    break;
                case CompressionType.RepeatedSegments:
                    {
                        byte[] small = new byte[1000];
                        new Random(123453).NextBytes(small);
                        for (int i = 0; i < fileSize / 1000; i++)
                        {
                            small.CopyTo(bytes, 1000 * i);
                        }
                    }
                    break;
                case CompressionType.VeryRepetitive:
                    {
                        byte[] small = new byte[100];
                        new Random(123453).NextBytes(small);
                        for (int i = 0; i < fileSize / 100; i++)
                        {
                            small.CopyTo(bytes, 100 * i);
                        }
                        break;
                    }
                case CompressionType.NormalData:
                    new Random(123453).NextBytes(bytes);
                    break;
            }
            return bytes;
        }
    }
}
