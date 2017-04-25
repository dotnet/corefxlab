// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    public abstract class OwnedBuffer<T> : IDisposable, IKnown
    {
        protected OwnedBuffer() { }

        public abstract int Length { get; }

        public abstract Span<T> Span { get; }

        public Buffer<T> Buffer => new Buffer<T>(this, 0, Length);

        public ReadOnlyBuffer<T> ReadOnlyBuffer => new ReadOnlyBuffer<T>(this, 0, Length);

        public static implicit operator OwnedBuffer<T>(T[] array) => new Internal.OwnedArray<T>(array);

        public virtual BufferHandle Pin(int index = 0)
        {
            return BufferHandle.Create(this, index);
        }

        internal protected abstract bool TryGetArrayInternal(out ArraySegment<T> buffer);

        internal protected abstract unsafe bool TryGetPointerInternal(out void* pointer);

        #region Lifetime Management
        public abstract bool IsDisposed { get; }

        public void Dispose()
        {
            if (HasOutstandingReferences) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
        }

        protected abstract void Dispose(bool disposing);

        public abstract bool HasOutstandingReferences { get; }

        public abstract void AddReference();

        public abstract void Release();

        #endregion
    }
}