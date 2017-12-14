// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System
{

    public struct Range : IEnumerable<int>
    {
        public const int UnboundedFirst = Int32.MinValue;
        public const int UnboundedLast = Int32.MaxValue;

        public readonly int First;
        public readonly uint Length;
        public int Last => (int)(Length + First - 1);

        public Range(int first, uint length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("Length must be non-negative.", nameof(length));
            }

            First = first;
            Length = length;
        }

        public void Deconstruct(out int first, out int last)
        {
            first = First;
            last = Last;
        }
        public static Range Construct(int first, int last = UnboundedLast)
        {
            if (last == UnboundedLast) return new Range(first, (uint)((long)int.MaxValue - first));
            long length = (long)last - first + 1;
            if (length > uint.MaxValue || length < 0) throw new ArgumentOutOfRangeException(nameof(last));
            return new Range(first, (uint)length);
        }

        public struct Enumerator : IEnumerator<int>
        {
            int _first;
            int _last;

            internal Enumerator(int first, int last)
            {
                _first = first - 1;
                _last = last;
            }

            public bool MoveNext()
            {
                _first++;
                return _first <= _last;
            }

            public int Current => _first;

            object IEnumerator.Current => _first;

            void IDisposable.Dispose() { }

            void IEnumerator.Reset() => throw new NotSupportedException();
        }

        public Enumerator GetEnumerator()
            => new Enumerator(First, Last);

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
