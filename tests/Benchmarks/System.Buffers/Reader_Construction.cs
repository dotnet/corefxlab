// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Tests;
using System.Runtime.CompilerServices;

namespace System.Buffers.Benchmarks
{
    public class Reader_Construction
    {
        private static byte[] s_array;
        private static ReadOnlySequence<byte> s_ros;
        private static ReadOnlySequence<byte> s_rosSplit;

        [GlobalSetup]
        public void Setup()
        {
            s_array = new byte[1000];
            Random r = new Random(1776);
            r.NextBytes(s_array);
            s_ros = new ReadOnlySequence<byte>(s_array);
            s_rosSplit = BufferUtilities.CreateSplitBuffer(s_array, 10, 100);
        }

        [Benchmark]
        public void ConstructSingleSegment()
        {
            new BufferReader<byte>(s_ros);
        }

        [Benchmark]
        public void ConstructMultiSegment()
        {
            new BufferReader<byte>(s_rosSplit);
        }
    }
}
