// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public ref partial struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        public bool TryReadUntill(out ReadOnlyBuffer bytes, byte delimiter)
        {
            var range = new PositionRange(CurrentPosition, AdvanceToDelimiter(delimiter).GetValueOrDefault());
            if (range.Start == default || range.End == default)
            {
                bytes = default;
                return false;
            }

            bytes = new ReadOnlyBuffer(range.Start, range.End);
            return true;
        }

        public bool TryReadUntill(out ReadOnlyBuffer bytes, ReadOnlySpan<byte> delimiter)
        {
            var range = new PositionRange(CurrentPosition, PositionOf(delimiter).GetValueOrDefault());
            if (range.Start == default || range.End == default)
            {
                bytes = default;
                return false;
            }
            if (range.End != default)
            {
                SkipTo(range.End);
                Skip(delimiter.Length);
            }

            bytes = new ReadOnlyBuffer(range.Start, range.End);
            return true;
        }

        #region Helpers
        private static int CopyUnread(BufferReader<TSequence> bytes, Span<byte> buffer)
        {
            var first = bytes.UnreadSegment;
            if (first.Length > buffer.Length)
            {
                first.Slice(0, buffer.Length).CopyTo(buffer);
                return buffer.Length;
            }
            else if (first.Length == buffer.Length)
            {
                first.CopyTo(buffer);
                return buffer.Length;
            }
            else
            {
                first.CopyTo(buffer);
                return first.Length + CopyFromPosition(bytes._sequence, bytes._nextSegmentPosition, buffer.Slice(first.Length));
            }
        }

        private static int CopyFromPosition(TSequence sequence, Position from, Span<byte> buffer)
        {
            int copied = 0;
            while (sequence.TryGet(ref from, out ReadOnlyMemory<byte> memory, true))
            {
                var span = memory.Span;
                var toCopy = Math.Min(span.Length, buffer.Length - copied);
                span.Slice(0, toCopy).CopyTo(buffer.Slice(copied));
                copied += toCopy;
                if (copied >= buffer.Length) break;
            }
            return copied;
        }

        private Position? AdvanceToDelimiter(byte value)
        {
            var unread = UnreadSegment;
            var index = unread.IndexOf(value);
            if (index != -1)
            {
                _currentSegmentConsumedBytes += index;
                var result = _currentSegmentPosition + _currentSegmentConsumedBytes;
                _currentSegmentConsumedBytes++; // skip delimiter
                return result;
            }

            var nextPosition = _nextSegmentPosition;
            var currentPosition = _currentSegmentPosition;
            var previousPosition = _nextSegmentPosition;
            while (_sequence.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                var span = memory.Span;
                index = span.IndexOf(value);
                if (index != -1)
                {
                    _currentSegmentPosition = previousPosition;
                    _currentSegment = span;
                    _currentSegmentConsumedBytes = index + 1;
                    return _currentSegmentPosition + index;
                }
                previousPosition = _nextSegmentPosition;
            }

            _nextSegmentPosition = nextPosition;
            _currentSegmentPosition = currentPosition;
            return null;
        }

        private Position? PositionOf(ReadOnlySpan<byte> value)
        {
            var unread = UnreadSegment;
            var index = unread.IndexOf(value);
            if (index != -1) return _currentSegmentPosition + (_currentSegmentConsumedBytes + index);
            if (value.Length == 0) return default;

            Span<byte> temp = stackalloc byte[(value.Length - 1) * 2];
            var currentSegmentPosition = _currentSegmentPosition;
            var nextSegmentPosition = _nextSegmentPosition;
            var currentSegmentConsumedBytes = _currentSegmentConsumedBytes;

            while (true) {
                // Try Stitched Match
                int tempStartIndex = unread.Length - Math.Min(unread.Length, value.Length - 1);
                var candidatePosition = currentSegmentPosition + (currentSegmentConsumedBytes + tempStartIndex);
                int copied = CopyFromPosition(_sequence, candidatePosition, temp);
                if (copied < value.Length) return null;
                index = temp.Slice(0, copied).IndexOf(value);
                if (index < value.Length && index != -1) return candidatePosition + index;

                // Try Next Segment
                currentSegmentPosition = nextSegmentPosition;
                if (!_sequence.TryGet(ref nextSegmentPosition, out var memory, true)) return default;
                currentSegmentConsumedBytes = 0;
                unread = memory.Span;
                index = unread.IndexOf(value);
                if (index != -1) return currentSegmentPosition + index;
            }
        }
        #endregion
    }

    struct PositionRange
    {
        public readonly Position Start;
        public readonly Position End;

        public PositionRange(Position start, Position end)
        {
            Start = start;
            End = end;
        }
    }
}
