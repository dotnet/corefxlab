// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Buffers.Experimental
{
    // TODO: consider allowing Last > First. Ennumeration will count down.
    public readonly struct Range : IEnumerable<int>
    {
        public const int UnboundedFirst = Int32.MinValue;
        public const int UnboundedEnd = Int32.MaxValue;

        public readonly int First;
        public readonly int End; // End is exclusive

        public uint Length
        {
            get {
                if (IsBound) return (uint)((long)End - (long)First);
                throw new InvalidOperationException("cannot get length of unbound range");
            }
        }

        public Range(int first, uint length)
        {
            if (first == UnboundedFirst)
                throw new ArgumentOutOfRangeException(nameof(first));
            
            First = first;
            End = (int)(first + length);

            if (End < First) throw new ArgumentOutOfRangeException(nameof(length));
        }

        private Range(int first, int end)
        {
            First = first;
            End = end;
        }

        public bool IsBound => First != UnboundedFirst && End != UnboundedEnd;

        public void Deconstruct(out int first, out int end)
        {
            first = First;
            end = End;
        }

        public static Range Construct(int first, int end)
            => new Range(first, end);

        public struct Enumerator : IEnumerator<int>
        {
            int _current;
            int _end;

            internal Enumerator(int first, int end)
            {
                _current = first - 1;
                _end = end;
            }

            public bool MoveNext()
            {
                _current++;
                return _current < _end;
            }

            public int Current => _current;

            object IEnumerator.Current => _current;

            void IDisposable.Dispose() { }

            void IEnumerator.Reset() => throw new NotSupportedException();
        }

        public Enumerator GetEnumerator()
        {
            if(IsBound) return new Enumerator(First, End);
            throw new InvalidOperationException("cannot enumerate unbound range");
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool Contains(int value)
        {
            if (!IsBound) throw new InvalidOperationException("Unbound ranges cannot contain");
            return value >= First && value < End;
        }

        /// <summary>
        /// Returns true if this Range is a valid range for a zero based index of specified length.
        /// </summary>
        /// <param name="length">zero based length.</param>
        /// <returns></returns>
        public bool IsValid(int length)
        {
            if (First == UnboundedFirst)
            {
                if (End == UnboundedEnd) return true;
                return End <= length;
            }
            else // First is bounded
            {
                if (First < 0) return false;
                if (End == UnboundedEnd) return First <= length;
                if (First > length) return false;
                return End <= length;
            }
        }

        /// <summary>
        /// Converts an unbound Range (IsBound == false) to a bound one (IsBound == true).
        /// </summary>
        /// <param name="length">zero based length of an indexable 'list' the range is being bound to.</param>
        /// <returns>Bound Range (IsBound == true).</returns>
        /// <remarks>The method throws ArgumentOutOfRangeException Range.IsValid(lenght) returns false.</remarks>
        public Range Bind(int length)
        {
            if (!IsValid(length)) throw new ArgumentOutOfRangeException(nameof(length));
            if (IsBound) return this;

            int first = 0;
            if (First != UnboundedFirst) first = First;

            int end;
            if (End != UnboundedEnd) end = End;
            else end = length;

            return new Range(first, end);
        }

        /// <summary>
        /// Binds the range possibly adjusting it to fit in the length.
        /// </summary>
        /// <param name="length">zero based length.</param>
        /// <returns></returns>
        public Range BindToValid(int length)
        {
            int first = First;
            if (first < 0) first = 0;
            if (first > length - 1) first = length;

            int end = End;
            if (end == UnboundedEnd || end > length) end = length;

            return new Range(first, end);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            if (First != UnboundedFirst) sb.Append(First);
            sb.Append("..");
            if (End != UnboundedEnd) sb.Append(End);
            sb.Append(']');
            return sb.ToString();
        }
    }
}
