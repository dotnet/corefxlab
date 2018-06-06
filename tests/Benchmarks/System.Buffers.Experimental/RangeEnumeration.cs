// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace System.Buffers.Experimental.Benchmarks
{
    public class RangeEnumeration
    {
        [Params(10, 100, 1000, 10_000)]
        public uint Length;

        private Range _range;
        private int[] _array;

        [GlobalSetup]
        public void Setup()
        {
            _range = new Range(0, Length);
            _array = new int[Length];
        }

        [Benchmark(Baseline = true)]
        public void RegularForLoop()
        {
            for (int i = 0; i < Length; i++)
            {
            }
        }

        [Benchmark]
        public void EnumerationArray()
        {
            foreach (int value in _array)
            {
            }
        }

        [Benchmark]
        public void EnumerationRange()
        {
            foreach (int value in _range)
            {
            }
        }
    }
}
