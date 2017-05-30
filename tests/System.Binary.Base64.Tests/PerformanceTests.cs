// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Binary.Base64.Tests
{
    public class Base64PerformanceTests
    {
        private const int InnerCount = 1000;

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Encode(int numberOfBytes)
        {
            Base64Encoder encoder = new Base64Encoder();
            Span<byte> source = new byte[numberOfBytes];
            Base64TestHelper.InitalizeBytes(source);
            Span<byte> destination = new byte[Base64Encoder.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        encoder.Transform(source, destination, out int consumed, out int written);
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
            Base64TestHelper.InitalizeBytes(source.AsSpan());
            var destination = new char[Base64Encoder.ComputeEncodedLength(numberOfBytes)];

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
            Base64Encoder encoder = new Base64Encoder();
            Base64Decoder decoder = new Base64Decoder();

            Span<byte> source = new byte[numberOfBytes];
            Base64TestHelper.InitalizeBytes(source);
            Span<byte> encoded = new byte[Base64Encoder.ComputeEncodedLength(numberOfBytes)];
            encoder.Transform(source, encoded, out int consumed, out int written);

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        decoder.Transform(encoded, source, out int bytesConsumed, out int bytesWritten);
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
            Base64TestHelper.InitalizeBytes(source.AsSpan());
            char[] encoded = Convert.ToBase64String(source).ToCharArray();

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Convert.FromBase64CharArray(encoded, 0, encoded.Length);
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(1000)]
        [InlineData(5000)]
        [InlineData(10000)]
        [InlineData(20000)]
        [InlineData(50000)]
        private static void StichingTestNoStichingNeeded(int inputBufferSize)
        {
            Base64Decoder decoder = new Base64Decoder();
            Span<byte> source = new byte[inputBufferSize];
            Base64TestHelper.InitalizeDecodableBytes(source);
            Span<byte> expected = new byte[inputBufferSize];
            decoder.Transform(source, expected, out int expectedConsumed, out int expectedWritten);

            Base64TestHelper.SplitSourceIntoSpans(source, false, out ReadOnlySpan<byte> source1, out ReadOnlySpan<byte> source2);

            Span<byte> destination = new byte[inputBufferSize]; // Plenty of space

            int bytesConsumed = 0;
            int bytesWritten = 0;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        Base64TestHelper.DecodeNoNeedToStich(source1, source2, destination, out bytesConsumed, out bytesWritten);
                    }
                }
            }

            Assert.Equal(expectedConsumed, bytesConsumed);
            Assert.Equal(expectedWritten, bytesWritten);
            Assert.True(expected.SequenceEqual(destination));
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(10, 1000)]
        [InlineData(32, 1000)]
        [InlineData(50, 1000)]
        [InlineData(64, 1000)]
        [InlineData(100, 1000)]
        [InlineData(500, 1000)]
        [InlineData(600, 1000)]  // No Third Call
        [InlineData(10, 5000)]
        [InlineData(32, 5000)]
        [InlineData(50, 5000)]
        [InlineData(64, 5000)]
        [InlineData(100, 5000)]
        [InlineData(500, 5000)]
        [InlineData(3000, 5000)]  // No Third Call
        [InlineData(10, 10000)]
        [InlineData(32, 10000)]
        [InlineData(50, 10000)]
        [InlineData(64, 10000)]
        [InlineData(100, 10000)]
        [InlineData(500, 10000)]
        [InlineData(6000, 10000)]  // No Third Call
        [InlineData(10, 20000)]
        [InlineData(32, 20000)]
        [InlineData(50, 20000)]
        [InlineData(64, 20000)]
        [InlineData(100, 20000)]
        [InlineData(500, 20000)]
        [InlineData(12000, 20000)]  // No Third Call
        [InlineData(10, 50000)]
        [InlineData(32, 50000)]
        [InlineData(50, 50000)]
        [InlineData(64, 50000)]
        [InlineData(100, 50000)]
        [InlineData(500, 50000)]
        [InlineData(30000, 50000)]  // No Third Call
        private static void StichingTestStichingRequired(int stackSize, int inputBufferSize)
        {
            Base64Decoder decoder = new Base64Decoder();
            Span<byte> source = new byte[inputBufferSize];
            Base64TestHelper.InitalizeDecodableBytes(source);
            Span<byte> expected = new byte[inputBufferSize];
            decoder.Transform(source, expected, out int expectedConsumed, out int expectedWritten);

            Base64TestHelper.SplitSourceIntoSpans(source, true, out ReadOnlySpan<byte> source1, out ReadOnlySpan<byte> source2);

            Span<byte> destination = new byte[inputBufferSize]; // Plenty of space

            Span<byte> stackSpan;
            unsafe
            {
                byte* stackBytes = stackalloc byte[stackSize];
                stackSpan = new Span<byte>(stackBytes, stackSize);
            }

            int bytesConsumed = 0;
            int bytesWritten = 0;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        Base64TestHelper.DecodeStichUsingStack(source1, source2, destination, stackSpan, out bytesConsumed, out bytesWritten);
                    }
                }
            }

            Assert.Equal(expectedConsumed, bytesConsumed);
            Assert.Equal(expectedWritten, bytesWritten);
            Assert.True(expected.SequenceEqual(destination));
        }
    }
}
