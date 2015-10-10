// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Text.Utf16;

namespace System.Text.Utf8
{
    // TODO: We should have separate enumerators for code points and code units. Utf8String should implement IEnumerable<Utf8CodeUnit>
    public partial struct Utf8String : IEnumerable<Utf8CodeUnit>, IEquatable<Utf8String>, IComparable<Utf8String> 
    {
        private ByteSpan _buffer;

        // TODO: Reduce number of members when we get Span<byte> runtime support
        private byte[] _bytes;
        private int _index;
        private int _length;

        static readonly Utf8String s_empty = new Utf8String(new byte[0]);

        // TODO: Validate constructors, When should we copy? When should we just use the underlying array?
        // TODO: Should we be immutable/readonly?
        public Utf8String(ByteSpan buffer)
        {
            _buffer = buffer;
            _bytes = null;
            _index = 0;
            _length = 0;
        }

        public Utf8String(byte[] utf8bytes) : this(utf8bytes, 0, utf8bytes.Length)
        {
        }

        public Utf8String(byte[] utf8bytes, int index, int length)
        {
            if (utf8bytes == null)
            {
                throw new ArgumentNullException("utf8bytes");
            }
            if (index + length > utf8bytes.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            _buffer = default(ByteSpan);
            _bytes = utf8bytes;
            _index = index;
            _length = length;
        }

        public Utf8String(IEnumerable<UnicodeCodePoint> codePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8String(string s)
        {
            throw new NotImplementedException();
        }

        public static Utf8String Empty { get { return s_empty; } }

        /// <summary>
        /// Returns length of the string in UTF-8 code units (bytes)
        /// </summary>
        public int Length
        {
            get
            {
                if (_bytes != null)
                {
                    return _length;
                }
                else
                {
                    return _buffer.Length;
                }
            }
        }

        public Enumerator GetEnumerator()
        {
            if (_bytes != null)
            {
                return new Enumerator(_bytes, _index, _length);
            }
            else
            {
                return new Enumerator(_buffer);
            }
        }

        IEnumerator<Utf8CodeUnit> IEnumerable<Utf8CodeUnit>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CodePointEnumerable CodePoints
        {
            get
            {
                if (_bytes != null)
                {
                    return new CodePointEnumerable(_bytes, _index, _length);
                }
                else
                {
                    return new CodePointEnumerable(_buffer);
                }
            }
        }

        private Utf8CodeUnit GetCodeUnitAtPositionUnchecked(int i)
        {
            if (_bytes != null)
            {
                return (Utf8CodeUnit)_bytes[_index + i];
            }
            else
            {
                return (Utf8CodeUnit)_buffer[i];
            }
        }

        public Utf8CodeUnit this[int i]
        {
            get
            {
                if (i < 0 || i >= Length)
                {
                    throw new ArgumentOutOfRangeException("i");
                }
                else
                {
                    return GetCodeUnitAtPositionUnchecked(i);
                }
            }
        }

        public static explicit operator Utf8String(string s)
        {
            return new Utf8String(s);
        }

        public static explicit operator string(Utf8String s)
        {
            return s.ToString();
        }

        public override unsafe string ToString()
        {
            // get length first
            // TODO: Optimize for characters of length 1 or 2 in UTF-8 representation (no need to read anything)
            // TODO: is compiler gonna do the right thing here?
            // TODO: Should we use Linq's Count()?
            int len = 0;
            foreach (var codePoint in CodePoints)
            {
                len++;
                if (UnicodeCodePoint.IsSurrogate(codePoint))
                {
                    len++;
                }
            }

            char[] characters = new char[len];
            fixed (char* pinnedCharacters = characters)
            {
                ByteSpan buffer = new ByteSpan((byte*)pinnedCharacters, len * 2);
                foreach (var codePoint in CodePoints)
                {
                    int bytesEncoded;
                    if (!Utf16LittleEndianEncoder.TryEncodeCodePoint(codePoint, buffer, out bytesEncoded))
                    {
                        // TODO: Change Exception type
                        throw new Exception("invalid character");
                    }
                    buffer = buffer.Slice(bytesEncoded);
                }
            }

            // TODO: We already have a char[] and this will copy, how to avoid that
            return new string(characters);
        }

        public unsafe bool Equals(Utf8String other)
        {
            if (_bytes == null && other._bytes == null)
            {
                return _buffer.Equals(other._buffer);
            }
            else if (_bytes != null && other._bytes != null)
            {
                fixed (byte* pinnedBytes = _bytes) fixed (byte* pinnedOthersBytes = other._bytes)
                {
                    ByteSpan b1 = new ByteSpan(pinnedBytes + _index, _length);
                    ByteSpan b2 = new ByteSpan(pinnedOthersBytes + other._index, other._length);
                    return b1.Equals(b2);
                }
            }
            else if (_bytes != null && other._bytes == null)
            {
                fixed (byte* pinnedBytes = _bytes)
                fixed (byte* pinnedOthersBytes = other._bytes)
                {
                    ByteSpan b1 = new ByteSpan(pinnedBytes + _index, _length);
                    return b1.Equals(other._buffer);
                }
            }
            else // if (_bytes == null && other._bytes != null)
            {
                fixed (byte* pinnedOthersBytes = other._bytes)
                {
                    ByteSpan b2 = new ByteSpan(pinnedOthersBytes + other._index, other._length);
                    return other._buffer.Equals(b2);
                }
            }
        }

        public bool Equals(string other)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Utf8String left, Utf8String right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8String left, Utf8String right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Utf8String left, string right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8String left, string right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(string left, Utf8String right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(string left, Utf8String right)
        {
            return !right.Equals(left);
        }

        public int CompareTo(Utf8String other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Index in UTF-8 code units (bytes)</param>
        /// <returns>Length in UTF-8 code units (bytes)</returns>
        public Utf8String Substring(int index)
        {
            return Substring(index, Length - index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Index in UTF-8 code units (bytes)</param>
        /// <returns>Length in UTF-8 code units (bytes)</returns>
        public Utf8String Substring(int index, int length)
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

            if (index + length > Length)
            {
                // TODO: Should this be index or length?
                throw new ArgumentOutOfRangeException("index");
            }

            if (_bytes != null)
            {
                return new Utf8String(_bytes, _index + index, length);
            }
            else
            {
                return new Utf8String(_buffer.Slice(index, length));
            }
        }

        public Utf8String SubstringFrom(Utf8String substring)
        {
            throw new NotImplementedException();
        }

        public Utf8String SubstringTo(Utf8String substring)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
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

            return false;
        }

        private CodePointEnumerator GetCodePointEnumerator()
        {
            if (_bytes != null)
            {
                return new CodePointEnumerator(_bytes, _index, _length);
            }
            else
            {
                return new CodePointEnumerator(_buffer);
            }
        }

        public bool StartsWith(UnicodeCodePoint codePoint)
        {
            CodePointEnumerator e = GetCodePointEnumerator();
            if (!e.MoveNext())
            {
                return false;
            }

            return e.Current == codePoint;
        }

        public bool StartsWith(Utf8CodeUnit codeUnit)
        {
            if (Length == 0)
            {
                return false;
            }

            return GetCodeUnitAtPositionUnchecked(0) == codeUnit;
        }

        public bool StartsWith(Utf8String value)
        {
            return this.Substring(0, value.Length).Equals(value);
        }
    }
}
