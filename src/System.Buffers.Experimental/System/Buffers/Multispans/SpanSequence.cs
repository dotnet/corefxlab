// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    [Obsolete("we will use multiple Memory<T> instances to represent sequences of buffers")]
    public interface ISpanSequence<T> : IReadOnlySpanSequence<T>
    {
        new SpanSequenceEnumerator<T> GetEnumerator();
        bool TryGet(ref Position position, out Span<T> item, bool advance = false);
    }

    [Obsolete("we will use multiple Memory<T> instances to represent sequences of buffers")]
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
            Span<T> span;
            var result = _sequence.TryGet(ref _position, out span, advance: true);
            if (_position == Position.First) return true;
            if (_position == Position.AfterLast) return false;
            return result;
        }

        public Span<T> Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Span<T> span;
                if (!_sequence.TryGet(ref _position, out span, advance: false))
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return span;
            }
        }
    }
}

