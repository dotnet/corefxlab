// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReaderSequence
    {
        private Utf8JsonReader _jsonReader;
        private Span<byte> _span;
        private readonly bool _isFinalBlock;
        private ReadOnlySequence<byte>.Enumerator _enumerator;
        private byte[] _buffer;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.Value;

        public Utf8JsonReaderSequence(in ReadOnlySequence<byte> data)
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
            _span = _span.Slice(0, currentSpan.Length);

            _isFinalBlock = data.IsSingleSegment;
            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock);
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
            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock, state);
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
            bool noMoreData = !_enumerator.MoveNext();
            if (noMoreData)
                return false;

            ReadOnlySpan<byte> leftOver = default;
            if (_jsonReader.Consumed != _span.Length)
            {
                leftOver = _span.Slice((int)_jsonReader.Consumed);
            }

            _span = _buffer;
            leftOver.CopyTo(_span);

            ReadOnlySpan<byte> currentSpan = _enumerator.Current.Span;
            currentSpan.CopyTo(_span.Slice(leftOver.Length));
            _span = _span.Slice(0, currentSpan.Length + leftOver.Length);

            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock, _jsonReader.State);
            return _jsonReader.Read();
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }
    }
}
