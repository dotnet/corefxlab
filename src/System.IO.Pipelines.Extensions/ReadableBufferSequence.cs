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

        public Collections.Sequences.Position First => Collections.Sequences.Position.Create(_buffer.Start.Segment, _buffer.Start.Index);

        public bool TryGet(ref Collections.Sequences.Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            var p = new System.IO.Pipelines.Position(position.GetItem<object>(), position.Index);
            var result =  _buffer.TryGet(ref p, out item);
            if (advance)
            {
                position = Collections.Sequences.Position.Create(p.Segment, p.Index);
            }

            return result;
        }
    }
}
