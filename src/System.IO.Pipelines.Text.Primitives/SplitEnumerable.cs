using System.Collections;
using System.Collections.Generic;

namespace System.IO.Pipelines.Text.Primitives
{
    /// <summary>
    /// Exposes the enumerator, which supports a simple iteration over a collection of a specified type.
    /// </summary>
    public struct SplitEnumerable : IEnumerable<ReadableBuffer>
    {
        private ReadableBuffer _buffer;

        private int _count;

        private byte _delimiter;

        internal SplitEnumerable(ReadableBuffer buffer, byte delimiter)
        {
            _buffer = buffer;
            _delimiter = delimiter;
            _count = buffer.IsEmpty ? 0 : -1;
        }

        /// <summary>
        /// Count the number of elemnts in this sequence
        /// </summary>
        public int Count()
        {
            if (_count >= 0)
            {
                return _count;
            }

            int count = 1;
            var current = _buffer;
            ReadableBuffer ignore;
            ReadCursor cursor;
            while (current.TrySliceTo(_delimiter, out ignore, out cursor))
            {
                current = current.Slice(cursor).Slice(1);
                count++;
            }
            return _count = count;
        }
        /// <summary>
        ///  Returns an enumerator that iterates through the collection.
        /// </summary>
        public SplitEnumerator GetEnumerator()
            => new SplitEnumerator(_buffer, _delimiter);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        IEnumerator<ReadableBuffer> IEnumerable<ReadableBuffer>.GetEnumerator()
                    => GetEnumerator();
    }
}
