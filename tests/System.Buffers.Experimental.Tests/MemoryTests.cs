// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers.Native;
using System.Runtime.CompilerServices;
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
            using (var owned = new OwnedNativeBuffer(1024))
            {
                var span = owned.GetSpan();
                span[10] = 10;
                unsafe { Assert.Equal(10, owned.Pointer[10]); }

                var memory = owned.Memory;
                var array = memory.ToArray();
                Assert.Equal(memory.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Span.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }

            using (OwnedPinnedBuffer<byte> owned = new byte[1024])
            {
                var span = owned.GetSpan();
                span[10] = 10;
                Assert.Equal(10, owned.Array[10]);

                unsafe { Assert.Equal(10, owned.Pointer[10]); }

                var memory = owned.Memory;
                var array = memory.ToArray();
                Assert.Equal(memory.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Span.Slice(10, 20).CopyTo(copy);
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
            fixed (byte* pBytes = bytes)
            {
                var owner = new OwnedPinnedBuffer<byte>(bytes, pBytes);
                TestLifetime(owner);
            }
        }

        static void TestLifetime(MemoryManager<byte> owned)
        {
            Memory<byte> copyStoredForLater;
            try
            {
                Memory<byte> memory = owned.Memory;
                Memory<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                MemoryHandle r = memorySlice.Pin();
                r.Dispose(); // release reservation
            }
            finally
            {
                ((IDisposable)owned).Dispose(); // can finish dispose with no exception
            }
            Assert.Throws<ObjectDisposedException>(() =>
            {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }

        [Fact]
        public void AutoDispose()
        {
            MemoryManager<byte> owned = new AutoPooledBuffer(1000);
            owned.Pin();
            var memory = owned.Memory;
            var reservation = memory.Pin();
            owned.Unpin();
            reservation.Dispose();
        }

        [Fact]
        public void OnNoReferencesTest()
        {
            var owned = new CustomBuffer<byte>(255);
            var memory = owned.Memory;
            Assert.Equal(0, owned.OnNoRefencesCalledCount);

            using (memory.Pin())
            {
                Assert.Equal(0, owned.OnNoRefencesCalledCount);
            }
            owned.Release();
            Assert.Equal(1, owned.OnNoRefencesCalledCount);
        }

        [Fact(Skip = "This needs to be fixed and re-enabled or removed.")]
        public void RacyAccess()
        {
            for (int k = 0; k < 1000; k++)
            {
                var owners = new IMemoryOwner<byte>[128];
                var memories = new Memory<byte>[owners.Length];
                var reserves = new MemoryHandle[owners.Length];
                var disposeSuccesses = new bool[owners.Length];
                var reserveSuccesses = new bool[owners.Length];

                for (int i = 0; i < owners.Length; i++)
                {
                    var array = new byte[1024];
                    owners[i] = new OwnedArray<byte>(array);
                    memories[i] = owners[i].Memory;
                }

                var dispose_task = Task.Run(() =>
                {
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

                var reserve_task = Task.Run(() =>
                {
                    for (int i = owners.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            reserves[i] = memories[i].Pin();
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

    class CustomBuffer<T> : MemoryManager<T>
    {
        bool _disposed;
        int _referenceCount;
        int _noReferencesCalledCount;
        T[] _array;

        public CustomBuffer(int size)
        {
            _array = new T[size];
            _referenceCount = 1;
        }

        public int OnNoRefencesCalledCount => _noReferencesCalledCount;

        public bool IsDisposed => _disposed;

        protected bool IsRetained => _referenceCount > 0;

        public override Span<T> GetSpan()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            return new Span<T>(_array);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            unsafe
            {
                Retain();
                if ((uint)elementIndex > (uint)_array.Length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                void* pointer = Unsafe.Add<T>((void*)handle.AddrOfPinnedObject(), elementIndex);
                return new MemoryHandle(pointer, handle, this);
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

        public void Retain()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(CustomBuffer<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public bool Release()
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
                        ((IDisposable)this).Dispose();
                        _noReferencesCalledCount++;
                        return false;
                    }
                    return true;
                }
            }
        }

        public override void Unpin()
        {
            Release();
        }
    }

    class AutoDisposeBuffer<T> : ReferenceCountedMemory<T>
    {
        public AutoDisposeBuffer(T[] array)
        {
            _array = array;
        }

        public override Span<T> GetSpan()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(AutoDisposeBuffer<T>));
            return new Span<T>(_array);
        }

        protected override void Dispose(bool disposing)
        {
            _array = null;
            base.Dispose(disposing);
        }

        protected override void OnNoReferences()
        {
            Dispose(true);
        }

        protected override bool TryGetArray(out ArraySegment<T> arraySegment)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(AutoDisposeBuffer<T>));
            arraySegment = new ArraySegment<T>(_array);
            return true;
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            unsafe
            {
                Retain();
                if ((uint)elementIndex > (uint)_array.Length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                void* pointer = Unsafe.Add<T>((void*)handle.AddrOfPinnedObject(), elementIndex);
                return new MemoryHandle(pointer, handle, this);
            }
        }

        public override void Unpin()
        {
            Release();
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
