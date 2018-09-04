// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceTests
    {
        public static void TestMemoryManager(Func<MemoryManager<byte>> create, Action<MemoryManager<byte>> dispose = null)
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

        public static void TestAutoOwnedBuffer(Func<MemoryManager<byte>> create, Action<MemoryManager<byte>> dispose = null)
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

        static void RunTest(Action<MemoryManager<byte>> test, Func<MemoryManager<byte>> create,
            Action<MemoryManager<byte>> dispose)
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

        static void MemoryAccessBasics(MemoryManager<byte> buffer)
        {
            var span = buffer.GetSpan();
            span[10] = 10;

            var memory = buffer.Memory;
            Assert.Equal(memory.Length, span.Length);
            Assert.Equal(10, span[10]);

            Assert.Equal(10, memory.Span[10]);

            var array = memory.ToArray();
            Assert.Equal(memory.Length, array.Length);
            Assert.Equal(10, array[10]);

            Span<byte> copy = new byte[20];
            memory.Span.Slice(10, 20).CopyTo(copy);
            Assert.Equal(10, copy[0]);
        }

        // tests OwnedMemory.Span overloads
        static void Span(MemoryManager<byte> buffer)
        {
            var span = buffer.GetSpan();
            var memory = buffer.Memory;
            var fullSlice = buffer.GetSpan().Slice(0, memory.Length);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (byte)(i % 254 + 1);
                Assert.Equal(span[i], fullSlice[i]);
            }

            var slice = buffer.GetSpan().Slice(5, memory.Length - 5);
            Assert.Equal(span.Length - 5, slice.Length);

            for (int i = 0; i < slice.Length; i++)
            {
                Assert.Equal(span[i + 5], slice[i]);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.GetSpan().Slice(memory.Length, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                buffer.GetSpan().Slice(1, memory.Length);
            });
        }

        // tests that OwnedMemory.Memory and OwnedMemory.ReadOnlyMemory point to the same memory
        static void Buffer(MemoryManager<byte> buffer)
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

        static void Pin(MemoryManager<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Pin();
            unsafe
            {
                Assert.NotEqual(0L, new IntPtr(handle.Pointer).ToInt64());
            }
            handle.Dispose();
        }

        static void Dispose(MemoryManager<byte> buffer)
        {
            var length = buffer.Memory.Length;

            ((IDisposable)buffer).Dispose();

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var ignore = buffer.GetSpan();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.GetSpan().Slice(0, length);
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

        static void DisposeAuto(MemoryManager<byte> buffer)
        {
            var length = buffer.Memory.Length;

            ((IDisposable)buffer).Dispose();

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                var ignore = buffer.GetSpan();
            });

            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                buffer.GetSpan().Slice(0, length);
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

        static void OverRelease(MemoryManager<byte> buffer)
        {
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                buffer.Unpin();
            });
        }

        static void BufferLifetimeBasics(MemoryManager<byte> buffer)
        {
            Memory<byte> copyStoredForLater;
            try
            {
                Memory<byte> memory = buffer.Memory;
                Memory<byte> slice = memory.Slice(10);
                copyStoredForLater = slice;
                MemoryHandle handle = slice.Pin();
                handle.Dispose(); // release reservation
            }
            finally
            {
                ((IDisposable)buffer).Dispose(); // can finish dispose with no exception
            }
            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }

        static void BufferLifetimeBasicsAuto(MemoryManager<byte> buffer)
        {
            Memory<byte> copyStoredForLater;
            try
            {
                Memory<byte> memory = buffer.Memory;
                Memory<byte> slice = memory.Slice(10);
                copyStoredForLater = slice;
                var handle = slice.Pin();
                handle.Dispose(); // release reservation
            }
            finally
            {
                ((IDisposable)buffer).Dispose(); // can finish dispose with no exception
            }
            Assert.ThrowsAny<ObjectDisposedException>(() =>
            {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }

        static void MemoryHandleDoubleFree(MemoryManager<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Pin();
            buffer.Pin();
            handle.Dispose();
            handle.Dispose();
            buffer.Unpin();
            buffer.Unpin();
        }

        static void MemoryHandleDoubleFreeAuto(MemoryManager<byte> buffer)
        {
            var memory = buffer.Memory;
            var handle = memory.Pin();
            buffer.Pin();
            handle.Dispose();
            handle.Dispose();
            buffer.Unpin();
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
            using (var pinned = buffer.Pin())
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
            using (var pinned = buffer.Pin())
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
