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
        /// The buffer being tracked if segment owns the memory
        /// </summary>
        private OwnedMemory<byte> _ownedMemory;

        private Memory<byte> _memory;

        public void SetMemory(OwnedMemory<byte> buffer)
        {
            SetMemory(buffer, 0, 0);
        }

        public void SetMemory(OwnedMemory<byte> ownedMemory, int start, int end, bool readOnly = false)
        {
            _ownedMemory = ownedMemory;
            _ownedMemory.Retain();

            _memory = _ownedMemory.Memory;

            ReadOnly = readOnly;
            RunningLength = 0;
            Start = start;
            End = end;
            Next = null;
        }

        public void ResetMemory()
        {
            _ownedMemory.Release();
            _ownedMemory = null;
            _memory = default;
        }

        public Memory<byte> Memory => _memory;

        /// <summary>
        /// If true, data should not be written into the backing block after the End offset. Data between start and end should never be modified
        /// since this would break cloning.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// The amount of readable bytes in this segment. Is is the amount of bytes between Start and End.
        /// </summary>
        public int ReadableBytes => End - Start;

        /// <summary>
        /// The amount of writable bytes in this segment. It is the amount of bytes between Length and End
        /// </summary>
        public int WritableBytes => _memory.Length - End;

        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_memory.IsEmpty)
            {
                return "<EMPTY>";
            }

            var builder = new StringBuilder();
            var data = _memory.Slice(Start, ReadableBytes).Span;

            SpanLiteralExtensions.AppendAsLiteral(data, builder);
            return builder.ToString();
        }

        public static BufferSegment Clone(BufferSegment start, int startIndex, BufferSegment end, int endIndex, out BufferSegment lastSegment)
        {
            var beginOrig = start;
            var endOrig = end;

            if (beginOrig == endOrig)
            {
                lastSegment = new BufferSegment();
                lastSegment.SetMemory(beginOrig._ownedMemory, startIndex, endIndex);
                return lastSegment;
            }

            var beginClone = new BufferSegment();
            beginClone.SetMemory(beginOrig._ownedMemory, startIndex, beginOrig.End);
            var endClone = beginClone;

            beginOrig = beginOrig.Next;

            while (beginOrig != endOrig)
            {
                var next = new BufferSegment();
                next.SetMemory(beginOrig._ownedMemory, beginOrig.Start, beginOrig.End);
                endClone.SetNext(next);

                endClone = endClone.Next;
                beginOrig = beginOrig.Next;
            }

            lastSegment = new BufferSegment();
            lastSegment.SetMemory(endOrig._ownedMemory, endOrig.Start, endIndex);
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
