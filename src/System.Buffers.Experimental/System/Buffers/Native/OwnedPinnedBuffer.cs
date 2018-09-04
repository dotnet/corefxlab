// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Native
{
    // This is to support secnarios today covered by Memory<T> in corefxlab
    public class OwnedPinnedBuffer<T> : ReferenceCountedMemory<T>
    {
        public unsafe OwnedPinnedBuffer(T[] array, void* pointer, GCHandle handle = default)
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

        public static implicit operator OwnedPinnedBuffer<T>(T[] array) => new OwnedPinnedBuffer<T>(array);

        public unsafe static implicit operator IntPtr(OwnedPinnedBuffer<T> owner) => new IntPtr(owner.Pointer);

        public static implicit operator T[] (OwnedPinnedBuffer<T> owner) => owner.Array;

        public override Span<T> GetSpan()
        {
            if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(OwnedPinnedBuffer<T>));
            return new Span<T>(_array);
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

        public T[] Array => _array;

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

        public unsafe override MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex != 0 && (((uint)elementIndex) - 1) / Unsafe.SizeOf<T>() >= _array.Length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
            return new MemoryHandle(Unsafe.Add<T>(_pointer.ToPointer(), elementIndex), default, this);
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(OwnedPinnedBuffer<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        public override void Unpin()
        {
            // no-op
        }

        private GCHandle _handle;
        IntPtr _pointer;
        T[] _array;
    }
}

