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

        public override OwnedMemory<byte> Rent(int minimumBufferSize)
        {
            var array = ArrayPool<byte>.Shared.Rent(minimumBufferSize);
            return new OwnedArray<byte>(array);
        }

        public override void Return(OwnedMemory<byte> buffer)
        {
            ArraySegment<byte> segment;
            if (!buffer.Memory.TryGetArray(out segment)) {
                throw new Exception();
            }
            buffer.Dispose();
            ArrayPool<byte>.Shared.Return(segment.Array);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
