// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace System.Buffers.Benchmarks
{
    public class Reader_Binary
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static byte[] s_buffer;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[100_000];
            s_buffer = new byte[4096];
            Random r = new Random(42);
            r.NextBytes(s_array);
            s_ros = new ReadOnlySequence<byte>(s_array);
        }

        [Benchmark]
        public void ReadInt32()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);

            while (reader.TryRead(out int value))
            {
            }
        }

        [Benchmark]
        public void ReadInt32_BigEndian()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);

            while (reader.TryReadInt32BigEndian(out int value))
            {
            }
        }

        [Benchmark]
        public void ReadInt32_LittleEndian()
        {
            BufferReader<byte> reader = new BufferReader<byte>(s_ros);

            while (reader.TryReadInt32LittleEndian(out int value))
            {
            }
        }

        [Benchmark]
        public void PeekSpan()
        {
            const int Count = 1000;
            const int Iterations = 100_000;
            Span<byte> span = new Span<byte>(s_buffer, 0, Count);

            BufferReader<byte> reader = new BufferReader<byte>(s_ros);
            for (int i = 0; i < Iterations; i++)
                reader.Peek(span);
        }
    }
}
