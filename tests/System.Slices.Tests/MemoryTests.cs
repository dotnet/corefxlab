// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
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

                var copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }     
        }

        [Fact]
        public void ArrayMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            using (var owner = new OwnedArray<byte>(1024)) {
                Memory<byte> memory = owner.Memory;
                Memory<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                using (memorySlice.Reserve()) { // increments the "outstanding span" refcount
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; cannot dispose
                        owner.Dispose();
                    });
                    Span<byte> span = memorySlice.Span;
                    span[0] = 255;
                } // releases the refcount
            }
            Assert.Throws<ObjectDisposedException>(() => { // manager is disposed
                var span = copyStoredForLater.Span;
            }); 
        }

        [Fact]
        public unsafe void ReferenceCounting()
        {
            var owned = new CustomMemory();
            var memory = owned.Memory;
            Assert.Equal(0, owned.ReferenceCountChangeCount);
            Assert.Equal(0, owned.ReferenceCount);
            using (memory.Reserve()) {
                Assert.Equal(1, owned.ReferenceCountChangeCount);
                Assert.Equal(1, owned.ReferenceCount);
            }
            Assert.Equal(2, owned.ReferenceCountChangeCount);
            Assert.Equal(0, owned.ReferenceCount);
        }

        [Fact]
        public unsafe void CopyOnReserve()
        {
            var owned = new CustomMemory();
            ReadOnlyMemory<byte> memory = owned.Memory;
            var slice = memory.Slice(0, 1);

            // this copies on reserve
            using (slice.Reserve()) {
                Assert.Equal(0, owned.ReferenceCountChangeCount);
                Assert.Equal(0, owned.ReferenceCount);
            }
            Assert.Equal(0, owned.ReferenceCountChangeCount);
            Assert.Equal(0, owned.ReferenceCount);
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
        int _referenceCountChangeCount;

        public CustomMemory() : base(new byte[256], 0, 256) { }

        public int ReferenceCountChangeCount => _referenceCountChangeCount;

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

        protected override void OnReferenceCountChanged(int newReferenceCount)
        {
            _referenceCountChangeCount++;
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

        protected override void OnReferenceCountChanged(int newReferenceCount)
        {
            if (newReferenceCount == 0) {
                Dispose();
            }
        }
    }
}