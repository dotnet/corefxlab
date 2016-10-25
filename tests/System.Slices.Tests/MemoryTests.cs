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
                OwnedArray<byte> memory = new byte[1024];
                var span = memory.Span;
                span[10] = 10;
                Assert.Equal(10, memory.Array[10]);
            }

            using(var memory = new OwnedNativeMemory(1024)) {
                var span = memory.Span;
                span[10] = 10;
                unsafe { Assert.Equal(10, memory.Pointer[10]); }
            }

            using (OwnedPinnedArray<byte> memory = new byte[1024]) {
                var span = memory.Span;
                span[10] = 10;
                Assert.Equal(10, memory.Array[10]);

                unsafe { Assert.Equal(10, memory.Pointer[10]); }
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
        public void NativeMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            using (var owner = new OwnedNativeMemory(1024)) {
                Memory<byte> memory = owner.Memory;
                Memory<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                using (memorySlice.Reserve()) {
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; cannot dispose
                        owner.Dispose();
                    }); 
                    Span<byte> span = memorySlice.Span;
                    span[0] = 255;
                }
            }
            Assert.Throws<ObjectDisposedException>(() => { // manager is disposed
                var span = copyStoredForLater.Span;
            });
        }

        [Fact]
        public unsafe void PinnedArrayMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            var bytes = new byte[1024];
            fixed (byte* pBytes = bytes) {
                using (var owner = new OwnedPinnedArray<byte>(bytes, pBytes)) {
                    Memory<byte> memory = owner.Memory;
                    Memory<byte> memorySlice = memory.Slice(10);
                    copyStoredForLater = memorySlice;
                    using (memorySlice.Reserve()) {
                        Assert.Throws<InvalidOperationException>(() => { // memory is reserved; cannot dispose
                            owner.Dispose();
                        }); 
                        Span<byte> span = memorySlice.Span;
                        span[0] = 255;
                    }
                }   
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
    }

    class CustomMemory : OwnedMemory<byte>
    {
        byte[] _memory = new byte[256];
        int _referenceCountChangeCount;

        public int ReferenceCountChangeCount => _referenceCountChangeCount;

        protected override void DisposeCore()
        { }

        protected override Span<byte> GetSpanCore()
        {
            return _memory;
        }

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
        protected override bool TryGetArrayCore(out ArraySegment<byte> buffer)
        {
            buffer = default(ArraySegment<byte>);
            return false;
        }

        protected override unsafe bool TryGetPointerCore(out void* pointer)
        {
            pointer = null;
            return false;
        }

        protected override void OnReferenceCountChanged(int newReferenceCount)
        {
            _referenceCountChangeCount++;
        }
    }
}