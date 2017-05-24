// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that is owned by an external component.
    /// </summary>
    public class UnownedBuffer : OwnedBuffer<byte>
    {
        private ArraySegment<byte> _buffer;
        private int _referenceCount;
        private bool _disposed;

        public UnownedBuffer(ArraySegment<byte> buffer)
        {
            _buffer = buffer;
        }

        public override int Length => _buffer.Count;

        public override Span<byte> AsSpan(int index, int length)
        {
            if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(UnownedBuffer));
            if (length > _buffer.Count - index) throw new ArgumentOutOfRangeException();
            return new Span<byte>(_buffer.Array, _buffer.Offset + index, length);
        }

        public OwnedBuffer<byte> MakeCopy(int offset, int length, out int newStart, out int newEnd)
        {
            // Copy to a new Owned Buffer.
            var buffer = new byte[length];
            global::System.Buffer.BlockCopy(_buffer.Array, _buffer.Offset + offset, buffer, 0, length);
            newStart = 0;
            newEnd = length;
            return (OwnedArray<byte>)buffer;
        }

// In kestrel both MemoryPoolBlock and OwnedBuffer end up in the same assembly so
// this method access modifiers need to be `protected internal`
#if KESTREL_BY_SOURCE
        internal
#endif
        protected override bool TryGetArray(out ArraySegment<byte> buffer)
        {
            if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(UnownedBuffer));
            buffer = _buffer;
            return true;
        }

        // In kestrel both MemoryPoolBlock and OwnedBuffer end up in the same assembly so
        // this method access modifiers need to be `protected internal`
#if KESTREL_BY_SOURCE
        internal
#endif
        public override BufferHandle Pin(int index = 0)
        {
            unsafe
            {
                Retain();
                var handle = GCHandle.Alloc(_buffer.Array, GCHandleType.Pinned);
                var pointer = Add((void*)handle.AddrOfPinnedObject(), index + _buffer.Offset);
                return new BufferHandle(this, pointer, handle);
            }
        }

        public override void Retain()
        {
            if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(UnownedBuffer));
            Interlocked.Increment(ref _referenceCount);
        }

        public override void Release()
        {
            Interlocked.Decrement(ref _referenceCount);
        }

        public override bool IsRetained => _referenceCount > 0;

        protected override void Dispose(bool disposing)
        {
            _disposed = disposing;
        }

        public override bool IsDisposed => _disposed;
    }
}
