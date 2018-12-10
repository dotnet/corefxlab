﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
    {
        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<int> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<int> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumInt64Length + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumInt64Length) - 1);

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
                _currentDepth |= 1 << 31;

                Advance(idx);
            }
            else
            {
                WriteArrayFastIterate(ref propertyName, ref values);
            }
        }

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<int> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<int> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<int> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
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
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumInt64Length + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumInt64Length) - 1);

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
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
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
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<long> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<uint> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<uint> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumUInt64Length + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumUInt64Length) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = JsonWriterHelper.TryFormatUInt64Default(values[0], _buffer.Slice(idx), out int bytesWritten);
                // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
                // See: https://github.com/dotnet/corefx/issues/25425
                // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;

                    result = JsonWriterHelper.TryFormatUInt64Default(values[i], _buffer.Slice(idx), out bytesWritten);
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

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<uint> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<uint> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<uint> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<ulong> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<ulong> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumUInt64Length + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumUInt64Length) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = JsonWriterHelper.TryFormatUInt64Default(values[0], _buffer.Slice(idx), out int bytesWritten);
                // Using Utf8Formatter with default StandardFormat is roughly 30% slower (17 ns versus 12 ns)
                // See: https://github.com/dotnet/corefx/issues/25425
                // bool result = Utf8Formatter.TryFormat(value, byteBuffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;

                    result = JsonWriterHelper.TryFormatUInt64Default(values[i], _buffer.Slice(idx), out bytesWritten);
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

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<ulong> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<ulong> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<ulong> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<decimal> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<decimal> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumDecimalLength + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumDecimalLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
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

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<decimal> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<decimal> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<decimal> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<double> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<double> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumDoubleLength + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumDoubleLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
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

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<double> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<double> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<double> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<float> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<float> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumSingleLength + 1))
            {
                // Calculated based on the following: '"propertyName":[number0,number1,...,numberN]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (1 + JsonConstants.MaximumSingleLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
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

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<float> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<float> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<float> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<Guid> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<Guid> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumGuidLength + 3))
            {
                // Calculated based on the following: '"propertyName":["value0","value1",...,"valueN"]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (3 + JsonConstants.MaximumGuidLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                _buffer[idx++] = JsonConstants.Quote;
                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;
                _buffer[idx++] = JsonConstants.Quote;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    _buffer[idx++] = JsonConstants.Quote;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
                    Debug.Assert(result);
                    idx += bytesWritten;
                    _buffer[idx++] = JsonConstants.Quote;
                }

                _buffer[idx++] = JsonConstants.CloseBracket;

                Advance(idx);
            }
            else
            {
                WriteArrayFastIterate(ref propertyName, ref values);
            }
        }

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<Guid> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<Guid> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<Guid> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<DateTime> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTime> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumDateTimeLength + 3))
            {
                // Calculated based on the following: '"propertyName":["value0","value1",...,"valueN"]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (3 + JsonConstants.MaximumDateTimeLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                _buffer[idx++] = JsonConstants.Quote;
                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;
                _buffer[idx++] = JsonConstants.Quote;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    _buffer[idx++] = JsonConstants.Quote;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
                    Debug.Assert(result);
                    idx += bytesWritten;
                    _buffer[idx++] = JsonConstants.Quote;
                }

                _buffer[idx++] = JsonConstants.CloseBracket;

                Advance(idx);
            }
            else
            {
                WriteArrayFastIterate(ref propertyName, ref values);
            }
        }

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTime> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTime> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTime> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        public void WriteArray(ReadOnlySpan<byte> propertyName, ReadOnlySpan<DateTimeOffset> values)
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

        private void WriteArrayFast(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTimeOffset> values)
        {
            if (values.Length > 0 && values.Length < (int.MaxValue - 5 - propertyName.Length) / (JsonConstants.MaximumDateTimeOffsetLength + 3))
            {
                // Calculated based on the following: '"propertyName":["value0","value1",...,"valueN"]'
                int bytesNeeded = propertyName.Length + 5 + (values.Length * (3 + JsonConstants.MaximumDateTimeOffsetLength) - 1);

                if (_currentDepth >= 0)
                    bytesNeeded--;

                Ensure(bytesNeeded);

                WritePropertyName(ref propertyName, bytesNeeded, out int idx);

                _buffer[idx++] = JsonConstants.OpenBracket;

                _buffer[idx++] = JsonConstants.Quote;
                bool result = Utf8Formatter.TryFormat(values[0], _buffer.Slice(idx), out int bytesWritten);
                Debug.Assert(result);
                idx += bytesWritten;
                _buffer[idx++] = JsonConstants.Quote;

                for (int i = 1; i < values.Length; i++)
                {
                    _buffer[idx++] = JsonConstants.ListSeperator;
                    _buffer[idx++] = JsonConstants.Quote;
                    result = Utf8Formatter.TryFormat(values[i], _buffer.Slice(idx), out bytesWritten);
                    Debug.Assert(result);
                    idx += bytesWritten;
                    _buffer[idx++] = JsonConstants.Quote;
                }

                _buffer[idx++] = JsonConstants.CloseBracket;

                Advance(idx);
            }
            else
            {
                WriteArrayFastIterate(ref propertyName, ref values);
            }
        }

        private void WriteArrayFastIterate(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTimeOffset> values)
        {
            WriteStartFast(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                WriteValueFast(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFast(values[i]);
                }
            }
            WriteEndFast(JsonConstants.CloseBracket);
        }

        private void WriteArraySlow(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTimeOffset> values)
        {
            Debug.Assert(_writerOptions.Formatted || !_writerOptions.SkipValidation);

            if (_writerOptions.Formatted)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingArrayValues();
                }
                WriteArrayFormatted(ref propertyName, ref values);
            }
            else
            {
                Debug.Assert(!_writerOptions.SkipValidation);
                ValidateWritingArrayValues();
                WriteArrayFast(ref propertyName, ref values);
            }
        }

        private void WriteArrayFormatted(ref ReadOnlySpan<byte> propertyName, ref ReadOnlySpan<DateTimeOffset> values)
        {
            WriteStartFormatted(ref propertyName, JsonConstants.OpenBracket);
            if (values.Length != 0)
            {
                _currentDepth &= JsonConstants.RemoveFlagsBitMask;
                _currentDepth++;
                WriteValueFormatted(values[0]);
                _currentDepth |= 1 << 31;
                for (int i = 1; i < values.Length; i++)
                {
                    WriteValueFormatted(values[i]);
                }
                _currentDepth--;
            }
            WriteEndFormatted(JsonConstants.CloseBracket);
        }

        private void ValidateWritingArrayValues()
        {
            if (!_inObject)
            {
                JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
            }
        }
    }
}
