// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace System.Buffers
{
    public class BufferList : ReadOnlySequenceSegment<byte>
    {
        public BufferList(Memory<byte> bytes)
        {
            Memory = bytes;
            Next = null;
            RunningIndex = 0;
        }

        private BufferList(Memory<byte> bytes, long virtualIndex)
        {
            Memory = bytes;
            RunningIndex = virtualIndex;
        }

        public BufferList Append(Memory<byte> bytes)
        {
            if (Next != null) throw new InvalidOperationException("Node cannot be appended");
            var node = new BufferList(bytes, RunningIndex + Memory.Length);
            Next = node;
            return node;
        }

        public static (BufferList first, BufferList last) Create(params byte[][] buffers)
        {
            if (buffers.Length == 0 || (buffers.Length == 1 && buffers[0].Length == 0))
            {
                var list = new BufferList(Memory<byte>.Empty);
                return (list, list);
            }

            BufferList first = new BufferList(buffers[0]);
            var last = first;
            for (int i = 1; i < buffers.Length; i++)
            {
                last = last.Append(buffers[i]);
            }

            return (first, last);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            bool first = true;
            for (int i = 0; i < Math.Min(5, Memory.Length); i++)
            {
                if (!first) { builder.Append(", "); }
                first = false;
                builder.Append(Memory.Span[i]);
            }
            if (Memory.Length > 5 || Next != null)
            {
                builder.Append(", ...");
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}


