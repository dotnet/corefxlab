// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReaderSequence
    {
        private Utf8JsonReader _jsonReader;
        private ReadOnlySequence<byte> _data;
        private Span<byte> _span;
        private bool _isFinalBlock;
        private JsonReaderState _state;
        private ReadOnlySequence<byte>.Enumerator _enumerator;
        private byte[] _buffer;

        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySpan<byte> Value => _jsonReader.Value;

        public Utf8JsonReaderSequence(in ReadOnlySequence<byte> data, ReadOnlySpan<byte> leftOver = default)
        {
            _data = data;
            _enumerator = _data.GetEnumerator();
            _enumerator.MoveNext();
            ReadOnlySpan<byte> localSpan = _enumerator.Current.Span;
            _buffer = ArrayPool<byte>.Shared.Rent(localSpan.Length * 2);

            _span = _buffer;
            leftOver.CopyTo(_span);
            localSpan.CopyTo(_span.Slice(leftOver.Length));
            _span = _span.Slice(0, localSpan.Length + leftOver.Length);

            _isFinalBlock = _span.Length == 0;
            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock);
            
            _state = default;
        }

        public Utf8JsonReaderSequence(in ReadOnlySequence<byte> data, bool isFinalBlock, ReadOnlySpan<byte> leftOver = default, JsonReaderState state = default)
        {
            _data = data;
            _enumerator = _data.GetEnumerator();
            _enumerator.MoveNext();
            ReadOnlySpan<byte> localSpan = _enumerator.Current.Span;
            _buffer = ArrayPool<byte>.Shared.Rent(localSpan.Length * 2);

            _span = _buffer;
            leftOver.CopyTo(_span);
            localSpan.CopyTo(_span.Slice(leftOver.Length));
            _span = _span.Slice(0, _span.Length + leftOver.Length);

            _isFinalBlock = _span.Length == 0 || isFinalBlock;
            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock, state);
            
            if (!state.IsDefault)
            {
                _state = state;
            }
            else
            {
                _state = default;
            }
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
            _isFinalBlock = !_enumerator.MoveNext();
            if (_isFinalBlock)
                return false;

            _state = _jsonReader.State;

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

            _jsonReader = new Utf8JsonReader(_span, _isFinalBlock, _state);
            return _jsonReader.Read();
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }
    }
}
