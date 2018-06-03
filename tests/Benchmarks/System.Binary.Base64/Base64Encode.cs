using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System;
using System.Binary.Base64Experimental;
using System.Buffers;

namespace Benchmarks.System.Binary.Base64.Benchmarks
{
    public class Base64Encode
    {
        [Params(10, 100, 1000, 1000 * 1000)]
        public int NumberOfBytes;
        private static readonly StandardFormat format = new StandardFormat('M');

        static byte[] source;
        static byte[] bytesDestination;
        static char[] charDestination;
        static int consumed;
        static int written;

        [GlobalSetup]
        public void Setup()
        {
            source = new byte[NumberOfBytes];
            Base64TestHelper.InitalizeBytes(source.AsSpan());
            bytesDestination = new byte[Base64Experimental.GetMaxEncodedToUtf8Length(NumberOfBytes, format)];
            charDestination = new char[Base64Experimental.GetMaxEncodedToUtf8Length(NumberOfBytes, format)];
        }

        [Benchmark]
        public OperationStatus WithLineBreaks()
        {
            return Base64Experimental.EncodeToUtf8(source, bytesDestination, out consumed, out written, format);
        }

        [Benchmark(Baseline = true)]
        public int ConvertWithLineBreaks()
        {
            return Convert.ToBase64CharArray(source, 0, source.Length, charDestination, 0, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
