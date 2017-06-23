// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace System.IO.Pipelines.Networking.Sockets.Internal
{
    /// <summary>
    /// Simple pool over a byte[] that returns segments, using a queue to
    /// handle recycling of segments.
    /// </summary>
    internal class MicroBufferPool
    {
        private readonly byte[] _buffer;
        private readonly Queue<ushort> _recycled;
        private ushort _next;
        private readonly ushort _count;
        private readonly int _bytesPerItem;

        public int BytesPerItem => _bytesPerItem;

        public int Available
        {
            get
            {
                lock (_recycled)
                {
                    return (_count - _next) + _recycled.Count;
                }
            }
        }

        public int InUse
        {
            get
            {
                lock (_recycled)
                {
                    return _next - _recycled.Count;
                }
            }
        }

        public MicroBufferPool(int bytesPerItem, int count)
        {
            if (count <= 0 || count > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (bytesPerItem <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytesPerItem));
            }
            _buffer = new byte[(ulong)bytesPerItem * (ulong)count];
            _next = 0;
            _count = (ushort)count;
            _bytesPerItem = bytesPerItem;
            _recycled = new Queue<ushort>();
        }

        public bool TryTake(out ArraySegment<byte> segment)
        {
            int index;
            lock (_recycled)
            {
                if (_recycled.Count != 0)
                {
                    index = _recycled.Dequeue();
                }
                else if (_next < _count)
                {
                    index = _next++;
                }
                else
                {
                    segment = default;
                    return false;
                }
            }

            segment = new ArraySegment<byte>(_buffer, index * _bytesPerItem, _bytesPerItem);
            return true;
        }

        public void Recycle(ArraySegment<byte> segment)
        {
            // only put it back if it is a buffer we might have issued -
            // needs same array and count, aligned by count, and not out-of-range
            int index;
            if (segment.Array == _buffer && segment.Count == _bytesPerItem
                && (segment.Offset % _bytesPerItem) == 0
                && (index = segment.Offset / _bytesPerItem) >= 0
                && index < _count)
            {
                lock (_recycled)
                {
                    _recycled.Enqueue((ushort)index);
                }
            }
        }
    }
}
