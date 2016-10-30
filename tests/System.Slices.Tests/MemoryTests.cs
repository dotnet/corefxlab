// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using Xunit;

namespace System.Slices.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void SimpleTests()
        {
            {
                OwnedArray<byte> owned = new byte[1024];
                var span = owned.Span;
                span[10] = 10;
                Assert.Equal(10, owned.Array[10]);

                var memory = owned.Memory;
                var array = memory.ToArray();
                Assert.Equal(owned.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }     
        }

        [Fact]
        public void ArrayMemoryLifetime()
        {
            OwnedMemory<byte> owned = new OwnedArray<byte>(1024);

            Assert.Equal(1, owned.ReferenceCount);

            var baseMemory = owned.Memory;

            var reservation0 = baseMemory.Reserve();

            Assert.Equal(2, owned.ReferenceCount);

            var memory0 = reservation0.Memory;

            var reservation1 = memory0.Reserve();

            Assert.Equal(3, owned.ReferenceCount);

            NonDisposedDoesNotThrow(memory0);
            NonDisposedDoesNotThrow(reservation0);

            reservation0.Dispose();

            Assert.Equal(2, owned.ReferenceCount);

            DisposedDoesThrow(reservation0);
            DisposedDoesThrow(memory0);
            NonDisposedDoesNotThrow(reservation1);

            NonDisposedDoesNotThrow(owned);
            NonDisposedDoesNotThrow(baseMemory);

            var reservation2 = baseMemory.Reserve();

            Assert.Equal(3, owned.ReferenceCount);

            owned.Dispose();

            Assert.Equal(2, owned.ReferenceCount);

            DisposedDoesThrow(owned);
            DisposedDoesThrow(baseMemory);

            NonDisposedDoesNotThrow(reservation1);
            NonDisposedDoesNotThrow(reservation2);

            reservation1.Dispose();
            reservation2.Dispose();

            DisposedDoesThrow(reservation1);
            DisposedDoesThrow(reservation2);

            Assert.Equal(0, owned.ReferenceCount);
        }

        [Fact]
        public unsafe void ReferenceCounting()
        {
            var owned = new CustomMemory();
            var memory = owned.Memory;
            Assert.Equal(1, owned.ReferenceCount);
            using (memory.Reserve())
            {
                Assert.Equal(2, owned.ReferenceCount);
            }
            Assert.Equal(1, owned.ReferenceCount);
            owned.Dispose();
            Assert.Equal(0, owned.ReferenceCount);
        }

        [Fact]
        public unsafe void CopyOnReserve()
        {
            var owned = new CustomMemory();
            ReadOnlyMemory<byte> memory = owned.Memory;
            var slice = memory.Slice(0, 1);

            Assert.Equal(1, owned.ReferenceCount);

            // this copies on reserve
            using (var reservation = slice.Reserve())
            {
                Assert.Equal(1, owned.ReferenceCount);
            }
            Assert.Equal(1, owned.ReferenceCount);
        }

        [Fact]
        public unsafe void ReservationTrackingExpands()
        {
            OwnedMemory<byte> owned = new OwnedArray<byte>(1024);
            var baseMemory = owned.Memory;
            for(int i = 0; i<100; i++) {
                var r = baseMemory.Reserve();
                r.Dispose();
            }
        }

        [Fact]
        public void AutoDispose()
        {
            // If an object's Dispose method is called more than once, 
            // the object must ignore all calls after the first one.
            // The object must not throw an exception if its Dispose 
            // method is called multiple times.

            bool disposeStarted = false;
            bool disposeCompleted = false;

            var derived = new AutoDisposeMemory(1000);
            derived.OnDisposeStart = () => disposeStarted = true;
            derived.OnDisposeEnd = () => disposeCompleted = true;

            OwnedMemory<byte> owned = derived;

            Assert.NotNull(derived.BaseArray);

            var memory = owned.Memory;
            Assert.Equal(1, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);

            var reservation0 = memory.Reserve();
            Assert.Equal(2, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);

            var reservation1 = memory.Reserve();
            Assert.Equal(3, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);

            Assert.Equal(false, reservation1.IsDisposed);
            reservation1.Dispose();
            Assert.Equal(true, reservation1.IsDisposed);
            Assert.Equal(2, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);

            // Second Dispose is idempotent
            reservation1.Dispose();
            Assert.Equal(2, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);
            Assert.NotNull(derived.BaseArray);

            owned.Dispose();
            Assert.Equal(1, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);

            // Second Dispose is idempotent
            owned.Dispose();
            Assert.Equal(1, owned.ReferenceCount);
            Assert.Equal(false, owned.IsDisposed);
            Assert.NotNull(derived.BaseArray);

            // Full disposal not triggered
            Assert.False(disposeStarted);
            Assert.False(disposeCompleted);

            // Last reference disposal
            Assert.Equal(false, reservation0.IsDisposed);
            reservation0.Dispose();
            Assert.Equal(true, reservation0.IsDisposed);

            // Full disposal has been triggered
            Assert.True(disposeStarted);
            Assert.True(disposeCompleted);

            Assert.Equal(0, owned.ReferenceCount);
            Assert.Equal(true, owned.IsDisposed);
            Assert.Null(derived.BaseArray);

            // Second Dispose does not throw
            reservation0.Dispose();
        }

        static void NonDisposedDoesNotThrow<T>(OwnedMemory<T> owned)
        {
            Assert.False(owned.IsDisposed);
            Assert.True(owned.ReferenceCount >= 1);

            var span = owned.Span;
            var memory = owned.Memory;

            NonDisposedDoesNotThrow(memory);
        }

        static void NonDisposedDoesNotThrow<T>(ReservedMemory<T> reservation)
        {
            Assert.False(reservation.IsDisposed);

            var memory = reservation.Memory;
            NonDisposedDoesNotThrow(memory);
        }

        static void NonDisposedDoesNotThrow<T>(Memory<T> memory)
        {
            Assert.False(memory.IsDisposed);
            var span = memory.Span;
            using (var reservation = memory.Reserve()) { }
            memory = memory.Slice(0);

            ArraySegment<T> buffer;
            memory.TryGetArray(out buffer);

            unsafe
            {
                void* pointer;
                memory.TryGetPointer(out pointer);
            }
        }

        static void DisposedDoesThrow<T>(OwnedMemory<T> owned)
        {
            Assert.Throws<ObjectDisposedException>(() =>
            {
                var span = owned.Span;
            });

            Assert.Throws<ObjectDisposedException>(() =>
            {
                var memory = owned.Memory;
            });
        }

        static void DisposedDoesThrow<T>(ReservedMemory<T> reservation)
        {
            Assert.True(reservation.IsDisposed);

            Assert.Throws<ObjectDisposedException>(() =>
            {
                var memory = reservation.Memory;
            });
        }

        static void DisposedDoesThrow<T>(Memory<T> memory)
        {
            Assert.True(memory.IsDisposed);

            Assert.Throws<ObjectDisposedException>(() =>
            {
                var span = memory.Span;
            });
            Assert.Throws<ObjectDisposedException>(() =>
            {
                using (var reservation = memory.Reserve()) { }
            });
            Assert.Throws<ObjectDisposedException>(() =>
            {
                memory = memory.Slice(0);
            });
            Assert.Throws<ObjectDisposedException>(() =>
            {
                ArraySegment<T> buffer;
                memory.TryGetArray(out buffer);
            });
            Assert.Throws<ObjectDisposedException>(() =>
            {
                unsafe
                {
                    void* pointer;
                    memory.TryGetPointer(out pointer);
                }
            });
        }
    }

    class CustomMemory : OwnedMemory<byte>
    {
        public CustomMemory() : base(new byte[256], 0, 256) { }

        protected override ReservedMemory<byte> Reserve(ref Memory<byte> memory, long versionId, int reservationId)
        {
            if (memory.Length < Length) {
                var copy = memory.Span.ToArray();
                using (OwnedArray<byte> newOwned = copy)
                {
                    memory = newOwned.Memory;
                    return memory.Reserve();
                }
            }
            else {
                return base.Reserve(ref memory, versionId, reservationId);
            }
        }

        protected override ReservedReadOnlyMemory<byte> Reserve(ref ReadOnlyMemory<byte> memory, long versionId, int reservationId)
        {
            if (memory.Length < Length)
            {
                var copy = memory.Span.ToArray();
                using (OwnedArray<byte> newOwned = copy)
                {
                    memory = newOwned.Memory;
                    return memory.Reserve();
                }
            }
            else
            {
                return base.Reserve(ref memory, versionId, reservationId);
            }
        }
    }

    class AutoDisposeMemory : OwnedMemory<byte>
    {
        public AutoDisposeMemory(int length) : this(ArrayPool<byte>.Shared.Rent(length)) {
        }

        AutoDisposeMemory(byte[] array) : base(array, 0, array.Length) {
        }

        protected override void Dispose(bool disposing)
        {
            OnDisposeStart?.Invoke();
            ArrayPool<byte>.Shared.Return(Array);
            base.Dispose(disposing);
        }

        public byte[] BaseArray => Array;


        protected override void DisposeComplete()
        {
            OnDisposeEnd?.Invoke();
        }

        public Action OnDisposeStart { get; set; }
        public Action OnDisposeEnd { get; set; }
    }
}