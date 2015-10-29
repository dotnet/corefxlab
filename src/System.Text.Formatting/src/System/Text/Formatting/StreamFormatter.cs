// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Buffers;

namespace System.Text.Formatting
{
    public struct StreamFormatter : IFormatter, IDisposable
    {
        Stream _stream;
        FormattingData _formattingData;
        byte[] _buffer;

        public StreamFormatter(Stream stream) : this(stream, FormattingData.InvariantUtf16)
        {
        }

        public StreamFormatter(Stream stream, FormattingData formattingData, int bufferSize = 256)
        {
            _buffer = null;
            if (bufferSize > 0)
            {
                _buffer = BufferPool.Shared.RentBuffer(bufferSize);
            }
            _formattingData = formattingData;
            _stream = stream;
        }

        Span<byte> IFormatter.FreeBuffer
        {
            get
            {
                if (_buffer == null)
                {
                    _buffer = BufferPool.Shared.RentBuffer(256);
                }
                return new Span<byte>(_buffer);
            }
        }

        FormattingData IFormatter.FormattingData
        {
            get
            {
                return _formattingData;
            }
        }

        void IFormatter.ResizeBuffer()
        {
            BufferPool.Shared.Enlarge(ref _buffer, _buffer.Length * 2);
        }

        // ISSUE
        // I would like to lazy write to the stream, but unfortunatelly this seems to be exclusive with this type being a struct. 
        // If the write was lazy, passing this struct by value could result in data loss.
        // A stack frame could write more data to the buffer, and then when the frame pops, the infroamtion about how much was written could be lost. 
        // On the other hand, I cannot make this type a class and keep using it as it can be used today (i.e. pass streams around and create instances of this type on demand).
        // Too bad we don't support move semantics and stack only structs.
        void IFormatter.CommitBytes(int bytes)
        {
            _stream.Write(_buffer, 0, bytes);
        }

        /// <summary>
        /// Returns buffers to the pool
        /// </summary>
        public void Dispose()
        {
            BufferPool.Shared.ReturnBuffer(ref _buffer);
            _stream = null;
        }
    }
}
