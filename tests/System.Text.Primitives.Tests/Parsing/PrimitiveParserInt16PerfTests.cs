// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests.Parsing
{
    public partial class PrimitiveParserInt16PerfTests
    {
        private const int InnerCount = 10000;

        private static readonly string[] s_Int16TextArray = new string[13]
        {
            "21474",
            "2",
            "-21474",
            "31484",
            "-21",
            "-2",
            "214",
            "2147",
            "-2147",
            "-9345",
            "9345",
            "1000",
            "-214"
        };

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("10732")] // standard parse
        [InlineData("32767")] // max value
        [InlineData("0")]
        [InlineData("-32768")] // min value
        private static void PrimitiveParserByteSpanToInt16(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseInt16(utf8ByteSpan, out short value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        //       //[Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt16_VariableLength()
        {
            int textLength = s_Int16TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_Int16TextArray[i]);
            }

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        PrimitiveParser.InvariantUtf8.TryParseInt16(utf8ByteSpan, out short value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("10737")] // standard parse
        [InlineData("32767")] // max value
        [InlineData("0")]
        [InlineData("-32768")] // min value
        [InlineData("2147")]
        [InlineData("2")]
        [InlineData("-21474")]
        [InlineData("21474")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("214")]
        [InlineData("2147")]
        [InlineData("-2147")]
        [InlineData("-48")]
        [InlineData("48")]
        [InlineData("483")]
        [InlineData("21")]
        [InlineData("-214")]
        [InlineData("+21474")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("+214")]
        [InlineData("+2147")]
        [InlineData("+21475")]
        [InlineData("+48")]
        [InlineData("+483")]
        [InlineData("+21437")]
        [InlineData("000000000000000000001235abcdfg")]
        [InlineData("2147abcdefghijklmnop")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("214abcdefghijklmnop")]
        [InlineData("-2147abcdefghijklmnop")]
        [InlineData("21474abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("487abcdefghijklmnop")]
        [InlineData("-483abcdefghijklmnop")]
        [InlineData("-4836abcdefghijklmnop")]
        [InlineData("21abcdefghijklmnop")]
        [InlineData("+000000000000000000001235abcdfg")]
        [InlineData("+2147abcdefghijklmnop")]
        [InlineData("+214abcdefghijklmnop")]
        [InlineData("+21474abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+487abcdefghijklmnop")]
        [InlineData("+483abcdefghijklmnop")]
        [InlineData("+4836abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        private static void PrimitiveParserByteSpanToInt16_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseInt16(utf8ByteSpan, out short value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt16_BytesConsumed_VariableLength()
        {
            int textLength = s_Int16TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_Int16TextArray[i]);
            }

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        PrimitiveParser.InvariantUtf8.TryParseInt16(utf8ByteSpan, out short value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("10737")] // standard parse
        [InlineData("32767")] // max value
        [InlineData("0")]
        [InlineData("-32768")] // min value
        [InlineData("2147")]
        [InlineData("2")]
        [InlineData("-21474")]
        [InlineData("21474")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("214")]
        [InlineData("2147")]
        [InlineData("-2147")]
        [InlineData("-48")]
        [InlineData("48")]
        [InlineData("483")]
        [InlineData("21")]
        [InlineData("-214")]
        [InlineData("+21474")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("+214")]
        [InlineData("+2147")]
        [InlineData("+21475")]
        [InlineData("+48")]
        [InlineData("+483")]
        [InlineData("+21437")]
        [InlineData("000000000000000000001235")]
        private static void PrimitiveParserByteSpanToInt16_BytesConsumed_Baseline(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        short.TryParse(text, out short value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        private static void PrimitiveParserByteSpanToInt16_BytesConsumed_VariableLength_Baseline()
        {
            int textLength = s_Int16TextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_Int16TextArray[i]);
            }

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        short.TryParse(s_Int16TextArray[i % textLength], out short value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        //     //[Benchmark(InnerIterationCount = InnerCount)]
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
        public unsafe void ParseInt16Thai(string text)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.TryParseInt16(utf8Span, out short value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}


