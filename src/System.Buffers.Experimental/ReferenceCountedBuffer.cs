// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System.Buffers
{
    public abstract class ReferenceCountedBuffer<T> : OwnedBuffer<T>
    {
        int _referenceCount;
        bool _disposed;

        public override void Retain()
        {
            if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(ReferenceCountedBuffer<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public override void Release()
        {
            if (!IsRetained) BuffersExperimentalThrowHelper.ThrowInvalidOperationException();
            if (Interlocked.Decrement(ref _referenceCount) <= 0) {
                OnNoReferences();
            }
        }

        public override bool IsRetained => _referenceCount > 0;

        protected virtual void OnNoReferences()
        {
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = disposing;
        }

        public override bool IsDisposed => _disposed;
    }
}
