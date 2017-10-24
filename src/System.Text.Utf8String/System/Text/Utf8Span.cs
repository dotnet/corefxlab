// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Utf16;

namespace System.Text.Utf8
{
    [DebuggerDisplay("{ToString()}u8")]
    public ref partial struct Utf8Span
    {
        private readonly ReadOnlySpan<byte> _buffer;

        private const int StringNotFound = -1;

        static Utf8Span s_empty => default;

        // TODO: Validate constructors, When should we copy? When should we just use the underlying array?
        // TODO: Should we be immutable/readonly?
        public Utf8Span(ReadOnlySpan<byte> utf8Bytes) => _buffer = utf8Bytes;

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

        public static explicit operator string(Utf8Span utf8String) => utf8String.ToString();

        public ReadOnlySpan<byte> Bytes => _buffer;

        public override string ToString() => Utf8ToUtf16(Bytes);

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

        // TODO: implement Utf8Span.CompareTo
        public int CompareTo(Utf8Span other) => throw new NotImplementedException();

        public int CompareTo(Utf8String other) => CompareTo(other.Span);

        public int CompareTo(string other) => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        #region Slicing
        // TODO: Re-evaluate all Substring family methods and check their parameters name
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

        public Utf8Span TrimStart(uint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8Span TrimStart(byte[] trimCodeUnits)
        {
            throw new NotImplementedException();
        }

        public Utf8Span TrimStart(Utf8Span trimCharacters)
        {
            if (trimCharacters == Empty)
            {
                // Trim Whitespace
                return TrimStart();
            }

            Utf8CodePointEnumerator it = GetEnumerator();
            Utf8CodePointEnumerator itPrefix = trimCharacters.GetEnumerator();

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

        public Utf8Span TrimEnd(uint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8Span TrimEnd(Utf8Span trimCharacters)
        {
            if (trimCharacters == Empty)
            {
                // Trim Whitespace
                return TrimEnd();
            }

            Utf8CodePointReverseEnumerator it = new Utf8CodePointReverseEnumerator(Bytes);
            Utf8CodePointEnumerator itPrefix = trimCharacters.GetEnumerator();

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

        public Utf8Span Trim(uint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }
        #endregion

        // TODO: should we even have index based operations?
        #region Index-based operations
        public Utf8Span Substring(int index) => Substring(index, Bytes.Length - index);

        public Utf8Span Substring(int index, int length)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (length < 0)
            {
                // TODO: Should we support that?
                throw new ArgumentOutOfRangeException("length");
            }

            if (length == 0)
            {
                return Empty;
            }

            if (index > Bytes.Length - length)
            {
                // TODO: Should this be index or length?
                throw new ArgumentOutOfRangeException("index");
            }

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

            return LastIndexOf(Bytes, value.Bytes);
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

        public bool IsSubstringAt(int index, Utf8Span s)
        {
            if (index < 0 || index + s.Bytes.Length > Bytes.Length)
            {
                return false;
            }

            return Substring(index, s.Bytes.Length).Equals(s);
        }
        #endregion

        #region should be moved to other assemblies, e.g. SystemBuffers.Text
        private static string Utf8ToUtf16(ReadOnlySpan<byte> utf8Bytes)
        {
            // TODO: why do we return status here?
            var status = Encodings.Utf8.ToUtf16Length(utf8Bytes, out int bytesNeeded);
            var result = new String(' ', bytesNeeded >> 1);
            unsafe
            {
                fixed (char* pResult = result)
                {
                    var resultBytes = new Span<byte>((void*)pResult, bytesNeeded);
                    if (Encodings.Utf8.ToUtf16(utf8Bytes, resultBytes, out int consumed, out int written) == OperationStatus.Done)
                    {
                        Debug.Assert(written == resultBytes.Length);
                        return result;
                    }
                }
            }
            return String.Empty; // TODO: is this what we want to do if Bytes are invalid UTF8? Can Bytes be invalid UTF8?
        }

        private static int LastIndexOf(ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> values)
        {
            if (buffer.Length < values.Length) return -1;
            if (values.Length == 0) return 0;

            int candidateLength = buffer.Length;
            var firstByte = values[0];
            while (true)
            {
                int index = LastIndexOf(buffer.Slice(0, candidateLength), firstByte);
                if (index == -1) return -1;
                var slice = buffer.Slice(index);
                if (slice.StartsWith(values)) return index;
                candidateLength = index;
            }
        }

        private static int LastIndexOf(ReadOnlySpan<byte> buffer, byte value)
        {
            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                if (buffer[i] == value) return i;
            }
            return -1;
        }
        #endregion
    }
}
