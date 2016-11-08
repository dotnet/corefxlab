// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct SequenceEnumerator<T>
    {
        Position _position;
        ISequence<T> _sequence;
        T _current;

        public SequenceEnumerator(ISequence<T> sequence) {
            _sequence = sequence;
            _position = Position.First;
            _current = default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() {
            return _sequence.TryGet(ref _position, out _current, advance: true);
        }

        public T Current => _current;
    }
}
