// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;

namespace System.Text
{
    public static class Utf8Extensions
    {
        // TODO: These should really be extension properties, not extension methods.

        /// <summary>
        /// Allows enumerating individual Unicode scalars of a <see cref="ReadOnlySpan{Utf8Char}"/>.
        /// </summary>
        public static Utf8CharSpanUnicodeScalarEnumerable GetScalars(this ReadOnlySpan<Utf8Char> value) => throw null;

        /// <summary>
        /// Allows enumerating the text elements of a <see cref="Utf8String"/>.
        /// Individual text elements are represented by <see cref="Utf8TextElement"/>.
        /// </summary>
        public static Utf8StringTextElementEnumerable GetTextElements(this Utf8String value) => throw null;

        /// <summary>
        /// Allows enumerating the text elements of a <see cref="ReadOnlySpan{Utf8Char}"/>.
        /// Individual text elements are represented by <see cref="ReadOnlySpan{Utf8Char}"/>.
        /// </summary>
        public static Utf8CharSpanTextElementEnumerable GetTextElements(this ReadOnlySpan<Utf8Char> value) => throw null;

        public ref struct Utf8CharSpanTextElementEnumerable
        {
            public Utf8SpanTextElementEnumerator GetEnumerator() => throw null;
        }

        public struct Utf8StringTextElementEnumerable : IEnumerable<Utf8TextElement>
        {
            public Utf8TextElementEnumerator GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            IEnumerator<Utf8TextElement> IEnumerable<Utf8TextElement>.GetEnumerator() => throw null;
        }

        public ref struct Utf8CharSpanUnicodeScalarEnumerable
        {
            public Utf8SpanUnicodeScalarEnumerator GetEnumerator() => throw null;
        }
    }
}
