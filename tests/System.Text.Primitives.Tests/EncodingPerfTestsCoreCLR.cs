// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public partial class EncodingPerfComparisonTests
    {
        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf8toUtf8Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];
            utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);

            var temp = utf8.GetChars(utf8Buffer);
            byte[] output = Encoding.Convert(utf8, utf8, utf8Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf8.GetBytes(utf8.GetChars(utf8Buffer), 0, temp.Length, output, 0);
            }
        }

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf16toUtf8Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);
            }
        }

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf32toUtf8Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
            Encoding utf8 = Encoding.UTF8;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);

            int utf8Length = utf8.GetByteCount(characters);

            var temp = utf32.GetChars(utf32Buffer);
            byte[] output = Encoding.Convert(utf32, utf8, utf32Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf8.GetBytes(utf32.GetChars(utf32Buffer), 0, temp.Length, output, 0);
            }
        }

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf8toUtf16Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8 = Encoding.UTF8;
            int utf8Length = utf8.GetByteCount(characters);
            var utf8Buffer = new byte[utf8Length];
            utf8.GetBytes(characters, 0, characters.Length, utf8Buffer, 0);

            Encoding utf16 = Encoding.Unicode;
            int utf16Length = utf16.GetCharCount(utf8Buffer);
            var utf16Buffer = new char[utf16Length];

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf16.GetChars(utf8Buffer, 0, utf8Buffer.Length, utf16Buffer, 0);
            }
        }

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf16toUtf16Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf16 = Encoding.Unicode;
            int utf16Length = utf16.GetByteCount(characters);
            var utf16Buffer = new byte[utf16Length];
            utf16.GetBytes(characters, 0, characters.Length, utf16Buffer, 0);

            var temp = utf16.GetChars(utf16Buffer);
            byte[] output = Encoding.Convert(utf16, utf16, utf16Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf16.GetBytes(utf16.GetChars(utf16Buffer), 0, temp.Length, output, 0);
            }
        }

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf32toUtf16Encoding(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
            Encoding utf16 = Encoding.Unicode;
            int utf32Length = utf32.GetByteCount(characters);
            var utf32Buffer = new byte[utf32Length];
            utf32.GetBytes(characters, 0, characters.Length, utf32Buffer, 0);

            var temp = utf32.GetChars(utf32Buffer);
            byte[] output = Encoding.Convert(utf32, utf16, utf32Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    utf16.GetBytes(utf32.GetChars(utf32Buffer), 0, temp.Length, output, 0);
            }
        }
    }
}

