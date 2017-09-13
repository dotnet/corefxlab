// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.IO.Pipelines
{
    public struct ReadableBufferSequence : ISequence<ReadOnlyMemory<byte>>
    {
        private readonly ReadableBuffer _buffer;

        public ReadableBufferSequence(ReadableBuffer buffer) : this()
        {
            _buffer = buffer;
        }

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == Position.First)
            {
                // First is already sliced
                item = _buffer.First;
                if (advance)
                {
                    if (_buffer.Start.IsEnd)
                    {
                        position = Position.AfterLast;
                    }
                    else
                    {
                        position.ObjectPosition = _buffer.Start.Segment.Next;
                        if (position.ObjectPosition == null)
                        {
                            position = Position.AfterLast;
                        }
                    }
                }
                return true;
            }
            else if (position == Position.AfterLast)
            {
                item = default;
                return false;
            }

            var currentSegment = (BufferSegment)position.ObjectPosition;
            if (advance)
            {
                position.ObjectPosition = currentSegment.Next;
                if (position.ObjectPosition == null)
                {
                    position = Position.AfterLast;
                }
            }
            if (currentSegment == _buffer.End.Segment)
            {
                item = currentSegment.Buffer.Slice(currentSegment.Start, _buffer.End.Index - currentSegment.Start);
            }
            else
            {
                item = currentSegment.Buffer.Slice(currentSegment.Start, currentSegment.End - currentSegment.Start);
            }
            return true;
        }
    }
}
