// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public partial class EncodingPerfComparisonTests
    {
        public const ushort Utf16HighSurrogateFirstCodePoint = 0xD800;
        public const ushort Utf16HighSurrogateLastCodePoint = 0xDBFF;
        public const ushort Utf16LowSurrogateFirstCodePoint = 0xDC00;
        public const ushort Utf16LowSurrogateLastCodePoint = 0xDFFF;

        public const uint LastValidCodePoint = 0x10FFFF;

        public const byte Utf8OneByteLastCodePoint = 0x7F;
        public const ushort Utf8TwoBytesLastCodePoint = 0x7FF;
        public const ushort Utf8ThreeBytesLastCodePoint = 0xFFFF;

        public const int DataLength = 999;

        public const int RandomSeed = 42;

        [Benchmark]
        [InlineData(DataLength, 0x0, Utf8OneByteLastCodePoint)]
        [InlineData(DataLength, Utf8OneByteLastCodePoint + 1, Utf8TwoBytesLastCodePoint)]
        [InlineData(DataLength, Utf8TwoBytesLastCodePoint + 1, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, Utf16HighSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint)]
        [InlineData(DataLength, 0x0, Utf8ThreeBytesLastCodePoint)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII)]
        [InlineData(DataLength, 0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII)]
        public void EncodeFromUtf8toUtf8UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8Encoder = Encoding.UTF8;
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
                    if (!utf8.TryEncode(utf8Span, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
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
                    if (!utf8.TryEncode(characters, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
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
        public void EncodeFromUtf32toUtf8UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
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
                    if (!utf8.TryEncode(utf32Span, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
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
        public void EncodeFromUtf8toUtf16UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf8Encoder = Encoding.UTF8;
            int utf8Length = utf8Encoder.GetByteCount(characters);
            var utf8TempBuffer = new byte[utf8Length];
            utf8Encoder.GetBytes(characters, 0, characters.Length, utf8TempBuffer, 0);
            Span<byte> utf8Span = new Span<byte>(utf8TempBuffer);

            TextEncoder utf16 = TextEncoder.Utf16;
            Assert.True(utf16.TryComputeEncodedBytes(utf8Span, out int encodedBytes));
            var utf16Buffer = new byte[encodedBytes];
            var span = new Span<byte>(utf16Buffer);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                    if (!utf16.TryEncode(utf8Span, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
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
                    if (!utf16.TryEncode(characters, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
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
        public void EncodeFromUtf32toUtf16UsingTextEncoder(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            string inputString = GenerateStringData(length, minCodePoint, maxCodePoint, special);
            char[] characters = inputString.AsSpan().ToArray();
            Encoding utf32 = Encoding.UTF32;
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
                    if (!utf16.TryEncode(utf32Span, span, out int consumed, out int written))
                    {
                        throw new Exception(); // this should not happen
                    }
            }
        }

        private static string GenerateStringData(int length, int minCodePoint, int maxCodePoint, SpecialTestCases special = SpecialTestCases.None)
        {
            if (special != SpecialTestCases.None)
            {
                if (special == SpecialTestCases.AlternatingASCIIAndNonASCII) return GenerateStringAlternatingASCIIAndNonASCII(length);
                if (special == SpecialTestCases.MostlyASCIIAndSomeNonASCII) return GenerateStringWithMostlyASCIIAndSomeNonASCII(length);
                return "";
            }
            else
            {
                return GenerateValidString(length, minCodePoint, maxCodePoint);
            }
        }

        private static string GenerateValidString(int length, int minCodePoint, int maxCodePoint, bool ignoreSurrogates = false)
        {
            Random rand = new Random(RandomSeed);
            var plainText = new StringBuilder();
            for (int j = 0; j < length; j++)
            {
                var val = rand.Next(minCodePoint, maxCodePoint);

                if (ignoreSurrogates)
                {
                    while (val >= Utf16HighSurrogateFirstCodePoint && val <= Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters
                    }
                    plainText.Append((char)val);
                    continue;
                }

                if (j < length - 1)
                {
                    while (val >= Utf16LowSurrogateFirstCodePoint && val <= Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(minCodePoint, maxCodePoint); // skip surrogate characters if they can't be paired
                    }

                    if (val >= Utf16HighSurrogateFirstCodePoint && val <= Utf16HighSurrogateLastCodePoint)
                    {
                        plainText.Append((char)val);    // high surrogate
                        j++;
                        val = rand.Next(Utf16LowSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint); // low surrogate
                    }
                }
                else
                {
                    while (val >= Utf16HighSurrogateFirstCodePoint && val <= Utf16LowSurrogateLastCodePoint)
                    {
                        val = rand.Next(0, Utf8ThreeBytesLastCodePoint); // skip surrogate characters if they can't be paired
                    }
                }
                plainText.Append((char)val);
            }
            return plainText.ToString();
        }


        private static string GenerateStringAlternatingASCIIAndNonASCII(int charLength)
        {
            Random rand = new Random(RandomSeed * 2);
            var plainText = new StringBuilder();

            for (int j = 0; j < charLength/3; j++)
            {
                var ascii = rand.Next(0, Utf8OneByteLastCodePoint);
                var highSurrogate = rand.Next(Utf16HighSurrogateFirstCodePoint, Utf16HighSurrogateLastCodePoint);
                var lowSurrogate = rand.Next(Utf16LowSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint);

                plainText.Append((char)ascii);
                plainText.Append((char)highSurrogate);
                plainText.Append((char)lowSurrogate);
            }

            for (int j = 0; j < charLength % 3; j++)
            {
                plainText.Append((char)rand.Next(0, 0x7F));
            }

            return plainText.ToString();
        }

        private static string GenerateStringWithMostlyASCIIAndSomeNonASCII(int charLength)
        {
            Random rand = new Random(RandomSeed * 3);
            var plainText = new StringBuilder();

            int j = 0;
            while (j < charLength - 70)
            {
                for (int i = 0; i < rand.Next(20, 50); i++)
                {
                    var ascii = rand.Next(0, Utf8OneByteLastCodePoint);
                    plainText.Append((char)ascii);
                    j++;
                }
                for (int i = 0; i < rand.Next(5, 10); i++)
                {
                    var highSurrogate = rand.Next(Utf16HighSurrogateFirstCodePoint, Utf16HighSurrogateLastCodePoint);
                    var lowSurrogate = rand.Next(Utf16LowSurrogateFirstCodePoint, Utf16LowSurrogateLastCodePoint);
                    j += 2;
                }
            }

            while (j < charLength)
            {
                plainText.Append((char)rand.Next(0, 0x7F));
                j++;
            }

            return plainText.ToString();
        }

        public enum SpecialTestCases
        {
            None, AlternatingASCIIAndNonASCII, MostlyASCIIAndSomeNonASCII
        }
    }
}

