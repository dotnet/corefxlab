// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    public unsafe struct PinnedMemoryHandle<T>
    {
        readonly OwnedMemory<T> _owner;
        readonly GCHandle _handle;
        readonly void* _pointer;

        internal PinnedMemoryHandle(OwnedMemory<T> owner, int index)
        {
            _owner = owner;
            if (_owner.TryGetPointerInternal(out _pointer))
            {
                _pointer = Memory<T>.Add(_pointer, index);
            }
            else
            {
                ArraySegment<T> buffer;
                if (_owner.TryGetArrayInternal(out buffer))
                {
                    _handle = GCHandle.Alloc(buffer.Array, GCHandleType.Pinned);
                    _pointer = Memory<T>.Add((void*)_handle.AddrOfPinnedObject(), buffer.Offset + index);
                }
                else
                {
                    throw new InvalidOperationException("Memory cannot be pinned");
                }
            }

            _owner.AddReference();
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
