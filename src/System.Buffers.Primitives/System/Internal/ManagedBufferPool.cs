// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal sealed class ManagedBufferPool : BufferPool
    {
        readonly static ManagedBufferPool s_shared = new ManagedBufferPool();

        public static ManagedBufferPool Shared
        {
            get
            {
                return s_shared;
            }
        }

        public override OwnedBuffer<byte> Rent(int minimumBufferSize)
        {
            return new ArrayPoolMemory(minimumBufferSize);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private class ArrayPoolMemory : OwnedBuffer<byte>
        {
            public ArrayPoolMemory(int size) : base(ArrayPool<byte>.Shared.Rent(size))
            {
            }

            protected override void Dispose(bool disposing)
            {
                ArrayPool<byte>.Shared.Return(Array);

                base.Dispose(disposing);
            }
        }
    }
}
