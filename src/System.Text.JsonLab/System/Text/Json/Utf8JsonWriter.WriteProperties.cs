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
        public void WriteNull(string propertyName)
            => WriteNull(propertyName.AsSpan());

        public void WriteNull(ReadOnlySpan<char> propertyName)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNullWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteNullWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_writerOptions.SlowPath)
                WriteNullSlowWithEncoding(ref propertyName);
            else
                WriteNullFastWithEncoding(ref propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFastWithEncoding(ref ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.NullValue.Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.NullValue.Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlowWithEncoding(ref ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNullFormattedWithEncoding(ref propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNullFastWithEncoding(ref propertyName);
            }
        }

        private void WriteNullFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.NullValue.Length - indent - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.NullValue.Length + indent + JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        public void WriteNull(ReadOnlySpan<byte> propertyName)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNullSlow(ref propertyName);
            else
                WriteNullFast(ref propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFast(ref ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.NullValue.Length >= 0);

            // Calculated based on the following: ',"propertyName":null'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.NullValue.Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlow(ref ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNullFormatted(ref propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNullFast(ref propertyName);
            }
        }

        private void WriteNullFormatted(ref ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.NullValue.Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": null'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.NullValue.Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        public void WriteBoolean(string propertyName, bool value)
            => WriteBoolean(propertyName.AsSpan(), value);

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteBooleanWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteBooleanWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            if (_writerOptions.SlowPath)
                WriteBooleanSlowWithEncoding(ref propertyName, value);
            else
                WriteBooleanFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFastWithEncoding(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.FalseValue.Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":true' OR ',"encoded propertyName":false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.FalseValue.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(_buffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteBooleanFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteBooleanFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteBooleanFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.FalseValue.Length - indent - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": true' OR ',\r\n  "encoded propertyName": false'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.FalseValue.Length + indent + JsonWriterHelper.NewLineUtf8.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteBooleanSlow(ref propertyName, value);
            else
                WriteBooleanFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFast(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.FalseValue.Length >= 0);

            // Calculated based on the following: ',"propertyName":true' OR ',"propertyName":false'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.FalseValue.Length;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(_buffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlow(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteBooleanFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteBooleanFast(ref propertyName, value);
            }
        }

        private void WriteBooleanFormatted(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.FalseValue.Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": true' OR ',\r\n  "propertyName": false'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.FalseValue.Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            ReadOnlySpan<byte> valueSpan = JsonConstants.FalseValue;
            _tokenType = JsonTokenType.False;

            if (value)
            {
                bytesNeeded--;
                valueSpan = JsonConstants.TrueValue;
                _tokenType = JsonTokenType.True;
            }

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, long value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, long value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, long value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref propertyName, value);
            else
                WriteNumberFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ref ReadOnlySpan<byte> propertyName, long value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumInt64Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumInt64Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, _buffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            //bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, long value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNumberFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, long value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, long value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref propertyName, value);
            else
                WriteNumberFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ref ReadOnlySpan<byte> propertyName, long value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumInt64Length >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumInt64Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, _buffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            //bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, long value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNumberFast(ref propertyName, value);
            }
        }

        private void WriteNumberFormatted(ref ReadOnlySpan<byte> propertyName, long value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, int value)
            => WriteNumber(propertyName, (long)value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
            => WriteNumber(propertyName, (long)value);

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value)
            => WriteNumber(propertyName, (long)value);

        public void WriteNumber(string propertyName, uint value)
            => WriteNumber(propertyName, (ulong)value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, uint value)
            => WriteNumber(propertyName, (ulong)value);

        public void WriteNumber(ReadOnlySpan<byte> propertyName, uint value)
            => WriteNumber(propertyName, (ulong)value);

        public void WriteNumber(string propertyName, ulong value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, ulong value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, ulong value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref propertyName, value);
            else
                WriteNumberFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumUInt64Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumUInt64Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, _buffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNumberFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumUInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumUInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, ulong value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref propertyName, value);
            else
                WriteNumberFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumUInt64Length >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumUInt64Length;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, _buffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNumberFast(ref propertyName, value);
            }
        }

        private void WriteNumberFormatted(ref ReadOnlySpan<byte> propertyName, ulong value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumUInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumUInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, double value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, double value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, double value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref propertyName, value);
            else
                WriteNumberFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ref ReadOnlySpan<byte> propertyName, double value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumDoubleLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumDoubleLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNumberFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, double value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumDoubleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumDoubleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, double value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref propertyName, value);
            else
                WriteNumberFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ref ReadOnlySpan<byte> propertyName, double value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumDoubleLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumDoubleLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNumberFast(ref propertyName, value);
            }
        }

        private void WriteNumberFormatted(ref ReadOnlySpan<byte> propertyName, double value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumDoubleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumDoubleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, float value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, float value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, float value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref propertyName, value);
            else
                WriteNumberFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ref ReadOnlySpan<byte> propertyName, float value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumSingleLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumSingleLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNumberFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, float value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumSingleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumSingleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, float value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref propertyName, value);
            else
                WriteNumberFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ref ReadOnlySpan<byte> propertyName, float value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumSingleLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumSingleLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNumberFast(ref propertyName, value);
            }
        }

        private void WriteNumberFormatted(ref ReadOnlySpan<byte> propertyName, float value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumSingleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumSingleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, decimal value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, decimal value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, decimal value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref propertyName, value);
            else
                WriteNumberFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumDecimalLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumDecimalLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteNumberFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumDecimalLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumDecimalLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, decimal value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref propertyName, value);
            else
                WriteNumberFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumDecimalLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumDecimalLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteNumberFast(ref propertyName, value);
            }
        }

        private void WriteNumberFormatted(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumDecimalLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumDecimalLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteStringWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(ref propertyName, ref value);
            else
                WriteStringFastWithEncodingPropertyValue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingPropertyValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 6;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, _buffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingPropertyValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteStringFormattedWithEncodingPropertyValue(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteStringFastWithEncodingPropertyValue(ref propertyName, ref escapedValue);
            }
        }

        private void ValidateWritingPropertyWithEncoding(ref ReadOnlySpan<byte> propertyName)
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

        private void WriteStringFormattedWithEncodingPropertyValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

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

        private void WriteStringWithEncodingProperty(ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingProperty(ref propertyName, ref value);
            else
                WriteStringFastWithEncodingProperty(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"encoded propertyName":"value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 6;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(_buffer.Slice(idx));
            idx += escapedValue.Length;
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteStringFormattedWithEncodingProperty(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteStringFastWithEncodingProperty(ref propertyName, ref escapedValue);
            }
        }

        private void WriteStringFormattedWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - escapedValue.Length - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "value"'
            int bytesNeeded = propertyName.Length / 2 * 3 + escapedValue.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringWithEncodingValue(ref ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingValue(ref propertyName, ref value);
            else
                WriteStringFastWithEncodingValue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 6;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue, _buffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == escapedValue.Length);
            idx += written;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteStringFormattedWithEncodingValue(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteStringFastWithEncodingValue(ref propertyName, ref escapedValue);
            }
        }

        private void WriteStringFormattedWithEncodingValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length / 2 * 3 - 7 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "encoded value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length / 2 * 3 + 7 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

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

            WriteStringWithEncodingProperty(MemoryMarshal.AsBytes(propertyName), ref value);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, string value)
            => WriteString(propertyName, value.AsSpan());

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingValue(ref propertyName, MemoryMarshal.AsBytes(value));
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
            {
                //TODO: Add escaping.
                value = JsonWriterHelper.EscapeStringValue(value);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref propertyName, ref value);
            else
                WriteStringFast(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length + 6;

            if (_currentDepth >= 0)
                bytesNeeded--;
            
            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(_buffer.Slice(idx));
            idx += escapedValue.Length;
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteStringFormatted(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteStringFast(ref propertyName, ref escapedValue);
            }
        }

        private void ValidateWritingProperty(ref ReadOnlySpan<byte> propertyName)
        {
            if (JsonWriterHelper.IndexOfAnyEscape(propertyName) != -1)
                JsonThrowHelper.ThrowJsonWriterException("Property name must be properly escaped."); //TODO: Fix message

            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException("Cannot add a property within an array.");    //TODO: Add resouce message
            }
        }

        private void WriteStringFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonWriterHelper.NewLineUtf8.Length - propertyName.Length - escapedValue.Length - 7 - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "value"'
            int bytesNeeded = propertyName.Length + 7 + JsonWriterHelper.NewLineUtf8.Length + indent + escapedValue.Length;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(byteBuffer.Slice(idx));
            idx += escapedValue.Length;
            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(string propertyName, DateTime value)
            => WriteString(propertyName.AsSpan(), value);

        public void WriteString(ReadOnlySpan<char> propertyName, DateTime value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(ref propertyName, value);
            else
                WriteStringFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFastWithEncoding(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 6 - JsonConstants.MaximumDateTimeLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":"DateTime"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonConstants.MaximumDateTimeLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteStringFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteStringFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteStringFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 7 - JsonConstants.MaximumDateTimeLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "DateTime"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 7 + JsonConstants.MaximumDateTimeLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref propertyName, value);
            else
                WriteStringFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFast(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 6 - JsonConstants.MaximumDateTimeLength >= 0);

            // Calculated based on the following: ',"propertyName":"DateTime"'
            int bytesNeeded = propertyName.Length + 6 + JsonConstants.MaximumDateTimeLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlow(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteStringFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteStringFast(ref propertyName, value);
            }
        }

        private void WriteStringFormatted(ref ReadOnlySpan<byte> propertyName, DateTime value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 7 - JsonConstants.MaximumDateTimeLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "DateTime"'
            int bytesNeeded = propertyName.Length + 7 + JsonConstants.MaximumDateTimeLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(string propertyName, DateTimeOffset value)
            => WriteString(propertyName, value.DateTime);

        public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value)
            => WriteString(propertyName, value.DateTime);

        public void WriteString(ReadOnlySpan<byte> propertyName, DateTimeOffset value)
            => WriteString(propertyName, value.DateTime);

        public void WriteString(string propertyName, Guid value)
            => WriteString(propertyName.AsSpan(), value);

        public void WriteString(ReadOnlySpan<char> propertyName, Guid value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, Guid value)
        {
            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(ref propertyName, value);
            else
                WriteStringFastWithEncoding(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFastWithEncoding(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 6 - JsonConstants.MaximumGuidLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":"Guid"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonConstants.MaximumGuidLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteStringFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteStringFastWithEncoding(ref propertyName, value);
            }
        }

        private void WriteStringFormattedWithEncoding(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 7 - JsonConstants.MaximumGuidLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "Guid"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 7 + JsonConstants.MaximumGuidLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, Guid value)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref propertyName, value);
            else
                WriteStringFast(ref propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFast(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 6 - JsonConstants.MaximumGuidLength >= 0);

            // Calculated based on the following: ',"propertyName":"Guid"'
            int bytesNeeded = propertyName.Length + 6 + JsonConstants.MaximumGuidLength;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlow(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteStringFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteStringFast(ref propertyName, value);
            }
        }

        private void WriteStringFormatted(ref ReadOnlySpan<byte> propertyName, Guid value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 7 - JsonConstants.MaximumGuidLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "Guid"'
            int bytesNeeded = propertyName.Length + 7 + JsonConstants.MaximumGuidLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        // TODO: Add tests
        public void WriteBytesUnchecked(string propertyName, ReadOnlySpan<byte> utf8Bytes)
            => WriteBytesUnchecked(propertyName.AsSpan(), utf8Bytes);

        public void WriteBytesUnchecked(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, utf8Bytes);

            WriteBytesWithEncodingProperty(MemoryMarshal.AsBytes(propertyName), utf8Bytes);
        }

        private void WriteBytesWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            if (_writerOptions.SlowPath)
                WriteBytesSlowWithEncodingProperty(ref propertyName, ref utf8Bytes);
            else
                WriteBytesFastWithEncodingProperty(ref propertyName, ref utf8Bytes);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.None;
        }

        private void WriteBytesFastWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - utf8Bytes.Length - 4 >= 0);

            // Calculated based on the following: ',"encoded propertyName":utf8Bytes'
            int bytesNeeded = propertyName.Length / 2 * 3 + utf8Bytes.Length + 4;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            utf8Bytes.CopyTo(_buffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        private void WriteBytesSlowWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(ref propertyName);
                }
                WriteBytesFormattedWithEncodingProperty(ref propertyName, ref utf8Bytes);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(ref propertyName);
                WriteBytesFastWithEncodingProperty(ref propertyName, ref utf8Bytes);
            }
        }

        private void WriteBytesFormattedWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - utf8Bytes.Length - 5 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": utf8Bytes'
            int bytesNeeded = propertyName.Length / 2 * 3 + utf8Bytes.Length + 5 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        public void WriteBytesUnchecked(ref ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            if (_writerOptions.SlowPath)
                WriteBytesSlow(ref propertyName, ref utf8Bytes);
            else
                WriteBytesFast(ref propertyName, ref utf8Bytes);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.None;
        }

        private void WriteBytesFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - utf8Bytes.Length - 4 >= 0);

            // Calculated based on the following: ',"encoded propertyName":utf8Bytes'
            int bytesNeeded = propertyName.Length + utf8Bytes.Length + 4;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Ensure(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            utf8Bytes.CopyTo(_buffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        private void WriteBytesSlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(ref propertyName);
                }
                WriteBytesFormatted(ref propertyName, ref utf8Bytes);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(ref propertyName);
                WriteBytesFast(ref propertyName, ref utf8Bytes);
            }
        }

        private void WriteBytesFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> utf8Bytes)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - utf8Bytes.Length - 5 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": utf8Bytes'
            int bytesNeeded = propertyName.Length + utf8Bytes.Length + 5 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(ref propertyName, bytesNeeded, indent, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePropertyNameAndDepth(ref ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize || CurrentDepth >= JsonConstants.MaxPossibleDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WritePropertyNameEncoded(ref ReadOnlySpan<byte> propertyName, int bytesNeeded, out int idx)
        {
            idx = 0;

            if (_currentDepth < 0)
                _buffer[idx++] = JsonConstants.ListSeperator;

            _buffer[idx++] = JsonConstants.Quote;

            OperationStatus status = Encodings.Utf16.ToUtf8(propertyName, _buffer.Slice(idx), out int consumed, out int written);
            Debug.Assert(status != OperationStatus.DestinationTooSmall);

            if (status != OperationStatus.Done)
                JsonThrowHelper.ThrowArgumentExceptionInvalidUtf8String();

            Debug.Assert(consumed == propertyName.Length);
            idx += written;

            _buffer[idx++] = JsonConstants.Quote;

            _buffer[idx++] = JsonConstants.KeyValueSeperator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyNameEncodedAndFormatted(ref ReadOnlySpan<byte> propertyName, int bytesNeeded, int indent, out int idx)
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
        private void WritePropertyName(ref ReadOnlySpan<byte> propertyName, int bytesNeeded, out int idx)
        {
            idx = 0;

            if (_currentDepth < 0)
                _buffer[idx++] = JsonConstants.ListSeperator;

            _buffer[idx++] = JsonConstants.Quote;
            propertyName.CopyTo(_buffer.Slice(idx));
            idx += propertyName.Length;
            _buffer[idx++] = JsonConstants.Quote;

            _buffer[idx++] = JsonConstants.KeyValueSeperator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> WritePropertyNameFormatted(ref ReadOnlySpan<byte> propertyName, int bytesNeeded, int indent, out int idx)
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
