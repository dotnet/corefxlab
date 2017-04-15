// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that is owned by an external component.
    /// </summary>
    public class UnownedBuffer : OwnedBuffer<byte>
    {
        public UnownedBuffer(ArraySegment<byte> buffer)
        {
            _buffer = buffer;
        }

        public override int Length => _buffer.Count;

        public override Span<byte> Span => new Span<byte>(_buffer.Array, _buffer.Offset, _buffer.Count);

        public override Span<byte> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return Span.Slice(index, length);
        }

        public OwnedBuffer<byte> MakeCopy(int offset, int length, out int newStart, out int newEnd)
        {
            // Copy to a new Owned Buffer.
            var buffer = new byte[length];
            global::System.Buffer.BlockCopy(_buffer.Array, _buffer.Offset + offset, buffer, 0, length);
            newStart = 0;
            newEnd = length;
            return buffer;
        }

        public override BufferHandle Pin(int index = 0)
        {
            throw new NotImplementedException();
        }

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = _buffer;
            return true;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        private ArraySegment<byte> _buffer;
    }
}
