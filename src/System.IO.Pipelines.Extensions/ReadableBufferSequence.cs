// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
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

        public Position Start => new Position(_buffer.Start.Segment, _buffer.Start.Index);

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            var (data, index) = position.Get<object>();
            var p = new Position(data, index);
            var result =  _buffer.TryGet(ref p, out item);
            if (advance)
            {
                position = new Position(p.Segment, p.Index);
            }

            return result;
        }
    }
}
