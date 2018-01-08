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

        public Position Position => _sequence.Seek(_currentPosition, _index);

        public ReadOnlySpan<byte> Span => _currentSpan;

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
            while(_sequence.TryGet(ref _nextPosition, out var memory, true))
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

        public void Skip(int length)
        {
            if (length < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            _consumedBytes += length;

            while (!_end && length > 0)
            {
                if ((_index + length) < _currentSpan.Length)
                {
                    _index += length;
                    length = 0;
                    break;
                }

                var remaining = (_currentSpan.Length - _index);

                _index += remaining;
                length -= remaining;

                MoveNext();
            }

            if (length > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }
    }
}
