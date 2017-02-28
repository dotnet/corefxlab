// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// An enumerator over the <see cref="ReadableBuffer"/>
    /// </summary>
    public struct MemoryEnumerator
    {
        private SegmentEnumerator _segmentEnumerator;
        private Memory<byte> _current;

        /// <summary>
        /// 
        /// </summary>
        public MemoryEnumerator(ReadCursor start, ReadCursor end)
        {
            _segmentEnumerator = new SegmentEnumerator(start, end);
            _current = default(Memory<byte>);
        }

        /// <summary>
        /// The current <see cref="Memory{Byte}"/>
        /// </summary>
        public Memory<byte> Current => _current;

        internal BufferSegment CurrentSegment => _segmentEnumerator.Current.Segment;

        internal int CurrentSegmentStartIndex => _segmentEnumerator.Current.Start;

        /// <summary>
        /// Moves to the next <see cref="Memory{Byte}"/> in the <see cref="ReadableBuffer"/>
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (!_segmentEnumerator.MoveNext())
            {
                _current = Memory<byte>.Empty;
                return false;
            }
            var current = _segmentEnumerator.Current;
            _current = current.Segment.Memory.Slice(current.Start, current.Length);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            ThrowHelper.ThrowNotSupportedException();
        }
    }
}