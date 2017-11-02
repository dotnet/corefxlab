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
        ReadOnlyMemory<byte> _first; // pointer + index:int + length:int
        IReadOnlyBufferList<byte> _rest;
        long _totalLength;

        static readonly ReadOnlyBytes s_empty = new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyBufferList<byte> rest, long length)
        {
            if(length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (first.Length + (rest == null ? 0 : rest.Length) < length)
                throw new ArgumentOutOfRangeException(nameof(length), length, string.Format("{0}+{1}", first.Length, rest.Length));
            _rest = rest;
            _first = first;
            _totalLength = length;
        }

        public ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyBufferList<byte> rest) :
            this(first, rest, first.Length + rest.Length)
        { }

        public ReadOnlyBytes(ReadOnlyMemory<byte> buffer) :
            this(buffer, null, buffer.Length)
        { }

        public ReadOnlyBytes(IReadOnlyBufferList<byte> segments) :
            this(segments.First, segments.Rest, segments.Length)
        { }

        public ReadOnlyBytes(IReadOnlyBufferList<byte> segments, long length) :
            this(segments.First, segments.Rest, length)
        {
            // TODO: should they be runtime/release checks?
            Debug.Assert(segments != null);
            Debug.Assert(segments.Length >= length);
        }

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
            if (value.Length + position.Tag > _totalLength)
            {
                // TODO (pri 0): this cannot cast to int. What we need to do is store the high order length bits in position.IntegerPosition
                value = value.Slice(0, (int)_totalLength - position.Tag);
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

        public long Length => _totalLength;

        public bool IsEmpty => _first.Length == 0 && _rest == null;

        public static ReadOnlyBytes Empty => s_empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(long index, long length)
        {
            if(length==0) return Empty;

            var first = First;
            if (first.Length >= length + index)
            {
                var slice = first.Slice((int)index, (int)length);
                if(slice.Length > 0) {
                    return new ReadOnlyBytes(slice);
                }
                return ReadOnlyBytes.Empty;
            }
            if (first.Length > index)
            {
                Debug.Assert(_rest != null);
                return new ReadOnlyBytes(first.Slice((int)index), _rest, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBytes Slice(long index)
        {
            return Slice(index, _totalLength - index);
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

        public Cursor CursorOf(byte value)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int index = first.IndexOf(value);
            if (index != -1) return new Cursor(this, index);
            if (_rest == null) return default;
            return CursorOf(_rest, value);
        }

        private static Cursor CursorOf(IReadOnlyBufferList<byte> list, byte value)
        {
            ReadOnlySpan<byte> first = list.First.Span;
            int index = first.IndexOf(value);
            if (index != -1) return new Cursor(list, index);
            if (list.Rest == null) return default;
            return CursorOf(list.Rest, value);
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

        ReadOnlyBytes SliceRest(long index, long length)
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

        class BufferListNode : IReadOnlyBufferList<byte>
        {
            internal ReadOnlyMemory<byte> _first;
            internal BufferListNode _rest;
            public ReadOnlyMemory<byte> First => _first;

            public long Length => _first.Length + (_rest==null?0:_rest.Length);

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

            if (first == null) return Empty;

            if (first.Rest == null)
            {
                return new ReadOnlyBytes(first, first.First.Length);
            }
            else
            {
                return new ReadOnlyBytes(first);
            }
        }

        public struct Cursor
        {
            IReadOnlyBufferList<byte> _node;
            int _index;

            public Cursor(IReadOnlyBufferList<byte> node, int index)
            {
                _node = node;
                _index = index;
            }
        }
    }
}
