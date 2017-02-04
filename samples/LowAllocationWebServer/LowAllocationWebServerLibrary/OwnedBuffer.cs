using System;
using System.Buffers;
using System.Collections.Sequences;

namespace Microsoft.Net.Http
{
    class OwnedBuffer : OwnedMemory<byte>, IMemoryList<byte>, IReadOnlyMemoryList<byte>
    {
        public const int DefaultBufferSize = 1024;
        internal OwnedBuffer _next;
        int _written;

        public OwnedBuffer(int desiredSize = DefaultBufferSize) : base(Allocate(desiredSize))
        { }

        static byte[] Allocate(int size)
        {
            return new byte[size];
        }
        static void Free(byte[] array)
        {
        }

        public Memory<byte> First => Memory;

        public IMemoryList<byte> Rest => _next;

        public int WrittenByteCount => _written;

        ReadOnlyMemory<byte> IReadOnlyMemoryList<byte>.First => Memory;

        IReadOnlyMemoryList<byte> IReadOnlyMemoryList<byte>.Rest => _next;

        // TODO: maybe these should be renamed to no conflict with OwnedMemory<T>.Length?
        int? ISequence<Memory<byte>>.Length => null;
        int? ISequence<ReadOnlyMemory<byte>>.Length => null;

        public int CopyTo(Span<byte> buffer)
        {
            if (buffer.Length > _written) {
                Memory.Slice(0, _written).CopyTo(buffer);
                return _next.CopyTo(buffer.Slice(_written));
            }

            Memory.Slice(0, buffer.Length).CopyTo(buffer);
            return buffer.Length;
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            if (position == Position.First) {
                item = Memory.Slice(0, _written);
                if (advance) { position.IntegerPosition++; position.ObjectPosition = _next; }
                return true;
            }
            else if (position.ObjectPosition == null) { item = default(Memory<byte>); return false; }

            var sequence = (OwnedBuffer)position.ObjectPosition;
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
            else if (position.ObjectPosition == null) { item = default(ReadOnlyMemory<byte>); return false; }

            var sequence = (OwnedBuffer)position.ObjectPosition;
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
            var array = Array;
            base.Dispose(disposing);
            Free(array);
            if (_next != null) {
                _next.Dispose();
            }
            _next = null;
        }
    }
}
