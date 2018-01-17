// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Text;

namespace System.Buffers
{
    public class BufferList : IBufferList<byte>
    {
        private Memory<byte> _data;
        private BufferList _next;
        private long _virtualIndex;

        public BufferList(Memory<byte> bytes)
        {
            _data = bytes;
            _next = null;
            _virtualIndex = 0;
        }

        private BufferList(Memory<byte> bytes, long virtualIndex)
        {
            _data = bytes;
            _virtualIndex = virtualIndex;
        }

        public BufferList Append(Memory<byte> bytes)
        {
            if (_next != null) throw new InvalidOperationException("Node cannot be appended");
            var node = new BufferList(bytes, _virtualIndex + _data.Length);
            _next = node;
            return node;
        }

        public Memory<byte> Memory => _data;

        public IBufferList<byte> Next => _next;

        public long RunningLength => _virtualIndex;

        public Position First => new Position(this, 0);

        public int CopyTo(Span<byte> buffer)
        {
            int copied = 0;
            Position position = default;
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

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            var result = TryGet(ref position, out Memory<byte> memory, advance);
            item = memory;
            return result;
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            if (position == default)
            {
                item = default;
                return false;
            }

            var (list, index) = position.Get<BufferList>();
            item = list._data.Slice(index);
            if (advance) { position = new Position(list._next, 0); }
            return true;
        }

        public static (BufferList first, BufferList last) Create(params byte[][] buffers)
        {
            if(buffers.Length == 0 || (buffers.Length == 1 && buffers[0].Length == 0))
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
            for (int i = 0; i < Math.Min(5, _data.Length); i++)
            {
                if (!first) { builder.Append(", "); }
                first = false;
                builder.Append(_data.Span[i]);
            }
            if(_data.Length > 5 || _next != null)
            {
                builder.Append(", ...");
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}


