// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        private static readonly string[] s_UInt32TextArray = new string[10]
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

        private static readonly string[] s_UInt32TextArrayHex = new string[8]
        {
            "A2",
            "A29496",
            "A2949",
            "A",
            "A2949672",
            "A294",
            "A29",
            "A294967"
        };
        
        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineSimpleByteStarToUInt32(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(text, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        private static void BaselineSimpleByteStarToUInt32_VariableLength()
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(s_UInt32TextArray[i % 10], out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt32(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        private static void BaselineByteStarToUInt32_VariableLength()
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(s_UInt32TextArray[i % 10], NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffff")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt32Hex(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        private static void BaselineByteStarToUInt32Hex_VariableLength()
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        uint.TryParse(s_UInt32TextArrayHex[i % 8], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32(string text)
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
                            uint value;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in s_UInt32TextArray)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 10];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32_BytesConsumed(string text)
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
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32_BytesConsumed_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in s_UInt32TextArray)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 10];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt32(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        private unsafe static void PrimitiveParserByteSpanToUInt32_VariableLength(int index)
        {
            string text = s_UInt32TextArray[index];
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("4294967295")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt32_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        private unsafe static void PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength(int index)
        {
            string text = s_UInt32TextArray[index];
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.TryParseUInt32(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32Hex(string text)
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
                            uint value;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteStar, length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32Hex_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in s_UInt32TextArrayHex)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 8];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value);
                            DoNotIgnore(value, 0);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteStarToUInt32Hex_BytesConsumed(string text)
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
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteStar, length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteStarToUInt32Hex_BytesConsumed_VariableLength()
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string text in s_UInt32TextArrayHex)
            {
                byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
                byteArrayList.Add(utf8ByteArray);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        byte[] utf8ByteArray = byteArrayList[i % 8];
                        fixed (byte* utf8ByteStar = utf8ByteArray)
                        {
                            uint value;
                            int bytesConsumed;
                            PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteStar, utf8ByteArray.Length, out value, out bytesConsumed);
                            DoNotIgnore(value, bytesConsumed);
                        }
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt32Hex(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        private unsafe static void PrimitiveParserByteSpanToUInt32Hex_VariableLength(int index)
        {
            string text = s_UInt32TextArrayHex[index];
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteSpan, out value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt32Hex_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        private unsafe static void PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength(int index)
        {
            string text = s_UInt32TextArrayHex[index];
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        uint value;
                        int bytesConsumed;
                        PrimitiveParser.InvariantUtf8.Hex.TryParseUInt32(utf8ByteSpan, out value, out bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
