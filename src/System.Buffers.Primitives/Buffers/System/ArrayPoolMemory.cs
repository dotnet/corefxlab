// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Pools
{
    internal sealed partial class ManagedBufferPool : BufferPool
    {
        private class ArrayPoolMemory : OwnedBuffer<byte>
        {
            byte[] _array;
            public ArrayPoolMemory(int size) : base()
            {
                _array = ArrayPool<byte>.Shared.Rent(size);
            }

            protected ArrayPoolMemory()
            {
            }

            public override int Length => _array.Length;

            public override Span<byte> Span => _array;

            public override Span<byte> GetSpan(int index, int length)
            {
                if (IsDisposed) ThrowObjectDisposed();
                return new Span<byte>(_array, index, length);
            }

            protected override void Dispose(bool disposing)
            {
                ArrayPool<byte>.Shared.Return(_array);
                base.Dispose(disposing);
            }

            protected internal override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
            {
                buffer = new ArraySegment<byte>(_array);
                return true;
            }

            protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
            {
                pointer = null;
                return false;

            }
        }
    }
}
