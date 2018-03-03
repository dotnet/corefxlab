// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;

namespace Microsoft.Net
{
    class BufferSequence : ReadOnlySequenceSegment<byte>, IDisposable
    {
        public const int DefaultBufferSize = 1024;

        byte[] _array;
        int _written;

        public BufferSequence(int desiredSize = DefaultBufferSize)
        {
            _array = ArrayPool<byte>.Shared.Rent(desiredSize);
            Memory = new ReadOnlyMemory<byte>(_array, 0, 0);
        }

        public Span<byte> Free => new Span<byte>(_array, _written, _array.Length - _written);

        public BufferSequence Append(int desiredSize = DefaultBufferSize)
        {
            var next = new BufferSequence(desiredSize)
            {
                RunningIndex = RunningIndex + Memory.Length
            };
            Next = next;
            return next;
        }

        public void Advance(int bytes)
        {
            _written += bytes;
            Memory = new ReadOnlyMemory<byte>(_array, 0, _written);
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            var array = _array;
            _array = null;
            if (array != null) ArrayPool<byte>.Shared.Return(array);
            if (Next != null)
            {
                ((BufferSequence)Next).Dispose();
            }
            Next = null;
        }
    }
}
