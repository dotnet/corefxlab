// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    // This is a naive implementation of span. We will get a much better one later.
    public struct Span<T>
    {
        T[] _array;
        int _index;
        int _length;

        public Span(int length)
        {
            _array = new T[length];
            _index = 0;
            _length = length;
        }

        public Span(T[] array, int index, int length)
        {
            _array = array;
            _index = index;
            _length = length;
        }

        public Span(T[] array, int index)
        {
            _array = array;
            _index = index;
            _length = array.Length - index;
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _array[_index + index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int index, int count)
        {
            return new Span<T>(_array, _index + index, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int index)
        {
            return Slice(index, _length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(Span<T> from)
        {
            return new ReadOnlySpan<T>(from._array, from._index, from._length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Span<T>(T[] from)
        {
            return new Span<T>(from, 0, from.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(T[] items, int itemsIndex, int itemsCount)
        {
            Precondition.Require(itemsCount + itemsIndex <= items.Length);
            Precondition.Require(itemsCount <= Length);
            if (items.Length <= 8)
            {
                for (int index = 0; index < itemsCount; index++)
                {
                    _array[_index + index] = items[itemsIndex + index];
                }
            }
            Array.Copy(items, itemsIndex, _array, _index, itemsCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(T[] items)
        {
            Precondition.Require(items.Length <= Length);
            if (items.Length <= 8)
            {
                for (int index = 0; index < items.Length; index++)
                {
                    _array[index + _index] = items[index];
                }
            }
            Array.Copy(items, 0, _array, _index, items.Length);
        }

        public void Set(Span<T> items)
        {
            Set(items._array, items._index, items._length);
        }

        public void Set(ReadOnlySpan<T> items)
        {
            Set(items._array, items._index, items._length);
        }

        public T[] CreateArray()
        {
            T[] array = new T[_length];
            var arrayIndex = 0;
            var start = _index;
            var count = _length;
            while (count > 0)
            {
                array[arrayIndex++] = _array[start++];
                count--;
            }
            return array;
        }

        internal unsafe ByteSpan BorrowDisposableByteSpan()
        {
            var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            var pinned = handle.AddrOfPinnedObject() + _index;
            var byteSpan = new ByteSpan(((byte*)pinned.ToPointer()), _length, handle);     
            return byteSpan;
        }
    }

    public static class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> Slice<T>(this T[] array, int index = 0)
        {
            return array.Slice(index, array.Length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> Slice<T>(this T[] array, int index, int count)
        {
            return new Span<T>(array, index, count);
        }

        public static string CreateString(this ReadOnlySpan<byte> utf16)
        {
            var bytes = utf16.CreateArray();
            var str = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
            return str;
        }

        public static string CreateString(this Span<byte> utf16)
        {
            var roSpan = (ReadOnlySpan<byte>)utf16;
            return CreateString(roSpan);
        }

        public static bool StartsWith(this ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            if (left.Length < right.Length) return false;
            for (int index = 0; index < right.Length; index++)
            {
                if (left[index] != right[index]) return false;
            }
            return true;
        }
    }
}
