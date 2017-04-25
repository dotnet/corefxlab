﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public class EncodingPerfComparisonTests
    {
        [Benchmark]
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
                    utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
                span = new Span<byte>(utf8Buffer);
            }
        }

        [Benchmark]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void EncodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            int consumed;
            int encodedBytes;
            Assert.True(Utf8Encoder.TryComputeEncodedBytes(characters, out encodedBytes));
            byte[] utf8Buffer = new byte[encodedBytes];
            Span<byte> span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (!Utf8Encoder.TryEncode(characters, span, out consumed, out encodedBytes))
                    {
                        throw new Exception(); // this should not happen
                    }
            }
        }


        [Benchmark]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void DecodingPerformanceTestUsingCoreCLR(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            Encoding utf8 = Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];

            char[] charArray = characters.ToArray();
            utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
            Span<byte> span = new Span<byte>(utf8Buffer);
            Span<char> charSpan = span.NonPortableCast<byte, char>();
            char[] charSpanToArray = charSpan.ToArray();

            Encoding unicode = Encoding.UTF32;
            int uint32Length = unicode.GetByteCount(charSpanToArray);
            byte[] uint32Buffer = new byte[uint32Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    unicode.GetBytes(charSpanToArray, 0, charSpanToArray.Length, uint32Buffer, 0);
            }
        }

        [Benchmark]
        [InlineData(1000, 0x0000, 0x007F)]
        [InlineData(1000, 0x0080, 0x07FF)]
        [InlineData(1000, 0x0800, 0xFFFF)]
        public void DecodingPerformanceTestUsingCorefxlab(int charLength, int minCodePoint, int maxCodePoint)
        {
            string unicodeString = GenerateString(charLength, minCodePoint, maxCodePoint);
            ReadOnlySpan<char> characters = unicodeString.AsSpan();

            Encoding utf8 = Encoding.UTF8;

            int utf8Length = utf8.GetByteCount(unicodeString);
            byte[] utf8Buffer = new byte[utf8Length];

            char[] charArray = characters.ToArray();
            utf8.GetBytes(charArray, 0, characters.Length, utf8Buffer, 0);
            Span<byte> span = new Span<byte>(utf8Buffer);

            Encoding unicode = Encoding.UTF32;
            int uint32Length = unicode.GetByteCount(unicodeString);
            uint[] uint32Buffer = new uint[uint32Length];
            Span<uint> output = new Span<uint>(uint32Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (!Utf8Encoder.TryDecode(span, output, out int consumed, out int written))
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
                var val = rand.Next(minCodePoint, maxCodePoint);
                while (val >= 0xD800 && val <= 0xDFFF)
                {
                    val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }
    }
}

