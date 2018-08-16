// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Reader;
using System.Buffers.Testing;
using System.Buffers.Text;

namespace System.Buffers.Benchmarks
{
    public class Reader_ParseInt
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static ReadOnlySequence<byte> s_rosSplit;
        private static byte[] s_buffer;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[10_000_000];
            s_buffer = new byte[4096];

            Random r = new Random(42);

            Span<byte> span = new Span<byte>(s_array);

            // Generate ints across the entire range
            int next = r.Next(int.MinValue, int.MaxValue) + r.Next(-1, 1);
            while (Utf8Formatter.TryFormat(next, span, out int written) && span.Length > written)
            {
                next = r.Next(int.MinValue, int.MaxValue) + r.Next(-1, 1);
                span = span.Slice(written + 1);
            }

            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_buffer, 100, 1000);
        }

        [Benchmark]
        public void ParseInt32()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);

            while (reader.TryParse(out int value))
            {
                // Skip the delimiter
                reader.Advance(1);
            }
        }

        [Benchmark]
        public void ParseInt32_Split()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_rosSplit);

            while (reader.TryParse(out int value))
            {
                // Skip the delimiter
                reader.Advance(1);
            }
        }
    }
}
