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
    public readonly struct ReadOnlyBytes : IReadOnlyMemorySequence<byte>
    {
        readonly ReadOnlyMemory<byte> _first; // pointer + index:int + length:int
        readonly IReadOnlyMemoryList<byte> _all;
        readonly long _totalLength;

        static readonly ReadOnlyBytes s_empty = new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);

        public ReadOnlyBytes(ReadOnlyMemory<byte> buffer)
        {
            _first = buffer;
            _all = null;
            _totalLength = _first.Length;
        }

        public ReadOnlyBytes(IReadOnlyMemoryList<byte> segments, long length)
        {
            // TODO: should we skip all empty buffers, i.e. of _first.IsEmpty?
            _first = segments.First;
            _all = segments;
            _totalLength = length;
        }

        private ReadOnlyBytes(ReadOnlyMemory<byte> first, IReadOnlyMemoryList<byte> all, long length)
        {
            // TODO: add assert that first overlaps all (once we have Overlap on Span)
            _first = first;
            _all = all;
            _totalLength = length;
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
                    position.ObjectPosition = Rest;
                }
                return (!_first.IsEmpty || _all != null);
            }

            var rest = position.ObjectPosition as IReadOnlyMemoryList<byte>;
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

        internal IReadOnlyMemoryList<byte> Rest => _all?.Rest;

        public long Length => _totalLength;

        public long Index => _all == null ? 0 : _all.Index;

        public bool IsEmpty => _first.Length == 0 && _all == null;

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
                    return new ReadOnlyBytes(slice, _all, length);
                }
                return Empty;
            }
            if (first.Length > index)
            {
                Debug.Assert(_all != null);
                return new ReadOnlyBytes(first.Slice((int)index), _all, length);
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
            return first.IndexOfStraddling(Rest, bytes);
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
            var rest = Rest;
            if (rest == null) return -1;
            var index = rest.IndexOf(value);
            if (index != -1) return firstLength + index;
            return -1;
        }

        public ReadOnlyBytes Slice(Cursor from)
        {
            if (from._node == null) return Slice(First.Length - from._index);
            var headIndex = _all.Index + _all.First.Length - _first.Length;
            var newHeadIndex = from._node.Index;
            var diff = newHeadIndex - headIndex;
            // TODO: this could be optimized to avoid the Slice
            return new ReadOnlyBytes(from._node, Length - diff).Slice(from._index);
        }

        public ReadOnlyBytes Slice(Cursor from, Cursor to)
        {
            if (from._node == null)
            {
                var indexFrom = First.Length - from._index;
                var indexTo = First.Length - to._index;
                return Slice(indexFrom, indexTo - indexFrom + 1);
            }

            var headIndex = _all.Index + _all.First.Length - _first.Length;
            var newHeadIndex = from._node.Index + from._index;
            var newEndIndex = to._node.Index + to._index;
            var slicedOffFront = newHeadIndex - headIndex;
            var length = newEndIndex - newHeadIndex;
            // TODO: this could be optimized to avoid the Slice
            var a = new ReadOnlyBytes(from._node, length + from._index);
            a = a.Slice(from._index);
            return a;
        }

        public Cursor CursorOf(byte value)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int index = first.IndexOf(value);
            if (index != -1)
            {
                if (_all == null)
                {
                    return new Cursor(null, first.Length - index);
                }
                else { 
                    var allIndex = index + (_all.First.Length - first.Length);
                    return new Cursor(_all, allIndex);
                }
            }
            if (Rest == null) return default;
            return CursorOf(Rest, value);
        }

        private static Cursor CursorOf(IReadOnlyMemoryList<byte> list, byte value)
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
            if (buffer.Length == firstLength || Rest == null) return firstLength;
            return firstLength + Rest.CopyTo(buffer.Slice(firstLength));
        }

        ReadOnlyBytes SliceRest(long index, long length)
        {
            if (Rest == null)
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
            var rest = new ReadOnlyBytes(Rest, length);
            rest = rest.Slice(index - First.Length, length);
            return rest;
        }

        class BufferListNode : IReadOnlyMemoryList<byte>
        {
            internal ReadOnlyMemory<byte> _data;
            internal BufferListNode _next;
            private long _runningIndex;

            public BufferListNode(ReadOnlyMemory<byte> data)
            {
                _data = data;
                _next = null;
                _runningIndex = 0;
            }

            private BufferListNode(ReadOnlyMemory<byte> data, long runningIndex)
            {
                _data = data;
                _runningIndex = runningIndex;
            }

            public BufferListNode Append(ReadOnlyMemory<byte> data)
            {
                if (_next != null) throw new InvalidOperationException("Node cannot be appended");
                var node = new BufferListNode(data, _runningIndex + Length);
                _next = node;
                return node;
            }

            public ReadOnlyMemory<byte> First => _data;
            public IReadOnlyMemoryList<byte> Rest => _next;

            public long Length => _data.Length;

            public long Index => _runningIndex;
            
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
                    item = _data;
                    if (advance) {
                        position.IntegerPosition++;
                        position.ObjectPosition = _next;
                    }
                    return (!_data.IsEmpty || _next != null);
                }
                else if (position.ObjectPosition == null)
                {
                    item = default;
                    return false;
                }

                var sequence = (BufferListNode)position.ObjectPosition;
                item = sequence._data;
                if (advance)
                {
                    position.ObjectPosition = sequence._next;
                    position.IntegerPosition++;
                }
                return true;
            }
        }

        public static ReadOnlyBytes Create(params byte[][] buffers)
        {
            if (buffers.Length == 0) return Empty;
            if (buffers.Length == 1) return new ReadOnlyBytes(buffers[0]);

            BufferListNode first = new BufferListNode(buffers[0]);
            var last = first;
            for(int i=1; i<buffers.Length; i++)
            {
                last = last.Append(buffers[i]);
            }

            return new ReadOnlyBytes(first, last.Index + last.Length);
        }

        public readonly struct Cursor
        {
            internal readonly IReadOnlyMemoryList<byte> _node;
            internal readonly int _index;

            public Cursor(IReadOnlyMemoryList<byte> node, int index)
            {
                _node = node;
                _index = index;
            }
        }
    }
}


