// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    // This is to support secnarios today covered by Buffer<T> in corefxlab
    public class OwnedPinnedBuffer<T> : OwnedBuffer<T>
    {
        public unsafe OwnedPinnedBuffer(T[] array, void* pointer, GCHandle handle = default(GCHandle))
        {
            var computedPointer = new IntPtr(Unsafe.AsPointer(ref array[0]));
            if (computedPointer != new IntPtr(pointer))
            {
                throw new InvalidOperationException();
            }
            _handle = handle;
            _pointer = new IntPtr(pointer);
            _array = array;
        }

        public unsafe OwnedPinnedBuffer(T[] array) : this(array, GCHandle.Alloc(array, GCHandleType.Pinned))
        { }

        private unsafe OwnedPinnedBuffer(T[] array, GCHandle handle) : this(array, handle.AddrOfPinnedObject().ToPointer(), handle)
        { }

        public static implicit operator OwnedPinnedBuffer<T>(T[] array)
        {
            return new OwnedPinnedBuffer<T>(array);
        }

        public override int Length => _array.Length;

        public override Span<T> Span
        {
            get
            {
                if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(nameof(OwnedPinnedBuffer<T>));
                return _array;
            }
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

        public T[] Array => _array;

        public unsafe static implicit operator IntPtr(OwnedPinnedBuffer<T> owner)
        {
            return new IntPtr(owner.Pointer);
        }

        public static implicit operator T[] (OwnedPinnedBuffer<T> owner)
        {
            return owner.Array;
        }

        protected override void Dispose(bool disposing)
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
            _array = null;
            _pointer = IntPtr.Zero;
            base.Dispose(disposing);
        }

        public override BufferHandle Pin(int index = 0)
        {
            return BufferHandle.Create(this, index, _handle);
        }

        protected override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(nameof(OwnedPinnedBuffer<T>));
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(nameof(OwnedPinnedBuffer<T>));
            pointer = _pointer.ToPointer();
            return true;
        }

        private GCHandle _handle;
        IntPtr _pointer;
        T[] _array;
    }
}

