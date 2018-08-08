// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader
    {
        /// <summary>
        /// Try to read everything up to the given delimiter. Will position the reader past the delimiter if found.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="movePastDelimiter">True to move past the delimiter if found.</param>
        /// <returns>True if the data was found.</returns>
        public bool TryReadUntil(out ReadOnlySpan<byte> span, byte delimiter, bool movePastDelimiter = true)
        {
            ReadOnlySpan<byte> remaining = CurrentSegmentIndex == 0 ? CurrentSegment : UnreadSegment;
            int index = remaining.IndexOf(delimiter);
            if (index != -1)
            {
                span = index == 0 ? default : remaining.Slice(0, index);
                Advance(index + (movePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadUntilSlow(out span, delimiter, remaining.Length, movePastDelimiter);
        }

        private bool TryReadUntilSlow(out ReadOnlySpan<byte> span, byte delimiter, int skip, bool movePastDelimiter)
        {
            if (!TryReadUntilInternal(out ReadOnlySequence<byte> sequence, delimiter, movePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given delimiter. Will position the reader past the delimiter if found.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="movePastDelimiter">True to move past the delimiter if found.</param>
        /// <returns>True if the data was found.</returns>
        public bool TryReadUntil(out ReadOnlySequence<byte> sequence, byte delimiter, bool movePastDelimiter = true)
        {
            return TryReadUntilInternal(out sequence, delimiter, movePastDelimiter);
        }

        public bool TryReadUntilInternal(out ReadOnlySequence<byte> sequence, byte delimiter, bool movePastDelimiter, int skip = 0)
        {
            BufferReader copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<byte> remaining = CurrentSegmentIndex == 0 ? CurrentSegment : UnreadSegment;

            while (!End)
            {
                int index = remaining.IndexOf(delimiter);
                if (index != -1)
                {
                    // Found the delimiter. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    sequence = Sequence.Slice(copy.Position, Position);
                    if (movePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSegment;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Try to read everything up to the given delimiters. Will position the reader past the delimiter if found.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="movePastDelimiter">True to move past the delimiter if found.</param>
        /// <returns>True if the data was found.</returns>
        public bool TryReadUntilAny(out ReadOnlySpan<byte> span, ReadOnlySpan<byte> delimiters, bool movePastDelimiter = true)
        {
            ReadOnlySpan<byte> remaining = UnreadSegment;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                span = remaining.Slice(0, index);
                Advance(index + (movePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadUntilAnySlow(out span, delimiters, remaining.Length, movePastDelimiter);
        }

        private bool TryReadUntilAnySlow(out ReadOnlySpan<byte> span, ReadOnlySpan<byte> delimiters, int skip, bool movePastDelimiter)
        {
            if (!TryReadUntilAnyInternal(out ReadOnlySequence<byte> sequence, delimiters, movePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given delimiters. Will position the reader past the delimiter if found.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="movePastDelimiter">True to move past the delimiter if found.</param>
        /// <returns>True if the data was found.</returns>
        public bool TryReadUntilAny(out ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> delimiters, bool movePastDelimiter = true)
        {
            return TryReadUntilAnyInternal(out sequence, delimiters, movePastDelimiter);
        }

        public bool TryReadUntilAnyInternal(out ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> delimiters, bool movePastDelimiter, int skip = 0)
        {
            BufferReader copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<byte> remaining = CurrentSegmentIndex == 0 ? CurrentSegment : UnreadSegment;

            while (!End)
            {
                int index = remaining.IndexOfAny(delimiters);
                if (index != -1)
                {
                    // Found one of the delimiters. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    sequence = Sequence.Slice(copy.Position, Position);
                    if (movePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSegment;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        public bool TryReadUntil(out ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter.Length == 0)
            {
                sequence = default;
                return true;
            }

            int matched = 0;
            BufferReader copy = this;
            SequencePosition start = Position;
            SequencePosition end = Position;
            while (!End)
            {
                if (Read() == delimiter[matched])
                {
                    matched++;
                }
                else
                {
                    end = Position;
                    matched = 0;
                }
                if (matched >= delimiter.Length)
                {
                    sequence = Sequence.Slice(start, end);
                    return true;
                }
            }
            this = copy;
            sequence = default;
            return false;
        }
    }
}
