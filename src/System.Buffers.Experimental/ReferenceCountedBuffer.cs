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

        public override bool Release()
        {
            int newRefCount = Interlocked.Decrement(ref _referenceCount);
            if (newRefCount < 0) BuffersExperimentalThrowHelper.ThrowInvalidOperationException();
            if (newRefCount == 0) 
            {
                OnNoReferences();
                return false;
            }
            return true;
        }

        protected override bool IsRetained => _referenceCount > 0;

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
