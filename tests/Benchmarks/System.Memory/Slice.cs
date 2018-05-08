// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace System.Memory.Benchmarks
{
    /// <summary>
    /// A class containing benchmarks for comparing constructs that do the same task in
    /// different ways with the same end outcome.
    /// </summary>
    [HardwareCounters(HardwareCounter.InstructionRetired)] // comment if you can't run it as Admin on Windows
    public class Slice
    {
        const int SliceSpanDataSize = 1000;

        static readonly Memory<int> SliceSpanBuffer = new int[SliceSpanDataSize];

        /// <summary>
        /// A method that tests slicing a Memory<T> before reading the Span property.
        /// 
        /// The useful attribute to this test is more likely the instructions retired
        /// than the time it takes. Compare these results with the other method for doing
        /// the same task (MemorySpanThenSlice).
        /// </summary>
        [Benchmark]
        public Span<int> MemorySliceThenSpan() => SliceSpanBuffer.Slice(0, SliceSpanDataSize - 1).Span;

        /// <summary>
        /// A method that tests reading the Span from a Memory<T> then slicing the Span.
        /// 
        /// The useful attribute to this test is more likely the instructions retired
        /// than the time it takes. Compare these results with the other method for doing
        /// the same task (MemorySliceThenSpan).
        /// </summary>
        [Benchmark]
        public Span<int> MemorySpanThenSlice() => SliceSpanBuffer.Span.Slice(0, SliceSpanDataSize - 1);
    }
}
