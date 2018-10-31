// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.IO;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReaderStream
    {
        private JsonUtf8Reader _jsonReader;
        private Span<byte> _buffer;
        private Stream _stream;
        private byte[] _pooledArray;
        private bool _isFinalBlock;
        private long _consumed;

        const int FirstSegmentSize = 1_024; //TODO: Is this necessary?
        const int StreamSegmentSize = 4_096;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.ValueSpan;
        public long Consumed => _consumed + _jsonReader.BytesConsumed;

        public Utf8JsonReaderStream(Stream jsonStream)
        {
            if (!jsonStream.CanRead || !jsonStream.CanSeek)
                JsonThrowHelper.ThrowArgumentException("Stream must be readable and seekable.");

            _pooledArray = ArrayPool<byte>.Shared.Rent(FirstSegmentSize);
            int numberOfBytes = jsonStream.Read(_pooledArray, 0, FirstSegmentSize);
            _buffer = _pooledArray.AsSpan(0, numberOfBytes);
            _stream = jsonStream;

            _isFinalBlock = numberOfBytes == 0;
            _jsonReader = new JsonUtf8Reader(_buffer, _isFinalBlock);
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
                _consumed += _jsonReader.BytesConsumed;
                int leftOver = _buffer.Length - (int)_jsonReader.BytesConsumed;
                int amountToRead = StreamSegmentSize;
                if (leftOver > 0)
                {
                    _stream.Position -= leftOver;

                    if (_jsonReader.BytesConsumed == 0)
                    {
                        if (leftOver > 1_000_000_000)
                            JsonThrowHelper.ThrowArgumentException("Cannot fit left over data from the previous chunk and the next chunk of data into a 2 GB buffer.");

                        // This is guaranteed to not overflow due to the check above.
                        amountToRead += leftOver * 2;
                        ResizeBuffer(amountToRead);
                    }
                }

                if (_pooledArray.Length < StreamSegmentSize)
                    ResizeBuffer(StreamSegmentSize);

                int numberOfBytes = _stream.Read(_pooledArray, 0, amountToRead);
                _isFinalBlock = numberOfBytes == 0; // TODO: Can this be inferred differently based on leftOver and numberOfBytes

                _buffer = _pooledArray.AsSpan(0, numberOfBytes);

                _jsonReader = new JsonUtf8Reader(_buffer, _isFinalBlock, _jsonReader.CurrentState);
                result = _jsonReader.Read();
            } while (!result && !_isFinalBlock);

            return result;
        }

        private void ResizeBuffer(int minimumSize)
        {
            Debug.Assert(minimumSize > 0);
            Debug.Assert(_pooledArray != null);
            ArrayPool<byte>.Shared.Return(_pooledArray);
            _pooledArray = ArrayPool<byte>.Shared.Rent(minimumSize);
        }

        public void Dispose()
        {
            if (_pooledArray != null)
            {
                ArrayPool<byte>.Shared.Return(_pooledArray);
                _pooledArray = null;
            }
        }
    }
}
