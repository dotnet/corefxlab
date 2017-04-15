// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers.Pools
{
    public unsafe sealed partial class NativeBufferPool : BufferPool
    {
        internal sealed class BufferManager : OwnedBuffer<byte>
        {
            public BufferManager(NativeBufferPool pool, IntPtr memory, int length)
            {
                _pool = pool;
                _pointer = memory;
                _length = length;
            }

            public IntPtr Pointer => _pointer;

            public override int Length => _length;

            public override Span<byte> Span => new Span<byte>(_pointer.ToPointer(), _length);

            public override Span<byte> GetSpan(int index, int length)
            {
                if (IsDisposed) ThrowObjectDisposed();
                return Span.Slice(0, length);
            }

            public override BufferHandle Pin(int index = 0)
            {
                return BufferHandle.Create(this, index);
            }

            protected override void Dispose(bool disposing)
            {
                _pool.Return(this);
                base.Dispose(disposing);
            }

            protected internal override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }

            protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
            {
                pointer = _pointer.ToPointer();
                return true;
            }

            private readonly NativeBufferPool _pool;
            IntPtr _pointer;
            int _length;
        }
    }
}
