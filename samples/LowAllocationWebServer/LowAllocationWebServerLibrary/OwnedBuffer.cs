// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Sequences;

namespace Microsoft.Net.Http
{
    class OwnedBuffer : OwnedBuffer<byte>, IBufferList<byte>, IReadOnlyBufferList<byte>
    {
        public const int DefaultBufferSize = 1024;

        public OwnedBuffer(int desiredSize = DefaultBufferSize)
        {
            _array = Allocate(desiredSize);
        }

        static byte[] Allocate(int size)
        {
            return new byte[size];
        }
        static void Free(byte[] array)
        {
        }

        public Buffer<byte> First => Buffer;

        public IBufferList<byte> Rest => _next;

        public int WrittenByteCount => _written;

        ReadOnlyBuffer<byte> IReadOnlyBufferList<byte>.First => Buffer;

        IReadOnlyBufferList<byte> IReadOnlyBufferList<byte>.Rest => _next;

        public override int Length => _array.Length;

        public override Span<byte> Span => _array;

        public int CopyTo(Span<byte> buffer)
        {
            if (buffer.Length > _written) {
                Buffer.Slice(0, _written).CopyTo(buffer);
                return _next.CopyTo(buffer.Slice(_written));
            }

            Buffer.Slice(0, buffer.Length).CopyTo(buffer);
            return buffer.Length;
        }

        public bool TryGet(ref Position position, out Buffer<byte> item, bool advance = true)
        {
            if (position == Position.First) {
                item = Buffer.Slice(0, _written);
                if (advance) { position.IntegerPosition++; position.ObjectPosition = _next; }
                return true;
            }
            else if (position.ObjectPosition == null) { item = default(Buffer<byte>); return false; }

            var sequence = (OwnedBuffer)position.ObjectPosition;
            item = sequence.Buffer.Slice(0, _written);
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

        public bool TryGet(ref Position position, out ReadOnlyBuffer<byte> item, bool advance = true)
        {
            if (position == Position.First) {
                item = Buffer.Slice(0, _written);
                if (advance) { position.IntegerPosition++; position.ObjectPosition = _next; }
                return true;
            }
            else if (position.ObjectPosition == null) { item = default(ReadOnlyBuffer<byte>); return false; }

            var sequence = (OwnedBuffer)position.ObjectPosition;
            item = sequence.Buffer.Slice(0, _written);
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

        public OwnedBuffer Enlarge(int desiredSize = DefaultBufferSize)
        {
            _next = new OwnedBuffer(desiredSize);
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
            Free(array);
            if (_next != null) {
                _next.Dispose();
            }
            _next = null;
        }

        public override Span<byte> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return Span.Slice(index, length);
        }

        public override BufferHandle Pin(int index = 0)
        {
            throw new NotImplementedException();
        }

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = new ArraySegment<byte>(_array);
            return true;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        internal OwnedBuffer _next;
        int _written;
        byte[] _array;
    }
}
