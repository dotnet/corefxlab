// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public struct ReadOnlyBytes : IReadOnlyBufferList<byte>
    {
        ReadOnlyMemory<byte> _first;
        int _totalLength;
        IReadOnlyBufferList<byte> _rest;
        
        static readonly ReadOnlyBytes s_empty = new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyBufferList<byte> rest, int length)
        {
            Debug.Assert(rest != null || length <= first.Length);
            _rest = rest;
            _first = first;
            _totalLength = length;
        }

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyBufferList<byte> rest) :
            this(first, rest, rest==null?first.Length:Unspecified)
        {
        }

        public ReadOnlyBytes(ReadOnlyMemory<byte> buffer) :
            this(buffer, null, buffer.Length)
        { }

        public ReadOnlyBytes(IReadOnlyBufferList<byte> segments, int length) :
            this(segments.First, segments.Rest, length)
        { }

        public ReadOnlyBytes(IReadOnlyBufferList<byte> segments) :
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

            var rest = position.ObjectPosition as IReadOnlyBufferList<byte>;
            if (rest == null)
            {
                value = default;
                return false;
            }

            value = rest.First;
            // we need to slice off the last segment based on length of this. ReadOnlyBytes is a potentially shorted view over a longer buffer list.
            if (value.Length + position.Tag > _totalLength && _totalLength != Unspecified)
            {
                value = value.Slice(0, _totalLength - position.Tag);
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

        public ReadOnlyMemory<byte> First => _first;

        public IReadOnlyBufferList<byte> Rest => _rest;

        public int? Length
        {
            get {
                if (_totalLength == Unspecified)
                {
                    return null;
                }
                return _totalLength;
            }
        }

        public bool IsEmpty => _first.Length == 0 && _rest == null;

        public static ReadOnlyBytes Empty => s_empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(int index, int length)
        {
            if(length==0) return ReadOnlyBytes.Empty;

            var first = First;
            if (first.Length >= length + index)
            {
                var slice = first.Slice(index, length);
                if(slice.Length > 0) {
                    return new ReadOnlyBytes(first.Slice(index, length));
                }
                return ReadOnlyBytes.Empty;
            }
            if (first.Length > index)
            {
                Debug.Assert(_rest != null);
                return new ReadOnlyBytes(first.Slice(index), _rest, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(int index)
        {
            if (_totalLength != Unspecified) {
                return Slice(index, _totalLength - index);
            }
            else {
                var first = First;
                if (first.Length > index) {
                    var slice = first.Slice(index);
                    if(slice.Length > 0 || _rest!=null) {
                        return new ReadOnlyBytes(slice, _rest);
                    }
                    return ReadOnlyBytes.Empty;
                }

                var toSlice = index - first.Length;
                if (_rest == null && toSlice == 0) return Empty;
                var rest = new ReadOnlyBytes(_rest);
                if(toSlice > 0) {
                    rest = rest.Slice(toSlice);
                }
                return rest;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(ReadOnlySpan<byte> bytes)
        {
            var first = _first.Span;
            var index = first.IndexOf(bytes);
            if (index != -1) return index;
            return first.IndexOfStraddling(_rest, bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(byte value)
        {
            var first = _first.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;
            return IndexOfRest(value, first.Length);
        }

        int IndexOfRest(byte value, int firstLength)
        {
            if (_rest == null) return -1;
            var index = _rest.IndexOf(value);
            if (index != -1) return firstLength + index;
            return -1;
        }

        public int CopyTo(Span<byte> buffer)
        {
            var first = First;
            var firstLength = first.Length;
            if (firstLength > buffer.Length)
            {
                first.Slice(0, buffer.Length).Span.CopyTo(buffer);
                return buffer.Length;
            }
            first.Span.CopyTo(buffer);
            if (buffer.Length == firstLength || _rest == null) return firstLength;
            return firstLength + _rest.CopyTo(buffer.Slice(firstLength));
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
            if (_totalLength != Unspecified) return _totalLength;

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
            _totalLength = length + _first.Length;
            return _totalLength;
        }

        // this is used for unspecified _length; ReadOnlyBytes can be created from list of buffers of unknown total size, 
        // and knowing the length is not needed in amy operations, e.g. slicing small section of the front.
        const int Unspecified = -1;

        class BufferListNode : IReadOnlyBufferList<byte>
        {
            internal ReadOnlyMemory<byte> _first;
            internal BufferListNode _rest;
            public ReadOnlyMemory<byte> First => _first;

            public int? Length {
                get {
                    throw new NotImplementedException();
                }
            }

            public IReadOnlyBufferList<byte> Rest => _rest;

            public int CopyTo(Span<byte> buffer)
            {
                int copied = 0;
                var position = Position.First;
                ReadOnlyMemory<byte> segment;
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
                        segment.Span.CopyTo(free);
                        copied += segment.Length;
                    }
                    free = buffer.Slice(copied);
                    if (free.Length == 0) break;
                }
                return copied;
            }

            public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
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
        }

        public static ReadOnlyBytes Create(params byte[][] buffers)
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

            if (first.Rest == null)
            {
                return new ReadOnlyBytes(first, first.First.Length);
            }
            else
            {
                return new ReadOnlyBytes(first);
            }
        }
    }
}
