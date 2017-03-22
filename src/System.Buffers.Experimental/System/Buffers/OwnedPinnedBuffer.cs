// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public class OwnedPinnedBuffer<T> : OwnedBuffer<T>
    {
        private GCHandle _handle;
        IntPtr _pointer;
        T[] _array;

        public override int Length => _array.Length;

        public override Span<T> Span => _array;

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

        public T[] Array => _array;

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
        {
            _handle = handle;
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

        public override Span<T> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return Span.Slice(index, length);
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = _pointer.ToPointer();
            return true;
        }

        protected override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(_array);
            return true;
        }
    }
}

