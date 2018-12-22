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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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

        public void WriteString(string propertyName, string value, bool suppressEscaping = false)
            => WriteString(propertyName.AsSpan(), value.AsSpan(), suppressEscaping);

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(ref propertyName, ref value);

            if (!suppressEscaping)
                WriteStringSuppressFalse(ref propertyName, ref value);
            else
                WriteStringSuppressTrue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteStringSuppressTrue(ref ReadOnlySpan<char> escapedPropertyName, ref ReadOnlySpan<char> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);
            if (valueIdx != -1)
            {
                WriteStringEscapeValueOnly(ref escapedPropertyName, ref value, valueIdx);
            }
            else
            {
                WriteStringByOptions(ref escapedPropertyName, ref value);
            }
        }

        private void WriteStringEscapeValueOnly(ref ReadOnlySpan<char> escapedPropertyName, ref ReadOnlySpan<char> value, int firstEscapeIndex)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);

            char[] valueArray = ArrayPool<char>.Shared.Rent(firstEscapeIndex + 6 * (value.Length - firstEscapeIndex));
            Span<char> span = valueArray;
            JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndex, out int charsWritten);
            value = span.Slice(0, charsWritten);

            WriteStringByOptions(ref escapedPropertyName, ref value);

            ArrayPool<char>.Shared.Return(valueArray);
        }

        private void WriteStringSuppressFalse(ref ReadOnlySpan<char> propertyName, ref ReadOnlySpan<char> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(valueIdx >= -1 && valueIdx < int.MaxValue / 2);
            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            // Equivalent to: valueIdx != -1 || propertyIdx != -1
            if (valueIdx + propertyIdx != -2)
            {
                WriteStringEscapePropertyOrValue(ref propertyName, ref value, propertyIdx, valueIdx);
            }
            else
            {
                WriteStringByOptions(ref propertyName, ref value);
            }
        }

        private void WriteStringEscapePropertyOrValue(ref ReadOnlySpan<char> propertyName, ref ReadOnlySpan<char> value, int firstEscapeIndexProp, int firstEscapeIndexVal)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            char[] valueArray = null;
            char[] propertyArray = null;

            if (firstEscapeIndexVal != -1)
            {
                int length = firstEscapeIndexVal + 6 * (value.Length - firstEscapeIndexVal);
                Span<char> span;
                if (length > 256)
                {
                    valueArray = ArrayPool<char>.Shared.Rent(length);
                    span = valueArray;
                }
                else
                {
                    // Cannot create a span directly since the span gets exposed outside this method.
                    unsafe
                    {
                        char* ptr = stackalloc char[length];
                        span = new Span<char>(ptr, length);
                    }
                }
                JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndexVal, out int charsWritten);
                value = span.Slice(0, charsWritten);
            }

            if (firstEscapeIndexProp != -1)
            {
                int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
                Span<char> span;
                if (length > 256)
                {
                    propertyArray = ArrayPool<char>.Shared.Rent(length);
                    span = propertyArray;
                }
                else
                {
                    // Cannot create a span directly since the span gets exposed outside this method.
                    unsafe
                    {
                        char* ptr = stackalloc char[length];
                        span = new Span<char>(ptr, length);
                    }
                }
                JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int charsWritten);
                propertyName = span.Slice(0, charsWritten);
            }

            WriteStringByOptions(ref propertyName, ref value);

            if (valueArray != null)
                ArrayPool<char>.Shared.Return(valueArray);

            if (propertyArray != null)
                ArrayPool<char>.Shared.Return(propertyArray);
        }

        private void WriteStringByOptions(ref ReadOnlySpan<char> propertyName, ref ReadOnlySpan<char> value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringIndented(ref propertyName, ref value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringMinimized(ref propertyName, ref value);
            }
        }

        private void WriteStringByOptions(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringIndented(ref propertyName, ref value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringMinimized(ref propertyName, ref value);
            }
        }

        private void WriteStringIndented(ref ReadOnlySpan<char> escapedPropertyName, ref ReadOnlySpan<char> escapedValue)
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
                bool result = JsonWriterHelper.TryWriteIndentation(_buffer.Slice(idx), indent, out int bytesWritten);
                idx += bytesWritten;
                if (result)
                {
                    break;
                }
                indent -= bytesWritten;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            ReadOnlySpan<byte> byteSpan = MemoryMarshal.AsBytes(escapedPropertyName);
            int partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(byteSpan.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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
            _buffer[idx++] = JsonConstants.Space;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            byteSpan = MemoryMarshal.AsBytes(escapedValue);
            partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(byteSpan.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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

        private void WriteStringMinimized(ref ReadOnlySpan<char> escapedPropertyName, ref ReadOnlySpan<char> escapedValue)
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

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            ReadOnlySpan<byte> byteSpan = MemoryMarshal.AsBytes(escapedPropertyName);
            int partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(byteSpan.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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

            byteSpan = MemoryMarshal.AsBytes(escapedValue);
            partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(byteSpan.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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

        private void WriteStringWithEncodingPropertyValue(ReadOnlySpan<char> escapedPropertyName, ReadOnlySpan<char> escapedValue)
        {
            if (_writerOptions.SlowPath)
                WriteStringSlowWithEncodingPropertyValue(MemoryMarshal.AsBytes(escapedPropertyName), MemoryMarshal.AsBytes(escapedValue));
            else
                WriteStringFastWithEncodingPropertyValue(MemoryMarshal.AsBytes(escapedPropertyName), MemoryMarshal.AsBytes(escapedValue));

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringFastWithEncodingPropertyValue(ReadOnlySpan<byte> escapedPropertyName, ReadOnlySpan<byte> escapedValue)
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
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedPropertyName.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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

        private void WriteStringSlowWithEncodingPropertyValue(ReadOnlySpan<byte> escapedPropertyName, ReadOnlySpan<byte> escapedValue)
        {
            Debug.Assert(_writerOptions.Indented || !_writerOptions.SkipValidation);

            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingProperty();
                }
                WriteStringFormattedWithEncodingPropertyValue(escapedPropertyName, escapedValue);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingProperty();
                WriteStringFastWithEncodingPropertyValue(escapedPropertyName, escapedValue);
            }
        }

        private void WriteStringFormattedWithEncodingPropertyValue(ReadOnlySpan<byte> escapedPropertyName, ReadOnlySpan<byte> escapedValue)
        {
            int indent = Indentation;

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

            if (_tokenType != JsonTokenType.None)
                WriteNewLine(ref idx);

            while (true)
            {
                bool result = JsonWriterHelper.TryWriteIndentation(_buffer.Slice(idx), indent, out int bytesWritten);
                idx += bytesWritten;
                if (result)
                {
                    break;
                }
                indent -= bytesWritten;
                AdvanceAndGrow(idx);
                idx = 0;
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
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedPropertyName.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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
            _buffer[idx++] = JsonConstants.Space;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            partialConsumed = 0;
            while (true)
            {
                OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(escapedValue.Slice(partialConsumed), _buffer.Slice(idx), out int consumed, out int written);
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

        public void WriteStringSkipEscape(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(ref propertyName, ref value);

            WriteStringSuppressTrue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidatePropertyAndValue(ref propertyName, ref value);

            if (!suppressEscaping)
                WriteStringSuppressFalse(ref propertyName, ref value);
            else
                WriteStringSuppressTrue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteStringSuppressTrue(ref ReadOnlySpan<byte> escapedPropertyName, ref ReadOnlySpan<byte> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);
            if (valueIdx != -1)
            {
                WriteStringEscapeValueOnly(ref escapedPropertyName, ref value, valueIdx);
            }
            else
            {
                WriteStringByOptions(ref escapedPropertyName, ref value);
            }
        }

        private void WriteStringEscapeValueOnly(ref ReadOnlySpan<byte> escapedPropertyName, ref ReadOnlySpan<byte> value, int firstEscapeIndex)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);

            byte[] valueArray = ArrayPool<byte>.Shared.Rent(firstEscapeIndex + 6 * (value.Length - firstEscapeIndex));
            Span<byte> span = valueArray;
            JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndex, out int bytesWritten);
            value = span.Slice(0, bytesWritten);

            WriteStringByOptions(ref escapedPropertyName, ref value);

            ArrayPool<byte>.Shared.Return(valueArray);
        }

        private void WriteStringSuppressFalse(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);
            int propertyIdx = JsonWriterHelper.NeedsEscaping(propertyName);

            Debug.Assert(valueIdx >= -1 && valueIdx < int.MaxValue / 2);
            Debug.Assert(propertyIdx >= -1 && propertyIdx < int.MaxValue / 2);

            // Equivalent to: valueIdx != -1 || propertyIdx != -1
            if (valueIdx + propertyIdx != -2)
            {
                WriteStringEscapePropertyOrValue(ref propertyName, ref value, valueIdx, propertyIdx);
            }
            else
            {
                WriteStringByOptions(ref propertyName, ref value);
            }
        }

        private void WriteStringEscapePropertyOrValue(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<byte> value, int firstEscapeIndexVal, int firstEscapeIndexProp)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);
            Debug.Assert(int.MaxValue / 6 >= propertyName.Length);

            byte[] valueArray = null;
            byte[] propertyArray = null;

            if (firstEscapeIndexVal != -1)
            {
                int length = firstEscapeIndexVal + 6 * (value.Length - firstEscapeIndexVal);

                Span<byte> span;
                if (length > 256)
                {
                    valueArray = ArrayPool<byte>.Shared.Rent(length);
                    span = valueArray;
                }
                else
                {
                    // Cannot create a span directly since the span gets exposed outside this method.
                    unsafe
                    {
                        byte* ptr = stackalloc byte[length];
                        span = new Span<byte>(ptr, length);
                    }
                }
                JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndexVal, out int bytesWritten);
                value = span.Slice(0, bytesWritten);
            }

            if (firstEscapeIndexProp != -1)
            {
                int length = firstEscapeIndexProp + 6 * (propertyName.Length - firstEscapeIndexProp);
                Span<byte> span;
                if (length > 256)
                {
                    propertyArray = ArrayPool<byte>.Shared.Rent(length);
                    span = propertyArray;
                }
                else
                {
                    // Cannot create a span directly since the span gets exposed outside this method.
                    unsafe
                    {
                        byte* ptr = stackalloc byte[length];
                        span = new Span<byte>(ptr, length);
                    }
                }
                JsonWriterHelper.EscapeString(ref propertyName, ref span, firstEscapeIndexProp, out int bytesWritten);
                propertyName = span.Slice(0, bytesWritten);
            }

            WriteStringByOptions(ref propertyName, ref value);

            if (valueArray != null)
                ArrayPool<byte>.Shared.Return(valueArray);

            if (propertyArray != null)
                ArrayPool<byte>.Shared.Return(propertyArray);
        }

        private void WriteStringMinimized(ref ReadOnlySpan<byte> escapedPropertyName, ref ReadOnlySpan<byte> escapedValue)
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

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            CopyLoop(ref escapedPropertyName, ref idx);

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

            CopyLoop(ref escapedValue, ref idx);

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringIndented(ref ReadOnlySpan<byte> escapedPropertyName, ref ReadOnlySpan<byte> escapedValue)
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
                bool result = JsonWriterHelper.TryWriteIndentation(_buffer.Slice(idx), indent, out int bytesWritten);
                idx += bytesWritten;
                if (result)
                {
                    break;
                }
                indent -= bytesWritten;
                AdvanceAndGrow(idx);
                idx = 0;
            }

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            CopyLoop(ref escapedPropertyName, ref idx);

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
            _buffer[idx++] = JsonConstants.Space;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

            CopyLoop(ref escapedValue, ref idx);

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Quote;

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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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
                JsonWriterHelper.EscapeString(propertyName, _buffer, out _, out _);
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

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(propertyName, _buffer.Slice(idx), out int consumed, out int written);
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

            OperationStatus status = Buffers.Text.Encodings.Utf16.ToUtf8(propertyName, byteBuffer.Slice(idx), out int consumed, out int written);
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

        private void GrowAndEnsure()
        {
            int previousSpanLength = _buffer.Length;
            GrowSpan(4096);
            if (_buffer.Length <= previousSpanLength)
            {
                GrowSpan(4096);
                if (_buffer.Length <= previousSpanLength)
                {
                    //TODO: Use Throwhelper and fix message.
                    throw new OutOfMemoryException("Failed to get larger buffer when growing.");
                }
            }
        }

        private void AdvanceAndGrow(int alreadyWritten)
        {
            Debug.Assert(alreadyWritten >= 0);
            Advance(alreadyWritten);
            GrowAndEnsure();
        }

        private void AdvanceAndGrow(int alreadyWritten, int minimumSize)
        {
            Debug.Assert(minimumSize > 6 && minimumSize <= 128);
            Advance(alreadyWritten);
            int previousSpanLength = _buffer.Length;
            GrowSpan(4096);
            if (_buffer.Length <= minimumSize)
            {
                GrowSpan(4096);
                if (_buffer.Length <= minimumSize)
                {
                    //TODO: Use Throwhelper and fix message.
                    throw new OutOfMemoryException("Failed to get larger buffer when growing after advancing.");
                }
            }
        }

        private void CopyLoop(ref ReadOnlySpan<byte> span, ref int idx)
        {
            while (true)
            {
                if (span.Length <= _buffer.Length - idx)
                {
                    span.CopyTo(_buffer.Slice(idx));
                    idx += span.Length;
                    break;
                }

                span.Slice(0, _buffer.Length - idx).CopyTo(_buffer.Slice(idx));
                span = span.Slice(_buffer.Length - idx);
                AdvanceAndGrow(_buffer.Length);
                idx = 0;
            }
        }
    }
}
