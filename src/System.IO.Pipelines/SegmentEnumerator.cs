namespace System.IO.Pipelines
{
    internal struct SegmentEnumerator
    {
        private BufferSegment _segment;
        private SegmentPart _current;
        private int _startIndex;
        private readonly int _endIndex;
        private readonly BufferSegment _endSegment;

        /// <summary>
        /// 
        /// </summary>
        public SegmentEnumerator(ReadCursor start, ReadCursor end)
        {
            _startIndex = start.Index;
            _segment = start.Segment;
            _endSegment = end.Segment;
            _endIndex = end.Index;
            _current = default(SegmentPart);
        }

        /// <summary>
        /// The current <see cref="Memory{Byte}"/>
        /// </summary>
        public SegmentPart Current => _current;

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

            _current = new SegmentPart()
            {
                Segment = _segment,
                Start = start,
                End = end,
            };

            if (_segment == _endSegment)
            {
                _segment = null;
            }
            else
            {
                _segment = _segment.Next;
                if (_segment == null)
                {
                    throw new InvalidOperationException("Segments ended by end was never seen");
                }
            }

            return true;
        }

        public SegmentEnumerator GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            ThrowHelper.ThrowNotSupportedException();
        }

        internal struct SegmentPart
        {
            public BufferSegment Segment;
            public int Start;
            public int End;

            public int Length => End - Start;
        }
    }
}