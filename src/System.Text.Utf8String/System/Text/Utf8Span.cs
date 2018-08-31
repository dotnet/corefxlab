// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Primitives;

namespace System.Text.Utf8
{
    // TODO: should we validate that the bytes are valid UTF8 in constructors
    // TODO: Should we copy the array in Utf8Span ctors? This type is not-immutable. Utf8String is. 
    [DebuggerDisplay("{ToString()}u8")]
    public ref struct Utf8Span
    {
        private readonly ReadOnlySpan<byte> _buffer;

        private const int StringNotFound = -1;

        static Utf8Span s_empty => default;

        public Utf8Span(ReadOnlySpan<byte> utf8Bytes) => _buffer = utf8Bytes;

        // TODO: this is expensive. Do we want this ctor?
        public Utf8Span(string utf16String)
        {
            if (utf16String == null)
            {
                throw new ArgumentNullException(nameof(utf16String), "String cannot be null");
            }

            if (utf16String == string.Empty)
            {
                _buffer = ReadOnlySpan<byte>.Empty;
            }
            else
            {
                _buffer = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(utf16String));
            }
        }

        public Utf8Span(Utf8String utf8String) => _buffer = utf8String.Bytes;

        /// <summary>
        /// This constructor is for use by the compiler.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Utf8Span(RuntimeFieldHandle utf8Bytes, int length)
        {
            var buffer = new byte[length];
            RuntimeHelpers.InitializeArray(buffer, utf8Bytes);
            _buffer = buffer;
        }

        private Utf8Span(byte[] noCopyUtf8Bytes) => _buffer = noCopyUtf8Bytes;

        public static Utf8Span Empty => s_empty;

        public bool IsEmpty => Bytes.Length == 0;

        public Utf8CodePointEnumerator GetEnumerator() => new Utf8CodePointEnumerator(_buffer);

        public static implicit operator ReadOnlySpan<byte>(Utf8Span utf8Span) => utf8Span.Bytes;

        public static explicit operator Utf8Span(string utf16String) => new Utf8Span(utf16String);

        public static explicit operator string(Utf8Span utf8Span) => utf8Span.ToString();

        public ReadOnlySpan<byte> Bytes => _buffer;

        public override string ToString() => Encodings.Utf8.ToString(Bytes);

        public bool ReferenceEquals(Utf8Span other) => Bytes == other._buffer;

        public bool Equals(Utf8Span other) => Bytes.SequenceEqual(other._buffer);

        public bool Equals(Utf8String other) => Equals(other.Span);

        public bool Equals(string other)
        {
            Utf8CodePointEnumerator thisEnumerator = GetEnumerator();
            Debug.Assert(BitConverter.IsLittleEndian);
            Utf16LittleEndianCodePointEnumerator otherEnumerator = new Utf16LittleEndianCodePointEnumerator(other);

            while (true)
            {
                bool hasNext = thisEnumerator.MoveNext();
                if (hasNext != otherEnumerator.MoveNext())
                {
                    return false;
                }

                if (!hasNext)
                {
                    return true;
                }

                if (thisEnumerator.Current != otherEnumerator.Current)
                {
                    return false;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Utf8String)
            {
                return Equals((Utf8String)obj);
            }
            if (obj is string)
            {
                return Equals((string)obj);
            }

            // obj cannot be Utf8Span since it cannot be boxed 
            return false;
        }

        // TODO: write better hashing function
        public override int GetHashCode()
        {
            unchecked
            {
                if (Bytes.Length <= 4)
                {
                    int hash = Bytes.Length;
                    for (int i = 0; i < Bytes.Length; i++)
                    {
                        hash <<= 8;
                        hash ^= (byte)Bytes[i];
                    }
                    return hash;
                }
                else
                {
                    int hash = Bytes.Length;
                    hash ^= (byte)Bytes[0];
                    hash <<= 8;
                    hash ^= (byte)Bytes[1];
                    hash <<= 8;
                    hash ^= (byte)Bytes[Bytes.Length - 2];
                    hash <<= 8;
                    hash ^= (byte)Bytes[Bytes.Length - 1];
                    return hash;
                }
            }
        }

        public static bool operator ==(Utf8Span left, Utf8Span right) => left.Equals(right);

        public static bool operator !=(Utf8Span left, Utf8Span right) => !left.Equals(right);

        // TODO: do we like all these O(N) operators? 
        public static bool operator ==(Utf8Span left, string right) => left.Equals(right);

        public static bool operator !=(Utf8Span left, string right) => !left.Equals(right);

        public static bool operator ==(string left, Utf8Span right) => right.Equals(left);

        public static bool operator !=(string left, Utf8Span right) => !right.Equals(left);

        public int CompareTo(Utf8Span other) => SequenceCompareTo(Bytes, other.Bytes);

        public int CompareTo(Utf8String other) => CompareTo(other.Span);

        public int CompareTo(string other)
        {
            Utf8CodePointEnumerator thisEnumerator = GetEnumerator();
            Debug.Assert(BitConverter.IsLittleEndian);
            Utf16LittleEndianCodePointEnumerator otherEnumerator = new Utf16LittleEndianCodePointEnumerator(other);

            while (true)
            {
                var thisHasNext = thisEnumerator.MoveNext();
                var otherHasNext = otherEnumerator.MoveNext();
                if (!thisHasNext && !otherHasNext) return 0;
                if (!thisHasNext) return -1;
                if (!otherHasNext) return 1;

                var thisCurrent = thisEnumerator.Current;
                var otherCurrent = otherEnumerator.Current;

                if (thisCurrent == otherCurrent) continue;
                return thisCurrent.CompareTo(otherCurrent);
            }
        }

        public bool StartsWith(uint codePoint)
        {
            Utf8CodePointEnumerator e = GetEnumerator();
            if (!e.MoveNext())
            {
                return false;
            }

            return e.Current == codePoint;
        }

        public bool StartsWith(Utf8Span value) => Bytes.StartsWith(value.Bytes);

        public bool EndsWith(Utf8Span value)
        {
            if (Bytes.Length < value.Bytes.Length)
            {
                return false;
            }

            return this.Substring(Bytes.Length - value.Bytes.Length, value.Bytes.Length).Equals(value);
        }

        public bool EndsWith(uint codePoint)
        {
            var e = new Utf8CodePointReverseEnumerator(Bytes);
            if (!e.MoveNext()) return false;
            return e.Current == codePoint;
        }

        #region Slicing
        public bool TrySubstringFrom(Utf8Span value, out Utf8Span result)
        {
            int idx = IndexOf(value);

            if (idx == StringNotFound)
            {
                result = default;
                return false;
            }

            result = Substring(idx);
            return true;
        }

        public bool TrySubstringFrom(uint codePoint, out Utf8Span result)
        {
            int idx = IndexOf(codePoint);

            if (idx == StringNotFound)
            {
                result = default;
                return false;
            }

            result = Substring(idx);
            return true;
        }

        public bool TrySubstringTo(Utf8Span value, out Utf8Span result)
        {
            int idx = IndexOf(value);

            if (idx == StringNotFound)
            {
                result = default;
                return false;
            }

            result = Substring(0, idx);
            return true;
        }

        public bool TrySubstringTo(uint codePoint, out Utf8Span result)
        {
            int idx = IndexOf(codePoint);

            if (idx == StringNotFound)
            {
                result = default;
                return false;
            }

            result = Substring(0, idx);
            return true;
        }

        public Utf8Span TrimStart()
        {
            Utf8CodePointEnumerator it = GetEnumerator();
            while (it.MoveNext() && Unicode.IsWhitespace(it.Current))
            {
            }

            return Substring(it.PositionInCodeUnits);
        }

        public Utf8Span TrimStart(uint[] codePoints)
        {
            if (codePoints == null || codePoints.Length == 0) return TrimStart(); // Trim Whitespace

            Utf8CodePointEnumerator it = GetEnumerator();
            while (it.MoveNext())
            {
                if (Array.IndexOf(codePoints, it.Current) == -1)
                {
                    break;
                }
            }

            return Substring(it.PositionInCodeUnits);
        }

        public Utf8Span TrimStart(Utf8Span characters)
        {
            if (characters == Empty)
            {
                // Trim Whitespace
                return TrimStart();
            }

            Utf8CodePointEnumerator it = GetEnumerator();
            Utf8CodePointEnumerator itPrefix = characters.GetEnumerator();

            while (it.MoveNext())
            {
                bool found = false;
                // Iterate over prefix set
                while (itPrefix.MoveNext())
                {
                    if (it.Current == itPrefix.Current)
                    {
                        // Character found, don't check further
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // Reached the end, char was not found
                    break;
                }

                itPrefix.Reset();
            }

            return Substring(it.PositionInCodeUnits);
        }

        public Utf8Span TrimEnd()
        {
            var it = new Utf8CodePointReverseEnumerator(Bytes);
            while (it.MoveNext() && Unicode.IsWhitespace(it.Current))
            {
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        public Utf8Span TrimEnd(uint[] codePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8Span TrimEnd(Utf8Span characters)
        {
            if (characters == Empty)
            {
                // Trim Whitespace
                return TrimEnd();
            }

            Utf8CodePointReverseEnumerator it = new Utf8CodePointReverseEnumerator(Bytes);
            Utf8CodePointEnumerator itPrefix = characters.GetEnumerator();

            while (it.MoveNext())
            {
                bool found = false;
                // Iterate over prefix set
                while (itPrefix.MoveNext())
                {
                    if (it.Current == itPrefix.Current)
                    {
                        // Character found, don't check further
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // Reached the end, char was not found
                    break;
                }

                itPrefix.Reset();
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        public Utf8Span Trim()
        {
            return TrimStart().TrimEnd();
        }

        public Utf8Span Trim(uint[] codePoints)
        {
            throw new NotImplementedException();
        }
        #endregion

        // TODO: should we even have index based operations?
        #region Index-based operations
        public Utf8Span Substring(int index) => index == 0 ? this : Substring(index, Bytes.Length - index);

        public Utf8Span Substring(int index, int length)
        {
            if (length == 0) return Empty;
            if (index == 0 && length == Bytes.Length) return this;

            return new Utf8Span(_buffer.Slice(index, length));
        }

        public int IndexOf(Utf8Span value) => Bytes.IndexOf(value.Bytes);

        public int IndexOf(uint codePoint)
        {
            Utf8CodePointEnumerator it = GetEnumerator();
            while (it.MoveNext())
            {
                if (it.Current == codePoint)
                {
                    return it.PositionInCodeUnits;
                }
            }

            return StringNotFound;
        }

        // TODO: Replace implementation once we have Span.LastIndexOf
        public int LastIndexOf(Utf8Span value)
        {
            // Special case for looking for empty strings
            if (value.Bytes.Length == 0)
            {
                // Maintain parity with .NET C#'s LastIndexOf
                return Bytes.Length == 0 ? 0 : Bytes.Length - 1;
            }

            return Bytes.LastIndexOf(value.Bytes);
        }

        public int LastIndexOf(uint codePoint)
        {
            var it = new Utf8CodePointReverseEnumerator(Bytes);
            while (it.MoveNext())
            {
                if (it.Current == codePoint)
                {
                    // Move to beginning of code point
                    it.MoveNext();
                    return it.PositionInCodeUnits;
                }
            }

            return StringNotFound;
        }
        #endregion

        static int SequenceCompareTo(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            var minLength = left.Length;
            if (minLength > right.Length) minLength = right.Length;
            for (int i = 0; i < minLength; i++)
            {
                var result = left[i].CompareTo(right[i]);
                if (result != 0) return result;
            }
            return left.Length.CompareTo(right.Length);
        }
    }
}
