// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public struct SequenceEnumerator<T>
    {
        SequencePosition _position;
        ISequence<T> _sequence;
        T _current;

        public SequenceEnumerator(ISequence<T> sequence)
        {
            _sequence = sequence;
            _position = default;
            _current = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _sequence.TryGet(ref _position, out _current);

        public T Current => _current;
    }

    public struct SequenceEnumerator<T, TSequence>
        where TSequence : ISequence<T>
    {
        SequencePosition _position;
        TSequence _sequence;
        T _current;

        public SequenceEnumerator(TSequence sequence)
        {
            _sequence = sequence;
            _position = default;
            _current = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _sequence.TryGet(ref _position, out _current);

        public T Current => _current;
    }
}
