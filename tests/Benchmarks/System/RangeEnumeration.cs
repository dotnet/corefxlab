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

        [GlobalSetup]
        public void Setup()
        {
            _range = new Range(0, Length);
        }

        [Benchmark]
        public void Enumeration()
        {
            foreach (int value in _range)
            {
            }
        }
    }
}
