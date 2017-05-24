// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Binary.Base64.Tests
{
    public class Base64PerformanceTests
    {
        private const int InnerCount = 10;

        static void InitalizeBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rnd.Next(0, byte.MaxValue + 1);
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Encode(int numberOfBytes)
        {
            Span<byte> source = new byte[numberOfBytes];
            InitalizeBytes(source);
            Span<byte> destination = new byte[Base64.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Base64.Encode(source, destination);
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64EncodeBaseline(int numberOfBytes)
        {
            var source = new byte[numberOfBytes];
            InitalizeBytes(source.AsSpan());
            var destination = new char[Base64.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Convert.ToBase64CharArray(source, 0, source.Length, destination, 0);
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Decode(int numberOfBytes)
        {
            Span<byte> source = new byte[numberOfBytes];
            InitalizeBytes(source);
            Span<byte> encoded = new byte[Base64.ComputeEncodedLength(numberOfBytes)];
            var encodedBytesCount = Base64.Encode(source, encoded);

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Base64.Decode(encoded, source);
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64DecodeBaseline(int numberOfBytes)
        {
            var source = new byte[numberOfBytes];
            InitalizeBytes(source.AsSpan());
            char[] encoded = Convert.ToBase64String(source).ToCharArray();

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Convert.FromBase64CharArray(encoded, 0, encoded.Length);
                }
            }
        }
    }
}
