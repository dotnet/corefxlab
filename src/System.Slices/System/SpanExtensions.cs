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
    public static partial class SpanExtensions
    {
        public static bool StartsWith(this ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> slice)
        {
            if (slice.Length > bytes.Length)
            {
                return false;
            }

            for (int i = 0; i < slice.Length; i++)
            {
                if (bytes[i] != slice[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool StartsWith<T>(this ReadOnlySpan<T> items, ReadOnlySpan<T> slice)
            where T : struct, IEquatable<T>
        {
            if (slice.Length > items.Length) return false;
            return items.Slice(0, slice.Length).SequenceEqual(slice);
        }

        /// <summary>Searches for the specified value and returns the index of the first occurrence within the entire <see cref="T:System.Span" />.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <paramref name="slice" />, if found; otherwise, –1.</returns>
        /// <param name="slice">The <see cref="T:System.Span" /> to search.</param>
        /// <param name="value">The value to locate in <paramref name="slice" />.</param>
        /// <typeparam name="T">The type of the elements of the slice.</typeparam>
        public static int IndexOf<T>(this ReadOnlySpan<T> slice, T value)
           where T : struct, IEquatable<T>
        {
            // @todo: Can capture and iterate ref with DangerousGetPinnedReference() like the IL used to, but need C#7 for that.
            int length = slice.Length;
            for (int i = 0; i < length; i++)
            {
                if (value.Equals(slice[i]))
                    return i;
            }
            return -1;
        }

        // TODO (pri 2): this needs to be optimized; Ati is doing the work
        public static int IndexOf(this ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> values)
        {
            int slicedSoFar = 0;
            while (true)
            {
                ReadOnlySpan<byte> candidate = buffer.Slice(slicedSoFar);
                var index = candidate.IndexOf(values[0]);
                if (index != -1)
                {
                    candidate = candidate.Slice(index);
                    if (candidate.Length < values.Length) return -1;
                    candidate = candidate.Slice(0, values.Length);
                    if (candidate.SequenceEqual(values))
                    {
                        return index + slicedSoFar;
                    }
                    slicedSoFar += index + 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this Span<byte> buffer, ReadOnlySpan<byte> values)
        {
            return IndexOf((ReadOnlySpan<byte>)buffer, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlyMemory<byte> memory, ReadOnlySpan<byte> values)
        {
            return IndexOf(memory.Span, values);
        }
    }
}

