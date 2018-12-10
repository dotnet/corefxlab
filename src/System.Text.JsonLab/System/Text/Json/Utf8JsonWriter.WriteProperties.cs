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
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNullWithEncoding(MemoryMarshal.AsBytes(propertyName));
        }

        private void WriteNullWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            if (_writerOptions.SlowPath)
                WriteNullSlowWithEncoding(propertyName);
            else
                WriteNullFastWithEncoding(propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFastWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.NullValue.Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.NullValue.Length;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlowWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNullFormattedWithEncoding(propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNullFastWithEncoding(propertyName);
            }
        }

        private void WriteNullFormattedWithEncoding(ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.NullValue.Length - indent - JsonWriterHelper.NewLineUtf8.Length >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": null'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.NullValue.Length + indent + JsonWriterHelper.NewLineUtf8.Length;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        public void WriteNull(ReadOnlySpan<byte> propertyName)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNullSlow(propertyName);
            else
                WriteNullFast(propertyName);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Null;
        }

        private void WriteNullFast(ReadOnlySpan<byte> propertyName)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.NullValue.Length >= 0);

            // Calculated based on the following: ',"propertyName":null'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.NullValue.Length;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlow(ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNullFormatted(propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNullFast(propertyName);
            }
        }

        private void WriteNullFormatted(ReadOnlySpan<byte> propertyName)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.NullValue.Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": null'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.NullValue.Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            JsonConstants.NullValue.CopyTo(byteBuffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
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
            if (_writerOptions.SlowPath)
                WriteBooleanSlowWithEncoding(propertyName, value);
            else
                WriteBooleanFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFastWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
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

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlowWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteBooleanFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteBooleanFastWithEncoding(propertyName, value);
            }
        }

        private void WriteBooleanFormattedWithEncoding(ReadOnlySpan<byte> propertyName, bool value)
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

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteBooleanSlow(propertyName, value);
            else
                WriteBooleanFast(propertyName, value);

            _currentDepth |= 1 << 31;
        }

        private void WriteBooleanFast(ReadOnlySpan<byte> propertyName, bool value)
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

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlow(ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteBooleanFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteBooleanFast(propertyName, value);
            }
        }

        private void WriteBooleanFormatted(ReadOnlySpan<byte> propertyName, bool value)
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

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            valueSpan.CopyTo(byteBuffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, long value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, long value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, long value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, long value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumInt64Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumInt64Length;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            //bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, long value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, long value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

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
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, long value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumInt64Length >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumInt64Length;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            //bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, long value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, long value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

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
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, ulong value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, ulong value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumUInt64Length >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumUInt64Length;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, ulong value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, ulong value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumUInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumUInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

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
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, ulong value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumUInt64Length >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumUInt64Length;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = JsonWriterHelper.TryFormatUInt64Default(value, byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, ulong value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, ulong value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumUInt64Length - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumUInt64Length + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

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
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, double value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, double value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumDoubleLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumDoubleLength;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, double value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumDoubleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumDoubleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, double value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, double value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumDoubleLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumDoubleLength;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, double value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumDoubleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumDoubleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, float value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, float value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, float value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, float value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumSingleLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumSingleLength;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, float value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumSingleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumSingleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, float value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, float value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumSingleLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumSingleLength;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, float value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumSingleLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumSingleLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(string propertyName, decimal value)
            => WriteNumber(propertyName.AsSpan(), value);

        public void WriteNumber(ReadOnlySpan<char> propertyName, decimal value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, decimal value)
        {
            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(propertyName, value);
            else
                WriteNumberFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFastWithEncoding(ReadOnlySpan<byte> propertyName, decimal value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 4 - JsonConstants.MaximumDecimalLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 4 + JsonConstants.MaximumDecimalLength;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteNumberFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteNumberFastWithEncoding(propertyName, value);
            }
        }

        private void WriteNumberFormattedWithEncoding(ReadOnlySpan<byte> propertyName, decimal value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 5 - JsonConstants.MaximumDecimalLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": number'
            int bytesNeeded = propertyName.Length / 2 * 3 + 5 + JsonConstants.MaximumDecimalLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, decimal value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteNumberSlow(propertyName, value);
            else
                WriteNumberFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberFast(ReadOnlySpan<byte> propertyName, decimal value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 4 - JsonConstants.MaximumDecimalLength >= 0);

            // Calculated based on the following: ',"propertyName":number'
            int bytesNeeded = propertyName.Length + 4 + JsonConstants.MaximumDecimalLength;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteNumberFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteNumberFast(propertyName, value);
            }
        }

        private void WriteNumberFormatted(ReadOnlySpan<byte> propertyName, decimal value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 5 - JsonConstants.MaximumDecimalLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": number'
            int bytesNeeded = propertyName.Length + 5 + JsonConstants.MaximumDecimalLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

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

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncodingPropertyValue(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
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

            Advance(idx);
        }

        private void WriteStringWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
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

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncodingProperty(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
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

            Advance(idx);
        }

        private void WriteStringWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            //TODO: Add ReadOnlySpan<char> overload to this check
            if (JsonWriterHelper.IndexOfAnyEscape(value) != -1)
                value = JsonWriterHelper.EscapeStringValue(value);

            if (_writerOptions.SlowPath)
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

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormattedWithEncodingValue(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
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
                value = JsonWriterHelper.EscapeStringValue(value);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlow(propertyName, value);
            else
                WriteStringFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFast(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - escapedValue.Length - 6 >= 0);

            // Calculated based on the following: ',"propertyName":"value"'
            int bytesNeeded = propertyName.Length + escapedValue.Length + 6;

            if (_currentDepth >= 0)
                bytesNeeded--;

            Span<byte> byteBuffer = GetSpan(bytesNeeded);

            int idx = 0;

            if (_currentDepth < 0)
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

            Advance(idx);
        }

        private void WriteStringSlow(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormatted(propertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
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

            Advance(idx);
        }

        public void WriteString(string propertyName, DateTime value)
            => WriteString(propertyName.AsSpan(), value);

        public void WriteString(ReadOnlySpan<char> propertyName, DateTime value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(propertyName, value);
            else
                WriteStringFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFastWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 6 - JsonConstants.MaximumDateTimeLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":"DateTime"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonConstants.MaximumDateTimeLength;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteStringFastWithEncoding(propertyName, value);
            }
        }

        private void WriteStringFormattedWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 7 - JsonConstants.MaximumDateTimeLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "DateTime"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 7 + JsonConstants.MaximumDateTimeLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteStringSlow(propertyName, value);
            else
                WriteStringFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFast(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 6 - JsonConstants.MaximumDateTimeLength >= 0);

            // Calculated based on the following: ',"propertyName":"DateTime"'
            int bytesNeeded = propertyName.Length + 6 + JsonConstants.MaximumDateTimeLength;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlow(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteStringFast(propertyName, value);
            }
        }

        private void WriteStringFormatted(ReadOnlySpan<byte> propertyName, DateTime value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 7 - JsonConstants.MaximumDateTimeLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "DateTime"'
            int bytesNeeded = propertyName.Length + 7 + JsonConstants.MaximumDateTimeLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

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
            JsonWriterHelper.ValidateProperty(propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value);
        }

        private void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, Guid value)
        {
            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(propertyName, value);
            else
                WriteStringFastWithEncoding(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFastWithEncoding(ReadOnlySpan<byte> propertyName, Guid value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 6 - JsonConstants.MaximumGuidLength >= 0);

            // Calculated based on the following: ',"encoded propertyName":"Guid"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 6 + JsonConstants.MaximumGuidLength;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncoding(ReadOnlySpan<byte> propertyName, Guid value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteStringFormattedWithEncoding(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteStringFastWithEncoding(propertyName, value);
            }
        }

        private void WriteStringFormattedWithEncoding(ReadOnlySpan<byte> propertyName, Guid value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - 7 - JsonConstants.MaximumGuidLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": "Guid"'
            int bytesNeeded = propertyName.Length / 2 * 3 + 7 + JsonConstants.MaximumGuidLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, Guid value)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteStringSlow(propertyName, value);
            else
                WriteStringFast(propertyName, value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteStringFast(ReadOnlySpan<byte> propertyName, Guid value)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 6 - JsonConstants.MaximumGuidLength >= 0);

            // Calculated based on the following: ',"propertyName":"Guid"'
            int bytesNeeded = propertyName.Length + 6 + JsonConstants.MaximumGuidLength;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            byteBuffer[idx++] = JsonConstants.Quote;

            bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            byteBuffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlow(ReadOnlySpan<byte> propertyName, Guid value)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteStringFormatted(propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteStringFast(propertyName, value);
            }
        }

        private void WriteStringFormatted(ReadOnlySpan<byte> propertyName, Guid value)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - 7 - JsonConstants.MaximumGuidLength - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "propertyName": "Guid"'
            int bytesNeeded = propertyName.Length + 7 + JsonConstants.MaximumGuidLength + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

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
                WriteBytesSlowWithEncodingProperty(propertyName, utf8Bytes);
            else
                WriteBytesFastWithEncodingProperty(propertyName, utf8Bytes);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.None;
        }

        private void WriteBytesFastWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - utf8Bytes.Length - 4 >= 0);

            // Calculated based on the following: ',"encoded propertyName":utf8Bytes'
            int bytesNeeded = propertyName.Length / 2 * 3 + utf8Bytes.Length + 4;

            Span<byte> byteBuffer = WritePropertyNameEncoded(propertyName, bytesNeeded, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        private void WriteBytesSlowWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingPropertyWithEncoding(propertyName);
                }
                WriteBytesFormattedWithEncodingProperty(propertyName, utf8Bytes);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingPropertyWithEncoding(propertyName);
                WriteBytesFastWithEncodingProperty(propertyName, utf8Bytes);
            }
        }

        private void WriteBytesFormattedWithEncodingProperty(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length / 2 * 3 - utf8Bytes.Length - 5 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": utf8Bytes'
            int bytesNeeded = propertyName.Length / 2 * 3 + utf8Bytes.Length + 5 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameEncodedAndFormatted(propertyName, bytesNeeded, indent, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        public void WriteBytesUnchecked(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            JsonWriterHelper.ValidateProperty(propertyName);

            if (_writerOptions.SlowPath)
                WriteBytesSlow(propertyName, utf8Bytes);
            else
                WriteBytesFast(propertyName, utf8Bytes);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.None;
        }

        private void WriteBytesFast(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - utf8Bytes.Length - 4 >= 0);

            // Calculated based on the following: ',"encoded propertyName":utf8Bytes'
            int bytesNeeded = propertyName.Length + utf8Bytes.Length + 4;

            Span<byte> byteBuffer = WritePropertyName(propertyName, bytesNeeded, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        private void WriteBytesSlow(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty(propertyName);
                }
                WriteBytesFormatted(propertyName, utf8Bytes);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty(propertyName);
                WriteBytesFast(propertyName, utf8Bytes);
            }
        }

        private void WriteBytesFormatted(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> utf8Bytes)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - propertyName.Length - utf8Bytes.Length - 5 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  "encoded propertyName": utf8Bytes'
            int bytesNeeded = propertyName.Length + utf8Bytes.Length + 5 + JsonWriterHelper.NewLineUtf8.Length + indent;

            Span<byte> byteBuffer = WritePropertyNameFormatted(propertyName, bytesNeeded, indent, out int idx);

            utf8Bytes.CopyTo(byteBuffer.Slice(idx));
            idx += utf8Bytes.Length;

            Advance(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePropertyNameAndDepth(ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize || CurrentDepth >= JsonConstants.MaxPossibleDepth)
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
