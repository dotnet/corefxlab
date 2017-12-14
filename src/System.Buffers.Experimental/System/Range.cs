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
        public const uint UnboundedLength = UInt32.MaxValue;

        public readonly int First;
        public readonly int Last; // Last is exclusive

        public uint Length
        {
            get {
                if (First == UnboundedFirst || Last == UnboundedLast) return UnboundedLength;
                return (uint)((long)Last - (long)First);
            }
        }

        public Range(int first, uint length)
        {
            if (first == UnboundedFirst)
                throw new ArgumentOutOfRangeException(nameof(first));
            
            First = first;
            Last = (int)(first + length);

            if (Last < First) throw new ArgumentOutOfRangeException(nameof(length));
        }

        private Range(int first, int last)
        {
            First = first;
            Last = last;
        }

        public void Deconstruct(out int first, out int last)
        {
            first = First;
            last = Last;
        }
        public static Range Construct(int first, int last)
            => new Range(first, last);

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
                return _first < _last;
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
