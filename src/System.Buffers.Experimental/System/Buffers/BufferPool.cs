// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public abstract class BufferPool : IDisposable
    {
        public abstract OwnedMemory<byte> Rent(int minimumBufferSize);

        public abstract void Return(OwnedMemory<byte> buffer);

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