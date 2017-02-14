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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlyMemory<byte> memory, ReadOnlySpan<byte> values)
        {
            return SpanExtensions.IndexOf(memory.Span, values);
        }
    }
}

