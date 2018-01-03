// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public class BufferReader
    {
        public static BufferReader<TSequence> Create<TSequence>(TSequence buffer) where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            return new BufferReader<TSequence>(buffer);
        }
    }

    public ref struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        private ReadOnlySpan<byte> _currentSpan;
        private int _index;

        private TSequence _sequence;
        private Position _currentPosition;
        private Position _nextPosition;
               
        private int _consumedBytes;
        private bool _end;

        public BufferReader(TSequence buffer)
        {
            _end = false;
            _index = 0;
            _consumedBytes = 0;
            _sequence = buffer;
            _currentPosition = _sequence.Start;
            _nextPosition = _currentPosition;
            _currentSpan = default;
            MoveNext();
        }

        public bool End => _end;

        public int Index => _index;

        public Position Position => _currentPosition + _index;

        public ReadOnlySpan<byte> CurrentSegment => _currentSpan;

        public ReadOnlySpan<byte> UnreadSegment => _currentSpan.Slice(_index);

        public int ConsumedBytes => _consumedBytes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (_end)
            {
                return -1;
            }
            return _currentSpan[_index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Take()
        {
            if (_end)
            {
                return -1;
            }

            var value = _currentSpan[_index];

            _index++;
            _consumedBytes++;

            if (_index >= _currentSpan.Length)
            {
                MoveNext();
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void MoveNext()
        {
            var previous = _nextPosition;
            while (_sequence.TryGet(ref _nextPosition, out var memory, true))
            {
                _currentPosition = previous;
                _currentSpan = memory.Span;
                _index = 0;
                if (_currentSpan.Length > 0)
                {
                    return;
                }
            }
            _end = true;
        }

        public void Skip(int byteCount)
        {
            if (byteCount < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            _consumedBytes += byteCount;

            while (!_end && byteCount > 0)
            {
                if ((_index + byteCount) < _currentSpan.Length)
                {
                    _index += byteCount;
                    byteCount = 0;
                    break;
                }

                var remaining = (_currentSpan.Length - _index);

                _index += remaining;
                byteCount -= remaining;

                MoveNext();
            }

            if (byteCount > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        public void SkipTo(Position position)
        {
            _currentPosition = position;
            _nextPosition = position;
            if (_sequence.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory))
            {
                _currentSpan = memory.Span;
            }
            else
            {
                _currentSpan = default;
            }
            _index = 0;
        }
      
        public Position? PositionOf(byte value)
        {
            var unread = UnreadSegment;
            var index = unread.IndexOf(value);
            if (index != -1) return _currentPosition + (_index + index);

            var nextSegmentPosition = _nextPosition;
            var currentSegmentPosition = _nextPosition;
            while (_sequence.TryGet(ref nextSegmentPosition, out var memory, true))
            {
                var segment = memory.Span;
                index = segment.IndexOf(value);
                if (index != -1) return currentSegmentPosition + index;
                currentSegmentPosition = nextSegmentPosition;
            }
            return default;
        }

        public Position? PositionOf(ReadOnlySpan<byte> value)
        {
            var unread = UnreadSegment;
            var index = unread.IndexOf(value);
            if (index != -1) return _currentPosition + (_index + index);
            if (value.Length == 0) return default;

            Span<byte> temp = stackalloc byte[(value.Length - 1) * 2];
            var currentSegmentPosition = _currentPosition;
            var nextSegmentPosition = _nextPosition;
            var currentSegmentConsumedBytes = _index;

            while (true)
            {
                // Try Stitched Match
                int tempStartIndex = unread.Length - Math.Min(unread.Length, value.Length - 1);
                var candidatePosition = currentSegmentPosition + (currentSegmentConsumedBytes + tempStartIndex);
                int copied = CopyFromPosition(_sequence, candidatePosition, temp);
                if (copied < value.Length) return null;

                index = temp.Slice(0, copied).IndexOf(value);
                if (index < value.Length)
                {
                    if (index != -1) return candidatePosition + index;
                }

                currentSegmentPosition = nextSegmentPosition;
                // Try Next Segment
                if (!_sequence.TryGet(ref nextSegmentPosition, out var memory, true))
                {
                    return default;
                }
                currentSegmentConsumedBytes = 0;
                unread = memory.Span;

                index = unread.IndexOf(value);
                if (index != -1) return currentSegmentPosition + index;
            }
        }

        public static int CopyTo(BufferReader<TSequence> bytes, Span<byte> buffer)
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
                return first.Length + CopyFromPosition(bytes._sequence, bytes._nextPosition, buffer.Slice(first.Length));
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
    }
}
