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
        internal IntPtr State { get; private set; }
        internal BrotliDecoderResult LastDecoderResult { get; set; }
        internal MemoryStream BufferStream { get; private set; }
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
            BufferStream = new MemoryStream();
        }

        internal void Dispose()
        {
            if (!_isDisposed && State != IntPtr.Zero)
            {
                BrotliNative.BrotliDecoderDestroyInstance(State);
                BufferStream.Dispose();
            }
            _isDisposed = true;
        }

        internal void RemoveBytes(int numberOfBytes)
        {
            ArraySegment<byte> buf;
            if (BufferStream.TryGetBuffer(out buf))
            {
                Buffer.BlockCopy(buf.Array, numberOfBytes, buf.Array, 0, (int)BufferStream.Length - numberOfBytes);
                BufferStream.SetLength(BufferStream.Length - numberOfBytes);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
