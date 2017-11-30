// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public abstract class MemoryPool<T> : IDisposable
    {
        public static MemoryPool<T> Default => Internal.ArrayMemoryPool<T>.Shared;

        public abstract OwnedMemory<T> Rent(int minimumBufferSize);

        public abstract int MaxBufferSize { get;  }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MemoryPool()
        {
            Dispose(false);
        }

        protected abstract void Dispose(bool disposing);
    }
}
