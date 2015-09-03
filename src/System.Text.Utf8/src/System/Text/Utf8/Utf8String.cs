// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public struct Utf8String : IEquatable<Utf8String>, IComparable<Utf8String>
    {
        byte[] _array;
        unsafe byte* _buffer;
        int _bufferLength;

        [CLSCompliant(false)]
        public unsafe Utf8String(byte* utf8, int length)
        {
            _array = null;
            _buffer = utf8;
            _bufferLength = length;
        }

        public Utf8String(byte[] utf8)
        {
            unsafe
            {
                _buffer = null;
            }
            _bufferLength = utf8.Length;
            _array = utf8;
        }

        public int Length { get { return _array == null ? _bufferLength : _array.Length; } }

        public int CompareTo(Utf8String other) { throw new NotImplementedException(); }

        public bool Equals(Utf8String other) { throw new NotImplementedException(); }

        public bool Equals(string other) { throw new NotImplementedException(); }

        public int CompareTo(string other) { throw new NotImplementedException(); }

        public byte[] ToArray() { throw new NotImplementedException(); }

        public override string ToString() { throw new NotImplementedException(); }

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


