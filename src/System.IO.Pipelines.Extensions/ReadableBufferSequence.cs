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

        public Position Start => _buffer.Start;

        public Position Seek(Position origin, long offset)
            => _buffer.Seek(origin, offset);

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
            => _buffer.TryGet(ref position, out item, advance);
    }
}
