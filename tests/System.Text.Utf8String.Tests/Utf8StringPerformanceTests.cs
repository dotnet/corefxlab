// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Xunit.Performance;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Utf8.Tests
{
    public class Utf8StringPerformanceTests
    {
        private string GetRandomString(int length, int minCodePoint, int maxCodePoint)
        {
            Random r = new Random(42);
            StringBuilder sb = new StringBuilder(length);
            while (length-- != 0)
            {
                sb.Append((char)r.Next(minCodePoint, maxCodePoint));
            }
            return sb.ToString();
        }

        [Benchmark(InnerIterationCount = 500)]
        [InlineData(5, 32, 126, "Short ASCII string", true)]
        [InlineData(5, 32, 0xD7FF, "Short string", true)]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void ConstructFromString(int length, int minCodePoint, int maxCodePoint, string description, bool useInnerLoop = false)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < (useInnerLoop ? Benchmark.InnerIterationCount : 1); i++)
                    {
                        utf8s = new Utf8String(s);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 500)]
        [InlineData(5, 32, 126, "Short ASCII string", true)]
        [InlineData(5, 32, 0xD7FF, "Short string", true)]
        [InlineData(50000, 32, 126, "Long ASCII string")]
        [InlineData(50000, 32, 0xD7FF, "Long string")]
        public void EnumerateCodePointsConstructFromByteArray(int length, int minCodePoint, int maxCodePoint, string description, bool useInnerLoop = false)
        {
            string s = GetRandomString(length, minCodePoint, maxCodePoint);
            Utf8String utf8s = new Utf8String(s);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < (useInnerLoop ? Benchmark.InnerIterationCount : 1); i++)
                    {
                        foreach (var codePoint in utf8s)
                        {
                        }
                    }
                }
            }
        }
    }
}
