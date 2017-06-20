// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Runtime.CompilerServices;

namespace System.Text.Primitives.Tests
{
    public class ParserPerfTests
    {
        private const int InnerCount = 10000;

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

        //[Benchmark(InnerIterationCount = InnerCount)]
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

        //[Benchmark(InnerIterationCount = InnerCount)]
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(int value, int consumed)
        {
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("107374182")] // standard parse
        [InlineData("2147483647")] // max value
        [InlineData("0")]
        [InlineData("-2147483648")] // min value
        [InlineData("214748364")]
        [InlineData("2")]
        [InlineData("21474836")]
        [InlineData("-21474")]
        [InlineData("21474")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("214")]
        [InlineData("-21474836")]
        [InlineData("-214748364")]
        [InlineData("2147")]
        [InlineData("-2147")]
        [InlineData("-214748")]
        [InlineData("-2147483")]
        [InlineData("214748")]
        [InlineData("21")]
        [InlineData("2147483")]
        [InlineData("-214")]
        [InlineData("+21474")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("+21474836")]
        [InlineData("+214748364")]
        [InlineData("+2147")]
        [InlineData("+214748")]
        [InlineData("+2147483")]
        [InlineData("+2147483648")]
        [InlineData("+214")]
        [InlineData("000000000000000000001235abcdfg")]
        [InlineData("214748364abcdefghijklmnop")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("21474836abcdefghijklmnop")]
        [InlineData("-21474abcdefghijklmnop")]
        [InlineData("21474abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("214abcdefghijklmnop")]
        [InlineData("-21474836abcdefghijklmnop")]
        [InlineData("-214748364abcdefghijklmnop")]
        [InlineData("2147abcdefghijklmnop")]
        [InlineData("-2147abcdefghijklmnop")]
        [InlineData("-214748abcdefghijklmnop")]
        [InlineData("-2147483abcdefghijklmnop")]
        [InlineData("214748abcdefghijklmnop")]
        [InlineData("21abcdefghijklmnop")]
        [InlineData("2147483abcdefghijklmnop")]
        [InlineData("-214abcdefghijklmnop")]
        [InlineData("+21474abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+21474836abcdefghijklmnop")]
        [InlineData("+214748364abcdefghijklmnop")]
        [InlineData("+2147abcdefghijklmnop")]
        [InlineData("+214748abcdefghijklmnop")]
        [InlineData("+2147483abcdefghijklmnop")]
        [InlineData("+2147483648abcdefghijklmnop")]
        [InlineData("+214abcdefghijklmnop")]
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

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        private static void ParseTestNew(int count)
        {
            string text = GenerateRandomDigitString(count);
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            int final = 0;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int totalConsumed = 0;
                        while (totalConsumed < utf8ByteSpan.Length)
                        {
                            PrimitiveParser.InvariantUtf8.TryParseInt32(utf8ByteSpan.Slice(totalConsumed), out int value, out int bytesConsumed);
                            totalConsumed += bytesConsumed;
                            final |= value;
                        }
                    }
                }
            }
            Assert.Equal(-1, final);
        }

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        private static void ParseTestNew_OLD(int count)
        {
            string text = GenerateRandomDigitString(count);
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            int final = 0;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int totalConsumed = 0;
                        while (totalConsumed < utf8ByteSpan.Length)
                        {
                            PrimitiveParser.InvariantUtf8.TryParseInt32_OLD(utf8ByteSpan.Slice(totalConsumed), out int value, out int bytesConsumed);
                            totalConsumed += bytesConsumed;
                            final |= value;
                        }
                    }
                }
            }
            Assert.Equal(-1, final);
        }

        private static string GenerateRandomDigitString(int count = 1000)
        {
            Random rnd = new Random(count);
            var builder = new StringBuilder();

            for (int j = 0; j < count; j++)
            {
                int sign = rnd.Next(0, 3);
                if (sign == 1) builder.Append("+");
                if (sign == 2) builder.Append("-");
                var length = rnd.Next(1, 14);
                for (int i = 0; i < length; i++)
                {
                    int digit = rnd.Next(0, 10);
                    builder.Append(digit.ToString());
                }
            }
            return builder.ToString();
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
        [InlineData("107374182")] // standard parse
        [InlineData("2147483647")] // max value
        [InlineData("0")]
        [InlineData("-2147483648")] // min value
        [InlineData("214748364")]
        [InlineData("2")]
        [InlineData("21474836")]
        [InlineData("-21474")]
        [InlineData("21474")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("214")]
        [InlineData("-21474836")]
        [InlineData("-214748364")]
        [InlineData("2147")]
        [InlineData("-2147")]
        [InlineData("-214748")]
        [InlineData("-2147483")]
        [InlineData("214748")]
        [InlineData("21")]
        [InlineData("2147483")]
        [InlineData("-214")]
        [InlineData("+21474")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("+21474836")]
        [InlineData("+214748364")]
        [InlineData("+2147")]
        [InlineData("+214748")]
        [InlineData("+2147483")]
        [InlineData("+2147483648")]
        [InlineData("+214")]
        [InlineData("000000000000000000001235abcdfg")]
        [InlineData("214748364abcdefghijklmnop")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("21474836abcdefghijklmnop")]
        [InlineData("-21474abcdefghijklmnop")]
        [InlineData("21474abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("214abcdefghijklmnop")]
        [InlineData("-21474836abcdefghijklmnop")]
        [InlineData("-214748364abcdefghijklmnop")]
        [InlineData("2147abcdefghijklmnop")]
        [InlineData("-2147abcdefghijklmnop")]
        [InlineData("-214748abcdefghijklmnop")]
        [InlineData("-2147483abcdefghijklmnop")]
        [InlineData("214748abcdefghijklmnop")]
        [InlineData("21abcdefghijklmnop")]
        [InlineData("2147483abcdefghijklmnop")]
        [InlineData("-214abcdefghijklmnop")]
        [InlineData("+21474abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+21474836abcdefghijklmnop")]
        [InlineData("+214748364abcdefghijklmnop")]
        [InlineData("+2147abcdefghijklmnop")]
        [InlineData("+214748abcdefghijklmnop")]
        [InlineData("+2147483abcdefghijklmnop")]
        [InlineData("+2147483648abcdefghijklmnop")]
        [InlineData("+214abcdefghijklmnop")]
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed_BASE(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        int.TryParse(text, out int value);
                        DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed_VariableLength_BASE()
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
                        int.TryParse(s_Int32TextArray[i % textLength], out int value);
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
        [InlineData("214748364")]
        [InlineData("2")]
        [InlineData("21474836")]
        [InlineData("-21474")]
        [InlineData("21474")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("214")]
        [InlineData("-21474836")]
        [InlineData("-214748364")]
        [InlineData("2147")]
        [InlineData("-2147")]
        [InlineData("-214748")]
        [InlineData("-2147483")]
        [InlineData("214748")]
        [InlineData("21")]
        [InlineData("2147483")]
        [InlineData("-214")]
        [InlineData("+21474")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("+21474836")]
        [InlineData("+214748364")]
        [InlineData("+2147")]
        [InlineData("+214748")]
        [InlineData("+2147483")]
        [InlineData("+2147483648")]
        [InlineData("+214")]
        [InlineData("000000000000000000001235abcdfg")]
        [InlineData("214748364abcdefghijklmnop")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("21474836abcdefghijklmnop")]
        [InlineData("-21474abcdefghijklmnop")]
        [InlineData("21474abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("214abcdefghijklmnop")]
        [InlineData("-21474836abcdefghijklmnop")]
        [InlineData("-214748364abcdefghijklmnop")]
        [InlineData("2147abcdefghijklmnop")]
        [InlineData("-2147abcdefghijklmnop")]
        [InlineData("-214748abcdefghijklmnop")]
        [InlineData("-2147483abcdefghijklmnop")]
        [InlineData("214748abcdefghijklmnop")]
        [InlineData("21abcdefghijklmnop")]
        [InlineData("2147483abcdefghijklmnop")]
        [InlineData("-214abcdefghijklmnop")]
        [InlineData("+21474abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+21474836abcdefghijklmnop")]
        [InlineData("+214748364abcdefghijklmnop")]
        [InlineData("+2147abcdefghijklmnop")]
        [InlineData("+214748abcdefghijklmnop")]
        [InlineData("+2147483abcdefghijklmnop")]
        [InlineData("+2147483648abcdefghijklmnop")]
        [InlineData("+214abcdefghijklmnop")]
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed_OLD(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseInt32_OLD(utf8ByteSpan, out int value, out int bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt32_BytesConsumed_VariableLength_OLD()
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
                        PrimitiveParser.InvariantUtf8.TryParseInt32_OLD(utf8ByteSpan, out int value, out int bytesConsumed);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        /*[Benchmark(InnerIterationCount = InnerCount)]
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
        }*/
    }
}
