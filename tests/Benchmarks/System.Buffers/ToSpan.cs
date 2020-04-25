using System;
using System.Buffers;
using System.Buffers.Tests;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Buffers
{
    public class ToSpan
    {
        public static ReadOnlySequence<byte> Sequence = BufferFactory.Create(Enumerable.Repeat(0, 10000000)
            .Select(x => new Memory<byte>(new byte[] { 0 })));

        [Benchmark]
        public void NewToSpan()
        {
            var Result = Sequence.ToSpan();
        }

        [Benchmark]
        public void OldToSpan()
        {
            SequencePosition position = Sequence.Start;
            ResizableArray<byte> array = new ResizableArray<byte>(1024);
            while (Sequence.TryGet(ref position, out ReadOnlyMemory<byte> buffer))
            {
                array.AddAll(buffer.Span);
            }
            var Result = array.Span;
        }
    }
}
