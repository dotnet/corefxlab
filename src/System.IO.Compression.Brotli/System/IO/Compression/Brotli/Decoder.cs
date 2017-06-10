// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO.Compression.Brotli.Resources;
using System.Text;

namespace System.IO.Compression
{
    internal sealed class Decoder
    {
        internal IntPtr _state = IntPtr.Zero;
        internal BrotliDecoderResult _lastDecoderResult = BrotliDecoderResult.NeedsMoreInput;
        internal MemoryStream _bufferStream;
        private bool _isDisposed = false;

        internal Decoder()
        {
            _isDisposed = false;
            InitializeDecoder();
        }

        private void InitializeDecoder()
        {
            _state = BrotliNative.BrotliDecoderCreateInstance();
            if (_state == IntPtr.Zero)
            {
                throw new System.IO.IOException(BrotliEx.DecoderInstanceCreate);//TODO Create exception
            }
            _bufferStream = new MemoryStream();
        }

        internal void Dispose()
        {
            if (!_isDisposed && _state != IntPtr.Zero)
            {
                BrotliNative.BrotliDecoderDestroyInstance(_state);
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
                _bufferStream.SetLength(_bufferStream.Length - numberOfBytes);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
