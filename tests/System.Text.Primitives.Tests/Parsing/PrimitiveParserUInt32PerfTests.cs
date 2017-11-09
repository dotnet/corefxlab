// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Buffers.Text;

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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(text, out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(s_UInt32TextArray[i % 10], out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(s_UInt32TextArray[i % 10], NumberStyles.None, CultureInfo.InvariantCulture, out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        uint.TryParse(s_UInt32TextArrayHex[i % 8], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value);
                        TestHelper.DoNotIgnore(value, 0);
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
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteSpanToUInt32_BytesConsumed_VariableLength()
        {
            int textLength = s_UInt32TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_UInt32TextArray[i]);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
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
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed, 'X');
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        private unsafe static void PrimitiveParserByteSpanToUInt32Hex_BytesConsumed_VariableLength()
        {
            int textLength = s_UInt32TextArrayHex.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_UInt32TextArrayHex[i]);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        Utf8Parser.TryParse(utf8ByteSpan, out uint value, out int bytesConsumed, 'X');
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
