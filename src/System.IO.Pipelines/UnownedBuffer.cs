// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that is owned by an external component.
    /// </summary>
    public class UnownedBuffer : OwnedBuffer<byte>
    {
        private ArraySegment<byte> _buffer;

        public UnownedBuffer(ArraySegment<byte> buffer) : base(buffer.Array, buffer.Offset, buffer.Count)
        {
            _buffer = buffer;
        }

        public OwnedBuffer<byte> MakeCopy(int offset, int length, out int newStart, out int newEnd)
        {
            // Copy to a new Owned Buffer.
            var buffer = new byte[length];
            System.Buffer.BlockCopy(_buffer.Array, _buffer.Offset + offset, buffer, 0, length);
            newStart = 0;
            newEnd = length;
            return buffer;
        }
    }
}
