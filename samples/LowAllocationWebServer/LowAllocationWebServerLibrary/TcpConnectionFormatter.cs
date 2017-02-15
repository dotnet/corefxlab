// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Text;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;

namespace Microsoft.Net.Http
{
    public class TcpConnectionFormatter : ITextOutput, IDisposable
    {
        static byte[] s_terminator = new Utf8String("0\r\n\r\n").Bytes.ToArray();
        const int ChunkPrefixSize = 10;

        TcpConnection _connection;
        int _written;
        byte[] _buffer;
        bool _headerSent;

        public TcpConnectionFormatter(TcpConnection connection, int bufferSize = 4096)
        {
            _connection = connection;
            _buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        }

        public TextEncoder Encoding => TextEncoder.Utf8;

        public Span<byte> Buffer {
            get {
                var buffer = _buffer.Slice(ChunkPrefixSize + _written);
                if (buffer.Length > 2) return buffer.Slice(0, buffer.Length - 2);
                return Span<byte>.Empty;
            }
        }

        public void Advance(int bytes)
        {
            _written += bytes;
            if (_written >= _buffer.Length) throw new InvalidOperationException();
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
            if (_written < 1) throw new NotImplementedException();
            Send();
            _written = 0;
        }

        private void Send()
        {
            ReadOnlySpan<byte> toSend;
            // if send headers
            if (!_headerSent)
            {
                toSend = _buffer.Slice(ChunkPrefixSize, _written);
                _written = 0;
                _headerSent = true;
            }
            else
            {
                var chunkPrefixBuffer = _buffer.Slice(0, ChunkPrefixSize);
                var prefixLength = WriteChunkPrefix(chunkPrefixBuffer, _written);

                _buffer[ChunkPrefixSize + _written++] = 13;
                _buffer[ChunkPrefixSize + _written++] = 10;

                toSend = _buffer.Slice(ChunkPrefixSize - prefixLength, _written + prefixLength);
                _written = 0;
            }

            var array = toSend.ToArray();
            Console.WriteLine(new Utf8String(toSend).ToString());
            _connection.Send(toSend);
        }

        void Finish()
        {
            if (_written > 0)
            {
                Send();
                _connection.Send((ReadOnlySpan<byte>)s_terminator);
            }
        }

        public void Dispose()
        {
            Finish();
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }
        int WriteChunkPrefix(Span<byte> chunkPrefixBuffer, int chunkLength)
        {
            if (!PrimitiveFormatter.TryFormat(chunkLength, chunkPrefixBuffer, out var written, 'X', TextEncoder.Utf8))
            {
                throw new Exception("cannot format chunk length");
            }

            for (int i = written - 1; i >= 0; i--)
            {
                chunkPrefixBuffer[ChunkPrefixSize - written + i - 2] = chunkPrefixBuffer[i];
            }

            chunkPrefixBuffer[ChunkPrefixSize - 2] = 13;
            chunkPrefixBuffer[ChunkPrefixSize - 1] = 10;

            return written + 2;
        }

        /// <summary>
        ///  Append End of Headers
        /// </summary>
        public void AppendEoh()
        {
            if (_headerSent) throw new Exception("headers already sent");
            this.AppendHttpNewLine();
            Send();
        }
    }
}
