// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Native
{
    public unsafe sealed partial class NativeMemoryPool : MemoryPool<byte>
    {
        internal sealed class Memory : ReferenceCountedMemory<byte>
        {
            public Memory(NativeMemoryPool pool, IntPtr memory, int length)
            {
                _pool = pool;
                _pointer = memory;
                _length = length;
            }

            public IntPtr Pointer => _pointer;

            public override Span<byte> GetSpan()
            {
                if (IsDisposed) BuffersExperimentalThrowHelper.ThrowObjectDisposedException(nameof(NativeMemoryPool.Memory));
                return new Span<byte>(_pointer.ToPointer(), _length);
            }

            protected override void Dispose(bool disposing)
            {
                _pool.Return(this);
                base.Dispose(disposing);
            }

            protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
            {
                arraySegment = default;
                return false;
            }

            public override MemoryHandle Pin(int elementIndex = 0)
            {
                Retain();
                if (elementIndex < 0 || elementIndex > _length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
                return new MemoryHandle(Unsafe.Add<byte>(_pointer.ToPointer(), elementIndex), default, this);
            }

            public override void Unpin()
            {
                Release();
            }

            private readonly NativeMemoryPool _pool;
            IntPtr _pointer;
            int _length;
        }
    }
}
