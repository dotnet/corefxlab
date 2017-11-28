﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public struct PositionRange
    {
        public Position From;
        public Position To;
    }

    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public readonly struct ReadOnlyBytes : ISequence<ReadOnlyMemory<byte>>
    {
        readonly ReadOnlyMemory<byte> _first; // pointer + index:int + length:int
        readonly IMemoryList<byte> _all;

        // For multi-segment ROB, this is the total length of the ROB
        // For single-segment ROB that are slices, this is the offset of the first byte from the original ROB
        // Otherwise zero
        readonly long _totalLengthOrVirtualIndex;

        static readonly ReadOnlyBytes s_empty = new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);

        public ReadOnlyBytes(ReadOnlyMemory<byte> memory) : this(memory, 0)
        {
        }

        private ReadOnlyBytes(ReadOnlyMemory<byte> memory, int virtualIndex)
        {
            _first = memory;
            _all = null;
            _totalLengthOrVirtualIndex = virtualIndex;
        }

        public ReadOnlyBytes(IMemoryList<byte> segments, long length)
        {
            // TODO: should we skip all empty buffers, i.e. of _first.IsEmpty?
            _first = segments.Memory;
            _all = segments;
            _totalLengthOrVirtualIndex = length;
        }

        private ReadOnlyBytes(ReadOnlyMemory<byte> first, IMemoryList<byte> all, long length)
        {
            // TODO: add assert that first overlaps all (once we have Overlap on Span)
            _first = first;
            _all = all;
            _totalLengthOrVirtualIndex = _all == null ? 0 : length;
        }

        public Position First
        {
            get {
                if (_all == null) return Position.Create((int)_totalLengthOrVirtualIndex);
                return Position.Create(_all.Memory.Length - _first.Length, _all);
            }
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> value, bool advance = true)
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
                    if(index > 0) value = value.Slice(index);
                    return true;
                }
                else
                {
                    value = ReadOnlyMemory<byte>.Empty;
                    return false;
                }
            }
            else
            {
                value = memory.Slice(index);
                if (advance) position = Position.Create(0, segment.Rest);
                return true;
            }
        }

        public ReadOnlyMemory<byte> Memory => _first;

        internal IMemoryList<byte> Rest => _all?.Rest;

        public long Length => _all == null ? _first.Length : _totalLengthOrVirtualIndex;

        private long VirtualIndex => _all == null ? _totalLengthOrVirtualIndex : _all.VirtualIndex + (_all.Memory.Length - _first.Length);

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
            var totalLength = Length;
            if (index > totalLength) throw new ArgumentOutOfRangeException(nameof(index));
            if (totalLength - index < length) throw new ArgumentOutOfRangeException(nameof(length));

            if (length==0) return Empty;

            if(_all == null)
            {
                if (index > _first.Length) throw new ArgumentOutOfRangeException(nameof(index));
                if (length > _first.Length - index) throw new ArgumentOutOfRangeException(nameof(length));

                return new ReadOnlyBytes(_first.Slice((int)index, (int)length), (int)(_totalLengthOrVirtualIndex + index));
            }

            var first = Memory;
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
            return Slice(index, Length - index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long IndexOf(ReadOnlySpan<byte> bytes)
        {
            var first = _first.Span;
            var index = first.IndexOf(bytes);
            if (index != -1) return index;
            return first.IndexOfStraddling(Rest, bytes);
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
            long index = Sequence.IndexOf(rest, value); 
            if (index != -1) return firstLength + index;
            return -1;
        }

        public ReadOnlyBytes Slice(Position from)
        {
            var (segment, index) = from.Get<IMemoryList<byte>>();
            if (segment == null) return Slice(index - _totalLengthOrVirtualIndex);
            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = segment.VirtualIndex;
            var diff = newHeadIndex - headIndex;
            // TODO: this could be optimized to avoid the Slice
            return new ReadOnlyBytes(segment, Length - diff).Slice(index);
        }

        public ReadOnlyBytes Slice(Position from, Position to)
        {
            var (fromSegment, fromIndex) = from.Get<IMemoryList<byte>>();
            var (toSegment, toIndex) = to.Get<IMemoryList<byte>>();

            if (fromSegment == null)
            {
                return Slice(fromIndex - _totalLengthOrVirtualIndex, toIndex - fromIndex);
            }

            var headIndex = _all.VirtualIndex + _all.Memory.Length - _first.Length;
            var newHeadIndex = fromSegment.VirtualIndex + fromIndex;
            var newEndIndex = toSegment.VirtualIndex + toIndex;
            var slicedOffFront = newHeadIndex - headIndex;
            var length = newEndIndex - newHeadIndex;
            // TODO: this could be optimized to avoid the Slice
            var slice = new ReadOnlyBytes(fromSegment, length + fromIndex);
            slice = slice.Slice(fromIndex);
            return slice;
        }

        public ReadOnlyBytes Slice(PositionRange range)
        {
            if (range.To.IsEnd || range.From.IsEnd) return Empty;
            return Slice(range.From, range.To);
        }

        public Position PositionOf(byte value)
        {
            ReadOnlySpan<byte> first = _first.Span;
            int index = first.IndexOf(value);
            if (index != -1)
            {
                if (_all == null)
                {
                    return Position.Create(index);
                }
                else { 
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
                    return Position.Create(index);
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
            var first = Memory;
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
                if (Memory.Length == index && length == 0)
                {
                    return new ReadOnlyBytes(ReadOnlyMemory<byte>.Empty);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index or length");
                }
            }

            // TODO (pri 2): this could be optimized
            var rest = new ReadOnlyBytes(Rest, length + index - _first.Length);
            rest = rest.Slice(index - Memory.Length, length);
            return rest;
        }

        public SequenceEnumerator<ReadOnlyMemory<byte>, ReadOnlyBytes> GetEnumerator()
            => new SequenceEnumerator<ReadOnlyMemory<byte>, ReadOnlyBytes>(this);
    }
}


