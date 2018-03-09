// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers.Tests
{
    public class CustomMemoryForTest<T> : OwnedMemory<T>
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

        public override int Length => _array.Length;

        public override bool IsDisposed => _disposed;

        protected override bool IsRetained => _referenceCount > 0;

        public override Span<T> Span
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(CustomMemoryForTest<T>));
                return new Span<T>(_array, 0, _array.Length);
            }
        }

        public override MemoryHandle Pin(int byteOffset = 0)
        {
            unsafe
            {
                Retain();
                if (byteOffset != 0 && (((uint)byteOffset) - 1) / Unsafe.SizeOf<T>() >= _array.Length) throw new ArgumentOutOfRangeException(nameof(byteOffset));
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                void* pointer = Unsafe.Add<byte>((void*)handle.AddrOfPinnedObject(), byteOffset);
                return new MemoryHandle(this, pointer, handle);
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

        public override void Retain()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(CustomMemoryForTest<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public override bool Release()
        {
            while (true)
            {
                int currentCount = Volatile.Read(ref _referenceCount);
                if (currentCount <= 0)
                    throw new InvalidOperationException();
                if (Interlocked.CompareExchange(ref _referenceCount, currentCount - 1, currentCount) == currentCount)
                {
                    if (currentCount == 1)
                    {
                        Dispose();
                        _noReferencesCalledCount++;
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}

