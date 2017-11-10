// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Runtime.InteropServices;

namespace Microsoft.Net
{
    class BufferSequence : ReferenceCountedBuffer<byte>, IMemorySequence<byte>, IReadOnlyMemoryList<byte>
    {
        public const int DefaultBufferSize = 1024;

        byte[] _array;
        int _written;
        BufferSequence _next;

        public BufferSequence(int desiredSize = DefaultBufferSize)
        {
            _array = new byte[desiredSize];
        }

        public Memory<byte> First => Memory;

        public override Span<byte> Span
        {
            get {
                if (IsDisposed) throw new ObjectDisposedException(nameof(BufferSequence));
                return _array.AsSpan();
            }
        }

        public IMemorySequence<byte> Rest => _next;

        public int WrittenByteCount => _written;

        public override int Length => _array.Length;

        public long Index => throw new NotImplementedException();

        IReadOnlyMemoryList<byte> IReadOnlyMemoryList<byte>.Rest => _next;

        ReadOnlyMemory<byte> IReadOnlyMemorySequence<byte>.First => First;

        public int CopyTo(Span<byte> buffer)
        {
            if (buffer.Length > _written) {
                Memory.Slice(0, _written).Span.CopyTo(buffer);
                return _next.CopyTo(buffer.Slice(_written));
            }

            Memory.Slice(0, buffer.Length).Span.CopyTo(buffer);
            return buffer.Length;
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            if (position == Position.First) {
                item = Memory.Slice(0, _written);
                if (advance) { position.IntegerPosition++; position.ObjectPosition = _next; }
                return true;
            }
            else if (position.ObjectPosition == null) { item = default; return false; }

            var sequence = (BufferSequence)position.ObjectPosition;
            item = sequence.Memory.Slice(0, _written);
            if (advance) {
                if (position == Position.First) {
                    position.ObjectPosition = _next;
                }
                else {
                    position.ObjectPosition = sequence._next;
                }
                position.IntegerPosition++;
            }
            return true;
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == Position.First) {
                item = Memory.Slice(0, _written);
                if (advance) { position.IntegerPosition++; position.ObjectPosition = _next; }
                return true;
            }
            else if (position.ObjectPosition == null) { item = default; return false; }

            var sequence = (BufferSequence)position.ObjectPosition;
            item = sequence.Memory.Slice(0, _written);
            if (advance) {
                if (position == Position.First) {
                    position.ObjectPosition = _next;
                }
                else {
                    position.ObjectPosition = sequence._next;
                }
                position.IntegerPosition++;
            }
            return true;
        }

        public BufferSequence Append(int desiredSize = DefaultBufferSize)
        {
            _next = new BufferSequence(desiredSize);
            return _next;
        }

        public void Advance(int bytes)
        {
            _written = bytes;
        }

        protected override void Dispose(bool disposing)
        {
            var array = _array;
            base.Dispose(disposing);
            if (_next != null) {
                _next.Dispose();
            }
            _next = null;
        }

        protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(BufferSequence));
            arraySegment = new ArraySegment<byte>(_array);
            return true;
        }

        public override MemoryHandle Pin()
        {
            unsafe
            {
                Retain();
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                return new MemoryHandle(this, (void*)handle.AddrOfPinnedObject(), handle);
            }
        }
    }
}
