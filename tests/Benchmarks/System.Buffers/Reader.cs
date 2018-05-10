// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Reader;
using System.Buffers.Text;
using System.Text;

namespace System.Buffers.Benchmarks
{
    public class Reader
    {
        static byte[] s_array;
        static ReadOnlySequence<byte> s_ros;

        [GlobalSetup]
        public void Setup()
        {
            var sections = 100000;
            var section = "1234 ";
            var builder = new StringBuilder(sections * section.Length);
            for (int i = 0; i < sections; i++)
            {
                builder.Append(section);
            }
            s_array = Encoding.UTF8.GetBytes(builder.ToString());
            s_ros = new ReadOnlySequence<byte>(s_array);
        }

        [Benchmark(Baseline = true)]
        public void ParseInt32Utf8Parser()
        {
            var span = new ReadOnlySpan<byte>(s_array);

            int totalConsumed = 0;
            while (Utf8Parser.TryParse(span.Slice(totalConsumed), out int value, out int consumed))
            {
                totalConsumed += consumed + 1;
            }
        }

        [Benchmark]
        public void ParseInt32BufferReader()
        {
            var reader = BufferReader.Create(s_ros);

            while (BufferReaderExtensions.TryParse(ref reader, out int value))
            {
                reader.Advance(1); // advance past the delimiter
            }
        }

        [Benchmark]
        public void ParseInt32BufferReaderRaw()
        {
            var reader = BufferReader.Create(s_ros);

            while (Utf8Parser.TryParse(reader.CurrentSegment.Slice(reader.ConsumedBytes), out int value, out int consumed))
            {
                reader.Advance(consumed + 1);
            }
        }
    }
}
