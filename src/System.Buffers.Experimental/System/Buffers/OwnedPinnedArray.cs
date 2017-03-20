// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    // This is to support secnarios today covered by Buffer<T> in corefxlab
    public class OwnedPinnedBuffer<T> : OwnedBuffer<T>
    {
        private GCHandle _handle;

        public unsafe OwnedPinnedBuffer(T[] array, void* pointer, GCHandle handle = default(GCHandle)) :
            base(array, 0, array.Length, new IntPtr(pointer))
        {
            var computedPointer = new IntPtr(Unsafe.AsPointer(ref Array[0]));
            if (computedPointer != new IntPtr(pointer))
            {
                throw new InvalidOperationException();
            }
            _handle = handle;
        }

        public unsafe OwnedPinnedBuffer(T[] array) : this(array, GCHandle.Alloc(array, GCHandleType.Pinned))
        { }

        private OwnedPinnedBuffer(T[] array, GCHandle handle) : base(array, 0, array.Length, handle.AddrOfPinnedObject())
        {
            _handle = handle;
        }

        public static implicit operator OwnedPinnedBuffer<T>(T[] array)
        {
            return new OwnedPinnedBuffer<T>(array);
        }

        public new unsafe byte* Pointer => (byte*)base.Pointer.ToPointer();
        public new T[] Array => base.Array;

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
            base.Dispose(disposing);
        }
    }
}

