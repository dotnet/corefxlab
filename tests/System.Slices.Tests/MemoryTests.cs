// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using Xunit;

namespace System.Slices.Tests
{
    public class MemoryTests
    {
        [Fact]
        public unsafe void SimpleTestS()
        {
            {
                OwnedMemory<byte> memory = (OwnedMemory<byte>)new byte[1024];
                var span = memory.Span;
                span[10] = 10;
                ArraySegment<byte> arraySegment;

                Assert.True(memory.Memory.TryGetArray(out arraySegment, null));
                Assert.Equal(10, arraySegment.Array[10]);
            }

            using(var memory = new OwnedMemory<byte>(1024, MemoryType.Native)) {
                var span = memory.Span;
                span[10] = 10;
                void* pointer;

                Assert.True(memory.Memory.TryGetPointer(out pointer));
                unsafe { Assert.Equal(10, ((byte*)pointer)[10]); }
            }

            using (var memory = new OwnedMemory<byte>(1024, MemoryType.Pinned)) {
                var span = memory.Span;
                span[10] = 10;

                ArraySegment<byte> arraySegment;

                Assert.True(memory.Memory.TryGetArray(out arraySegment, null));
                Assert.Equal(10, arraySegment.Array[10]);
                unsafe
                { 
                    void* pointer;
                    Assert.True(memory.Memory.TryGetPointer(out pointer));
                    Assert.Equal(10, ((byte*)pointer)[10]);
                }
            }
        }

        [Fact]
        public void ArrayMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            using (var owner = new OwnedMemory<byte>(1024)) {
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
            using (var owner = new OwnedMemory<byte>(1024, MemoryType.Native)) {
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
                using (var owner = new OwnedMemory<byte>(bytes, pBytes)) {
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
    }
}