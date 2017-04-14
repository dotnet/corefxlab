// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Buffers.Pools
{
    public unsafe sealed partial class NativeBufferPool : BufferPool
    {
        internal sealed class BufferManager : OwnedBuffer<byte>
        {
            private readonly NativeBufferPool _pool;

            public BufferManager(NativeBufferPool pool, IntPtr memory, int length) : base(null, 0, length, memory)
            {
                _pool = pool;
            }

            public new IntPtr Pointer => base.Pointer;

            protected override void Dispose(bool disposing)
            {
                _pool.Return(this);

                base.Dispose(disposing);
            }
        }
    }
}
