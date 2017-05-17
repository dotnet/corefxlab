// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Text.Formatting;

namespace System.Text.Json
{
    public struct JsonWriter
    {
        readonly bool _prettyPrint;
        readonly ITextOutput _output;
        readonly JsonEncoderState _encoderState;

        int _indent;
        bool _firstItem;

        // These next 2 properties are used to check for whether we can take the fast path
        // for invariant UTF-8 or UTF-16 processing. Otherwise, we need to go through the
        // slow path which makes use of the (possibly generic) encoder.
        private bool UseFastUtf8 => _encoderState == JsonEncoderState.UseFastUtf8;
        private bool UseFastUtf16 => _encoderState == JsonEncoderState.UseFastUtf16;

        /// <summary>
        /// Constructs a JSON writer with a specified <paramref name="output"/>.
        /// </summary>
        /// <param name="output">An instance of <see cref="ITextOutput" /> used for writing bytes to an output channel.</param>
        /// <param name="prettyPrint">Specifies whether to add whitespace to the output text for user readability.</param>
        public JsonWriter(ITextOutput output, bool prettyPrint = false)
        {
            _output = output;
            _prettyPrint = prettyPrint;

            _indent = -1;
            _firstItem = true;

            var encoder = output.Encoder;
            if (encoder.IsInvariantUtf8)
                _encoderState = JsonEncoderState.UseFastUtf8;
            else if (encoder.IsInvariantUtf16)
                _encoderState = JsonEncoderState.UseFastUtf16;
            else
                _encoderState = JsonEncoderState.UseFullEncoder;
        }

        /// <summary>
        /// Write the starting tag of an object. This is used for adding an object to an 
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteObjectStart()
        {
            WriteItemSeperator();
            WriteSpacing(false);
            WriteChar(JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        /// <summary>
        /// Write the starting tag of an object. This is used for adding an object to a
        /// nested object. If this is used while inside a nested array, the property
        /// name will be written and result in invalid JSON.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteObjectStart(string name)
        {
            WriteStartAttribute(name);
            WriteChar(JsonConstants.OpenBrace);

            _firstItem = true;
            _indent++;
        }

        /// <summary>
        /// Writes the end tag for an object.
        /// </summary>
        public void WriteObjectEnd()
        {
            _firstItem = false;
            _indent--;
            WriteSpacing();
            WriteChar(JsonConstants.CloseBrace);
        }

        /// <summary>
        /// Write the starting tag of an array. This is used for adding an array to a nested
        /// array of other items. If this is used while inside a nested object, the property
        /// name will be missing and result in invalid JSON.
        /// </summary>
        public void WriteArrayStart()
        {
            WriteItemSeperator();
            WriteSpacing();
            WriteChar(JsonConstants.OpenBracket);

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
            WriteStartAttribute(name);
            WriteChar(JsonConstants.OpenBracket);

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
            WriteSpacing();
            WriteChar(JsonConstants.CloseBracket);
        }

        /// <summary>
        /// Write a quoted string value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteAttribute(string name, string value)
        {
            WriteStartAttribute(name);
            WriteQuotedString(value);
        }

        /// <summary>
        /// Write a signed integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, long value)
        {
            WriteStartAttribute(name);
            WriteNumber(value);
        }

        /// <summary>
        /// Write an unsigned integer value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteAttribute(string name, ulong value)
        {
            WriteStartAttribute(name);
            WriteNumber(value);
        }

        /// <summary>
        /// Write a boolean value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The boolean value to be written to JSON data.</param>
        public void WriteAttribute(string name, bool value)
        {
            WriteStartAttribute(name);
            if (value)
                WriteAscii(JsonConstants.TrueValue);
            else
                WriteAscii(JsonConstants.FalseValue);
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTime value)
        {
            WriteStartAttribute(name);
            WriteDateTime(value);
        }

        /// <summary>
        /// Write a <see cref="DateTimeOffset"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, DateTimeOffset value)
        {
            WriteStartAttribute(name);
            WriteDateTimeOffset(value);
        }

        /// <summary>
        /// Write a <see cref="Guid"/> value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        /// <param name="value">The <see cref="Guid"/> value to be written to JSON data.</param>
        public void WriteAttribute(string name, Guid value)
        {
            WriteStartAttribute(name);
            WriteGuid(value);
        }

        /// <summary>
        /// Write a null value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteAttributeNull(string name)
        {
            WriteStartAttribute(name);
            WriteAscii(JsonConstants.NullValue);
        }

        /// <summary>
        /// Write an undefined value along with a property name into the current object.
        /// </summary>
        /// <param name="name">The name of the property (i.e. key) within the containing object.</param>
        public void WriteAttributeUndefined(string name)
        {
            WriteStartAttribute(name);
            WriteAscii(JsonConstants.UndefinedValue);
        }

        /// <summary>
        /// Writes a quoted string value into the current array.
        /// </summary>
        /// <param name="value">The string value that will be quoted within the JSON data.</param>
        public void WriteValue(string value)
        {
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();
            WriteQuotedString(value);
        }

        /// <summary>
        /// Write a signed integer value into the current array.
        /// </summary>
        /// <param name="value">The signed integer value to be written to JSON data.</param>
        public void WriteValue(long value)
        {
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();
            WriteNumber(value);
        }

        /// <summary>
        /// Write a unsigned integer value into the current array.
        /// </summary>
        /// <param name="value">The unsigned integer value to be written to JSON data.</param>
        public void WriteValue(ulong value)
        {
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
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();
            if (value)
                WriteAscii(JsonConstants.TrueValue);
            else
                WriteAscii(JsonConstants.FalseValue);
        }

        /// <summary>
        /// Write a <see cref="DateTime"/> value into the current array.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be written to JSON data.</param>
        public void WriteValue(DateTime value)
        {
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
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();
            WriteAscii(JsonConstants.NullValue);
        }

        /// <summary>
        /// Write an undefined value into the current array.
        /// </summary>
        public void WriteUndefined()
        {
            WriteItemSeperator();
            _firstItem = false;
            WriteSpacing();
            WriteAscii(JsonConstants.UndefinedValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteStartAttribute(string name)
        {
            WriteItemSeperator();
            _firstItem = false;

            WriteSpacing();
            WriteQuotedString(name);
            WriteChar(JsonConstants.KeyValueSeperator);

            if (_prettyPrint)
                WriteChar(JsonConstants.Space);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteChar(char value)
        {
            var output = _output;
            if (UseFastUtf8)
            {
                if (output.Buffer.Length < 1)
                    output.Enlarge();

                output.Buffer.DangerousGetPinnableReference() = (byte)value;
                output.Advance(1);
            }
            else if (UseFastUtf16)
            {
                if (output.Buffer.Length < 2)
                    output.Enlarge(2);

                unsafe
                {
                    Unsafe.AsRef<char>(Unsafe.AsPointer<byte>(ref output.Buffer.DangerousGetPinnableReference())) = value;
                }

                output.Advance(2);
            }
            else
            {
                unsafe
                {
                    // Slow path, if we are dealing with non-invariant.
                    ReadOnlySpan<char> chSpan = new ReadOnlySpan<char>(&value, 1);
                    Write(chSpan);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteQuotedString(string value)
        {
            WriteChar(JsonConstants.Quote);
            // TODO: We need to handle escaping.
            Write(value.AsSpan());
            WriteChar(JsonConstants.Quote);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumber(long value)
        {
            int written;
            while (!value.TryFormat(_output.Buffer, out written, JsonConstants.NumberFormat, _output.Encoder))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteNumber(ulong value)
        {
            int written;
            while (!value.TryFormat(_output.Buffer, out written, JsonConstants.NumberFormat, _output.Encoder))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDateTime(DateTime value)
        {
            int written;
            while (!value.TryFormat(_output.Buffer, out written, JsonConstants.DateTimeFormat, _output.Encoder))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDateTimeOffset(DateTimeOffset value)
        {
            int written;
            while (!value.TryFormat(_output.Buffer, out written, JsonConstants.DateTimeFormat, _output.Encoder))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteGuid(Guid value)
        {
            int written;
            while (!value.TryFormat(_output.Buffer, out written, JsonConstants.GuidFormat, _output.Encoder))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write(ReadOnlySpan<char> value)
        {
            int written;
            while (!_output.Encoder.TryEncode(value, _output.Buffer, out int consumed, out written))
                _output.Enlarge();

            _output.Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteAscii(ReadOnlySpan<char> values)
        {
            if (UseFastUtf8)
            {
                int len = values.Length;
                Span<byte> buffer = _output.Buffer;
                if (buffer.Length < len)
                {
                    _output.Enlarge(len);
                    buffer = _output.Buffer;
                }

                ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();
                ref char chars = ref values.DangerousGetPinnableReference();
                for (var i = 0; i < len; i++)
                    Unsafe.Add(ref utf8Bytes, i) = (byte)Unsafe.Add(ref chars, i);
            }
            else if (UseFastUtf16)
            {
                int needed = values.Length * sizeof(char);
                Span<byte> buffer = _output.Buffer;
                if (buffer.Length < needed)
                {
                    _output.Enlarge(needed);
                    buffer = _output.Buffer;
                }

                Span<char> span = buffer.NonPortableCast<byte, char>();
                values.CopyTo(span);
            }
            else
            {
                Write(values);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteItemSeperator()
        {
            if (_firstItem) return;

            WriteChar(JsonConstants.ListSeperator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacing(bool newLine = true)
        {
            if (!_prettyPrint) return;

            if (UseFastUtf8)
                WriteSpacingUtf8(newLine);
            else if (UseFastUtf16)
                WriteSpacingUtf16(newLine);
            else
                WriteSpacingSlow(newLine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingSlow(bool newLine)
        {
            if (newLine)
            {
                WriteChar(JsonConstants.CarriageReturn);
                WriteChar(JsonConstants.LineFeed);
            }

            int indent = _indent;
            while (indent-- >= 0)
            {
                WriteChar(JsonConstants.Space);
                WriteChar(JsonConstants.Space);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingUtf8(bool newline)
        {
            var indent = _indent;
            var bytesNeeded = newline ? 2 : 0;
            bytesNeeded += (indent + 1) * 2;

            var buffer = _output.Buffer;
            if (buffer.Length < bytesNeeded)
            {
                _output.Enlarge(bytesNeeded);
                buffer = _output.Buffer;
            }

            ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();
            int idx = 0;

            if (newline)
            {
                Unsafe.Add(ref utf8Bytes, idx++) = (byte)JsonConstants.CarriageReturn;
                Unsafe.Add(ref utf8Bytes, idx++) = (byte)JsonConstants.LineFeed;
            }

            while (indent-- >= 0)
            {
                Unsafe.Add(ref utf8Bytes, idx++) = (byte)JsonConstants.Space;
                Unsafe.Add(ref utf8Bytes, idx++) = (byte)JsonConstants.Space;
            }

            _output.Advance(bytesNeeded);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpacingUtf16(bool newline)
        {
            var indent = _indent;
            var bytesNeeded = newline ? 2 : 0;
            bytesNeeded += (indent + 1) * 2;
            bytesNeeded *= sizeof(char);

            var buffer = _output.Buffer;
            if (buffer.Length < bytesNeeded)
            {
                _output.Enlarge(bytesNeeded);
                buffer = _output.Buffer;
            }


            var span = buffer.NonPortableCast<byte, char>();
            ref char utf16Bytes = ref span.DangerousGetPinnableReference();
            int idx = 0;

            if (newline)
            {
                Unsafe.Add(ref utf16Bytes, idx++) = JsonConstants.CarriageReturn;
                Unsafe.Add(ref utf16Bytes, idx++) = JsonConstants.LineFeed;
            }

            while (indent-- >= 0)
            {
                Unsafe.Add(ref utf16Bytes, idx++) = JsonConstants.Space;
                Unsafe.Add(ref utf16Bytes, idx++) = JsonConstants.Space;
            }

            _output.Advance(bytesNeeded);
        }
    }
}
