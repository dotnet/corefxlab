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
    public class EncodeFromUtf32toUtf8
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
        static byte[] utf8Destination;
        static byte[] utf32Source;
        static byte[] utf16Source;
        static byte[] utf16Destination;
        static Encoding utf8Encoding;
        static Encoding utf32Encoding;

        [GlobalSetup]
        public void Setup()
        {
            inputString = GenerateStringData(Length, this.CodePointInfo.MinCodePoint, this.CodePointInfo.MaxCodePoint, this.CodePointInfo.Special);
            characters = inputString.AsSpan().ToArray();
            utf8Encoding = Encoding.UTF8;
            int utf8Length = utf8Encoding.GetByteCount(characters);
            utf8Destination = new byte[utf8Length];
            utf32Encoding = Encoding.UTF32;
            
            int utf32Length = utf32Encoding.GetByteCount(characters);
            utf32Source = new byte[utf32Length];
            utf32Encoding.GetBytes(characters, 0, characters.Length, utf32Source, 0);
        }

        [Benchmark(Baseline = true)]
        public void UsingEncoding()
        {
            utf32Encoding.GetChars(utf32Source, 0, utf32Source.Length, characters, 0);
            utf8Encoding.GetBytes(characters, 0, characters.Length, utf8Destination, 0);
        }

        [Benchmark]
        public OperationStatus UsingTextEncoder()
        {
            var status = Encodings.Utf32.ToUtf8(utf32Source, utf8Destination, out int consumed, out int written);
            if (status != OperationStatus.Done)
                throw new Exception();

            return status;
        }
    }
}
