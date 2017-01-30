// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public interface IMemoryList<T> : ISequence<Memory<T>>
    {
        int CopyTo(Span<T> buffer);
        Memory<T> First { get; }
        IMemoryList<T> Rest { get; }
    }

    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public struct ReadWriteBytes : IMemoryList<byte>
    {
        Memory<byte> _first;
        IMemoryList<byte> _rest;
        int _length;

        static readonly ReadWriteBytes s_empty = new ReadWriteBytes(Memory<byte>.Empty);

        public ReadWriteBytes(Memory<byte> first, IMemoryList<byte> rest, int length)
        {
            _rest = rest;
            _first = first;
            _length = length;
        }

        public ReadWriteBytes(Memory<byte> first, IMemoryList<byte> rest) :
            this(first, rest, Unspecified)
        { }

        public ReadWriteBytes(Memory<byte> memory) :
            this(memory, null, memory.Length)
        { }

        public ReadWriteBytes(IMemoryList<byte> segments, int length) :
            this(segments.First, segments.Rest, length)
        { }

        public ReadWriteBytes(IMemoryList<byte> segments) :
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

            var rest = position.ObjectPosition as IMemoryList<byte>;
            if (rest == null)
            {
                value = default(Memory<byte>);
                return false;
            }

            value = rest.First;
            // we need to slice off the last segment based on length of this. ReadOnlyBytes is a potentially shorted view over a longer buffer list.
            if (value.Length + position.Tag > _length && _length != Unspecified)
            {
                value = value.Slice(0, _length - position.Tag);
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

        public Memory<Byte> First => _first;

        public IMemoryList<byte> Rest => _rest;

        public int? Length
        {
            get {
                if (_length == Unspecified)
                {
                    return null;
                }
                return _length;
            }
        }

        public static ReadWriteBytes Empty => s_empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(int index, int length)
        {
            if (First.Length >= length + index)
            {
                return new ReadWriteBytes(First.Slice(index, length));
            }
            if (First.Length > index)
            {
                return new ReadWriteBytes(_first.Slice(index), _rest, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(int index)
        {
            return Slice(index, ComputeLength() - index); // TODO (pri 1): this computes the Length; it should not.
        }

        public int CopyTo(Span<byte> buffer)
        {
            var first = First;
            if (first.Length > buffer.Length)
            {
                first.Slice(buffer.Length).CopyTo(buffer);
                return buffer.Length;
            }
            first.CopyTo(buffer);
            // TODO (pri 2): do we need to compute the length here?
            return first.Length + _rest.CopyTo(buffer.Slice(first.Length, ComputeLength() - first.Length));
        }

        ReadWriteBytes SliceRest(int index, int length)
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
        public int ComputeLength()
        {
            if (_length != Unspecified) return _length;

            int length = 0;
            if (_rest != null)
            {
                Position position = new Position();
                Memory<byte> segment;
                while (_rest.TryGet(ref position, out segment))
                {
                    length += segment.Length;
                }
            }
            return length + _first.Length;
        }

        // this is used for unspecified _length; ReadOnlyBytes can be created from list of buffers of unknown total size, 
        // and knowing the length is not needed in amy operations, e.g. slicing small section of the front.
        const int Unspecified = -1;

        class MemoryListNode : IMemoryList<byte>
        {
            internal Memory<byte> _first;
            internal MemoryListNode _rest;
            public Memory<byte> First => _first;

            public int? Length
            {
                get {
                    throw new NotImplementedException();
                }
            }

            public IMemoryList<byte> Rest => _rest;

            public int CopyTo(Span<byte> buffer)
            {
                int copied = 0;
                var position = Position.First;
                Memory<byte> segment;
                var free = buffer;
                while (TryGet(ref position, out segment, true))
                {
                    if (segment.Length > free.Length)
                    {
                        segment.Span.Slice(0, free.Length).CopyTo(free);
                        copied += free.Length;
                    }
                    else
                    {
                        segment.CopyTo(free);
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
                else if (position.ObjectPosition == null) { item = default(Memory<byte>); return false; }

                var sequence = (MemoryListNode)position.ObjectPosition;
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
        }

        public static ReadWriteBytes Create(params byte[][] buffers)
        {
            MemoryListNode first = null;
            MemoryListNode current = null;
            foreach (var buffer in buffers)
            {
                if (first == null)
                {
                    current = new MemoryListNode();
                    first = current;
                }
                else
                {
                    current._rest = new MemoryListNode();
                    current = current._rest;
                }
                current._first = buffer;
            }
            return new ReadWriteBytes(first);
        }
    }
}