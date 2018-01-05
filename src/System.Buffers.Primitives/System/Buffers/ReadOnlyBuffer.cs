// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public readonly partial struct ReadOnlyBuffer : ISequence<ReadOnlyMemory<byte>>
    {
        internal readonly Position BufferStart;
        internal readonly Position BufferEnd;

        public static readonly ReadOnlyBuffer Empty = new ReadOnlyBuffer(new byte[0]);

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
        public bool IsSingleSpan => BufferStart.Segment == BufferEnd.Segment;

        public ReadOnlyMemory<byte> First
        {
            get
            {
                TryGetBuffer(BufferStart, BufferEnd, out var first, out _);
                return first;
            }
        }

        /// <summary>
        /// A cursor to the start of the <see cref="ReadOnlyBuffer"/>.
        /// </summary>
        public Position Start => BufferStart;

        /// <summary>
        /// A cursor to the end of the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public Position End => BufferEnd;

        public ReadOnlyBuffer(Position start, Position end)
        {
            Debug.Assert(start.Segment != null);
            Debug.Assert(end.Segment != null);

            BufferStart = start;
            BufferEnd = end;
        }

        public ReadOnlyBuffer(IMemoryList<byte> startSegment, int offset, IMemoryList<byte> endSegment, int endIndex)
        {
            Debug.Assert(startSegment != null);
            Debug.Assert(endSegment != null);
            Debug.Assert(startSegment.Memory.Length >= offset);
            Debug.Assert(endSegment.Memory.Length >= endIndex);

            BufferStart = new Position(startSegment, offset);
            BufferEnd = new Position(endSegment, endIndex);
        }

        public ReadOnlyBuffer(byte[] data) : this(data, 0, data.Length)
        {
        }

        public ReadOnlyBuffer(byte[] data, int offset, int length)
        {
            if (data == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            BufferStart = new Position(data, offset);
            BufferEnd = new Position(data, offset + length);
        }

        public ReadOnlyBuffer(OwnedMemory<byte> data, int offset, int length)
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


            BufferStart = new Position(data, offset);
            BufferEnd = new Position(data, offset + length);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadOnlyBuffer Slice(long start, long length)
        {
            var begin = Seek(BufferStart, BufferEnd, start, false);
            var end = Seek(begin, BufferEnd, length, false);
            return new ReadOnlyBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadOnlyBuffer Slice(long start, Position end)
        {
            BoundsCheck(BufferEnd, end);
            var begin = Seek(BufferStart, end, start);
            return new ReadOnlyBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="Position"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="Position"/> of the slice</param>
        public ReadOnlyBuffer Slice(Position start, Position end)
        {
            BoundsCheck(BufferEnd, end);
            BoundsCheck(end, start);

            return new ReadOnlyBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="Position"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadOnlyBuffer Slice(Position start, long length)
        {
            BoundsCheck(BufferEnd, start);

            var end = Seek(start, BufferEnd, length, false);

            return new ReadOnlyBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadOnlyBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="Position"/> at which to begin this slice.</param>
        public ReadOnlyBuffer Slice(Position start)
        {
            BoundsCheck(BufferEnd, start);

            return new ReadOnlyBuffer(start, BufferEnd);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadOnlyBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadOnlyBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadOnlyBuffer Slice(long start)
        {
            if (start == 0) return this;

            var begin = Seek(BufferStart, BufferEnd, start, false);
            return new ReadOnlyBuffer(begin, BufferEnd);
        }

        /// <summary>
        /// Copy the <see cref="ReadOnlyBuffer"/> to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{Byte}"/>.</param>
        public void CopyTo(Span<byte> destination)
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
        /// Converts the <see cref="ReadOnlyBuffer"/> to a <see cref="T:byte[]"/>
        /// </summary>
        public byte[] ToArray()
        {
            var buffer = new byte[Length];
            CopyTo(buffer);
            return buffer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var buffer in this)
            {
                SpanLiteralExtensions.AppendAsLiteral(buffer.Span, sb);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns an enumerator over the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public Position Move(Position cursor, long count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            return Seek(cursor, BufferEnd, count, false);
        }

        public bool TryGet(ref Position cursor, out ReadOnlyMemory<byte> data, bool advance = true)
        {
            var result = TryGetBuffer(cursor, End, out data, out var next);
            if (advance)
            {
                cursor = next;
            }

            return result;
        }
        
        /// <summary>
        /// An enumerator over the <see cref="ReadOnlyBuffer"/>
        /// </summary>
        public struct Enumerator
        {
            private readonly ReadOnlyBuffer _readOnlyBuffer;
            private Position _next;
            private Position _current;

            private ReadOnlyMemory<byte> _currentMemory;

            /// <summary>
            ///
            /// </summary>
            public Enumerator(ReadOnlyBuffer readOnlyBuffer)
            {
                _readOnlyBuffer = readOnlyBuffer;
                _currentMemory = default;
                _current = default;
                _next = readOnlyBuffer.Start;
            }

            /// <summary>
            /// The current <see cref="Buffer{Byte}"/>
            /// </summary>
            public ReadOnlyMemory<byte> Current
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

            public Position CreateCursor(int index)
            {
                return _readOnlyBuffer.Move(_current, index);
            }
        }
    }
}
