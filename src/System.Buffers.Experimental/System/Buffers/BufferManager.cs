// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Runtime.CompilerServices;

namespace System.Buffers.Pools
{
    public unsafe sealed partial class NativeBufferPool : BufferPool
    {
        internal sealed class BufferManager : ReferenceCountedBuffer<byte>
        {
            public BufferManager(NativeBufferPool pool, IntPtr memory, int length)
            {
                _pool = pool;
                _pointer = memory;
                _length = length;
            }

            public IntPtr Pointer => _pointer;

            public override int Length => _length;

            public override Span<byte> AsSpan(int index, int length)
            {
                if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(BufferManager));
                return new Span<byte>(_pointer.ToPointer(), _length).Slice(index, length);
            }

            protected override void Dispose(bool disposing)
            {
                _pool.Return(this);
                base.Dispose(disposing);
            }

            protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
            {
                arraySegment = default;
                return false;
            }

            public override BufferHandle Pin(int index = 0)
            {
                Retain();
                return new BufferHandle(this, Unsafe.Add<byte>(_pointer.ToPointer(), index));
            }

            private readonly NativeBufferPool _pool;
            IntPtr _pointer;
            int _length;
        }
    }
}
