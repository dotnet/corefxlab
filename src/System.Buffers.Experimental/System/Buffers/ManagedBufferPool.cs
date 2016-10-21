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
            return new OwnedMemory<T>(array, this);
        }

        unsafe void IMemoryDisposer<T>.Return(OwnedMemory<T> buffer)
        {
            if (buffer?.Owner != this) throw new InvalidOperationException("buffer not rented from this pool");
            ArraySegment<T> ownedArray;
            buffer.Memory.TryGetArray(out ownedArray, null);
            ArrayPool<T>.Shared.Return(ownedArray.Array);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
