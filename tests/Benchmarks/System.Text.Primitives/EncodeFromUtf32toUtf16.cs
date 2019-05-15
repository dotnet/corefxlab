// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Generic;

namespace System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf32toUtf16
    {
        public IEnumerable<CodePoint> GetEncodingPerformanceTestData()
        {
            return EncoderHelper.GetEncodingPerformanceTestData();
        }

        [Params(99, 999, 9999)]
        public int Length;

        [ParamsSource(nameof(GetEncodingPerformanceTestData))]
        public CodePoint CodePointInfo;

        private static char[] _characters;
        private static byte[] _utf32Source;
        private static byte[] _utf16Destination;
        private static Encoding _utf32Encoding;

        [GlobalSetup]
        public void Setup()
        {
            string inputString = EncoderHelper.GenerateStringData(Length, CodePointInfo.MinCodePoint, CodePointInfo.MaxCodePoint, CodePointInfo.Special);
            _characters = inputString.AsSpan().ToArray();

            _utf32Encoding = Encoding.UTF32;
            _utf32Source = Text.Encoding.UTF32.GetBytes(inputString);

            OperationStatus status = Buffers.Text.TextEncodings.Utf32.ToUtf16Length(_utf32Source, out int needed);
            if (status != OperationStatus.Done)
                throw new Exception();

            _utf16Destination = new byte[needed];
        }

        [Benchmark(Baseline = true)]
        public int EncodeFromUtf32toUtf16UsingEncoding()
        {
            return _utf32Encoding.GetChars(_utf32Source, 0, _utf32Source.Length, _characters, 0);
        }

        [Benchmark]
        public OperationStatus EncodeFromUtf32toUtf16UsingTextEncoder()
        {
            OperationStatus status = Buffers.Text.TextEncodings.Utf32.ToUtf16(_utf32Source, _utf16Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
