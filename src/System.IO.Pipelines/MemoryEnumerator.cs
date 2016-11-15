using System;

namespace System.IO.Pipelines
{
    /// <summary>
    /// An enumerator over the <see cref="ReadableBuffer"/>
    /// </summary>
    public struct MemoryEnumerator
    {
        private BufferSegment _segment;
        private Memory<byte> _current;
        private int _startIndex;
        private readonly int _endIndex;
        private readonly BufferSegment _endSegment;

        /// <summary>
        /// 
        /// </summary>
        public MemoryEnumerator(ReadCursor start, ReadCursor end)
        {
            _startIndex = start.Index;
            _segment = start.Segment;
            _endSegment = end.Segment;
            _endIndex = end.Index;
            _current = Memory<byte>.Empty;
        }

        /// <summary>
        /// The current <see cref="Memory{Byte}"/>
        /// </summary>
        public Memory<byte> Current => _current;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Moves to the next <see cref="Memory{Byte}"/> in the <see cref="ReadableBuffer"/>
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_segment == null)
            {
                return false;
            }

            int start = _segment.Start;
            int end = _segment.End;

            if (_startIndex != 0)
            {
                start = _startIndex;
                _startIndex = 0;
            }

            if (_segment == _endSegment)
            {
                end = _endIndex;
            }

            _current = _segment.Memory.Slice(start, end - start);

            if (_segment == _endSegment)
            {
                _segment = null;
            }
            else
            {
                _segment = _segment.Next;
            }

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