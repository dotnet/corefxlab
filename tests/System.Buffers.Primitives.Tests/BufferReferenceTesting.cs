// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceTests
    {
        public static void TestOwnedBuffer(Func<OwnedMemory<byte>> create, Action<OwnedMemory<byte>> dispose = null)
        {
            RunTest(BufferLifetimeBasics, create, dispose);
            RunTest(MemoryHandleDoubleFree, create, dispose);
            RunTest(Span, create, dispose);
            RunTest(Buffer, create, dispose);
            RunTest(Pin, create, dispose);
            RunTest(Dispose, create, dispose);
            RunTest(buffer => TestBuffer(() => buffer.Memory), create, dispose);
            //RunTest(OverRelease, create, dispose); // TODO: corfxlab #1571
        }

        public static void TestAutoOwnedBuffer(Func<OwnedMemory<byte>> create, Action<OwnedMemory<byte>> dispose = null)
        {
            RunTest(BufferLifetimeBasicsAuto, create, dispose);
            RunTest(MemoryHandleDoubleFreeAuto, create, dispose);
            RunTest(Span, create, dispose);
            RunTest(Buffer, create, dispose);
            RunTest(Pin, create, dispose);
            RunTest(DisposeAuto, create, dispose);
            RunTest(buffer => TestBuffer(() => buffer.Memory), create, dispose);
            //RunTest(OverRelease, create, dispose); // TODO: corfxlab #1571
        }

        static void RunTest(Action<OwnedMemory<byte>> test, Func<OwnedMemory<byte>> create,
            Action<OwnedMemory<byte>> dispose)
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

        static void MemoryAccessBasics(OwnedMemory<byte> buffer)
        {
            var span = buffer.Span;
            span[10] = 10;

            Assert.Equal(buffer.Length, span.Length);
            Assert.Equal(10, span[10]);

            var memory = buffer.Memory;
            Assert.Equal(buffer.Length, memory.Length);
            Assert.Equal(10, memory.Span[10]);

            var array = memory.ToArray();
            Assert.Equal(buffer.Length, array.Length);
            Assert.Equal(10, array[10]);

            Span<byte> copy = new byte[20];
            memory.Slice(10, 20).Span.CopyTo(copy);
            Assert.Equal(10, copy[0]);
        }

        // tests OwnedMemory.Span overloads
        static void Span(OwnedMemory<byte> buffer)
        {
            var span = buffer.Span;
            var fullSlice = buffer.Span.Slice(0, buffer.Length);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (byte)(i % 254 + 1);
                Assert.Equal(span[i], fullSlice[i]);
            }

            var slice = buffer.Span.Slice(5, buffer.Length - 5);
            Assert.Equal(span.Length - 5, slice.Length);

            for (int i = 0; i < slice.Length; i++)
            {
                Assert.Equal(span[i + 5], slice[i]);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.Span.Slice(buffer.Length, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.Span.Slice(1, buffer.Length);
            });
        }

        // tests that OwnedMemory.Memory and OwnedMemory.ReadOnlyMemory point to the same memory
        static void Buffer(OwnedMemory<byte> buffer)
        {
            var rwBuffer = buffer.Memory;
            var rwSpan = rwBuffer.Span;

            ReadOnlyMemory<byte> roBuffer = buffer.Memory;
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

        static void Pin(OwnedMemory<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Retain(pin: true);
            unsafe
            {
                Assert.NotEqual(0L, new IntPtr(handle.Pointer).ToInt64());
            }
            handle.Dispose();
        }

        static void Dispose(OwnedMemory<byte> buffer)
        {
            var length = buffer.Length;

            buffer.Release();

            buffer.Dispose();
            Assert.True(buffer.IsDisposed);

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var ignore = buffer.Span;
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.Span.Slice(0, length);
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.Pin();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var rwBuffer = buffer.Memory;
            });

            Assert.ThrowsAny<ObjectDisposedException>((Action)(() =>
            {
                ReadOnlyMemory<byte> roBuffer = buffer.Memory;
            }));
        }

        static void DisposeAuto(OwnedMemory<byte> buffer)
        {
            var length = buffer.Length;

            buffer.Dispose();
            Assert.True(buffer.IsDisposed);

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var ignore = buffer.Span;
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.Span.Slice(0, length);
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.Pin();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var rwBuffer = buffer.Memory;
            });

            Assert.ThrowsAny<ObjectDisposedException>((Action)(() =>
            {
                ReadOnlyMemory<byte> roBuffer = buffer.Memory;
            }));
        }

        static void OverRelease(OwnedMemory<byte> buffer)
        {
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                buffer.Release();
            });
        }

        static void BufferLifetimeBasics(OwnedMemory<byte> buffer)
        {
            Memory<byte> copyStoredForLater;
            try
            {
                Memory<byte> memory = buffer.Memory;
                Memory<byte> slice = memory.Slice(10);
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
                buffer.Release();
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

        static void BufferLifetimeBasicsAuto(OwnedMemory<byte> buffer)
        {
            Memory<byte> copyStoredForLater;
            try
            {
                Memory<byte> memory = buffer.Memory;
                Memory<byte> slice = memory.Slice(10);
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

        static void MemoryHandleDoubleFree(OwnedMemory<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Retain(pin: true);
            buffer.Retain();
            handle.Dispose();
            handle.Dispose();
            Assert.True(buffer.Release());
            Assert.False(buffer.Release());
        }

        static void MemoryHandleDoubleFreeAuto(OwnedMemory<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Retain(pin: true);
            buffer.Retain();
            handle.Dispose();
            handle.Dispose();
            Assert.False(buffer.Release());
        }

        public static void TestBuffer(Func<Memory<byte>> create)
        {
            BufferBasics(create());
            BufferLifetime(create());
        }

        public static void TestBuffer(Func<ReadOnlyMemory<byte>> create)
        {
            BufferBasics(create());
            BufferLifetime(create());
        }

        static void BufferBasics(Memory<byte> buffer)
        {
            var span = buffer.Span;
            Assert.Equal(buffer.Length, span.Length);
            Assert.True(buffer.IsEmpty || buffer.Length != 0);
            Assert.True(!buffer.IsEmpty || buffer.Length == 0);

            for (int i = 0; i < span.Length; i++) span[i] = 100;

            var array = buffer.ToArray();
            for (int i = 0; i < array.Length; i++) Assert.Equal(array[i], span[i]);

            if (MemoryMarshal.TryGetArray<byte>(buffer, out var segment))
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

        static void BufferLifetime(Memory<byte> buffer)
        {
            var array = buffer.ToArray();
            using (var pinned = buffer.Retain(pin: true))
            {
                unsafe
                {
                    var p = (byte*)pinned.Pointer;
                    if (buffer.Length == 0)
                    {
                        Assert.True(null == p);
                    }
                    else
                    {
                        Assert.True(null != p);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            Assert.Equal(array[i], p[i]);
                        }
                    }
                }
            }

            // TODO: the following using statement does not work ...
            // AutoDisposeBuffer is getting disposed above. Are we ok with this?
            //using(var handle = buffer.Retain())
            //{
            //}
        }

        static void BufferBasics(ReadOnlyMemory<byte> buffer)
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

        static void BufferLifetime(ReadOnlyMemory<byte> buffer)
        {
            var array = buffer.ToArray();
            using (var pinned = buffer.Retain(pin: true))
            {
                unsafe
                {
                    var p = (byte*)pinned.Pointer;
                    if (buffer.Length == 0)
                    {
                        Assert.True(null == p);
                    }
                    else
                    {
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
}
