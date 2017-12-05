// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers.Internal
{
    internal sealed class ArrayMemoryPool<T> : MemoryPool<T>
    {
        const int DefaultSize = 4096;

        readonly static ArrayMemoryPool<T> s_shared = new ArrayMemoryPool<T>();

        public static ArrayMemoryPool<T> Shared => s_shared;

        public override int MaxBufferSize => 1024 * 1024 * 1024;

        public override OwnedMemory<T> Rent(int minimumBufferSize = DefaultSize)
        {
            if (minimumBufferSize == 0) minimumBufferSize = 32;
            else if (minimumBufferSize == AnySize) minimumBufferSize = DefaultSize;
            else if (minimumBufferSize > MaxBufferSize) throw new ArgumentOutOfRangeException(nameof(minimumBufferSize));
            return new ArrayPoolMemory(minimumBufferSize);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private sealed class ArrayPoolMemory : OwnedMemory<T>
        {
            T[] _array;
            bool _disposed;
            int _referenceCount;
            
            public ArrayPoolMemory(int size)
            {
                _array = ArrayPool<T>.Shared.Rent(size);
            }

            public override int Length => _array.Length;

            public override bool IsDisposed => _disposed;

            protected override bool IsRetained => _referenceCount > 0;

            public override Span<T> Span
            {
                get
                {
                    if (IsDisposed) ThrowObjectDisposedException(nameof(ArrayPoolMemory));
                    return _array;
                }
            }

            protected override void Dispose(bool disposing)
            {
                var array = Interlocked.Exchange(ref _array, null);
                if (array != null)  {
                    _disposed = true;
                    ArrayPool<T>.Shared.Return(array);
                }
            }

            protected override bool TryGetArray(out ArraySegment<T> arraySegment)
            {
                if (IsDisposed) ThrowObjectDisposedException(nameof(ArrayMemoryPool<T>));
                arraySegment = new ArraySegment<T>(_array);
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
                if (IsDisposed) ThrowObjectDisposedException(nameof(ArrayPoolMemory));
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

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static void ThrowObjectDisposedException(string objectName)
                => throw new ObjectDisposedException(objectName);
        }
    }
}
