// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System
{
    public partial struct Span<T>
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
            private readonly object _object;
            private readonly UIntPtr _offset;
            private readonly int _length;
            private int _position;

            internal Enumerator(Span<T> span)
            {
                _object = span.Object;
                _offset = span.Offset;
                _length = span.Length;
                _position = -1;
            }

            public T Current
            {
                get
                {
                    Contract.RequiresInRange(_position, (uint)_length);
                    return PtrUtils.Get<T>(_object, _offset, (UIntPtr)_position);
                }
            }

            public bool MoveNext()
            {
                return ++_position < _length;
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
            Span<T> _span;    // The slice being enumerated.
            int _position; // The current position.

            public EnumeratorObject(Span<T> span)
            {
                _span = span;
                _position = -1;
            }

            public T Current
            {
                get { return _span[_position]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                _span = default(Span<T>);
                _position = -1;
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
    }
}
