// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace System.Slices.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void SimpleTestS()
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
            Memory<byte> copyStoredForLater;
            var owner = new OwnedArray<byte>(1024);
            try {
                Memory<byte> memory = owner.Memory;
                Memory<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                var r = memorySlice.Reserve();
                try { // increments the "outstanding span" refcount
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; cannot dispose
                        owner.Dispose();
                    });
                    Assert.Throws<ObjectDisposedException>(() => {
                        Span<byte> span = memorySlice.Span;
                        span[0] = 255;
                    });
                }
                finally {
                    Assert.Throws<ObjectDisposedException>(() => {
                       r.Dispose(); // releases the refcount
                    });
                }
            }
            finally {
                Assert.Throws<InvalidOperationException>(() => {
                    owner.Dispose();
                });
            }
            Assert.Throws<ObjectDisposedException>(() => { // manager is disposed
                var span = copyStoredForLater.Span;
            }); 
        }

        [Fact]
        public void RacyAccess()
        {
            for(int k = 0; k < 1000; k++) {
                var owners   = new OwnedArray<byte>[128];
                var memories = new Memory<byte>[owners.Length];
                var reserves = new DisposableReservation[owners.Length];
                var disposeSuccesses = new bool[owners.Length];
                var reserveSuccesses = new bool[owners.Length];

                for (int i = 0; i < owners.Length; i++) {
                    owners[i] = new OwnedArray<byte>(1024);
                    memories[i] = owners[i].Memory;
                }

                var dispose_task = Task.Run(() => {
                    for (int i = 0; i < owners.Length; i++) {
                        try {
                            owners[i].Dispose();
                            disposeSuccesses[i] = true;
                        } catch (InvalidOperationException) {
                            disposeSuccesses[i] = false;
                        }
                    }
                });

                var reserve_task = Task.Run(() => {
                    for (int i = owners.Length - 1; i >= 0; i--) {
                        try {
                            reserves[i] = memories[i].Reserve();
                            reserveSuccesses[i] = true;
                        } catch (ObjectDisposedException) {
                            reserveSuccesses[i] = false;
                        }
                    }
                });

                Task.WaitAll(reserve_task, dispose_task);

                for(int i = 0; i < owners.Length; i++) {
                    Assert.False(disposeSuccesses[i] && reserveSuccesses[i]);
                }
            }
        }

        [Fact]
        public unsafe void ReferenceCounting()
        {
            var owned = new CustomMemory();
            var memory = owned.Memory;
            Assert.Equal(0, owned.OnZeroRefencesCount);
            Assert.False(owned.HasOutstandingReferences);
            using (memory.Reserve()) {
                Assert.Equal(0, owned.OnZeroRefencesCount);
                Assert.True(owned.HasOutstandingReferences);
            }
            Assert.Equal(1, owned.OnZeroRefencesCount);
            Assert.False(owned.HasOutstandingReferences);
        }

        [Fact]
        public unsafe void CopyOnReserve()
        {
            var owned = new CustomMemory();
            ReadOnlyMemory<byte> memory = owned.Memory;
            var slice = memory.Slice(0, 1);

            // this copies on reserve
            using (slice.Reserve()) {
                Assert.Equal(0, owned.OnZeroRefencesCount);
                Assert.True(owned.HasZeroReferences);
            }
            Assert.Equal(0, owned.OnZeroRefencesCount);
            Assert.True(owned.HasZeroReferences);
        }

        [Fact]
        public void AutoDispose()
        {
            OwnedMemory<byte> owned = new AutoDisposeMemory(1000);
            var memory = owned.Memory;
            Assert.Equal(false, owned.IsDisposed);
            var reservation = memory.Reserve();
            Assert.Equal(false, owned.IsDisposed);
            owned.Release();
            Assert.Equal(false, owned.IsDisposed);
            reservation.Dispose();
            Assert.Equal(true, owned.IsDisposed);
        }
    }

    class CustomMemory : OwnedMemory<byte>
    {
        int _onZeroRefencesCount;

        public CustomMemory() : base(new byte[256], 0, 256) { }

        public int OnZeroRefencesCount => _onZeroRefencesCount;

        protected override DisposableReservation Reserve(ref ReadOnlyMemory<byte> memory)
        {
            if (memory.Length < Length) {
                var copy = memory.Span.ToArray();
                OwnedArray<byte> newOwned = copy;
                memory = newOwned.Memory;
                return memory.Reserve();
            }
            else {
                return base.Reserve(ref memory);
            }
        }

        protected override void OnZeroReferences()
        {
            _onZeroRefencesCount++;
        }
    }

    class AutoDisposeMemory : OwnedMemory<byte>
    {
        public AutoDisposeMemory(int length) : this(ArrayPool<byte>.Shared.Rent(length)) {
        }

        AutoDisposeMemory(byte[] array) : base(array, 0, array.Length) {
            AddReference();
        }

        protected override void Dispose(bool disposing)
        {
            ArrayPool<byte>.Shared.Return(Array);
            base.Dispose(disposing);
        }

        protected override void OnZeroReferences()
        {
            Dispose();
        }
    }
}