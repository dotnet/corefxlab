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

        public Position First => Position.Create(_buffer.BufferStart.Index, _buffer.BufferStart.Segment);

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                // First is already sliced
                item = _buffer.First;
                if (advance)
                {
                    if (_buffer.Start.IsEnd)
                    {
                        position = Position.End;
                    }
                    else
                    {
                        position.SetItem(_buffer.Start.Segment.Next);
                    }
                }
                return true;
            }
            else if (position == Position.End)
            {
                item = default;
                return false;
            }

            var currentSegment = position.GetItem<BufferSegment>();
            if (advance)
            {
                position.SetItem(currentSegment.Next);
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
