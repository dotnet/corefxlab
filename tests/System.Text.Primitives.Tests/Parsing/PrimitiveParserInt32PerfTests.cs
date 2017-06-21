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
        [InlineData("+2147483647")]
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
        [InlineData("+2147483647abcdefghijklmnop")]
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
        [InlineData("+2147483647")]
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
        [InlineData("+2147483647abcdefghijklmnop")]
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
        [InlineData("-2147483647")] // min value
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
        [InlineData("+2147483647")]
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
        [InlineData("+2147483647abcdefghijklmnop")]
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

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("๑๐๗๓๗๔๑๘๒")]
        [InlineData("๒๑๔๗๔๘๓๖๔๗")]
        [InlineData("๐")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๘")]
        [InlineData("๒๑๔๗๔๘๓๖๔")]
        [InlineData("๒")]
        [InlineData("๒๑๔๗๔๘๓๖")]
        [InlineData("ลบ๒๑๔๗๔")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("ลบ๒๑")]
        [InlineData("ลบ๒")]
        [InlineData("๒๑๔")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔")]
        [InlineData("๒๑๔๗")]
        [InlineData("ลบ๒๑๔๗")]
        [InlineData("ลบ๒๑๔๗๔๘")]
        [InlineData("ลบ๒๑๔๗๔๘๓")]
        [InlineData("๒๑๔๗๔๘")]
        [InlineData("๒๑")]
        [InlineData("๒๑๔๗๔๘๓")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("+๒๑๔๗๔")]
        [InlineData("+๒๑")]
        [InlineData("+๒")]
        [InlineData("+๒๑๔๗๔๘๓๖")]
        [InlineData("+๒๑๔๗๔๘๓๖๔")]
        [InlineData("+๒๑๔๗")]
        [InlineData("+๒๑๔๗๔๘")]
        [InlineData("+๒๑๔๗๔๘๓")]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗")]
        [InlineData("+๒๑๔")]
        [InlineData("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg")]
        [InlineData("๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("๒abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("ลบ๒๑abcdefghijklmnop")]
        [InlineData("ลบ๒abcdefghijklmnop")]
        [InlineData("๒๑๔abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("๒๑๔๗abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("๒๑abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        [InlineData("+๒abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("+๒๑๔๗abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗abcdefghijklmnop")]
        [InlineData("+๒๑๔abcdefghijklmnop")]
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

        //[Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("๑๐๗๓๗๔๑๘๒")]
        [InlineData("๒๑๔๗๔๘๓๖๔๗")]
        [InlineData("๐")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔๘")]
        [InlineData("๒๑๔๗๔๘๓๖๔")]
        [InlineData("๒")]
        [InlineData("๒๑๔๗๔๘๓๖")]
        [InlineData("ลบ๒๑๔๗๔")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("ลบ๒๑")]
        [InlineData("ลบ๒")]
        [InlineData("๒๑๔")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔")]
        [InlineData("๒๑๔๗")]
        [InlineData("ลบ๒๑๔๗")]
        [InlineData("ลบ๒๑๔๗๔๘")]
        [InlineData("ลบ๒๑๔๗๔๘๓")]
        [InlineData("๒๑๔๗๔๘")]
        [InlineData("๒๑")]
        [InlineData("๒๑๔๗๔๘๓")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("+๒๑๔๗๔")]
        [InlineData("+๒๑")]
        [InlineData("+๒")]
        [InlineData("+๒๑๔๗๔๘๓๖")]
        [InlineData("+๒๑๔๗๔๘๓๖๔")]
        [InlineData("+๒๑๔๗")]
        [InlineData("+๒๑๔๗๔๘")]
        [InlineData("+๒๑๔๗๔๘๓")]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗")]
        [InlineData("+๒๑๔")]
        [InlineData("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg")]
        [InlineData("๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("๒abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("ลบ๒๑abcdefghijklmnop")]
        [InlineData("ลบ๒abcdefghijklmnop")]
        [InlineData("๒๑๔abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("๒๑๔๗abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("๒๑abcdefghijklmnop")]
        [InlineData("๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("ลบ๒๑๔abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        [InlineData("+๒abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖๔abcdefghijklmnop")]
        [InlineData("+๒๑๔๗abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔๘๓๖๔๗abcdefghijklmnop")]
        [InlineData("+๒๑๔abcdefghijklmnop")]
        public unsafe void ParseInt32Thai_OLD(string text)
        {
            ReadOnlySpan<byte> utf8Span = UtfEncode(text, false);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.TryParseInt32_OLD(utf8Span, out int value, out int bytesConsumed, 'G', s_thaiEncoder);
                        DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        static byte[][] s_thaiUtf8DigitsAndSymbols = new byte[][]
{
            new byte[] { 0xe0, 0xb9, 0x90 }, new byte[] { 0xe0, 0xb9, 0x91 }, new byte[] { 0xe0, 0xb9, 0x92 },
            new byte[] { 0xe0, 0xb9, 0x93 }, new byte[] { 0xe0, 0xb9, 0x94 }, new byte[] { 0xe0, 0xb9, 0x95 }, new byte[] { 0xe0, 0xb9, 0x96 },
            new byte[] { 0xe0, 0xb9, 0x97 }, new byte[] { 0xe0, 0xb9, 0x98 }, new byte[] { 0xe0, 0xb9, 0x99 }, new byte[] { 0xE0, 0xB8, 0x88, 0xE0, 0xB8, 0x94 }, null,
            new byte[] { 0xE0, 0xB8, 0xAA, 0xE0, 0xB8, 0xB4, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x87, 0xE0, 0xB8, 0x97, 0xE0, 0xB8, 0xB5, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x83,
                0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0x8D, 0xE0, 0xB9, 0x88, 0xE0, 0xB9, 0x82, 0xE0, 0xB8, 0x95, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xAB, 0xE0, 0xB8, 0xA5, 0xE0,
                0xB8, 0xB7, 0xE0, 0xB8, 0xAD, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0x81, 0xE0, 0xB8, 0xB4, 0xE0, 0xB8, 0x99 },
            new byte[] { 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x9A }, new byte[] { 43 }, new byte[] { 0xE0, 0xB9, 0x84, 0xE0, 0xB8, 0xA1, 0xE0, 0xB9, 0x88, 0xE0, 0xB9,
                0x83, 0xE0, 0xB8, 0x8A, 0xE0, 0xB9, 0x88, 0xE0, 0xB8, 0x95, 0xE0, 0xB8, 0xB1, 0xE0, 0xB8, 0xA7, 0xE0, 0xB9, 0x80, 0xE0, 0xB8, 0xA5, 0xE0, 0xB8, 0x82 },
            new byte[] { 69 }, new byte[] { 101 },
};

        static TextEncoder s_thaiEncoder = TextEncoder.CreateUtf8Encoder(s_thaiUtf8DigitsAndSymbols);

        private byte[] UtfEncode(string s, bool utf16)
        {
            if (utf16)
                return Text.Encoding.Unicode.GetBytes(s);
            else
                return Text.Encoding.UTF8.GetBytes(s);
        }
    }
}
