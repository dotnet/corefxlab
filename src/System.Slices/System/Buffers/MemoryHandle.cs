// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Buffers
{
    public unsafe struct MemoryHandle
    {
        IKnown _owner;
        GCHandle _handle;
        void* _pointer;

        internal static MemoryHandle Create<T>(OwnedMemory<T> owner, int index)
        {
            void* pointer;
            GCHandle handle;
            if (owner.TryGetPointerInternal(out pointer))
            {
                pointer = Memory<T>.Add(pointer, index);
            }
            else
            {
                ArraySegment<T> buffer;
                if (owner.TryGetArrayInternal(out buffer))
                {
                    handle = GCHandle.Alloc(buffer.Array, GCHandleType.Pinned);
                    pointer = Memory<T>.Add((void*)handle.AddrOfPinnedObject(), buffer.Offset + index);
                }
                else
                {
                    throw new InvalidOperationException("Memory cannot be pinned");
                }
            }

            owner.AddReference();

            return new MemoryHandle {
                _owner = owner,
                _handle = handle,
                _pointer = pointer
            };
        }

        public void* PinnedPointer
        {
            get
            {
                return _pointer;
            }
        }

        public void Free()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }

            _owner.Release();
        }
    }
}
