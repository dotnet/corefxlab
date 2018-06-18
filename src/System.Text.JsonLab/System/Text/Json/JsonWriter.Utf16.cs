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
    public ref struct JsonWriterUtf16<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
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
        public JsonWriterUtf16(BufferWriter<TBufferWriter> bufferWriter, bool prettyPrint = false)
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
            WriteStart(CalculateStartBytesNeeded(sizeof(char)), JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        private void WriteStart(int bytesNeeded, byte token)
        {
            Span<char> charBuffer = MemoryMarshal.Cast<byte, char>(EnsureBuffer(bytesNeeded));

            int idx = 0;
            if (!_firstItem)
                charBuffer[idx++] = (char)JsonConstants.ListSeperator;

            if (_prettyPrint)
            {
                idx = PrettyPrintStart(charBuffer, idx);
            }

            charBuffer[idx] = (char)token;
            _bufferWriter.Advance(bytesNeeded);
        }

        private int PrettyPrintStart(Span<char> charBuffer, int idx)
        {
            int indent = _indent;

            while (indent-- >= 0)
            {
                charBuffer[idx++] = (char)JsonConstants.Space;
                charBuffer[idx++] = (char)JsonConstants.Space;
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(char), 4);  // quote {name} quote colon open-brace, hence 4
            WriteStart(nameSpan, bytesNeeded, JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        private void WriteStart(ReadOnlySpan<char> nameSpanChar, int bytesNeeded, byte token)
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
            WriteEnd(CalculateEndBytesNeeded(sizeof(char), JsonWriter.s_newLineUtf16.Length), JsonConstants.CloseBrace);
        }

        private void WriteEnd(int bytesNeeded, byte token)
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
            WriteStart(CalculateStartBytesNeeded(sizeof(char)), JsonConstants.OpenBracket);

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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateBytesNeeded(nameSpan, sizeof(char), 4);
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

            WriteEnd(CalculateEndBytesNeeded(sizeof(char), JsonWriter.s_newLineUtf16Length), JsonConstants.CloseBracket);
        }

        /// <summary>
        /// Write a quoted string value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteAttribute(string name, string value)
        {
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            ReadOnlySpan<char> valueSpan = value.AsSpan();
            int bytesNeeded = CalculateAttributeBytesNeeded(nameSpan, valueSpan, sizeof(char));
            WriteAttribute(nameSpan, valueSpan, bytesNeeded);
        }

        private void WriteAttribute(ReadOnlySpan<char> nameSpanChar, ReadOnlySpan<char> valueSpanChar, int bytesNeeded)
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
            WriteAttribute(nameSpan, bytesNeeded);
            WriteNumber(value); //TODO: attempt to optimize by combining this with WriteAttributeUtf16
        }

        /// <summary>
        /// Write a boolean value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteAttribute(string name, bool value)
        {
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
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
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
            WriteAttribute(nameSpan, bytesNeeded);
            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteAttributeNull(string name)
        {
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            int bytesNeeded = CalculateStartAttributeBytesNeeded(nameSpan, sizeof(char));
            WriteAttribute(nameSpan, bytesNeeded);
            WriteJsonValue(JsonConstants.NullValue);
        }

        private void WriteAttribute(ReadOnlySpan<char> nameSpanChar, int bytesNeeded)
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
            ReadOnlySpan<char> valueSpanChar = value.AsSpan();
            int bytesNeeded = CalculateValueBytesNeeded(valueSpanChar, sizeof(char), 2);
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
            int bytesNeeded = CalculateValueBytesNeeded(sizeof(char), JsonWriter.s_newLineUtf16Length);
            bool insertNegationSign = false;
            if (value < 0)
            {
                insertNegationSign = true;
                value = -value;
                bytesNeeded += sizeof(char);
            }

            int digitCount = JsonWriter.CountDigits((ulong)value);
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

            JsonWriter.WriteDigitsUInt64D((ulong)value, charBuffer.Slice(idx, digitCount));

            _bufferWriter.Advance(bytesNeeded);
        }

        /// <summary>
        /// Write a unsigned integer value into the current array.
        /// </summary>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteValue(ulong value)
        {
            //TODO: Optimize, just like WriteValue(long value)
            WriteItemSeperatorUtf16();
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
            WriteItemSeperatorUtf16();
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
            WriteItemSeperatorUtf16();
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
            WriteItemSeperatorUtf16();
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
            WriteItemSeperatorUtf16();
            _firstItem = false;
            WriteSpacing();

            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value into the current array.
        /// </summary>
        public void WriteNull()
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

        public void Flush() => _bufferWriter.Flush();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumber(long value)
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
        private void WriteJsonValue(ReadOnlySpan<byte> values)
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
        private void WriteControl(byte value)
        {
            Span<byte> buffer = EnsureBuffer(2);
            Unsafe.As<byte, char>(ref MemoryMarshal.GetReference(buffer)) = (char)value;
            _bufferWriter.Advance(2);
        }

        // TODO: Once public methods are optimized, remove this.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperatorUtf16()
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
            if ((uint)needed > (uint)buffer.Length)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateValueBytesNeeded(ReadOnlySpan<char> span, int numBytes, int extraCharacterCount)
        {
            int bytesNeeded = 0;
            if (!_firstItem)
                bytesNeeded = numBytes;

            if (_prettyPrint)
            {
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf16Length;    // For the new line, \r\n or \n
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf16Length + 1;    // For the new line, \r\n or \n, and the space after the colon
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf16Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
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
                int bytesNeededForPrettyPrint = JsonWriter.s_newLineUtf16Length + 1;    // For the new line, \r\n or \n,  and the space after the colon
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddNewLineAndIndentation(Span<char> buffer)
        {
            int offset = 0;
            // \r\n versus \n, depending on OS
            if (JsonWriter.s_newLineUtf16Length == 2)
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
}
