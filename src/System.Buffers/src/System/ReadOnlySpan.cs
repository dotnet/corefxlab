// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    // This is a naive implementation of span. We will get a much better one later.
    public struct ReadOnlySpan<T>
    {
        internal T[] _array;
        internal int _index;
        internal int _length;

        public ReadOnlySpan(T[] array, int index, int count)
        {
            _array = array;
            _index = index;
            _length = count;
        }
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _length;
            }
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _array[_index + index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int index, int count)
        {
            return new Span<T>(_array, _index + index, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int index)
        {
            return Slice(index, _length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(T[] from)
        {
            return new ReadOnlySpan<T>(from, 0, from.Length);
        }

        public T[] CreateArray()
        {
            T[] array = new T[_length];
            var arrayIndex = 0;
            var start = _index;
            var count = _length;
            while(count > 0) {
                array[arrayIndex++] = _array[start++];
                count--;
            }
            return array;
        }
    }
}
