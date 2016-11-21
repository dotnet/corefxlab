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
    public partial class PrimitiveParserPerfComparisonTests
    {
        private static string[] textArray = new string[10]
        {
            "42",
            "429496",
            "429496729",
            "42949",
            "4",
            "42949672",
            "4294",
            "429",
            "4294967295",
            "4294967"
        };

        private static int LOAD_ITERATIONS = 30000;

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineSimpleByteStarToUInt32(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        uint value;
                        uint.TryParse(text, out value);
                        sum += value;
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        private static void BaselineSimpleByteStarToUInt32_VariableLength()
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        uint value;
                        uint.TryParse(textArray[i % 10], out value);
                        sum += value;
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt32(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        uint value;
                        uint.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        sum += value;
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        private static void BaselineByteStarToUInt32_VariableLength()
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        uint value;
                        uint.TryParse(textArray[i % 10], NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        sum += value;
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void InternalParserByteStarToUInt32(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                uint sum = 0;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            uint value;
                            InternalParser.TryParseUInt32(utf8ByteStar, 0, length, fd, nf, out value, out bytesConsumed);
                            sum += value;
                        }
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        private unsafe static void InternalParserByteStarToUInt32_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in textArray)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                int bytesConsumed;
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 10];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            InternalParser.TryParseUInt32(utf8ByteStar, 0, utf8ByteArray.Length, fd, nf, out value, out bytesConsumed);
                            sum += value;
                        }
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            uint value;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, length, out value);
                            sum += value;
                        }
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in textArray)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 10];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value);
                            sum += value;
                        }
                    }
                }
                Console.WriteLine(sum);
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("5000000000")] // basic overflow
        [InlineData("12819950000000")] // heavy overflow
        [InlineData("0231281995")] // leading zero
        [InlineData("00000000128")] // many leading zeroes
        [InlineData("-128")] // negative value handling for unsigned types
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32_BytesConsumed(string text)
        {
            int length = text.Length;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                int bytesConsumedSum = 0;
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LOAD_ITERATIONS; i++)
                        {
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, length, out value, out bytesConsumed);
                            sum += value;
                            bytesConsumedSum += bytesConsumed;
                        }
                    }
                }
                Console.WriteLine(sum);
                Console.WriteLine(bytesConsumedSum);
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32_BytesConsumed_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in textArray)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                uint sum = 0;
                int bytesConsumedSum = 0;
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LOAD_ITERATIONS; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 10];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value, out bytesConsumed);
                            sum += value;
                            bytesConsumedSum += bytesConsumed;
                        }
                    }
                }
                Console.WriteLine(sum);
                Console.WriteLine(bytesConsumedSum);
            }
        }
    }
}
