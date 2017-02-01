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
    class OwnedLinkableBuffer : OwnedMemory<byte>, IMemoryList<byte>
    {
        internal OwnedLinkableBuffer _next;
        int _written;

        public OwnedLinkableBuffer(int desiredSize = 1024) : base(ArrayPool<byte>.Shared.Rent(desiredSize))
        { }

        public Memory<byte> First => Memory;

        public Memory<byte> Free => Memory.Slice(_written);

        public IMemoryList<byte> Rest => _next;

        int? ISequence<Memory<byte>>.Length => null;

        public int CopyTo(Span<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = true)
        {
            throw new NotImplementedException();
        }

        public void Enlarge(int desiredSize)
        {
            _next = new OwnedLinkableBuffer(desiredSize);
        }

        protected override void Dispose(bool disposing)
        {
            var array = Array;
            base.Dispose(disposing);
            ArrayPool<byte>.Shared.Return(array);
            _next.Dispose();
        }

        internal void Advance(int bytes)
        {
            _written += bytes;
        }
    }

    public class ConnectionFormatter : ITextOutput, IDisposable
    {
        static byte[] s_terminator = new Utf8String("0\r\n\r\n").Bytes.ToArray();
        const int ChunkPrefixSize = 10;

        TcpConnection _connection;
        int _written;
        byte[] _buffer = new byte[1024];
        int _headersEndIndex = 0;

        public ConnectionFormatter(TcpConnection connection)
        {
            _connection = connection;
        }

        public EncodingData Encoding => EncodingData.InvariantUtf8;

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
            if (_headersEndIndex > 0)
            {
                toSend = _buffer.Slice(ChunkPrefixSize, _headersEndIndex);
                _written = 0;
                _headersEndIndex = -1; // headers sent
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

        public void Finish()
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
        }
        int WriteChunkPrefix(Span<byte> chunkPrefixBuffer, int chunkLength)
        {
            if (!PrimitiveFormatter.TryFormat(chunkLength, chunkPrefixBuffer, out var written, 'X', EncodingData.InvariantUtf8))
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
            if (_headersEndIndex == -1) throw new Exception("headers already sent");
            this.AppendHttpNewLine();
            _headersEndIndex += _written;
            Send();
        }
    }
}
