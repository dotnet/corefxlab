// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

// Open question: should this type have APIs which manufacture System.Utf8String instances,
// e.g., PadLeft(int), Replace(Utf8Char, Utf8Char), etc.? How about APIs which
// allocate in general, e.g., Split(Utf8Char)?

namespace System.Text
{
    /// <summary>
    /// A segment of a <see cref="Utf8String"/>.
    /// </summary>
    public readonly struct Utf8StringSegment : IComparable<Utf8StringSegment>, IEnumerable<Utf8Char>, IEquatable<Utf8StringSegment>
    {
        public Utf8StringSegment(Utf8String value) => throw null;

        public Utf8StringSegment(Utf8String value, int startIndex) => throw null;

        public Utf8StringSegment(Utf8String value, int startIndex, int length) => throw null;

        public static bool operator ==(Utf8StringSegment a, Utf8StringSegment b) => throw null;

        public static bool operator !=(Utf8StringSegment a, Utf8StringSegment b) => throw null;

        public static implicit operator ReadOnlyMemory<Utf8Char>(Utf8StringSegment value) => throw null;

        public static implicit operator ReadOnlySpan<Utf8Char>(Utf8StringSegment value) => throw null;

        public static implicit operator Utf8StringSegment(Utf8String value) => throw null;

        public Utf8Char this[int index] => throw null;

        public static Utf8StringSegment Empty => default;

        public int Length => throw null;

        public ReadOnlyMemory<Utf8Char> AsMemory() => throw null;

        public ReadOnlySpan<Utf8Char> AsSpan() => throw null;

        public int CompareTo(Utf8StringSegment other) => throw null;

        public bool Contains(UnicodeScalar value) => throw null;

        public bool Contains(Utf8Char value) => throw null;

        public bool Contains(Utf8String value) => throw null;

        public bool Contains(Utf8StringSegment value) => throw null;

        public void CopyTo(Span<Utf8Char> destination) => throw null;

        public bool EndsWith(UnicodeScalar value) => throw null;

        public bool EndsWith(Utf8Char value) => throw null;

        public bool EndsWith(Utf8String value) => throw null;

        public bool EndsWith(Utf8StringSegment value) => throw null;

        public override bool Equals(object value) => throw null;

        public bool Equals(Utf8String value) => throw null;

        public bool Equals(Utf8StringSegment value) => throw null;

        public static bool Equals(Utf8StringSegment a, Utf8StringSegment b) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public override int GetHashCode() => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void GetUtf8String(out Utf8String value, out int startIndex, out int length) => throw null;

        public int IndexOf(UnicodeScalar value) => throw null;

        public int IndexOf(Utf8Char value) => throw null;

        public int IndexOf(Utf8String value) => throw null;

        public int IndexOf(Utf8StringSegment value) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public static bool IsEmpty(Utf8StringSegment value) => throw null;

        public static bool IsEmptyOrWhiteSpace(Utf8StringSegment value) => throw null;

        public int LastIndexOf(UnicodeScalar value) => throw null;

        public int LastIndexOf(Utf8Char value) => throw null;

        public int LastIndexOf(Utf8String value) => throw null;

        public int LastIndexOf(Utf8StringSegment value) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool StartsWith(UnicodeScalar value) => throw null;

        public bool StartsWith(Utf8Char value) => throw null;

        public bool StartsWith(Utf8String value) => throw null;

        public bool StartsWith(Utf8StringSegment value) => throw null;

        public Utf8StringSegment Substring(int startIndex) => throw null;

        public Utf8StringSegment Substring(int startIndex, int length) => throw null;

        public override string ToString() => throw null;

        public Utf8Char[] ToUtf8CharArray() => throw null;

        public Utf8String ToUtf8String() => throw null;

        public Utf8StringSegment Trim() => throw null;

        public Utf8StringSegment Trim(Utf8Char trimChar) => throw null;

        public Utf8StringSegment Trim(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        public Utf8StringSegment TrimEnd() => throw null;

        public Utf8StringSegment TrimEnd(Utf8Char trimChar) => throw null;

        public Utf8StringSegment TrimEnd(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        public Utf8StringSegment TrimStart() => throw null;

        public Utf8StringSegment TrimStart(Utf8Char trimChar) => throw null;

        public Utf8StringSegment TrimStart(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        public static bool TryCreateFromMemory(ReadOnlyMemory<Utf8Char> memory, out Utf8StringSegment segment) => throw null;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<Utf8Char> IEnumerable<Utf8Char>.GetEnumerator() => GetEnumerator();

        public readonly struct Enumerator : IEnumerator<Utf8Char>
        {
            public Utf8Char Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw null;
        }
    }
}
