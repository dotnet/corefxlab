// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;

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
            var buffer = new ArrayPoolBuffer(minimumBufferSize);
            return buffer;
        }

        protected override void Dispose(bool disposing)
        {
        }

        private sealed class ArrayPoolBuffer : OwnedBuffer<byte>
        {
            byte[] _array;
            bool _disposed;
            int _referenceCount;
            
            public ArrayPoolBuffer(int size)
            {
                _array = ArrayPool<byte>.Shared.Rent(size);
            }

            public override int Length => _array.Length;

            public override bool IsDisposed => _disposed;

            public override bool IsRetained => _referenceCount > 0;

            public override Span<byte> AsSpan(int index, int length)
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ArrayPoolBuffer));
                return new Span<byte>(_array, index, length);
            }

            protected override void Dispose(bool disposing)
            {
                _disposed = true;
                if (_array != null)
                {
                    ArrayPool<byte>.Shared.Return(_array);
                    _array = null;
                }
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

            public override void Retain()
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ArrayPoolBuffer));
                Interlocked.Increment(ref _referenceCount);
            }

            public override void Release()
            {
                if (!IsRetained) BufferPrimitivesThrowHelper.ThrowInvalidOperationException();
                if(Interlocked.Decrement(ref _referenceCount) <= 0)
                {
                    Dispose();
                }
            }
        }
    }
}
