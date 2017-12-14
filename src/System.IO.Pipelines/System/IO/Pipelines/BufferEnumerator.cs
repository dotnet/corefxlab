// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    /// <summary>
    /// An enumerator over the <see cref="ReadableBuffer"/>
    /// </summary>
    public struct BufferEnumerator
    {
        private ReadCursor _cursor;
        private ReadCursor _next;
        private readonly ReadCursor _end;

        private ReadOnlyMemory<byte> _current;

        /// <summary>
        ///
        /// </summary>
        public BufferEnumerator(ReadCursor start, ReadCursor end)
        {
            _cursor = default;
            _next = start;
            _end = end;
            _current = default;
        }

        /// <summary>
        /// The current <see cref="Buffer{Byte}"/>
        /// </summary>
        public ReadOnlyMemory<byte> Current
        {
            get => _current;
            set => _current = value;
        }

        /// <summary>
        /// Moves to the next <see cref="Buffer{Byte}"/> in the <see cref="ReadableBuffer"/>
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_next.IsDefault)
            {
                return false;
            }

            _cursor = _next;

            return ReadCursorOperations.TryGetBuffer(_cursor, _end, out _current, out _next);
        }

        public BufferEnumerator GetEnumerator()
        {
            return this;
        }

        public void Reset()
        {
            PipelinesThrowHelper.ThrowNotSupportedException();
        }

        public ReadCursor CreateCursor(int offset)
        {
            return new ReadCursor(_cursor.Segment, _cursor.Index + offset);
        }
    }
}
