// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace Microsoft.Net.Http
{
    public static class MemoryPool {

        public static OwnedMemory<byte> Rent(int size)
        {
            return new PooledMemory(size);
        }

        class PooledMemory : OwnedMemory<byte>
        {
            public PooledMemory(int size) : base(ArrayPool<byte>.Shared.Rent(size)) { }

            protected override void Dispose(bool disposing)
            {
                var array = Array;
                base.Dispose(disposing);
                ArrayPool<byte>.Shared.Return(array);
            }
        }
    }
}
