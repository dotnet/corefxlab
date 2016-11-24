using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Internal;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineSimpleByteStarToUInt64(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt64(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt64Hex(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void InternalParserByteStarToUInt64(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            InternalParser.TryParseUInt64(utf8ByteStar, 0, length, fd, nf, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void InternalParserByteSpanToUInt64(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            InternalParser.TryParseUInt64(utf8ByteSpan, fd, nf, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt64(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8ByteStar, length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt64_BytesConsumed(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8ByteStar, length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.TryParseUInt64(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt64Hex(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8ByteStar, length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt64Hex_BytesConsumed(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            ulong value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8ByteStar, length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64Hex(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64Hex_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        ulong value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt64(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
