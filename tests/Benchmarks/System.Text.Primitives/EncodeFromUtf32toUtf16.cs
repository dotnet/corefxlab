using BenchmarkDotNet.Attributes;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using System.Text.Primitives.Tests.Encoding;
using static System.Text.Primitives.Tests.Encoding.TextEncoderTestHelper;

namespace Benchmarks.System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf32toUtf16
    {
        public IEnumerable<CodePoint> GetEncodingPerformanceTestData()
        {
            return TextEncoderTestHelper.GetEncodingPerformanceTestData();
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
            var inputString = GenerateStringData(Length, this.CodePointInfo.MinCodePoint, this.CodePointInfo.MaxCodePoint, this.CodePointInfo.Special);
            _characters = inputString.AsSpan().ToArray();
            var utf8Encoding = Encoding.UTF8;
            int utf8Length = utf8Encoding.GetByteCount(_characters);
            var utf8Source = new byte[utf8Length];
            _utf32Encoding = Encoding.UTF32;

            var status = Encodings.Utf8.ToUtf16Length(utf8Source, out int needed);
            if (status != OperationStatus.Done)
                throw new Exception();

            _utf16Destination = new byte[needed];
            
            int utf32Length = _utf32Encoding.GetByteCount(_characters);
            _utf32Source = new byte[utf32Length];
            _utf32Encoding.GetBytes(_characters, 0, _characters.Length, _utf32Source, 0);
        }

        [Benchmark(Baseline = true)]
        public int EncodeFromUtf32toUtf16UsingEncoding()
        {
            return _utf32Encoding.GetChars(_utf32Source, 0, _utf32Source.Length, _characters, 0);
        }

        [Benchmark]
        public OperationStatus EncodeFromUtf32toUtf16UsingTextEncoder()
        {
            OperationStatus status = Encodings.Utf32.ToUtf16(_utf32Source, _utf16Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
