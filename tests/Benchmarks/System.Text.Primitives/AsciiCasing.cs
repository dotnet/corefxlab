using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace System.Text.Primitives.Benchmarks
{
    public class AsciiCasing
    {
        private byte[] bytes;
        private byte[] output;

        [Params("/plaintext", "text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
        public string Text;


        [GlobalSetup]
        public void Setup()
        {
            bytes = Encoding.ASCII.GetBytes(Text);
            output = new byte[bytes.Length];
        }

        [Benchmark]
        public OperationStatus AsciiToLowerInPlace() => Buffers.Text.Encodings.Ascii.ToLowerInPlace(bytes, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToLower() => Buffers.Text.Encodings.Ascii.ToLower(bytes, output, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToUpperInPlace() => Buffers.Text.Encodings.Ascii.ToUpperInPlace(bytes, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToUpper() => Buffers.Text.Encodings.Ascii.ToUpper(bytes, output, out int bytesChanged);
    }
}
