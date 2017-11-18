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
        readonly IMemoryList<byte> _all;

        // For multi-segment ROB, this is the total length of the ROB
        // For single-segment ROB that are slices, this is the offset of the first byte from the original ROB
        // Otherwise zero
        readonly long _totalLengthOrVirtualIndex;

        static readonly ReadWriteBytes s_empty = new ReadWriteBytes(Memory<byte>.Empty);

        public ReadWriteBytes(Memory<byte> memory) : this(memory, 0)
        {
        }

        private ReadWriteBytes(Memory<byte> memory, int virtualIndex)
        {
            _first = memory;
            _all = null;
            _totalLengthOrVirtualIndex = virtualIndex;
        }

        public ReadWriteBytes(IMemoryList<byte> segments, long length)
        {
            // TODO: should we skip all empty buffers, i.e. of _first.IsEmpty?
            _first = segments.Memory;
            _all = segments;
            _totalLengthOrVirtualIndex = length;
        }

        private ReadWriteBytes(Memory<byte> first, IMemoryList<byte> all, long length)
        {
            // TODO: add assert that first overlaps all (once we have Overlap on Span)
            _first = first;
            _all = all;
            _totalLengthOrVirtualIndex = _all == null ? 0 : length;
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

            var (segment, index) = position.Get<IMemoryList<byte>>();

            if (segment == null)
            {
                if (_all == null) // single segment ROB
                {
                    value = _first.Slice(index - (int)_totalLengthOrVirtualIndex);
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
            var lengthOfFirst = virtualIndex - VirtualIndex;
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

        internal IMemoryList<byte> Rest => _all?.Rest;

        public long Length => _all == null ? _first.Length : _totalLengthOrVirtualIndex;

        private long VirtualIndex => _all == null ? _totalLengthOrVirtualIndex : _all.VirtualIndex + (_all.Memory.Length - _first.Length);

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
            var totalLength = Length;
            if (index > totalLength) throw new ArgumentOutOfRangeException(nameof(index));
            if (totalLength - index < length) throw new ArgumentOutOfRangeException(nameof(length));

            if (length == 0) return Empty;

            if (_all == null)
            {
                if (index > _first.Length) throw new ArgumentOutOfRangeException(nameof(index));
                if (length > _first.Length - index) throw new ArgumentOutOfRangeException(nameof(length));

                return new ReadWriteBytes(_first.Slice((int)index, (int)length), (int)(_totalLengthOrVirtualIndex + index));
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

        public ReadWriteBytes Slice(Position from)
        {
            var (segment, index) = from.Get<IMemoryList<byte>>();
            if (segment == null) return Slice(First.Length - index);
            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = segment.VirtualIndex;
            var diff = newHeadIndex - headIndex;
            // TODO: this could be optimized to avoid the Slice
            return new ReadWriteBytes(segment, Length - diff).Slice(index);
        }

        public ReadWriteBytes Slice(Position from, Position to)
        {
            var (fromSegment, fromIndex) = from.Get<IMemoryList<byte>>();
            var (toSegment, toIndex) = to.Get<IMemoryList<byte>>();

            if (fromSegment == null)
            {
                var indexFrom = First.Length - fromIndex;
                var indexTo = First.Length - toIndex;
                return Slice(indexFrom, indexTo - indexFrom + 1);
            }

            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = fromSegment.VirtualIndex + fromIndex;
            var newEndIndex = toSegment.VirtualIndex + toIndex;
            var slicedOffFront = newHeadIndex - headIndex;
            var length = newEndIndex - newHeadIndex;
            // TODO: this could be optimized to avoid the Slice
            var slice = new ReadWriteBytes(fromSegment, length + fromIndex);
            slice = slice.Slice(fromIndex);
            return slice;
        }

        public Position PositionOf(byte value)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int index = first.IndexOf(value);
            if (index != -1)
            {
                if (_all == null)
                {
                    return Position.Create(first.Length - index);
                }
                else
                {
                    var allIndex = index + (_all.Memory.Length - first.Length);
                    return Position.Create(allIndex, _all);
                }
            }
            if (Rest == null) return Position.End;
            return PositionOf(Rest, value);
        }

        public Position PositionAt(int index)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int firstLength = first.Length;

            if (index < firstLength)
            {
                if (_all == null)
                {
                    return Position.Create(firstLength - index);
                }
                else
                {
                    var allIndex = index + (_all.Memory.Length - firstLength);
                    return Position.Create(allIndex, _all);
                }
            }
            if (Rest == null) return default;
            return PositionAt(Rest, index - firstLength);
        }

        private static Position PositionOf(IMemoryList<byte> list, byte value)
        {
            ReadOnlySpan<byte> first = list.Memory.Span;
            int index = first.IndexOf(value);
            if (index != -1) return Position.Create(index, list);
            if (list.Rest == null) return Position.End;
            return PositionOf(list.Rest, value);
        }

        private static Position PositionAt(IMemoryList<byte> list, int index)
        {
            if (list == null) return Position.End;
            ReadOnlySpan<byte> first = list.Memory.Span;
            int firstLength = first.Length;

            if (index < firstLength)
            {
                return Position.Create(index, list);
            }
            return PositionAt(list.Rest, index - firstLength);
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
    }
}


