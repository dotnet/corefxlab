// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Collections.Generic;
using System.Linq;

namespace System.Text.Primitives.Tests
{
    // to run *just* these tests from the \scripts\PerfHarness folder:
    // ..\..\dotnet\dotnet run -c Release --assembly System.Text.Primitives.Tests --perf:typenames System.Text.Primitives.Tests.EncodingPerfComparisonTests
    public class EncodingPerfComparisonTests
    {
        public static IEnumerable<object[]> TestCases()
        {
            int[] lengths = { 1000, 8, 16, 32, 64, 128 };
            (int,int)[] ranges = { (0x0000, 0x007F), (0x0080, 0x07FF), (0x0800, 0xFFFF) };
            return from len in lengths
                   from range in ranges
                   select new object[] { len, range.Item1, range.Item2 };
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void EncodingPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            Encoding utf8 = Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span;

            char[] charArray = characters.ToArray();

            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < limit; i++)
                    {
                        utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
                    }
                }
                span = new Span<byte>(utf8Buffer);
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void EncodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            Assert.True(Utf8Encoder.TryComputeEncodedBytes(characters, out int encodedBytes));
            Assert.Equal(Encoding.UTF8.GetByteCount(unicodeString), encodedBytes);
            byte[] utf8Buffer = new byte[encodedBytes];
            Span<byte> span = new Span<byte>(utf8Buffer);

            Assert.True(Utf8Encoder.TryEncode(characters, span, out int consumed, out encodedBytes));
            Assert.Equal(characters.Length, consumed);
            Assert.Equal(utf8Buffer.Length, encodedBytes);

            Assert.Equal(
                Convert.ToBase64String(Encoding.UTF8.GetBytes(unicodeString)),
                Convert.ToBase64String(utf8Buffer));

            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < limit; i++)
                    {
                        if (!Utf8Encoder.TryEncode(characters, span, out consumed, out encodedBytes))
                        {
                            throw new Exception($"{nameof(Utf8Encoder.TryEncode)} returned false; {nameof(consumed)}={consumed}, {nameof(encodedBytes)}={encodedBytes}"); // this should not happen
                        }
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void EncodingLenPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();
            int bytesNeeded;
            Assert.True(Utf8Encoder.TryComputeEncodedBytes(characters, out bytesNeeded));
            Assert.Equal(Encoding.UTF8.GetByteCount(unicodeString), bytesNeeded);

            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < limit; i++)
                    {
                        if (!Utf8Encoder.TryComputeEncodedBytes(characters, out bytesNeeded))
                        {
                            throw new Exception($"{nameof(Utf8Encoder.TryComputeEncodedBytes)} returned false; {nameof(bytesNeeded)}={bytesNeeded}"); // this should not happen
                        }
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void EncodingLenPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            int bytesNeeded = Encoding.UTF8.GetByteCount(unicodeString);

            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < limit; i++)
                    {
                        bytesNeeded = Encoding.UTF8.GetByteCount(unicodeString);
                    }
                }
            }
        }

        public string GenerateString(int charLength, int minCodePoint, int maxCodePoint)
        {
            Random rand = new Random();
            var plainText = new StringBuilder();
            for (int j = 0; j < charLength; j++)
            {
                var val = rand.Next(minCodePoint, maxCodePoint);
                while (val >= 0xD800 && val <= 0xDFFF)
                {
                    val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void DecodingPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            var bytes = Encoding.UTF8.GetBytes(unicodeString);

            var decoder = Encoding.UTF8;
            Assert.Equal(unicodeString, decoder.GetString(bytes));
            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i=0; i<limit; i++)
                    {
                        decoder.GetString(bytes);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 1000)]
        [MemberData(nameof(TestCases))]
        public void DecodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            var bytes = Encoding.UTF8.GetBytes(unicodeString);

            var decoder = TextEncoder.Utf8;
            Assert.True(decoder.TryDecode(bytes, out string text, out int bytesConsumed));
            Assert.Equal(bytes.Length, bytesConsumed);
            Assert.Equal(unicodeString, text);

            int limit = GetIterationCount(charLength, Benchmark.InnerIterationCount);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i=0; i<limit; i++)
                    {
                        if(!decoder.TryDecode(bytes, out text, out bytesConsumed))
                        {
                            throw new Exception($"{nameof(decoder.TryDecode)} returned false; {nameof(bytesConsumed)}={bytesConsumed}"); // this should not happen
                        }
                    }
                }
            }
        }

        static int GetIterationCount(int charLength, long innerIterationCount)
        {
            int multiplier = 1000 / charLength;
            return (int)(innerIterationCount * multiplier);
        }
    }
}

