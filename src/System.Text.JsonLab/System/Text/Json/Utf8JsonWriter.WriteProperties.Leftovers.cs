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
    }
}
