// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace System.Memory.Benchmarks
{
    public class SimdConfig : ManualConfig
    {
        public SimdConfig()
        {
            // https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/clr-configuration-knobs.md
            Add(Job.ShortRun.With(new EnvironmentVariable[] { new EnvironmentVariable("COMPlus_FeatureSIMD", "1") }).WithId("SIMD Enabled")); 
            Add(Job.ShortRun.With(new EnvironmentVariable[] { new EnvironmentVariable("COMPlus_FeatureSIMD", "0") }).WithId("SIMD Disabled"));
        }
    }

    [Config(typeof(SimdConfig))]
    public class IndexOf
    {
        static byte[] s_buffer = Enumerable.Repeat(byte.MinValue, 2000).ToArray();

        [GlobalSetup]
        public void PrintInfo() => Console.WriteLine($"// Vector.IsHardwareAccelerated={Vector.IsHardwareAccelerated}"); // just print the info to make sure the config works

        [Benchmark(OperationsPerInvoke = 6 * 4)] // avoid loop overhead, do manuall loop unrolling
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(4)]
        [Arguments(8)]
        [Arguments(16)]
        [Arguments(30)]
        [Arguments(1000)]
        public int SpanIndexOf(int at)
        {
            const byte searchedValue = byte.MaxValue;

            Span<byte> span = s_buffer; // this has a price, so the OperationsPerInvoke needs to be big to not spoil the result of IndexOf benchmark
            span[at] = searchedValue;

            int value = 0;

            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);
            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);
            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);
            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);
            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);
            value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue); value += span.IndexOf(searchedValue);

            return value;
        }
    }
}

