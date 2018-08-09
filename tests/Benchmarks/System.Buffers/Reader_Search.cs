// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Reader;
using System.Buffers.Testing;
using System.Buffers.Text;

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
        public void TryReadUntil_Sequence()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            while (reader.TryReadUntil(out ReadOnlySequence<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Sequence()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadUntilAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Sequence()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadUntilAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntil_Span()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            while (reader.TryReadUntil(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntil_Span_OneSegment()
        {
            BufferReader reader = new BufferReader(s_ros);
            while (reader.TryReadUntil(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Span()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadUntilAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Span_OneSegment()
        {
            BufferReader reader = new BufferReader(s_ros);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadUntilAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Span()
        {
            BufferReader reader = new BufferReader(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadUntilAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Span_OneSegment()
        {
            BufferReader reader = new BufferReader(s_ros);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadUntilAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }
    }
}
