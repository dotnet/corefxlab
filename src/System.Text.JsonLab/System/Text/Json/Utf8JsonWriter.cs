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

        // The highest order bit of _indent is used to discern whether we are writing the first item in a list or not.
        // if (_indent >> 31) == 1, add a list separator before writing the item
        // else, no list separator is needed since we are writing the first item.
        private int _indent;

        private const int RemoveFlagsBitMask = 0x7FFFFFFF;
        private const int MaxDepth = (int.MaxValue - 2_000_001_000) / 2;  // 73_741_323 (to account for double space indentation), leaving 1_000 buffer for "JSONifying"
        private const int MaxTokenSize = 1_000_000_000; // 1 GB
        private const int MaxCharacterTokenSize = 1_000_000_000 / 3; // 333 million characters, i.e. 333 MB

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public Utf8JsonWriter2(TBufferWriter bufferWriter, JsonWriterOptions options = default)
        {
            _bufferWriter = new BufferWriter<TBufferWriter>(bufferWriter);
            _options = options;
            _indent = 0;
            _tokenType = JsonTokenType.None;
            _inObject = false;
            _bitStack = default;
        }

        public void Flush() => _bufferWriter.Flush();

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
            if ((_indent & RemoveFlagsBitMask) >= MaxDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            if (_options.SlowPath)
                WriteStartSlow(token);
            else
                WriteStartFast(token);

            _indent &= RemoveFlagsBitMask;
            _indent++;
        }

        private void WriteStartFast(byte token)
        {
            // Calculated based on the following: ',[' OR ',{'
            int bytesNeeded = 2;
            if (_indent < 0)
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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - 2 - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  [' OR ',\r\n  {'
            int bytesNeeded = JsonWriterHelper.NewLineUtf8.Length + 2 + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

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
            if (propertyName.Length > MaxTokenSize || (_indent & RemoveFlagsBitMask) >= MaxDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            if (_options.SlowPath)
                WriteStartSlow(propertyName, token);
            else
                WriteStartFast(propertyName, token);

            _indent &= RemoveFlagsBitMask;
            _indent++;
        }

        private void WriteStartFast(ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 >= 0);

            // Calculated based on the following: ',"propertyName":[' OR ',"propertyName":{'
            int bytesNeeded = propertyName.Length + 5;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = token;

            Debug.Assert(idx == bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - JsonWriterHelper.NewLineUtf8.Length - 6 - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": [' OR ',\r\n  "propertyName": {'
            int bytesNeeded = propertyName.Length + 6 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;
            byteBuffer[idx++] = token;

            Debug.Assert(idx == bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteStartArray(string propertyName)
            => WriteStartArray(propertyName.AsSpan());

        public void WriteStartObject(string propertyName)
            => WriteStartObject(propertyName.AsSpan());

        public void WriteStartArray(ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize || (_indent & RemoveFlagsBitMask) >= MaxDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            WriteStartArrayWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        public void WriteStartObject(ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize || (_indent & RemoveFlagsBitMask) >= MaxDepth)
                JsonThrowHelper.ThrowJsonWriterException("Depth too large.");

            WriteStartObjectWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteStartArrayWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteStartSlowWithEncoding(propertyName, JsonConstants.OpenBracket);
            else
                WriteStartFastWithEncoding(propertyName, JsonConstants.OpenBracket);

            _indent &= RemoveFlagsBitMask;
            _indent++;
            _tokenType = JsonTokenType.StartArray;
        }

        private void WriteStartObjectWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteStartSlowWithEncoding(propertyName, JsonConstants.OpenBrace);
            else
                WriteStartFastWithEncoding(propertyName, JsonConstants.OpenBrace);

            _indent &= RemoveFlagsBitMask;
            _indent++;
            _tokenType = JsonTokenType.StartObject;
        }

        private void WriteStartFastWithEncoding(ReadOnlySpan<byte> propertyName, byte token)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 >= 0);

            // Calculated based on the following: ',"encoded propertyName":[' OR ',"encoded propertyName":{'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = token;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - JsonWriterHelper.NewLineUtf8.Length - 6 - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": [' OR ',\r\n  "encoded propertyName": {'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;
            byteBuffer[idx++] = token;

            Debug.Assert(idx <= bytesNeeded);

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
            _indent |= 1 << 31;
            _indent--;

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
                if (_indent == int.MaxValue)
                {
                    _indent = 0;
                }

                int indent = _indent & RemoveFlagsBitMask;

                // This is guaranteed not to overflow.
                Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - 1 - indent * 2 >= 0);

                // For new line (\r\n or \n), indentation (based on depth) and end token ('}' or ']').
                int bytesNeeded = JsonWriterHelper.NewLineUtf8.Length + 1 + indent * 2;

                _bufferWriter.Ensure(bytesNeeded);
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                int idx = 0;

                WriteNewLine(byteBuffer, ref idx);
                WriteIndentation(byteBuffer, ref idx, indent);

                byteBuffer[idx++] = token;

                Debug.Assert(idx == bytesNeeded);

                _bufferWriter.Advance(idx);
            }
        }

        public void WriteNull(string propertyName)
            => WriteNull(propertyName.AsSpan());

        public void WriteNull(ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            WriteNullWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteNullWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_options.SlowPath)
                WriteNullSlowWithEncoding(propertyName);
            else
                WriteNullFastWithEncoding(propertyName);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFastWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 8 >= 0);

            // Calculated based on the following: ',"encoded propertyName":null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 8;

            if (_indent >= 0)
                bytesNeeded--;

            ReadOnlySpan<byte> valueSpan = JsonConstants.NullValue;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 9 - indent * 2 - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 9 + indent * 2 + JsonWriterHelper.NewLineUtf8.Length;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.NullValue;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx <= bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteNull(ReadOnlySpan<byte> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            if (_options.SlowPath)
                WriteNullSlow(propertyName);
            else
                WriteNullFast(propertyName);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFast(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 8 >= 0);

            // Calculated based on the following: ',"propertyName":null'
            int bytesNeeded = propertyName.Length + 8;

            if (_indent >= 0)
                bytesNeeded--;

            ReadOnlySpan<byte> valueSpan = JsonConstants.NullValue;
            _tokenType = JsonTokenType.False;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx == bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 9 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": null'
            int bytesNeeded = propertyName.Length + 9 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.NullValue;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx == bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteBoolean(string propertyName, bool value)
            => WriteBoolean(propertyName.AsSpan(), value);

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            WriteBooleanWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteBooleanWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            if (_options.SlowPath)
                WriteBooleanSlowWithEncoding(propertyName, value);
            else
                WriteBooleanFastWithEncoding(propertyName, value);

            _indent |= 1 << 31;
        }

        private void WriteBooleanFastWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 9 >= 0);

            // Calculated based on the following: ',"encoded propertyName":true' OR ',"encoded propertyName":false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 9;

            if (_indent >= 0)
                bytesNeeded--;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 10 - indent * 2 - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": true' OR ',\r\n  "encoded propertyName": false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 10 + indent * 2 + JsonWriterHelper.NewLineUtf8.Length;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx <= bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            if (_options.SlowPath)
                WriteBooleanSlow(propertyName, value);
            else
                WriteBooleanFast(propertyName, value);

            _indent |= 1 << 31;
        }

        private void WriteBooleanFast(ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 9 >= 0);

            // Calculated based on the following: ',"propertyName":true' OR ',"propertyName":false'
            int bytesNeeded = propertyName.Length + 9;

            if (_indent >= 0)
                bytesNeeded--;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx == bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 10 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": true' OR ',\r\n  "propertyName": false'
            int bytesNeeded = propertyName.Length + 10 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Debug.Assert(idx == bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteNumber(string propertyName, int value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            if (_options.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, int value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 15 >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 15;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 16 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 16 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Debug.Assert(idx <= bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Argument too large.");

            if (_options.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, int value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 15 >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 15;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 16 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 16 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Debug.Assert(idx <= bytesNeeded);

            _bufferWriter.Advance(idx);
        }

        // TODO: Consider re-factoring to reduce duplication
        private void WriteStringWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = EscapeStringValue(value);

            if (_options.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(propertyName, value);
            else
                WriteStringFastWithEncodingPropertyValue(propertyName, value);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 6;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out consumed, out written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = JsonConstants.Quote;

            status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out consumed, out written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 6;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 7 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 6;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowFormatException();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx <= bytesNeeded);

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
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize || value.Length > MaxCharacterTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Arguments too large.");

            WriteStringWithEncodingPropertyValue(MemoryMarshal.AsBytes(propertyName), MemoryMarshal.AsBytes(value));
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxCharacterTokenSize || value.Length > MaxTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Arguments too large.");

            WriteStringWithEncodingProperty(MemoryMarshal.AsBytes(propertyName), value);
        }
        public void WriteString(ReadOnlySpan<byte> propertyName, string value)
            => WriteString(propertyName, value.AsSpan());

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxTokenSize || value.Length > MaxCharacterTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Arguments too large.");

            WriteStringWithEncodingValue(propertyName, MemoryMarshal.AsBytes(value));
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > MaxTokenSize || value.Length > MaxTokenSize)
                JsonThrowHelper.ThrowJsonWriterException("Arguments too large.");

            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
            {
                //TODO: Add escaping.
                value = EscapeStringValue(value);
            }

            if (_options.SlowPath)
                WriteStringSlow(propertyName, value);
            else
                WriteStringFast(propertyName, value);

            _indent |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private ReadOnlySpan<byte> EscapeStringValue(ReadOnlySpan<byte> value)
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

            if (_indent >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx == bytesNeeded);

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
            int indent = _indent & RemoveFlagsBitMask;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - propertyName.Length - escapedValue.Length - 7 - indent * 2 >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "value"'
            int bytesNeeded = propertyName.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent * 2 + escapedValue.Length;

            if (_indent >= 0)
                bytesNeeded--;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_indent < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(byteBuffer, ref idx);

            WriteIndentation(byteBuffer, ref idx, indent);

            byteBuffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(byteBuffer.Slice(idx));
            idx += propertyName.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;
            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Debug.Assert(idx == bytesNeeded);

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
        private void WriteIndentation(Span<byte> byteBuffer, ref int idx, int indent)
        {
            while (indent-- > 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }
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
