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

        public Position First => Position.Create(_buffer.BufferStart.Segment, _buffer.BufferStart.Index);

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                position = First;
            }
            else if (position == Position.End)
            {
                item = default;
                return false;
            }

            var (segment, index) = position.Get<object>();
            var result = ReadCursorOperations.TryGetBuffer(new ReadCursor(segment, index), _buffer.End, out item, out var next);
            if (advance)
            {
                position = Position.Create(next.Segment, next.Index);
            }
            return result;
        }
    }
}
