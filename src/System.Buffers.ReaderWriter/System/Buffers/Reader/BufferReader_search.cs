// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader<T> where T : unmanaged, IEquatable<T>
    {
        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public bool TryReadTo(out ReadOnlySpan<T> span, T delimiter, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = MemoryExtensions.IndexOf(remaining, delimiter);
            if (index != -1)
            {
                span = index == 0 ? default : remaining.Slice(0, index);
                Advance(index + (advancePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadToSlow(out span, delimiter, remaining.Length, advancePastDelimiter);
        }

        private bool TryReadToSlow(out ReadOnlySpan<T> span, T delimiter, int skip, bool advancePastDelimiter)
        {
            if (!TryReadToInternal(out ReadOnlySequence<T> sequence, delimiter, advancePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiter"/>.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiter">The delimiter to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public bool TryReadTo(out ReadOnlySequence<T> sequence, T delimiter, bool advancePastDelimiter = true)
        {
            return TryReadToInternal(out sequence, delimiter, advancePastDelimiter);
        }

        public bool TryReadToInternal(out ReadOnlySequence<T> sequence, T delimiter, bool advancePastDelimiter, int skip = 0)
        {
            BufferReader<T> copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<T> remaining = UnreadSpan;

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
                    if (advancePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSpan;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiters"/>.
        /// </summary>
        /// <param name="span">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the the <paramref name="delimiters"/> were found.</returns>
        public bool TryReadToAny(out ReadOnlySpan<T> span, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                span = remaining.Slice(0, index);
                Advance(index + (advancePastDelimiter ? 1 : 0));
                return true;
            }

            return TryReadToAnySlow(out span, delimiters, remaining.Length, advancePastDelimiter);
        }

        private bool TryReadToAnySlow(out ReadOnlySpan<T> span, ReadOnlySpan<T> delimiters, int skip, bool advancePastDelimiter)
        {
            if (!TryReadToAnyInternal(out ReadOnlySequence<T> sequence, delimiters, advancePastDelimiter, skip))
            {
                span = default;
                return false;
            }

            span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
            return true;
        }

        /// <summary>
        /// Try to read everything up to the given <paramref name="delimiters"/>.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiters">The delimiters to look for.</param>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the the <paramref name="delimiters"/> were found.</returns>
        public bool TryReadToAny(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            return TryReadToAnyInternal(out sequence, delimiters, advancePastDelimiter);
        }

        private bool TryReadToAnyInternal(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiters, bool advancePastDelimiter, int skip = 0)
        {
            BufferReader<T> copy = this;
            if (skip > 0)
                Advance(skip);
            ReadOnlySpan<T> remaining = CurrentSpanIndex == 0 ? CurrentSpan : UnreadSpan;

            while (!End)
            {
                int index = delimiters.Length == 2
                    ? remaining.IndexOfAny(delimiters[0], delimiters[1])
                    : remaining.IndexOfAny(delimiters);

                if (index != -1)
                {
                    // Found one of the delimiters. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    sequence = Sequence.Slice(copy.Position, Position);
                    if (advancePastDelimiter)
                    {
                        Advance(1);
                    }
                    return true;
                }

                Advance(remaining.Length);
                remaining = CurrentSpan;
            }

            // Didn't find anything, reset our original state.
            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Try to read data until the given <paramref name="delimiter"/> sequence.
        /// </summary>
        /// <param name="sequence">The read data, if any.</param>
        /// <param name="delimiter">The multi (T) delimiter.</param>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> sequence if found.</param>
        /// <returns>True if the <paramref name="delimiter"/> was found.</returns>
        public unsafe bool TryReadTo(out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiter, bool advancePastDelimiter = true)
        {
            if (delimiter.Length == 0)
            {
                sequence = default;
                return true;
            }

            BufferReader<T> copy = this;

            Span<T> peekBuffer;
            if (delimiter.Length * sizeof(T) < 512)
            {
                T* t = stackalloc T[delimiter.Length];
                peekBuffer = new Span<T>(t, delimiter.Length);
            }
            else
            {
                peekBuffer = new Span<T>(new T[delimiter.Length]);
            }

            while (!End)
            {
                if (!TryReadTo(out sequence, delimiter[0], advancePastDelimiter: false))
                {
                    this = copy;
                    return false;
                }

                if (delimiter.Length == 1)
                {
                    return true;
                }

                ReadOnlySpan<T> next = Peek(peekBuffer);
                if (next.SequenceEqual(delimiter))
                {
                    if (advancePastDelimiter)
                    {
                        Advance(delimiter.Length);
                    }
                    return true;
                }
                else
                {
                    Advance(1);
                }
            }

            this = copy;
            sequence = default;
            return false;
        }

        /// <summary>
        /// Skip until the given <paramref name="delimiter"/>, if found.
        /// </summary>
        /// <param name="advancePastDelimiter">True to move past the <paramref name="delimiter"/> if found.</param>
        /// <returns>True if the given <paramref name="delimiter"/> was found.</returns>
        public bool TrySkipTo(T delimiter, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOf(delimiter);
            if (index != -1)
            {
                Advance(index);
                return true;
            }

            return TryReadToInternal(out _, delimiter, advancePastDelimiter);
        }

        /// <summary>
        /// Skip until any of the given <paramref name="delimiters"/>, if found.
        /// </summary>
        /// <param name="advancePastDelimiter">True to move past the first found instance of any of the given <paramref name="delimiters"/>.</param>
        /// <returns>True if any of the given <paramref name="delimiters"/> was found.</returns>
        public bool TrySkipToAny(ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            ReadOnlySpan<T> remaining = UnreadSpan;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                Advance(index);
                return true;
            }

            return TryReadToAnyInternal(out _, delimiters, advancePastDelimiter);
        }

        /// <summary>
        /// Skip consecutive instances of the given <paramref name="value"/>.
        /// </summary>
        /// <returns>True if any Ts were skipped.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SkipPast(T value)
        {
            int start = Consumed;
            ReadOnlySpan<T> unread = UnreadSpan;
            int i = 0;
            for (; i < unread.Length; i++)
            {
                T val = unread[i];
                if (!val.Equals(value))
                {
                    break;
                }
            }
            Advance(i);
            if (i == unread.Length)
            {
                SkipPastSlow(value);
            }

            return start != Consumed;
        }

        /// <summary>
        /// Skip consecutive instances of any of the given <paramref name="values"/>.
        /// </summary>
        /// <returns>True if any Ts were skipped.</returns>
        public bool SkipPastAny(ReadOnlySpan<T> values)
        {
            int start = Consumed;
            ReadOnlySpan<T> unread = UnreadSpan;
            int i = 0;
            for (; i < unread.Length; i++)
            {
                T val = unread[i];
                if (values.IndexOf(val) == -1)
                {
                    break;
                }
            }
            Advance(i);
            if (i == unread.Length)
            {
                SkipPastAnySlow(values);
            }

            return start != Consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SkipPastAny(T value0, T value1, T value2, T value3)
        {
            int start = Consumed;
            ReadOnlySpan<T> unread = UnreadSpan;
            int i = 0;
            for (; i < unread.Length; i++)
            {
                T val = unread[i];
                if (!val.Equals(value0) && !val.Equals(value1) && !val.Equals(value2) && !val.Equals(value3))
                {
                    break;
                }
            }
            Advance(i);
            if (i == unread.Length)
            {
                SkipPastAnySlow(value0, value1, value2, value3);
            }

            return start != Consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SkipPastAny(T value0, T value1, T value2)
        {
            int start = Consumed;
            ReadOnlySpan<T> unread = UnreadSpan;
            int i = 0;
            for (; i < unread.Length; i++)
            {
                T val = unread[i];
                if (!val.Equals(value0) && !val.Equals(value1) && !val.Equals(value2))
                {
                    break;
                }
            }
            Advance(i);
            if (i == unread.Length)
            {
                SkipPastAnySlow(value0, value1, value2);
            }

            return start != Consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SkipPastAny(T value0, T value1)
        {
            int start = Consumed;
            ReadOnlySpan<T> unread = UnreadSpan;
            int i = 0;
            for (; i < unread.Length; i++)
            {
                T val = unread[i];
                if (!val.Equals(value0) && !val.Equals(value1))
                {
                    break;
                }
            }
            Advance(i);
            if (i == unread.Length)
            {
                SkipPastAnySlow(value0, value1);
            }

            return start != Consumed;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SkipPastSlow(T value)
        {
            while (!End)
            {
                T val = CurrentSpan[CurrentSpanIndex];
                if (!val.Equals(value))
                {
                    break;
                }
                Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SkipPastAnySlow(T value0, T value1)
        {
            while (!End)
            {
                T val = CurrentSpan[CurrentSpanIndex];
                if (!val.Equals(value0) && !val.Equals(value1))
                {
                    break;
                }
                Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SkipPastAnySlow(T value0, T value1, T value2)
        {
            while (!End)
            {
                T val = CurrentSpan[CurrentSpanIndex];
                if (!val.Equals(value0) && !val.Equals(value1) && !val.Equals(value2))
                {
                    break;
                }
                Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SkipPastAnySlow(T value0, T value1, T value2, T value3)
        {
            while (!End)
            {
                T val = CurrentSpan[CurrentSpanIndex];
                if (!val.Equals(value0) && !val.Equals(value1) && !val.Equals(value2) && !val.Equals(value3))
                {
                    break;
                }
                Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SkipPastAnySlow(ReadOnlySpan<T> values)
        {
            while (!End)
            {
                T val = CurrentSpan[CurrentSpanIndex];
                if (values.IndexOf(val) == -1)
                {
                    break;
                }
                Advance(1);
            }
        }

        /// <summary>
        /// Check to see if the given <paramref name="next"/> values are next.
        /// </summary>
        /// <param name="advancePast">Move past the <paramref name="next"/> values if found.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNext(ReadOnlySpan<T> next, bool advancePast)
        {
            ReadOnlySpan<T> unread = UnreadSpan;
            if (unread.Length >= next.Length)
            {
                if (unread.StartsWith(next))
                {
                    if (advancePast)
                    {
                        Advance(next.Length);
                    }
                    return true;
                }
                return false;
            }
            return IsNextSlow(next, advancePast);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe bool IsNextSlow(ReadOnlySpan<T> next, bool advancePast)
        {
            // Call PeekSlow directly since we know there isn't enough space.
            Debug.Assert(UnreadSpan.Length < next.Length);

            T* t = stackalloc T[next.Length];
            ReadOnlySpan<T> peek = PeekSlow(new Span<T>(t, next.Length));

            if (next.SequenceEqual(peek))
            {
                if (advancePast)
                {
                    Advance(next.Length);
                }
                return true;
            }

            return false;
        }
    }
}
