// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public class EncodingPerfComparisonTests
    {
        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void EncodingPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            Encoding utf8 = Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span;

            char[] charArray = characters.ToArray();

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
                    }
                }
                span = new Span<byte>(utf8Buffer);
            }
        }

        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
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

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        if (!Utf8Encoder.TryEncode(characters, span, out consumed, out encodedBytes))
                        {
                            throw new Exception($"{nameof(Utf8Encoder.TryEncode)} returned false; {nameof(consumed)}={consumed}, {nameof(encodedBytes)}={encodedBytes}"); // this should not happen
                        }
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void EncodingLenPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();
            int bytesNeeded;
            Assert.True(Utf8Encoder.TryComputeEncodedBytes(characters, out bytesNeeded));
            Assert.Equal(Encoding.UTF8.GetByteCount(unicodeString), bytesNeeded);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        if (!Utf8Encoder.TryComputeEncodedBytes(characters, out bytesNeeded))
                        {
                            throw new Exception($"{nameof(Utf8Encoder.TryComputeEncodedBytes)} returned false; {nameof(bytesNeeded)}={bytesNeeded}"); // this should not happen
                        }
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void EncodingLenPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            int bytesNeeded = Encoding.UTF8.GetByteCount(unicodeString);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
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

        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void DecodingPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            var bytes = Encoding.UTF8.GetBytes(unicodeString);

            var decoder = Encoding.UTF8;
            Assert.Equal(unicodeString, decoder.GetString(bytes));
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i=0; i<Benchmark.InnerIterationCount; i++)
                    {
                        decoder.GetString(bytes);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = 250)]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void DecodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            var bytes = Encoding.UTF8.GetBytes(unicodeString);

            var decoder = TextEncoder.Utf8;
            Assert.True(decoder.TryDecode(bytes, out string text, out int bytesConsumed));
            Assert.Equal(bytes.Length, bytesConsumed);
            Assert.Equal(unicodeString, text);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i=0; i<Benchmark.InnerIterationCount; i++)
                    {
                        if(!decoder.TryDecode(bytes, out text, out bytesConsumed))
                        {
                            throw new Exception($"{nameof(decoder.TryDecode)} returned false; {nameof(bytesConsumed)}={bytesConsumed}"); // this should not happen
                        }
                    }
                }
            }
        }

    }
}

