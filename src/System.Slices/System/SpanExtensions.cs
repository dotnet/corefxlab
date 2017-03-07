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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith(this ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> slice)
        {
            int length = slice.Length;
            return bytes.Length >= length && (length == 0 || bytes.Slice(0, length).SequenceEqual(slice));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith<T>(this ReadOnlySpan<T> items, ReadOnlySpan<T> slice)
            where T : struct, IEquatable<T>
        {
            int length = slice.Length;
            return items.Length >= length && (length == 0 || items.Slice(0, length).SequenceEqual(slice));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlyMemory<byte> memory, ReadOnlySpan<byte> values)
        {
            return SpanExtensions.IndexOf(memory.Span, values);
        }

        /// <summary>
        /// Searches for the specified value starting at the specified index and returns the index of its first occurrence. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="index">The index within the span from where to start the search.</param>
        /// <param name="count">The number of bytes to search starting from the index.</param>
        /// <param name="value">The value to search for.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this Span<byte> span, int index, int count, byte value)
        {
            return IndexOfHelper(ref span.DangerousGetPinnableReference(), index, count, value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value starting at the specified index and returns the index of its first occurrence. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="index">The index within the span from where to start the search.</param>
        /// <param name="count">The number of bytes to search starting from the index.</param>
        /// <param name="value">The value to search for.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlySpan<byte> span, int index, int count, byte value)
        {
            return IndexOfHelper(ref span.DangerousGetPinnableReference(), index, count, value, span.Length);
        }

        private static int IndexOfHelper(ref byte searchSpace, int index, int count, byte value, int spanLength)
        {
            if (index >= spanLength || count == 0)
            {
                return -1;
            }

            int remainingLength = Math.Min(count, spanLength - index);

            while (remainingLength >= 8)
            {
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;

                remainingLength -= 8;
            }

            while (remainingLength >= 4)
            {
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;

                remainingLength -= 4;
            }

            while (remainingLength > 0)
            {
                if (value == Unsafe.Add(ref searchSpace, ++index))
                    return index;

                remainingLength--;
            }
            return -1;
        }
    }
}

