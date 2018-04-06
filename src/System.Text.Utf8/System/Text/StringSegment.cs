// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Open question: should this type have APIs which manufacture System.String instances;
// e.g., Normalize(), PadLeft(int), Replace(char, char), etc.? How about APIs which
// allocate in general; e.g., Split(char), which might return a StringSegment[]?

// This type solves some of the shortcomings of ReadOnlyMemory<char> and ReadOnlySpan<char>.
//
// - Unlike ROM<char>, callers don't have to worry about lifetime / lease semantics. There's no
//   fear that the underlying memory will be ripped out from under them if they hold on to the
//   instance.
//
// - Unlike ROS<char>, this type is heapable, so it can exist as a field in a normal (non-ref) type.
//
// - Unlike ROM<char>, this type directly implements much of the same API surface as System.String,
//   so callers can use it directly rather than converting to Span over and over.
//
// - Unlike ROM<char> and ROS<char>, this type implements GetHashCode, Equals, and similar methods,
//   so it's suitable for use in dictionaries or other containers.
//
// - Unlike ROM<char> and ROS<char>, you're _guaranteed_ to be able to get a System.String instance
//   back out of this, so you can call existing APIs with shape (string, int offset, int length)
//   without allocating or copying the string data.
//
// Open question: are these benefits worth introducing this type? We'd need to provide guidance on
// when to use String, StringSegment, ReadOnlyMemory<char>, and ReadOnlySpan<char>.
//
// Preliminary guidance:
//
// - If you're a synchronous API author, take ReadOnlySpan<char> in your leaf APIs.
// - If you're an asynchronous API author, take ReadOnlyMemory<char> in your leaf APIs.
// - If you're an API author who needs to return standalone data not tied to any
//   other lifetime (e.g., you're a deserializer), return String or StringSegment.
// - If you're an application developer, use String and StringSegment as fields in
//   your classes unless you know you need to potentially deal with unmanaged memory.

namespace System.Text
{
    /// <summary>
    /// A segment of a <see cref="string"/>.
    /// </summary>
    public readonly struct StringSegment : IComparable<StringSegment>, IEnumerable<char>, IEquatable<StringSegment>
    {
        private readonly int _length;
        private readonly int _startIndex;
        private readonly string _value;

        public StringSegment(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _value = value;
            _startIndex = 0;
            _length = value.Length;
        }

        public StringSegment(string value, int startIndex)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if ((uint)startIndex > (uint)value.Length)
            {
                // TODO: Actual exception message.
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            _value = value;
            _startIndex = startIndex;
            _length = value.Length - startIndex;
        }

        public StringSegment(string value, int startIndex, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if ((uint)startIndex > (uint)value.Length)
            {
                // TODO: Actual exception message.
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if ((uint)length > (uint)(value.Length - startIndex))
            {
                // TODO: Actual exception message.
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _value = value;
            _startIndex = startIndex;
            _length = length;
        }

        // non-validating ctor for internal use, allows null inputs
        private StringSegment(string value, bool ignored)
        {
            _value = value;
            _startIndex = 0;
            _length = (value != null) ? value.Length : 0;
        }

        // non-validating ctor for internal use, allows null inputs
        private StringSegment(string value, int startIndex, int length, bool ignored)
        {
            _value = value;
            _startIndex = startIndex;
            _length = length;
        }

        public static bool operator ==(StringSegment a, StringSegment b) => a.AsSpan().SequenceEqual(b.AsSpan());

        public static bool operator !=(StringSegment a, StringSegment b) => !(a == b);

        public static implicit operator ReadOnlyMemory<char>(StringSegment value) => value.AsMemory();

        public static implicit operator ReadOnlySpan<char>(StringSegment value) => value.AsSpan();

        public static implicit operator StringSegment(string value) => new StringSegment(value, false /* ignored */);

        public ref readonly char this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_length)
                {
                    // TODO: Real exception message.
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                // TODO: Don't go through AsSpan() below; return the computed offset directly as a single 'lea'.
                return ref Unsafe.Add(ref MemoryMarshal.GetReference(AsSpan()), index);
            }
        }

        public static StringSegment Empty => default;

        public bool IsEmpty => (_length == 0);

        public int Length => _length;

        public ReadOnlyMemory<char> AsMemory()
        {
            // TODO: Make the below call non-validating.

            return _value.AsMemory(_startIndex, _length);
        }

        public ReadOnlySpan<char> AsSpan()
        {
            // TODO: If we knew the exact offset of String._firstChar, we could immediately use
            // return new ReadOnlySpan<char>(ref [str + offset_to_firstchar + startIndex], length),
            // and this would work regardless of whether string was null or not. (The pointer would
            // be bogus if string is null, but who cares.) Since this is the workhorse routine used
            // by tons of other methods on this type, we should optimize this as much as possible.

            return ((ReadOnlySpan<char>)_value).Slice(_startIndex, _length);
        }

        public static int Compare(StringSegment segmentA, StringSegment segmentB)
            => CompareNoValidation(segmentA, segmentB, CultureInfo.CurrentCulture, CompareOptions.None);

        public static int Compare(StringSegment segmentA, StringSegment segmentB, bool ignoreCase)
            => CompareNoValidation(segmentA, segmentB, CultureInfo.CurrentCulture, (ignoreCase) ? CompareOptions.IgnoreCase : CompareOptions.None);

        public static int Compare(StringSegment segmentA, StringSegment segmentB, bool ignoreCase, CultureInfo culture)
            => Compare(segmentA, segmentB, culture, (ignoreCase) ? CompareOptions.IgnoreCase : CompareOptions.None);

        public static int Compare(StringSegment segmentA, StringSegment segmentB, CultureInfo culture, CompareOptions options)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            return CompareNoValidation(segmentA, segmentB, culture, options);
        }

        public static int Compare(StringSegment segmentA, StringSegment segmentB, StringComparison comparisonType)
            => segmentA.AsSpan().CompareTo(segmentB.AsSpan(), comparisonType);

        private static int CompareNoValidation(StringSegment segmentA, StringSegment segmentB, CultureInfo culture, CompareOptions options)
            => culture.CompareInfo.Compare(segmentA._value, segmentA._startIndex, segmentA._length, segmentB._value, segmentB._startIndex, segmentB._length, options);

        public static int CompareOrdinal(StringSegment segmentA, StringSegment segmentB) => segmentA.AsSpan().CompareTo(segmentB.AsSpan(), StringComparison.Ordinal);

        public int CompareTo(StringSegment other) => CompareNoValidation(this, other, CultureInfo.CurrentCulture, CompareOptions.None);

        public bool Contains(char value)
        {
            // TODO: Replace IndexOf below with Contains when it's made public, which will give better performance.
            return AsSpan().IndexOf(value) >= 0;
        }

        public bool Contains(char value, StringComparison comparisonType) => throw null;

        public bool Contains(string value) => throw null;

        public bool Contains(string value, StringComparison comparisonType) => throw null;

        public bool Contains(StringSegment value) => throw null;

        public bool Contains(StringSegment value, StringComparison comparisonType) => throw null;

        public void CopyTo(Span<char> destination) => AsSpan().CopyTo(destination);

        public bool EndsWith(char value)
        {
            // TODO: Improve line below to skip validation (_value null check on indexer access, out-of-bounds check on indexer).
            return (_length > 0) && (_value[_startIndex + _length - 1] == value);
        }

        public bool EndsWith(string value) => throw null;

        public bool EndsWith(string value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool EndsWith(string value, StringComparison comparisonType) => throw null;

        public bool EndsWith(StringSegment value) => throw null;

        public bool EndsWith(StringSegment value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool EndsWith(StringSegment value, StringComparison comparisonType) => throw null;

        public override bool Equals(object value)
        {
            if (value is StringSegment asStringSegment)
            {
                return Equals(asStringSegment);
            }
            else if (value is string asString)
            {
                return Equals(asString);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(string value) => AsSpan().SequenceEqual(value);

        public bool Equals(string value, StringComparison comparisonType) => AsSpan().Equals((ReadOnlySpan<char>)value, comparisonType);

        public bool Equals(StringSegment value) => AsSpan().SequenceEqual(value);

        public bool Equals(StringSegment value, StringComparison comparisonType) => AsSpan().Equals((ReadOnlySpan<char>)value, comparisonType);

        public static bool Equals(StringSegment a, StringSegment b) => a.AsSpan().SequenceEqual(b.AsSpan());

        public static bool Equals(StringSegment a, StringSegment b, StringComparison comparisonType) => a.AsSpan().Equals(b.AsSpan(), comparisonType);

        public Enumerator GetEnumerator() => new Enumerator(this);

        public override int GetHashCode()
        {
            // TODO: Where's Marvin32?
            throw null;
        }

        public int GetHashCode(StringComparison comparisonType) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetString(out string value, out int startIndex, out int length)
        {
            value = (_length == 0) ? default : _value; // normalize empty -> null
            startIndex = _startIndex;
            length = _length;
        }

        public int IndexOf(char value) => AsSpan().IndexOf(value);

        public int IndexOf(char value, StringComparison comparisonType) => throw null;

        public int IndexOf(string value) => IndexOf(value, StringComparison.CurrentCulture);

        public int IndexOf(string value, StringComparison comparisonType) => AsSpan().IndexOf(value, comparisonType);

        public int IndexOf(StringSegment value) => IndexOf(value, StringComparison.CurrentCulture);

        public int IndexOf(StringSegment value, StringComparison comparisonType) => AsSpan().IndexOf(value, comparisonType);

        public int IndexOfAny(ReadOnlySpan<char> anyOf) => AsSpan().IndexOfAny(anyOf);

        public bool IsEmptyOrWhiteSpace()
        {
            // TODO: Perf improvements to the below code.
            foreach (char ch in AsSpan())
            {
                if (!Char.IsWhiteSpace(ch)) { return false; }
            }

            return true; // no non-whitespace character found
        }

        public bool IsNormalized() => throw null;

        public bool IsNormalized(NormalizationForm normalizationForm) => throw null;

        public int LastIndexOf(char value) => AsSpan().LastIndexOf(value);

        public int LastIndexOf(string value) => LastIndexOf(value, StringComparison.CurrentCulture);

        public int LastIndexOf(string value, StringComparison comparisonType) => throw null;

        public int LastIndexOf(StringSegment value) => LastIndexOf(value, StringComparison.CurrentCulture);

        public int LastIndexOf(StringSegment value, StringComparison comparisonType) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<char> anyOf) => AsSpan().LastIndexOfAny(anyOf);

        public bool StartsWith(char value)
        {
            // TODO: Improve line below to skip validation (_value null check on indexer access, out-of-bounds check on indexer).
            return (_length > 0) && (_value[_startIndex] == value);
        }

        public bool StartsWith(string value) => throw null;

        public bool StartsWith(string value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool StartsWith(string value, StringComparison comparisonType) => throw null;

        public bool StartsWith(StringSegment value) => throw null;

        public bool StartsWith(StringSegment value, bool ignoreCase, CultureInfo culture) => throw null;

        public bool StartsWith(StringSegment value, StringComparison comparisonType) => throw null;

        public StringSegment Substring(int startIndex)
        {
            if ((uint)startIndex > (uint)_length)
            {
                // TODO: Real exception message.
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            return new StringSegment(_value, _startIndex + startIndex, _length - startIndex, false /* ignored */); // non-validating
        }

        public StringSegment Substring(int startIndex, int length)
        {
            if ((uint)startIndex > (uint)_length)
            {
                // TODO: Real exception message.
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if ((uint)length > (uint)(_length - startIndex))
            {
                // TODO: Real exception message.
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return new StringSegment(_value, _startIndex + startIndex, length, false /* ignored */); // non-validating
        }

        public char[] ToCharArray() => AsSpan().ToArray();

        public override string ToString()
        {
            if (_length == 0)
            {
                // default(StringSegment)
                return String.Empty;
            }
            else if (_startIndex == 0 && _length == _value.Length)
            {
                // StringSegment backed by a full (non-sliced) string
                return _value;
            }
            else
            {
                // In an ideal world we could mutate this instance on-the-fly (even though
                // it's marked as a readonly struct) by clever use of unsafe code in order
                // to cache the generated string. However, this is dangerous because this
                // instance may be accessed by multiple threads concurrently, and mutating
                // this instance could result in a torn struct, which could lead to runtime
                // corruption.

                // TODO: Improve ReadOnlySpan<char>.ToString() to be non-pinning.
                return AsSpan().ToString();
            }
        }

        public StringSegment Trim() => TrimStart().TrimEnd();

        public StringSegment Trim(char trimChar) => TrimStart(trimChar).TrimEnd(trimChar);

        public StringSegment Trim(ReadOnlySpan<char> trimChars) => throw null;

        public StringSegment TrimEnd()
        {
            // TODO: Can we skip indexer bounds checks in the below code?
            // TODO: Is there a faster check than Char.IsWhiteSpace?

            for (int i = Length - 1; i >= 0; i--)
            {
                if (!Char.IsWhiteSpace(this[i]))
                {
                    return new StringSegment(_value, _startIndex, i + 1, false /* ignored */); // no validation
                }
            }

            return Empty;
        }

        public StringSegment TrimEnd(char trimChar)
        {
            // TODO: Can we skip indexer bounds checks in the below code?

            for (int i = Length - 1; i >= 0; i--)
            {
                if (this[i] != trimChar)
                {
                    return new StringSegment(_value, _startIndex, i + 1, false /* ignored */); // no validation
                }
            }

            return Empty;
        }

        public StringSegment TrimEnd(ReadOnlySpan<char> trimChars) => throw null;

        public StringSegment TrimStart()
        {
            // TODO: Can we skip indexer bounds checks in the below code?
            // TODO: Is there a faster check than Char.IsWhiteSpace?

            for (int i = 0; i < Length; i++)
            {
                if (!Char.IsWhiteSpace(this[i]))
                {
                    return new StringSegment(_value, _startIndex + i, _length - i, false /* ignored */); // no validation
                }
            }

            return Empty;
        }

        public StringSegment TrimStart(char trimChar)
        {
            // TODO: Can we skip indexer bounds checks in the below code?

            for (int i = 0; i < Length; i++)
            {
                if (this[i] != trimChar)
                {
                    return new StringSegment(_value, _startIndex + i, _length - i, false /* ignored */); // no validation
                }
            }

            return Empty;
        }

        public StringSegment TrimStart(ReadOnlySpan<char> trimChars) => throw null;

        public static bool TryCreateFromMemory(ReadOnlyMemory<char> memory, out StringSegment segment)
        {
            bool retVal = MemoryMarshal.TryGetString(memory, out var text, out var startIndex, out var length);
            segment = new StringSegment(text, startIndex, length, false /* ignored */); // non-validating
            return retVal;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<char>
        {
            private int _currentOffset;
            private readonly StringSegment _segment;

            internal Enumerator(StringSegment segment)
            {
                _segment = segment;
                _currentOffset = 0;
                Current = default;
            }

            public char Current { get; private set; }

            public void Dispose()
            {
                /* no-op */
            }

            public bool MoveNext()
            {
                if ((uint)_currentOffset >= (uint)_segment.Length)
                {
                    // went past the end of the segment
                    Current = default;
                    return false;
                }

                Current = _segment[_currentOffset++];
                return true;
            }

            public void Reset()
            {
                _currentOffset = 0;
                Current = default;
            }

            object IEnumerator.Current => Current;
        }
    }
}
