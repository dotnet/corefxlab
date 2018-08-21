// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace System.Text.Primitives.Benchmarks
{
    public class AsciiCasing
    {
        private byte[] _bytes;
        private byte[] _output;

        [Params("/plaintext", "text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
        public string Text;

        [GlobalSetup]
        public void Setup()
        {
            _bytes = Encoding.ASCII.GetBytes(Text);
            _output = new byte[_bytes.Length];
        }

        [Benchmark]
        public OperationStatus AsciiToLowerInPlace() => Buffers.Text.Encodings.Ascii.ToLowerInPlace(_bytes, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToLower() => Buffers.Text.Encodings.Ascii.ToLower(_bytes, _output, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToUpperInPlace() => Buffers.Text.Encodings.Ascii.ToUpperInPlace(_bytes, out int bytesChanged);

        [Benchmark]
        public OperationStatus AsciiToUpper() => Buffers.Text.Encodings.Ascii.ToUpper(_bytes, _output, out int bytesChanged);
    }
}
