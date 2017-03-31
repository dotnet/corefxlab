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
        public static void CopyTo<T>(this T[] array, Span<T> span)
        {
            array.AsSpan().CopyTo(span);
        }

        /// <summary>
        /// Creates a new readonly span over the portion of the target string.
        /// </summary>
        /// <param name="text">The target string.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ReadOnlySpan<char> AsSpanTemp(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            int textLength = text.Length;

            if (textLength == 0) return ReadOnlySpan<char>.Empty;

            fixed (char* charPointer = text)
            {
                return ReadOnlySpan<char>.DangerousCreate(text, ref *charPointer, textLength);
            }
        }
    }
}

