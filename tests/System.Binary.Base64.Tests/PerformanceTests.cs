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

        // Pre-computing this table using a custom string(s_characters) and GenerateEncodingMapAndVerify (found in tests)
        static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,         //A..H
            73, 74, 75, 76, 77, 78, 79, 80,         //I..P
            81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
            89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
            103, 104, 105, 106, 107, 108, 109, 110, //g..n
            111, 112, 113, 114, 115, 116, 117, 118, //o..v
            119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
            52, 53, 54, 55, 56, 57, 43, 47,         //4..9, +, /
            61                                      // =
        };

        static void InitalizeBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rnd.Next(0, byte.MaxValue + 1);
            }
        }

        static void InitalizeDecodableBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                int index = (byte)rnd.Next(0, s_encodingMap.Length - 1);    // Do not pick '='
                bytes[i] = s_encodingMap[index];
            }
        }

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Encode(int numberOfBytes)
        {
            Span<byte> source = new byte[numberOfBytes];
            InitalizeBytes(source);
            Span<byte> destination = new byte[Base64Encoder.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Base64Encoder.TryEncode(source, destination, out int consumed, out int written);
                }
            }
        }

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64EncodeBaseline(int numberOfBytes)
        {
            var source = new byte[numberOfBytes];
            InitalizeBytes(source.AsSpan());
            var destination = new char[Base64Encoder.ComputeEncodedLength(numberOfBytes)];

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Convert.ToBase64CharArray(source, 0, source.Length, destination, 0);
                }
            }
        }

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1000 * 1000)]
        [InlineData(1000 * 1000 * 50)]
        private static void Base64Decode(int numberOfBytes)
        {
            Span<byte> source = new byte[numberOfBytes];
            InitalizeBytes(source);
            Span<byte> encoded = new byte[Base64Encoder.ComputeEncodedLength(numberOfBytes)];
            Base64Encoder.TryEncode(source, encoded, out int consumed, out int written);

            foreach (var iteration in Benchmark.Iterations) {
                using (iteration.StartMeasurement()) {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Base64Encoder.TryDecode(encoded, source, out int bytesConsumed, out int bytesWritten);
                }
            }
        }

        //[Benchmark(InnerIterationCount = InnerCount)]
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


        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(10, 1000)]
        [InlineData(32, 1000)]
        [InlineData(50, 1000)]
        [InlineData(64, 1000)]
        [InlineData(100, 1000)]
        [InlineData(500, 1000)]
        [InlineData(10, 10000)]
        [InlineData(32, 10000)]
        [InlineData(50, 10000)]
        [InlineData(64, 10000)]
        [InlineData(100, 10000)]
        [InlineData(500, 10000)]
        private static void StichingTestNoStichingNeeded(int stackSize, int inputBufferSize)
        {
            Span<byte> source = new byte[inputBufferSize];
            InitalizeDecodableBytes(source);

            int alignedBoundary = inputBufferSize / 5 * 2;  // 1000 -> 400
            ReadOnlySpan<byte> source1 = source.Slice(0, alignedBoundary);
            ReadOnlySpan<byte> source2 = source.Slice(alignedBoundary, inputBufferSize - alignedBoundary);

            Span<byte> destination = new byte[inputBufferSize]; // Plenty of space

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int afterMergeSlice = 0;
                        if (!Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
                        {
                            int leftOverBytes = source1.Length - bytesConsumed;
                            if (leftOverBytes < 4)
                            {
                                Span<byte> stackSpan;

                                unsafe
                                {
                                    byte* stackBytes = stackalloc byte[stackSize];
                                    stackSpan = new Span<byte>(stackBytes, stackSize);
                                }

                                source1.Slice(bytesConsumed).CopyTo(stackSpan);
                                int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);
                                source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                                Base64Encoder.TryDecode(stackSpan, destination.Slice(bytesWritten), out bytesConsumed, out bytesWritten);
                                afterMergeSlice = bytesConsumed - leftOverBytes;
                            }
                        }
                        Base64Encoder.TryDecode(source2.Slice(afterMergeSlice), destination.Slice(bytesWritten), out bytesConsumed, out bytesWritten);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(10, 1000)]
        [InlineData(32, 1000)]
        [InlineData(50, 1000)]
        [InlineData(64, 1000)]
        [InlineData(100, 1000)]
        [InlineData(500, 1000)]
        [InlineData(10, 10000)]
        [InlineData(32, 10000)]
        [InlineData(50, 10000)]
        [InlineData(64, 10000)]
        [InlineData(100, 10000)]
        [InlineData(500, 10000)]
        private static void StichingTestStichingRequired(int stackSize, int inputBufferSize)
        {
            Span<byte> source = new byte[inputBufferSize];
            InitalizeDecodableBytes(source);

            int misalignedBoundary = inputBufferSize / 5 * 2 + 2;  // 1000 -> 402
            ReadOnlySpan<byte> source1 = source.Slice(0, misalignedBoundary);
            ReadOnlySpan<byte> source2 = source.Slice(misalignedBoundary, inputBufferSize - misalignedBoundary);

            Span<byte> destination = new byte[inputBufferSize]; // Plenty of space

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int afterMergeSlice = 0;
                        if (!Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
                        {
                            int leftOverBytes = source1.Length - bytesConsumed;
                            if (leftOverBytes < 4)
                            {
                                Span<byte> stackSpan;

                                unsafe
                                {
                                    byte* stackBytes = stackalloc byte[stackSize];
                                    stackSpan = new Span<byte>(stackBytes, stackSize);
                                }

                                source1.Slice(bytesConsumed).CopyTo(stackSpan);
                                int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);
                                source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                                Base64Encoder.TryDecode(stackSpan, destination.Slice(bytesWritten), out bytesConsumed, out bytesWritten);
                                afterMergeSlice = bytesConsumed - leftOverBytes;
                            }
                        }
                        Base64Encoder.TryDecode(source2.Slice(afterMergeSlice), destination.Slice(bytesWritten), out bytesConsumed, out bytesWritten);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(1000)]
        [InlineData(10000)]
        private static void StichingTestNoThirdCall(int inputBufferSize)
        {
            Span<byte> source = new byte[inputBufferSize];
            InitalizeDecodableBytes(source);

            int misalignedBoundary = inputBufferSize / 5 * 2 + 2;  // 1000 -> 402
            ReadOnlySpan<byte> source1 = source.Slice(0, misalignedBoundary);
            ReadOnlySpan<byte> source2 = source.Slice(misalignedBoundary, inputBufferSize - misalignedBoundary);

            Span<byte> destination = new byte[inputBufferSize]; // Plenty of space

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        if (Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
                        {
                            int leftOverBytes = source1.Length - bytesConsumed;
                            if (leftOverBytes < 4)
                            {
                                Span<byte> stackSpan;

                                unsafe
                                {
                                    int completeSecondBufferSize = inputBufferSize / 5 * 3; // 1000 -> 600;
                                    byte* stackBytes = stackalloc byte[completeSecondBufferSize];
                                    stackSpan = new Span<byte>(stackBytes, completeSecondBufferSize);
                                }

                                source1.Slice(bytesConsumed).CopyTo(stackSpan);
                                int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);
                                source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                                Base64Encoder.TryDecode(stackSpan, destination.Slice(bytesWritten), out bytesConsumed, out bytesWritten);
                            }
                        }
                    }
                }
            }
        }
    }
}
