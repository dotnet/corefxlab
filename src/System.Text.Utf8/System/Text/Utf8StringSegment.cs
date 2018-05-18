// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Open question: should this type have APIs which manufacture System.Utf8String instances,
// e.g., PadLeft(int), Replace(Utf8Char, Utf8Char), etc.? How about APIs which
// allocate in general, e.g., Split(Utf8Char)?

namespace System.Text
{
    /// <summary>
    /// A segment of a <see cref="Utf8String"/>.
    /// </summary>
    public readonly struct Utf8StringSegment : IEquatable<Utf8StringSegment>
    {
        private readonly Utf8String _value;
        private readonly int _offset;
        private readonly int _count;

        public Utf8StringSegment(Utf8String value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _value = value;
            _offset = 0;
            _count = value.Length;
        }

        public Utf8StringSegment(Utf8String value, int startIndex)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if ((uint)startIndex > (uint)value.Length)
            {
                // TODO: Real error message
                throw new ArgumentOutOfRangeException(paramName: nameof(startIndex));
            }

            _value = value;
            _offset = startIndex;
            _count = value.Length - startIndex;
        }

        public Utf8StringSegment(Utf8String value, int startIndex, int length)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if ((uint)startIndex > (uint)value.Length)
            {
                // TODO: Real error message
                throw new ArgumentOutOfRangeException(paramName: nameof(startIndex));
            }

            if ((uint)length > (uint)(value.Length - startIndex))
            {
                // TODO: Real error message
                throw new ArgumentOutOfRangeException(paramName: nameof(length));
            }

            _value = value;
            _offset = startIndex;
            _count = length;
        }

        // non-validating ctor for internal use
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Utf8StringSegment(Utf8String value, int startIndex, int length, object unused)
        {
            _value = value;
            _offset = startIndex;
            _count = length;
        }

        public static bool operator ==(Utf8StringSegment a, Utf8StringSegment b)
        {
            // Ordinal equality check.

            if (IsEmpty(a))
            {
                return IsEmpty(b);
            }
            else if (IsEmpty(b))
            {
                return false;
            }
            else
            {
                // Both 'a' and 'b' are non-empty, so dereferencing shouldn't throw unless we've been torn.
                return a._value.Bytes.Slice(a._offset, a._count).SequenceEqual(b._value.Bytes.Slice(b._offset, b._count));
            }
        }

        public static bool operator !=(Utf8StringSegment a, Utf8StringSegment b) => !(a == b);

        public static implicit operator ReadOnlyMemory<Utf8Char>(Utf8StringSegment value) => throw null;

        public static implicit operator ReadOnlySpan<Utf8Char>(Utf8StringSegment value) => throw null;

        public static implicit operator Utf8StringSegment(Utf8String value) => throw null;

        public static Utf8StringSegment Empty => default;

        public int Length => _count;

        public ScalarSequence Scalars => throw null;

        public ReadOnlyMemory<Utf8Char> AsMemory() => throw null;

        public ReadOnlySpan<Utf8Char> AsSpan() => throw null;

        public int CompareTo(Utf8StringSegment other) => throw null;

        public bool Contains(UnicodeScalar value) => throw null;

        public bool Contains(Utf8Char value) => throw null;

        public bool Contains(Utf8String value) => throw null;

        public bool Contains(Utf8StringSegment value) => throw null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Utf8StringSegment CreateWithoutValidation(Utf8String value, int startIndex, int length)
        {
            if (value is null)
            {
                Debug.Assert(startIndex == 0);
                Debug.Assert(length == 0);
            }
            else
            {
                Debug.Assert((uint)startIndex <= (uint)value.Length);
                Debug.Assert((uint)length <= (uint)(value.Length - startIndex));
            }

            return new Utf8StringSegment(value, startIndex, length, null /* unused */);
        }

        public void CopyTo(Span<Utf8Char> destination) => throw null;

        public bool EndsWith(UnicodeScalar value) => throw null;

        public bool EndsWith(Utf8Char value) => throw null;

        public bool EndsWith(Utf8String value) => throw null;

        public bool EndsWith(Utf8StringSegment value) => throw null;

        public override bool Equals(object value) => (value is Utf8StringSegment other) && (this == other);

        public bool Equals(Utf8String value) => throw null;

        public bool Equals(Utf8StringSegment value) => (this == value);

        public static bool Equals(Utf8StringSegment a, Utf8StringSegment b) => (a == b);

        public Enumerator GetEnumerator() => throw null;

        public override int GetHashCode() => Marvin.ComputeHash32((_count != 0) ? _value.Bytes.Slice(_offset, _count) : ReadOnlySpan<byte>.Empty, Marvin.Utf8StringSeed);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetUtf8String(out Utf8String value, out int startIndex, out int length)
        {
            value = _value;
            startIndex = _offset;
            length = _count;
        }

        public int IndexOf(UnicodeScalar value) => throw null;

        public int IndexOf(Utf8Char value) => throw null;

        public int IndexOf(Utf8String value) => throw null;

        public int IndexOf(Utf8StringSegment value) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public static bool IsEmpty(Utf8StringSegment value) => (value._count == 0);

        public static bool IsEmptyOrWhiteSpace(Utf8StringSegment value) => (value._value == null) || (Utf8TrimHelpers.TrimWhiteSpace(value._value.Bytes.Slice(value._offset, value._count), TrimType.Both).Length == 0);

        public int LastIndexOf(UnicodeScalar value) => throw null;

        public int LastIndexOf(Utf8Char value) => throw null;

        public int LastIndexOf(Utf8String value) => throw null;

        public int LastIndexOf(Utf8StringSegment value) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool StartsWith(UnicodeScalar value) => throw null;

        public bool StartsWith(Utf8Char value) => throw null;

        public bool StartsWith(Utf8String value) => throw null;

        public bool StartsWith(Utf8StringSegment value) => throw null;

        public Utf8StringSegment Substring(int startIndex)
        {
            if ((uint)startIndex >= _count)
            {
                if (startIndex == _count)
                {
                    return Empty;
                }
                else
                {
                    // TODO: Real exception message
                    throw new ArgumentOutOfRangeException(paramName: nameof(startIndex));
                }
            }

            // TODO: Should this validate that we're not substringing in the middle of a multi-byte sequence?
            // Utf8String.Substring will perform this check, and we may want to be consistent.

            return CreateWithoutValidation(_value, _offset + startIndex, _count - startIndex);
        }

        public Utf8StringSegment Substring(int startIndex, int length)
        {
            if ((uint)startIndex > _count)
            {
                // TODO: Real exception message
                throw new ArgumentOutOfRangeException(paramName: nameof(startIndex));
            }

            if ((uint)length > (uint)(_count - _offset))
            {
                // TODO: Real exception message
                throw new ArgumentOutOfRangeException(paramName: nameof(length));
            }

            if (length != 0)
            {
                // TODO: Should this validate that we're not substringing in the middle of a multi-byte sequence?
                // Utf8String.Substring will perform this check, and we may want to be consistent.

                return CreateWithoutValidation(_value, _offset + startIndex, length);
            }
            else
            {
                return Empty;
            }
        }

        public override string ToString() => throw null;

        public Utf8Char[] ToUtf8CharArray() => throw null;

        public Utf8String ToUtf8String() => (_value != null) ? _value.Substring(_offset, _count) : Utf8String.Empty;

        public Utf8StringSegment Trim() => TrimInternal(TrimType.Both);

        public Utf8StringSegment TrimEnd() => TrimInternal(TrimType.End);

        private Utf8StringSegment TrimInternal(TrimType trimType)
        {
            if (_count == 0)
            {
                return this; // nothing to trim
            }

            ReadOnlySpan<byte> untrimmed = _value.Bytes;
            ReadOnlySpan<byte> trimmed = Utf8TrimHelpers.TrimWhiteSpace(untrimmed.Slice(_offset, _count), trimType);

            if (trimmed.Length != 0)
            {
                // Use the span's memory address offset to determine how much data was trimmed from the front.
                return CreateWithoutValidation(
                    value: _value,
                    startIndex: _offset + (int)(nuint)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(untrimmed), ref MemoryMarshal.GetReference(trimmed)),
                    length: trimmed.Length);
            }

            return Empty;
        }

        public Utf8StringSegment TrimStart() => TrimInternal(TrimType.Start);

        public static bool TryCreateFromMemory(ReadOnlyMemory<Utf8Char> memory, out Utf8StringSegment segment) => throw null;

        public readonly struct Enumerator : IEnumerator<Utf8Char>
        {
            public Utf8Char Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw null;
        }

        public struct ScalarEnumerator : IEnumerator<UnicodeScalar>
        {
            public UnicodeScalar Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw new NotImplementedException();
        }

        public readonly struct ScalarSequence : IEnumerable<UnicodeScalar>
        {
            public ScalarEnumerator GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            IEnumerator<UnicodeScalar> IEnumerable<UnicodeScalar>.GetEnumerator() => throw null;
        }
    }
}
