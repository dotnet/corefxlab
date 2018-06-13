// Copyright (c) Microsoft. All rights reserved.
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
        private static readonly byte[] s_newLineUtf8 = Encoding.UTF8.GetBytes(Environment.NewLine);
        private static readonly char[] s_newLineUtf16 = Environment.NewLine.ToCharArray();

        private readonly bool _prettyPrint;
        private BufferWriter<TBufferWriter> _bufferWriter;
        private readonly bool _isUtf8;

        private int _indent;
        private bool _firstItem;

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public JsonWriter(BufferWriter<TBufferWriter> bufferWriter, bool isUtf8, bool prettyPrint = false)
        {
            _bufferWriter = bufferWriter;
            _prettyPrint = prettyPrint;
            _isUtf8 = isUtf8;

            _indent = -1;
            _firstItem = true;
        }

        /// <summary>
        /// Write the starting tag of an object. This is used for adding an object to an
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteObjectStart()
        {
            if (_isUtf8)
            {
                WriteStartUtf8(CalculateStartBytesNeeded(sizeof(byte)), JsonConstants.OpenBrace);
            }
            else
            {
                WriteStartUtf16(CalculateStartBytesNeeded(sizeof(char)), JsonConstants.OpenBrace);
            }

            _firstItem = true;
            _indent++;
        }

        private void WriteStartUtf8(int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
            {
                int indent = _indent;

                while (indent-- >= 0)
                {
                    byteBuffer[idx++] = JsonConstants.Space;
                    byteBuffer[idx++] = JsonConstants.Space;
                }
            }

            byteBuffer[idx] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        private void WriteStartUtf16(int bytesNeeded, byte token)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));

            int idx = 0;
            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
            {
                int indent = _indent;

                while (indent-- >= 0)
                {
                    charBuffer[idx++] = (char)JsonConstants.Space;
                    charBuffer[idx++] = (char)JsonConstants.Space;
                }
            }

            charBuffer[idx] = (char)token;
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
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(byte), 4);  // quote {name} quote colon open-brace, hence 4
                WriteStartUtf8(nameSpan, bytesNeeded, JsonConstants.OpenBrace);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(char), 4);  // quote {name} quote colon open-brace, hence 4
                WriteStartUtf16(nameSpan, bytesNeeded, JsonConstants.OpenBrace);
            }

            _firstItem = true;
            _indent++;
        }

        private void WriteStartUtf8(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
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

            if (_prettyPrint)
                byteBuffer[idx++] = JsonConstants.Space;

            byteBuffer[idx++] = token;

            _bufferWriter.Advance(idx);
            _firstItem = false;
        }

        private void WriteStartUtf16(ReadOnlySpan<char> nameSpanChar, int bytesNeeded, byte token)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));
            int idx = 0;

            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            charBuffer[idx++] = (char)JsonConstants.Quote;

            nameSpanChar.CopyTo(charBuffer.Slice(idx));

            idx += nameSpanChar.Length;

            charBuffer[idx++] = (char)JsonConstants.Quote;

            charBuffer[idx++] = (char)JsonConstants.KeyValueSeperator;

            if (_prettyPrint)
                charBuffer[idx++] = (char)JsonConstants.Space;

            charBuffer[idx] = (char)token;

            _bufferWriter.Advance(bytesNeeded);
            _firstItem = false;
        }

        /// <summary>
        /// Writes the end tag for an object.
        /// </summary>
        public void WriteObjectEnd()
        {
            _firstItem = false;
            _indent--;
            if (_isUtf8)
            {
                WriteEndUtf8(CalculateEndBytesNeeded(sizeof(byte), s_newLineUtf8.Length), JsonConstants.CloseBrace);
            }
            else
            {
                WriteEndUtf16(CalculateEndBytesNeeded(sizeof(char), s_newLineUtf16.Length), JsonConstants.CloseBrace);
            }
        }

        private void WriteEndUtf8(int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            byteBuffer[idx] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        private void WriteEndUtf16(int bytesNeeded, byte token)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));
            int idx = 0;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            charBuffer[idx] = (char)token;
            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write the starting tag of an array. This is used for adding an array to a nested
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteArrayStart()
        {
            if (_isUtf8)
            {
                WriteStartUtf8(CalculateStartBytesNeeded(sizeof(byte)), JsonConstants.OpenBracket);
            }
            else
            {
                WriteStartUtf16(CalculateStartBytesNeeded(sizeof(char)), JsonConstants.OpenBracket);
            }

            _firstItem = true;
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
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(byte), 4);
                WriteStartUtf8(nameSpan, bytesNeeded, JsonConstants.OpenBracket);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(char), 4);
                WriteStartUtf16(nameSpan, bytesNeeded, JsonConstants.OpenBracket);
            }

            _firstItem = true;
            _indent++;
        }

        /// <summary>
        /// Writes the end tag for an array.
        /// </summary>
        public void WriteArrayEnd()
        {
            _firstItem = false;
            _indent--;

            if (_isUtf8)
            {
                WriteEndUtf8(CalculateEndBytesNeeded(sizeof(byte), s_newLineUtf8.Length), JsonConstants.CloseBracket);
            }
            else
            {
                WriteEndUtf16(CalculateEndBytesNeeded(sizeof(char), s_newLineUtf16.Length), JsonConstants.CloseBracket);
            }
        }

        /// <summary>
        /// Write a quoted string value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteAttribute(string name, string value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                ReadOnlySpan<byte> valueSpan = MemoryMarshal.AsBytes(value.AsSpan());
                int bytesNeeded = CalculateAttributeBytesNeeded(nameSpan, valueSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, valueSpan, bytesNeeded);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                ReadOnlySpan<char> valueSpan = value.AsSpan();
                int bytesNeeded = CalculateAttributeBytesNeeded(nameSpan, valueSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, valueSpan, bytesNeeded);
            }
        }

        private void WriteAttributeUtf8(ReadOnlySpan<byte> nameSpanByte, ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
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

            if (_prettyPrint)
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
            _firstItem = false;
        }

        private void WriteAttributeUtf16(ReadOnlySpan<char> nameSpanChar, ReadOnlySpan<char> valueSpanChar, int bytesNeeded)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));
            int idx = 0;

            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            charBuffer[idx++] = (char)JsonConstants.Quote;

            nameSpanChar.CopyTo(charBuffer.Slice(idx));

            idx += nameSpanChar.Length;

            charBuffer[idx++] = (char)JsonConstants.Quote;

            charBuffer[idx++] = (char)JsonConstants.KeyValueSeperator;

            if (_prettyPrint)
                charBuffer[idx++] = (char)JsonConstants.Space;

            charBuffer[idx++] = (char)JsonConstants.Quote;

            valueSpanChar.CopyTo(charBuffer.Slice(idx));

            charBuffer[idx + valueSpanChar.Length] = (char)JsonConstants.Quote;

            _bufferWriter.Advance(bytesNeeded);
            _firstItem = false;
        }

        /// <summary>
        /// Write a signed integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, long value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteNumberUtf8(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf8
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteNumberUtf16(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf16
            }
        }

        /// <summary>
        /// Write an unsigned integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, ulong value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf8
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf16
            }
        }

        /// <summary>
        /// Write a boolean value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteAttribute(string name, bool value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                if (value)
                    WriteJsonValueUtf8(JsonConstants.TrueValue);
                else
                    WriteJsonValueUtf8(JsonConstants.FalseValue);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                if (value)
                    WriteJsonValueUtf16(JsonConstants.TrueValue);
                else
                    WriteJsonValueUtf16(JsonConstants.FalseValue);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTime value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteDateTime(value);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteDateTime(value);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTimeOffset value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteDateTimeOffset(value);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteDateTimeOffset(value);
            }
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, Guid value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteGuid(value);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteGuid(value);
            }
        }

        /// <summary>
        /// Write a null value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteAttributeNull(string name)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
                WriteAttributeUtf8(nameSpan, bytesNeeded);
                WriteJsonValueUtf8(JsonConstants.NullValue);
            }
            else
            {
                ReadOnlySpan<char> nameSpan = name.AsSpan();
                int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
                WriteAttributeUtf16(nameSpan, bytesNeeded);
                WriteJsonValueUtf16(JsonConstants.NullValue);
            }
        }

        private void WriteAttributeUtf8(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
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

            if (_prettyPrint)
                byteBuffer[idx++] = JsonConstants.Space;

            _bufferWriter.Advance(idx);
            _firstItem = false;
        }

        private void WriteAttributeUtf16(ReadOnlySpan<char> nameSpanChar, int bytesNeeded)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));
            int idx = 0;

            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            charBuffer[idx++] = (char)JsonConstants.Quote;

            nameSpanChar.CopyTo(charBuffer.Slice(idx));

            idx += nameSpanChar.Length;

            charBuffer[idx++] = (char)JsonConstants.Quote;

            charBuffer[idx++] = (char)JsonConstants.KeyValueSeperator;

            if (_prettyPrint)
                charBuffer[idx] = (char)JsonConstants.Space;

            _bufferWriter.Advance(bytesNeeded);
            _firstItem = false;
        }

        /// <summary>
        /// Writes a quoted string value into the current array.
        /// </summary>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteValue(string value)
        {
            if (_isUtf8)
            {
                ReadOnlySpan<byte> valueSpan = MemoryMarshal.AsBytes(value.AsSpan());
                int bytesNeeded = CalculateValueBytesNeeded(valueSpan, sizeof(byte), 2);
                WriteValueUtf8(valueSpan, bytesNeeded);
            }
            else
            {
                ReadOnlySpan<char> valueSpan = value.AsSpan();
                int bytesNeeded = CalculateValueBytesNeeded(valueSpan, sizeof(char), 2);
                WriteValueUtf16(valueSpan, bytesNeeded);
            }
        }

        private void WriteValueUtf8(ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            byteBuffer[idx++] = JsonConstants.Quote;


            OperationStatus status = Encodings.Utf16.ToUtf8(valueSpanByte, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(consumed == valueSpanByte.Length);
            if (status != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowFormatException();
            }

            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            _bufferWriter.Advance(idx);
            _firstItem = false;
        }

        private void WriteValueUtf16(ReadOnlySpan<char> valueSpanChar, int bytesNeeded)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));
            int idx = 0;

            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            charBuffer[idx++] = (char)JsonConstants.Quote;

            valueSpanChar.CopyTo(charBuffer.Slice(idx));

            idx += valueSpanChar.Length;

            charBuffer[idx] = (char)JsonConstants.Quote;

            _bufferWriter.Advance(bytesNeeded);
            _firstItem = false;
        }

        /// <summary>
        /// Write a signed integer value into the current array.
        /// </summary>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteValue(long value)
        {
            if (_isUtf8)
            {
                WriteValueUtf8(value, CalculateValueBytesNeeded(sizeof(byte), s_newLineUtf8.Length));
            }
            else
            {
                WriteValueUtf16(value, CalculateValueBytesNeeded(sizeof(char), s_newLineUtf16.Length));
            }
        }

        private void WriteValueUtf8(long value, int bytesNeeded)
        {
            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += sizeof(byte);
            }

            int digitCount = CountDigits((ulong)value);
            bytesNeeded += sizeof(byte) * digitCount;
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            _firstItem = false;
            if (_prettyPrint)
                idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

            if (insertNegationSign)
                byteBuffer[idx++] = (byte)'-';

            WriteDigitsUInt64D((ulong)value, byteBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        private void WriteValueUtf16(long value, int bytesNeeded)
        {
            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += sizeof(char);
            }

            int digitCount = CountDigits((ulong)value);
            bytesNeeded += sizeof(char) * digitCount;
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));

            int idx = 0;
            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            _firstItem = false;
            if (_prettyPrint)
                idx += AddNewLineAndIndentation(charBuffer.Slice(idx));

            if (insertNegationSign)
                charBuffer[idx++] = '-';

            WriteDigitsUInt64D((ulong)value, charBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigitsUInt64D(ulong value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigitsUInt64D(ulong value, Span<char> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (char)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountDigits(ulong value)
        {
            int digits = 1;
            uint part;
            if (value >= 10000000)
            {
                if (value >= 100000000000000)
                {
                    part = (uint)(value / 100000000000000);
                    digits += 14;
                }
                else
                {
                    part = (uint)(value / 10000000);
                    digits += 7;
                }
            }
            else
            {
                part = (uint)value;
            }

            if (part < 10)
            {
                // no-op
            }
            else if (part < 100)
            {
                digits += 1;
            }
            else if (part < 1000)
            {
                digits += 2;
            }
            else if (part < 10000)
            {
                digits += 3;
            }
            else if (part < 100000)
            {
                digits += 4;
            }
            else if (part < 1000000)
            {
                digits += 5;
            }
            else
            {
                Debug.Assert(part < 10000000);
                digits += 6;
            }

            return digits;
        }

        /// <summary>
        /// Write a unsigned integer value into the current array.
        /// </summary>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteValue(ulong value)
        {
            if (_isUtf8)
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf8();
                _firstItem = false;
                WriteSpacingUtf8();

                WriteNumber(value);
            }
            else
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf16();
                _firstItem = false;
                WriteSpacingUtf16();

                WriteNumber(value);
            }
        }

        /// <summary>
        /// Write a boolean value into the current array.
        /// </summary>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteValue(bool value)
        {
            if (_isUtf8)
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf8();
                _firstItem = false;
                WriteSpacingUtf8();

                if (value)
                    WriteJsonValueUtf8(JsonConstants.TrueValue);
                else
                    WriteJsonValueUtf8(JsonConstants.FalseValue);
            }
            else
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf16();
                _firstItem = false;
                WriteSpacingUtf16();

                if (value)
                    WriteJsonValueUtf16(JsonConstants.TrueValue);
                else
                    WriteJsonValueUtf16(JsonConstants.FalseValue);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteValue(DateTime value)
        {
            if (_isUtf8)
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf8();
                _firstItem = false;
                WriteSpacingUtf8();

                WriteDateTime(value);
            }
            else
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf16();
                _firstItem = false;
                WriteSpacingUtf16();

                WriteDateTime(value);
            }
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteValue(DateTimeOffset value)
        {
            if (_isUtf8)
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf8();
                _firstItem = false;
                WriteSpacingUtf8();

                WriteDateTimeOffset(value);
            }
            else
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf16();
                _firstItem = false;
                WriteSpacingUtf16();

                WriteDateTimeOffset(value);
            }
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteValue(Guid value)
        {
            if (_isUtf8)
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf8();
                _firstItem = false;
                WriteSpacingUtf8();

                WriteGuid(value);
            }
            else
            {
                //TODO: Optimize, just like WriteValue(long value)
                WriteItemSeperatorUtf16();
                _firstItem = false;
                WriteSpacingUtf16();

                WriteGuid(value);
            }
        }

        /// <summary>
        /// Write a null value into the current array.
        /// </summary>
        public void WriteNull()
        {
            if (_isUtf8)
            {
                int bytesNeeded = (_firstItem ? 0 : 1) + (_prettyPrint ? 2 + (_indent + 1) * 2 : 0) + JsonConstants.NullValue.Length;
                Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
                int idx = 0;
                if (_firstItem)
                {
                    _firstItem = false;
                }
                else
                {
                    byteBuffer[idx++] = JsonConstants.ListSeperator;
                }

                if (_prettyPrint)
                {
                    int indent = _indent;
                    byteBuffer[idx++] = JsonConstants.CarriageReturn;
                    byteBuffer[idx++] = JsonConstants.LineFeed;

                    while (indent-- >= 0)
                    {
                        byteBuffer[idx++] = JsonConstants.Space;
                        byteBuffer[idx++] = JsonConstants.Space;
                    }
                }

                Debug.Assert(byteBuffer.Slice(idx).Length >= JsonConstants.NullValue.Length);
                JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));

                _bufferWriter.Advance(bytesNeeded);
            }
            else
            {
                ReadOnlySpan<byte> nullLiteral = (BitConverter.IsLittleEndian ? JsonConstants.NullValueUtf16LE : JsonConstants.NullValueUtf16BE);
                int charsNeeded = (_firstItem ? 0 : 1) + (_prettyPrint ? 2 + (_indent + 1) * 2 : 0);
                int bytesNeeded = charsNeeded * 2 + nullLiteral.Length;
                Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
                Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(byteBuffer);
                int idx = 0;
                if (_firstItem)
                {
                    _firstItem = false;
                }
                else
                {
                    charBuffer[idx++] = (char)JsonConstants.ListSeperator;
                }

                if (_prettyPrint)
                {
                    int indent = _indent;
                    charBuffer[idx++] = (char)JsonConstants.CarriageReturn;
                    charBuffer[idx++] = (char)JsonConstants.LineFeed;

                    while (indent-- >= 0)
                    {
                        charBuffer[idx++] = (char)JsonConstants.Space;
                        charBuffer[idx++] = (char)JsonConstants.Space;
                    }
                }

                Debug.Assert(byteBuffer.Slice(idx * 2).Length >= nullLiteral.Length);
                nullLiteral.CopyTo(byteBuffer.Slice(idx * 2));

                _bufferWriter.Advance(bytesNeeded);
            }
        }

        public void Flush() => _bufferWriter.Flush();

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
        private void WriteNumberUtf16(long value)
        {
            //TODO: Optimize, this is too slow
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!CustomFormatter.TryFormat(value, buffer, out written, JsonConstants.NumberFormat, SymbolTable.InvariantUtf16))
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
        private void WriteJsonValueUtf16(ReadOnlySpan<byte> values)
        {
            Span<byte> buffer = _bufferWriter.Buffer;
            int written;
            while (!SymbolTable.InvariantUtf16.TryEncode(values, buffer, out int consumed, out written))
            {
                buffer = EnsureBuffer();
            }

            _bufferWriter.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteControlUtf8(byte value)
        {
            Span<byte> buffer = EnsureBuffer(1);
            MemoryMarshal.GetReference(buffer) = value;
            _bufferWriter.Advance(1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteControlUtf16(byte value)
        {
            Span<byte> buffer = EnsureBuffer(2);
            Unsafe.As<byte, char>(ref MemoryMarshal.GetReference(buffer)) = (char)value;
            _bufferWriter.Advance(2);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperatorUtf8()
        {
            if (_firstItem) return;

            WriteControlUtf8(JsonConstants.ListSeperator);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperatorUtf16()
        {
            if (_firstItem) return;

            WriteControlUtf16(JsonConstants.ListSeperator);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingUtf8(bool newline = true)
        {
            if (!_prettyPrint) return;

            var indent = _indent;
            var bytesNeeded = newline ? 2 : 0;
            bytesNeeded += (indent + 1) * 2;

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

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingUtf16(bool newline = true)
        {
            if (!_prettyPrint) return;

            var indent = _indent;
            var bytesNeeded = newline ? 2 : 0;
            bytesNeeded += (indent + 1) * 2;
            bytesNeeded *= sizeof(char);

            Span<byte> buffer = EnsureBuffer(bytesNeeded);
            var span = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(span);
            int idx = 0;

            if (newline)
            {
                Unsafe.Add(ref utf16Bytes, idx++) = (char)JsonConstants.CarriageReturn;
                Unsafe.Add(ref utf16Bytes, idx++) = (char)JsonConstants.LineFeed;
            }

            while (indent-- >= 0)
            {
                Unsafe.Add(ref utf16Bytes, idx++) = (char)JsonConstants.Space;
                Unsafe.Add(ref utf16Bytes, idx++) = (char)JsonConstants.Space;
            }

            _bufferWriter.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> EnsureBuffer(int needed = 256)
        {
            _bufferWriter.Ensure(needed);
            Span<byte> buffer = _bufferWriter.Buffer;
            if (buffer.Length < needed)
                JsonThrowHelper.ThrowOutOfMemoryException();
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateStartBytesNeeded(int numBytes)
        {
            int bytesNeeded = numBytes;

            if (!_firstItem)
                bytesNeeded *= 2;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }
            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateEndBytesNeeded(int numBytes, int newLineLength)
        {
            int bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = newLineLength;  // For the new line, \r\n or \n
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }
            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateValueBytesNeeded(int numBytes, int newLineLength)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = newLineLength;  // For the new line, \r\n or \n
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            return bytesNeeded;
        }

        private int CalculateValueBytesNeeded(ReadOnlySpan<byte> span, int numBytes, int extraCharacterCount)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf8.Length;    // For the new line, \r\n or \n
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * extraCharacterCount;

            if (Encodings.Utf16.ToUtf8Length(span, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentException("Invalid or incomplete UTF-8 string");
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateBytesNeeded(ReadOnlySpan<byte> span, int numBytes, int extraCharacterCount)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf8.Length + 1;    // For the new line, \r\n or \n, and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * extraCharacterCount;

            if (Encodings.Utf16.ToUtf8Length(span, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentException("Invalid or incomplete UTF-8 string");
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateStartAttributeBytesNeeded(ReadOnlySpan<byte> nameSpan, int numBytes)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf8.Length + 1;    // For the new line, \r\n or \n, and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * 3;    // quote {name} quote colon, hence 3

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentException("Invalid or incomplete UTF-8 string");
            }
            bytesNeeded += bytesNeededValue;
            return bytesNeeded;
        }

        private int CalculateAttributeBytesNeeded(ReadOnlySpan<byte> nameSpan, ReadOnlySpan<byte> valueSpan, int numBytes)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf8.Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * 5;    //quote {name} quote colon quote {value} quote, hence 5

            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededName) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentException("Invalid or incomplete UTF-8 string");
            }
            if (Encodings.Utf16.ToUtf8Length(nameSpan, out int bytesNeededValue) != OperationStatus.Done)
            {
                JsonThrowHelper.ThrowArgumentException("Invalid or incomplete UTF-8 string");
            }
            bytesNeeded += bytesNeededName;
            bytesNeeded += bytesNeededValue;

            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateValueBytesNeeded(ReadOnlySpan<char> span, int numBytes, int extraCharacterCount)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf16.Length;    // For the new line, \r\n or \n
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * extraCharacterCount;
            bytesNeeded += MemoryMarshal.AsBytes(span).Length;

            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateBytesNeeded(ReadOnlySpan<char> span, int numBytes, int extraCharacterCount)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf16.Length + 1;    // For the new line, \r\n or \n, and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * extraCharacterCount;
            bytesNeeded += MemoryMarshal.AsBytes(span).Length;

            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateStartAttributeBytesNeeded(ReadOnlySpan<char> nameSpan, int numBytes)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf16.Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * 3;    // quote {name} quote colon, hence 3
            bytesNeeded += MemoryMarshal.AsBytes(nameSpan).Length;

            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateAttributeBytesNeeded(ReadOnlySpan<char> nameSpan, ReadOnlySpan<char> valueSpan, int numBytes)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = s_newLineUtf16.Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
                bytesNeededForPrettyPrint += (_indent + 1) * 2;
                bytesNeeded += numBytes * bytesNeededForPrettyPrint;
            }

            bytesNeeded += numBytes * 5;    //quote {name} quote colon quote {value} quote, hence 5
            bytesNeeded += MemoryMarshal.AsBytes(nameSpan).Length;
            bytesNeeded += MemoryMarshal.AsBytes(valueSpan).Length;

            return bytesNeeded;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddNewLineAndIndentation(Span<byte> buffer)
        {
            int offset = 0;
            // \r\n versus \n, depending on OS
            if (s_newLineUtf8.Length == 2)
                buffer[offset++] = JsonConstants.CarriageReturn;

            buffer[offset++] = JsonConstants.LineFeed;

            int indent = _indent;

            while (indent-- >= 0)
            {
                buffer[offset++] = JsonConstants.Space;
                buffer[offset++] = JsonConstants.Space;
            }
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddNewLineAndIndentation(Span<char> buffer)
        {
            int offset = 0;
            // \r\n versus \n, depending on OS
            if (s_newLineUtf16.Length == 2)
                buffer[offset++] = (char)JsonConstants.CarriageReturn;

            buffer[offset++] = (char)JsonConstants.LineFeed;

            int indent = _indent;

            while (indent-- >= 0)
            {
                buffer[offset++] = (char)JsonConstants.Space;
                buffer[offset++] = (char)JsonConstants.Space;
            }

            return offset;
        }
    }

    //TODO: Move to a separate file
    public static class JsonWriter
    {
        public static JsonWriter<TBufferWriter> Create<TBufferWriter>(
            TBufferWriter bufferWriter,
            bool isUtf8,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriter<TBufferWriter>(BufferWriter.Create(bufferWriter), isUtf8, prettyPrint);
        }

        public static JsonWriter<TBufferWriter> Create<TBufferWriter>(
            BufferWriter<TBufferWriter> bufferWriter,
            bool isUtf8,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriter<TBufferWriter>(bufferWriter, isUtf8, prettyPrint);
        }
    }
}
