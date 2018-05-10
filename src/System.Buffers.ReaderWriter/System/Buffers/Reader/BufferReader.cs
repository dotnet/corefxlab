// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref struct BufferReader
    {
        private SequencePosition _currentSequencePosition;
        private SequencePosition _nextSequencePosition;

        public BufferReader(ReadOnlySequence<byte> buffer)
        {
            End = false;
            CurrentSegmentIndex = 0;
            ConsumedBytes = 0;
            Sequence = buffer;
            _currentSequencePosition = Sequence.Start;
            _nextSequencePosition = _currentSequencePosition;
            CurrentSegment = ReadOnlySpan<byte>.Empty;
            MoveNext();
        }

        public static BufferReader Create(ReadOnlySequence<byte> buffer)
        {
            return new BufferReader(buffer);
        }

        public bool End { get; private set; }
        public int CurrentSegmentIndex { get; private set; }

        public ReadOnlySequence<byte> Sequence { get; }

        public SequencePosition Position => Sequence.GetPosition(CurrentSegmentIndex, _currentSequencePosition);

        public ReadOnlySpan<byte> CurrentSegment { get; private set; }

        public ReadOnlySpan<byte> UnreadSegment => CurrentSegment.Slice(CurrentSegmentIndex);

        public int ConsumedBytes { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (End)
            {
                return -1;
            }
            return CurrentSegment[CurrentSegmentIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (End)
            {
                return -1;
            }

            var value = CurrentSegment[CurrentSegmentIndex];
            CurrentSegmentIndex++;
            ConsumedBytes++;

            if (CurrentSegmentIndex >= CurrentSegment.Length)
            {
                MoveNext();
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void MoveNext()
        {
            var previous = _nextSequencePosition;
            while (Sequence.TryGet(ref _nextSequencePosition, out var memory, true))
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

                var remaining = (CurrentSegment.Length - CurrentSegmentIndex);

                CurrentSegmentIndex += remaining;
                byteCount -= remaining;

                MoveNext();
            }

            if (byteCount > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }
        }

        internal static int Peek(BufferReader bytes, Span<byte> destination)
        {
            var first = bytes.UnreadSegment;
            if (first.Length > destination.Length)
            {
                first.Slice(0, destination.Length).CopyTo(destination);
                return destination.Length;
            }
            else if (first.Length == destination.Length)
            {
                first.CopyTo(destination);
                return destination.Length;
            }
            else
            {
                first.CopyTo(destination);
                int copied = first.Length;

                var next = bytes._nextSequencePosition;
                while (bytes.Sequence.TryGet(ref next, out ReadOnlyMemory<byte> nextSegment, true))
                {
                    var nextSpan = nextSegment.Span;
                    if (nextSpan.Length > 0)
                    {
                        var toCopy = Math.Min(nextSpan.Length, destination.Length - copied);
                        nextSpan.Slice(0, toCopy).CopyTo(destination.Slice(copied));
                        copied += toCopy;
                        if (copied >= destination.Length) break;
                    }
                }
                return copied;
            }
        }
    }
}
