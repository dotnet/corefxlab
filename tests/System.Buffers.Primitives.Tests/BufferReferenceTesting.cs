// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceTests
    {
        public static void Run(Func<OwnedBuffer<byte>> create)
        {
            BufferLifetimeBasics(create());
            ThrowOnAccessAfterDispose(create());
            PinAddReferenceRelease(create());
            MemoryHandleDoubleFree(create());
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
            Assert.Throws<ObjectDisposedException>(() =>
            {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }

        static void ThrowOnAccessAfterDispose(OwnedBuffer<byte> buffer)
        {
            var length = buffer.Length;
            var span = buffer.AsSpan();
            Assert.Equal(length, span.Length);
            buffer.Release();
            buffer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => {
                var spanDisposed = buffer.AsSpan();
            });

            Assert.Throws<ObjectDisposedException>(() => {
                var spanDisposed = buffer.AsSpan(0, length);
            });
        }

        static void PinAddReferenceRelease(OwnedBuffer<byte> buffer)
        {
            var memory = buffer.Buffer;
            Assert.False(buffer.IsRetained);
            var h = memory.Pin();
            Assert.True(buffer.IsRetained);
            h.Dispose();
            Assert.False(buffer.IsRetained);
        }

        static void MemoryHandleDoubleFree(OwnedBuffer<byte> buffer)
        {
            var memory = buffer.Buffer;
            var h = memory.Pin();
            Assert.True(buffer.IsRetained);
            buffer.Retain();
            Assert.True(buffer.IsRetained);
            h.Dispose();
            Assert.True(buffer.IsRetained);
            h.Dispose();
            Assert.True(buffer.IsRetained);
            buffer.Release();
            Assert.False(buffer.IsRetained);
        }
    }
}
