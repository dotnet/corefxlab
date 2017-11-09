// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Microsoft.Xunit.Performance;
using System.Buffers.Text;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        private const int LargerInnerCount = InnerCount * 10;

        private static readonly string[] s_SByteTextArray = new string[17]
        {
           "95",
            "2",
            "112",
            "-112",
            "-21",
            "-2",
            "114",
            "-114",
            "-124",
            "117",
            "-117",
            "-14",
            "14",
            "74",
            "21",
            "83",
            "-127"
        };

        [Benchmark(InnerIterationCount = LargerInnerCount)]
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
                        Utf8Parser.TryParse(utf8ByteSpan, out sbyte value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = LargerInnerCount)]
        private static void PrimitiveParserByteSpanToSByte_BytesConsumed_VariableLength()
        { 
            int textLength = s_SByteTextArray.Length; 
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength); 
            for (var i = 0; i<textLength; i++) 
            { 
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_SByteTextArray[i]); 
            } 

            foreach (var iteration in Benchmark.Iterations) 
            { 
                using (iteration.StartMeasurement()) 
                { 
                    for (int i = 0; i<Benchmark.InnerIterationCount; i++) 
                    {
                        ReadOnlySpan<byte> utf8ByteSpan = utf8ByteArray[i % textLength];
                        Utf8Parser.TryParse(utf8ByteSpan, out sbyte value, out int bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    } 
                } 
            } 
        } 

        [Benchmark(InnerIterationCount = LargerInnerCount)]
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
        [InlineData("00000000000000000000123")]
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

        [Benchmark(InnerIterationCount = LargerInnerCount)]

        private static void PrimitiveParserByteSpanToSByte_BytesConsumed_VariableLength_Baseline()
        {
            int textLength = s_SByteTextArray.Length;
            byte[][] utf8ByteArray = (byte[][])Array.CreateInstance(typeof(byte[]), textLength);
            for (var i = 0; i < textLength; i++)
            {
                utf8ByteArray[i] = Text.Encoding.UTF8.GetBytes(s_SByteTextArray[i]);
            }
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        sbyte.TryParse(s_SByteTextArray[i % textLength], out sbyte value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = LargerInnerCount)]
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
                        CustomParser.TryParseSByte(utf8Span, out sbyte value, out int bytesConsumed, 'G', TestHelper.ThaiTable);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
