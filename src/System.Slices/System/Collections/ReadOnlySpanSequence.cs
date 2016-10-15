// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Collections.Sequences
{
    public interface IReadOnlySpanSequence<T>
    {
        ReadOnlySpanSequenceEnumerator<T> GetEnumerator();
        bool TryGet(ref Position position, out ReadOnlySpan<T> item, bool advance = false);

        /// <summary>
        /// Total number of items (Ts) in all spans in the sequence.
        /// </summary>
        int? TotalLength { get; }

        /// <summary>
        /// Number of spans in the sequence.
        /// </summary>
        int? Count { get; }
    }

    public struct ReadOnlySpanSequenceEnumerator<T>
    {
        Position _position;
        IReadOnlySpanSequence<T> _sequence;

        public ReadOnlySpanSequenceEnumerator(IReadOnlySpanSequence<T> sequence)
        {
            _sequence = sequence;
            _position = Position.BeforeFirst;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            ReadOnlySpan<T> span;
            var result = _sequence.TryGet(ref _position, out span, advance:true);
            if (_position == Position.First) return true;
            if (_position == Position.AfterLast) return false;
            return result;
        }

        public ReadOnlySpan<T> Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ReadOnlySpan<T> span;
                if(!_sequence.TryGet(ref _position, out span, advance: false))
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }

                return span;
            }
        }
    }
}

