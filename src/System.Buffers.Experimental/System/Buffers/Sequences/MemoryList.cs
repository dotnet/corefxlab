// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Text;

namespace System.Buffers
{
    public class MemoryList : IMemoryList<byte>
    {
        private Memory<byte> _data;
        private MemoryList _next;
        private long _virtualIndex;

        public MemoryList(Memory<byte> bytes)
        {
            _data = bytes;
            _next = null;
            _virtualIndex = 0;
        }

        private MemoryList(Memory<byte> bytes, long virtualIndex)
        {
            _data = bytes;
            _virtualIndex = virtualIndex;
        }

        public MemoryList Append(Memory<byte> bytes)
        {
            if (_next != null) throw new InvalidOperationException("Node cannot be appended");
            var node = new MemoryList(bytes, _virtualIndex + _data.Length);
            _next = node;
            return node;
        }

        public Memory<byte> Memory => _data;

        public IMemoryList<byte> Next => _next;

        public long VirtualIndex => _virtualIndex;

        public Position First => Position.Create(this);

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
                item = _data;
                if (advance)
                {
                    position = Position.Create(_next);
                }
                return (!_data.IsEmpty || _next != null);
            }
            else if (position.IsEnd)
            {
                item = default;
                return false;
            }

            var sequence = position.GetItem<MemoryList>();
            item = sequence._data;
            if (advance) { position = Position.Create(sequence._next); }
            return true;
        }

        public static (MemoryList first, MemoryList last) Create(params byte[][] buffers)
        {
            if(buffers.Length == 0 || (buffers.Length == 1 && buffers[0].Length == 0))
            {
                var list = new MemoryList(Memory<byte>.Empty);
                return (list, list);
            }

            MemoryList first = new MemoryList(buffers[0]);
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


