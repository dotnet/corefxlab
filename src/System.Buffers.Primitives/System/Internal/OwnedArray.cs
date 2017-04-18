// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal sealed class OwnedArray<T> : OwnedBuffer<T>
    {
        public OwnedArray(int length)
        {
            _array = new T[length];
        }

        public OwnedArray(T[] array)
        {
            _array = array;
        }

        public OwnedArray(ArraySegment<T> segment)
        {
            _array = segment.AsSpan().ToArray();
        }

        public static implicit operator T[] (OwnedArray<T> owner)
        {
            return owner._array;
        }

        public static implicit operator OwnedArray<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        public static implicit operator OwnedArray<T>(ArraySegment<T> segment)
        {
            return new OwnedArray<T>(segment);
        }

        public override int Length => _array.Length;

        public override Span<T> Span
        {
            get
            {
                if (IsDisposed) ThrowObjectDisposed();
                return _array;
            }
        }

        public override Span<T> GetSpan(int index, int length)
        {
            return Span.Slice(index, length);
        }

        protected internal override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        T[] _array;
    }
}