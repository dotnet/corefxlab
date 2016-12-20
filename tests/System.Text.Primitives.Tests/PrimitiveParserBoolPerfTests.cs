using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Text.Internal;

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
        private static void InternalParserByteSpanToBool(string text)
        {
            bool value;
            int bytesConsumed;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < LoadIterations; i++)
                    {
                        InternalParser.TryParseBoolean(utf8ByteSpan, fd, nf, out value, out bytesConsumed);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("True")]
        [InlineData("False")]
        private unsafe static void InternalParserByteStarToBool(string text)
        {
            bool value;
            int bytesConsumed;
            byte[] utf8ByteArray = Encoding.UTF8.GetBytes(text);
            EncodingData fd = EncodingData.InvariantUtf8;
            TextFormat nf = new TextFormat('N');
            foreach (var iteration in Benchmark.Iterations)
            {
                fixed (byte* utf8ByteStar = utf8ByteArray)
                {
                    using (iteration.StartMeasurement())
                    {
                        for (int i = 0; i < LoadIterations; i++)
                        {
                            InternalParser.TryParseBoolean(utf8ByteStar, 0, utf8ByteArray.Length, fd, nf, out value, out bytesConsumed);
                        }
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
