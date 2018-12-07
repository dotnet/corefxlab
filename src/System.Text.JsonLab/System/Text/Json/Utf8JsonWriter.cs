// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonWriter2<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
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

        public void WriteNull(string propertyName)
            => WriteNull(propertyName.AsSpan());

        public void WriteNull(ReadOnlySpan<char> propertyName)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNullWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteNullWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteNullSlowWithEncoding(propertyName);
            else
                WriteNullFastWithEncoding(propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFastWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 8 >= 0);

            // Calculated based on the following: ',"encoded propertyName":null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 8;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            _bufferWriter.Advance(idx);
        }

        private void WriteNullSlowWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNullFormattedWithEncoding(propertyName);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNullFastWithEncoding(propertyName);
            }
        }

        private void WriteNullFormattedWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 9 - indent - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 9 + indent + JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            _bufferWriter.Advance(idx);
        }

        public void WriteNull(ReadOnlySpan<byte> propertyName)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_options.SlowPath)
                WriteNullSlow(propertyName);
            else
                WriteNullFast(propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFast(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 8 >= 0);

            // Calculated based on the following: ',"propertyName":null'
            int bytesNeeded = propertyName.Length + 8;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            _bufferWriter.Advance(idx);
        }

        private void WriteNullSlow(ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNullFormatted(propertyName);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNullFast(propertyName);
            }
        }

        private void WriteNullFormatted(ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 9 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": null'
            int bytesNeeded = propertyName.Length + 9 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            _bufferWriter.Advance(idx);
        }

        public void WriteBoolean(string propertyName, bool value)
            => WriteBoolean(propertyName.AsSpan(), value);

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteBooleanWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteBooleanWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            if (_options.SlowPath)
                WriteBooleanSlowWithEncoding(propertyName, value);
            else
                WriteBooleanFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFastWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 9 >= 0);

            // Calculated based on the following: ',"encoded propertyName":true' OR ',"encoded propertyName":false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 9;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            _bufferWriter.Advance(idx);
        }

        private void WriteBooleanSlowWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteBooleanFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteBooleanFastWithEncoding(propertyName, value);
            }
        }

        private void WriteBooleanFormattedWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 10 - indent - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": true' OR ',\r\n  "encoded propertyName": false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 10 + indent + JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            _bufferWriter.Advance(idx);
        }

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_options.SlowPath)
                WriteBooleanSlow(propertyName, value);
            else
                WriteBooleanFast(propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFast(ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 9 >= 0);

            // Calculated based on the following: ',"propertyName":true' OR ',"propertyName":false'
            int bytesNeeded = propertyName.Length + 9;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            _bufferWriter.Advance(idx);
        }

        private void WriteBooleanSlow(ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteBooleanFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteBooleanFast(propertyName, value);
            }
        }

        private void WriteBooleanFormatted(ReadOnlySpan<byte> propertyName, bool value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 10 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": true' OR ',\r\n  "propertyName": false'
            int bytesNeeded = propertyName.Length + 10 + JsonWriterHelper.NewLineUtf8.Length + indent;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            _bufferWriter.Advance(idx);
        }

        public void WriteNumber(string propertyName, int value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            if (_options.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 15 >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 15;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _bufferWriter.Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 16 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 16 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _bufferWriter.Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_options.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, int value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 15 >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 15;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _bufferWriter.Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, int value)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, int value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 16 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 16 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = EscapeStringValue(value);

            if (_options.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(propertyName, value);
            else
                WriteStringFastWithEncodingPropertyValue(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 6;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringSlowWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncodingPropertyValue(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteStringFastWithEncodingPropertyValue(propertyName, escapedValue);
            }
        }

        private void ValidateWritingPropertyWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            // TODO: Add "char" like escape check
            if (JsonWriterHelper.IndexOfAnyEscape(propertyName) != -1)
            {
                //JsonThrowHelper.ThrowJsonWriterException("Property name must be properly escaped."); //TODO: Fix message
            }

            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException("Cannot add a property within an array.");    //TODO: Add resouce message
            }
        }

        private void WriteStringFormattedWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = EscapeStringValue(value);

            if (_options.SlowPath)
                WriteStringSlowWithEncodingProperty(propertyName, value);
            else
                WriteStringFastWithEncodingProperty(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 6;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringSlowWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncodingProperty(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteStringFastWithEncodingProperty(propertyName, escapedValue);
            }
        }

        private void WriteStringFormattedWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = EscapeStringValue(value);

            if (_options.SlowPath)
                WriteStringSlowWithEncodingValue(propertyName, value);
            else
                WriteStringFastWithEncodingValue(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 6;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringSlowWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormattedWithEncodingValue(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteStringFastWithEncodingValue(propertyName, escapedValue);
            }
        }

        private void WriteStringFormattedWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        public void WriteString(string propertyName, string value)
            => WriteString(propertyName.AsSpan(), value.AsSpan());

        public void WriteString(string propertyName, ReadOnlySpan<char> value)
            => WriteString(propertyName.AsSpan(), value);

        public void WriteString(string propertyName, ReadOnlySpan<byte> value)
            => WriteString(propertyName.AsSpan(), value);

        public void WriteString(ReadOnlySpan<char> propertyName, string value)
            => WriteString(propertyName, value.AsSpan());

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingPropertyValue(MemoryMarshal.AsBytes(propertyName), MemoryMarshal.AsBytes(value));
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingProperty(MemoryMarshal.AsBytes(propertyName), value);
        }
        public void WriteString(ReadOnlySpan<byte> propertyName, string value)
            => WriteString(propertyName, value.AsSpan());

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingValue(propertyName, MemoryMarshal.AsBytes(value));
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
            {
                //TODO: Add escaping.
                value = EscapeStringValue(value);
            }

            if (_options.SlowPath)
                WriteStringSlow(propertyName, value);
            else
                WriteStringFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private static ReadOnlySpan<byte> EscapeStringValue(ReadOnlySpan<byte> value)
        {
            //TODO: Add escaping logic.
            return value;
        }

        private void WriteStringFast(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length + 6;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
        }

        private void WriteStringSlow(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_options.Formatted || !_options.SkipValidation);

            if (_options.Formatted)
            {
                if (!_options.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormatted(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_options.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteStringFast(propertyName, escapedValue);
            }
        }

        private void ValidateWritingProperty(ReadOnlySpan<byte> propertyName)
        {
            if (JsonWriterHelper.IndexOfAnyEscape(propertyName) != -1)
                JsonThrowHelper.ThrowJsonWriterException("Property name must be properly escaped."); //TODO: Fix message

            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException("Cannot add a property within an array.");    //TODO: Add resouce message
            }
        }

        private void WriteStringFormatted(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - propertyName.Length - escapedValue.Length - 7 - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "value"'
            int bytesNeeded = propertyName.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent + escapedValue.Length;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePropertyNameAndDepth(ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize || CurrentDepth >= JsonConstants.MaxDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyNameEncoded(ReadOnlySpan<byte> propertyName, int bytesNeeded, out int idx)
        {
            if (_currentDepth >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            return byteBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyNameEncodedAndFormatted(ReadOnlySpan<byte> propertyName, int bytesNeeded, int indent, out int idx)
        {
            if (_currentDepth >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            return byteBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyName(ReadOnlySpan<byte> propertyName, int bytesNeeded, out int idx)
        {
            if (_currentDepth >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            return byteBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyNameFormatted(ReadOnlySpan<byte> propertyName, int bytesNeeded, int indent, out int idx)
        {
            if (_currentDepth >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            return byteBuffer;
        }
    }
}
