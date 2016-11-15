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

        public ReadableBufferReader(ReadableBuffer buffer)
        {
            _end = false;
            _index = 0;
            _overallIndex = 0;
            _enumerator = buffer.GetEnumerator();
            _currentMemory = default(Span<byte>);
            while (_enumerator.MoveNext())
            {
                if (!_enumerator.Current.IsEmpty)
                {
                    _currentMemory = _enumerator.Current.Span;
                    return;
                }
            }
            _end = true;
        }

        public bool End => _end;

        public int Index => _overallIndex;

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
            _index++;
            _overallIndex++;

            if (_index >= _currentMemory.Length)
            {
                MoveNext();
            }

            return value;
        }

        private void MoveNext()
        {
            if (_enumerator.MoveNext())
            {
                _currentMemory = _enumerator.Current.Span;
                _index = 0;
            }
            else
            {
                _end = true;
            }
        }
    }
}