// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
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

        public override UnsafeMemory<byte> Rent(int minimumBufferSize)
        {
            var array = ArrayPool<byte>.Shared.Rent(minimumBufferSize);
            return new UnsafeMemory<byte>(array, 0, array.Length);
        }

        public override void Return(UnsafeMemory<byte> buffer)
        {
            ArraySegment<byte> segment;
            unsafe
            {
                if(!buffer.TryGetArray(out segment)) {
                    throw new Exception("this buffer was not rented from this pool.");
                }
            }
            ArrayPool<byte>.Shared.Return(segment.Array);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
