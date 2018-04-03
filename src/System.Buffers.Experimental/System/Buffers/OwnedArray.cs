﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    public class OwnedArray<T> : MemoryManager<T>
    {
        T[] _array;
        int _referenceCount;

        public OwnedArray(int length)
        {
            _array = new T[length];
        }

        public OwnedArray(T[] array)
        {
            if (array == null) ThrowArgumentNullException(nameof(array));
            _array = array;
        }

        public static implicit operator OwnedArray<T>(T[] array) => new OwnedArray<T>(array);

        public override int Length => _array.Length;

        public override Span<T> GetSpan()
        {
            if (IsDisposed) ThrowObjectDisposedException(nameof(OwnedArray<T>));
            return new Span<T>(_array);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            unsafe
            {
                Retain();
                if ((uint)elementIndex > (uint)_array.Length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                void* pointer = Unsafe.Add<T>((void*)handle.AddrOfPinnedObject(), elementIndex);
                return new MemoryHandle(pointer, handle, this);
            }
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed) ThrowObjectDisposedException(nameof(OwnedArray<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _array = null;
        }

        public void Retain()
        {
            if (IsDisposed) ThrowObjectDisposedException(nameof(OwnedArray<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public override void Unpin()
        {
            int newRefCount = Interlocked.Decrement(ref _referenceCount);
            if (newRefCount < 0) ThrowInvalidOperationException();
            if (newRefCount == 0)
            {
                OnNoReferences();
            }
        }

        protected virtual void OnNoReferences()
        {
        }

        protected bool IsRetained => _referenceCount > 0;

        public bool IsDisposed => _array == null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowObjectDisposedException(string objectName)
            => throw new ObjectDisposedException(objectName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowInvalidOperationException()
            => throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentNullException(string argumentName)
    => throw new ArgumentNullException(argumentName);
    }
}
