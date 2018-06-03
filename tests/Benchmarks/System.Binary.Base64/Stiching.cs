using BenchmarkDotNet.Attributes;
using System;
using System.Binary.Base64Experimental;
using Base64Decoder = System.Buffers.Text.Base64;

namespace Benchmarks.System.Binary.Base64.Benchmarks
{
    public class Stiching
    {
        [Params(1000, 5000, 10000, 20000, 50000)]
        public int InputBufferSize;

        static byte[] source;
        static byte[] alignedSource1;
        static byte[] alignedSource2;
        static byte[] notAlignedSource1;
        static byte[] notAlignedSource2;
        static byte[] destination;
        static byte[] expected;
        static int bytesConsumed;
        static int bytesWritten;

        [GlobalSetup]
        public void Setup()
        {
            source = new byte[InputBufferSize];
            Base64TestHelper.InitalizeDecodableBytes(source);
            expected = new byte[InputBufferSize];
            Base64Decoder.DecodeFromUtf8(source, expected, out int expectedConsumed, out int expectedWritten);

            Base64TestHelper.SplitSourceIntoSpans(source, false, out ReadOnlySpan<byte> _alignedSource1, out ReadOnlySpan<byte> _alignedSource2);
            alignedSource1 = _alignedSource1.ToArray();
            alignedSource2 = _alignedSource2.ToArray();

            Base64TestHelper.SplitSourceIntoSpans(source, false, out ReadOnlySpan<byte> _notAlignedSource1, out ReadOnlySpan<byte> _notAlignedSource2);
            notAlignedSource1 = _notAlignedSource1.ToArray();
            notAlignedSource2 = _notAlignedSource2.ToArray();

            destination = new byte[InputBufferSize]; // Plenty of space

            bytesConsumed = 0;
            bytesWritten = 0;
        }

        [Benchmark(Baseline = true)]
        [Arguments(0)]
        public void NoStiching(int stackSize)
        {
            Base64TestHelper.DecodeNoNeedToStich(alignedSource1, alignedSource2, destination, out bytesConsumed, out bytesWritten);
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
            Base64TestHelper.DecodeStichUsingStack(notAlignedSource1, notAlignedSource2, destination, stackSpan, out bytesConsumed, out bytesWritten);
        }

        [Benchmark]
        [Arguments(600)] // This would be actualy transformed to larger size.
        public void StichingRequiredNoThirdCall(int stackSize)
        {
            Span<byte> stackSpan = stackalloc byte[600 * InputBufferSize / 1000];
            Base64TestHelper.DecodeStichUsingStack(notAlignedSource1, notAlignedSource2, destination, stackSpan, out bytesConsumed, out bytesWritten);
        }
    }
}
