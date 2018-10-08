// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace System.Buffers.Tests
{
    public static class BufferFactory
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
                    };

                    if (segment != null)
                    {
                        segment.Next = newSegment;
                        newSegment.RunningIndex = segment.RunningIndex + segment.Memory.Length;
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
            if (buffers.Length == 1)
                return new ReadOnlySequence<byte>(buffers[0]);
            var list = new List<Memory<byte>>();
            foreach (var buffer in buffers)
                list.Add(buffer);
            return Create(list.ToArray());
        }

        public static ReadOnlySequence<byte> CreateUtf8(params string[] buffers)
        {
            if (buffers.Length == 1)
                return new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(buffers[0]));
            var list = new List<Memory<byte>>();
            foreach (var buffer in buffers)
                list.Add(Encoding.UTF8.GetBytes(buffer));
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
