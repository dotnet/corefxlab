// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Native
{
    public class OwnedNativeBuffer : ReferenceCountedMemory<byte>
    {
        public OwnedNativeBuffer(int length) : this(length, Marshal.AllocHGlobal(length))
        { }

        public OwnedNativeBuffer(int length, IntPtr address)
        {
            _length = length;
            _pointer = address;
        }

        public static implicit operator IntPtr(OwnedNativeBuffer owner)
        {
            return owner._pointer;
        }

        ~OwnedNativeBuffer()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (_pointer != null)
            {
                Marshal.FreeHGlobal(_pointer);
            }
            base.Dispose(disposing);
            _pointer = IntPtr.Zero;
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            Retain();
            if (elementIndex < 0 || elementIndex > _length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
            unsafe
            {
                return new MemoryHandle(Unsafe.Add<byte>(_pointer.ToPointer(), elementIndex), default, this);
            }
        }
        protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
        {
            arraySegment = default;
            return false;
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();
        
        public unsafe override Span<byte> GetSpan()
        {
            if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(OwnedNativeBuffer));
            return new Span<byte>(_pointer.ToPointer(), _length);
        }

        public override void Unpin()
        {
            Release();
        }

        int _length;
        IntPtr _pointer;
    }
}
