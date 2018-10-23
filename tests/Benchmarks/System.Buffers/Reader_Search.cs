// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Tests;

namespace System.Buffers.Benchmarks
{
    public class Reader_Search
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static ReadOnlySequence<byte> s_rosSplit;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[1_000_000];
            Random r = new Random(42);
            r.NextBytes(s_array);
            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_array, 10, 100);
        }

        [Benchmark]
        public void TryReadTo_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TryReadTo(out ReadOnlySequence<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_2_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_5_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void Reader_Position()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            for (int i = 0; i < 10_000; i++)
            {
                SequencePosition position = reader.Position;
            }
        }

        [Benchmark]
        public void TryReadTo_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TryReadTo(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadTo_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            while (reader.TryReadTo(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_2_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_2_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_5_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadToAny_5_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public unsafe void TryReadTo_2Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte* b = stackalloc byte[] { 0x0, 0x1 };
            Span<byte> span = new Span<byte>(b, 2);
            while (reader.TryReadTo(out ReadOnlySequence<byte> bytes, span))
            {
            }
        }

        [Benchmark]
        public void TryAdvanceTo()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TryAdvanceTo(42))
            {
            }
        }

        [Benchmark]
        public void TryAdvanceTo_SingleSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            while (reader.TryAdvanceTo(42))
            {
            }
        }

        [Benchmark]
        public unsafe void TrySkipToAny_AndSkipPastAny()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte* b = stackalloc byte[] { 0x00, 0xFF };
            Span<byte> span = new Span<byte>(b, 2);
            while (reader.TryAdvanceToAny(span, advancePastDelimiter: false))
            {
                reader.AdvancePastAny(span);
            }
        }
    }
}
