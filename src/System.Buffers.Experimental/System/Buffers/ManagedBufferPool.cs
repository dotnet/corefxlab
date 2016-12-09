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
            var ownedArray = buffer as OwnedArray<byte>;
            if (ownedArray == null) throw new InvalidOperationException("buffer not rented from this pool");
            ArrayPool<byte>.Shared.Return(ownedArray.Array);
            buffer.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
