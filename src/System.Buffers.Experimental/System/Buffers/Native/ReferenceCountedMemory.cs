// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System.Buffers.Native
{
    public abstract class ReferenceCountedMemory<T> : MemoryManager<T>
    {
        int _referenceCount;
        bool _disposed;

        public void Retain()
        {
            if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(ReferenceCountedMemory<T>));
            Interlocked.Increment(ref _referenceCount);
        }

        public bool Release()
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

        protected bool IsRetained => _referenceCount > 0;

        protected virtual void OnNoReferences()
        {
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = disposing;
        }

        public bool IsDisposed => _disposed;
    }
}
