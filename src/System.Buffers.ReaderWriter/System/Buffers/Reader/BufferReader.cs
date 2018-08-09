// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader
    {
        private SequencePosition _currentPosition;
        private SequencePosition _nextPosition;

        /// <summary>
        /// Create a <see cref="BufferReader" over the given <see cref="ReadOnlySequence{byte}"/>./>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BufferReader(in ReadOnlySequence<byte> buffer)
        {
            CurrentSpanIndex = 0;
            ConsumedBytes = 0;
            Sequence = buffer;
            _currentPosition = Sequence.Start;
            _nextPosition = _currentPosition;

            if (buffer.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
            {
                End = false;
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
                End = true;
                CurrentSpan = default;
            }
        }

        /// <summary>
        /// True when there is no more data in the <see cref="Sequence"/>.
        /// </summary>
        public bool End { get; private set; }

        /// <summary>
        /// The underlying <see cref="ReadOnlySequence{byte}"/> for the reader.
        /// </summary>
        public ReadOnlySequence<byte> Sequence { get; }

        /// <summary>
        /// The current position in the <see cref="Sequence"/>.
        /// </summary>
        public SequencePosition Position => Sequence.GetPosition(CurrentSpanIndex, _currentPosition);

        /// <summary>
        /// The current segment in the <see cref="Sequence"/>.
        /// </summary>
        public ReadOnlySpan<byte> CurrentSpan { get; private set; }

        /// <summary>
        /// The index in the <see cref="CurrentSpan"/>.
        /// </summary>
        public int CurrentSpanIndex { get; private set; }

        /// <summary>
        /// The unread portion of the <see cref="CurrentSpan"/>.
        /// </summary>
        public ReadOnlySpan<byte> UnreadSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => CurrentSpanIndex == 0 ? CurrentSpan : CurrentSpan.Slice(CurrentSpanIndex);
        }

        /// <summary>
        /// The total number of bytes processed by the reader.
        /// </summary>
        public int ConsumedBytes { get; private set; }

        /// <summary>
        /// Peeks at the next byte value without advancing the reader.
        /// </summary>
        /// <returns>The next byte or -1 if at the end of the buffer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek() => End ? -1 : CurrentSpan[CurrentSpanIndex];

        /// <summary>
        /// Read the next byte value.
        /// </summary>
        /// <returns>The next byte or -1 if at the end of the buffer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (End)
            {
                return -1;
            }

            byte value = CurrentSpan[CurrentSpanIndex];
            CurrentSpanIndex++;
            ConsumedBytes++;

            if (CurrentSpanIndex >= CurrentSpan.Length)
            {
                GetNextSpan();
            }

            return value;
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
                while (Sequence.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
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
            End = true;
        }

        /// <summary>
        /// Move the reader ahead the specified number of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int byteCount)
        {
            if (byteCount == 0)
            {
                return;
            }

            if (byteCount < 0 || End)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            ConsumedBytes += byteCount;

            if (CurrentSpanIndex < CurrentSpan.Length - byteCount)
            {
                CurrentSpanIndex += byteCount;
            }
            else
            {
                // Current segment doesn't have enough space, scan forward through segments
                AdvanceToNextSpan(byteCount);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AdvanceToNextSpan(int byteCount)
        {
            while (!End && byteCount > 0)
            {
                if (CurrentSpanIndex < CurrentSpan.Length - byteCount)
                {
                    CurrentSpanIndex += byteCount;
                    byteCount = 0;
                    break;
                }

                int remaining = (CurrentSpan.Length - CurrentSpanIndex);

                CurrentSpanIndex += remaining;
                byteCount -= remaining;

                GetNextSpan();
            }

            if (byteCount > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        /// <summary>
        /// Peek forward the number of bytes specified by <paramref name="count"/>.
        /// </summary>
        /// <returns>Span over the peeked data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> Peek(int count)
        {
            ReadOnlySpan<byte> firstSpan = UnreadSpan;
            if (firstSpan.Length >= count)
            {
                return firstSpan.Slice(0, count);
            }

            // Not enough contiguous bytes, allocate and copy what we can get
            return PeekSlow(new byte[count]);
        }

        /// <summary>
        /// Peek forward the number of bytes in <paramref name="copyBuffer"/>, copying into
        /// <paramref name="copyBuffer"/> if needed.
        /// </summary>
        /// <param name="copyBuffer">
        /// Temporary buffer to copy into if there isn't a contiguous span within the existing data to return.
        /// Also describes the count of bytes to peek.
        /// </param>
        /// <returns>Span over the peeked data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> Peek(Span<byte> copyBuffer)
        {
            ReadOnlySpan<byte> firstSpan = UnreadSpan;
            if (firstSpan.Length >= copyBuffer.Length)
            {
                return firstSpan.Slice(0, copyBuffer.Length);
            }

            return PeekSlow(copyBuffer);
        }

        private ReadOnlySpan<byte> PeekSlow(Span<byte> destination)
        {
            ReadOnlySpan<byte> firstSpan = UnreadSpan;
            firstSpan.CopyTo(destination);
            int copied = firstSpan.Length;

            SequencePosition next = _nextPosition;
            while (Sequence.TryGet(ref next, out ReadOnlyMemory<byte> nextSegment, true))
            {
                ReadOnlySpan<byte> nextSpan = nextSegment.Span;
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
