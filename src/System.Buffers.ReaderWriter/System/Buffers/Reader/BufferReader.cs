// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref struct BufferReader
    {
        private SequencePosition _currentSequencePosition;
        private SequencePosition _nextSequencePosition;

        private BufferReader(ReadOnlySequence<byte> buffer)
        {
            End = false;
            CurrentSegmentIndex = 0;
            ConsumedBytes = 0;
            Sequence = buffer;
            _currentSequencePosition = Sequence.Start;
            _nextSequencePosition = _currentSequencePosition;
            CurrentSegment = ReadOnlySpan<byte>.Empty;
            GetNextSegment();
        }

        /// <summary>
        /// Create a <see cref="BufferReader" over the given <see cref="ReadOnlySequence{byte}"/>./>
        /// </summary>
        public static BufferReader Create(ReadOnlySequence<byte> buffer)
        {
            return new BufferReader(buffer);
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
        public SequencePosition Position => Sequence.GetPosition(CurrentSegmentIndex, _currentSequencePosition);

        /// <summary>
        /// The current segment in the <see cref="Sequence"/>.
        /// </summary>
        public ReadOnlySpan<byte> CurrentSegment { get; private set; }

        /// <summary>
        /// The index in the <see cref="CurrentSegment"/>.
        /// </summary>
        public int CurrentSegmentIndex { get; private set; }

        /// <summary>
        /// The unread portion of the <see cref="CurrentSegment"/>.
        /// </summary>
        public ReadOnlySpan<byte> UnreadSegment => CurrentSegment.Slice(CurrentSegmentIndex);

        /// <summary>
        /// The total number of bytes processed by the reader.
        /// </summary>
        public int ConsumedBytes { get; private set; }

        /// <summary>
        /// Peeks at the next byte value without advancing the reader.
        /// </summary>
        /// <returns>The next byte or -1 if at the end of the buffer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            return End ? -1 : CurrentSegment[CurrentSegmentIndex];
        }

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

            byte value = CurrentSegment[CurrentSegmentIndex];
            CurrentSegmentIndex++;
            ConsumedBytes++;

            if (CurrentSegmentIndex >= CurrentSegment.Length)
            {
                GetNextSegment();
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GetNextSegment()
        {
            SequencePosition previous = _nextSequencePosition;
            while (Sequence.TryGet(ref _nextSequencePosition, out ReadOnlyMemory<byte> memory, advance: true))
            {
                _currentSequencePosition = previous;
                CurrentSegment = memory.Span;
                CurrentSegmentIndex = 0;
                if (CurrentSegment.Length > 0)
                {
                    return;
                }
            }
            End = true;
        }

        /// <summary>
        /// Move the reader ahead the specified number of bytes.
        /// </summary>
        public void Advance(int byteCount)
        {
            if (byteCount < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            ConsumedBytes += byteCount;

            while (!End && byteCount > 0)
            {
                if ((CurrentSegmentIndex + byteCount) < CurrentSegment.Length)
                {
                    CurrentSegmentIndex += byteCount;
                    byteCount = 0;
                    break;
                }

                int remaining = (CurrentSegment.Length - CurrentSegmentIndex);

                CurrentSegmentIndex += remaining;
                byteCount -= remaining;

                GetNextSegment();
            }

            if (byteCount > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        internal static int Peek(in BufferReader buffer, Span<byte> destination)
        {
            ReadOnlySpan<byte> firstSpan = buffer.UnreadSegment;
            if (firstSpan.Length > destination.Length)
            {
                firstSpan.Slice(0, destination.Length).CopyTo(destination);
                return destination.Length;
            }
            else if (firstSpan.Length == destination.Length)
            {
                firstSpan.CopyTo(destination);
                return destination.Length;
            }
            else
            {
                firstSpan.CopyTo(destination);
                int copied = firstSpan.Length;

                SequencePosition next = buffer._nextSequencePosition;
                while (buffer.Sequence.TryGet(ref next, out ReadOnlyMemory<byte> nextSegment, true))
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
                return copied;
            }
        }
    }
}
