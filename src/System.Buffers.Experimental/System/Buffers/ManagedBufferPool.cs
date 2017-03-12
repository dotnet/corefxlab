// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Pools
{
    public sealed class ManagedBufferPool : BufferPool
    {
        static ManagedBufferPool s_shared = new ManagedBufferPool();

        public static ManagedBufferPool Shared
        {
            get
            {
                return s_shared;
            }
        }

        public override OwnedMemory<byte> Rent(int minimumBufferSize)
        {
            return new ArrayPoolMemory(minimumBufferSize);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private class ArrayPoolMemory : OwnedMemory<byte>
        {
            public ArrayPoolMemory(int size) : base(ArrayPool<byte>.Shared.Rent(size))
            {
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                ArrayPool<byte>.Shared.Return(Array);
            }
        }
    }
}
