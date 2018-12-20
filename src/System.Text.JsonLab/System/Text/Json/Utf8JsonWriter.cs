// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        private const int MinimumSizeThreshold = 256;

        private IBufferWriter<byte> _output;
        private int _buffered;
        private Span<byte> _buffer;

        public long BytesWritten
        {
            get
            {
                Debug.Assert(BytesCommitted <= long.MaxValue - _buffered);
                return BytesCommitted + _buffered;
            }
        }

        public long BytesCommitted { get; private set; }

        private bool _inObject;
        private bool _isNotPrimitive;
        private JsonTokenType _tokenType;
        private readonly JsonWriterOptions _writerOptions;
        private BitStack _bitStack;

        // The highest order bit of _currentDepth is used to discern whether we are writing the first item in a list or not.
        // if (_currentDepth >> 31) == 1, add a list separator before writing the item
        // else, no list separator is needed since we are writing the first item.
        private int _currentDepth;

        private int Indentation => CurrentDepth * 2;

        public int CurrentDepth => _currentDepth & JsonConstants.RemoveFlagsBitMask;

        public JsonWriterState CurrentState => new JsonWriterState
        {
            _bytesWritten = BytesWritten,
            _bytesCommitted = BytesCommitted,
            _inObject = _inObject,
            _isNotPrimitive = _isNotPrimitive,
            _tokenType = _tokenType,
            _writerOptions = _writerOptions,
            _bitStack = _bitStack,
        };

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public Utf8JsonWriter2(IBufferWriter<byte> bufferWriter, JsonWriterState state = default)
        {
            _output = bufferWriter ?? throw new ArgumentNullException(nameof(bufferWriter));
            _buffered = 0;
            BytesCommitted = 0;
            _buffer = _output.GetSpan();

            _inObject = state._inObject;
            _isNotPrimitive = state._isNotPrimitive;
            _tokenType = state._tokenType;
            _writerOptions = state._writerOptions;
            _bitStack = state._bitStack;

            _currentDepth = 0;
        }

        public Utf8JsonWriter2(Span<byte> outputSpan, JsonWriterState state = default)
        {
            _output = null;
            _buffered = 0;
            BytesCommitted = 0;
            _buffer = outputSpan;

            _inObject = state._inObject;
            _isNotPrimitive = state._isNotPrimitive;
            _tokenType = state._tokenType;
            _writerOptions = state._writerOptions;
            _bitStack = state._bitStack;

            _currentDepth = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Advance(int count)
        {
            Debug.Assert(count >= 0 && _buffered <= int.MaxValue - count);

            _buffered += count;
            _buffer = _buffer.Slice(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckSizeAndGrow(int count)
        {
            Debug.Assert(count >= 0);

            if (_buffer.Length < count)
                GrowSpan(count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowSpan(int count)
        {
            Flush();

            if (_output == null)
            {
                if (count < MinimumSizeThreshold)
                {
                    throw new ArgumentException("The output span provided is too small.");
                }
            }
            else
            {
                _buffer = _output.GetSpan(count);

                if (_buffer.Length < count && count < MinimumSizeThreshold)
                {
                    throw new ArgumentException("The IBufferWriter could not provide a span that is large enough to continue.");
                }

                Debug.Assert(_buffer.Length >= Math.Min(count, MinimumSizeThreshold));
            }
        }

        public void Flush(bool isFinalBlock = true)
        {
            //TODO: Fix exception message and check other potential conditions for invalid end.
            if (isFinalBlock && !_writerOptions.SkipValidation && CurrentDepth != 0)
                JsonThrowHelper.ThrowJsonWriterException("Invalid end of JSON.");

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Flush()
        {
            BytesCommitted += _buffered;
            if (_output == null)
            {
                Debug.Assert(_buffer.Length >= _buffered);
                _buffer = _buffer.Slice(_buffered);
            }
            else
            {
                _output.Advance(_buffered);
            }
            _buffered = 0;
        }

        public void WriteStartArray()
        {
            WriteStart(JsonConstants.OpenBracket);
            _tokenType = JsonTokenType.StartArray;
        }

        public void WriteStartObject()
        {
            WriteStart(JsonConstants.OpenBrace);
            _tokenType = JsonTokenType.StartObject;
        }

        private void WriteStart(byte token)
        {
            // TODO: Use throw helper with proper error messages
            if (CurrentDepth >= JsonConstants.MaxWriterDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            if (_writerOptions.SlowPath)
                WriteStartSlow(token);
            else
                WriteStartFast(token);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _isNotPrimitive = true;
        }

        private void WriteStartFast(byte token)
        {
            // Calculated based on the following: ',[' OR ',{'
            int bytesNeeded = 2;
            if (_currentDepth < 0)
            {
                Span<byte> byteBuffer = GetSpan(bytesNeeded);
                byteBuffer[0] = JsonConstants.ListSeperator;
                byteBuffer[1] = token;
                Advance(bytesNeeded);
            }
            else
            {
                bytesNeeded--;
                Span<byte> byteBuffer = GetSpan(bytesNeeded);
                byteBuffer[0] = token;
                Advance(bytesNeeded);
            }
        }

        private void WriteStartSlow(byte token)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateStart(token);
                    UpdateBitStackOnStart(token);
                }
                WriteStartFormatted(token);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateStart(token);
                UpdateBitStackOnStart(token);
                WriteStartFast(token);
            }
        }

        private void ValidateStart(byte token)
        {
            if (_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.None && _tokenType != JsonTokenType.StartArray);
                JsonThrowHelper.ThrowJsonWriterException(token, _tokenType);
            }
            else
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                if (_tokenType != JsonTokenType.None && !_isNotPrimitive)
                {
                    JsonThrowHelper.ThrowJsonWriterException(token, _tokenType);
                }
            }
        }

        private void WriteStartFormatted(byte token)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - 2 - indent >= 0);

            // Calculated based on the following: ',\r\n  [' OR ',\r\n  {'
            int bytesNeeded = JsonWriterHelper.NewLineUtf8.Length + 2 + indent;

            if (_currentDepth >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = token;

            Debug.Assert(idx == bytesNeeded);

            Advance(idx);
        }

        public void WriteStartArray(ReadOnlySpan<byte> propertyName, bool suppressEscaping = false)
        {
            WriteStart(ref propertyName, JsonConstants.OpenBracket, suppressEscaping);
            _tokenType = JsonTokenType.StartArray;
        }

        public void WriteStartObject(ReadOnlySpan<byte> propertyName, bool suppressEscaping = false)
        {
            WriteStart(ref propertyName, JsonConstants.OpenBrace, suppressEscaping);
            _tokenType = JsonTokenType.StartObject;
        }

        private unsafe void WriteStart(ref ReadOnlySpan<byte> propertyName, byte token, bool suppressEscaping)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxTokenSize || CurrentDepth >= JsonConstants.MaxWriterDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);

            ReadOnlySpan<byte> escapedSpan = propertyName;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedSpan = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStartSlow(ref escapedSpan, token);
            else
                WriteStartFast(ref escapedSpan, token);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
        }

        private void WriteStartFast(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 >= 0);

            // Calculated based on the following: ',"propertyName":[' OR ',"propertyName":{'
            int bytesNeeded = propertyName.Length + 5;

            if (_currentDepth >= 0)
                bytesNeeded--;

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = token;

            Advance(idx);
        }

        private void WriteStartSlow(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateStartWithPropertyName(token);
                    UpdateBitStackOnStart(token);
                }
                WriteStartFormatted(ref propertyName, token);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateStartWithPropertyName(token);
                UpdateBitStackOnStart(token);
                WriteStartFast(ref propertyName, token);
            }
        }

        private void ValidateStartWithPropertyName(byte token)
        {
            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resouce message
            }
        }

        private void WriteStartFormatted(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - JsonWriterHelper.NewLineUtf8.Length - 6 - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": [' OR ',\r\n  "propertyName": {'
            int bytesNeeded = propertyName.Length + 6 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            _buffer[idx++] = token;

            Advance(idx);
        }

        public void WriteStartArray(string propertyName, bool suppressEscaping = false)
            => WriteStartArray(propertyName.AsSpan(), suppressEscaping);

        public void WriteStartObject(string propertyName, bool suppressEscaping = false)
            => WriteStartObject(propertyName.AsSpan(), suppressEscaping);

        public void WriteStartObject(ReadOnlySpan<char> propertyName, bool suppressEscaping = false)
        {
            ValidatePropertyNameAndDepth(ref propertyName);

            WriteStartObjectWithEncoding(MemoryMarshal.AsBytes(propertyName), suppressEscaping);
        }

        private unsafe void WriteStartObjectWithEncoding(ReadOnlySpan<byte> propertyName, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedSpan = propertyName;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedSpan = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStartSlowWithEncoding(ref escapedSpan, JsonConstants.OpenBrace);
            else
                WriteStartFastWithEncoding(ref escapedSpan, JsonConstants.OpenBrace);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _tokenType = JsonTokenType.StartObject;
        }

        public void WriteStartArray(ReadOnlySpan<char> propertyName, bool suppressEscaping = false)
        {
            ValidatePropertyNameAndDepth(ref propertyName);

            WriteStartArrayWithEncoding(MemoryMarshal.AsBytes(propertyName), suppressEscaping);
        }

        private unsafe void WriteStartArrayWithEncoding(ReadOnlySpan<byte> propertyName, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedSpan = propertyName;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedSpan = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStartSlowWithEncoding(ref escapedSpan, JsonConstants.OpenBracket);
            else
                WriteStartFastWithEncoding(ref escapedSpan, JsonConstants.OpenBracket);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _tokenType = JsonTokenType.StartArray;
        }

        private void WriteStartObjectWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_writerOptions.SlowPath)
                WriteStartSlowWithEncoding(ref propertyName, JsonConstants.OpenBrace);
            else
                WriteStartFastWithEncoding(ref propertyName, JsonConstants.OpenBrace);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _tokenType = JsonTokenType.StartObject;
        }

        private void WriteStartFastWithEncoding(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 >= 0);

            // Calculated based on the following: ',"encoded propertyName":[' OR ',"encoded propertyName":{'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5;

            if (_currentDepth >= 0)
                bytesNeeded--;

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = token;

            Advance(idx);
        }

        private void WriteStartSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateStartWithPropertyName(token);
                    UpdateBitStackOnStart(token);
                }
                WriteStartFormattedWithEncoding(ref propertyName, token);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateStartWithPropertyName(token);
                UpdateBitStackOnStart(token);
                WriteStartFastWithEncoding(ref propertyName, token);
            }
        }

        private void WriteStartFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, byte token)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - JsonWriterHelper.NewLineUtf8.Length - 6 - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": [' OR ',\r\n  "encoded propertyName": {'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            _buffer[idx++] = token;

            Advance(idx);
        }

        public void WriteEndArray()
        {
            WriteEnd(JsonConstants.CloseBracket);
            _tokenType = JsonTokenType.EndArray;
        }

        public void WriteEndObject()
        {
            WriteEnd(JsonConstants.CloseBrace);
            _tokenType = JsonTokenType.EndObject;
        }

        private void WriteEnd(byte token)
        {
            _currentDepth |= 1 << 31;
            _currentDepth--;

            if (_writerOptions.SlowPath)
                WriteEndSlow(token);
            else
                WriteEndFast(token);
        }

        private void WriteEndFast(byte token)
        {
            Span<byte> byteBuffer = GetSpan(1);
            byteBuffer[0] = token;
            Advance(1);
        }

        private void WriteEndSlow(byte token)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateEnd(token);
                }
                WriteEndFormatted(token);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateEnd(token);
                WriteEndFast(token);
            }
        }

        private void ValidateEnd(byte token)
        {
            if (_bitStack.CurrentDepth <= 0)
                JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resource message

            if (token == JsonConstants.CloseBracket)
            {
                if (_inObject)
                {
                    Debug.Assert(_tokenType != JsonTokenType.None);
                    JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resource message
                }
            }
            else
            {
                Debug.Assert(token == JsonConstants.CloseBrace);

                if (!_inObject)
                {
                    JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resource message
                }
            }

            _inObject = _bitStack.Pop();
        }

        private void WriteEndFormatted(byte token)
        {
            // Do not format/indent empty JSON object/array.
            if ((token == JsonConstants.CloseBrace && _tokenType == JsonTokenType.StartObject)
                || (token == JsonConstants.CloseBracket && _tokenType == JsonTokenType.StartArray))
            {
                WriteEndFast(token);
            }
            else
            {
                // Necessary if WriteEndX is called without a corresponding WriteStartX first.
                // Checking for int.MaxValue because int.MinValue - 1 = int.MaxValue
                if (_currentDepth == int.MaxValue)
                {
                    _currentDepth = 0;
                }

                int indent = Indentation;

                // This is guaranteed not to overflow.
                Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - 1 - indent >= 0);

                // For new line (\r\n or \n), indentation (based on depth) and end token ('}' or ']').
                int bytesNeeded = JsonWriterHelper.NewLineUtf8.Length + 1 + indent;

                Span<byte> byteBuffer = GetSpan(bytesNeeded);

                int idx = 0;

                WriteNewLine(ref byteBuffer, ref idx);

                idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

                byteBuffer[idx++] = token;

                Debug.Assert(idx == bytesNeeded);

                Advance(idx);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> GetSpan(int bytesNeeded)
        {
            CheckSizeAndGrow(bytesNeeded);
            Debug.Assert(_buffer.Length >= bytesNeeded);
            return _buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNewLine(ref Span<byte> byteBuffer, ref int idx)
        {
            // Write '\r\n' OR '\n', depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
                byteBuffer[idx++] = JsonConstants.CarriageReturn;

            byteBuffer[idx++] = JsonConstants.LineFeed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNewLine(ref int idx)
        {
            // Write '\r\n' OR '\n', depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
            {
                while (_buffer.Length <= idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }
                _buffer[idx++] = JsonConstants.CarriageReturn;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.LineFeed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateBitStackOnStart(byte token)
        {
            if (token == JsonConstants.OpenBracket)
            {
                _bitStack.PushFalse();
                _inObject = false;
            }
            else
            {
                Debug.Assert(token == JsonConstants.OpenBrace);
                _bitStack.PushTrue();
                _inObject = true;
            }
        }
    }
}
