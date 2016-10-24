// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public class ManagedBufferPool<T> : IMemoryAllocator<T>
    {
        static ManagedBufferPool<T> s_shared = new ManagedBufferPool<T>();

        public static ManagedBufferPool<T> Shared
        {
            get
            {
                return s_shared;
            }
        }

        public MemoryAllocatorType AllocatorType => MemoryAllocatorType.Pooled;

        public MemoryType MemoryType => MemoryType.Managed;

        public OwnedMemory<T> Allocate(int minimumBufferSize)
        {
            var array = ArrayPool<T>.Shared.Rent(minimumBufferSize);
            return new OwnedArray<T>(array, this);
        }

        unsafe void IMemoryCollector<T>.Deallocate(OwnedMemory<T> buffer)
        {
            if (buffer?.Owner != this) throw new InvalidOperationException("buffer not rented from this pool");

            ArraySegment<T> ownedArray;
            buffer.Memory.TryGetArray(out ownedArray, null);
            ArrayPool<T>.Shared.Return(ownedArray.Array);
        }

        public void Dispose()
        {

        }
    }
}
