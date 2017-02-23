﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines
{
    // TODO: Pool segments
    internal class BufferSegment : IDisposable
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
        /// The buffer being tracked
        /// </summary>
        private OwnedMemory<byte> _buffer;

        private Memory<byte> _memory;

        public BufferSegment(OwnedMemory<byte> buffer)
        {
            _buffer = buffer;
            Start = 0;
            End = 0;

            _buffer.AddReference();
            _memory = _buffer.Memory;
        }

        public BufferSegment(OwnedMemory<byte> buffer, int start, int end)
        {
            _buffer = buffer;
            Start = start;
            End = end;
            ReadOnly = true;

            // For unowned buffers, we need to make a copy here so that the caller can 
            // give up the give this buffer back to the caller
            var unowned = buffer as UnownedBuffer;
            if (unowned != null)
            {
                _buffer = unowned.MakeCopy(start, end - start, out Start, out End);
            }

            _buffer.AddReference();
            _memory = _buffer.Memory;
        }

        public Memory<byte> Memory => _memory;

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

        public void Dispose()
        {
            Debug.Assert(_buffer.HasOutstandingReferences);

            _buffer.Release();

            if (!_buffer.HasOutstandingReferences)
            {
                _buffer.Dispose();
            }
        }


        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            var data = _buffer.Memory.Slice(Start, ReadableBytes).Span;

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
                lastSegment = new BufferSegment(beginOrig._buffer, beginBuffer.Index, endBuffer.Index);
                return lastSegment;
            }

            var beginClone = new BufferSegment(beginOrig._buffer, beginBuffer.Index, beginOrig.End);
            var endClone = beginClone;

            beginOrig = beginOrig.Next;

            while (beginOrig != endOrig)
            {
                endClone.Next = new BufferSegment(beginOrig._buffer, beginOrig.Start, beginOrig.End);

                endClone = endClone.Next;
                beginOrig = beginOrig.Next;
            }

            lastSegment = new BufferSegment(endOrig._buffer, endOrig.Start, endBuffer.Index);
            endClone.Next = lastSegment;

            return beginClone;
        }
    }
}
