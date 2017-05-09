// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public partial class EncodingPerfComparisonTests
    {
        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf8toUtf8UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];
            utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);

            var utf8Bytes = new byte[utf8Length];
            var utf8Chars = new char[characters.Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        utf8.GetChars(utf8Buffer, 0, utf8Buffer.Length, utf8Chars, 0);
                        utf8.GetBytes(utf8Chars, 0, utf8Chars.Length, utf8Bytes, 0);
                    }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf16toUtf8UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf32toUtf8UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
            Encoding utf8 = Encoding.UTF8;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);

            int utf8Length = utf8.GetByteCount(characters);
            var utf8Bytes = new byte[utf8Length];
            var utf16Chars = new char[characters.Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        utf32.GetChars(utf32Buffer, 0, utf32Buffer.Length, utf16Chars, 0);
                        utf8.GetBytes(utf16Chars, 0, utf16Chars.Length, utf8Bytes, 0);
                    }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf8toUtf16UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];
            utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);

            var utf16Chars = new char[characters.Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        utf8.GetChars(utf8Buffer, 0, utf8Buffer.Length, utf16Chars, 0);
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf16toUtf16UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf16 = Encoding.Unicode;
            int utf16Length = utf16.GetByteCount(characters);
            var utf16Buffer = new byte[utf16Length];
            utf16.GetBytes(characters, 0, characters.Length, utf16Buffer, 0);

            var utf16Chars = new char[characters.Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        utf16.GetChars(utf16Buffer, 0, utf16Buffer.Length, utf16Chars, 0);
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf32toUtf16UsingEncoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);

            var utf16Chars = new char[characters.Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        utf32.GetChars(utf32Buffer, 0, utf32Buffer.Length, utf16Chars, 0);
            }
        }
    }
}

