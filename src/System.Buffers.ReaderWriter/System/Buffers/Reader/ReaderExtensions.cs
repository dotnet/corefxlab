﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public static partial class ReaderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ReadOnlySpan<T> ToSpan<T>(this in ReadOnlySequence<T> sequence, Allocator<T> allocator)
        {
            if (sequence.IsSingleSegment)
                return sequence.First.Span;

            int length = checked((int)sequence.Length);
            Span<T> destination = allocator(length);
            sequence.CopyTo(destination);
            return destination;
        }
    }
}