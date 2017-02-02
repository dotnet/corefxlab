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
        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private static void BaselineStringToBool(string text)
        {
            bool value;
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        bool.TryParse(text, out value);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private static void PrimitiveParserByteSpanToBool(string text)
        {
            bool value;
            int bytesConsumed;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        PrimitiveParser.InvariantUtf8.TryParseBoolean(utf8ByteSpan, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private unsafe static void PrimitiveParserByteStarToBool(string text)
        {
            bool value;
            int bytesConsumed;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            PrimitiveParser.InvariantUtf8.TryParseBoolean(utf8ByteStar, utf8ByteArray.Length, out value, out bytesConsumed);
                        }
                    }
                }
            }
        }
    }
}
