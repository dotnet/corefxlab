// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.IO.Pipelines
{
    public struct ReadableBufferSequence : ISequence<ReadOnlyMemory<byte>>
    {
        private readonly ReadOnlyBuffer _buffer;

        public ReadableBufferSequence(ReadOnlyBuffer buffer) : this()
        {
            _buffer = buffer;
        }

        public Collections.Sequences.Position First => Collections.Sequences.Position.Create(_buffer.BufferStart.Segment, _buffer.BufferStart.Index);

        public bool TryGet(ref Collections.Sequences.Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                item = default;
                return false;
            }

            var (segment, index) = position.Get<object>();
            var result = ReadCursorOperations.TryGetBuffer(new Position(segment, index), _buffer.End, out item, out var next);
            if (advance)
            {
                position = Collections.Sequences.Position.Create(next.Segment, next.Index);
            }
            return result;
        }
    }
}
