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
    public ref struct JsonWriterUtf8<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        private readonly bool _prettyPrint;
        private BufferWriter<TBufferWriter> _bufferWriter;

        private int _indent;
        private bool _firstItem;

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="bufferWriter"/>.
        /// </summary>
        /// <param name="bufferWriter">An instance of <see cref="ITextBufferWriter" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public JsonWriterUtf8(BufferWriter<TBufferWriter> bufferWriter, bool prettyPrint = false)
        {
            _bufferWriter = bufferWriter;
            _prettyPrint = prettyPrint;

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
            WriteStart(CalculateStartBytesNeeded(sizeof(byte)), JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        private void WriteStart(int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);

            int idx = 0;
            if (!_firstItem)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            if (_prettyPrint)
            {
                idx = PrettyPrintStart(byteBuffer, idx);
            }

            byteBuffer[idx] = token;
            _bufferWriter.Advance(bytesNeeded);
        }

        private int PrettyPrintStart(Span<byte> byteBuffer, int idx)
        {
            int indent = _indent;

            while (indent-- >= 0)
            {
                byteBuffer[idx++] = JsonConstants.Space;
                byteBuffer[idx++] = JsonConstants.Space;
            }

            return idx;
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
            int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(byte), 4);  // quote {name} quote colon open-brace, hence 4
            WriteStart(nameSpan, bytesNeeded, JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        private void WriteStart(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded, byte token)
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

        /// <summary>
        /// Writes the end tag for an object.
        /// </summary>
        public void WriteObjectEnd()
        {
            _firstItem = false;
            _indent--;
            WriteEnd(CalculateEndBytesNeeded(sizeof(byte), JsonWriter.s_newLineUtf8.Length), JsonConstants.CloseBrace);
        }

        private void WriteEnd(int bytesNeeded, byte token)
        {
            Span<byte> byteBuffer = EnsureBuffer(bytesNeeded);
            int idx = 0;

            if (_prettyPrint)
                idx += AddNewLineAndIndentation(byteBuffer.Slice(idx));

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
            WriteStart(CalculateStartBytesNeeded(sizeof(byte)), JsonConstants.OpenBracket);

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
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(byte), 4);
            WriteStart(nameSpan, bytesNeeded, JsonConstants.OpenBracket);

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

            WriteEnd(CalculateEndBytesNeeded(sizeof(byte), JsonWriter.s_newLineUtf8Length), JsonConstants.CloseBracket);
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
            int bytesNeeded = CalculateAttributeBytesNeeded(nameSpan, valueSpan, sizeof(byte));
            WriteAttribute(nameSpan, valueSpan, bytesNeeded);
        }

        private void WriteAttribute(ReadOnlySpan<byte> nameSpanByte, ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
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

        /// <summary>
        /// Write a signed integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, long value)
        {
            ReadOnlySpan<byte> nameSpan = MemoryMarshal.AsBytes(name.AsSpan());
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(byte));
            WriteAttribute(nameSpan, bytesNeeded);
            WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttribute
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
            WriteAttribute(nameSpan, bytesNeeded);
            WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttribute
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
            WriteAttribute(nameSpan, bytesNeeded);
            if (value)
                WriteJsonValue(JsonConstants.TrueValue);
            else
                WriteJsonValue(JsonConstants.FalseValue);
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
            WriteAttribute(nameSpan, bytesNeeded);
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
            WriteAttribute(nameSpan, bytesNeeded);
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
            WriteAttribute(nameSpan, bytesNeeded);
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
            WriteAttribute(nameSpan, bytesNeeded);
            WriteJsonValue(JsonConstants.NullValue);
        }

        private void WriteAttribute(ReadOnlySpan<byte> nameSpanByte, int bytesNeeded)
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

        /// <summary>
        /// Writes a quoted string value into the current array.
        /// </summary>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteValue(string value)
        {
            ReadOnlySpan<byte> valueSpan = MemoryMarshal.AsBytes(value.AsSpan());
            int bytesNeeded = CalculateValueBytesNeeded(valueSpan, sizeof(byte), 2);
            WriteValue(valueSpan, bytesNeeded);
        }

        private void WriteValue(ReadOnlySpan<byte> valueSpanByte, int bytesNeeded)
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

        /// <summary>
        /// Write a signed integer value into the current array.
        /// </summary>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteValue(long value)
        {
            WriteValue(value, CalculateValueBytesNeeded(sizeof(byte), JsonWriter.s_newLineUtf8Length));
        }

        private void WriteValue(long value, int bytesNeeded)
        {
            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += sizeof(byte);
            }

            int digitCount = JsonWriter.CountDigits((ulong)value);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigitsUInt64D(ulong value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(JsonWriter.CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }

        /// <summary>
        /// Write a unsigned integer value into the current array.
        /// </summary>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteValue(ulong value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();

            WriteNumber(value);
        }

        /// <summary>
        /// Write a boolean value into the current array.
        /// </summary>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteValue(bool value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();

            if (value)
                WriteJsonValue(JsonConstants.TrueValue);
            else
                WriteJsonValue(JsonConstants.FalseValue);
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteValue(DateTime value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();

            WriteDateTime(value);
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteValue(DateTimeOffset value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();

            WriteDateTimeOffset(value);
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteValue(Guid value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();

            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value into the current array.
        /// </summary>
        public void WriteNull()
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

        public void Flush() => _bufferWriter.Flush();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumber(long value)
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
        private void WriteJsonValue(ReadOnlySpan<byte> values)
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
        private void WriteControl(byte value)
        {
            Span<byte> buffer = EnsureBuffer(1);
            MemoryMarshal.GetReference(buffer) = value;
            _bufferWriter.Advance(1);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperator()
        {
            if (_firstItem) return;

            WriteControl(JsonConstants.ListSeperator);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacing(bool newline = true)
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf8Length;    // For the new line, \r\n or \n
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf8Length + 1;    // For the new line, \r\n or \n, and the space after the colon
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf8Length + 1;    // For the new line, \r\n or \n, and the space after the colon
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf8Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
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
        private int AddNewLineAndIndentation(Span<byte> buffer)
        {
            int offset = 0;
            // \r\n versus \n, depending on OS
            if (JsonWriter.s_newLineUtf8Length == 2)
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
    }
}
