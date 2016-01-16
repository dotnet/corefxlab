// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System
{
    public partial struct ReadOnlySpan<T>
    {
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorObject(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new EnumeratorObject(this);
        }

        /// <summary>
        /// A struct-based enumerator, to make fast enumerations possible.
        /// This isn't designed for direct use, instead see GetEnumerator.
        /// </summary>
        public struct Enumerator
        {
            ReadOnlySpan<T> _span;    // The slice being enumerated.
            int _position; // The current position.

            internal Enumerator(ReadOnlySpan<T> span)
            {
                _span = span;
                _position = -1;
            }

            public T Current
            {
                get { return _span[_position]; }
            }

            public bool MoveNext()
            {
                return ++_position < _span.Length;
            }

            public void Reset()
            {
                _position = -1;
            }
        }

        /// <summary>
        /// enumerator that implements <see cref="IEnumerator{T}"/> pattern (including <see cref="IDisposable"/>).
        /// it is used by LINQ and foreach when Slice is accessed via <see cref="IEnumerable{T}"/>
        /// it is reference type to avoid boxing when calling interface methods on stuctures
        /// </summary>
        private class EnumeratorObject : IEnumerator<T>
        {
            ReadOnlySpan<T> _span;    // The slice being enumerated.
            int _position; // The current position.

            public EnumeratorObject(ReadOnlySpan<T> span)
            {
                _span = span;
                _position = -1;
            }

            public T Current
            {
                get { return _span.GetItemWithoutBoundariesCheck(_position); }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                _span = default(ReadOnlySpan<T>);
                _position = -1;
            }

            public bool MoveNext()
            {
                int nextItemIndex = _position + 1;
                if (nextItemIndex < _span.Length)
                {
                    _position = nextItemIndex;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _position = -1;
            }
        }
    }
}
