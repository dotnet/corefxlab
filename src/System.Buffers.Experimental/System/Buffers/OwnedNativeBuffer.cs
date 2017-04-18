// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers
{
    public class OwnedNativeBuffer : OwnedBuffer<byte>
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

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = default(ArraySegment<byte>);
            return false;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(nameof(OwnedNativeBuffer));
            pointer = _pointer.ToPointer();
            return true;
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

        public override int Length => _length;
        
        public unsafe override Span<byte> Span
        {
            get
            {
                if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(nameof(OwnedNativeBuffer));
                return new Span<byte>(_pointer.ToPointer(), _length);
            }
        }

        int _length;
        IntPtr _pointer;
    }
}
