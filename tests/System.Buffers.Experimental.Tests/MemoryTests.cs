// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using Xunit;

namespace System.Slices.Tests
{
    public class MemoryTests
    {
        [Fact(Skip = "ReadOnlyBytesTests are flaky")]
        public void SimpleTestS()
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

        [Fact(Skip = "ReadOnlyBytesTests are flaky")]
        public void NativeMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            var owner = new OwnedNativeMemory(1024);
            try {
                Memory<byte> memory = owner.Memory;
                Memory<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                var r = memorySlice.Reserve();
                try {
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; cannot dispose
                        owner.Dispose();
                    }); 
                    Assert.Throws<ObjectDisposedException>(() => {
                        // memory is disposed
                        Span<byte> span = memorySlice.Span;
                        span[0] = 255;
                    });
                }
                finally {
                    Assert.Throws<ObjectDisposedException>(() => {
                        // memory is disposed
                        r.Dispose();
                    });
                }
            }
            finally {
                Assert.Throws<InvalidOperationException>(() => {
                    // memory is still reserved as Release failed.
                    owner.Dispose();
                });
            }
            Assert.Throws<ObjectDisposedException>(() => { // manager is disposed
                var span = copyStoredForLater.Span;
            });
        }

        [Fact(Skip = "ReadOnlyBytesTests are flaky")]
        public unsafe void PinnedArrayMemoryLifetime()
        {
            Memory<byte> copyStoredForLater;
            var bytes = new byte[1024];
            fixed (byte* pBytes = bytes) {
                var owner = new OwnedPinnedArray<byte>(bytes, pBytes);
                try {
                    Memory<byte> memory = owner.Memory;
                    Memory<byte> memorySlice = memory.Slice(10);
                    copyStoredForLater = memorySlice;
                    var r = memorySlice.Reserve();
                    try {
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
                            // memory has been Disposed, so cannot free reservation
                            r.Dispose();
                        });
                    }
                }
                finally {
                    Assert.Throws<InvalidOperationException>(() => {
                        // memory is still reserved as Release failed.
                        owner.Dispose();
                    });
                }
            }
            Assert.Throws<ObjectDisposedException>(() => { // manager is disposed
                var span = copyStoredForLater.Span;
            });
        }
    }
}