// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public class EncodingPerfComparisonTests
    {
        [Benchmark]
        [InlineData(false, 1000, 0x0020, 0x007F)]
        [InlineData(false, 1000, 0x0080, 0x07FF)]
        [InlineData(false, 1000, 0x0800, 0xd7FF)]
        [InlineData(false, 1000, 0x0020, 0xd7FF)]
        [InlineData(true, 1000, 0x0020, 0x007F)]
        [InlineData(true, 1000, 0x0080, 0x07FF)]
        [InlineData(true, 1000, 0x0800, 0xd7FF)]
        [InlineData(true, 1000, 0x0020, 0xd7FF)]
        public void EncodingPerformanceTestUsingCoreCLR(bool convertToArrayInLoop, int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.Slice();

            Encoding utf8 = Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span;

            char[] charArray = characters.ToArray();

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (convertToArrayInLoop)
                        utf8.GetBytes(characters.ToArray(), 0, characters.Length, utf8Buffer, 0);
                    else
                        utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
                span = new Span<byte>(utf8Buffer);
            }
        }

        [Benchmark]
        [InlineData(1000, 0x0020, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xd7FF)]
        [InlineData(1000, 0x0020, 0xd7FF)]
        public void EncodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.Slice();

            int encodedBytes;

            int utf8Length = Utf8Encoder.ComputeEncodedBytes(characters);
            byte[] utf8Buffer = new byte[utf8Length];
            Span<byte> span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (!Utf8Encoder.TryEncode(characters, span, out encodedBytes))
                    {
                        throw new Exception(); // this should not happen
                    }
            }
        }

        public string GenerateString(int charLength, int minCodePoint, int maxCodePoint)
        {
            Random rand = new Random();
            var plainText = new StringBuilder();
            for (int j = 0; j < charLength; j++)
            {
                plainText.Append((char)rand.Next(minCodePoint, maxCodePoint));
            }
            return plainText.ToString();
        }
    }
}

