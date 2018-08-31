// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public SequencePosition First => new SequencePosition(this, 0);

        public int CopyTo(Span<byte> buffer)
        {
            int copied = 0;
            SequencePosition position = default;
            var free = buffer;
            while (TryGet(ref position, out ReadOnlyMemory<byte> segment, true))
            {
                if (segment.Length > free.Length)
                {
                    segment.Span.Slice(0, free.Length).CopyTo(free);
                    copied += free.Length;
                }
                else
                {
                    segment.Span.CopyTo(free);
                    copied += segment.Length;
                }
                free = buffer.Slice(copied);
                if (free.Length == 0) break;
            }
            return copied;
        }

        public bool TryGet(ref SequencePosition position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if (position.Equals(default))
            {
                item = default;
                return false;
            }

            var (list, index) = position.Get<BufferList>();
            item = list.Memory.Slice(index);
            if (advance) { position = new SequencePosition(list.Next, 0); }
            return true;
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


