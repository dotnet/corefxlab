// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserSBytePerfTests
    {
        private const int InnerCount = 100000;

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("107")] // standard parse
        [InlineData("127")] // max value
        [InlineData("0")]
        [InlineData("-128")] // min value
        [InlineData("147")]
        [InlineData("2")]
        [InlineData("105")]
        [InlineData("-111")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("-13")]
        [InlineData("-8")]
        [InlineData("-83")]
        [InlineData("+127")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("00000000000000000000123abcdfg")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("14abcdefghijklmnop")]
        [InlineData("-14abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("+14abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+111abcdefghijklmnop")]
        [InlineData("+000000000000000000123abcdfg")]
        private static void PrimitiveParserByteSpanToSByte_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseSByte(utf8ByteSpan, out sbyte value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("107")] // standard parse
        [InlineData("127")] // max value
        [InlineData("0")]
        [InlineData("-128")] // min value
        [InlineData("147")]
        [InlineData("2")]
        [InlineData("105")]
        [InlineData("-111")]
        [InlineData("-21")]
        [InlineData("-2")]
        [InlineData("-13")]
        [InlineData("-8")]
        [InlineData("-83")]
        [InlineData("+127")]
        [InlineData("+21")]
        [InlineData("+2")]
        [InlineData("00000000000000000000123abcdfg")]
        [InlineData("2abcdefghijklmnop")]
        [InlineData("14abcdefghijklmnop")]
        [InlineData("-14abcdefghijklmnop")]
        [InlineData("-21abcdefghijklmnop")]
        [InlineData("-2abcdefghijklmnop")]
        [InlineData("+14abcdefghijklmnop")]
        [InlineData("+21abcdefghijklmnop")]
        [InlineData("+2abcdefghijklmnop")]
        [InlineData("+111abcdefghijklmnop")]
        [InlineData("+000000000000000000123abcdfg")]
        private static void PrimitiveParserByteSpanToSByte_BytesConsumed_Baseline(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            var utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        sbyte.TryParse(text, out sbyte value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = InnerCount)]
        [InlineData("๑๑๑")]
        [InlineData("๑๒๔")]
        [InlineData("௧௨")]
        [InlineData("๔๘")]
        [InlineData("๒")]
        [InlineData("+๑๑๑")]
        [InlineData("+๑๒๔")]
        [InlineData("+๑๒")]
        [InlineData("+๔๘")]
        [InlineData("+๒")]
        [InlineData("๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๔abcdfg")]
        [InlineData("๑๒๔abcdefghijklmnop")]
        [InlineData("๒abcdefghijklmnop")]
        [InlineData("๒๑bcdefghijklmnop")]
        [InlineData("ลบabcdefghijklmnop")]
        [InlineData("๑๑๑abcdefghijklmnop")]
        [InlineData("ลบabcdefghijklmnop")]
        [InlineData("๒๑abcdefghijklmnop")]
        [InlineData("๒๑abcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        [InlineData("+๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๐๑๒๔abcdfg")]
        [InlineData("+๑๒๔abcdefghijklmnop")]
        [InlineData("+๒abcdefghijklmnop")]
        [InlineData("+๒๑bcdefghijklmnop")]
        [InlineData("+ลบabcdefghijklmnop")]
        [InlineData("+๑๑๑abcdefghijklmnop")]
        [InlineData("+ลบabcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        [InlineData("+๒๑abcdefghijklmnop")]
        public unsafe void ParseSByteThai(string text)
        {
            ReadOnlySpan<byte> utf8Span = TestHelper.UtfEncode(text, false);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        PrimitiveParser.TryParseSByte(utf8Span, out sbyte value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
