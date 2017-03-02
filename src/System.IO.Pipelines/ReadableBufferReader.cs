// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct ReadableBufferReader
    {
        private Span<byte> _currentSpan;
        private int _index;
        private SegmentEnumerator _enumerator;
        private int _overallIndex;
        private bool _end;

        public ReadableBufferReader(ReadableBuffer buffer) : this(buffer.Start, buffer.End)
        {
        }

        public ReadableBufferReader(ReadCursor start, ReadCursor end) : this()
        {
            _end = false;
            _index = 0;
            _overallIndex = 0;
            _enumerator = new SegmentEnumerator(start, end);
            _currentSpan = default(Span<byte>);
            MoveNext();
        }

        public bool End => _end;

        public int Index => _overallIndex;

        public ReadCursor Cursor
        {
            get
            {
                var part = _enumerator.Current;

                if (_end)
                {
                    return new ReadCursor(part.Segment, part.Start + _currentSpan.Length);
                }

                return new ReadCursor(part.Segment, part.Start + _index);
            }
        }

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
            var value = Peek();
            if (!_end)
            {
                _index++;
                _overallIndex++;
            }

            if (_index >= _currentSpan.Length)
            {
                MoveNext();
            }

            return value;
        }

        private void MoveNext()
        {
            while (_enumerator.MoveNext())
            {
                if (_enumerator.Current.Length != 0)
                {
                    var part = _enumerator.Current;
                    _currentSpan = part.Segment.Memory.Span.Slice(part.Start, part.Length);
                    _index = 0;
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

            while (!_end && length > 0)
            {
                if ((_index + length) < _currentSpan.Length)
                {
                    _index += length;
                    length = 0;
                    break;
                }

                length -= (_currentSpan.Length - _index);
                MoveNext();
            }

            if (length > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }
    }
}