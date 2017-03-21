using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using Microsoft.Xunit.Performance;

namespace Benchmarks
{
    /// <summary>
    /// A class containing benchmarks for comparing constructs that do the same task in
    /// different ways with the same end outcome.
    /// </summary>
    public class ComparisonBench
    {
        #region Slice+Span vs Span+Slice

        const int SliceSpanIterations = 100000;
        const int SliceSpanDataSize = 1000;

        static readonly Buffer<int> SliceSpanBuffer = new int[SliceSpanDataSize];

        /// <summary>
        /// A method that tests slicing a Buffer<T> before reading the Span property.
        /// 
        /// The useful attribute to this test is more likely the instructions retired
        /// than the time it takes. Compare these results with the other method for doing
        /// the same task (MemorySpanThenSlice).
        /// </summary>
        [Benchmark]
        public static void MemorySliceThenSpan()
        {
            GetBookEnds(out int start, out int length);

            Benchmark.Iterate(() => {
                for (var i = 0; i < SliceSpanIterations; i++)
                    DoSliceThenSpan(start, length);
            });
        }

        /// <summary>
        /// A method that tests reading the Span from a Buffer<T> then slicing the Span.
        /// 
        /// The useful attribute to this test is more likely the instructions retired
        /// than the time it takes. Compare these results with the other method for doing
        /// the same task (MemorySliceThenSpan).
        /// </summary>
        [Benchmark]
        public static void MemorySpanThenSlice()
        {
            GetBookEnds(out int start, out int length);

            Benchmark.Iterate(() => {
                for (var i = 0; i < SliceSpanIterations; i++)
                    DoSpanThenSlice(start, length);
            });
        }

        /// <summary>
        /// This function should not be inline or the compiler is likely to optimize
        /// the code completely away since using the result would incur a cost that
        /// we are not trying to measure.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Span<int> DoSpanThenSlice(int start, int length)
        {
            return SliceSpanBuffer.Span.Slice(start, length);
        }

        /// <summary>
        /// This function should not be inline or the compiler is likely to optimize
        /// the code completely away since using the result would incur a cost that
        /// we are not trying to measure.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Span<int> DoSliceThenSpan(int start, int length)
        {
            return SliceSpanBuffer.Slice(start, length).Span;
        }

        static void GetBookEnds(out int start, out int length)
        {
            Random rnd = new Random(42);
            start = rnd.Next(0, SliceSpanDataSize / 2);
            length = rnd.Next(0, SliceSpanDataSize / 2);
        }

        #endregion Slice+Span vs Span+Slice
    }
}
