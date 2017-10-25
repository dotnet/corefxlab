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
    public class Utf8String
    {
        private readonly byte[] _buffer;

        private const int StringNotFound = -1;

        static Utf8String s_empty = new Utf8String(string.Empty);

        public Utf8String(ReadOnlySpan<byte> utf8Bytes) => _buffer = utf8Bytes.ToArray();
        public Utf8String(Utf8Span utf8Span) => _buffer = utf8Span.Bytes.ToArray();

        public Utf8String(string utf16String)
        {
            if (utf16String == null)
            {
                throw new ArgumentNullException("s", "String cannot be null");
            }

            if (utf16String == string.Empty)
            {
                _buffer = ReadOnlySpan<byte>.Empty.ToArray();
            }
            else
            {
                _buffer = Encoding.UTF8.GetBytes(utf16String);
            }
        }

        /// <summary>
        /// This constructor is for use by the compiler.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Utf8String(RuntimeFieldHandle utf8Bytes, int length) {
            _buffer = new byte[length];
            RuntimeHelpers.InitializeArray(_buffer, utf8Bytes);
        }

        private Utf8String(byte[] noCopyUtf8Bytes) => _buffer = noCopyUtf8Bytes;

        public static Utf8String Empty => s_empty;

        public bool IsEmpty => Bytes.Length == 0;

        public Utf8CodePointEnumerator GetEnumerator() => new Utf8CodePointEnumerator(_buffer);

        public static implicit operator ReadOnlySpan<byte>(Utf8String utf8String) => utf8String.Bytes;
        
        public static explicit operator Utf8String(string utf16String) => new Utf8String(utf16String);
        
        public static explicit operator string(Utf8String utf8String) => utf8String.ToString();

        public ReadOnlySpan<byte> Bytes => _buffer;
        public ReadOnlyMemory<byte> Memory => _buffer;

        internal Utf8Span Span => new Utf8Span(Bytes);

        public override string ToString() => Span.ToString();

        public bool ReferenceEquals(Utf8String other) => Object.ReferenceEquals(this, other);

        public bool Equals(Utf8String other) => Bytes.SequenceEqual(other.Bytes);
        public bool Equals(Utf8Span other) => Bytes.SequenceEqual(other.Bytes);
        public bool Equals(string other) => Span.Equals(other);

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

        public override int GetHashCode() => Span.GetHashCode();

        public static bool operator ==(Utf8String left, Utf8String right) => left.Equals(right);
        public static bool operator !=(Utf8String left, Utf8String right) => !left.Equals(right);
        public static bool operator ==(Utf8String left, Utf8Span right) => left.Equals(right);
        public static bool operator !=(Utf8String left, Utf8Span right) => !left.Equals(right);
        public static bool operator ==(Utf8Span left, Utf8String right) => right.Equals(left);
        public static bool operator !=(Utf8Span left, Utf8String right) => !right.Equals(left);

        // TODO: do we like all these O(N) operators? 
        public static bool operator ==(Utf8String left, string right) => left.Equals(right);
        public static bool operator !=(Utf8String left, string right) => !left.Equals(right);
        public static bool operator ==(string left, Utf8String right) => right.Equals(left);
        public static bool operator !=(string left, Utf8String right) => !right.Equals(left);

        public int CompareTo(Utf8String other) => Span.CompareTo(other);
        public int CompareTo(string other) => Span.CompareTo(other);
        public int CompareTo(Utf8Span other) => Span.CompareTo(other);

        public bool StartsWith(uint codePoint) => Span.StartsWith(codePoint);

        public bool StartsWith(Utf8String value) => Span.StartsWith(value.Span);

        public bool StartsWith(Utf8Span value) => Span.StartsWith(value);

        public bool EndsWith(Utf8String value) => Span.EndsWith(value.Span);

        public bool EndsWith(Utf8Span value) => Span.EndsWith(value);

        public bool EndsWith(uint codePoint) => Span.EndsWith(codePoint);

        #region Slicing
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

        public Utf8String Trim() => TrimStart().TrimEnd();

        public Utf8String TrimStart()
        {
            Utf8CodePointEnumerator it = GetEnumerator();
            while (it.MoveNext() && Unicode.IsWhitespace(it.Current))
            {
            }

            return Substring(it.PositionInCodeUnits);
        }

        public Utf8String TrimStart(uint[] trimCodePoints) {
            if (trimCodePoints == null || trimCodePoints.Length == 0) return TrimStart(); // Trim Whitespace

            Utf8CodePointEnumerator it = GetEnumerator();       
            while (it.MoveNext()) {
                if(Array.IndexOf(trimCodePoints, it.Current) == -1){
                    break;
                }
            }

            return Substring(it.PositionInCodeUnits);
        }

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
            while (it.MoveNext() && Unicode.IsWhitespace(it.Current))
            {
            }

            return Substring(0, it.PositionInCodeUnits);
        }

        public Utf8String TrimEnd(uint[] trimCodePoints) => throw new NotImplementedException();

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

        public Utf8String Trim(uint[] trimCodePoints) => throw new NotImplementedException();
        #endregion

        // TODO: should we even have index based operations?
        #region Index-based operations
        public Utf8String Substring(int index) => index==0?this:Substring(index, Bytes.Length - index);

        public Utf8String Substring(int index, int length)
        {
            if (length == 0)
            {
                return Empty;
            }
            if(index == 0 && length == Bytes.Length) return this;

            return new Utf8String(_buffer.AsSpan().Slice(index, length));
        }

        public int IndexOf(Utf8String value) => Bytes.IndexOf(value.Bytes);

        public int IndexOf(uint codePoint) => Span.IndexOf(codePoint);

        public int LastIndexOf(Utf8String value) => Span.LastIndexOf(value.Span);

        public int LastIndexOf(uint codePoint) => Span.LastIndexOf(codePoint);

        public bool IsSubstringAt(int index, Utf8String utf8String) => Span.IsSubstringAt(index, utf8String.Span);
        #endregion
    }
}
