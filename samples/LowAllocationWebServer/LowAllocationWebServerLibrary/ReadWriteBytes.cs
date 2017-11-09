// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public struct ReadWriteBytes : IMemorySequence<byte>
    {
        Memory<byte> _first;
        IMemorySequence<byte> _rest;
        long _length;

        static readonly ReadWriteBytes s_empty = new ReadWriteBytes(Memory<byte>.Empty);

        public ReadWriteBytes(Memory<byte> first, IMemorySequence<byte> rest, long length)
        {
            _rest = rest;
            _first = first;
            _length = length;
        }

        public ReadWriteBytes(Memory<byte> first, IMemorySequence<byte> rest) :
            this(first, rest, Unspecified)
        { }

        public ReadWriteBytes(Memory<byte> buffer) :
            this(buffer, null, buffer.Length)
        { }

        public ReadWriteBytes(IMemorySequence<byte> segments, long length) :
            this(segments.First, segments.Rest, length)
        { }

        public ReadWriteBytes(IMemorySequence<byte> segments) :
            this(segments.First, segments.Rest, Unspecified)
        { }

        public bool TryGet(ref Position position, out Memory<byte> value, bool advance = true)
        {
            if (position == Position.First)
            {
                value = _first;
                if (advance)
                {
                    position.IntegerPosition++;
                    position.Tag = _first.Length; // this is needed to know how much to slice off the last segment; see below
                    position.ObjectPosition = _rest;
                }
                return true;
            }

            var rest = position.ObjectPosition as IMemorySequence<byte>;
            if (rest == null)
            {
                value = default;
                return false;
            }

            value = rest.First;
            // we need to slice off the last segment based on length of this. ReadOnlyBytes is a potentially shorted view over a longer buffer list.
            if (value.Length + position.Tag > _length && _length != Unspecified)
            {
                value = value.Slice(0, (int)(_length - position.Tag));
                if (value.Length == 0) return false;
            }
            if (advance)
            {
                position.IntegerPosition++;
                position.Tag += value.Length;
                position.ObjectPosition = rest.Rest;
            }
            return true;
        }

        public Memory<byte> First => _first;

        public IMemorySequence<byte> Rest => _rest;

        public static ReadWriteBytes Empty => s_empty;

        ReadOnlyMemory<byte> IReadOnlyMemorySequence<byte>.First => _first;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(long index, long length)
        {
            if (First.Length >= length + index)
            {
                return new ReadWriteBytes(First.Slice((int)index, (int)length));
            }
            if (First.Length > index)
            {
                return new ReadWriteBytes(_first.Slice((int)index), _rest, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(long index)
        {
            return Slice(index, ComputeLength() - index); // TODO (pri 1): this computes the Length; it should not.
        }

        public int CopyTo(Span<byte> buffer)
        {
            var first = First;
            if (first.Length > buffer.Length)
            {
                first.Slice(buffer.Length).Span.CopyTo(buffer);
                return buffer.Length;
            }
            first.Span.CopyTo(buffer);
            return first.Length + _rest.CopyTo(buffer.Slice(first.Length));
        }

        ReadWriteBytes SliceRest(long index, long length)
        {
            if (_rest == null)
            {
                if (First.Length == index && length == 0)
                {
                    return new ReadWriteBytes(Memory<byte>.Empty);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index or length");
                }
            }

            // TODO (pri 2): this could be optimized
            var rest = new ReadWriteBytes(_rest, length);
            rest = rest.Slice(index - First.Length, length);
            return rest;
        }

        // TODO (pri 3): do we want this to be public? Maybe Lazy<int> length?
        // TODO (pri 3): the problem is that this makes the type mutable, and since the type is a struct, the mutation can be lost when the stack unwinds.
        public long ComputeLength()
        {
            if (_length != Unspecified) return _length;

            int length = 0;
            if (_rest != null)
            {
                Position position = new Position();
                while (_rest.TryGet(ref position, out Memory<byte> segment))
                {
                    length += segment.Length;
                }
            }
            return length + _first.Length;
        }

        // this is used for unspecified _length; ReadOnlyBytes can be created from list of buffers of unknown total size, 
        // and knowing the length is not needed in amy operations, e.g. slicing small section of the front.
        const int Unspecified = -1;

        class BufferListNode : IMemorySequence<byte>
        {
            internal Memory<byte> _first;
            internal BufferListNode _rest;
            public Memory<byte> First => _first;

            public IMemorySequence<byte> Rest => _rest;

            ReadOnlyMemory<byte> IReadOnlyMemorySequence<byte>.First => _first;

            public int CopyTo(Span<byte> buffer)
            {
                int copied = 0;
                var position = Position.First;
                var free = buffer;
                while (TryGet(ref position, out Memory<byte> segment, true))
                {
                    if (segment.Length > free.Length)
                    {
                        segment.Span.Slice(0, free.Length).CopyTo(free);
                        copied += free.Length;
                    }
                    else
                    {
                        segment.Span.CopyTo(free);
                        copied += segment.Length;
                    }
                    free = buffer.Slice(copied);
                    if (free.Length == 0) break;
                }
                return copied;
            }

            public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
            {
                if (position == Position.First)
                {
                    item = _first;
                    if (advance) { position.IntegerPosition++; position.ObjectPosition = _rest; }
                    return true;
                }
                else if (position.ObjectPosition == null) { item = default; return false; }

                var sequence = (BufferListNode)position.ObjectPosition;
                item = sequence._first;
                if (advance)
                {
                    if (position == Position.First)
                    {
                        position.ObjectPosition = _rest;
                    }
                    else
                    {
                        position.ObjectPosition = sequence._rest;
                    }
                    position.IntegerPosition++;
                }
                return true;
            }

            public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
            {
                if(TryGet(ref position, out Memory<byte> memory, advance))
                {
                    item = memory;
                    return true;
                }
                item = default;
                return false;
            }
        }

        public static ReadWriteBytes Create(params byte[][] buffers)
        {
            BufferListNode first = null;
            BufferListNode current = null;
            foreach (var buffer in buffers)
            {
                if (first == null)
                {
                    current = new BufferListNode();
                    first = current;
                }
                else
                {
                    current._rest = new BufferListNode();
                    current = current._rest;
                }
                current._first = buffer;
            }
            return new ReadWriteBytes(first);
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (TryGet(ref position, out Memory<byte> memory, advance))
            {
                item = memory;
                return true;
            }
            item = default;
            return false;
        }
    }
}
