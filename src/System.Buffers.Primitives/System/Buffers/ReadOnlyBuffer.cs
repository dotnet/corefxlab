// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public readonly partial struct ReadOnlyBuffer<T> : ISequence<ReadOnlyMemory<T>> where T : IEquatable<T>
    {
        internal readonly SequencePosition BufferStart;
        internal readonly SequencePosition BufferEnd;

        public static readonly ReadOnlyBuffer<T> Empty = new ReadOnlyBuffer<T>(new T[0]);

        /// <summary>
        /// Length of the <see cref="ReadOnlyBuffer"/> in bytes.
        /// </summary>
        public long Length => GetLength(BufferStart, BufferEnd);

        /// <summary>
        /// Determines if the <see cref="ReadOnlyBuffer"/> is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Determins if the <see cref="ReadOnlyBuffer"/> is a single <see cref="Memory{Byte}"/>.
        /// </summary>
        public bool IsSingleSegment => BufferStart.Segment == BufferEnd.Segment;

        public ReadOnlyMemory<T> First
        {
            get
            {
                TryGetBuffer(BufferStart, BufferEnd, out var first, out _);
                return first;
            }
        }

        /// <summary>
        /// A position to the start of the <see cref="ReadOnlyBuffer"/>.
        /// </summary>
        public SequencePosition Start => BufferStart;

        /// <summary>
        /// A position to the end of the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public SequencePosition End => BufferEnd;

        private ReadOnlyBuffer(SequencePosition start, SequencePosition end)
        {
            Debug.Assert(start.Segment != null);
            Debug.Assert(end.Segment != null);

            BufferStart = start;
            BufferEnd = end;
        }

        public ReadOnlyBuffer(IBufferList<T> startSegment, int offset, IBufferList<T> endSegment, int endIndex)
        {
            Debug.Assert(startSegment != null);
            Debug.Assert(endSegment != null);
            Debug.Assert(startSegment.Memory.Length >= offset);
            Debug.Assert(endSegment.Memory.Length >= endIndex);

            BufferStart = new SequencePosition(startSegment, offset);
            BufferEnd = new SequencePosition(endSegment, endIndex);
        }

        public ReadOnlyBuffer(T[] data) : this(data, 0, data.Length)
        {
        }

        public ReadOnlyBuffer(T[] data, int offset, int length)
        {
            if (data == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            BufferStart = new SequencePosition(data, offset);
            BufferEnd = new SequencePosition(data, offset + length);
        }

        public ReadOnlyBuffer(OwnedMemory<T> data, int offset, int length)
        {

            if (data == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            if (offset < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset);
            }

            if (length < 0 || length > data.Length - offset)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }


            BufferStart = new SequencePosition(data, offset);
            BufferEnd = new SequencePosition(data, offset + length);
        }

        public ReadOnlyBuffer(IEnumerable<Memory<T>> buffers)
        {
            ReadOnlyBufferSegment segment = null;
            ReadOnlyBufferSegment first = null;
            foreach (var buffer in buffers)
            {
                var newSegment = new ReadOnlyBufferSegment()
                {
                    Memory = buffer,
                    RunningIndex = segment?.RunningIndex ?? 0
                };

                if (segment != null)
                {
                    segment.Next = newSegment;
                }
                else
                {
                    first = newSegment;
                }

                segment = newSegment;
            }

            if (first == null)
            {
                first = segment = new ReadOnlyBufferSegment();
            }

            BufferStart = new SequencePosition(first, 0);
            BufferEnd = new SequencePosition(segment, segment.Memory.Length);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadOnlyBuffer<T> Slice(long start, long length)
        {
            var begin = Seek(BufferStart, BufferEnd, start, false);
            var end = Seek(begin, BufferEnd, length, false);
            return new ReadOnlyBuffer<T>(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadOnlyBuffer<T> Slice(long start, SequencePosition end)
        {
            BoundsCheck(BufferEnd, end);
            var begin = Seek(BufferStart, end, start);
            return new ReadOnlyBuffer<T>(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="SequencePosition"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="SequencePosition"/> of the slice</param>
        public ReadOnlyBuffer<T> Slice(SequencePosition start, SequencePosition end)
        {
            BoundsCheck(BufferEnd, end);
            BoundsCheck(end, start);

            return new ReadOnlyBuffer<T>(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="SequencePosition"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadOnlyBuffer<T> Slice(SequencePosition start, long length)
        {
            BoundsCheck(BufferEnd, start);

            var end = Seek(start, BufferEnd, length, false);

            return new ReadOnlyBuffer<T>(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadOnlyBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="SequencePosition"/> at which to begin this slice.</param>
        public ReadOnlyBuffer<T> Slice(SequencePosition start)
        {
            BoundsCheck(BufferEnd, start);

            return new ReadOnlyBuffer<T>(start, BufferEnd);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadOnlyBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadOnlyBuffer<T> Slice(long start)
        {
            if (start == 0) return this;

            var begin = Seek(BufferStart, BufferEnd, start, false);
            return new ReadOnlyBuffer<T>(begin, BufferEnd);
        }

        /// <summary>
        /// Copy the <see cref="ReadOnlyBuffer"/> to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{Byte}"/>.</param>
        public void CopyTo(Span<T> destination)
        {
            if (Length > destination.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.destination);
            }

            foreach (var buffer in this)
            {
                buffer.Span.CopyTo(destination);
                destination = destination.Slice(buffer.Length);
            }
        }

        /// <summary>
        /// Converts the <see cref="ReadOnlyBuffer"/> to a <see cref="T[]"/>
        /// </summary>
        public T[] ToArray()
        {
            var buffer = new T[Length];
            CopyTo(buffer);
            return buffer;
        }

        public override string ToString()
        {
            if (typeof(T) == typeof(byte))
            {
                if (this is ReadOnlyBuffer<byte> bytes)
                {
                    var sb = new StringBuilder();
                    foreach (var buffer in bytes)
                    {
                        SpanLiteralExtensions.AppendAsLiteral(buffer.Span, sb);
                    }
                    return sb.ToString();
                }
            }
            return base.ToString();
        }

        /// <summary>
        /// Returns an enumerator over the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public SequencePosition Seek(SequencePosition origin, long offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            return Seek(origin, BufferEnd, offset, false);
        }

        public bool TryGet(ref SequencePosition position, out ReadOnlyMemory<T> data, bool advance = true)
        {
            var result = TryGetBuffer(position, End, out data, out var next);
            if (advance)
            {
                position = next;
            }

            return result;
        }

        public SequencePosition? PositionOf(T value)
        {
            SequencePosition position = Start;
            SequencePosition result = position;
            while (TryGet(ref position, out var memory))
            {
                var index = memory.Span.IndexOf(value);
                if (index != -1)
                {
                    return Seek(result, index);
                }
                result = position;
            }
            return null;
        }

        /// <summary>
        /// An enumerator over the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public struct Enumerator
        {
            private readonly ReadOnlyBuffer<T> _readOnlyBuffer;
            private SequencePosition _next;
            private SequencePosition _current;

            private ReadOnlyMemory<T> _currentMemory;

            /// <summary>
            ///
            /// </summary>
            public Enumerator(ReadOnlyBuffer<T> readOnlyBuffer)
            {
                _readOnlyBuffer = readOnlyBuffer;
                _currentMemory = default;
                _current = default;
                _next = readOnlyBuffer.Start;
            }

            /// <summary>
            /// The current <see cref="Buffer{Byte}"/>
            /// </summary>
            public ReadOnlyMemory<T> Current
            {
                get => _currentMemory;
                set => _currentMemory = value;
            }

            /// <summary>
            /// Moves to the next <see cref="Buffer{Byte}"/> in the <see cref="ReadOnlyBuffer"/>
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (_next.Segment == null)
                {
                    return false;
                }

                _current = _next;

                return _readOnlyBuffer.TryGet(ref _next, out _currentMemory);
            }

            public Enumerator GetEnumerator()
            {
                return this;
            }

            public void Reset()
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }
    }
}
