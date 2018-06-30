﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref struct JsonWriter<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        private readonly bool _prettyPrint;
        private BufferWriter<TBufferWriter> _bufferWriter;

        // The highest order bit of _indent is used to discern whether we are writing the first item in a list or not.
        // if (_indent >> 31) == 1, add a list separator before writing the item
        // else, no list separator is needed since we are writing the first item.
        private int _indent;

        private const int RemoveFlagsBitMask = 0x7FFFFFFF;

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public JsonWriter(TBufferWriter bufferWriter, bool prettyPrint = false)
        {
            _bufferWriter = BufferWriter.Create(bufferWriter);
            _prettyPrint = prettyPrint;
            _indent = 0;
        }

        public void Flush() => _bufferWriter.Flush();

        /// <summary>
        /// Write the starting tag of an object. This is used for adding an object to an
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteObjectStart()
        {
            if (_prettyPrint)
            {
                WriteStartUtf8Pretty(JsonConstants.OpenBrace);
            }
            else
            {
                int bytesNeeded = 1;

                if (_indent < 0)
                {
                    bytesNeeded = 2;
                }

                _bufferWriter.Ensure(bytesNeeded);
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                if (_indent < 0)
                {
                    byteBuffer[0] = JsonConstants.ListSeperator;
                }

                byteBuffer[bytesNeeded - 1] = JsonConstants.OpenBrace;
                _bufferWriter.Advance(bytesNeeded);
            }

            _indent &= RemoveFlagsBitMask;
            _indent++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteStartUtf8Pretty(byte token)
        {
            int indent = _indent & RemoveFlagsBitMask;

            int bytesNeeded = 1 + indent * 2;

            if (_indent < 0)
            {
                bytesNeeded++;
            }

            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            while (indent-- > 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }

            byteBuffer[idx] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        private void WriteStartUtf8(int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            if (_indent < 0)
            {
                byteBuffer[0] = JsonConstants.ListSeperator;
            }

            byteBuffer[bytesNeeded - 1] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write the starting tag of an object. This is used for adding an object to a
        /// nested object. If this is used while inside a nested array, the property
        /// name will be written and result in invalid JSON.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteObjectStart(string name)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());

            if (_prettyPrint)
            {
                WriteStartUtf8Pretty(nameSpan, JsonConstants.OpenBrace);
            }
            else
            {
                while (!TryWriteStartUtf8(nameSpan, JsonConstants.OpenBrace))
                    EnsureBuffer();
            }

            _indent &= RemoveFlagsBitMask;
            _indent++;

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteStartUtf8Pretty(ReadOnlySpan<byte> nameSpanByte, byte token)
        {
            // quote {name} quote colon open-brace, hence 4
            int bytesNeeded = 4;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            if (Encodings.Utf16.ToUtf8Length(nameSpanByte, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededValue;

            int indent = _indent & RemoveFlagsBitMask;

            // For the new line, \r\n or \n, and the space after the colon
            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + 1 + indent * 2;

            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            // \r\n versus \n, depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
            {
                byteBuffer[idx++] = JsonConstants.CarriageReturn;
            }

            byteBuffer[idx++] = JsonConstants.LineFeed;

            while (indent-- > 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == nameSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        private void WriteStartUtf8Pretty(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded, byte token)
        {
            int indent = _indent & RemoveFlagsBitMask;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            // \r\n versus \n, depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
            {
                byteBuffer[idx++] = JsonConstants.CarriageReturn;
            }

            byteBuffer[idx++] = JsonConstants.LineFeed;

            while (indent-- > 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == nameSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        private bool TryWriteStartUtf8(ReadOnlySpan<byte> nameSpanByte, byte token)
        {
            int idx = 0;

            try
            {
                Span<byte> byteBuffer = _bufferWriter.Buffer;
                if (_indent < 0)
                {
                    byteBuffer[idx++] = JsonConstants.ListSeperator;
                }

                byteBuffer[idx++] = JsonConstants.Quote;

                OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
                if (status == OperationStatus.DestinationTooSmall) return false;
                if (status != OperationStatus.Done)
                {
                    JsonThrowHelper.ThrowFormatException();
                }
                Debug.Assert(consumed == nameSpanByte.Length);
                idx += written;

                byteBuffer[idx++] = JsonConstants.Quote;

                byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

                byteBuffer[idx++] = token;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            _bufferWriter.Advance(idx);
            return true;
        }

        private void WriteStartUtf8(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == nameSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
        }

        /// <summary>
        /// Writes the end tag for an object.
        /// </summary>
        public void WriteObjectEnd()
        {
            _indent |= 1 << 31;
            _indent--;

            if (_prettyPrint)
            {
                WriteEndUtf8Pretty(JsonConstants.CloseBrace);
            }
            else
            {
                _bufferWriter.Ensure(1);
                Span<byte> byteBuffer = _bufferWriter.Buffer;
                byteBuffer[0] = JsonConstants.CloseBrace;
                _bufferWriter.Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteEndUtf8Pretty(byte token)
        {
            int indent = _indent & RemoveFlagsBitMask;

            // For the new line, \r\n or \n + indentation + }
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent * 2;

            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            // \r\n versus \n, depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
            {
                byteBuffer[idx++] = JsonConstants.CarriageReturn;
            }

            byteBuffer[idx++] = JsonConstants.LineFeed;

            while (indent-- > 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }

            byteBuffer[idx] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write the starting tag of an array. This is used for adding an array to a nested
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteArrayStart()
        {
            WriteStartUtf8(CalculateStartBytesNeeded(sizeof(byte)), JsonConstants.OpenBracket);

            _indent &= RemoveFlagsBitMask;
            _indent++;
        }

        /// <summary>
        /// Write the starting tag of an array. This is used for adding an array to a
        /// nested object. If this is used while inside a nested array, the property
        /// name will be written and result in invalid JSON.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteArrayStart(string name)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());

            if (_prettyPrint)
            {
                WriteStartUtf8Pretty(nameSpan, JsonConstants.OpenBracket);
            }
            else
            {
                while (!TryWriteStartUtf8(nameSpan, JsonConstants.OpenBracket))
                    EnsureBuffer();
            }

            _indent &= RemoveFlagsBitMask;
            _indent++;
        }

        /// <summary>
        /// Writes the end tag for an array.
        /// </summary>
        public void WriteArrayEnd()
        {
            _indent |= 1 << 31;
            _indent--;

            if (_prettyPrint)
            {
                WriteEndUtf8Pretty(JsonConstants.CloseBracket);
            }
            else
            {
                _bufferWriter.Ensure(1);
                Span<byte> byteBuffer = _bufferWriter.Buffer;
                byteBuffer[0] = JsonConstants.CloseBracket;
                _bufferWriter.Advance(1);
            }
        }

        /// <summary>
        /// Write a quoted string value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteAttribute(string name, string value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            ReadOnlySpan<byte> valueSpan = MemoryMarshal.AsBytes(value.AsSpan());

            if (_prettyPrint)
            {
                WriteAttributeUtf8Pretty(nameSpan, valueSpan);
            }
            else
            {
                while (!TryWriteAttributeUtf8(nameSpan, valueSpan))
                    EnsureBuffer();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteAttributeUtf8Pretty(ReadOnlySpan<byte> nameSpan, ReadOnlySpan<byte> valueSpan)
        {
            //quote {name} quote colon quote {value} quote, hence 5
            int bytesNeeded = 5;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededName) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            if (Encodings.Utf16.ToUtf8Length(valueSpan, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededName;
            bytesNeeded += bytesNeededValue;

            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + 1 + (_indent & RemoveFlagsBitMask) * 2;
            WriteAttributeUtf8Pretty(nameSpan, valueSpan, bytesNeeded);
        }

        private void WriteAttributeUtf8Pretty(ReadOnlySpan<byte> nameSpanByte, ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == nameSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = JsonConstants.Quote;

            status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out consumed, out written);
            Debug.Assert(consumed == valueSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
        }

        private bool TryWriteAttributeUtf8(ReadOnlySpan<byte> nameSpanByte, ReadOnlySpan<byte> valueSpanByte)
        {
            int idx = 0;
            try
            {
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                if (_indent < 0)
                {
                    byteBuffer[idx++] = JsonConstants.ListSeperator;
                }

                byteBuffer[idx++] = JsonConstants.Quote;

                OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
                if (status == OperationStatus.DestinationTooSmall) return false;
                if (status != OperationStatus.Done)
                {
                    JsonThrowHelper.ThrowFormatException();
                }
                Debug.Assert(consumed == nameSpanByte.Length);
                idx += written;

                byteBuffer[idx++] = JsonConstants.Quote;

                byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

                byteBuffer[idx++] = JsonConstants.Quote;

                status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out consumed, out written);
                if (status == OperationStatus.DestinationTooSmall) return false;
                if (status != OperationStatus.Done)
                {
                    JsonThrowHelper.ThrowFormatException();
                }
                Debug.Assert(consumed == valueSpanByte.Length);
                idx += written;

                byteBuffer[idx++] = JsonConstants.Quote;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
            return true;
        }

        /// <summary>
        /// Write a signed integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, long value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            if (_prettyPrint)
            {
                WriteAttributeUtf8Pretty(nameSpan, value);
            }
            else
            {
                while (!TryWriteAttributeUtf8(nameSpan, value))
                    EnsureBuffer();
            }
        }

        private bool TryWriteAttributeUtf8(ReadOnlySpan<byte> nameSpanByte, long value)
        {
            int idx = 0;
            try
            {
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                if (_indent < 0)
                {
                    byteBuffer[idx++] = JsonConstants.ListSeperator;
                }

                byteBuffer[idx++] = JsonConstants.Quote;

                OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
                if (status == OperationStatus.DestinationTooSmall) return false;
                if (status != OperationStatus.Done)
                {
                    JsonThrowHelper.ThrowFormatException();
                }
                Debug.Assert(consumed == nameSpanByte.Length);
                idx += written;

                byteBuffer[idx++] = JsonConstants.Quote;

                byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

                if (!JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten)) return false;
                // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
                // See: https://github.com/dotnet/corefx/issues/25425
                //if (!Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten)) return false;
                idx += bytesWritten;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteAttributeUtf8Pretty(ReadOnlySpan<byte> nameSpan, long value)
        {
            // quote {name} quote colon, hence 3
            int bytesNeeded = 3;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededName) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededName;

            // For the new line, \r\n or \n, and the space after the colon
            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + 1 + (_indent & RemoveFlagsBitMask) * 2;

            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += 1;
            }

            int digitCount = JsonWriterHelper.CountDigits((ulong)value);
            bytesNeeded += digitCount;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpan, byteBuffer.Slice(idx), out int consumed, out int written);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }
            Debug.Assert(consumed == nameSpan.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            byteBuffer[idx++] = JsonConstants.Space;

            _indent |= 1 << 31;

            if (insertNegationSign)
            {
                byteBuffer[idx++] = (byte)'-';
            }

            JsonWriterHelper.WriteDigitsUInt64D((ulong)value, byteBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write an unsigned integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, ulong value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf8
        }

        /// <summary>
        /// Write a boolean value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteAttribute(string name, bool value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            if (value)
            {
                WriteJsonValueUtf8(JsonConstants.TrueValue);
            }
            else
            {
                WriteJsonValueUtf8(JsonConstants.FalseValue);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTime value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            WriteDateTime(value);
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTimeOffset value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            WriteDateTimeOffset(value);
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, Guid value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteAttributeNull(string name)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttributeUtf8(nameSpan, bytesNeeded);
            WriteJsonValueUtf8(JsonConstants.NullValue);
        }

        private void WriteAttributeUtf8(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            if (_prettyPrint)
            {
                idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));
            }

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(nameSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == nameSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            byteBuffer[idx++] = JsonConstants.KeyValueSeperator;

            if (_prettyPrint)
            {
                byteBuffer[idx++] = JsonConstants.Space;
            }

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
        }

        /// <summary>
        /// Writes a quoted string value into the current array.
        /// </summary>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteValue(string value)
        {
            ReadOnlySpan<byte> valueSpan = MemoryMarshal.AsBytes(value.AsSpan());

            if (_prettyPrint)
            {
                WriteValueUtf8Pretty(valueSpan);
            }
            else
            {
                while (!TryWriteValueUtf8(valueSpan))
                    EnsureBuffer();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteValueUtf8Pretty(ReadOnlySpan<byte> valueSpanByte)
        {
            int bytesNeeded = CalculateValueBytesNeededPretty(valueSpanByte);
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }
            Debug.Assert(consumed == valueSpanByte.Length);

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
        }

        private bool TryWriteValueUtf8(ReadOnlySpan<byte> valueSpanByte)
        {
            int idx = 0;

            try
            {
                Span<byte> byteBuffer = _bufferWriter.Buffer;

                if (_indent < 0)
                {
                    byteBuffer[idx++] = JsonConstants.ListSeperator;
                }

                byteBuffer[idx++] = JsonConstants.Quote;

                OperationStatus status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
                if (status == OperationStatus.DestinationTooSmall) return false;
                if (status != OperationStatus.Done)
                {
                    JsonThrowHelper.ThrowFormatException();
                }
                Debug.Assert(consumed == valueSpanByte.Length);

                idx += written;

                byteBuffer[idx++] = JsonConstants.Quote;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
            return true;
        }

        private void WriteValueUtf8(ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }
            Debug.Assert(consumed == valueSpanByte.Length);

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
        }

        /// <summary>
        /// Write a signed integer value into the current array.
        /// </summary>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteValue(long value)
        {
            if (_prettyPrint)
            {
                WriteValueUtf8Pretty(value);
            }
            else
            {
                while (!TryWriteValueUtf8(value))
                    EnsureBuffer();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteValueUtf8Pretty(long value)
        {
            int bytesNeeded = 0;
            if (_indent < 0)
            {
                bytesNeeded = 1;
            }
            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + (_indent & RemoveFlagsBitMask) * 2;

            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += 1;
            }

            int digitCount = JsonWriterHelper.CountDigits((ulong)value);
            bytesNeeded += digitCount;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            _indent |= 1 << 31;

            idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            if (insertNegationSign)
            {
                byteBuffer[idx++] = (byte)'-';
            }

            JsonWriterHelper.WriteDigitsUInt64D((ulong)value, byteBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryWriteValueUtf8(long value)
        {
            int idx = 0;

            Span<byte> byteBuffer = _bufferWriter.Buffer;
            if (_indent < 0)
            {
                if (byteBuffer.Length == 0) return false;
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            if (!JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten)) return false;
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            //if (!Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten)) return false;
            idx += bytesWritten;

            _bufferWriter.Advance(idx);
            _indent |= 1 << 31;
            return true;
        }

        private void WriteValueUtf8(long value, int bytesNeeded)
        {
            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += 1;
            }

            int digitCount = JsonWriterHelper.CountDigits((ulong)value);
            bytesNeeded += digitCount;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (_indent < 0)
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            _indent |= 1 << 31;

            if (insertNegationSign)
            {
                byteBuffer[idx++] = (byte)'-';
            }

            JsonWriterHelper.WriteDigitsUInt64D((ulong)value, byteBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write a unsigned integer value into the current array.
        /// </summary>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteValue(ulong value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf8();
            _indent |= 1 << 31;
            WriteSpacingUtf8();

            WriteNumber(value);
        }

        /// <summary>
        /// Write a boolean value into the current array.
        /// </summary>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteValue(bool value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf8();
            _indent |= 1 << 31;
            WriteSpacingUtf8();

            if (value)
            {
                WriteJsonValueUtf8(JsonConstants.TrueValue);
            }
            else
            {
                WriteJsonValueUtf8(JsonConstants.FalseValue);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteValue(DateTime value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf8();
            _indent |= 1 << 31;
            WriteSpacingUtf8();

            WriteDateTime(value);
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteValue(DateTimeOffset value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf8();
            _indent |= 1 << 31;
            WriteSpacingUtf8();

            WriteDateTimeOffset(value);
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteValue(Guid value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf8();
            _indent |= 1 << 31;
            WriteSpacingUtf8();

            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value into the current array.
        /// </summary>
        public void WriteNull()
        {
            int bytesNeeded = (_indent >= 0 ? 0 : 1) + (_prettyPrint ? 2 + _indent * 2 : 0) + JsonConstants.NullValue.Length;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;
            if (_indent >= 0)
            {
                _indent |= 1 << 31;
            }
            else
            {
                byteBuffer[idx++] = JsonConstants.ListSeperator;
            }

            if (_prettyPrint)
            {
                int indent = _indent & RemoveFlagsBitMask;
                byteBuffer[idx++] = JsonConstants.CarriageReturn;
                byteBuffer[idx++] = JsonConstants.LineFeed;

                while (indent-- > 0)
                {
                    byteBuffer[idx++] = JsonConstants.Space;
                    byteBuffer[idx++] = JsonConstants.Space;
                }
            }

            Debug.Assert(byteBuffer.Slice(idx).Length >= JsonConstants.NullValue.Length);
            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));

            _bufferWriter.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumberUtf8(long value)
        {
            //TODO: Optimize, this is too slow
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.NumberFormat, SymbolTable.InvariantUtf8))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumber(ulong value)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.NumberFormat, SymbolTable.InvariantUtf8))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDateTime(DateTime value)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.DateTimeFormat, SymbolTable.InvariantUtf8))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDateTimeOffset(DateTimeOffset value)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.DateTimeFormat, SymbolTable.InvariantUtf8))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteGuid(Guid value)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.GuidFormat, SymbolTable.InvariantUtf8))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteJsonValueUtf8(ReadOnlySpan<byte> values)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!SymbolTable.InvariantUtf8.TryEncode(values, buffer, out int consumed, out written))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteControlUtf8(byte value)
        {
            MemoryMarshal.GetReference(EnsureBuffer(1)) = value;
            _bufferWriter.Advance(1);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperatorUtf8()
        {
            if (_indent >= 0)
            {
                return;
            }

            WriteControlUtf8(JsonConstants.ListSeperator);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingUtf8(bool newline = true)
        {
            if (!_prettyPrint)
            {
                return;
            }

            int indent = _indent & RemoveFlagsBitMask;
            int bytesNeeded = newline ? 2 : 0;
            bytesNeeded += indent * 2;

            Span<byte> buffer = EnsureBuffer(bytesNeeded);
            ref byte utf8Bytes = ref MemoryMarshal.GetReference(buffer);
            int idx = 0;

            if (newline)
            {
                Unsafe.Add(ref utf8Bytes, idx++) = JsonConstants.CarriageReturn;
                Unsafe.Add(ref utf8Bytes, idx++) = JsonConstants.LineFeed;
            }

            while (indent-- >= 0)
            {
                Unsafe.Add(ref utf8Bytes, idx++) = JsonConstants.Space;
                Unsafe.Add(ref utf8Bytes, idx++) = JsonConstants.Space;
            }

            _bufferWriter.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> EnsureBuffer(int needed = 256)
        {
            _bufferWriter.Ensure(needed);
            Span<byte> buffer = _bufferWriter.Buffer;
            if (buffer.Length < needed)
            {
                JsonThrowHelper.ThrowOutOfMemoryException();
            }

            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateStartBytesNeeded(int numBytes)
        {
            int bytesNeeded = numBytes;

            if (_indent < 0)
            {
                bytesNeeded *= 2;
            }

            return bytesNeeded;
        }

        private int CalculateValueBytesNeededPretty(ReadOnlySpan<byte> span)
        {
            int bytesNeeded = 2;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            // For the new line, \r\n or \n
            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + (_indent & RemoveFlagsBitMask) * 2;

            if (Encodings.Utf16.ToUtf8Length(span, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateValueBytesNeeded(ReadOnlySpan<byte> span)
        {
            int bytesNeeded = 2;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            if (Encodings.Utf16.ToUtf8Length(span, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateStartAttributeBytesNeededPretty(ReadOnlySpan<byte> nameSpan)
        {
            // quote {name} quote colon, hence 3
            int bytesNeeded = 3;
            if (_indent < 0)
            {
                bytesNeeded++;
            }

            // For the new line, \r\n or \n, and the space after the colon
            bytesNeeded += JsonWriterHelper.NewLineUtf8.Length + 1  + (_indent & RemoveFlagsBitMask) * 2;

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateStartAttributeBytesNeeded(ReadOnlySpan<byte> nameSpan, int numBytes)
        {
            int bytesNeeded = 0;
            if (_indent < 0)
            {
                bytesNeeded = numBytes;
            }

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = JsonWriterHelper.NewLineUtf8.Length + 1;    // For the new line, \r\n or \n, and the space after the colon
                bytesNeededForPrettyPrint += _indent * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * 3;    // quote {name} quote colon, hence 3

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddNewLineAndIndentation(Span<byte> buffer)
        {
            int offset = 0;
            // \r\n versus \n, depending on OS
            if (JsonWriterHelper.NewLineUtf8.Length == 2)
            {
                buffer[offset++] = JsonConstants.CarriageReturn;
            }

            buffer[offset++] = JsonConstants.LineFeed;

            int indent = _indent & RemoveFlagsBitMask;

            while (indent-- > 0)
            {
                buffer[offset++] = JsonConstants.Space;
                buffer[offset++] = JsonConstants.Space;
            }
            return offset;
        }
    }
}
