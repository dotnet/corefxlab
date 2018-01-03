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

    public ref partial struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        private TSequence _sequence;
        private Position _currentSegmentPosition;
        private Position _nextSegmentPosition;

        private ReadOnlySpan<byte> _currentSegment;
        private int _currentSegmentConsumedBytes;
        
        // TODO: this should be long, but it will impact the HTTP parser. We should meassure perf impact in isolation
        private int _totalConsumedBytes;
        private bool _hasCompleted;

        public BufferReader(TSequence buffer)
        {
            _hasCompleted = false;
            _currentSegmentConsumedBytes = 0;
            _totalConsumedBytes = 0;
            _sequence = buffer;
            _currentSegmentPosition = _sequence.Start;
            _nextSegmentPosition = _currentSegmentPosition;
            _currentSegment = default;
            MoveNext();
        }

        public bool End => _hasCompleted;

        public int CurrentSegmentIndex => _currentSegmentConsumedBytes;

        public Position CurrentPosition => _currentSegmentPosition + _currentSegmentConsumedBytes;

        public ReadOnlySpan<byte> CurrentSegment => _currentSegment;

        public ReadOnlySpan<byte> UnreadSegment => _currentSegment.Slice(_currentSegmentConsumedBytes);

        public int ConsumedBytes => _totalConsumedBytes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (_hasCompleted)
            {
                return -1;
            }
            return _currentSegment[_currentSegmentConsumedBytes];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Take()
        {
            if (_hasCompleted)
            {
                return -1;
            }

            var value = _currentSegment[_currentSegmentConsumedBytes];

            _currentSegmentConsumedBytes++;
            _totalConsumedBytes++;

            if (_currentSegmentConsumedBytes >= _currentSegment.Length)
            {
                MoveNext();
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void MoveNext()
        {
            var previous = _nextSegmentPosition;
            while (_sequence.TryGet(ref _nextSegmentPosition, out var memory, true))
            {
                _currentSegmentPosition = previous;
                _currentSegment = memory.Span;
                _currentSegmentConsumedBytes = 0;
                if (_currentSegment.Length > 0)
                {
                    return;
                }
            }
            _hasCompleted = true;
        }

        public void Skip(int byteCount)
        {
            if (byteCount < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            _totalConsumedBytes += byteCount;

            while (!_hasCompleted && byteCount > 0)
            {
                if ((_currentSegmentConsumedBytes + byteCount) < _currentSegment.Length)
                {
                    _currentSegmentConsumedBytes += byteCount;
                    byteCount = 0;
                    break;
                }

                var remaining = (_currentSegment.Length - _currentSegmentConsumedBytes);

                _currentSegmentConsumedBytes += remaining;
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
            _currentSegmentPosition = position;
            _nextSegmentPosition = position;
            if (_sequence.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                _currentSegment = memory.Span;
            }
            else
            {
                _currentSegment = default;
            }
            _currentSegmentConsumedBytes = 0;
        }
    }
}
