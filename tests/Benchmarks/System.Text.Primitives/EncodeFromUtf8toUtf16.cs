using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Code;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Primitives.Tests.Encoding;
using static System.Text.Primitives.Tests.Encoding.TextEncoderTestHelper;

namespace Benchmarks.System.Text.Primitives.Benchmarks
{
    public class EncodeFromUtf8toUtf16
    {
        public IEnumerable<CodePoint> GetEncodingPerformanceTestData()
        {
            yield return new CodePoint(0x0, TextEncoderConstants.Utf8OneByteLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf8OneByteLastCodePoint + 1, TextEncoderConstants.Utf8TwoBytesLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf8TwoBytesLastCodePoint + 1, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            yield return new CodePoint(TextEncoderConstants.Utf16HighSurrogateFirstCodePoint, TextEncoderConstants.Utf16LowSurrogateLastCodePoint);
            yield return new CodePoint(0x0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            yield return new CodePoint(0, 0, SpecialTestCases.AlternatingASCIIAndNonASCII);
            yield return new CodePoint(0, 0, SpecialTestCases.MostlyASCIIAndSomeNonASCII);
        }

        [Params(99, 999, 9999)]
        public int Length;

        [ParamsSource(nameof(GetEncodingPerformanceTestData))]
        public CodePoint CodePointInfo;

        static string inputString;
        static char[] characters;
        static byte[] utf8Source;
        static byte[] utf16Destination;
        static Encoding utf8Encoding;

        [GlobalSetup]
        public void Setup()
        {
            inputString = GenerateStringData(Length, this.CodePointInfo.MinCodePoint, this.CodePointInfo.MaxCodePoint, this.CodePointInfo.Special);
            characters = inputString.AsSpan().ToArray();
            utf8Encoding = Encoding.UTF8;
            int utf8Length = utf8Encoding.GetByteCount(characters);
            utf8Source = new byte[utf8Length];
            utf8Encoding.GetBytes(characters, 0, characters.Length, utf8Source, 0);

            var status = Encodings.Utf8.ToUtf16Length(utf8Source, out int needed);
            if (status != OperationStatus.Done)
                throw new Exception();

            utf16Destination = new byte[needed];
        }

        [Benchmark(Baseline = true)]
        public void EncodeFromUtf8toUtf16UsingEncoding()
        {
            utf8Encoding.GetChars(utf8Source, 0, utf8Source.Length, characters, 0);
        }

        [Benchmark]
        public OperationStatus EncodeFromUtf8toUtf16UsingTextEncoder()
        {
            var status = Encodings.Utf8.ToUtf16(utf8Source, utf16Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
