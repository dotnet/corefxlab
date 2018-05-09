// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text;

namespace System.Buffers.Tests
{
    internal static class BufferFactory
    {
        private class ReadOnlyBufferSegment : ReadOnlySequenceSegment<byte>
        {

            public static ReadOnlySequence<byte> Create(IEnumerable<Memory<byte>> buffers)
            {
                ReadOnlyBufferSegment segment = null;
                ReadOnlyBufferSegment first = null;
                foreach (var buffer in buffers)
                {
                    var newSegment = new ReadOnlyBufferSegment()
                    {
                        Memory = buffer,
                        RunningIndex = segment?.Memory.Length ?? 0
                    };

                    if (segment != null)
                    {
                        segment.Next = newSegment;
                    }
                    else
                    {
                        first = newSegment;
                    }

                    segment = newSegment;
                }

                if (first == null)
                {
                    first = segment = new ReadOnlyBufferSegment();
                }

                return new ReadOnlySequence<byte>(first, 0, segment, segment.Memory.Length);
            }
        }

        public static ReadOnlySequence<byte> Create(params byte[][] buffers)
        {
            if (buffers.Length == 1) return new ReadOnlySequence<byte>(buffers[0]);
            var list = new List<Memory<byte>>();
            foreach (var b in buffers) list.Add(b);
            return Create(list.ToArray());
        }

        public static ReadOnlySequence<byte> Create(IEnumerable<Memory<byte>> buffers)
        {
            return ReadOnlyBufferSegment.Create(buffers);
        }

        public static ReadOnlySequence<byte> Parse(string text)
        {
            var segments = text.Split('|');
            var buffers = new List<Memory<byte>>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return Create(buffers.ToArray());
        }
    }
}
