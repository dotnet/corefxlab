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

        public unsafe override Span<byte> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            if (index > _length || length > (_length - index)) throw new IndexOutOfRangeException();
            return new Span<byte>((byte*)_pointer.ToPointer() + index, length);
        }

        public override BufferHandle Pin(int index = 0)
        {
            throw new NotImplementedException();
        }

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = default(ArraySegment<byte>);
            return false;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = _pointer.ToPointer();
            return true;
        }

        public unsafe byte* Pointer => (byte*)_pointer.ToPointer();

        public override int Length => _length;

        public unsafe override Span<byte> Span => new Span<byte>(_pointer.ToPointer(), _length);

        int _length;
        IntPtr _pointer;
    }
}
