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
            using(var owned = new OwnedNativeMemory(1024)) {
                var span = owned.Span;
                span[10] = 10;
                unsafe { Assert.Equal(10, owned.Pointer[10]); }

                var memory = owned.Memory;
                var array = memory.ToArray();
                Assert.Equal(owned.Length, array.Length);
                Assert.Equal(10, array[10]);

                Span<byte> copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }

            using (OwnedPinnedArray<byte> owned = new byte[1024]) {
                var span = owned.Span;
                span[10] = 10;
                Assert.Equal(10, owned.Array[10]);

                unsafe { Assert.Equal(10, owned.Pointer[10]); }

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
        public void NativeMemoryLifetime()
        {
            var owner = new OwnedNativeMemory(1024);
            TestMemoryLifetime(owner);
        }

        [Fact]
        public unsafe void PinnedArrayMemoryLifetime()
        {
            var bytes = new byte[1024];
            fixed (byte* pBytes = bytes) {
                var owner = new OwnedPinnedArray<byte>(bytes, pBytes);
                TestMemoryLifetime(owner);
            }
        }

        public static void TestMemoryLifetime(OwnedMemory<byte> owned)
        {
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
}