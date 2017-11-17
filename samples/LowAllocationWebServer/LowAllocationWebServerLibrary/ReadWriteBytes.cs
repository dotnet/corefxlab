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
    public readonly struct ReadWriteBytes : ISequence<Memory<byte>>
    {
        readonly Memory<byte> _first; // pointer + index:int + length:int
        readonly IMemorySegment<byte> _all;

        // For multi-segment ROB, this is the total length of the ROB
        // For single-segment ROB that are slices, this is the offset of the first byte from the original ROB
        // Otherwise zero
        readonly long _totalLengthOrOffset;

        static readonly ReadWriteBytes s_empty = new ReadWriteBytes(Memory<byte>.Empty);

        public ReadWriteBytes(Memory<byte> buffer) : this(buffer, 0)
        {
        }

        private ReadWriteBytes(Memory<byte> buffer, int offset)
        {
            _first = buffer;
            _all = null;
            _totalLengthOrOffset = offset;
        }

        public ReadWriteBytes(IMemorySegment<byte> segments, long length)
        {
            // TODO: should we skip all empty buffers, i.e. of _first.IsEmpty?
            _first = segments.Memory;
            _all = segments;
            _totalLengthOrOffset = length;
        }

        private ReadWriteBytes(Memory<byte> first, IMemorySegment<byte> all, long length)
        {
            // TODO: add assert that first overlaps all (once we have Overlap on Span)
            _first = first;
            _all = all;
            _totalLengthOrOffset = _all == null ? 0 : length;
        }

        public bool TryGet(ref Position position, out Memory<byte> value, bool advance = true)
        {
            if (position == default)
            {
                value = _first;
                if (advance) position.SetItem(Rest);
                return (!_first.IsEmpty || _all != null);
            }
            if (position.IsEnd)
            {
                value = default;
                return false;
            }

            var (segment, index) = position.Get<IMemorySegment<byte>>();

            if (segment == null)
            {
                if (_all == null) // single segment ROB
                {
                    value = _first.Slice(index - (int)_totalLengthOrOffset);
                    if (advance) position = Position.End;
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }

            var memory = segment.Memory;

            // We need to slice off the last segment based on length of this ROB. 
            // This ROB is a potentially shorted view over a longer segment list.
            var virtualIndex = segment.VirtualIndex;
            var lengthOfFirst = virtualIndex - Index;
            var lengthOfEnd = lengthOfFirst + memory.Length;
            if (lengthOfEnd > Length)
            {
                if (advance) position = Position.End;
                value = memory.Slice(0, (int)(Length - lengthOfFirst));
                if (index < value.Length)
                {
                    if (index > 0) value = value.Slice(index);
                    return true;
                }
                else
                {
                    value = Memory<byte>.Empty;
                    return false;
                }
            }
            else
            {
                value = memory.Slice(index);
                if (advance) position.SetItem(segment.Rest);
                return true;
            }
        }

        public Memory<byte> First => _first;

        internal IMemorySegment<byte> Rest => _all?.Rest;

        public long Length => _all == null ? _first.Length : _totalLengthOrOffset;

        public long Index => _all == null ? _totalLengthOrOffset : _all.VirtualIndex + (_all.Memory.Length - _first.Length);

        public bool IsEmpty => _first.Length == 0 && _all == null;

        public static ReadWriteBytes Empty => s_empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(Range range)
        {
            return Slice(range.Index, range.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(long index, long length)
        {
            if (length == 0) return Empty;

            if (_all == null)
            {
                if (index > _first.Length) throw new ArgumentOutOfRangeException(nameof(index));
                if (length > _first.Length - index) throw new ArgumentOutOfRangeException(nameof(length));

                return new ReadWriteBytes(_first.Slice((int)index, (int)length), (int)(_totalLengthOrOffset + index));
            }

            var first = First;
            if (first.Length >= length + index)
            {
                var slice = first.Slice((int)index, (int)length);
                if (slice.Length > 0)
                {
                    return new ReadWriteBytes(slice, _all, length);
                }
                return Empty;
            }
            if (first.Length > index)
            {
                Debug.Assert(_all != null);
                return new ReadWriteBytes(first.Slice((int)index), _all, length);
            }
            return SliceRest(index, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadWriteBytes Slice(long index)
        {
            return Slice(index, Length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long IndexOf(byte value)
        {
            var first = _first.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;
            return IndexOfRest(value, first.Length);
        }

        long IndexOfRest(byte value, int firstLength)
        {
            var rest = Rest;
            if (rest == null) return -1;
            long index = rest.IndexOf(value);
            if (index != -1) return firstLength + index;
            return -1;
        }

        public ReadWriteBytes Slice(Cursor from)
        {
            if (from._node == null) return Slice(First.Length - from._index);
            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = from._node.VirtualIndex;
            var diff = newHeadIndex - headIndex;
            // TODO: this could be optimized to avoid the Slice
            return new ReadWriteBytes(from._node, Length - diff).Slice(from._index);
        }

        public ReadWriteBytes Slice(Cursor from, Cursor to)
        {
            if (from._node == null)
            {
                var indexFrom = First.Length - from._index;
                var indexTo = First.Length - to._index;
                return Slice(indexFrom, indexTo - indexFrom + 1);
            }

            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = from._node.VirtualIndex + from._index;
            var newEndIndex = to._node.VirtualIndex + to._index;
            var slicedOffFront = newHeadIndex - headIndex;
            var length = newEndIndex - newHeadIndex;
            // TODO: this could be optimized to avoid the Slice
            var a = new ReadWriteBytes(from._node, length + from._index);
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
                else
                {
                    var allIndex = index + (_all.Memory.Length - first.Length);
                    return new Cursor(_all, allIndex);
                }
            }
            if (Rest == null) return default;
            return CursorOf(Rest, value);
        }

        public Cursor CursorAt(int index)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int firstLength = first.Length;

            if (index < firstLength)
            {
                if (_all == null)
                {
                    return new Cursor(null, firstLength - index);
                }
                else
                {
                    var allIndex = index + (_all.Memory.Length - firstLength);
                    return new Cursor(_all, allIndex);
                }
            }
            if (Rest == null) return default;
            return CursorAt(Rest, index - firstLength);
        }

        private static Cursor CursorOf(IMemorySegment<byte> list, byte value)
        {
            ReadOnlySpan<byte> first = list.Memory.Span;
            int index = first.IndexOf(value);
            if (index != -1) return new Cursor(list, index);
            if (list.Rest == null) return default;
            return CursorOf(list.Rest, value);
        }

        private static Cursor CursorAt(IMemorySegment<byte> list, int index)
        {
            if (list == null) return default;
            ReadOnlySpan<byte> first = list.Memory.Span;
            int firstLength = first.Length;

            if (index < firstLength)
            {
                return new Cursor(list, index);
            }
            return CursorAt(list.Rest, index - firstLength);
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

        ReadWriteBytes SliceRest(long index, long length)
        {
            if (Rest == null)
            {
                if (First.Length == index && length == 0)
                {
                    return Empty;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index or length");
                }
            }

            // TODO (pri 2): this could be optimized
            var rest = new ReadWriteBytes(Rest, length + index - _first.Length);
            rest = rest.Slice(index - First.Length, length);
            return rest;
        }

        public SequenceEnumerator<Memory<byte>, ReadWriteBytes> GetEnumerator()
            => new SequenceEnumerator<Memory<byte>, ReadWriteBytes>(this);

        class BufferListNode : IMemorySegment<byte>
        {
            internal Memory<byte> _data;
            internal BufferListNode _next;
            private long _runningIndex;

            public BufferListNode(Memory<byte> data)
            {
                _data = data;
                _next = null;
                _runningIndex = 0;
            }

            private BufferListNode(Memory<byte> data, long runningIndex)
            {
                _data = data;
                _runningIndex = runningIndex;
            }

            public BufferListNode Append(Memory<byte> data)
            {
                if (_next != null) throw new InvalidOperationException("Node cannot be appended");
                var node = new BufferListNode(data, _runningIndex + Length);
                _next = node;
                return node;
            }

            public Memory<byte> Memory => _data;
            public IMemorySegment<byte> Rest => _next;

            public long Length => _data.Length;

            public long VirtualIndex => _runningIndex;

            public int CopyTo(Span<byte> buffer)
            {
                int copied = 0;
                Position position = default;
                var free = buffer;
                while (TryGet(ref position, out ReadOnlyMemory<byte> segment, true))
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
                var result = TryGet(ref position, out Memory<byte> memory, advance);
                item = memory;
                return result;
            }

            public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
            {
                if (position == default)
                {
                    item = _data;
                    if (advance)
                    {
                        position.SetItem(_next);
                    }
                    return (!_data.IsEmpty || _next != null);
                }
                else if (position.IsEnd)
                {
                    item = default;
                    return false;
                }

                var sequence = position.GetItem<BufferListNode>();
                item = sequence._data;
                if (advance) { position.SetItem(sequence._next); }
                return true;
            }
        }

        public static ReadWriteBytes Create(params byte[][] buffers)
        {
            if (buffers.Length == 0) return Empty;
            if (buffers.Length == 1) return new ReadWriteBytes(buffers[0]);

            BufferListNode first = new BufferListNode(buffers[0]);
            var last = first;
            for (int i = 1; i < buffers.Length; i++)
            {
                last = last.Append(buffers[i]);
            }

            return new ReadWriteBytes(first, last.VirtualIndex + last.Length);
        }

        public readonly struct Cursor
        {
            internal readonly IMemorySegment<byte> _node;
            internal readonly int _index;

            public Cursor(IMemorySegment<byte> node, int index)
            {
                _node = node;
                _index = index;
            }
        }
    }
}


