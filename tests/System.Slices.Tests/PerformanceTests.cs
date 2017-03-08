// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Xunit.Performance;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Slices.Tests
{
    public class PerformanceTests
    {
        [Benchmark(InnerIterationCount = 1000000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public void SpanStartsWith(int length)
        {
            byte[] a = new byte[length];
            byte[] b = {1, 2, 3, 4, 5};

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (byte)(i + 1);
            }

            var bytes = new ReadOnlySpan<byte>(a);
            var slice = new ReadOnlySpan<byte>(b, 0, Math.Min(length, b.Length));

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        bool result = bytes.StartsWith(slice);
                    }
                }
            }
        }

        private static volatile int _sResult = -1;
        private const byte LookupVal = 99;

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void GetDataValueAtEnd(int length, out Span<byte> bytes, out int startIndex, out int count)
        {
            var a = new byte[length];
            a[length - 1] = LookupVal;
            bytes = new Span<byte>(a);
            startIndex = length / 2;
            count = length - startIndex;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void GetDataValueAtStart(int length, out Span<byte> bytes, out int startIndex, out int count)
        {
            var a = new byte[length];
            a[0] = LookupVal;
            bytes = new Span<byte>(a);
            startIndex = 0;
            count = length - startIndex;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void GetDataValueAtMiddle(int length, out Span<byte> bytes, out int startIndex, out int count)
        {
            var a = new byte[length];
            a[length / 2] = LookupVal;
            bytes = new Span<byte>(a);
            startIndex = length / 4;
            count = length / 2 + 1;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestBenchIndexOf(Span<byte> bytes, int startIndex, int count)
        {
            _sResult += bytes.IndexOf(LookupVal, startIndex, count);
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void TestBenchSlice(Span<byte> bytes, int startIndex, int count)
        {
            _sResult += bytes.Slice(startIndex, count).IndexOf(LookupVal);
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueAtEnd(int length)
        {
            GetDataValueAtEnd(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchIndexOf(bytes, startIndex, count);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithSliceComparisonValueAtEnd(int length)
        {
            GetDataValueAtEnd(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchSlice(bytes, startIndex, count);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueAtStart(int length)
        {
            GetDataValueAtStart(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchIndexOf(bytes, startIndex, count);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithSliceComparisonValueAtStart(int length)
        {
            GetDataValueAtStart(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchSlice(bytes, startIndex, count);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueInMiddle(int length)
        {
            GetDataValueAtMiddle(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchIndexOf(bytes, startIndex, count);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 100000)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(5000)]
        public static void SpanIndexOfWithSliceComparisonValueInMiddle(int length)
        {
            GetDataValueAtMiddle(length, out Span<byte> bytes, out int startIndex, out int count);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        TestBenchSlice(bytes, startIndex, count);
                    }
                }
            }
        }
    }
}
