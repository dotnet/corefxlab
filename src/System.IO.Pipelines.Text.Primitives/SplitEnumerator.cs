// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Sequences;

namespace System.IO.Pipelines.Text.Primitives
{
    /// <summary>
    /// Supports a simple iteration over a sequence of buffers from a Split operation.
    /// </summary>
    public struct SplitEnumerator : IEnumerator<ReadOnlyBuffer<byte>>
    {
        private readonly byte _delimiter;
        private ReadOnlyBuffer<byte> _current, _remainder;
        internal SplitEnumerator(ReadOnlyBuffer<byte> remainder, byte delimiter)
        {
            _current = default;
            _remainder = remainder;
            _delimiter = delimiter;
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public ReadOnlyBuffer<byte> Current => _current;

        object IEnumerator.Current => _current;

        /// <summary>
        /// Releases all resources owned by the instance
        /// </summary>
        public void Dispose() { }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        public bool MoveNext()
        {
            if (_remainder.TrySliceTo(_delimiter, out _current, out Position cursor))
            {
                _remainder = _remainder.Slice(cursor).Slice(1);
                return true;
            }
            // once we're out of splits, yield whatever is left
            if (_remainder.IsEmpty)
            {
                return false;
            }
            _current = _remainder;
            _remainder = default;
            return true;
        }
    }
}
