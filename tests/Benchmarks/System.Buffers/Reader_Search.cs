// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Reader;
using System.Buffers.Testing;

namespace System.Buffers.Benchmarks
{
    public class Reader_Search
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static ReadOnlySequence<byte> s_rosSplit;

        private static byte[] s_arrayBlobs;
        private static ReadOnlySequence<byte> s_rosBlobs;
        private static ReadOnlySequence<byte> s_rosBlobsSplit;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[1_000_000];
            Random r = new Random(42);
            r.NextBytes(s_array);
            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_array, 10, 100);

            s_arrayBlobs = new byte[1_000_000];
            int remaining = s_arrayBlobs.Length;
            Span<byte> blobs = new Span<byte>(s_arrayBlobs);
            byte value = 0x00;
            while (remaining > 0)
            {
                int run = Math.Min(r.Next(3, 15), remaining);
                for (int j = 0; j < run; j++)
                {
                    blobs[j] = value;
                }
                value++;
                remaining -= run;
                blobs.Slice(run);
            }

            s_rosBlobs = new ReadOnlySequence<byte>(s_arrayBlobs);
            s_rosBlobsSplit = BufferUtilities.CreateSplitBuffer(s_arrayBlobs, 10, 100);
        }

        [Benchmark]
        public void TryReadUntil_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TryReadTo(out ReadOnlySequence<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Sequence()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySequence<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntil_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TryReadTo(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntil_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            while (reader.TryReadTo(out ReadOnlySpan<byte> bytes, 42))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_2_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            byte[] delimiters = { 0, 255 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public void TryReadUntilAny_5_Span_OneSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            byte[] delimiters = { 2, 3, 5, 7, 11 };
            while (reader.TryReadToAny(out ReadOnlySpan<byte> bytes, delimiters))
            {
            }
        }

        [Benchmark]
        public unsafe void TryReadUntil_2Span()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte* b = stackalloc byte[] { 0x0, 0x1 };
            Span<byte> span = new Span<byte>(b, 2);
            while (reader.TryReadTo(out ReadOnlySequence<byte> bytes, span))
            {
            }
        }

        [Benchmark]
        public void TrySkipTo()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            while (reader.TrySkipTo(42))
            {
            }
        }

        [Benchmark]
        public void TrySkipTo_SingleSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            while (reader.TrySkipTo(42))
            {
            }
        }

        [Benchmark]
        public unsafe void TrySkipToAny_AndSkipPastAny()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);
            byte* b = stackalloc byte[] { 0x00, 0xFF };
            Span<byte> span = new Span<byte>(b, 2);
            while (reader.TrySkipToAny(span, advancePastDelimiter: false))
            {
                reader.SkipPastAny(span);
            }
        }

        [Benchmark]
        public unsafe void SkipPast()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobsSplit);
            byte value = 0x00;
            while (reader.SkipPast(value))
            {
                value++;
            }
        }

        [Benchmark]
        public unsafe void SkipPast_SingleSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobs);
            byte value = 0x00;
            while (reader.SkipPast(value))
            {
                value++;
            }
        }
    }
}
