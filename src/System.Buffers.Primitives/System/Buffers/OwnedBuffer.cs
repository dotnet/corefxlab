// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    public abstract class OwnedBuffer<T> : IDisposable, IRetainable
    {
        protected OwnedBuffer() { }

        public abstract int Length { get; }

        public abstract Span<T> Span { get; }

        public Buffer<T> Buffer => new Buffer<T>(this, 0, Length);

        public ReadOnlyBuffer<T> ReadOnlyBuffer => new ReadOnlyBuffer<T>(this, 0, Length);

        public static implicit operator OwnedBuffer<T>(T[] array) => new Internal.OwnedArray<T>(array);

        public virtual BufferHandle Pin(int index = 0)
        {
            unsafe
            {
                void* pointer;
                var handle = default(GCHandle);

                if (!TryGetPointerAt(index, out pointer)) {
                    ArraySegment<T> buffer;
                    if (TryGetArrayInternal(out buffer)) {
                        handle = GCHandle.Alloc(buffer.Array, GCHandleType.Pinned);
                        pointer = Add((void*)handle.AddrOfPinnedObject(), buffer.Offset + index);
                    }
                    else {
                        throw new InvalidOperationException("Memory cannot be pinned");
                    }
                }

                Retain();
                return new BufferHandle(this, pointer, handle);
            }
        }

        protected static unsafe void* Add(void* pointer, int offset)
        {
            return (byte*)pointer + ((ulong)Unsafe.SizeOf<T>() * (ulong)offset);
        }

        internal protected abstract bool TryGetArrayInternal(out ArraySegment<T> buffer);

        internal protected abstract unsafe bool TryGetPointerAt(int index, out void* pointer);

        #region Lifetime Management
        public abstract bool IsDisposed { get; }

        public void Dispose()
        {
            if (IsRetained) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
        }

        protected abstract void Dispose(bool disposing);

        public abstract bool IsRetained { get; }

        public abstract void Retain();

        public abstract void Release();

        #endregion
    }
}