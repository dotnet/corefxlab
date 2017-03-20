// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Pools;

namespace System.Buffers
{
    public abstract class BufferPool : IDisposable
    {
        public static BufferPool Default => ManagedBufferPool.Shared;

        public abstract OwnedBuffer<byte> Rent(int minimumBufferSize);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BufferPool()
        {
            Dispose(false);
        }

        protected abstract void Dispose(bool disposing);
    }
}