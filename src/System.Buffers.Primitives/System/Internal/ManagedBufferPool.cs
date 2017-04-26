// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;

namespace System.Buffers.Internal
{
    internal sealed class ManagedBufferPool : BufferPool
    {
        readonly static ManagedBufferPool s_shared = new ManagedBufferPool();

        public static ManagedBufferPool Shared
        {
            get
            {
                return s_shared;
            }
        }

        public override OwnedBuffer<byte> Rent(int minimumBufferSize)
        {
            return new ArrayPoolMemory(minimumBufferSize);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private class ArrayPoolMemory : ReferenceCountedBuffer<byte>
        {
            public ArrayPoolMemory(int size)
            {
                _array = ArrayPool<byte>.Shared.Rent(size);
            }

            public override int Length => _array.Length;

            public override Span<byte> Span
            {
                get
                {
                    if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ManagedBufferPool));
                    return _array;
                }
            }

            protected override void Dispose(bool disposing)
            {
                ArrayPool<byte>.Shared.Return(_array);
                base.Dispose(disposing);
            }

            protected internal override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ManagedBufferPool));
                buffer = new ArraySegment<byte>(_array);
                return true;
            }

            protected internal override unsafe bool TryGetPointerAt(int index, out void* pointer)
            {
                pointer = null;
                return false;
            }

            byte[] _array;
        }
    }
}