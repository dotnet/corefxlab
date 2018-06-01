// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers.Tests
{
    public class CustomMemoryForTest<T> : MemoryManager<T>
    {
        private bool _disposed;
        private int _referenceCount;
        private int _noReferencesCalledCount;
        private T[] _array;

        public CustomMemoryForTest(T[] array)
        {
            _array = array;
            _referenceCount = 1;
        }

        public int OnNoRefencesCalledCount => _noReferencesCalledCount;


        public bool IsDisposed => _disposed;

        protected bool IsRetained => _referenceCount > 0;

        public override Span<T> GetSpan()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(CustomMemoryForTest<T>));
            return new Span<T>(_array, 0, _array.Length);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            unsafe
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(CustomMemoryForTest<T>));
                Interlocked.Increment(ref _referenceCount);
                try
                {
                    if ((uint)elementIndex > (uint)(_array.Length))
                    {
                        throw new ArgumentOutOfRangeException(nameof(elementIndex));
                    }

                    var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                    return new MemoryHandle(Unsafe.Add<T>((void*)handle.AddrOfPinnedObject(), elementIndex), handle, this);
                }
                catch
                {
                    Unpin();
                    throw;
                }
            }
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(CustomMemoryForTest<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _array = null;
            }

            _disposed = true;
            
        }

        public override void Unpin()
        {
            int newRefCount = Interlocked.Decrement(ref _referenceCount);

            if (newRefCount < 0)
                throw new InvalidOperationException();

            if (newRefCount == 0)
            {
                _noReferencesCalledCount++;
            }
        }
    }
}

