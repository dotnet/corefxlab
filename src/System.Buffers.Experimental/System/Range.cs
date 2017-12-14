// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace System
{
    // TODO: consider allowing Last > First. Ennumeration will count down.
    public struct Range : IEnumerable<int>
    {
        public const int UnboundedFirst = Int32.MinValue;
        public const int UnboundedLast = Int32.MaxValue;

        public readonly int First;
        public readonly int Last; // Last is exclusive

        public uint Length
        {
            get {
                if (IsBound) return (uint)((long)Last - (long)First);
                throw new InvalidOperationException("cannot get length of unbound range");
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

        public bool IsBound => First != UnboundedFirst && Last != UnboundedLast;

        public void Deconstruct(out int first, out int last)
        {
            first = First;
            last = Last;
        }
        public static Range Construct(int first, int last)
            => new Range(first, last);

        public struct Enumerator : IEnumerator<int>
        {
            int _current;
            int _last;

            internal Enumerator(int first, int last)
            {
                _current = first - 1;
                _last = last;
            }

            public bool MoveNext()
            {
                _current++;
                return _current < _last;
            }

            public int Current => _current;

            object IEnumerator.Current => _current;

            void IDisposable.Dispose() { }

            void IEnumerator.Reset() => throw new NotSupportedException();
        }

        // TODO: write benchmark for this
        public Enumerator GetEnumerator()
        {
            if(IsBound) return new Enumerator(First, Last);
            throw new InvalidOperationException("cannot enumerate unbound range");
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
