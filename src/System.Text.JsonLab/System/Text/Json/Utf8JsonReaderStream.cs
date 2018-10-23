// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReaderStream
    {
        private Utf8JsonReader _jsonReader;
        private Span<byte> _span;
        private Stream _stream;
        private byte[] _buffer;

        const int StreamSegmentSize = 4_096;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.Value;

        public Utf8JsonReaderStream(Stream jsonStream)
        {
            if (!jsonStream.CanRead)
                JsonThrowHelper.ThrowArgumentException("Stream must be readable");

            _buffer = ArrayPool<byte>.Shared.Rent(StreamSegmentSize);
            int numberOfBytes = jsonStream.Read(_buffer, 0, StreamSegmentSize);
            _span = _buffer.AsSpan(0, numberOfBytes);
            _stream = jsonStream;

            bool isFinalBlock = numberOfBytes < StreamSegmentSize;
            _jsonReader = new Utf8JsonReader(_span, isFinalBlock);
        }

        public bool Read()
        {
            bool result = _jsonReader.Read();
            if (!result)
            {
                return ReadNext();
            }
            return result;
        }

        private bool ReadNext()
        {
            bool result = false;
            bool isFinalBlock = false;

            do
            {
                ReadOnlySpan<byte> leftOver = default;
                if (_jsonReader.Consumed < _span.Length)
                {
                    leftOver = _span.Slice((int)_jsonReader.Consumed);
                }

                if (leftOver.Length > _buffer.Length - StreamSegmentSize)
                {
                    if (leftOver.Length > int.MaxValue - StreamSegmentSize)
                        JsonThrowHelper.ThrowArgumentException("Cannot fit left over data from the previous chunk and the next chunk of data into a 2 GB buffer.");

                    ResizeBuffer(leftOver.Length + StreamSegmentSize);
                }

                leftOver.CopyTo(_buffer);

                int numberOfBytes = _stream.Read(_buffer, leftOver.Length, StreamSegmentSize);
                isFinalBlock = numberOfBytes < StreamSegmentSize;

                _span = _buffer.AsSpan(0, leftOver.Length + numberOfBytes);   // This is gauranteed to not overflow

                _jsonReader = new Utf8JsonReader(_span, isFinalBlock, _jsonReader.State);
                result = _jsonReader.Read();
            } while (!result && !isFinalBlock);

            return result;
        }

        private void ResizeBuffer(int minimumSize)
        {
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = ArrayPool<byte>.Shared.Rent(minimumSize);
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }
    }
}
