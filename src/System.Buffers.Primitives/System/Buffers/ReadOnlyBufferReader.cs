// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public static class BufferReader
    {
        public static BufferReader<TSequence> Create<TSequence>(TSequence buffer) where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            return new BufferReader<TSequence>(buffer);
        }

        public static int Peek<TSequence>(BufferReader<TSequence> reader, Span<byte> destination)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
            => BufferReader<TSequence>.Peek(reader, destination);
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
            _currentSpan = ReadOnlySpan<byte>.Empty;
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

        internal static int Peek(BufferReader<TSequence> bytes, Span<byte> destination)
        {
            var first = bytes.UnreadSegment;
            if (first.Length > destination.Length)
            {
                first.Slice(0, destination.Length).CopyTo(destination);
                return destination.Length;
            }
            else if (first.Length == destination.Length)
            {
                first.CopyTo(destination);
                return destination.Length;
            }
            else
            {
                first.CopyTo(destination);
                int copied = first.Length;

                var next = bytes._nextPosition;
                while (bytes._sequence.TryGet(ref next, out ReadOnlyMemory<byte> nextSegment, true))
                {
                    var nextSpan = nextSegment.Span;
                    if (nextSpan.Length > 0)
                    {
                        var toCopy = Math.Min(nextSpan.Length, destination.Length - copied);
                        nextSpan.Slice(0, toCopy).CopyTo(destination.Slice(copied));
                        copied += toCopy;
                        if (copied >= destination.Length) break;
                    }
                }
                return copied;
            }
        }
    }
}
