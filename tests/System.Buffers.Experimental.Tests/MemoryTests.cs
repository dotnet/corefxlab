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
            using(var owned = new OwnedNativeBuffer(1024)) {
                var span = owned.Span;
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
                var span = owned.Span;
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
    }
}
