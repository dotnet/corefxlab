// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;
using Microsoft.Xunit.Performance;
using System.Buffers;
using System.Buffers.Text;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
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
                        Utf8Parser.TryParseInt16(utf8ByteSpan, out short value);
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
                        Utf8Parser.TryParseInt16(utf8ByteSpan, out short value, out int bytesConsumed);
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
                        Utf8Parser.TryParseInt16(utf8ByteSpan, out short value, out int bytesConsumed);
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

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("๑๐๗๓๗")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("๐")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("๒")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("ลบ๒๑")]
        [InlineData("ลบ๒")]
        [InlineData("๒๑๔")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("๒๑๔๗")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("๒๑๔๗๔")]
        [InlineData("๒๑")]
        [InlineData("๒๑๔๗")]
        [InlineData("ลบ๒๑๔")]
        [InlineData("+๒๑๔๗๔")]
        [InlineData("+๒๑")]
        [InlineData("+๒")]
        [InlineData("+๒๑๔๗๔")]
        [InlineData("+๒๘๓๖๔")]
        [InlineData("+๒๑๔๗")]
        [InlineData("+๒๑๔๗")]
        [InlineData("+๒๑๔")]
        [InlineData("+๒๑๔")]
        [InlineData("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๓๕abcdfg")]
        [InlineData("๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("๒abcdefghijklmnop")]
        [InlineData("๒๑๔๗๖abcdefghijklmnop")]
        [InlineData("ลบ๒๑abcdefghijklmnop")]
        [InlineData("๒๑๔๗abcdefghijklmnop")]
        [InlineData("ลบ๒๑abcdefghijklmnop")]
        [InlineData("ลบ๒abcdefghijklmnop")]
        [InlineData("๒๑๔abcdefghijklmnop")]
        [InlineData("ลบ๒๑abcdefghijklmnop")]
        [InlineData("๒๑abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๔abcdefghijklmnop")]
        [InlineData("+๒abcdefghijklmnop")]
        [InlineData("+๒๑๔๗๖abcdefghijklmnop")]
        [InlineData("+ลบ๒๑abcdefghijklmnop")]
        [InlineData("+๒๑๔๗abcdefghijklmnop")]
        [InlineData("+ลบ๒๑abcdefghijklmnop")]
        [InlineData("+ลบ๒abcdefghijklmnop")]
        [InlineData("+๒๑๔abcdefghijklmnop")]
        [InlineData("+ลบ๒๑abcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        [InlineData("+ลบ๑๔abcdefghijklmnop")]
        public void ParseInt16Thai(string text)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        CustomParser.TryParseInt16(utf8Span, out short value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}


