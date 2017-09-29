// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines
{
    internal class BufferSegment
    {
        /// <summary>
        /// The Start represents the offset into Array where the range of "active" bytes begins. At the point when the block is leased
        /// the Start is guaranteed to be equal to 0. The value of Start may be assigned anywhere between 0 and
        /// Buffer.Length, and must be equal to or less than End.
        /// </summary>
        public int Start;

        /// <summary>
        /// The End represents the offset into Array where the range of "active" bytes ends. At the point when the block is leased
        /// the End is guaranteed to be equal to Start. The value of Start may be assigned anywhere between 0 and
        /// Buffer.Length, and must be equal to or less than End.
        /// </summary>
        public int End;

        /// <summary>
        /// Reference to the next block of data when the overall "active" bytes spans multiple blocks. At the point when the block is
        /// leased Next is guaranteed to be null. Start, End, and Next are used together in order to create a linked-list of discontiguous
        /// working memory. The "active" memory is grown when bytes are copied in, End is increased, and Next is assigned. The "active"
        /// memory is shrunk when bytes are consumed, Start is increased, and blocks are returned to the pool.
        /// </summary>
        public BufferSegment Next;

        /// <summary>
        /// Combined length of all segments before this
        /// </summary>
        public long RunningLength;

        /// <summary>
        /// The buffer being tracked
        /// </summary>
        private OwnedMemory<byte> _owned;

        private Memory<byte> _buffer;

        public void SetMemory(OwnedMemory<byte> buffer)
        {
            SetMemory(buffer, 0, 0);
        }

        public void SetMemory(OwnedMemory<byte> buffer, int start, int end, bool readOnly = false)
        {
            _owned = buffer;
            _owned.Retain();
            _buffer = _owned.Memory;

            RunningLength = 0;
            Start = start;
            End = end;
        }

        public void ResetMemory()
        {
            _owned.Release();
            _owned = null;
        }

        public Memory<byte> Buffer => _buffer;

        /// <summary>
        /// If true, data should not be written into the backing block after the End offset. Data between start and end should never be modified
        /// since this would break cloning.
        /// </summary>
        public bool ReadOnly { get; }

        /// <summary>
        /// The amount of readable bytes in this segment. Is is the amount of bytes between Start and End.
        /// </summary>
        public int ReadableBytes => End - Start;

        /// <summary>
        /// The amount of writable bytes in this segment. It is the amount of bytes between Length and End
        /// </summary>
        public int WritableBytes => _buffer.Length - End;

        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_owned == null)
            {
                return "<NO MEMORY ATTACHED>";
            }

            var builder = new StringBuilder();
            var data = _owned.Memory.Slice(Start, ReadableBytes).Span;

            for (int i = 0; i < ReadableBytes; i++)
            {
                builder.Append((char)data[i]);
            }
            return builder.ToString();
        }

        public static BufferSegment Clone(ReadCursor beginBuffer, ReadCursor endBuffer, out BufferSegment lastSegment)
        {
            var beginOrig = beginBuffer.Segment;
            var endOrig = endBuffer.Segment;

            if (beginOrig == endOrig)
            {
                lastSegment = new BufferSegment();
                lastSegment.SetMemory(beginOrig._owned, beginBuffer.Index, endBuffer.Index);
                return lastSegment;
            }

            var beginClone = new BufferSegment();
            beginClone.SetMemory(beginOrig._owned, beginBuffer.Index, beginOrig.End);
            var endClone = beginClone;

            beginOrig = beginOrig.Next;

            while (beginOrig != endOrig)
            {
                var next = new BufferSegment();
                next.SetMemory(beginOrig._owned, beginOrig.Start, beginOrig.End);
                endClone.SetNext(next);

                endClone = endClone.Next;
                beginOrig = beginOrig.Next;
            }

            lastSegment = new BufferSegment();
            lastSegment.SetMemory(endOrig._owned, endOrig.Start, endBuffer.Index);
            endClone.SetNext(lastSegment);

            return beginClone;
        }

        public void SetNext(BufferSegment segment)
        {
            Debug.Assert(segment != null);
            Debug.Assert(Next == null);

            Next = segment;

            segment = this;
            while (segment.Next != null)
            {
                segment.Next.RunningLength = segment.RunningLength + segment.ReadableBytes;
                segment = segment.Next;
            }
        }
    }
}
