// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Runtime.CompilerServices;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        private const int InnerCount = 100000;

        private static readonly string[] s_Int32TextArray = new string[20]
        {
            "214748364",
            "2",
            "21474836",
            "-21474",
            "21474",
            "-21",
            "-2",
            "214",
            "-21474836",
            "-214748364",
            "2147",
            "-2147",
            "-214748",
            "-2147483",
            "214748",
            "-2147483648",
            "2147483647",
            "21",
            "2147483",
            "-214"
        };

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("107374182")] // standard parse
        [InlineData("2147483647")] // max value
        [InlineData("0")]
        [InlineData("-2147483648")] // min value
        private static void PrimitiveParserByteSpanToInt32(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for(int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseInt32(utf8ByteSpan, out int value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt32_VariableLength()
        {
            int textLength = s_Int32TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_Int32TextArray[i]);
            }

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        PrimitiveParser.InvariantUtf8.TryParseInt32(utf8ByteSpan, out int value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("107374182")] // standard parse
        [InlineData("2147483647")] // max value
        [InlineData("0")]
        [InlineData("-2147483648")] // min value
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseInt32(utf8ByteSpan, out int value, out int bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed_VariableLength()
        {
            int textLength = s_Int32TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_Int32TextArray[i]);
            }

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        PrimitiveParser.InvariantUtf8.TryParseInt32(utf8ByteSpan, out int value, out int bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("๑๐๗๓๗๔๑๘๒")] // standard parse
        [InlineData("๒๑๔๗๔๘๓๖๔๗")] // max value
        [InlineData("๐")]
        [InlineData("๑๐๗")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๘")] // min value
        public unsafe void ParseInt32Thai(string text)
        {
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.TryParseInt32(utf8Span, out int value, out int bytesConsumed, 'G', s_thaiEncoder);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
