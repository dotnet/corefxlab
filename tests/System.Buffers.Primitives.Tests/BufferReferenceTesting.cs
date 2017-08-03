// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceTests
    {
        public static void TestOwnedBuffer(Func<OwnedBuffer<byte>> create, Action<OwnedBuffer<byte>> dispose = null)
        {
            RunTest(BufferLifetimeBasics, create, dispose);
            RunTest(MemoryHandleDoubleFree, create, dispose);
            RunTest(AsSpan, create, dispose);
            RunTest(Buffer, create, dispose);
            RunTest(Pin, create, dispose);
            RunTest(Dispose, create, dispose);
            RunTest(buffer => TestBuffer(() => buffer.Buffer), create, dispose);
            //RunTest(OverRelease, create, dispose); // TODO: corfxlab #1571
        }

        static void RunTest(Action<OwnedBuffer<byte>> test, Func<OwnedBuffer<byte>> create,
            Action<OwnedBuffer<byte>> dispose)
        {
            var buffer = create();
            try
            {
                test(buffer);
            }
            finally
            {
                dispose?.Invoke(buffer);
            }
        }

        static void MemoryAccessBasics(OwnedBuffer<byte> buffer)
        {
            var span = buffer.AsSpan();
            span[10] = 10;

            Assert.Equal(buffer.Length, span.Length);
            Assert.Equal(10, span[10]);

            var memory = buffer.Buffer;
            Assert.Equal(buffer.Length, memory.Length);
            Assert.Equal(10, memory.Span[10]);

            var array = memory.ToArray();
            Assert.Equal(buffer.Length, array.Length);
            Assert.Equal(10, array[10]);

            Span<byte> copy = new byte[20];
            memory.Slice(10, 20).CopyTo(copy);
            Assert.Equal(10, copy[0]);
        }

        // tests OwnedBuffer.AsSpan overloads
        static void AsSpan(OwnedBuffer<byte> buffer)
        {
            var span = buffer.AsSpan();
            var fullSlice = buffer.AsSpan(0, buffer.Length);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (byte)(i % 254 + 1);
                Assert.Equal(span[i], fullSlice[i]);
            }

            var slice = buffer.AsSpan(5, buffer.Length - 5);
            Assert.Equal(span.Length - 5, slice.Length);

            for (int i = 0; i < slice.Length; i++)
            {
                Assert.Equal(span[i + 5], slice[i]);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.AsSpan(buffer.Length, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.AsSpan(1, buffer.Length);
            });
        }

        // tests that OwnedBuffer.Buffer and OwnedBuffer.ReadOnlyBuffer point to the same memory
        static void Buffer(OwnedBuffer<byte> buffer)
        {
            var rwBuffer = buffer.Buffer;
            var rwSpan = rwBuffer.Span;

            var roBuffer = buffer.ReadOnlyBuffer;
            var roSpan = roBuffer.Span;

            Assert.Equal(roSpan.Length, rwSpan.Length);

            for (int i = 0; i < roSpan.Length; i++)
            {
                var value = roSpan[i];
                byte newValue = (byte)(value + 1);
                rwSpan[i] = newValue;
                Assert.Equal(newValue, roSpan[i]);
            }
        }

        static void Pin(OwnedBuffer<byte> buffer)
        {
            var memory = buffer.Buffer;
            Assert.False(buffer.IsRetained);
            var handle = memory.Retain(pin: true);
            unsafe
            {
                Assert.NotEqual(0L, new IntPtr(handle.PinnedPointer).ToInt64());
            }
            Assert.True(buffer.IsRetained);
            handle.Dispose();
            Assert.False(buffer.IsRetained);
        }

        static void Dispose(OwnedBuffer<byte> buffer)
        {
            var length = buffer.Length;

            buffer.Dispose();
            Assert.True(buffer.IsDisposed);
            Assert.False(buffer.IsRetained);

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.AsSpan();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.AsSpan(0, length);
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.Pin();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var rwBuffer = buffer.Buffer;
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var roBuffer = buffer.ReadOnlyBuffer;
            });
        }

        static void OverRelease(OwnedBuffer<byte> buffer)
        {
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                buffer.Release();
            });
        }

        static void BufferLifetimeBasics(OwnedBuffer<byte> buffer)
        {
            Buffer<byte> copyStoredForLater;
            try
            {
                Buffer<byte> memory = buffer.Buffer;
                Buffer<byte> slice = memory.Slice(10);
                copyStoredForLater = slice;
                var handle = slice.Retain();
                try
                {
                    Assert.Throws<InvalidOperationException>(() =>
                    { // memory is reserved; premature dispose check fires
                        buffer.Dispose();
                    });
                }
                finally
                {
                    handle.Dispose(); // release reservation
                }
            }
            finally
            {
                buffer.Dispose(); // can finish dispose with no exception
            }
            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });

            Assert.True(buffer.IsDisposed);
        }

        static void MemoryHandleDoubleFree(OwnedBuffer<byte> buffer)
        {
            var memory = buffer.Buffer;
            var handle = memory.Retain(pin: true);
            Assert.True(buffer.IsRetained);
            buffer.Retain();
            Assert.True(buffer.IsRetained);
            handle.Dispose();
            Assert.True(buffer.IsRetained);
            handle.Dispose();
            Assert.True(buffer.IsRetained);
            buffer.Release();
            Assert.False(buffer.IsRetained);
        }

        public static void TestBuffer(Func<Buffer<byte>> create)
        {
            BufferBasics(create());
            BufferLifetime(create());
        }

        public static void TestBuffer(Func<ReadOnlyBuffer<byte>> create)
        {
            BufferBasics(create());
            BufferLifetime(create());
        }

        static void BufferBasics(Buffer<byte> buffer)
        {
            var span = buffer.Span;
            Assert.Equal(buffer.Length, span.Length);
            Assert.True(buffer.IsEmpty || buffer.Length != 0);
            Assert.True(!buffer.IsEmpty || buffer.Length == 0);

            for (int i = 0; i < span.Length; i++) span[i] = 100;

            var array = buffer.ToArray();
            for (int i = 0; i < array.Length; i++) Assert.Equal(array[i], span[i]);

            if (buffer.TryGetArray(out var segment))
            {
                Assert.Equal(segment.Count, array.Length);
                for (int i = 0; i < array.Length; i++) Assert.Equal(array[i], segment.Array[i + segment.Offset]);
            }

            if (buffer.Length > 0)
            {
                var slice = buffer.Slice(1);
                for (int i = 0; i < slice.Length; i++) slice.Span[i] = 101;

                for (int i = 0; i < slice.Length; i++) Assert.Equal(slice.Span[i], span[i + 1]);
            }
        }

        static void BufferLifetime(Buffer<byte> buffer)
        {
            var array = buffer.ToArray();
            using (var pinned = buffer.Retain(pin: true))
            {
                unsafe
                {
                    var p = (byte*)pinned.PinnedPointer;
                    Assert.True(null != p);
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        Assert.Equal(array[i], p[i]);
                    }
                }
            }

            // TODO: the following using statement does not work ...
            // AutoDisposeBuffer is getting disposed above. Are we ok with this?
            //using(var handle = buffer.Retain())
            //{
            //}
        }

        static void BufferBasics(ReadOnlyBuffer<byte> buffer)
        {
            var span = buffer.Span;
            Assert.Equal(buffer.Length, span.Length);
            Assert.True(buffer.IsEmpty || buffer.Length != 0);
            Assert.True(!buffer.IsEmpty || buffer.Length == 0);

            var array = buffer.ToArray();
            for (int i = 0; i < array.Length; i++) Assert.Equal(array[i], span[i]);

            if (buffer.Length > 0)
            {
                var slice = buffer.Slice(1);
                for (int i = 0; i < slice.Length; i++) Assert.Equal(slice.Span[i], span[i + 1]);
            }
        }

        static void BufferLifetime(ReadOnlyBuffer<byte> buffer)
        {
            var array = buffer.ToArray();
            using (var pinned = buffer.Retain(pin: true))
            {
                unsafe
                {
                    var p = (byte*)pinned.PinnedPointer;
                    Assert.True(null != p);
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        Assert.Equal(array[i], p[i]);
                    }
                }
            }
        }
    }
}
