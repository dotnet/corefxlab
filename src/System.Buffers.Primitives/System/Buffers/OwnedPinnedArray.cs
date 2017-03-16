// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    // This is to support secnarios today covered by Memory<T> in corefxlab
    public class OwnedPinnedArray<T> : OwnedMemory<T>
    {
        private GCHandle _handle;

        public unsafe OwnedPinnedArray(T[] array, void* pointer, GCHandle handle = default(GCHandle)) :
            base(array, 0, array.Length, new IntPtr(pointer))
        {
            var computedPointer = new IntPtr(Unsafe.AsPointer(ref Array[0]));
            if (computedPointer != new IntPtr(pointer)) {
                throw new InvalidOperationException();
            }
            _handle = handle;
        }

        public unsafe OwnedPinnedArray(T[] array) : this(array, GCHandle.Alloc(array, GCHandleType.Pinned))
        { }

        private OwnedPinnedArray(T[] array, GCHandle handle) : base(array, 0, array.Length, handle.AddrOfPinnedObject())
        {
            _handle = handle;
        }

        public static implicit operator OwnedPinnedArray<T>(T[] array)
        {
            return new OwnedPinnedArray<T>(array);
        }

        public new unsafe byte* Pointer => (byte*)base.Pointer.ToPointer();
        public new T[] Array => base.Array;

        public unsafe static implicit operator IntPtr(OwnedPinnedArray<T> owner)
        {
            return new IntPtr(owner.Pointer);
        }

        public static implicit operator T[] (OwnedPinnedArray<T> owner)
        {
            return owner.Array;
        }

        protected override void Dispose(bool disposing)
        {
            if (_handle.IsAllocated) {
                _handle.Free();
            }
            base.Dispose(disposing);
        }
    }
}

