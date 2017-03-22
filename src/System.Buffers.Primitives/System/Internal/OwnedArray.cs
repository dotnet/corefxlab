// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal sealed class OwnedArray<T> : OwnedBuffer<T>
    {
        T[] _array;

        public override int Length => _array.Length;

        public override Span<T> Span => _array;

        public override Span<T> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return new Span<T>(_array, index, length);
        }

        public static implicit operator T[](OwnedArray<T> owner) {
            return owner._array;
        }

        public static implicit operator OwnedArray<T>(T[] array) {
            return new OwnedArray<T>(array);
        }

        public OwnedArray(int length) 
        {
            _array = new T[length];
        }

        public OwnedArray(T[] array)
        {
            _array = array;
        }

        protected override void Dispose(bool disposing)
        {
            _array = null;
            base.Dispose(disposing);
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
    }

    internal class OwnerEmptyMemory<T> : OwnedBuffer<T>
    {
        public readonly static OwnedBuffer<T> Shared = new OwnerEmptyMemory<T>();
        static readonly T[] s_empty = new T[0];

        public override int Length => 0;

        public override Span<T> Span => Span<T>.Empty;

        public override Span<T> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return new Span<T>(s_empty, index, length);
        }

        protected override void Dispose(bool disposing)
        {}

        protected internal override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(s_empty);
            return true;
        }

        protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }
    }
}