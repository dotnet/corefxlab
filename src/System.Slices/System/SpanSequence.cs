// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System
{
    public interface ISpanSequence<T>
    {
        SpanSequenceEnumerator<T> GetEnumerator();
        Span<T> GetAt(ref Position position, bool advance = false);
    }

    public struct SpanSequenceEnumerator<T>
    {
        Position _position;
        ISpanSequence<T> _sequence;

        public SpanSequenceEnumerator(ISpanSequence<T> sequence)
        {
            _sequence = sequence;
            _position = Position.BeforeFirst;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var result = _sequence.GetAt(ref _position, advance:true);
            return _position.IsValid;
        }

        public Span<T> Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return _sequence.GetAt(ref _position);
            }
        }
    }
}

