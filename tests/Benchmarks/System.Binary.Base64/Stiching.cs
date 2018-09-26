// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Binary.Base64Experimental.Tests;
using Base64Decoder = System.Buffers.Text.Base64; // This name problematic, since Base64 is part of namespace.

namespace System.Binary.Base64.Benchmarks
{
    public class Stiching
    {
        [Params(1_000, 5_000, 10_000, 20_000, 50_000)]
        public int InputBufferSize;

        private static byte[] _source;
        private static byte[] _alignedSource1;
        private static byte[] _alignedSource2;
        private static byte[] _notAlignedSource1;
        private static byte[] _notAlignedSource2;
        private static byte[] _destination;
        private static byte[] _expected;

        [GlobalSetup]
        public void Setup()
        {
            _source = new byte[InputBufferSize];
            Base64TestHelper.InitalizeDecodableBytes(_source);
            _expected = new byte[InputBufferSize];
            Base64Decoder.DecodeFromUtf8(_source, _expected, out int expectedConsumed, out int expectedWritten);

            Base64TestHelper.SplitSourceIntoSpans(_source, false, out ReadOnlySpan<byte> alignedSource1, out ReadOnlySpan<byte> alignedSource2);
            _alignedSource1 = alignedSource1.ToArray();
            _alignedSource2 = alignedSource2.ToArray();

            Base64TestHelper.SplitSourceIntoSpans(_source, false, out ReadOnlySpan<byte> notAlignedSource1, out ReadOnlySpan<byte> notAlignedSource2);
            _notAlignedSource1 = notAlignedSource1.ToArray();
            _notAlignedSource2 = notAlignedSource2.ToArray();

            _destination = new byte[InputBufferSize]; // Plenty of space
        }

        [Benchmark(Baseline = true)]
        public void NoStiching()
        {
            Base64TestHelper.DecodeNoNeedToStich(_alignedSource1, _alignedSource2, _destination, out _, out _);
        }

        [Benchmark]
        [Arguments(10)]
        [Arguments(32)]
        [Arguments(50)]
        [Arguments(64)]
        [Arguments(100)]
        [Arguments(500)]
        public void StichingRequired(int stackSize)
        {
            Span<byte> stackSpan = stackalloc byte[stackSize];
            Base64TestHelper.DecodeStichUsingStack(_notAlignedSource1, _notAlignedSource2, _destination, stackSpan, out _, out _);
        }

        [Benchmark]
        public void StichingRequiredNoThirdCall()
        {
            Span<byte> stackSpan = stackalloc byte[600 * InputBufferSize / 1000];
            Base64TestHelper.DecodeStichUsingStack(_notAlignedSource1, _notAlignedSource2, _destination, stackSpan, out _, out _);
        }
    }
}
