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
    public partial class Utf8String
    {
        private readonly byte[] _buffer;

        private const int StringNotFound = -1;

        static Utf8String s_empty = new Utf8String(string.Empty);

        public Utf8String(ReadOnlySpan<byte> buffer) => _buffer = buffer.ToArray();
        public Utf8String(Utf8Span utf8Span) => _buffer = utf8Span.Bytes.ToArray();
        private Utf8String(byte[] dangerousBuffer) => _buffer = dangerousBuffer;

        public Utf8String(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("s", "String cannot be null");
            }

            if (str == string.Empty)
            {
                _buffer = ReadOnlySpan<byte>.Empty.ToArray();
            }
            else
            {
                _buffer = Encoding.UTF8.GetBytes(str);
            }
        }

        /// <summary>
        /// This constructor is for use by the compiler.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Utf8String(RuntimeFieldHandle utf8Data, int length) {
            _buffer = new byte[length];
            RuntimeHelpers.InitializeArray(_buffer, utf8Data);
        }

        public static explicit operator Utf8String(ArraySegment<byte> utf8Bytes)
            => new Utf8String(utf8Bytes);

        public static Utf8String Empty { get { return s_empty; } }

        /// <summary>
        /// Returns length of the string in UTF-8 code units (bytes)
        /// </summary>
        public int Length => _buffer.Length;

        public static implicit operator ReadOnlySpan<byte>(Utf8String utf8) => utf8.Bytes;
        
        public static explicit operator Utf8String(string s) => new Utf8String(s);
        
        public static explicit operator string(Utf8String s) => s.ToString();

        public ReadOnlySpan<byte> Bytes => _buffer;

        public override string ToString()
        {
            // TODO: why do we return status here?
            var status = Encodings.Utf8.ToUtf16Length(this.Bytes, out int bytesNeeded);
            var result = new String(' ', bytesNeeded >> 1);
            unsafe {
                fixed(char* pResult = result){
                    var resultBytes = new Span<byte>((void*)pResult, bytesNeeded);
                    if(Encodings.Utf8.ToUtf16(this.Bytes, resultBytes, out int consumed, out int written) == OperationStatus.Done){
                        Debug.Assert(written == resultBytes.Length);
                        return result;
                    }
                }
            }
            return String.Empty; // TODO: is this what we want to do if Bytes are invalid UTF8? Can Bytes be invalid UTF8?
        }

        public bool ReferenceEquals(Utf8String other) => Object.ReferenceEquals(this, other);

        public bool Equals(Utf8String other) => Bytes.SequenceEqual(other.Bytes);

        // TODO: is this efficient enough?
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

        public static bool operator ==(Utf8String left, Utf8String right) => left.Equals(right);
        public static bool operator !=(Utf8String left, Utf8String right) => !left.Equals(right);

        // TODO: do we like all these O(N) operators? 
        public static bool operator ==(Utf8String left, string right) => left.Equals(right);
        public static bool operator !=(Utf8String left, string right) => !left.Equals(right);
        public static bool operator ==(string left, Utf8String right) => right.Equals(left);
        public static bool operator !=(string left, Utf8String right) => !right.Equals(left);

        // TODO: implement Utf8String.CompareTo
        public int CompareTo(Utf8String other) => throw new NotImplementedException();
        public int CompareTo(string other) => throw new NotImplementedException();

        /// <summary>
        ///
        /// </summary>
        /// <param name="index">Index in UTF-8 code units (bytes)</param>
        /// <returns>Length in UTF-8 code units (bytes)</returns>
        // TODO: should all Utf8String slicing operations return Utf8Span?
        public Utf8String Substring(int index) => Substring(index, Length - index);

        /// <summary>
        ///
        /// </summary>
        /// <param name="index">Index in UTF-8 code units (bytes)</param>
        /// <returns>Length in UTF-8 code units (bytes)</returns>
        public Utf8String Substring(int index, int length)
        {
            if (length == 0)
            {
                return Empty;
            }

            return new Utf8String(_buffer.AsSpan().Slice(index, length));
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
                int pos = restOfTheString.IndexOf(value.Bytes[0]);
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
        public int IndexOf(byte codeUnit) => _buffer.AsSpan().IndexOf(codeUnit);

        // TODO: Should this be public?
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

        // TODO: Naive algorithm, reimplement faster - implemented to keep parity with IndexOf
        public int LastIndexOf(Utf8String value)
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

            Utf8String restOfTheString = this;

            for (int i = Length - 1; i >= value.Length - 1; restOfTheString = Substring(0, i--))
            {
                int pos = restOfTheString.LastIndexOf(value.Bytes[value.Length - 1]);
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
                if (codeUnit == Bytes[i])
                {
                    return i;
                }
            }

            return StringNotFound;
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

        // TODO: Re-evaluate all Substring family methods and check their parameters name
        public bool TrySubstringFrom(Utf8String value, out Utf8String result)
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

        public bool TrySubstringFrom(byte codeUnit, out Utf8String result)
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

        public bool TrySubstringFrom(uint codePoint, out Utf8String result)
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

        public bool TrySubstringTo(Utf8String value, out Utf8String result)
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

        public bool TrySubstringTo(byte codeUnit, out Utf8String result)
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

        public bool TrySubstringTo(uint codePoint, out Utf8String result)
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

        public bool IsSubstringAt(int index, Utf8String s)
        {
            if (index < 0 || index + s.Length > Length)
            {
                return false;
            }

            return Substring(index, s.Length).Equals(s);
        }

        public void CopyTo(Span<byte> buffer) => _buffer.CopyTo(buffer);

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
                        hash ^= (byte)Bytes[i];
                    }
                    return hash;
                }
                else
                {
                    int hash = Length;
                    hash ^= (byte)Bytes[0];
                    hash <<= 8;
                    hash ^= (byte)Bytes[1];
                    hash <<= 8;
                    hash ^= (byte)Bytes[Length - 2];
                    hash <<= 8;
                    hash ^= (byte)Bytes[Length - 1];
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

        public Utf8CodePointEnumerator GetEnumerator()
        {
            return new Utf8CodePointEnumerator(_buffer);
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

        public bool StartsWith(byte codeUnit)
        {
            if (Length == 0)
            {
                return false;
            }

            return Bytes[0] == codeUnit;
        }

        public bool StartsWith(Utf8String value)
        {
            if (value.Length > this.Length)
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

            return Bytes[Length - 1] == codeUnit;
        }

        public bool EndsWith(Utf8String value)
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

        public Utf8String TrimStart()
        {
            Utf8CodePointEnumerator it = GetEnumerator();
            while (it.MoveNext() && Utf8Helper.IsWhitespace(it.Current))
            {
            }

            return Substring(it.PositionInCodeUnits);
        }

        // TODO: implement Utf8String.TrimStart
        public Utf8String TrimStart(uint[] trimCodePoints) => throw new NotImplementedException();
        public Utf8String TrimStart(byte[] trimCodeUnits) => throw new NotImplementedException();

        public Utf8String TrimStart(Utf8String trimCharacters)
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

        public Utf8String TrimEnd()
        {
            var it = new Utf8CodePointReverseEnumerator(Bytes);
            while (it.MoveNext() && Utf8Helper.IsWhitespace(it.Current))
            {
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        // TODO: implement Utf8String.TrimEnd
        public Utf8String TrimEnd(uint[] trimCodePoints) => throw new NotImplementedException();

        public Utf8String TrimEnd(byte[] trimCodeUnits) => throw new NotImplementedException();

        public Utf8String TrimEnd(Utf8String trimCharacters)
        {
            if (trimCharacters == Empty)
            {
                // Trim Whitespace
                return TrimEnd();
            }

            var it = new Utf8CodePointReverseEnumerator(Bytes);
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

        public Utf8String Trim()
        {
            return TrimStart().TrimEnd();
        }

        // TODO: implement Utf8String.Trim
        public Utf8String Trim(uint[] trimCodePoints) => throw new NotImplementedException();

        public Utf8String Trim(byte[] trimCodeUnits) => throw new NotImplementedException();
    }
}
