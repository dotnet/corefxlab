﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Threading;
using System.Buffers;

namespace System.Buffers.Internal
{
    internal sealed class OwnedArray<T> : ReferenceCountedBuffer<T>
    {
        T[] _array;

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

        public static implicit operator T[] (OwnedArray<T> owner) => owner._array;

        public static implicit operator OwnedArray<T>(T[] array) => new OwnedArray<T>(array);

        public static implicit operator OwnedArray<T>(ArraySegment<T> segment) => new OwnedArray<T>(segment);

        public override int Length => _array.Length;

        public override Span<T> Span
        {
            get
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(OwnedArray<T>));
                return _array;
            }
        }

        protected internal override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(OwnedArray<T>));
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _array = null;
        }
    }
}