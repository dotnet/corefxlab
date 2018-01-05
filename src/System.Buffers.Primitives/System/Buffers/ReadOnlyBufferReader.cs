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
        private Position _currentPosition;
        private Position _nextPosition;
        private bool _end;
       
        public TSequence Sequence { get; }
        public long ConsumedBytes { get; private set; }

        public BufferReader(TSequence buffer)
        {
            _end = false;
            _index = 0;
            ConsumedBytes = 0;
            Sequence = buffer;
            _currentPosition = Sequence.Start;
            _nextPosition = _currentPosition;
            _currentSpan = default;
            MoveNext();
        }

        public bool End => _end;

        public int Index => _index;

        public Position Position => _currentPosition + _index;

        public ReadOnlySpan<byte> Span => _currentSpan;


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
            ConsumedBytes++;

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
            while (Sequence.TryGet(ref _nextPosition, out var memory, true))
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

            ConsumedBytes += length;

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
