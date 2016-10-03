// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public interface ISpanSequence<T>
    {
        SpanSequenceEnumerator<T> GetEnumerator();
        bool TryGet(ref Position position, out Span<T> item, bool advance = false);

        /// <summary>
        /// Total number of items (Ts) in all spans in the sequence.
        /// </summary>
        int? TotalLength { get; }

        /// <summary>
        /// Number of spans in the sequence.
        /// </summary>
        int? Count { get; }
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
            Span<T> span;
            return _sequence.TryGet(ref _position, out span, advance:true);
        }

        public Span<T> Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Span<T> span;
                if(_sequence.TryGet(ref _position, out span, advance: false)) {
                    return span;
                }
                throw new InvalidOperationException();
            }
        }
    }
}

