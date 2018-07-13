using System;
using System.Text;

namespace Benchmarks.System.Text.Primitives
{
    public class Utf8ByteArrayArgument
    {
        public byte[] Bytes { get; }

        public string Text { get; }

        public Utf8ByteArrayArgument(string text)
        {
            Text = text;
            Bytes = Encoding.UTF8.GetBytes(text);
        }

        public ReadOnlySpan<byte> CreateSpan() => new ReadOnlySpan<byte>(Bytes);

        public override string ToString() => Text; // will be used by BenchmarkDotNet to print human friendly output in the results
    }
}
