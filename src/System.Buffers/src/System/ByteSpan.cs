// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System {

    public unsafe struct ByteSpan : IEquatable<ByteSpan> {
        internal byte* _data;
        internal int _length;

        [CLSCompliant(false)]
        public ByteSpan(byte* data, int length)
        {
            _data = data;
            _length = length;
        }

        public static ByteSpan Empty
        {
            get { return new ByteSpan(); }
        }

        public byte this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index >= Length) Environment.FailFast("index out of range");
                return _data[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index >= Length) Environment.FailFast("index out of range");
                _data[index] = value;
            }
        }

        [CLSCompliant(false)]
        public bool TrySet(byte* value, int valueLength)
        {
            if (valueLength > Length)
                return false;
            BufferInternal.MemoryCopy(value, _data, _length, valueLength);
            return true;
        }

        [CLSCompliant(false)]
        public bool TryCopyTo(byte* value, int valueLength)
        {
            if (Length > valueLength)
                return false;
            BufferInternal.MemoryCopy(_data, value, valueLength, _length);
            return true;
        }

        [CLSCompliant(false)]
        public unsafe byte* UnsafeBuffer
        {
            get { return _data; }
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _length;
            }
        }

        public ByteSpan Slice(int index)
        {
            if (index >= Length)
                return new ByteSpan(_data, 0);

            var data = _data + index;
            var length = _length - index;
            return new ByteSpan(data, length);
        }

        public ByteSpan Slice(int index, int count)
        {
            Precondition.Require(index + count < Length);

            var data = _data + index;
            return new ByteSpan(data, count);
        }

        public byte[] CreateArray()
        {
            if(_length == 0) {
                return new byte[0];
            }
            byte[] array = new byte[_length];
            Marshal.Copy((IntPtr)UnsafeBuffer, array, 0, _length);
            return array;
        }
        
        public bool Equals(ByteSpan buffer)
        {
            return BufferInternal.MemoryEqual(_data, _length, buffer._data, buffer._length);
        }
    }
}