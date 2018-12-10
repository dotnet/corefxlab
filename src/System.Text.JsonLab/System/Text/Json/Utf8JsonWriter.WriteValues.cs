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
    public ref partial struct Utf8JsonWriter2<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        public void WriteNull()
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(bool value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(int value)
            => WriteValue((long)value);

        public void WriteValue(long value)
        {
            if (_writerOptions.SlowPath)
                WriteValueSlow(value);
            else
                WriteValueFast(value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteValueFast(long value)
        {
            // Calculated based on the following: ',number'
            int bytesNeeded = 1 + JsonConstants.MaximumInt64Length;

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteValueSlow(long value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(uint value)
            => WriteValue((ulong)value);

        public void WriteValue(ulong value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(double value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(float value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(decimal value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(string utf16Text)
            => WriteValue(utf16Text.AsSpan());

        public void WriteValue(ReadOnlySpan<char> utf16Text)
        {
            JsonWriterHelper.ValidateValue(ref utf16Text);

            WriteValueWithEncodingValue(MemoryMarshal.AsBytes(utf16Text));
        }

        private void WriteValueWithEncodingValue(ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
                WriteValueSlowWithEncodingValue(ref value);
            else
                WriteValueFastWithEncodingValue(ref value);

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

            Ensure(bytesNeeded);

            Span<byte> byteBuffer = WriteValue(bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteValue(ReadOnlySpan<byte> utf8Text)
        {
            JsonWriterHelper.ValidateValue(ref utf8Text);

            if (JsonWriterHelper.IndexOfAnyEscape(utf8Text) != -1)
            {
                //TODO: Add escaping.
                utf8Text = JsonWriterHelper.EscapeStringValue(utf8Text);
            }

            if (_writerOptions.SlowPath)
                WriteValueSlow(ref utf8Text);
            else
                WriteValueFast(ref utf8Text);

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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(DateTime value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(DateTimeOffset value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteValue(Guid value)
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
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
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

        public void WriteComment(string utf16Text)
            => WriteComment(utf16Text.AsSpan());

        public void WriteComment(ReadOnlySpan<char> utf16Text)
        {
            JsonWriterHelper.ValidateValue(ref utf16Text);

            WriteCommentWithEncodingValue(MemoryMarshal.AsBytes(utf16Text));
        }

        private void WriteCommentWithEncodingValue(ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.Formatted)
                WriteCommentFormattedWithEncodingValue(ref value);
            else
                WriteCommentFastWithEncodingValue(ref value);
        }

        private void WriteCommentFastWithEncodingValue(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length / 2 * 3 - 4 >= 0);

            // Calculated based on the following: '/*encoded value*/'
            int bytesNeeded = escapedValue.Length / 2 * 3 + 4;

            Ensure(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
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

            Ensure(bytesNeeded);

            Span<byte> byteBuffer = _buffer;

            int idx = 0;

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref byteBuffer, ref idx);

            idx += JsonWriterHelper.WriteIndentation(byteBuffer.Slice(idx, indent));

            byteBuffer[idx++] = JsonConstants.Solidus;
            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, byteBuffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            byteBuffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk
            byteBuffer[idx++] = JsonConstants.Solidus;

            Advance(idx);
        }

        public void WriteComment(ReadOnlySpan<byte> utf8Text)
        {
            JsonWriterHelper.ValidateValue(ref utf8Text);

            if (JsonWriterHelper.IndexOfAnyEscape(utf8Text) != -1)
            {
                //TODO: Add escaping.
                utf8Text = JsonWriterHelper.EscapeStringValue(utf8Text);
            }

            if (_writerOptions.Formatted)
                WriteCommentFormatted(ref utf8Text);
            else
                WriteCommentFast(ref utf8Text);
        }

        private void WriteCommentFast(ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - escapedValue.Length - 4 >= 0);

            // Calculated based on the following: '/*value*/'
            int bytesNeeded = escapedValue.Length + 4;

            Ensure(bytesNeeded);

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

            Ensure(bytesNeeded);

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
                if (_tokenType != JsonTokenType.PropertyName)
                {
                    JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
                }
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
