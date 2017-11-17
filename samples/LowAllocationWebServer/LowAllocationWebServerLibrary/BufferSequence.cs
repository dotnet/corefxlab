// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Sequences;

namespace Microsoft.Net
{
    class BufferSequence : IMemorySegment<byte>, IDisposable
    {
        public const int DefaultBufferSize = 1024;

        byte[] _array;
        int _written;
        BufferSequence _next;

        public BufferSequence(int desiredSize = DefaultBufferSize)
        {
            _array = ArrayPool<byte>.Shared.Rent(desiredSize);
        }

        public Memory<byte> Memory => new Memory<byte>(_array);

        public Memory<byte> WrittenMemory => new Memory<byte>(_array, 0, _written);

        public ReadOnlySpan<byte> Written => new ReadOnlySpan<byte>(_array, 0, _written);

        public Span<byte> Free => new Span<byte>(_array, _written, _array.Length - _written);

        public IMemorySegment<byte> Rest => _next;

        public int WrittenByteCount => _written;

        public long VirtualIndex => throw new NotImplementedException();

        public int CopyTo(Span<byte> buffer)
        {
            if (buffer.Length > _written) {
                Written.CopyTo(buffer);
                return _next.CopyTo(buffer.Slice(_written));
            }

            Written.Slice(0, buffer.Length).CopyTo(buffer);
            return buffer.Length;
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            if (position == default) {
                item = Memory.Slice(0, _written);
                if (advance) { position.SetItem(_next); }
                return true;
            }
            else if (position.IsEnd) { item = default; return false; }

            var sequence = position.GetItem<BufferSequence>();
            item = sequence.Memory.Slice(0, sequence._written);
            if (advance) { position.SetItem(sequence._next); }
            return true;
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                item = Memory.Slice(0, _written);
                if (advance) { position.SetItem(_next); }
                return true;
            }
            else if (position.IsEnd) { item = default; return false; }

            var sequence = position.GetItem<BufferSequence>();
            item = sequence.WrittenMemory;
            if (advance) { position.SetItem(sequence._next); }
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

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            var array = _array;
            _array = null;
            if (array != null) ArrayPool<byte>.Shared.Return(array);
            if (_next != null) {
                _next.Dispose();
            }
            _next = null;
        }
    }
}
