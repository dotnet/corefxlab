// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReaderSequence
    {
        private JsonUtf8Reader _jsonReader;
        private Span<byte> _span;
        private readonly bool _isFinalBlock;
        private ReadOnlySequence<byte>.Enumerator _enumerator;
        private byte[] _buffer;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.ValueSpan;

        public Utf8JsonReaderSequence(in ReadOnlySequence<byte> data)
        {
            _enumerator = data.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                if (_enumerator.Current.Length != 0)
                    break;
            }
            ReadOnlySpan<byte> currentSpan = _enumerator.Current.Span;
            _buffer = ArrayPool<byte>.Shared.Rent(currentSpan.Length * 2);  //TODO: Support data straddling more than 1 segment

            _span = _buffer;
            currentSpan.CopyTo(_span);
            _span = _span.Slice(0, currentSpan.Length);

            _isFinalBlock = data.IsSingleSegment;
            _jsonReader = new JsonUtf8Reader(_span, _isFinalBlock);
        }

        public Utf8JsonReaderSequence(in ReadOnlySequence<byte> data, bool isFinalBlock, JsonReaderState state = default)
        {
            _enumerator = data.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                if (_enumerator.Current.Length != 0)
                    break;
            }
            ReadOnlySpan<byte> currentSpan = _enumerator.Current.Span;
            _buffer = ArrayPool<byte>.Shared.Rent(currentSpan.Length * 2);

            _span = _buffer;
            currentSpan.CopyTo(_span);
            _span = _span.Slice(0, _span.Length);

            _isFinalBlock = isFinalBlock;
            _jsonReader = new JsonUtf8Reader(_span, _isFinalBlock, state);
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
            while (true)
            {
                bool noMoreData = !_enumerator.MoveNext();
                if (noMoreData)
                    return false;
                if (_enumerator.Current.Length != 0)
                    break;
            }

            ReadOnlySpan<byte> currentSpan = _enumerator.Current.Span;

            ReadOnlySpan<byte> leftOver = default;
            if (_jsonReader.BytesConsumed != _span.Length)
            {
                leftOver = _span.Slice((int)_jsonReader.BytesConsumed);
            }

            int bufferSize = leftOver.Length + currentSpan.Length;
            if (bufferSize > _buffer.Length)
            {
                ResizeBuffer(bufferSize);
            }

            _span = _buffer;
            leftOver.CopyTo(_span);
            
            currentSpan.CopyTo(_span.Slice(leftOver.Length));
            _span = _span.Slice(0, bufferSize);

            _jsonReader = new JsonUtf8Reader(_span, _isFinalBlock, _jsonReader.CurrentState);
            return _jsonReader.Read();
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
