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

        private const byte LookupVal = 99;

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void GetData(int length, int location, out Span<byte> bytes, out int startIndex, out int count)
        {
            // location = 0 - value is at start
            // location = 1 - value is at middle
            // location = 2 - value is at end
            byte[] a = GetArray(length);

            switch (location)
            {
                case 0:
                    a[0] = LookupVal;
                    startIndex = 0;
                    count = length;
                    break;
                case 1:
                    a[length / 2] = LookupVal;
                    startIndex = length / 4;
                    count = length / 2 + 1;
                    break;
                default:
                    a[length - 1] = LookupVal;
                    startIndex = length / 2;
                    count = length - startIndex;
                    break;
            }

            bytes = new Span<byte>(a);
        }
        
        private static byte[] GetArray(int length)
        {
            var rand = new Random(42);
            var a = new byte[length];
            for (int i = 0; i < a.Length; i++)
            {
                byte temp = (byte)rand.Next(0, 255);
                if (temp == LookupVal)
                {
                    temp++;
                }
                a[i] = temp;
            }
            return a;
        }
        
        private static void TestBenchIndexOf(Span<byte> bytes, int startIndex, int count)
        {
            var temp = bytes.IndexOf(LookupVal, startIndex, count);
            if (temp == -1)
            {
                Console.WriteLine(temp);
            }
        }
        
        private static void TestBenchSlice(Span<byte> bytes, int startIndex, int count)
        {
            var temp = bytes.Slice(startIndex, count).IndexOf(LookupVal);
            if (temp == -1)
            {
                Console.WriteLine(temp);
            }
        }

        [Benchmark(InnerIterationCount = 1000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueAtEnd(int length)
        {
            GetData(length, 2, out Span<byte> bytes, out int startIndex, out int count);

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

        [Benchmark(InnerIterationCount = 1000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithSliceComparisonValueAtEnd(int length)
        {
            GetData(length, 2, out Span<byte> bytes, out int startIndex, out int count);

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

        [Benchmark(InnerIterationCount = 10000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueAtStart(int length)
        {
            GetData(length, 0, out Span<byte> bytes, out int startIndex, out int count);

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

        [Benchmark(InnerIterationCount = 10000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithSliceComparisonValueAtStart(int length)
        {
            GetData(length, 0, out Span<byte> bytes, out int startIndex, out int count);

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

        [Benchmark(InnerIterationCount = 1000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithIndexAndCountComparisonValueInMiddle(int length)
        {
            GetData(length, 1, out Span<byte> bytes, out int startIndex, out int count);

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

        [Benchmark(InnerIterationCount = 1000000)]
        [InlineData(1000)]
        public static void SpanIndexOfWithSliceComparisonValueInMiddle(int length)
        {
            GetData(length, 1, out Span<byte> bytes, out int startIndex, out int count);

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
