// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct ReadableBufferReader
    {
        private Span<byte> _currentSpan;
        private int _index;
        private SegmentEnumerator _enumerator;
        private int _consumedBytes;
        private bool _end;

        public ReadableBufferReader(ReadableBuffer buffer) : this(buffer.Start, buffer.End)
        {
        }

        public ReadableBufferReader(ReadCursor start, ReadCursor end)
        {
            _end = false;
            _index = 0;
            _consumedBytes = 0;
            _currentSpan = default(Span<byte>);
            _enumerator = new SegmentEnumerator(start, end);

            AdvanceSegmentInlined();
        }

        public bool End => _end;

        public int Index => _index;

        public ReadCursor Cursor
        {
            get
            {
                var part = _enumerator.Current;
                var index = !_end ? _index : _currentSpan.Length;

                return new ReadCursor()
                {
                    Segment = part.Segment,
                    Index = part.Start + index
                };
            }
        }

        public Span<byte> Span => _currentSpan;

        public int ConsumedBytes => _consumedBytes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (!_end)
            {
                return _currentSpan[_index];
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Take()
        {
            if (!_end)
            {
                var index = _index;
                var value = _currentSpan[index];

                _index = index + 1;
                _consumedBytes++;

                if (index + 1 >= _currentSpan.Length)
                {
                    AdvanceSegmentNoInline();
                }

                return value;
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int length)
        {
            if (length < 0 || _end)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            _consumedBytes += length;

            var remaining = _currentSpan.Length - _index;
            if (length < remaining)
            {
                _index += length;
            }
            else
            {
                SkipMultiSegment(length - remaining);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SkipMultiSegment(int length)
        {
            AdvanceSegmentInlined();
            if (length > 0 && _end)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            do
            {
                var remaining = _currentSpan.Length - _index;
                if (length < remaining)
                {
                    _index += length;
                    return;
                }

                length -= remaining;
                AdvanceSegmentNoInline();
            } while (length > 0 && !_end);

            if (length > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AdvanceSegmentNoInline()
        {
            AdvanceSegmentInlined();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AdvanceSegmentInlined()
        {
            while (_enumerator.MoveNext())
            {
                var part = _enumerator.Current;
                var length = part.Length;
                if (length != 0)
                {
                    _currentSpan = part.Segment.Buffer.Span.Slice(part.Start, length);
                    _index = 0;
                    return;
                }
            }

            _end = true;
        }
    }
}