// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public struct ReadOnlyBytes : IReadOnlyMemoryList<byte>
    {
        ReadOnlyMemory<byte> _first;
        IReadOnlyMemoryList<byte> _rest;
        int _length;

        static readonly ReadOnlyBytes s_empty = new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyMemoryList<byte> rest, int length)
        {
            _rest = rest;
            _first = first;
            _length = length;
        }

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyMemoryList<byte> rest) :
            this(first, rest, Unspecified)
        { }

        public ReadOnlyBytes(ReadOnlyMemory<byte> memory) :
            this(memory, null, memory.Length)
        { }

        public ReadOnlyBytes(IReadOnlyMemoryList<byte> segments, int length) :
            this(segments.First, segments.Rest, length)
        { }

        public ReadOnlyBytes(IReadOnlyMemoryList<byte> segments) :
            this(segments.First, segments.Rest, Unspecified)
        { }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> value, bool advance = true)
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

            var rest = position.ObjectPosition as IReadOnlyMemoryList<byte>;
            if (rest == null)
            {
                value = default(ReadOnlyMemory<byte>);
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

        public ReadOnlyMemory<Byte> First => _first;

        public IReadOnlyMemoryList<byte> Rest => _rest;

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

        public static ReadOnlyBytes Empty => s_empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(int index, int length)
        {
            if (First.Length >= length + index)
            {
                return new ReadOnlyBytes(First.Slice(index, length));
            }
            if (First.Length > index)
            {
                return new ReadOnlyBytes(_first.Slice(index), _rest, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(int index)
        {
            return Slice(index, ComputeLength() - index); // TODO (pri 1): this computes the Length; it should not.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(ReadOnlySpan<byte> bytes)
        {
            var first = _first.Span;
            var index = first.IndexOf(bytes);
            if (index != -1) return index;

            return first.IndexOfStraddling(_rest, bytes);
        }

        public int IndexOf(byte value)
        {
            var first = _first.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;

            if (_rest == null) return -1;

            index = _rest.IndexOf(value);
            if (index != -1) return first.Length + index;

            return -1;
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

        ReadOnlyBytes SliceRest(int index, int length)
        {
            if (_rest == null)
            {
                if (First.Length == index && length == 0)
                {
                    return new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index or length");
                }
            }

            // TODO (pri 2): this could be optimized
            var rest = new ReadOnlyBytes(_rest, length);
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
                ReadOnlyMemory<byte> segment;
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
    }
}