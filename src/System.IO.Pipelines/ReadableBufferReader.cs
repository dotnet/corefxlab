// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct ReadableBufferReader
    {
        private Span<byte> _currentMemory;
        private int _index;
        private MemoryEnumerator _enumerator;
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
            _enumerator = new MemoryEnumerator(start, end);
            _currentMemory = default(Span<byte>);
            MoveNext();
        }

        public bool End => _end;

        public int Index => _overallIndex;

        public ReadCursor Cursor
        {
            get
            {
                var currentSegment = _enumerator.CurrentSegment;
                if (_end)
                {
                    return new ReadCursor(currentSegment, currentSegment.Start + _currentMemory.Length);
                }
                return new ReadCursor(currentSegment, currentSegment.Start + _index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (_end)
            {
                return -1;
            }
            return _currentMemory[_index];
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

            if (_index >= _currentMemory.Length)
            {
                MoveNext();
            }

            return value;
        }

        private void MoveNext()
        {
            while (_enumerator.MoveNext())
            {
                if (!_enumerator.Current.IsEmpty)
                {
                    _currentMemory = _enumerator.Current.Span;
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
                if ((_index + length) < _currentMemory.Length)
                {
                    _index += length;
                    length = 0;
                    break;
                }

                length -= (_currentMemory.Length - _index);
                MoveNext();
            }

            if (length > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }
    }
}