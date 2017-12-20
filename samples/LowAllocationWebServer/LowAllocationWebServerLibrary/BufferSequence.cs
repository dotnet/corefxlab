// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Sequences;
using Position = System.Collections.Sequences.Position;

namespace Microsoft.Net
{
    class BufferSequence : IMemoryList<byte>, IDisposable
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

        public IMemoryList<byte> Next => _next;

        public int WrittenByteCount => _written;

        public long VirtualIndex => throw new NotImplementedException();

        public Position First => Position.Create(this);

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
                item = default;
                return false;
            }

            var (buffer, index) = position.Get<BufferSequence>();
            item = buffer.Memory.Slice(index, buffer._written - index);
            if (advance) { position = Position.Create(buffer._next); }
            return true;
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                item = default;
                return false;
            }

            var (buffer, index) = position.Get<BufferSequence>();
            item = buffer.WrittenMemory.Slice(index);
            if (advance) { position = Position.Create(buffer._next); }
            return true;
        }

        public BufferSequence Append(int desiredSize = DefaultBufferSize)
        {
            _next = new BufferSequence(desiredSize);
            return _next;
        }

        public void Advance(int bytes) => _written = bytes;

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
