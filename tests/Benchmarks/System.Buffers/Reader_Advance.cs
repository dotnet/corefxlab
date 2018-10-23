// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Tests;

namespace System.Buffers.Benchmarks
{
    public class Reader_Advance
    {
        private static byte[] s_arrayBlobs;
        private static ReadOnlySequence<byte> s_rosBlobs;
        private static ReadOnlySequence<byte> s_rosBlobsSplit;

        [GlobalSetup]
        public void Setup()
        {
            Random r = new Random(42);

            // Create random runs of 0x00 - 0xFF
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
        public unsafe void Advance()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobsSplit);
            try
            {
                while (!reader.End)
                {
                    reader.Advance(255);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [Benchmark]
        public unsafe void Advance_SingleSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobs);
            while (!reader.End)
            {
                reader.Advance(1);
            }
        }

        [Benchmark]
        public unsafe void AdvancePast()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobsSplit);
            byte value = 0x00;
            while (reader.AdvancePast(value) != 0)
            {
                value++;
            }
        }

        [Benchmark]
        public unsafe void AdvancePast_SingleSegment()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosBlobs);
            byte value = 0x00;
            while (reader.AdvancePast(value) != 0)
            {
                value++;
            }
        }
    }
}
