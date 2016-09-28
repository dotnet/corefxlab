// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct SequenceEnumerator<T>
    {
        Position _position;
        ISequence<T> _sequence;
        T _item;

        public SequenceEnumerator(ISequence<T> sequence)
        {
            _sequence = sequence;
            _position = Position.BeforeFirst;
            _item = default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _item = _sequence.TryGetItem(ref _position);
            if (_position.IsValid) {
                return true;
            }
            return false;
        }

        public T Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return _item;
            }
        }
    }

    public static class SequenceEnumeratorExtensions
    {
        public static SequenceEnumerator<T> GetEnumerator<T>(this ISequence<T> sequence)
        {
            return new SequenceEnumerator<T>(sequence);
        }
    }
}
