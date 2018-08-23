// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader<T> where T : unmanaged, IEquatable<T>
    {
        private SequencePosition _currentPosition;
        private SequencePosition _nextPosition;
        private bool _moreData;

        /// <summary>
        /// Create a <see cref="BufferReader" over the given <see cref="ReadOnlySequence{T}"/>./>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BufferReader(in ReadOnlySequence<T> buffer)
        {
            CurrentSpanIndex = 0;
            Consumed = 0;
            Sequence = buffer;
            _currentPosition = Sequence.Start;
            _nextPosition = _currentPosition;

            if (buffer.TryGet(ref _nextPosition, out ReadOnlyMemory<T> memory, advance: true))
            {
                _moreData = true;
                CurrentSpan = memory.Span;
                if (CurrentSpan.Length == 0)
                {
                    // No space in the first span, move to one with space
                    GetNextSpan();
                }
            }
            else
            {
                // No space in any spans and at end of sequence
                _moreData = false;
                CurrentSpan = default;
            }
        }

        /// <summary>
        /// True when there is no more data in the <see cref="Sequence"/>.
        /// </summary>
        public bool End => !_moreData;

        /// <summary>
        /// The underlying <see cref="ReadOnlySequence{T}"/> for the reader.
        /// </summary>
        public ReadOnlySequence<T> Sequence { get; }

        /// <summary>
        /// The current position in the <see cref="Sequence"/>.
        /// </summary>
        public SequencePosition Position => Sequence.GetPosition(CurrentSpanIndex, _currentPosition);

        /// <summary>
        /// The current segment in the <see cref="Sequence"/>.
        /// </summary>
        public ReadOnlySpan<T> CurrentSpan { get; private set; }

        /// <summary>
        /// The index in the <see cref="CurrentSpan"/>.
        /// </summary>
        public int CurrentSpanIndex { get; private set; }

        /// <summary>
        /// The unread portion of the <see cref="CurrentSpan"/>.
        /// </summary>
        public ReadOnlySpan<T> UnreadSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => CurrentSpanIndex == 0 ? CurrentSpan : CurrentSpan.Slice(CurrentSpanIndex);
        }

        /// <summary>
        /// The total number of {T}s processed by the reader.
        /// </summary>
        public int Consumed { get; private set; }

        /// <summary>
        /// Peeks at the next value without advancing the reader.
        /// </summary>
        /// <param name="value">The next value or default if at the end.</param>
        /// <returns>False if at the end of the reader.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(out T value)
        {
            if (End)
            {
                value = default;
                return false;
            }
            else
            {
                value = CurrentSpan[CurrentSpanIndex];
                return true;
            }
        }

        /// <summary>
        /// Read the next value and advance the reader.
        /// </summary>
        /// <param name="value">The next value or default if at the end.</param>
        /// <returns>False if at the end of the reader.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRead(out T value)
        {
            if (End)
            {
                value = default;
                return false;
            }

            value = CurrentSpan[CurrentSpanIndex];
            CurrentSpanIndex++;
            Consumed++;

            if (CurrentSpanIndex >= CurrentSpan.Length)
            {
                GetNextSpan();
            }

            return true;
        }

        /// <summary>
        /// Get the next segment with available space, if any.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GetNextSpan()
        {
            if (!Sequence.IsSingleSegment)
            {
                SequencePosition previousNextPosition = _nextPosition;
                while (Sequence.TryGet(ref _nextPosition, out ReadOnlyMemory<T> memory, advance: true))
                {
                    _currentPosition = previousNextPosition;
                    CurrentSpan = memory.Span;
                    CurrentSpanIndex = 0;
                    if (CurrentSpan.Length > 0)
                    {
                        return;
                    }
                }
            }
            _moreData = false;
        }

        /// <summary>
        /// Move the reader ahead the specified number of positions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            if (count == 0)
            {
                return;
            }

            if (count < 0 || End)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            Consumed += count;

            if (CurrentSpanIndex < CurrentSpan.Length - count)
            {
                CurrentSpanIndex += count;
            }
            else
            {
                // Current segment doesn't have enough space, scan forward through segments
                AdvanceToNextSpan(count);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AdvanceToNextSpan(int count)
        {
            while (!End && count > 0)
            {
                if (CurrentSpanIndex < CurrentSpan.Length - count)
                {
                    CurrentSpanIndex += count;
                    count = 0;
                    break;
                }

                int remaining = (CurrentSpan.Length - CurrentSpanIndex);

                CurrentSpanIndex += remaining;
                count -= remaining;

                GetNextSpan();
            }

            if (count > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        /// <summary>
        /// Peek forward the number of positions specified by <paramref name="count"/>.
        /// </summary>
        /// <returns>Span over the peeked data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Peek(int count)
        {
            ReadOnlySpan<T> firstSpan = UnreadSpan;
            if (firstSpan.Length >= count)
            {
                return firstSpan.Slice(0, count);
            }

            // Not enough contiguous Ts, allocate and copy what we can get
            return PeekSlow(new T[count]);
        }

        /// <summary>
        /// Peek forward the number of positions in <paramref name="copyBuffer"/>, copying into
        /// <paramref name="copyBuffer"/> if needed.
        /// </summary>
        /// <param name="copyBuffer">
        /// Temporary buffer to copy into if there isn't a contiguous span within the existing data to return.
        /// Also describes the count of positions to peek.
        /// </param>
        /// <returns>Span over the peeked data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Peek(Span<T> copyBuffer)
        {
            ReadOnlySpan<T> firstSpan = UnreadSpan;
            if (firstSpan.Length >= copyBuffer.Length)
            {
                return firstSpan.Slice(0, copyBuffer.Length);
            }

            return PeekSlow(copyBuffer);
        }

        internal ReadOnlySpan<T> PeekSlow(Span<T> destination)
        {
            ReadOnlySpan<T> firstSpan = UnreadSpan;
            firstSpan.CopyTo(destination);
            int copied = firstSpan.Length;

            SequencePosition next = _nextPosition;
            while (Sequence.TryGet(ref next, out ReadOnlyMemory<T> nextSegment, true))
            {
                ReadOnlySpan<T> nextSpan = nextSegment.Span;
                if (nextSpan.Length > 0)
                {
                    int toCopy = Math.Min(nextSpan.Length, destination.Length - copied);
                    nextSpan.Slice(0, toCopy).CopyTo(destination.Slice(copied));
                    copied += toCopy;
                    if (copied >= destination.Length)
                        break;
                }
            }

            return destination.Slice(0, copied);
        }
    }
}
