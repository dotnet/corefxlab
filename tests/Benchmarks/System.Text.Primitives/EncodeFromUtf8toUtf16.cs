// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Generic;

namespace System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf8toUtf16
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
        private static byte[] _utf8Source;
        private static byte[] _utf16Destination;
        private static Encoding _utf8Encoding;

        [GlobalSetup]
        public void Setup()
        {
            string inputString = EncoderHelper.GenerateStringData(Length, CodePointInfo.MinCodePoint, CodePointInfo.MaxCodePoint, CodePointInfo.Special);
            _characters = inputString.AsSpan().ToArray();
            _utf8Encoding = Encoding.UTF8;
            int utf8Length = _utf8Encoding.GetByteCount(_characters);
            _utf8Source = new byte[utf8Length];
            _utf8Encoding.GetBytes(_characters, 0, _characters.Length, _utf8Source, 0);

            OperationStatus status = Buffers.Text.TextEncodings.Utf8.ToUtf16Length(_utf8Source, out int needed);
            if (status != OperationStatus.Done)
                throw new Exception();

            _utf16Destination = new byte[needed];
        }

        [Benchmark(Baseline = true)]
        public void EncodeFromUtf8toUtf16UsingEncoding()
        {
            _utf8Encoding.GetChars(_utf8Source, 0, _utf8Source.Length, _characters, 0);
        }

        [Benchmark]
        public OperationStatus EncodeFromUtf8toUtf16UsingTextEncoder()
        {
            OperationStatus status = Buffers.Text.TextEncodings.Utf8.ToUtf16(_utf8Source, _utf16Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
