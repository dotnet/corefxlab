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

        [Benchmark(InnerIterationCount = 10000000)]
        [InlineData(1000)]
        public void SpanIndexOfWithIndexAndCountComparison(int length)
        {
            byte[] a = new byte[length];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (byte)(i + 1);
            }

            var bytes = new Span<byte>(a);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int result = bytes.IndexOf(250, 10, 255);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 10000000)]
        [InlineData(1000)]
        public void SpanIndexOfWithSliceComparison(int length)
        {
            byte[] a = new byte[length];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (byte)(i + 1);
            }

            var bytes = new Span<byte>(a);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int result = bytes.Slice(250, 10).IndexOf(255);
                    }
                }
            }
        }
    }
}
