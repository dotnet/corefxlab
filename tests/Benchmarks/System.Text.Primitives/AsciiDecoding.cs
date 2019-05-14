// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Utf8;
using BenchmarkDotNet.Attributes;

namespace System.Text.Primitives.Benchmarks
{
    public class AsciiDecoding
    {
        private byte[] bytes;

        [Params("/plaintext", "text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7")]
        public string Text;

        [GlobalSetup]
        public void Setup() => bytes = Encoding.ASCII.GetBytes(Text);

        [Benchmark]
        public string AsciiToStringPrimitives() => Buffers.Text.TextEncodings.Ascii.ToUtf16String(bytes);

        [Benchmark(Baseline = true)]
        public string AsciiToStringClr() => Encoding.ASCII.GetString(bytes);

        [Benchmark]
        public string Utf8ToStringTextEncoder() => new Utf8Span(bytes).ToString();
    }
}

