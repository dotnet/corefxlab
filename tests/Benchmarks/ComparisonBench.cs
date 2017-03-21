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
        const int StartMarker = 4;
        const int SliceLength = SliceSpanDataSize - 20;

        static readonly Buffer<int> Buffer = new int[SliceSpanDataSize];

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
            Benchmark.Iterate(() => {
                for (var i = 0; i < SliceSpanIterations; i++)
                    DoSliceThenSpan<int>(Buffer, StartMarker, SliceLength);
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
            Benchmark.Iterate(() => {
                for (var i = 0; i < SliceSpanIterations; i++)
                    DoSpanThenSlice<int>(Buffer, StartMarker, SliceLength);
            });
        }

        /// <summary>
        /// This function should not be inline or the compiler is likely to optimize
        /// the code completely away since using the result would incur a cost that
        /// we are not trying to measure.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Span<T> DoSpanThenSlice<T>(Buffer<T> buffer, int start, int length)
        {
            return buffer.Span.Slice(start, length);
        }

        /// <summary>
        /// This function should not be inline or the compiler is likely to optimize
        /// the code completely away since using the result would incur a cost that
        /// we are not trying to measure.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Span<T> DoSliceThenSpan<T>(Buffer<T> buffer, int start, int length)
        {
            return buffer.Slice(start, length).Span;
        }

        #endregion Slice+Span vs Span+Slice
    }
}
