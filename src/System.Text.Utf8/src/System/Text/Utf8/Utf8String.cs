// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.Text.Utf8
{
    public struct Utf8String : IEquatable<Utf8String>, IComparable<Utf8String>
    {
        static readonly Utf8String s_empty = new Utf8String(new byte[0]);

        byte[] _array;
        unsafe byte* _buffer;
        int _length;

        [CLSCompliant(false)]
        public unsafe Utf8String(byte* utf8, int length)
        {
            _array = null;
            _buffer = utf8;
            _length = length;
        }

        public Utf8String(byte[] utf8)
        {
            unsafe
            {
                _buffer = null;
            }
            _length = utf8.Length;
            _array = utf8;
        }

        public Utf8String(string utf16) : this(Encoding.UTF8.GetBytes(utf16))
        {
        }

        public static Utf8String Empty
        {
            get { return s_empty; }
        }

        public int Length { get { return _length; } }

        public int CompareTo(Utf8String other) { throw new NotImplementedException(); }

        public bool Equals(Utf8String other)
        {
            if (Length != other.Length)
            {
                return false;
            }

            unsafe
            {
                if (_buffer != null || other._buffer != null)
                {
                    throw new NotImplementedException();
                }
            }

            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] != other._array[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(string other) { throw new NotImplementedException(); }

        public int CompareTo(string other) { throw new NotImplementedException(); }

        // operators
        public static bool operator==(Utf8String left, Utf8String right) {
            return left.Equals(right);
        }
        public static bool operator!=(Utf8String left, Utf8String right)
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
        public static bool operator==(string left, Utf8String right)
        {
            return right.Equals(left);
        }
        public static bool operator !=(string left, Utf8String right)
        {
            return !right.Equals(left);
        }

        public byte[] CopyBytes() {
            unsafe
            {
                if (_buffer != null) { throw new NotImplementedException(); }
            }

            var copy = new byte[_array.Length];
            _array.CopyTo(copy, 0);
            return copy;
        }

        public override string ToString() {
            unsafe
            {
                if (_buffer != null)
                {
                    // TODO: this should be done without allocating the array
                    var array = new byte[_length];
                    for(int i=0; i<_length; i++)
                    {
                        array[i] = _buffer[i];
                    }
                    Encoding.UTF8.GetString(array, 0, _length);
                }
            }
            return Encoding.UTF8.GetString(_array, 0, _length);
        }

        public override bool Equals(object obj)
        {
            if (obj is Utf8String)
            {
                return Equals((Utf8String)obj);
            }
            else if (obj is string)
            {
                return Equals((string)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode() { throw new NotImplementedException(); }
    }
}



