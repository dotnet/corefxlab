﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    class MemoryList : IMemoryList<byte>
    {
        internal Memory<byte> _data;
        internal MemoryList _next;
        private long _virtualIndex;

        public MemoryList(Memory<byte> bytes)
        {
            _data = bytes;
            _next = null;
            _virtualIndex = 0;
        }

        private MemoryList(Memory<byte> bytes, long runningIndex)
        {
            _data = bytes;
            _virtualIndex = runningIndex;
        }

        public MemoryList Append(Memory<byte> bytes)
        {
            if (_next != null) throw new InvalidOperationException("Node cannot be appended");
            var node = new MemoryList(bytes, _virtualIndex + Length);
            _next = node;
            return node;
        }

        public Memory<byte> Memory => _data;

        public IMemoryList<byte> Rest => _next;

        public long Length => _data.Length;

        public long VirtualIndex => _virtualIndex;

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
                    position.SetItem(_next);
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
            if (advance) { position.SetItem(sequence._next); }
            return true;
        }
    }
}


