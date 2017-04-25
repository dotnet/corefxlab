// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

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
        public bool IsDisposed => _disposed;

        public void Dispose()
        {
            if (HasOutstandingReferences) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = disposing;
        }

        public abstract bool HasOutstandingReferences { get; }

        public abstract void AddReference();

        public abstract void Release();

        protected virtual void OnZeroReferences()
        { }

        bool _disposed;
        #endregion
    }

    public abstract class ReferenceCountedBuffer<T> : OwnedBuffer<T>
    {
        int _referenceCount;

        public override void AddReference()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public override void Release()
        {
            if (Interlocked.Decrement(ref _referenceCount) == 0) {
                OnZeroReferences();
            }
        }

        public override bool HasOutstandingReferences => _referenceCount > 0;
    }
}