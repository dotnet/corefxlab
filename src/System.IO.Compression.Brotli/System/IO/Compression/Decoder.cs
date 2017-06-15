// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO.Compression.Resources;
using System.Text;

namespace System.IO.Compression
{
    internal sealed class Decoder
    {
        internal IntPtr State { get; private set; }
        internal BrotliDecoderResult LastDecoderResult { get; set; }
        internal Stream BufferStream => _bufferStream;
        private MemoryStream _bufferStream;
        private bool _isDisposed = false;

        internal Decoder()
        {
            _isDisposed = false;
            InitializeDecoder();
        }

        private void InitializeDecoder()
        {
            State = BrotliNative.BrotliDecoderCreateInstance();
            if (State == IntPtr.Zero)
            {
                throw new System.IO.IOException(BrotliEx.DecoderInstanceCreate);
            }
            LastDecoderResult = BrotliDecoderResult.NeedsMoreInput;
            _bufferStream = new MemoryStream();
        }

        internal void Dispose()
        {
            if (!_isDisposed && State != IntPtr.Zero)
            {
                BrotliNative.BrotliDecoderDestroyInstance(State);
                _bufferStream.Dispose();
            }
            _isDisposed = true;
        }

        internal void RemoveBytes(int numberOfBytes)
        {
            ArraySegment<byte> buf;
            if (_bufferStream.TryGetBuffer(out buf))
            {
                Buffer.BlockCopy(buf.Array, numberOfBytes, buf.Array, 0, (int)_bufferStream.Length - numberOfBytes);
                _bufferStream.SetLength(BufferStream.Length - numberOfBytes);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
