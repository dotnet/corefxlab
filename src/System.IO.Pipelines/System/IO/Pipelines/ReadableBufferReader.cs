// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public ref struct ReadableBufferReader
    {
        private ReadOnlySpan<byte> _currentSpan;
        private int _index;
        private BufferEnumerator _enumerator;
        private int _consumedBytes;
        private bool _end;

        public ReadableBufferReader(ReadableBuffer buffer) : this(buffer.Start, buffer.End)
        {
        }

        public ReadableBufferReader(ReadCursor start, ReadCursor end) : this()
        {
            _end = false;
            _index = 0;
            _consumedBytes = 0;
            _enumerator = new BufferEnumerator(start, end);
            _currentSpan = default;
            MoveNext();
        }

        public bool End => _end;

        public int Index => _index;

        public ReadCursor Cursor => _enumerator.CreateCursor(_index);

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
            while (_enumerator.MoveNext())
            {
                _index = 0;
                var memory = _enumerator.Current;
                var length = memory.Length;
                if (length != 0)
                {
                    _currentSpan = memory.Span;
                    return;
                }
            }

            _end = true;
        }

        public void Skip(int length)
        {
            if (length < 0)
            {
                PipelinesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
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
                PipelinesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }
    }
}
