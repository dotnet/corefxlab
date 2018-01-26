// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Sequences;

namespace System.IO.Pipelines.Text.Primitives
{
    /// <summary>
    /// Exposes the enumerator, which supports a simple iteration over a collection of a specified type.
    /// </summary>
    public struct SplitEnumerable : IEnumerable<ReadOnlyBuffer<byte>>
    {
        private ReadOnlyBuffer<byte> _buffer;

        private int _count;

        private byte _delimiter;

        internal SplitEnumerable(ReadOnlyBuffer<byte> buffer, byte delimiter)
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
            while (current.TrySliceTo(_delimiter, out ReadOnlyBuffer<byte> ignore, out SequenceIndex cursor))
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

        IEnumerator<ReadOnlyBuffer<byte>> IEnumerable<ReadOnlyBuffer<byte>>.GetEnumerator()
                    => GetEnumerator();
    }
}
