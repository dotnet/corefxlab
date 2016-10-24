// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public sealed class ManagedBufferPool<T> : IBufferPool<T>
    {
        static ManagedBufferPool<T> s_shared = new ManagedBufferPool<T>();

        public static ManagedBufferPool<T> Shared
        {
            get
            {
                return s_shared;
            }
        }

        public OwnedMemory<T> Rent(int minimumBufferSize)
        {
            var array = ArrayPool<T>.Shared.Rent(minimumBufferSize);
            return new OwnedArray<T>(array);
        }

        public void Return(OwnedMemory<T> buffer)
        {
            var ownedArray = buffer as OwnedArray<T>;
            if (ownedArray == null) throw new InvalidOperationException("buffer not rented from this pool");
            ArrayPool<T>.Shared.Return(ownedArray.Array);
            buffer.Dispose();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
