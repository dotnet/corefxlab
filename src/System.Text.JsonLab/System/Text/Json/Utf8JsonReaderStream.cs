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
        private bool _isFinalBlock;
        private long _consumed;

        const int FirstSegmentSize = 1_024; //TODO: Is this necessary?
        const int StreamSegmentSize = 4_096;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.Value;
        public long Consumed => _consumed + _jsonReader.Consumed;

        public Utf8JsonReaderStream(Stream jsonStream)
        {
            if (!jsonStream.CanRead)
                JsonThrowHelper.ThrowArgumentException("Stream must be readable");

            _buffer = ArrayPool<byte>.Shared.Rent(FirstSegmentSize);
            int numberOfBytes = jsonStream.Read(_buffer, 0, FirstSegmentSize);
            _span = _buffer.AsSpan(0, numberOfBytes);
            _stream = jsonStream;

            _isFinalBlock = numberOfBytes == 0;
            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock);
            _consumed = 0;
        }

        public bool Read()
        {
            bool result = _jsonReader.Read();
            if (!result && !_isFinalBlock)
            {
                return ReadNext();
            }
            return result;
        }

        private bool ReadNext()
        {
            bool result = false;

            do
            {
                _consumed += _jsonReader.Consumed;
                int leftOver = _span.Length - (int)_jsonReader.Consumed;
                int amountToRead = StreamSegmentSize;
                if (leftOver > 0)
                {
                    _stream.Position -= leftOver;

                    // TODO: Should this be a settable property?
                    if (leftOver >= 1_000_000)
                    {
                        // A single JSON token exceeds 1 MB in size. Start doubling.
                        amountToRead = leftOver * 2;
                    }
                    else
                    {
                        if (_jsonReader.Consumed == 0)
                        {
                            if (leftOver > int.MaxValue - amountToRead)
                                JsonThrowHelper.ThrowArgumentException("Cannot fit left over data from the previous chunk and the next chunk of data into a 2 GB buffer.");

                            amountToRead += leftOver;   // This is gauranteed to not overflow
                            ResizeBuffer(amountToRead);
                        }
                    }
                }

                if (_buffer.Length < amountToRead)
                    ResizeBuffer(amountToRead);

                int numberOfBytes = _stream.Read(_buffer, 0, amountToRead);
                _isFinalBlock = numberOfBytes == 0;

                _span = _buffer.AsSpan(0, numberOfBytes);

                _jsonReader = new Utf8JsonReader(_span, _isFinalBlock, _jsonReader.State);
                result = _jsonReader.Read();
            } while (!result && !_isFinalBlock);

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
