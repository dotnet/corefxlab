// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Slices.Tests
{
    public class PerformanceTests
    {
        [Benchmark(InnerIterationCount = 10000000)]
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
        public int SpanIndexOfWithIndexAndCountComparisonValueAtEnd(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[length - 1] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = length / 2;
            int count = length - startIndex;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.IndexOf(lookupVal, startIndex, count);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
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
        public int SpanIndexOfWithSliceComparisonValueAtEnd(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[length - 1] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = length / 2;
            int count = length - startIndex;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.Slice(startIndex, count).IndexOf(lookupVal);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
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
        public int SpanIndexOfWithIndexAndCountComparisonValueAtStart(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[0] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = 0;
            int count = length - startIndex;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.IndexOf(lookupVal, startIndex, count);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
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
        public int SpanIndexOfWithSliceComparisonValueAtStart(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[0] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = 0;
            int count = length - startIndex;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.Slice(startIndex, count).IndexOf(lookupVal);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
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
        public int SpanIndexOfWithIndexAndCountComparisonValueInMiddle(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[length / 2] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = length / 4;
            int count = length / 2 + 1;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.IndexOf(lookupVal, startIndex, count);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
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
        public int SpanIndexOfWithSliceComparisonValueInMiddle(int length)
        {
            const byte lookupVal = 99;
            var a = new byte[length];
            a[length / 2] = lookupVal;

            var bytes = new Span<byte>(a);
            int startIndex = length / 4;
            int count = length / 2 + 1;
            var result = -1;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        result = bytes.Slice(startIndex, count).IndexOf(lookupVal);
                    }
                }
            }
            Console.WriteLine(result);
            return result;
        }
    }
}
