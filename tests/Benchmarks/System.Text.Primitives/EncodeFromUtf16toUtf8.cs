// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf16toUtf8
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
        private static byte[] _utf8Destination;
        private static byte[] _utf16Source;
        private static Encoding _utf8Encoding;

        [GlobalSetup]
        public void Setup()
        {
            string inputString = EncoderHelper.GenerateStringData(Length, this.CodePointInfo.MinCodePoint, CodePointInfo.MaxCodePoint, CodePointInfo.Special);
            _characters = inputString.AsSpan().ToArray();
            _utf8Encoding = Encoding.UTF8;
            int utf8Length = _utf8Encoding.GetByteCount(_characters);
            _utf8Destination = new byte[utf8Length];

            _utf16Source = MemoryMarshal.AsBytes(inputString.AsSpan()).ToArray();
        }

        [Benchmark(Baseline = true)]
        public int UsingEncoding()
        {
            return _utf8Encoding.GetBytes(_characters, 0, _characters.Length, _utf8Destination, 0);
        }

        [Benchmark]
        public OperationStatus UsingTextEncoder()
        {
            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(_utf16Source, _utf8Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
