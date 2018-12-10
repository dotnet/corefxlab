// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<int> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<long> values)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxTokenSize || CurrentDepth >= JsonConstants.MaxPossibleDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);

            if (_writerOptions.SlowPath)
                WriteArraySlow(ref propertyName, ref values);
            else
                WriteArrayFast(ref propertyName, ref values);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.EndArray;
        }

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<long> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 2) / (JsonConstants.MaximumInt64Length + 1))
            {
                // Calculated based on the following: '[number0,number1,...,numberN]'
                int bytesNeeded = 2 + values.Length * (1 + JsonConstants.MaximumInt64Length);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = JsonWriterHelper.TryFormatInt64Default(values[0], _buffer.Slice(idx), out int bytesWritten);
                // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
                // See: https://github.com/dotnet/corefx/issues/25425
                // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;

                    result = JsonWriterHelper.TryFormatInt64Default(values[i], _buffer.Slice(idx), out bytesWritten);
                    // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
                    // See: https://github.com/dotnet/corefx/issues/25425
                    // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
                    Debug.Assert(result);
                    idx += bytesWritten;
                }

                _buffer[idx++] = JsonConstants.CloseBracket;

                Advance(idx);
            }
            else
            {
                WriteArrayFastIterate(ref propertyName, ref values);
            }
        }

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<long> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            for (int i = 0; i < values.Length; i++)
            {
                WriteValueFast(values[i]);
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<long> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingValue();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<long> values)
        {
            int indent = Indentation;

            // This is guaranteed not to overflow.
            Debug.Assert(int.MaxValue - JsonConstants.MaximumInt64Length - 1 - JsonWriterHelper.NewLineUtf8.Length - indent >= 0);

            // Calculated based on the following: ',\r\n  number'
            int bytesNeeded = 1 + JsonWriterHelper.NewLineUtf8.Length + indent + JsonConstants.MaximumInt64Length;

            Span<byte> byteBuffer = WriteValueFormatted(bytesNeeded, indent, out int idx);

            bool result = JsonWriterHelper.TryFormatInt64Default(values[0], byteBuffer.Slice(idx), out int bytesWritten);
            // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
            // See: https://github.com/dotnet/corefx/issues/25425
            // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
            Debug.Assert(result);
            idx += bytesWritten;

            Advance(idx);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<uint> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<ulong> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<decimal> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<double> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<float> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<Guid> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<DateTime> values)
        {

        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<DateTimeOffset> values)
        {

        }
    }
}
