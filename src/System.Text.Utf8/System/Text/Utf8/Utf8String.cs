// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Utf16;

namespace System.Text.Utf8
{
    [DebuggerDisplay("{ToString()}u8")]
    public partial struct Utf8String : IEnumerable<Utf8CodeUnit>, IEquatable<Utf8String>, IComparable<Utf8String> 
    {
        private ReadOnlySpan<byte> _buffer;

        private const int StringNotFound = -1;

        static readonly Utf8String s_empty = default(Utf8String);

        // TODO: Validate constructors, When should we copy? When should we just use the underlying array?
        // TODO: Should we be immutable/readonly?
        public Utf8String(ReadOnlySpan<byte> buffer)
        {
            _buffer = buffer;
        }

        public Utf8String(byte[] utf8bytes)
        {
            _buffer = new ReadOnlySpan<byte>(utf8bytes);
        }

        public Utf8String(byte[] utf8bytes, int index, int length)
        {
            _buffer = new ReadOnlySpan<byte>(utf8bytes, index, length);
        }

        // TODO: reevaluate implementation
        public Utf8String(IEnumerable<UnicodeCodePoint> codePoints)
        {
            int len = GetUtf8LengthInBytes(codePoints);
            var newSpan = new Span<byte>(new byte[len]);
            _buffer = newSpan;
            foreach (UnicodeCodePoint codePoint in codePoints)
            {
                int encodedBytes;
                if (!Utf8Encoder.TryEncodeCodePoint(codePoint, newSpan, out encodedBytes))
                {
                    throw new ArgumentException("Invalid code point", "codePoints");
                }
                newSpan = newSpan.Slice(encodedBytes);
            }
        }

        public Utf8String(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s", "String cannot be null");
            }

            if (s == string.Empty)
            {
                _buffer = ReadOnlySpan<byte>.Empty;
            }
            else
            {
                _buffer = new ReadOnlySpan<byte>(GetUtf8BytesFromString(s));
            }
        }

        /// <summary>
        /// This constructor is for use by the compiler.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Utf8String(RuntimeFieldHandle utf8Data, int length) : this(CreateArrayFromFieldHandle(utf8Data, length))
        {
        }

        static byte[] CreateArrayFromFieldHandle(RuntimeFieldHandle utf8Data, int length)
        {
            var array = new byte[length];
            RuntimeHelpers.InitializeArray(array, utf8Data);
            return array;
        }

        public static Utf8String Empty { get { return s_empty; } }

        /// <summary>
        /// Returns length of the string in UTF-8 code units (bytes)
        /// </summary>
        public int Length
        {
            get
            {
                return _buffer.Length;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_buffer);
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
                return new CodePointEnumerable(_buffer);
            }
        }

        public Utf8CodeUnit this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // there is no need to check the boundaries -> Span is going to do this on it's own
                return (Utf8CodeUnit)_buffer[i];
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

        public ReadOnlySpan<byte> Bytes
        {
            get { return _buffer; }
        }
        public override string ToString()
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
                       
            unsafe
            {
                Span<byte> buffer;
                char* stackChars = null;
                char[] characters = null;

                if (len <= 256)
                {
                    char* stackallocedChars = stackalloc char[len];
                    stackChars = stackallocedChars;
                    buffer = new Span<byte>(stackChars, len * 2);
                }
                else
                {
                    // HACK: Can System.Buffers be used here?
                    characters = new char[len];
                    buffer = characters.Slice().Cast<char, byte>();
                }

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

                // TODO: We already have a char[] and this will copy, how to avoid that
                return stackChars != null 
                    ? new string(stackChars, 0, len)
                    : new string(characters);
            }
        }

        public bool ReferenceEquals(Utf8String other)
        {
            return _buffer.ReferenceEquals(other._buffer);
        }

        public bool Equals(Utf8String other)
        {
            return _buffer.SequenceEqual(other._buffer);
        }

        public bool Equals(string other)
        {
            CodePointEnumerator thisEnumerator = GetCodePointEnumerator();
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

            if (length == Length)
            {
                return this;
            }

            if (index + length > Length)
            {
                // TODO: Should this be index or length?
                throw new ArgumentOutOfRangeException("index");
            }

            return new Utf8String(_buffer.Slice(index, length));
        }

        // TODO: Naive algorithm, reimplement faster
        // TODO: Should this be public?
        public int IndexOf(Utf8String value)
        {
            if (value.Length == 0)
            {
                // TODO: Is this the right answer?
                // TODO: Does this even make sense?
                return 0;
            }

            if (Length == 0)
            {
                return StringNotFound;
            }

            Utf8String restOfTheString = this;
            for (int i = 0; restOfTheString.Length <= Length; restOfTheString = Substring(++i))
            {
                int pos = restOfTheString.IndexOf(value[0]);
                if (pos == StringNotFound)
                {
                    return StringNotFound;
                }
                i += pos;
                if (IsSubstringAt(i, value))
                {
                    return i;
                }
            }

            return StringNotFound;
        }

        // TODO: Should this be public?
        public int IndexOf(Utf8CodeUnit codeUnit)
        {
            // TODO: _buffer.IndexOf(codeUnit.Value); when Span has it

            for (int i = 0; i < Length; i++)
            {
                if (codeUnit == this[i])
                {
                    return i;
                }
            }

            return StringNotFound;
        }

        // TODO: Should this be public?
        public int IndexOf(UnicodeCodePoint codePoint)
        {
            CodePointEnumerator it = GetCodePointEnumerator();
            while (it.MoveNext())
            {
                if (it.Current == codePoint)
                {
                    return it.PositionInCodeUnits;
                }
            }
            
            return StringNotFound;
        }

        // TODO: Re-evaluate all Substring family methods and check their parameters name
        public bool TrySubstringFrom(Utf8String value, out Utf8String result)
        {
            int idx = IndexOf(value);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(idx);
            return true;
        }

        public bool TrySubstringFrom(Utf8CodeUnit codeUnit, out Utf8String result)
        {
            int idx = IndexOf(codeUnit);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(idx);
            return true;
        }

        public bool TrySubstringFrom(UnicodeCodePoint codePoint, out Utf8String result)
        {
            int idx = IndexOf(codePoint);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(idx);
            return true;
        }

        public bool TrySubstringTo(Utf8String value, out Utf8String result)
        {
            int idx = IndexOf(value);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(0, idx);
            return true;
        }

        public bool TrySubstringTo(Utf8CodeUnit codeUnit, out Utf8String result)
        {
            int idx = IndexOf(codeUnit);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(0, idx);
            return true;
        }

        public bool TrySubstringTo(UnicodeCodePoint codePoint, out Utf8String result)
        {
            int idx = IndexOf(codePoint);

            if (idx == StringNotFound)
            {
                result = default(Utf8String);
                return false;
            }

            result = Substring(0, idx);
            return true;
        }

        public bool IsSubstringAt(int index, Utf8String s)
        {
            if (index < 0 || index + s.Length > Length)
            {
                return false;
            }

            return Substring(index, s.Length).Equals(s);
        }

        public void CopyTo(Span<byte> buffer)
        {
            if (buffer.Length < Length)
            {
                throw new ArgumentException("buffer");
            }

            if (!_buffer.TryCopyTo(buffer))
            {
                throw new Exception("Internal error: range check already done, no errors expected here");
            }
        }

        public void CopyTo(byte[] buffer)
        {
            CopyTo(new Span<byte>(buffer));
        }

        // TODO: write better hashing function
        // TODO: span.GetHashCode() + some constant?
        public override int GetHashCode()
        {
            unchecked
            {
                if (Length <= 4)
                {
                    int hash = Length;
                    for (int i = 0; i < Length; i++)
                    {
                        hash <<= 8;
                        hash ^= (byte)this[i];
                    }
                    return hash;
                }
                else
                {
                    int hash = Length;
                    hash ^= (byte)this[0];
                    hash <<= 8;
                    hash ^= (byte)this[1];
                    hash <<= 8;
                    hash ^= (byte)this[Length - 2];
                    hash <<= 8;
                    hash ^= (byte)this[Length - 1];
                    return hash;
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

            return false;
        }

        private CodePointEnumerator GetCodePointEnumerator()
        {
            return new CodePointEnumerator(_buffer);
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

            return this[0] == codeUnit;
        }

        public bool StartsWith(Utf8String value)
        {
            if(value.Length > this.Length)
            {
                return false;
            }

            return this.Substring(0, value.Length).Equals(value);
        }

        public bool EndsWith(Utf8CodeUnit codeUnit)
        {
            if (Length == 0)
            {
                return false;
            }

            return this[Length - 1] == codeUnit;
        }

        public bool EndsWith(Utf8String value)
        {
            if (Length < value.Length)
            {
                return false;
            }

            return this.Substring(Length - value.Length, value.Length).Equals(value);
        }

        public bool EndsWith(UnicodeCodePoint codePoint)
        {
            throw new NotImplementedException();
        }

        private static int GetUtf8LengthInBytes(IEnumerable<UnicodeCodePoint> codePoints)
        {
            int len = 0;
            foreach (var codePoint in codePoints)
            {
                len += Utf8Encoder.GetNumberOfEncodedBytes(codePoint);
            }

            return len;
        }

        // TODO: This should return Utf16CodeUnits which should wrap byte[]/Span<byte>, same for other encoders
        private static byte[] GetUtf8BytesFromString(string s)
        {
            int len = 0;
            for (int i = 0; i < s.Length; /* intentionally no increment */)
            {
                UnicodeCodePoint codePoint;
                int encodedChars;
                if (!Utf16LittleEndianEncoder.TryDecodeCodePointFromString(s, i, out codePoint, out encodedChars))
                {
                    throw new ArgumentException("s", "Invalid surrogate pair in the string.");
                }

                if (encodedChars <= 0)
                {
                    // TODO: Fix exception type
                    throw new Exception("internal error");
                }

                int encodedBytes = Utf8Encoder.GetNumberOfEncodedBytes(codePoint);
                if (encodedBytes == 0)
                {
                    // TODO: Fix exception type
                    throw new Exception("Internal error: Utf16Decoder somehow got CodePoint out of range");
                }
                len += encodedBytes;

                i += encodedChars;
            }

            byte[] bytes = new byte[len];
           
            var p = new Span<byte>(bytes);
            for (int i = 0; i < s.Length; /* intentionally no increment */)
            {
                UnicodeCodePoint codePoint;
                int encodedChars;
                if (Utf16LittleEndianEncoder.TryDecodeCodePointFromString(s, i, out codePoint, out encodedChars))
                {
                    i += encodedChars;
                    int encodedBytes;
                    if (Utf8Encoder.TryEncodeCodePoint(codePoint, p, out encodedBytes))
                    {
                        p = p.Slice(encodedBytes);
                    }
                    else
                    {
                        // TODO: Fix exception type
                        throw new Exception("Internal error: Utf16Decoder somehow got CodePoint out of range or the buffer is too small");
                    }
                }
                else
                {
                    // TODO: Fix exception type
                    throw new Exception("Internal error: we did pre-validation of the string, nothing should go wrong");
                }
            }
        
            return bytes;
        }

        public Utf8String TrimStart()
        {
            CodePointEnumerator it = GetCodePointEnumerator();
            while (it.MoveNext() && UnicodeCodePoint.IsWhitespace(it.Current))
            {
            }

            return Substring(it.PositionInCodeUnits);
        }

        public Utf8String TrimStart(UnicodeCodePoint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8String TrimStart(Utf8CodeUnit[] trimCodeUnits)
        {
            throw new NotImplementedException();
        }

        public Utf8String TrimEnd()
        {
            CodePointReverseEnumerator it = CodePoints.GetReverseEnumerator();
            while (it.MoveNext() && UnicodeCodePoint.IsWhitespace(it.Current))
            {
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        public Utf8String TrimEnd(UnicodeCodePoint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8String TrimEnd(Utf8CodeUnit[] trimCodeUnits)
        {
            throw new NotImplementedException();
        }

        public Utf8String Trim()
        {
            return TrimStart().TrimEnd();
        }

        public Utf8String Trim(UnicodeCodePoint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8String Trim(Utf8CodeUnit[] trimCodeUnits)
        {
            throw new NotImplementedException();
        }

        // TODO: Name TBD, CopyArray? GetBytes?
        public byte[] CopyBytes()
        {
            return _buffer.CreateArray();
        }

        public Utf8CodeUnit[] CopyCodeUnits()
        {
            throw new NotImplementedException();
        }
    }
}
