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
                var r = memorySlice.Reserve();
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
        public unsafe void IsSliceOf_PointerVersusArraySpansMatch()
        {
            ulong[] data = new ulong[8];
            fixed (ulong* ptr = data)
            {
                Span<ulong> arrayBased = data;
                Span<ulong> pointerBased = new Span<ulong>(ptr, 8);

                // justification for match: Span<T> treats as equivalent:
                Assert.True(arrayBased == pointerBased);

                // comparing array to pointer says yes (despite being different internals)
                Assert.True(arrayBased.IsSliceOf(pointerBased));
                Assert.True(pointerBased.IsSliceOf(arrayBased));

                // comparing to self says yes
                Assert.True(arrayBased.IsSliceOf(arrayBased));
                Assert.True(pointerBased.IsSliceOf(pointerBased));
            }
        }

        [Fact]
        public unsafe void IsSliceOf_NonOverlappingSpansNeverMatch()
        {
            ulong[] data = new ulong[8];
            fixed (ulong* ptr = data)
            {
                Span<ulong> arrayBased = data;
                var x = arrayBased.Slice(1, 2);
                var y = arrayBased.Slice(5, 2);
                Assert.False(x.IsSliceOf(y));
                Assert.False(y.IsSliceOf(x));

                Span<ulong> pointerBased = new Span<ulong>(ptr, 8);
                x = pointerBased.Slice(1, 2);
                y = pointerBased.Slice(5, 2);
                Assert.False(x.IsSliceOf(y));
                Assert.False(y.IsSliceOf(x));
            }
        }

        [Fact]
        public unsafe void IsSliceOf_PartialOverlappingSpansNeverMatch()
        {
            ulong[] data = new ulong[8];
            fixed (ulong* ptr = data)
            {
                Span<ulong> arrayBased = data;
                var x = arrayBased.Slice(1, 4);
                var y = arrayBased.Slice(3, 4);
                Assert.False(x.IsSliceOf(y));
                Assert.False(y.IsSliceOf(x));

                Span<ulong> pointerBased = new Span<ulong>(ptr, 8);
                x = pointerBased.Slice(1, 4);
                y = pointerBased.Slice(3, 4);
                Assert.False(x.IsSliceOf(y));
                Assert.False(y.IsSliceOf(x));
            }
        }

        [Fact]
        public unsafe void IsSliceOf_EqualSpansAlwaysMatch()
        {
            ulong[] data = new ulong[8];
            fixed (ulong* ptr = data)
            {
                Span<ulong> arrayBased = data;
                Assert.True(arrayBased.IsSliceOf(arrayBased));
                for (int start = 0; start < 8; start++)
                {
                    for (int length = data.Length - start; length >= 0; length--)
                    {
                        Assert.True(arrayBased.Slice(start, length).IsSliceOf(arrayBased.Slice(start, length)));
                    }
                }

                Span<ulong> pointerBased = new Span<ulong>(ptr, 8);
                Assert.True(pointerBased.IsSliceOf(pointerBased));
                for (int start = 0; start < 8; start++)
                {
                    for (int length = data.Length - start; length >= 0; length--)
                    {
                        Assert.True(pointerBased.Slice(start, length).IsSliceOf(pointerBased.Slice(start, length)));
                    }
                }
            }
        }
        [Fact]
        public void IsSliceOf_SubSpansShouldMatch_BasicArrays()
        {
            int[] data = new int[20];

            var outer = new Span<int>(data);
            var inner = outer.Slice(6, 3);

            Assert.True(outer.IsSliceOf(outer));
            Assert.True(inner.IsSliceOf(inner));

            Assert.False(outer.IsSliceOf(inner));
            Assert.True(inner.IsSliceOf(outer));
            int start;
            Assert.True(inner.IsSliceOf(outer, out start));
            Assert.Equal(6, start);


            var innerInner = inner.Slice(1);
            Assert.True(innerInner.IsSliceOf(innerInner));
            Assert.True(innerInner.IsSliceOf(inner));
            Assert.True(innerInner.IsSliceOf(inner, out start));
            Assert.Equal(1, start);
            Assert.True(innerInner.IsSliceOf(outer));
            Assert.True(innerInner.IsSliceOf(outer, out start));
            Assert.Equal(7, start);
        }

        [Fact]
        public unsafe void IsSliceOf_SubSpansShouldMatch_BasicPointers()
        {
            int* data = stackalloc int[20];

            var outer = new Span<int>(data, 20);
            var inner = outer.Slice(6, 3);

            Assert.True(outer.IsSliceOf(outer));
            Assert.True(inner.IsSliceOf(inner));

            Assert.False(outer.IsSliceOf(inner));
            Assert.True(inner.IsSliceOf(outer));
            int start;
            Assert.True(inner.IsSliceOf(outer, out start));
            Assert.Equal(6, start);


            var innerInner = inner.Slice(1);
            Assert.True(innerInner.IsSliceOf(innerInner));
            Assert.True(innerInner.IsSliceOf(inner));
            Assert.True(innerInner.IsSliceOf(inner, out start));
            Assert.Equal(1, start);
            Assert.True(innerInner.IsSliceOf(outer));
            Assert.True(innerInner.IsSliceOf(outer, out start));
            Assert.Equal(7, start);
        }


        [Fact]
        public unsafe void IsSliceOf_MisalignedPointersShouldNotMatch()
        {
            byte* data = stackalloc byte[10 * sizeof(ulong)];
            var outer = new Span<ulong>(data, 10);
            var inner = new Span<ulong>(data + 8, 1);
            Assert.True(inner.IsSliceOf(outer));

            inner = new Span<ulong>(data + 11, 1);
            Assert.False(inner.IsSliceOf(outer));
        }
    }
}