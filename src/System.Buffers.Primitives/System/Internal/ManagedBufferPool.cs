// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.InteropServices;

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

            public override Span<byte> AsSpan(int index, int length)
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ManagedBufferPool));
                return new Span<byte>(_array, index, length);
            }

            protected override void Dispose(bool disposing)
            {
                if (_array != null)
                {
                    ArrayPool<byte>.Shared.Return(_array);
                    _array = null;
                }
                base.Dispose(disposing);
            }

            protected internal override bool TryGetArray(out ArraySegment<byte> buffer)
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ManagedBufferPool));
                buffer = new ArraySegment<byte>(_array);
                return true;
            }

            public override BufferHandle Pin(int index = 0)
            {
                unsafe
                {
                    Retain(); // this checks IsDisposed
                    var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                    var pointer = Add((void*)handle.AddrOfPinnedObject(), index);
                    return new BufferHandle(this, pointer, handle);
                }
            }

            byte[] _array;
        }
    }
}
