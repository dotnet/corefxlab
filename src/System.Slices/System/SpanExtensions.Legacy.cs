// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    public static partial class SpanExtensions
    {
        // @todo: Yanked from ReadOnlySpan in last design review. Just replace calls with "==".
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReferenceEquals<T>(this ReadOnlySpan<T> _this, ReadOnlySpan<T> other) => _this == other;

        // @todo: Yanked from Span in last design review. Just replace calls with "==".
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReferenceEquals<T>(this Span<T> _this, Span<T> other) => _this == other;

        // @todo: Yanked from Span in last design review. Just replace calls with CopyTo()
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<T>(this Span<T> span, ReadOnlySpan<T> array) => array.CopyTo(span);
    }
}
