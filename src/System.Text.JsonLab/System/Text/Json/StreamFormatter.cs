// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Buffers;
using System.Diagnostics;

namespace System.Text.JsonLab
{
    // TODO: Can this type even be disposable (i.e. can Utf8JsonWriter be disposable)?
    // It would force everyone to call Dispose and check if their TBufferWriter was a StreamFormatter
    internal class StreamFormatter : IBufferWriter<byte>, IDisposable
    {
        private Stream _stream;
        private byte[] _buffer;

        public StreamFormatter(Stream stream)
        {
            _stream = stream;
            _buffer = ArrayPool<byte>.Shared.Rent(256);
        }

        public void Advance(int count)
        {
            Debug.Assert(count >= 0 && count <= _buffer.Length);

            _stream.Write(_buffer, 0, count);
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            Debug.Assert(sizeHint >= 0);

            if (sizeHint == 0 || _buffer.Length >= sizeHint)
                return _buffer;

            int newSize = _buffer.Length * 2;

            // If newSize overflows, the following will be true and sufficient to correct for it.
            if (newSize < sizeHint)
                newSize = sizeHint;

            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = ArrayPool<byte>.Shared.Rent(newSize);
            return _buffer;
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            Debug.Assert(sizeHint >= 0);

            if (sizeHint == 0 || _buffer.Length >= sizeHint)
                return _buffer;

            int newSize = _buffer.Length * 2;

            // If newSize overflows, the following will be true and sufficient to correct for it.
            if (newSize < sizeHint)
                newSize = sizeHint;

            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = ArrayPool<byte>.Shared.Rent(newSize);
            return _buffer;
        }

        /// <summary>
        /// Returns buffers to the pool
        /// </summary>
        public void Dispose()
        {
            // TODO: Should we write what's left in the _buffer to the stream first?
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
            _stream = null;
        }
    }
}
