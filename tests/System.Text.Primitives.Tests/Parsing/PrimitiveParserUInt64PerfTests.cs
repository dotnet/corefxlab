// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Xunit;
using Microsoft.Xunit.Performance;
using System.Buffers;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineSimpleByteStarToUInt64(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, out value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt64(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private static void BaselineByteStarToUInt64Hex(string text)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        ulong.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        //[Benchmark]
        //[InlineData("2134567890")] // standard parse
        //[InlineData("18446744073709551615")] // max value
        //[InlineData("0")] // min value
        //private unsafe static void PrimitiveParserByteStarToUInt64(string text)
        //{
        //    int length = text.Length;
        //    byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        fixed (byte* utf8ByteStar = utf8ByteArray)
        //        {
        //            using (iteration.StartMeasurement())
        //            {
        //                for (int i = 0; i < TestHelper.LoadIterations; i++)
        //                {
        //                    ulong value;
        //                    Parsers.Utf8.TryParseUInt64(utf8ByteStar, length, out value);
        //                    TestHelper.DoNotIgnore(value, 0);
        //                }
        //            }
        //        }
        //    }
        //}

        //[Benchmark]
        //[InlineData("2134567890")] // standard parse
        //[InlineData("18446744073709551615")] // max value
        //[InlineData("0")] // min value
        //private unsafe static void PrimitiveParserByteStarToUInt64_BytesConsumed(string text)
        //{
        //    int length = text.Length;
        //    byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        fixed (byte* utf8ByteStar = utf8ByteArray)
        //        {
        //            using (iteration.StartMeasurement())
        //            {
        //                for (int i = 0; i < TestHelper.LoadIterations; i++)
        //                {
        //                    ulong value;
        //                    int bytesConsumed;
        //                    Parsers.Utf8.TryParseUInt64(utf8ByteStar, length, out value, out bytesConsumed);
        //                    TestHelper.DoNotIgnore(value, bytesConsumed);
        //                }
        //            }
        //        }
        //    }
        //}

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        Utf8Parser.TryParseUInt64(utf8ByteSpan, out value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("2134567890")] // standard parse
        [InlineData("18446744073709551615")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        int bytesConsumed;
                        Utf8Parser.TryParseUInt64(utf8ByteSpan, out value, out bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }

        //[Benchmark]
        //[InlineData("abcdef")] // standard parse
        //[InlineData("ffffffffffffffff")] // max value
        //[InlineData("0")] // min value
        //private unsafe static void PrimitiveParserByteStarToUInt64Hex(string text)
        //{
        //    int length = text.Length;
        //    byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        fixed (byte* utf8ByteStar = utf8ByteArray)
        //        {
        //            using (iteration.StartMeasurement())
        //            {
        //                for (int i = 0; i < TestHelper.LoadIterations; i++)
        //                {
        //                    ulong value;
        //                    Parsers.Utf8.Hex.TryParseUInt64(utf8ByteStar, length, out value);
        //                    TestHelper.DoNotIgnore(value, 0);
        //                }
        //            }
        //        }
        //    }
        //}

        //[Benchmark]
        //[InlineData("abcdef")] // standard parse
        //[InlineData("ffffffffffffffff")] // max value
        //[InlineData("0")] // min value
        //private unsafe static void PrimitiveParserByteStarToUInt64Hex_BytesConsumed(string text)
        //{
        //    int length = text.Length;
        //    byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        fixed (byte* utf8ByteStar = utf8ByteArray)
        //        {
        //            using (iteration.StartMeasurement())
        //            {
        //                for (int i = 0; i < TestHelper.LoadIterations; i++)
        //                {
        //                    ulong value;
        //                    int bytesConsumed;
        //                    Parsers.Utf8.Hex.TryParseUInt64(utf8ByteStar, length, out value, out bytesConsumed);
        //                    TestHelper.DoNotIgnore(value, bytesConsumed);
        //                }
        //            }
        //        }
        //    }
        //}

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64Hex(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        Utf8Parser.Hex.TryParseUInt64(utf8ByteSpan, out value);
                        TestHelper.DoNotIgnore(value, 0);
                    }
                }
            }
        }

        [Benchmark]
        [InlineData("abcdef")] // standard parse
        [InlineData("ffffffffffffffff")] // max value
        [InlineData("0")] // min value
        private unsafe static void PrimitiveParserByteSpanToUInt64Hex_BytesConsumed(string text)
        {
            byte[] utf8ByteArray = Text.Encoding.UTF8.GetBytes(text);
            ReadOnlySpan<byte> utf8ByteSpan = new ReadOnlySpan<byte>(utf8ByteArray);
            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < TestHelper.LoadIterations; i++)
                    {
                        ulong value;
                        int bytesConsumed;
                        Utf8Parser.Hex.TryParseUInt64(utf8ByteSpan, out value, out bytesConsumed);
                        TestHelper.DoNotIgnore(value, bytesConsumed);
                    }
                }
            }
        }
    }
}
