// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A collection of convenient span helpers, exposed as extension methods.
    /// </summary>
    public static partial class SpanExtensionsLabs
    {
        // span creation helpers:
        /// <summary>
        /// Creates a new slice over the portion of the target array segment.
        /// </summary>
        /// <param name="arraySegment">The target array segment.</param>
        /// </exception>
        public static Span<T> Slice<T>(this ArraySegment<T> arraySegment)
        {
            return new Span<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        public static Span<T> Slice<T>(this T[] array)
        {
            return new Span<T>(array);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target array beginning
        /// at 'start' index.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<T> Slice<T>(this T[] array, int start)
        {
            return new Span<T>(array, start);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target array beginning
        /// at 'start' index and with 'length' items.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <param name="length">The number of items in the new slice.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start or end index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<T> Slice<T>(this T[] array, int start, int length)
        {
            return new Span<T>(array, start, length);
        }

        public static void CopyTo<T>(this T[] array, Span<T> span)
        {
            array.Slice().CopyTo(span);
        }

        public static ArraySegment<T> Slice<T>(this ArraySegment<T> source, int count)
        {
            return new ArraySegment<T>(source.Array, source.Offset + count, source.Count - count);
        }
    }
}

