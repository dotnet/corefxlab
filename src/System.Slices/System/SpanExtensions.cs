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
            int spanLength = span.Length;
            if (index >= spanLength || count == 0)
            {
                return -1;
            }

            int length = Math.Min(count, spanLength - index);

            var retVal = SpanExtensions.IndexOf(span.Slice(index, length), value);
            return retVal == -1 ? retVal : index + retVal;

            /*ref byte searchSpace = ref span.DangerousGetPinnableReference();
            Unsafe.Add(ref searchSpace, index);

            return IndexOf(ref searchSpace, value, index, count, span.Length);*/
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf2(this Span<byte> span, int index, int count, byte value)
        {
            return IndexOf(ref span.DangerousGetPinnableReference(), value, index, count, span.Length);
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
            int spanLength = span.Length;
            if (index >= spanLength || count == 0)
            {
                return -1;
            }

            int length = Math.Min(count, spanLength - index);

            var retVal = SpanExtensions.IndexOf(span.Slice(index, length), value);
            return retVal == -1 ? retVal : index + retVal;

            /*ref byte searchSpace = ref span.DangerousGetPinnableReference();
            Unsafe.Add(ref searchSpace, index);

            return IndexOf(ref searchSpace, value, index, count, span.Length);*/
        }

        private static int IndexOf(ref byte searchSpace, byte value, int startIndex, int count, int spanLength)
        {
            Debug.Assert(count >= 0);
            Debug.Assert(spanLength >= 0);
            Debug.Assert(startIndex >= 0);

            int index = startIndex;

            if (startIndex >= spanLength || count == 0)
            {
                return -1;
            }

            int remainingLength = Math.Min(count, spanLength - startIndex);

            Unsafe.Add(ref searchSpace, startIndex);

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

