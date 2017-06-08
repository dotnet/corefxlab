// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Primitives.Tests.Encoding;
using System.Buffers;

namespace System.Text.Primitives.Tests
{
    public partial class EncodingPerfComparisonTests
    {
        private const int InnerCount = 1000;

        private static IEnumerable<object[]> GetEncodingPerformanceTestData()
        {
            var data = new List<object[]>();
            data.Add(new object[] { 99, 0x0, TextEncoderConstants.Utf8OneByteLastCodePoint });
            data.Add(new object[] { 99, TextEncoderConstants.Utf8OneByteLastCodePoint + 1, TextEncoderConstants.Utf8TwoBytesLastCodePoint });
            data.Add(new object[] { 99, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 99, TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint });
            data.Add(new object[] { 99, 0x0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 99, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII });
            data.Add(new object[] { 99, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII });
            data.Add(new object[] { 999, 0x0, TextEncoderConstants.Utf8OneByteLastCodePoint });
            data.Add(new object[] { 999, TextEncoderConstants.Utf8OneByteLastCodePoint + 1, TextEncoderConstants.Utf8TwoBytesLastCodePoint });
            data.Add(new object[] { 999, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 999, TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint });
            data.Add(new object[] { 999, 0x0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 999, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII });
            data.Add(new object[] { 999, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII });
            data.Add(new object[] { 9999, 0x0, TextEncoderConstants.Utf8OneByteLastCodePoint });
            data.Add(new object[] { 9999, TextEncoderConstants.Utf8OneByteLastCodePoint + 1, TextEncoderConstants.Utf8TwoBytesLastCodePoint });
            data.Add(new object[] { 9999, TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 9999, TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint });
            data.Add(new object[] { 9999, 0x0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint });
            data.Add(new object[] { 9999, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII });
            data.Add(new object[] { 9999, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII });
            return data;
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf8toUtf8UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Text.Encoding utf8Encoder = Text.Encoding.UTF8;
            int utf8Length = utf8Encoder.GetByteCount(characters);
            var utf8TempBuffer = new byte[utf8Length];
            utf8Encoder.GetBytes(characters, 0, characters.Length, utf8TempBuffer, 0);
            Span<byte> utf8Span = new Span<byte>(utf8TempBuffer);
            
            TextEncoder utf8 = TextEncoder.Utf8;
            var utf8Buffer = new byte[utf8Length];
            var span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        if (!utf8.TryEncode(utf8Span, span, out int consumed, out int written)) { throw new Exception(); }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf16toUtf8UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            ReadOnlySpan<char> characters = inputString.AsSpan();
            TextEncoder utf8 = TextEncoder.Utf8;
            Assert.True(utf8.TryComputeEncodedBytes(characters, out int encodedBytes));
            var utf8Buffer = new byte[encodedBytes];
            var span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        if (!utf8.TryEncode(characters, span, out int consumed, out int written)) { throw new Exception(); }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf32toUtf8UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Text.Encoding utf32 = Text.Encoding.UTF32;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);
            Span<byte> utf32ByteSpan = new Span<byte>(utf32Buffer);
            ReadOnlySpan<uint> utf32Span = utf32ByteSpan.NonPortableCast<byte, uint>();

            TextEncoder utf8 = TextEncoder.Utf8;
            Assert.True(utf8.TryComputeEncodedBytes(utf32Span, out int encodedBytes));
            var utf8Buffer = new byte[encodedBytes];
            var span = new Span<byte>(utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        if (!utf8.TryEncode(utf32Span, span, out int consumed, out int written)) { throw new Exception(); }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf8toUtf16UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            byte[] utf8Bytes = Text.Encoding.UTF8.GetBytes(inputString);

            Assert.True(TextEncoder.Utf16.TryComputeEncodedBytes(utf8Bytes, out int encodedBytes));
            Span<byte> utf16Bytes = new byte[encodedBytes];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        var result = Encoders.Utf16.ConvertFromUtf8(utf8Bytes, utf16Bytes, out int consumed, out int written);
                        if (result != TransformationStatus.Done)
                            throw new Exception();
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf16toUtf16UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            ReadOnlySpan<char> characters = inputString.AsSpan();
            TextEncoder utf16 = TextEncoder.Utf16;
            Assert.True(utf16.TryComputeEncodedBytes(characters, out int encodedBytes));
            var utf16Buffer = new byte[encodedBytes];
            var span = new Span<byte>(utf16Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        if (!utf16.TryEncode(characters, span, out int consumed, out int written)) { throw new Exception(); }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [MemberData(nameof(GetEncodingPerformanceTestData))]
        public void EncodeFromUtf32toUtf16UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Text.Encoding utf32 = Text.Encoding.UTF32;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);
            Span<byte> utf32ByteSpan = new Span<byte>(utf32Buffer);
            ReadOnlySpan<uint> utf32Span = utf32ByteSpan.NonPortableCast<byte, uint>();

            TextEncoder utf16 = TextEncoder.Utf16;
            Assert.True(utf16.TryComputeEncodedBytes(utf32Span, out int encodedBytes));
            var utf16Buffer = new byte[encodedBytes];
            var span = new Span<byte>(utf16Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        if (!utf16.TryEncode(utf32Span, span, out int consumed, out int written)) { throw new Exception(); }
            }
        }

        private static string GenerateStringData(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            if (special != SpecialTestCases.None)
            {
                if (special == SpecialTestCases.AlternatingASCIIAndNonASCII) return TextEncoderTestHelper.GenerateStringAlternatingASCIIAndNonASCII(length);
                if (special == SpecialTestCases.MostlyASCIIAndSomeNonASCII) return TextEncoderTestHelper.GenerateStringWithMostlyASCIIAndSomeNonASCII(length);
                return "";
            }
            else
            {
                return TextEncoderTestHelper.GenerateValidString(length, minCodePoint, maxCodePoint);
            }
        }

        public enum SpecialTestCases
        {
            None, AlternatingASCIIAndNonASCII, MostlyASCIIAndSomeNonASCII
        }
    }
}

