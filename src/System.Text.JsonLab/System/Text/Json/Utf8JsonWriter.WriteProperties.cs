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
        public void WriteNull(string propertyName, bool suppressEscaping = false)
            => WriteNull(propertyName.AsSpan(), suppressEscaping);

        public void WriteNull(ReadOnlySpan<char> propertyName, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNullWithEncoding(MemoryMarshal.AsBytes(propertyName), suppressEscaping);
        }

        private unsafe void WriteNullWithEncoding(ReadOnlySpan<byte> propertyName, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNullSlowWithEncoding(ref escapedValue);
            else
                WriteNullFastWithEncoding(ref escapedValue);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlowWithEncoding(ref ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNullFormattedWithEncoding(ref propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNull(ReadOnlySpan<byte> propertyName, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNullSlow(ref escapedValue);
            else
                WriteNullFast(ref escapedValue);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            JsonConstants.NullValue.CopyTo(_buffer.Slice(idx));
            idx += JsonConstants.NullValue.Length;

            Advance(idx);
        }

        private void WriteNullSlow(ref ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNullFormatted(ref propertyName);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteBoolean(string propertyName, bool value, bool suppressEscaping = false)
            => WriteBoolean(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteBooleanWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteBooleanWithEncoding(ReadOnlySpan<byte> propertyName, bool value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteBooleanSlowWithEncoding(ref escapedValue, value);
            else
                WriteBooleanFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(_buffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteBooleanFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteBoolean(ReadOnlySpan<byte> propertyName, bool value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteBooleanSlow(ref escapedValue, value);
            else
                WriteBooleanFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            valueSpan.CopyTo(_buffer.Slice(idx));
            idx += valueSpan.Length;

            Advance(idx);
        }

        private void WriteBooleanSlow(ref ReadOnlySpan<byte> propertyName, bool value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteBooleanFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(string propertyName, long value, bool suppressEscaping = false)
            => WriteNumber(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, long value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, long value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref escapedValue, value);
            else
                WriteNumberFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(ReadOnlySpan<byte> propertyName, long value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref escapedValue, value);
            else
                WriteNumberFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(string propertyName, int value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (long)value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (long)value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (long)value, suppressEscaping);

        public void WriteNumber(string propertyName, uint value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (ulong)value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, uint value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (ulong)value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<byte> propertyName, uint value, bool suppressEscaping = false)
            => WriteNumber(propertyName, (ulong)value, suppressEscaping);

        public void WriteNumber(string propertyName, ulong value, bool suppressEscaping = false)
            => WriteNumber(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, ulong value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, ulong value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref escapedValue, value);
            else
                WriteNumberFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(ReadOnlySpan<byte> propertyName, ulong value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref escapedValue, value);
            else
                WriteNumberFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(string propertyName, double value, bool suppressEscaping = false)
            => WriteNumber(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, double value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, double value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref escapedValue, value);
            else
                WriteNumberFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(ReadOnlySpan<byte> propertyName, double value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref escapedValue, value);
            else
                WriteNumberFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, double value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(string propertyName, float value, bool suppressEscaping = false)
            => WriteNumber(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, float value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, float value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref escapedValue, value);
            else
                WriteNumberFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(ReadOnlySpan<byte> propertyName, float value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref escapedValue, value);
            else
                WriteNumberFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, float value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(string propertyName, decimal value, bool suppressEscaping = false)
            => WriteNumber(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteNumber(ReadOnlySpan<char> propertyName, decimal value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteNumberWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteNumberWithEncoding(ReadOnlySpan<byte> propertyName, decimal value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlowWithEncoding(ref escapedValue, value);
            else
                WriteNumberFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlowWithEncoding(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteNumber(ReadOnlySpan<byte> propertyName, decimal value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteNumberSlow(ref escapedValue, value);
            else
                WriteNumberFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyName(ref propertyName, bytesNeeded, out int idx);

            bool result = Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        private void WriteNumberSlow(ref ReadOnlySpan<byte> propertyName, decimal value)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteNumberFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        private unsafe void WriteStringWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedPropertyName = propertyName;
            ReadOnlySpan<byte> escapedValue = value;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* propertyPtr = stackalloc byte[propertyName.Length];
                escapedPropertyName = new ReadOnlySpan<byte>(propertyPtr, propertyName.Length);

                Utf8JsonWriterHelpers.EscapeString(value, _buffer, out _, out _);
                byte* valuePtr = stackalloc byte[value.Length];
                escapedValue = new ReadOnlySpan<byte>(valuePtr, value.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(ref escapedPropertyName, ref escapedValue);
            else
                WriteStringFastWithEncodingPropertyValue(ref escapedPropertyName, ref escapedValue);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringSlowWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncodingPropertyValue(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
                WriteStringFastWithEncodingPropertyValue(ref propertyName, ref escapedValue);
            }
        }

        private void WriteStringSlowWithEncodingPropertyValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncodingPropertyValue(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
                WriteStringFastWithEncodingPropertyValue(ref propertyName, ref escapedValue);
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

        private unsafe void WriteStringWithEncodingProperty(ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedPropertyName = propertyName;
            ReadOnlySpan<byte> escapedValue = value;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* propertyPtr = stackalloc byte[propertyName.Length];
                escapedPropertyName = new ReadOnlySpan<byte>(propertyPtr, propertyName.Length);

                Utf8JsonWriterHelpers.EscapeString(value, _buffer, out _, out _);
                byte* valuePtr = stackalloc byte[value.Length];
                escapedValue = new ReadOnlySpan<byte>(valuePtr, value.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingProperty(ref escapedPropertyName, ref escapedValue);
            else
                WriteStringFastWithEncodingProperty(ref escapedPropertyName, ref escapedValue);

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

            CheckSizeAndGrow(bytesNeeded);

            WritePropertyNameEncoded(ref propertyName, bytesNeeded, out int idx);

            _buffer[idx++] = JsonConstants.Quote;
            escapedValue.CopyTo(_buffer.Slice(idx));
            idx += escapedValue.Length;
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringSlowWithEncodingProperty(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncodingProperty(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        private unsafe void WriteStringWithEncodingValue(ref ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedPropertyName = propertyName;
            ReadOnlySpan<byte> escapedValue = value;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* propertyPtr = stackalloc byte[propertyName.Length];
                escapedPropertyName = new ReadOnlySpan<byte>(propertyPtr, propertyName.Length);

                Utf8JsonWriterHelpers.EscapeString(value, _buffer, out _, out _);
                byte* valuePtr = stackalloc byte[value.Length];
                escapedValue = new ReadOnlySpan<byte>(valuePtr, value.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingValue(ref escapedPropertyName, ref escapedValue);
            else
                WriteStringFastWithEncodingValue(ref escapedPropertyName, ref escapedValue);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncodingValue(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteString(string propertyName, string value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value.AsSpan(), suppressEscaping);

        public void WriteString(string propertyName, ReadOnlySpan<char> value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteString(string propertyName, ReadOnlySpan<byte> value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteString(ReadOnlySpan<char> propertyName, string value, bool suppressEscaping = false)
            => WriteString(propertyName, value.AsSpan(), suppressEscaping);

        public void WriteString_old(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingPropertyValue(MemoryMarshal.AsBytes(propertyName), MemoryMarshal.AsBytes(value), suppressEscaping);
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingPropertyValue_new(propertyName, value, suppressEscaping);
        }

        private unsafe void WriteStringWithEncodingPropertyValue_new(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value, bool suppressEscaping)
        {
            ReadOnlySpan<char> escapedPropertyName = propertyName;
            ReadOnlySpan<char> escapedValue = value;

            if (!suppressEscaping && Utf8JsonWriterHelpers.NeedsEscaping(propertyName, out int idx))
                escapedPropertyName = Utf8JsonWriterHelpers.GetEscapedSpan(propertyName, idx);

            if (Utf8JsonWriterHelpers.NeedsEscaping(value, out idx))
                escapedValue = Utf8JsonWriterHelpers.GetEscapedSpan(value, idx);

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(MemoryMarshal.AsBytes(escapedPropertyName), MemoryMarshal.AsBytes(escapedValue));
            else
                WriteStringFastWithEncodingPropertyValue(MemoryMarshal.AsBytes(escapedPropertyName), MemoryMarshal.AsBytes(escapedValue));


            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingPropertyValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int idx = 0;
            if (_currentDepth < 0)
            {
                while (_buffer.Length <= idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            int partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Encodings.Utf16.ToUtf8(propertyName.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
                idx += written;
                if (status == OperationStatus.Done)
                {
                    break;
                }
                partialConsumed += consumed;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.KeyValueSeperator;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
                idx += written;
                if (status == OperationStatus.Done)
                {
                    break;
                }
                partialConsumed += consumed;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringFastWithEncodingPropertyValue(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> escapedValue)
        {
            int idx = 0;
            if (_currentDepth < 0)
            {
                while (_buffer.Length <= idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            int partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Encodings.Utf16.ToUtf8(propertyName.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
                idx += written;
                if (status == OperationStatus.Done)
                {
                    break;
                }
                partialConsumed += consumed;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.KeyValueSeperator;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Encodings.Utf16.ToUtf8(escapedValue.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
                idx += written;
                if (status == OperationStatus.Done)
                {
                    break;
                }
                partialConsumed += consumed;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void GrowAndEnsure()
        {
            int previousSpanLength = _buffer.Length;
            GrowSpan(6);
            if (_buffer.Length <= previousSpanLength)
            {
                GrowSpan(4096);
                if (_buffer.Length <= previousSpanLength)
                {
                    throw new OutOfMemoryException();
                }
            }
        }

        private void AdvanceAndGrow(int alreadyWritten)
        {
            Advance(alreadyWritten);
            GrowAndEnsure();
        }

        private void AdvanceAndGrow(int alreadyWritten, int minimumSize)
        {
            Debug.Assert(minimumSize > 6 && minimumSize <= 128);
            Advance(alreadyWritten);
            int previousSpanLength = _buffer.Length;
            GrowSpan(minimumSize);
            if (_buffer.Length <= minimumSize)
            {
                GrowSpan(4096);
                if (_buffer.Length <= minimumSize)
                {
                    throw new OutOfMemoryException();
                }
            }
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingProperty(MemoryMarshal.AsBytes(propertyName), ref value, suppressEscaping);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, string value, bool suppressEscaping = false)
            => WriteString(propertyName, value.AsSpan(), suppressEscaping);

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            WriteStringWithEncodingValue(ref propertyName, MemoryMarshal.AsBytes(value), suppressEscaping);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            ReadOnlySpan<byte> escapedPropertyName = propertyName;
            ReadOnlySpan<byte> escapedValue = value;

            if (!suppressEscaping && Utf8JsonWriterHelpers.NeedsEscaping(propertyName, out int idx))
                escapedPropertyName = Utf8JsonWriterHelpers.GetEscapedSpan(propertyName, idx);

            if (Utf8JsonWriterHelpers.NeedsEscaping(value, out idx))
                escapedValue = Utf8JsonWriterHelpers.GetEscapedSpan(value, idx);

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref escapedPropertyName, ref escapedValue);
            else
                WriteStringFast(ref escapedPropertyName, ref escapedValue);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);

            ReadOnlySpan<byte> escapedValue = value;

            if (Utf8JsonWriterHelpers.NeedsEscaping(value, out int idx))
                escapedValue = Utf8JsonWriterHelpers.GetEscapedSpan(value, idx);

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref propertyName, ref escapedValue);
            else
                WriteStringFast(ref propertyName, ref escapedValue);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            int idx = 0;
            if (_currentDepth < 0)
            {
                while (_buffer.Length <= idx)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                }
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            if (propertyName.Length > _buffer.Length - idx)
            {
                idx = WriteInLoop(ref propertyName, idx);
            }
            else
            {
                propertyName.CopyTo(_buffer.Slice(idx));
                idx += propertyName.Length;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.KeyValueSeperator;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            if (escapedValue.Length > _buffer.Length - idx)
            {
                idx = WriteInLoop(ref escapedValue, idx);
            }
            else
            {
                escapedValue.CopyTo(_buffer.Slice(idx));
                idx += escapedValue.Length;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
            return;
        }

        private int WriteInLoop(ref ReadOnlySpan<byte> span, int idx)
        {
            int partialConsumed = 0;
            while (true)
            {
                bool result = span.Slice(partialConsumed).TryCopyTo(_buffer.Slice(idx));
                if (!result)
                {
                    AdvanceAndGrow(idx);
                    idx = 0;
                    continue;
                }
                idx += span.Length - partialConsumed;
                break;
            }
            return idx;
        }

        private void WriteStringSlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormatted(ref propertyName, ref escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
                WriteStringFast(ref propertyName, ref escapedValue);
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

        public void WriteString(string propertyName, DateTime value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteString(ReadOnlySpan<char> propertyName, DateTime value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, DateTime value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(ref escapedValue, value);
            else
                WriteStringFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteString(ReadOnlySpan<byte> propertyName, DateTime value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref escapedValue, value);
            else
                WriteStringFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteString(string propertyName, DateTimeOffset value, bool suppressEscaping = false)
            => WriteString(propertyName, value.DateTime, suppressEscaping);

        public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value, bool suppressEscaping = false)
            => WriteString(propertyName, value.DateTime, suppressEscaping);

        public void WriteString(ReadOnlySpan<byte> propertyName, DateTimeOffset value, bool suppressEscaping = false)
            => WriteString(propertyName, value.DateTime, suppressEscaping);

        public void WriteString(string propertyName, Guid value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value, suppressEscaping);

        public void WriteString(ReadOnlySpan<char> propertyName, Guid value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            WriteStringWithEncoding(MemoryMarshal.AsBytes(propertyName), value, suppressEscaping);
        }

        private unsafe void WriteStringWithEncoding(ReadOnlySpan<byte> propertyName, Guid value, bool suppressEscaping)
        {
            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                byte* ptr = stackalloc byte[propertyName.Length];
                escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
            }

            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncoding(ref escapedValue, value);
            else
                WriteStringFastWithEncoding(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncoding(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        public void WriteString(ReadOnlySpan<byte> propertyName, Guid value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateProperty(ref propertyName);

            ReadOnlySpan<byte> escapedValue = propertyName;
            if (!suppressEscaping)
            {
                Utf8JsonWriterHelpers.EscapeString(propertyName, _buffer, out _, out _);
                unsafe
                {
                    byte* ptr = stackalloc byte[propertyName.Length];
                    escapedValue = new ReadOnlySpan<byte>(ptr, propertyName.Length);
                }
            }

            if (_writerOptions.SlowPath)
                WriteStringSlow(ref escapedValue, value);
            else
                WriteStringFast(ref escapedValue, value);

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

            CheckSizeAndGrow(bytesNeeded);

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
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormatted(ref propertyName, value);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePropertyNameAndDepth(ref ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize || CurrentDepth >= JsonConstants.MaxWriterDepth)
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

        private void ValidateWritingProperty()
        {
            if (!_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.StartObject);
                JsonThrowHelper.ThrowJsonWriterException("Cannot add a property within an array or as the first JSON token.");    //TODO: Add resouce message
            }
        }
    }
}
