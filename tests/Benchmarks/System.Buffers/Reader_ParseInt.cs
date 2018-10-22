// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers.Tests;

namespace System.Buffers.Benchmarks
{
    public class Reader_ParseInt
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static ReadOnlySequence<byte> s_rosSplit;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[10_000_000];

            BufferUtilities.FillIntegerUtf8Array(s_array, int.MinValue, int.MaxValue);

            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_array, 100, 1000);
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
