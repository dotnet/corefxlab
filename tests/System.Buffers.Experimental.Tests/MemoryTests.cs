// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Buffers.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void SimpleTests()
        {            
            using(var owned = new OwnedNativeBuffer(1024)) {
                var span = owned.AsSpan();
                span[10] = 10;
                unsafe { Assert.Equal(10, owned.Pointer[10]); }

                var memory = owned.Buffer;
                var array = memory.ToArray();
                Assert.Equal(owned.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }

            using (OwnedPinnedBuffer<byte> owned = new byte[1024]) {
                var span = owned.AsSpan();
                span[10] = 10;
                Assert.Equal(10, owned.Array[10]);

                unsafe { Assert.Equal(10, owned.Pointer[10]); }

                var memory = owned.Buffer;
                var array = memory.ToArray();
                Assert.Equal(owned.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }
        }

        [Fact]
        public void NativeMemoryLifetime()
        {
            var owner = new OwnedNativeBuffer(1024);
            TestLifetime(owner);
        }

        [Fact]
        public unsafe void PinnedArrayMemoryLifetime()
        {
            var bytes = new byte[1024];
            fixed (byte* pBytes = bytes) {
                var owner = new OwnedPinnedBuffer<byte>(bytes, pBytes);
                TestLifetime(owner);            
            }
        }

        static void TestLifetime(OwnedBuffer<byte> owned)
        {
            Buffer<byte> copyStoredForLater;
            try {
                Buffer<byte> memory = owned.Buffer;
                Buffer<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                var r = memorySlice.Retain();
                try {
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; premature dispose check fires
                        owned.Dispose();
                    });
                }
                finally {
                    r.Dispose(); // release reservation
                }
            }
            finally {
                owned.Dispose(); // can finish dispose with no exception
            }
            Assert.Throws<ObjectDisposedException>(() => {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }

        [Fact]
        public void AutoDispose()
        {
            OwnedBuffer<byte> owned = new AutoPooledBuffer(1000);
            owned.Retain();
            var memory = owned.Buffer;
            Assert.Equal(false, owned.IsDisposed);
            var reservation = memory.Retain();
            Assert.Equal(false, owned.IsDisposed);
            owned.Release();
            Assert.Equal(false, owned.IsDisposed);
            reservation.Dispose();
            Assert.Equal(true, owned.IsDisposed);
        }

        [Fact]
        public void OnNoReferencesTest()
        {
            var owned = new CustomBuffer<byte>(255);
            var memory = owned.Buffer;
            Assert.Equal(0, owned.OnNoRefencesCalledCount);
            
            using (memory.Retain())
            {
                Assert.Equal(0, owned.OnNoRefencesCalledCount);
            }
            Assert.Equal(1, owned.OnNoRefencesCalledCount);
        }

        [Fact(Skip = "This needs to be fixed and re-enabled or removed.")]
        public void RacyAccess()
        {
            for (int k = 0; k < 1000; k++)
            {
                var owners = new OwnedBuffer<byte>[128];
                var memories = new Buffer<byte>[owners.Length];
                var reserves = new BufferHandle[owners.Length];
                var disposeSuccesses = new bool[owners.Length];
                var reserveSuccesses = new bool[owners.Length];

                for (int i = 0; i < owners.Length; i++)
                {
                    var array = new byte[1024];
                    owners[i] = array;
                    memories[i] = owners[i].Buffer;
                }

                var dispose_task = Task.Run(() => {
                    for (int i = 0; i < owners.Length; i++)
                    {
                        try
                        {
                            owners[i].Dispose();
                            disposeSuccesses[i] = true;
                        }
                        catch (InvalidOperationException)
                        {
                            disposeSuccesses[i] = false;
                        }
                    }
                });

                var reserve_task = Task.Run(() => {
                    for (int i = owners.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            reserves[i] = memories[i].Retain();
                            reserveSuccesses[i] = true;
                        }
                        catch (ObjectDisposedException)
                        {
                            reserveSuccesses[i] = false;
                        }
                    }
                });

                Task.WaitAll(reserve_task, dispose_task);

                for (int i = 0; i < owners.Length; i++)
                {
                    Assert.False(disposeSuccesses[i] && reserveSuccesses[i]);
                }
            }
        }
    }

    class CustomBuffer<T> : OwnedBuffer<T>
    {
        bool _disposed;
        int _referenceCount;
        int _noReferencesCalledCount;
        T[] _array;

        public CustomBuffer(int size)
        {
            _array = new T[size];
        }

        public int OnNoRefencesCalledCount => _noReferencesCalledCount;

        public override int Length => _array.Length;

        public override bool IsDisposed => _disposed;

        protected override bool IsRetained => _referenceCount > 0;

        public override Span<T> AsSpan(int index, int length)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            return new Span<T>(_array, index, length);
        }

        public override Span<T> AsSpan()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            return new Span<T>(_array, 0, _array.Length);
        }

        public override BufferHandle Pin()
        {
            unsafe
            {
                Retain();
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                return new BufferHandle(this, (void*)handle.AddrOfPinnedObject(), handle);
            }
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            _array = null;
        }

        public override void Retain()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public override bool Release()
        {
            int newRefCount = Interlocked.Decrement(ref _referenceCount);
            if (newRefCount < 0) throw new InvalidOperationException();
            if (newRefCount == 0)
            {
               _noReferencesCalledCount++;
               return false;
            }
            return true;
        }
    }

    class AutoDisposeBuffer<T> : ReferenceCountedBuffer<T>
    {
        public AutoDisposeBuffer(T[] array)
        {
            _array = array;
        }

        public override int Length => _array.Length;

        public override Span<T> AsSpan(int index, int length)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(AutoDisposeBuffer<T>));
            return new Span<T>(_array, index, length);
        }

        protected override void Dispose(bool disposing)
        {
            _array = null;
            base.Dispose(disposing);
        }

        protected override void OnNoReferences()
        {
            Dispose();
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(AutoDisposeBuffer<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        public override BufferHandle Pin()
        {
            unsafe
            {
                Retain();
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                return new BufferHandle(this, (void*)handle.AddrOfPinnedObject(), handle);
            }
        }

        protected T[] _array;
    }

    class AutoPooledBuffer : AutoDisposeBuffer<byte>
    {
        public AutoPooledBuffer(int length) : base(ArrayPool<byte>.Shared.Rent(length))
        {
        }

        protected override void Dispose(bool disposing)
        {
            var array = _array;
            if (array != null)
            {
                ArrayPool<byte>.Shared.Return(array);
            }
            _array = null;
            base.Dispose(disposing);
        }
    }
}
