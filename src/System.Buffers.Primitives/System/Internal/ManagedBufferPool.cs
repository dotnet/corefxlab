// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public override OwnedMemory<byte> Rent(int minimumBufferSize)
        {
            var buffer = new ArrayPoolBuffer(minimumBufferSize);
            return buffer;
        }

        protected override void Dispose(bool disposing)
        {
        }

        private sealed class ArrayPoolBuffer : OwnedMemory<byte>
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

            protected override bool IsRetained => _referenceCount > 0;

            public override Span<byte> AsSpan()
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ArrayPoolBuffer));
                return new Span<byte>(_array);
            }

            protected override void Dispose(bool disposing)
            {
                var array = Interlocked.Exchange(ref _array, null);
                if (array != null)  {
                    _disposed = true;
                    ArrayPool<byte>.Shared.Return(array);
                }
            }

            protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ManagedBufferPool));
                arraySegment = new ArraySegment<byte>(_array);
                return true;
            }

            public override MemoryHandle Pin()
            {
                unsafe
                {
                    Retain(); // this checks IsDisposed
                    var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                    return new MemoryHandle(this, (void*)handle.AddrOfPinnedObject(), handle);
                }
            }

            public override void Retain()
            {
                if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(ArrayPoolBuffer));
                Interlocked.Increment(ref _referenceCount);
            }

            public override bool Release()
            {
                int newRefCount = Interlocked.Decrement(ref _referenceCount);
                if (newRefCount < 0) throw new InvalidOperationException();
                if (newRefCount == 0) 
                {
                    Dispose();
                    return false;
                }
                return true;
            }
        }
    }
}
