// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Generic;

namespace System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf32toUtf8
    {
        public IEnumerable<CodePoint> GetEncodingPerformanceTestData()
        {
            return EncoderHelper.GetEncodingPerformanceTestData();
        }

        [Params(99, 999, 9999)]
        public int Length;

        [ParamsSource(nameof(GetEncodingPerformanceTestData))]
        public CodePoint CodePointInfo;

        static char[] _characters;
        static byte[] _utf8Destination;
        static byte[] _utf32Source;
        static Encoding _utf8Encoding;
        static Encoding _utf32Encoding;

        [GlobalSetup]
        public void Setup()
        {
            string inputString = EncoderHelper.GenerateStringData(Length, CodePointInfo.MinCodePoint, CodePointInfo.MaxCodePoint, CodePointInfo.Special);
            _characters = inputString.AsSpan().ToArray();
            _utf8Encoding = Encoding.UTF8;
            int utf8Length = _utf8Encoding.GetByteCount(_characters);
            _utf8Destination = new byte[utf8Length];
            _utf32Encoding = Encoding.UTF32;
            
            int utf32Length = _utf32Encoding.GetByteCount(_characters);
            _utf32Source = new byte[utf32Length];
            _utf32Encoding.GetBytes(_characters, 0, _characters.Length, _utf32Source, 0);
        }

        [Benchmark(Baseline = true)]
        public int UsingEncoding()
        {
            _utf32Encoding.GetChars(_utf32Source, 0, _utf32Source.Length, _characters, 0);
            return _utf8Encoding.GetBytes(_characters, 0, _characters.Length, _utf8Destination, 0);
        }

        [Benchmark]
        public OperationStatus UsingTextEncoder()
        {
            OperationStatus status = Buffers.Text.Encodings.Utf32.ToUtf8(_utf32Source, _utf8Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
