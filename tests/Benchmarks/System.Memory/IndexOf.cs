// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace System.Memory.Benchmarks
{
    public class IndexOf
    {
        static byte[] s_buffer = new byte[2000];

        [GlobalSetup]
        public void PrintInfo() => Console.WriteLine($"// Vector.IsHardwareAccelerated={Vector.IsHardwareAccelerated}"); // just print the info to help understand the results

        [Benchmark(OperationsPerInvoke = 6 * 4)] // avoid loop overhead, do manuall loop unrolling
        [Arguments(0)] // we don't have more arguments here because the CPU cache gets warmed-up very soon and the results are the same for every $at
        [Arguments(1000)]
        public int SpanIndexOf(int at)
        {
            Span<byte> span = s_buffer; // this has a price, so the OperationsPerInvoke needs to be big to not spoil the result of IndexOf benchmark

            int value = 0;

            value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at];
            value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at];
            value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at];
            value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at]; value += span[at];

            return value;
        }
    }
}

