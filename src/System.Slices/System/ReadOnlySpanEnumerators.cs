// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;

namespace System
{
    public partial struct ReadOnlySpan<T>
    {
        public Enumerator GetEnumerator()
        {
            return new Enumerator(Object, Offset, Length);
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

            internal Enumerator(object obj, UIntPtr offset, int length)
            {
                _object = obj;
                _offset = offset;
                _length = length;
                _position = -1;
            }

            public T Current
            {
                get
                {
                    Contract.RequiresInRange(_position, (uint)_length);
                    return UnsafeUtilities.Get<T>(_object, _offset, (UIntPtr)_position);
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
    }
}
