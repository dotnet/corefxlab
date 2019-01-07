// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        public void WriteNullValue()
        {
            if (_writerOptions.SlowPath)
                WriteNullSlow();
            else
                WriteNullFast();

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFast()
        {
            // Calculated based on the following: ',null'
            int bytesNeeded = 1 + JsonConstants.NullValue.Length;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlow()
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteNullFormatted();
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteNullFast();
            }
        }

        private void WriteNullFormatted()
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - 1 - JsonConstants.NullValue.Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  null'
            int bytesNeeded = 1 + JsonConstants.NullValue.Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        public void WriteBooleanValue(bool value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
        }

        private void WriteValueFast(bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - 1 - JsonConstants.FalseValue.Length >= 0);

            // Calculated based on the following: ',true' OR ',false'
            int bytesNeeded = 1 + JsonConstants.FalseValue.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteValueSlow(bool value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(bool value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - 1 - JsonConstants.FalseValue.Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  true' OR ',\r\n  false'
            int bytesNeeded = 1 + JsonConstants.FalseValue.Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        public void WriteNumberValue(int value)
            => WriteNumberValue((long)value);

        public void WriteNumberValue(long value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueIndented(value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueMinimized(value);
            }

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueMinimized(long value)
        {
            int idx = 0;
            if (_currentDepth < 0)
            {
                if (_buffer.Length <= 0)
                {
                    GrowAndEnsure();
                }
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            int bytesWritten;
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out bytesWritten);
            while (!JsonWriterHelper.TryFormatInt64Default(value, _buffer.Slice(idx), out bytesWritten))
            {
                AdvanceAndGrow(idx, JsonConstants.MaximumInt64Length);
                idx = 0;
            }
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueIndented(long value)
        {
            int idx = 0;
            if (_currentDepth < 0)
            {
                while (_buffer.Length <= idx)
                {
                    GrowAndEnsure();
                }
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref idx);

            int indent = Indentation;
            while (true)
            {
                bool result = JsonWriterHelper.TryWriteIndentation(_buffer.Slice(idx), indent, out int written);
                idx += written;
                if (result)
                {
                    break;
                }
                indent -= written;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            int bytesWritten;
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out bytesWritten);
            while (!JsonWriterHelper.TryFormatInt64Default(value, _buffer.Slice(idx), out bytesWritten))
            {
                AdvanceAndGrow(idx, JsonConstants.MaximumInt64Length);
                idx = 0;
            }
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueFormatted(long value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumInt64Length - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumInt64Length;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumberValue(uint value)
            => WriteNumberValue((ulong)value);

        public void WriteNumberValue(ulong value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueFast(ulong value)
        {
            // Calculated based on the following: ',number'
            int bytesNeeded = 1 + JsonConstants.MaximumUInt64Length;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueSlow(ulong value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(ulong value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumUInt64Length - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumUInt64Length;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumberValue(double value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueFast(double value)
        {
            // Calculated based on the following: ',number'
            int bytesNeeded = 1 + JsonConstants.MaximumDoubleLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueSlow(double value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(double value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumDoubleLength - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumDoubleLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumberValue(float value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueFast(float value)
        {
            // Calculated based on the following: ',number'
            int bytesNeeded = 1 + JsonConstants.MaximumSingleLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueSlow(float value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(float value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumSingleLength - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumSingleLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumberValue(decimal value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueFast(decimal value)
        {
            // Calculated based on the following: ',number'
            int bytesNeeded = 1 + JsonConstants.MaximumDecimalLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueSlow(decimal value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(decimal value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumDecimalLength - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumDecimalLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteStringValue(string utf16Text, bool suppressEscaping = false)
            => WriteStringValue(utf16Text.AsSpan(), suppressEscaping);

        public void WriteStringValue(ReadOnlySpan<char> utf16Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf16Text);

            WriteValueWithEncodingValue(MemoryMarshal.AsBytes(utf16Text), suppressEscaping);
        }

        private unsafe void WriteValueWithEncodingValue(ReadOnlySpan<byte> value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = value;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(value, _buffer, out _, out _);
                byte* ptr = stackalloc byte[value.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, value.Length);
            }

            if (_writerOptions.SlowPath)
                WriteValueSlowWithEncodingValue(ref escapedValue);
            else
                WriteValueFastWithEncodingValue(ref escapedValue);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteValueFastWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"encoded value"'
            int bytesNeeded = escapedValue.Length / 2 * 3 + 3;

            if (_currentDepth >= 0)
                bytesNeeded--;

            CheckSizeAndGrow(bytesNeeded);

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteValueSlowWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormattedWithEncodingValue(ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFastWithEncodingValue(ref escapedValue);
            }
        }

        private void WriteValueFormattedWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length / 2 * 3 - 3 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded value"'
            int bytesNeeded = escapedValue.Length / 2 * 3 + 3 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteStringValue(ReadOnlySpan<byte> utf8Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf8Text);

            ReadOnlySpan<byte> escapedValue = utf8Text;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(utf8Text, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[utf8Text.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, utf8Text.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteValueSlow(ref escapedValue);
            else
                WriteValueFast(ref escapedValue);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteValueFast(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length - 3 >= 0);

            // Calculated based on the following: ',"value"'
            int bytesNeeded = escapedValue.Length + 3;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteValueSlow(ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(ref escapedValue);
            }
        }

        private void WriteValueFormatted(ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - escapedValue.Length - 3 - indent >= 0);

            // Calculated based on the following: ',\r\n  "value"'
            int bytesNeeded = 3 + JsonWriterHelper.NewLineUtf8.Length + indent + escapedValue.Length;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteStringValue(DateTime value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteValueFast(DateTime value)
        {
            // Calculated based on the following: ',"value"'
            int bytesNeeded = 3 + JsonConstants.MaximumDateTimeLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteValueSlow(DateTime value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(DateTime value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumDateTimeLength - 3 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "value"'
            int bytesNeeded = 3 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumDateTimeLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteStringValue(DateTimeOffset value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteValueFast(DateTimeOffset value)
        {
            // Calculated based on the following: ',"value"'
            int bytesNeeded = 3 + JsonConstants.MaximumDateTimeOffsetLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteValueSlow(DateTimeOffset value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(DateTimeOffset value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumDateTimeOffsetLength - 3 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "value"'
            int bytesNeeded = 3 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumDateTimeOffsetLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteStringValue(Guid value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteValueFast(Guid value)
        {
            // Calculated based on the following: ',"value"'
            int bytesNeeded = 3 + JsonConstants.MaximumGuidLength;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteValueSlow(Guid value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteValueFormatted(value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteValueFast(value);
            }
        }

        private void WriteValueFormatted(Guid value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumGuidLength - 3 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "value"'
            int bytesNeeded = 3 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumGuidLength;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);
            byteBuffer[idx++] = JsonConstants.Quote;
            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteCommentValue(string utf16Text, bool suppressEscaping = false)
            => WriteCommentValue(utf16Text.AsSpan(), suppressEscaping);

        public void WriteCommentValue(ReadOnlySpan<char> utf16Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf16Text);

            WriteCommentWithEncodingValue(MemoryMarshal.AsBytes(utf16Text), suppressEscaping);
        }

        private unsafe void WriteCommentWithEncodingValue(ReadOnlySpan<byte> value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = value;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(value, _buffer, out _, out _);
                byte* ptr = stackalloc byte[value.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, value.Length);
            }

            if (_writerOptions.Indented)
                WriteCommentFormattedWithEncodingValue(ref escapedValue);
            else
                WriteCommentFastWithEncodingValue(ref escapedValue);
        }

        private void WriteCommentFastWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length / 2 * 3 - 4 >= 0);

            // Calculated based on the following: '/*encoded value*/'
            int bytesNeeded = escapedValue.Length / 2 * 3 + 4;

            CheckSizeAndGrow(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk
            byteBuffer[idx++] = JsonConstants.Solidus;

            Advance(idx);
        }

        private void WriteCommentFormattedWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length / 2 * 3 - 4 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: '\r\n  /*encoded value*/'
            int bytesNeeded = escapedValue.Length / 2 * 3 + 4 + JsonWriterHelper.NewLineUtf8.Length + indent;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            CheckSizeAndGrow(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk
            byteBuffer[idx++] = JsonConstants.Solidus;

            Advance(idx);
        }

        public void WriteCommentValue(ReadOnlySpan<byte> utf8Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf8Text);

            ReadOnlySpan<byte> escapedValue = utf8Text;
            if (!suppressEscaping)
            {
                JsonWriterHelper.EscapeString(utf8Text, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[utf8Text.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, utf8Text.Length);
                }
            }

            if (_writerOptions.Indented)
                WriteCommentFormatted(ref escapedValue);
            else
                WriteCommentFast(ref escapedValue);
        }

        private void WriteCommentFast(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length - 4 >= 0);

            // Calculated based on the following: '/*value*/'
            int bytesNeeded = escapedValue.Length + 4;

            CheckSizeAndGrow(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;

            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk
            byteBuffer[idx++] = JsonConstants.Solidus;

            Advance(idx);
        }

        private void WriteCommentFormatted(ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - escapedValue.Length - 4 - indent >= 0);

            // Calculated based on the following: ',\r\n  /*value*/'
            int bytesNeeded = 4 + JsonWriterHelper.NewLineUtf8.Length + indent + escapedValue.Length;

            if (_tokenType == JsonTokenType.None)
                bytesNeeded -= JsonWriterHelper.NewLineUtf8.Length;

            CheckSizeAndGrow(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;

            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk
            byteBuffer[idx++] = JsonConstants.Solidus;

            Advance(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WriteValue(int bytesNeeded, out int idx)
        {
            if (_currentDepth >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            idx = 0;

            if (_currentDepth < 0)
                byteBuffer[idx++] = JsonConstants.ListSeperator;

            return byteBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WriteValueFormatted(int bytesNeeded, int indent, out int idx)
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
                WriteNewLine(ref byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            return byteBuffer;
        }

        private void ValidateWritingValue()
        {
            if (_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.None && _tokenType != JsonTokenType.StartArray);
                JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
            }
            else
            {
                if (!_isNotPrimitive && _tokenType != JsonTokenType.None)
                {
                    JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
                }
            }
        }
    }
}
