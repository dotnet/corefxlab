// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        private readonly JsonWriterOptions _options;
        private BufferWriter<TBufferWriter> _bufferWriter;
        private JsonTokenType _tokenType;
        private bool _inObject;
        private BitStack _bitStack;

        // The highest order bit of _currentDepth is used to discern whether we are writing the first item in a list or not.
        // if (_currentDepth >> 31) == 1, add a list separator before writing the item
        // else, no list separator is needed since we are writing the first item.
        private int _currentDepth;

        public int BytesWritten { get; private set; }

        private int Indentation => CurrentDepth * 2;

        public int CurrentDepth => _currentDepth & JsonConstants.RemoveFlagsBitMask;

        public JsonTokenType TokenType => _tokenType;

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public Utf8JsonWriter2(TBufferWriter bufferWriter, JsonWriterOptions options = default)
        {
            _bufferWriter = new BufferWriter<TBufferWriter>(bufferWriter);
            _options = options;
            _currentDepth = 0;
            BytesWritten = 0;
            _tokenType = JsonTokenType.None;
            _inObject = false;
            _bitStack = default;
        }

        public void Flush(bool isFinalBlock = true)
        {
            //TODO: Fix exception message and check other potential conditions for invalid end.
            if (isFinalBlock && !_options.SkipValidation && CurrentDepth != 0)
                JsonThrowHelper.ThrowJsonWriterException("Invalid end of JSON.");

            _bufferWriter.Flush();
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
            if (CurrentDepth >= JsonConstants.MaxDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            if (_options.SlowPath)
                WriteStartSlow(token);
            else
                WriteStartFast(token);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
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
                _bufferWriter.Advance(bytesNeeded);
            }
            else
            {
                bytesNeeded--;
                Span<byte> byteBuffer = GetSpan(bytesNeeded);
                byteBuffer[0] = token;
                _bufferWriter.Advance(bytesNeeded);
            }
        }

        private void WriteStartSlow(byte token)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateStart(token);
                }
                WriteStartFormatted(token);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateStart(token);
                WriteStartFast(token);
            }
        }

        private void ValidateStart(byte token)
        {
            if (_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.None && _tokenType != JsonTokenType.StartArray);
                if (_tokenType != JsonTokenType.PropertyName)
                {
                    JsonThrowHelper.ThrowJsonWriterException(token, _tokenType);    //TODO: Add resource message
                }
            }
            else
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                if (_tokenType == JsonTokenType.PropertyName)
                {
                    JsonThrowHelper.ThrowJsonWriterException(token, _tokenType);    //TODO: Add resource message
                }
            }

            UpdateBitStackOnStart(token);
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
                WriteNewLine(byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = token;

            Debug.Assert(idx == bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteStartArray(ReadOnlySpan<byte> propertyName)
        {
            WriteStart(propertyName, JsonConstants.OpenBracket);
            _tokenType = JsonTokenType.StartArray;
        }

        public void WriteStartObject(ReadOnlySpan<byte> propertyName)
        {
            WriteStart(propertyName, JsonConstants.OpenBrace);
            _tokenType = JsonTokenType.StartArray;
        }

        private void WriteStart(ReadOnlySpan<byte> propertyName, byte token)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxTokenSize || CurrentDepth >= JsonConstants.MaxDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);

            if (_options.SlowPath)
                WriteStartSlow(propertyName, token);
            else
                WriteStartFast(propertyName, token);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
        }

        private void WriteStartFast(ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 >= 0);

            // Calculated based on the following: ',"propertyName":[' OR ',"propertyName":{'
            int bytesNeeded = propertyName.Length + 5;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        private void WriteStartSlow(ReadOnlySpan<byte> propertyName, byte token)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateStart(propertyName, token);
                }
                WriteStartFormatted(propertyName, token);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateStart(propertyName, token);
                WriteStartFast(propertyName, token);
            }
        }

        private void ValidateStart(ReadOnlySpan<byte> propertyName, byte token)
        {
            if (JsonWriterHelper.IndexOfAnyEscape(propertyName) != -1)
                JsonThrowHelper.ThrowJsonWriterException("Property name must be properly escaped."); //TODO: Fix message

            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resouce message
            }

            UpdateBitStackOnStart(token);
        }

        private void WriteStartFormatted(ReadOnlySpan<byte> propertyName, byte token)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - JsonWriterHelper.NewLineUtf8.Length - 6 - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": [' OR ',\r\n  "propertyName": {'
            int bytesNeeded = propertyName.Length + 6 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        public void WriteStartArray(string propertyName)
            => WriteStartArray(propertyName.AsSpan());

        public void WriteStartObject(string propertyName)
            => WriteStartObject(propertyName.AsSpan());

        public void WriteStartArray(ReadOnlySpan<char> propertyName)
        {
            ValidatePropertyNameAndDepth(propertyName);

            WriteStartArrayWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        public void WriteStartObject(ReadOnlySpan<char> propertyName)
        {
            ValidatePropertyNameAndDepth(propertyName);

            WriteStartObjectWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteStartArrayWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteStartSlowWithEncoding(propertyName, JsonConstants.OpenBracket);
            else
                WriteStartFastWithEncoding(propertyName, JsonConstants.OpenBracket);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _tokenType = JsonTokenType.StartArray;
        }

        private void WriteStartObjectWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteStartSlowWithEncoding(propertyName, JsonConstants.OpenBrace);
            else
                WriteStartFastWithEncoding(propertyName, JsonConstants.OpenBrace);

            _currentDepth &= JsonConstants.RemoveFlagsBitMask;
            _currentDepth++;
            _tokenType = JsonTokenType.StartObject;
        }

        private void WriteStartFastWithEncoding(ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 >= 0);

            // Calculated based on the following: ',"encoded propertyName":[' OR ',"encoded propertyName":{'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        private void WriteStartSlowWithEncoding(ReadOnlySpan<byte> propertyName, byte token)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateStartWithEncoding(propertyName, token);
                }
                WriteStartFormattedWithEncoding(propertyName, token);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateStartWithEncoding(propertyName, token);
                WriteStartFastWithEncoding(propertyName, token);
            }
        }

        private void ValidateStartWithEncoding(ReadOnlySpan<byte> propertyName, byte token)
        {
            // TODO: Add "char" like escape check
            if (JsonWriterHelper.IndexOfAnyEscape(propertyName) != -1)
            {
                //JsonThrowHelper.ThrowJsonWriterException("Property name must be properly escaped."); //TODO: Fix message
            }

            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException(token);    //TODO: Add resouce message
            }

            UpdateBitStackOnStart(token);
        }

        private void WriteStartFormattedWithEncoding(ReadOnlySpan<byte> propertyName, byte token)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - JsonWriterHelper.NewLineUtf8.Length - 6 - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": [' OR ',\r\n  "encoded propertyName": {'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
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

            if (_options.SlowPath)
                WriteEndSlow(token);
            else
                WriteEndFast(token);
        }

        private void WriteEndFast(byte token)
        {
            Span<byte> byteBuffer = GetSpan(1);
            byteBuffer[0] = token;
            _bufferWriter.Advance(1);
        }

        private void WriteEndSlow(byte token)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateEnd(token);
                }
                WriteEndFormatted(token);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
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

                _bufferWriter.Ensure(bytesNeeded);
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                int idx = 0;

                WriteNewLine(byteBuffer, ref idx);

                idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

                byteBuffer[idx++] = token;

                Debug.Assert(idx == bytesNeeded);

                _bufferWriter.Advance(idx);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> GetSpan(int bytesNeeded)
        {
            _bufferWriter.Ensure(bytesNeeded);
            Debug.Assert(_bufferWriter.Buffer.Length >= bytesNeeded);
            return _bufferWriter.Buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNewLine(Span<byte> byteBuffer, ref int idx)
        {
            // Write '\r\n' OR '\n', depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
                byteBuffer[idx++] = JsonConstants.CarriageReturn;

            byteBuffer[idx++] = JsonConstants.LineFeed;
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
