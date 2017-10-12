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
    public partial ref struct Utf8Span
    {
        private readonly ReadOnlySpan<byte> _buffer;

        private const int StringNotFound = -1;

        static Utf8Span s_empty => default;

        // TODO: Validate constructors, When should we copy? When should we just use the underlying array?
        // TODO: Should we be immutable/readonly?
        public Utf8Span(ReadOnlySpan<byte> buffer)
        {
            _buffer = buffer;
        }

        public Utf8Span(byte[] utf8bytes)
        {
            _buffer = new ReadOnlySpan<byte>(utf8bytes);
        }

        public Utf8Span(byte[] utf8bytes, int index, int length)
        {
            _buffer = new ReadOnlySpan<byte>(utf8bytes, index, length);
        }

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
                _buffer = new ReadOnlySpan<byte>(GetUtf8BytesFromString(utf16String));
            }
        }

        public Utf8Span(Utf8String utf8String)
        {
            _buffer = utf8String.CopyBytes();
        }

        /// <summary>
        /// This constructor is for use by the compiler.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Utf8Span(RuntimeFieldHandle utf8Data, int length) : this(CreateArrayFromFieldHandle(utf8Data, length))
        {
        }

        public static explicit operator Utf8Span(ArraySegment<byte> utf8Bytes)
        {
            return new Utf8Span(utf8Bytes);
        }

        static byte[] CreateArrayFromFieldHandle(RuntimeFieldHandle utf8Data, int length)
        {
            var array = new byte[length];
            RuntimeHelpers.InitializeArray(array, utf8Data);
            return array;
        }

        public static Utf8Span Empty { get { return s_empty; } }

        /// <summary>
        /// Returns length of the string in UTF-8 code units (bytes)
        /// </summary>
        public int Length => _buffer.Length;

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_buffer);
        }

        public CodePointEnumerable CodePoints
        {
            get
            {
                return new CodePointEnumerable(_buffer);
            }
        }

        private byte this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
             get
                       {
                               // there is no need to check the boundaries -> Span is going to do this on it's own 
                                return (byte)_buffer[i];
                           }
        }

        public static implicit operator ReadOnlySpan<byte>(Utf8Span utf8)
        {
            return utf8.Bytes;
        }

        public static explicit operator Utf8Span(string s)
        {
            return new Utf8Span(s);
        }

        public static explicit operator string(Utf8Span s)
        {
            return s.ToString();
        }

        public ReadOnlySpan<byte> Bytes => _buffer;

        public override string ToString()
        {
            var status = Encodings.Utf8.ToUtf16Length(this.Bytes, out int needed);
            if (status != Buffers.OperationStatus.Done)
                return string.Empty;

            // UTF-16 is 2 bytes per char
            var chars = new char[needed >> 1];
            var utf16 = new Span<char>(chars).AsBytes();
            status = Encodings.Utf8.ToUtf16(this.Bytes, utf16, out int consumed, out int written);
            if (status != Buffers.OperationStatus.Done)
                return string.Empty;

            return new string(chars);
        }

        public bool ReferenceEquals(Utf8Span other)
        {
            return _buffer == other._buffer;
        }

        public bool Equals(Utf8Span other)
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

        public static bool operator ==(Utf8Span left, Utf8Span right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8Span left, Utf8Span right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Utf8Span left, string right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8Span left, string right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(string left, Utf8Span right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(string left, Utf8Span right)
        {
            return !right.Equals(left);
        }

        public int CompareTo(Utf8Span other)
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
        public Utf8Span Substring(int index)
        {
            return Substring(index, Length - index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index">Index in UTF-8 code units (bytes)</param>
        /// <returns>Length in UTF-8 code units (bytes)</returns>
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

            if (index > Length - length)
            {
                // TODO: Should this be index or length?
                throw new ArgumentOutOfRangeException("index");
            }

            return new Utf8Span(_buffer.Slice(index, length));
        }

        // TODO: Naive algorithm, reimplement faster
        // TODO: Should this be public?
        public int IndexOf(Utf8Span value)
        {
            if (value.Length == 0)
            {
                return 0;
            }

            if (Length == 0)
            {
                return StringNotFound;
            }

            Utf8Span restOfTheString = this;
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
        public int IndexOf(uint codePoint)
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

        // TODO: Naive algorithm, reimplement faster - implemented to keep parity with IndexOf
        public int LastIndexOf(Utf8Span value)
        {
            // Special case for looking for empty strings
            if (value.Length == 0)
            {
                // Maintain parity with .NET C#'s LastIndexOf
                return Length == 0 ? 0 : Length - 1;
            }

            if (Length == 0)
            {
                return StringNotFound;
            }

            Utf8Span restOfTheString = this;

            for (int i = Length - 1; i >= value.Length - 1; restOfTheString = Substring(0, i--))
            {
                int pos = restOfTheString.LastIndexOf(value[value.Length - 1]);
                if (pos == StringNotFound)
                {
                    return StringNotFound;
                }

                int substringStart = pos - (value.Length - 1);
                if (IsSubstringAt(substringStart, value))
                {
                    return substringStart;
                }

            }

            return StringNotFound;

        }

        public int LastIndexOf(byte codeUnit)
        {
            for (int i = Length - 1; i >= 0; i--)
            {
                if (codeUnit == this[i])
                {
                    return i;
                }
            }

            return StringNotFound;
        }

        public int LastIndexOf(uint codePoint)
        {
            CodePointReverseEnumerator it = CodePoints.GetReverseEnumerator();
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

        public bool TrySubstringFrom(byte codeUnit, out Utf8Span result)
        {
            int idx = IndexOf(codeUnit);

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

        public bool TrySubstringTo(byte codeUnit, out Utf8Span result)
        {
            int idx = IndexOf(codeUnit);

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

        public bool IsSubstringAt(int index, Utf8Span s)
        {
            if (index < 0 || index + s.Length > Length)
            {
                return false;
            }

            return Substring(index, s.Length).Equals(s);
        }

        public void CopyTo(Span<byte> buffer)
        {
            _buffer.CopyTo(buffer);
        }

        public void CopyTo(byte[] buffer)
        {
            _buffer.CopyTo(buffer);
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
            if (obj is Utf8Span)
            {
                return Equals((Utf8Span)obj);
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

        public bool StartsWith(uint codePoint)
        {
            CodePointEnumerator e = GetCodePointEnumerator();
            if (!e.MoveNext())
            {
                return false;
            }

            return e.Current == codePoint;
        }

        public bool StartsWith(byte codeUnit)
        {
            if (Length == 0)
            {
                return false;
            }

            return this[0] == codeUnit;
        }

        public bool StartsWith(Utf8Span value)
        {
            if(value.Length > this.Length)
            {
                return false;
            }

            return this.Substring(0, value.Length).Equals(value);
        }

        public bool EndsWith(byte codeUnit)
        {
            if (Length == 0)
            {
                return false;
            }

            return this[Length - 1] == codeUnit;
        }

        public bool EndsWith(Utf8Span value)
        {
            if (Length < value.Length)
            {
                return false;
            }

            return this.Substring(Length - value.Length, value.Length).Equals(value);
        }

        public bool EndsWith(uint codePoint)
        {
            throw new NotImplementedException();
        }

        private static int GetUtf8LengthInBytes(IEnumerable<uint> codePoints)
        {
            int len = 0;
            foreach (var codePoint in codePoints)
            {
                len += Utf8Helper.GetNumberOfEncodedBytes(codePoint);
            }

            return len;
        }

        // TODO: This should return Utf16CodeUnits which should wrap byte[]/Span<byte>, same for other encoders
        private static byte[] GetUtf8BytesFromString(string str)
        {
            var utf16 = str.AsReadOnlySpan().AsBytes();
            var status = Encodings.Utf16.ToUtf8Length(utf16, out int needed);
            if (status != Buffers.OperationStatus.Done)
                return null;

            var utf8 = new byte[needed];
            status = Encodings.Utf16.ToUtf8(utf16, utf8, out int consumed, out int written);
            if (status != Buffers.OperationStatus.Done)
                // This shouldn't happen...
                return null;

            return utf8;
        }

        public Utf8Span TrimStart()
        {
            CodePointEnumerator it = GetCodePointEnumerator();
            while (it.MoveNext() && Utf8Helper.IsWhitespace(it.Current))
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

            CodePointEnumerator it = GetCodePointEnumerator();
            CodePointEnumerator itPrefix = trimCharacters.GetCodePointEnumerator();
            
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
            CodePointReverseEnumerator it = CodePoints.GetReverseEnumerator();
            while (it.MoveNext() && Utf8Helper.IsWhitespace(it.Current))
            {
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        public Utf8Span TrimEnd(uint[] trimCodePoints)
        {
            throw new NotImplementedException();
        }

        public Utf8Span TrimEnd(byte[] trimCodeUnits)
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

            CodePointReverseEnumerator it = CodePoints.GetReverseEnumerator();
            CodePointEnumerator itPrefix = trimCharacters.GetCodePointEnumerator();

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

        public Utf8Span Trim(byte[] trimCodeUnits)
        {
            throw new NotImplementedException();
        }

        public byte[] CopyBytes()
        {
            return _buffer.ToArray();
        }

        private static bool IsWhiteSpace(byte codePoint)
        {
            return codePoint == ' ' || codePoint == '\n' || codePoint == '\r' || codePoint == '\t';
        }
    }
}
