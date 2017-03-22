// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers
{
    public class OwnedNativeBuffer : OwnedBuffer<byte>
    {
        IntPtr _pointer;
        int _length;

        public override int Length => _length;

        public unsafe override Span<byte> Span => new Span<byte>((byte*)_pointer.ToPointer(), _length);

        public OwnedNativeBuffer(int length) 
        {
            _pointer = Marshal.AllocHGlobal(length);
            _length = length;
        }

        public OwnedNativeBuffer(int length, IntPtr address)
        {
            _pointer = address;
            _length = length;
        }

        public static implicit operator IntPtr(OwnedNativeBuffer owner)
        {
            return owner._pointer;
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

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

        public override Span<byte> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            if (_length - index < length) throw new IndexOutOfRangeException();
            unsafe
            {
                return new Span<byte>((byte*)_pointer.ToPointer() + index, length);
            }
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = _pointer.ToPointer();
            return true;
        }

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = default(ArraySegment<byte>);
            return false;
        }
    }
}
