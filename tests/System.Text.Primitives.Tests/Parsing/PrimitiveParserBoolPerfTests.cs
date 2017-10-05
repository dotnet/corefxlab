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
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
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
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        Utf8Parser.TryParseBoolean(utf8ByteSpan, out value, out bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark]
        //[InlineData("True")]
        //[InlineData("False")]
        //private unsafe static void PrimitiveParserByteStarToBool(string text)
        //{
        //    bool value;
        //    int bytesConsumed;
        //    byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        fixed (byte* utf8ByteStar = utf8ByteArray)
        //        {
        //            using (iteration.StartMeasurement())
        //            {
        //                for (int i = 0; i < TestHelper.LoadIterations; i++)
        //                {
        //                    Parsers.Utf8.TryParseBoolean(utf8ByteStar, utf8ByteArray.Length, out value, out bytesConsumed);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
