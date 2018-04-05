// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

// Open question: should this type have APIs which manufacture System.String instances,
// e.g., Normalize(), PadLeft(int), Replace(char, char), etc.? How about APIs which
// allocate in general, e.g., Split(char)?

namespace System.Text
{
    /// <summary>
    /// A segment of a <see cref="string"/>.
    /// </summary>
    public readonly struct StringSegment : IComparable<StringSegment>, IEnumerable<char>, IEquatable<StringSegment>
    {
        public StringSegment(string value) => throw null;

        public StringSegment(string value, int startIndex) => throw null;

        public StringSegment(string value, int startIndex, int length) => throw null;

        public static bool operator ==(StringSegment a, StringSegment b) => throw null;

        public static bool operator !=(StringSegment a, StringSegment b) => throw null;

        public static implicit operator ReadOnlyMemory<char>(StringSegment value) => throw null;

        public static implicit operator ReadOnlySpan<char>(StringSegment value) => throw null;

        public static implicit operator StringSegment(string value) => throw null;

        public char this[int index] => throw null;

        public static StringSegment Empty => default;

        public int Length => throw null;

        public ReadOnlyMemory<char> AsMemory() => throw null;

        public ReadOnlySpan<char> AsSpan() => throw null;

        public static int Compare(StringSegment segmentA, StringSegment segmentB) => throw null;

        public static int Compare(StringSegment segmentA, StringSegment segmentB, bool ignoreCase) => throw null;

        public static int Compare(StringSegment segmentA, StringSegment segmentB, bool ignoreCase, CultureInfo culture) => throw null;

        public static int Compare(StringSegment segmentA, StringSegment segmentB, CultureInfo culture, CompareOptions options) => throw null;

        public static int Compare(StringSegment segmentA, StringSegment segmentB, StringComparison comparisonType) => throw null;

        public static int CompareOrdinal(StringSegment segmentA, StringSegment segmentB) => throw null;

        public int CompareTo(StringSegment other) => throw null;

        public bool Contains(char value) => throw null;

        public bool Contains(char value, StringComparison comparisonType) => throw null;

        public bool Contains(string value) => throw null;

        public bool Contains(string value, StringComparison comparisonType) => throw null;

        public bool Contains(StringSegment value) => throw null;

        public bool Contains(StringSegment value, StringComparison comparisonType) => throw null;

        public void CopyTo(Span<char> destination) => throw null;

        public bool EndsWith(char value) => throw null;

        public bool EndsWith(string value) => throw null;

        public bool EndsWith(string value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool EndsWith(string value, StringComparison comparisonType) => throw null;

        public bool EndsWith(StringSegment value) => throw null;

        public bool EndsWith(StringSegment value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool EndsWith(StringSegment value, StringComparison comparisonType) => throw null;

        public override bool Equals(object value) => throw null;

        public bool Equals(string value) => throw null;

        public bool Equals(string value, StringComparison comparisonType) => throw null;

        public bool Equals(StringSegment value) => throw null;

        public bool Equals(StringSegment value, StringComparison comparisonType) => throw null;

        public static bool Equals(StringSegment a, StringSegment b) => throw null;

        public static bool Equals(StringSegment a, StringSegment b, StringComparison comparisonType) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public override int GetHashCode() => throw null;

        public int GetHashCode(StringComparison comparisonType) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void GetString(out string value, out int startIndex, out int length) => throw null;

        public int IndexOf(char value) => throw null;

        public int IndexOf(char value, StringComparison comparisonType) => throw null;

        public int IndexOf(string value) => throw null;

        public int IndexOf(string value, StringComparison comparisonType) => throw null;

        public int IndexOf(StringSegment value) => throw null;

        public int IndexOf(StringSegment value, StringComparison comparisonType) => throw null;

        public int IndexOfAny(ReadOnlySpan<char> anyOf) => throw null;

        public static bool IsEmpty(StringSegment value) => throw null;

        public static bool IsEmptyOrWhiteSpace(StringSegment value) => throw null;

        public bool IsNormalized() => throw null;

        public bool IsNormalized(NormalizationForm normalizationForm) => throw null;

        public int LastIndexOf(char value) => throw null;

        public int LastIndexOf(string value) => throw null;

        public int LastIndexOf(string value, StringComparison comparisonType) => throw null;

        public int LastIndexOf(StringSegment value) => throw null;

        public int LastIndexOf(StringSegment value, StringComparison comparisonType) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<char> anyOf) => throw null;

        public bool StartsWith(char value) => throw null;

        public bool StartsWith(string value) => throw null;

        public bool StartsWith(string value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool StartsWith(string value, StringComparison comparisonType) => throw null;

        public bool StartsWith(StringSegment value) => throw null;

        public bool StartsWith(StringSegment value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool StartsWith(StringSegment value, StringComparison comparisonType) => throw null;

        public StringSegment Substring(int startIndex) => throw null;

        public StringSegment Substring(int startIndex, int length) => throw null;

        public char[] ToCharArray() => throw null;

        public override string ToString() => throw null;

        public StringSegment Trim() => throw null;

        public StringSegment Trim(char trimChar) => throw null;

        public StringSegment Trim(ReadOnlySpan<char> trimChars) => throw null;

        public StringSegment TrimEnd() => throw null;

        public StringSegment TrimEnd(char trimChar) => throw null;

        public StringSegment TrimEnd(ReadOnlySpan<char> trimChars) => throw null;

        public StringSegment TrimStart() => throw null;

        public StringSegment TrimStart(char trimChar) => throw null;

        public StringSegment TrimStart(ReadOnlySpan<char> trimChars) => throw null;

        public static bool TryCreateFromMemory(ReadOnlyMemory<char> memory, out StringSegment segment) => throw null;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();

        public readonly struct Enumerator : IEnumerator<char>
        {
            public char Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw null;
        }
    }
}
