// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct ReadableBufferReader
    {
        private BufferSegment _currentSegment;
        private Span<byte> _currentSpan;

        private int _cursorOffset;
        private int _spanIndex;
        private int _remainingBytes;
        private int _consumedBytes;

        public ReadableBufferReader(ReadableBuffer buffer)
        {
            var start = buffer.Start;
            var length = buffer.Length;
            var segment = start.Segment;
            var startIndex = start.Index;

            while (segment != null && segment.End == startIndex)
            {
                segment = segment.Next;
                if (segment == null)
                {
                    break;
                }

                startIndex = segment.Start;
            }

            if (length == 0)
            {
                _currentSpan = default(Span<byte>);
            }
            else
            {
                _currentSpan = segment.Memory.Span.Slice(startIndex, segment.End - startIndex);
            }

            _cursorOffset = startIndex;
            _currentSegment = segment;
            _remainingBytes = length;
            _consumedBytes = 0;
            _spanIndex = 0;
        }

        public bool End => _remainingBytes == 0;

        public int Index => _spanIndex;

        public ReadCursor Cursor => new ReadCursor(_currentSegment, _cursorOffset + _spanIndex);

        public Span<byte> Span => _currentSpan;

        public int ConsumedBytes => _consumedBytes;

        public int RemainingBytes => _remainingBytes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (_remainingBytes == 0)
            {
                return -1;
            }

            return _currentSpan[_spanIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Take()
        {
            if (_remainingBytes == 0)
            {
                return -1;
            }

            var spanIndex = _spanIndex;
            _remainingBytes--;
            _consumedBytes++;
            _spanIndex = spanIndex + 1;

            var value = _currentSpan[spanIndex];

            if (spanIndex >= _currentSpan.Length - 1 && _remainingBytes > 0)
            {
                TraverseSegments(0);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int length)
        {
            if ((uint)_remainingBytes < (uint)length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            _remainingBytes -= length;
            _consumedBytes += length;

            var spanLength = _currentSpan.Length;
            if (spanLength - length == _spanIndex)
            {
                _spanIndex += length;
                TraverseSegments(0);
            }
            else if (spanLength - length > _spanIndex)
            {
                _spanIndex += length;
            }
            else
            {
                TraverseSegments(length - (spanLength - _spanIndex));
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void TraverseSegments(int skip)
        {
            var segment = _currentSegment;
            if (_remainingBytes <= 0)
            {
                goto complete;
            }

            segment = segment.Next;
            var remaining = skip;
            var segmentLength = segment.ReadableBytes;
            while (segmentLength <= remaining || segmentLength == 0)
            {
                remaining -= segmentLength;

                if (segment.Next == null && _remainingBytes == 0)
                {
                    _spanIndex = 0;
                    _cursorOffset = segment.End;
                    goto complete;
                }
                segment = segment.Next;
                Debug.Assert(segment != null);
                segmentLength = segment.ReadableBytes;
            }

            _currentSegment = segment;
            _spanIndex = 0;
            _cursorOffset = segment.Start;
            _currentSpan = segment.Memory.Span.Slice(segment.Start, Math.Min(segment.End - segment.Start, _remainingBytes));
            return;
        complete:
            Debug.Assert(_remainingBytes == 0);
            _currentSegment = segment;
            _currentSpan = default(Span<byte>);
        }
    }
}