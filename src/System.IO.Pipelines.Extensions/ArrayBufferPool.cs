// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    public class ArrayBufferPool : IBufferPool
    {
        public static readonly ArrayBufferPool Instance = new ArrayBufferPool(ArrayPool<byte>.Shared);

        private readonly ArrayPool<byte> _pool;

        public ArrayBufferPool(ArrayPool<byte> pool)
        {
            _pool = pool;
        }

        public OwnedMemory<byte> Lease(int size)
        {
            // Unfortunately this allocates.... (we could pool the owned array objects though)
            return new OwnedArray<byte>(_pool.Rent(size));
        }

        public void Dispose()
        {
            // Nothing to do here
        }
    }
}
