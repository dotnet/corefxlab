// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

            _bufferWriter.Advance(idx);
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

            _bufferWriter.Advance(idx);
        }

        public void WriteValue(bool value)
        {

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

            _bufferWriter.Advance(idx);
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

            _bufferWriter.Advance(idx);
        }

        public void WriteValue(uint value)
            => WriteValue((ulong)value);

        public void WriteValue(ulong value)
        {

        }

        public void WriteValue(double value)
        {

        }

        public void WriteValue(float value)
        {

        }

        public void WriteValue(decimal value)
        {

        }

        public void WriteValue(string utf16Text)
        {

        }

        public void WriteValue(ReadOnlySpan<char> utf16Text)
        {

        }

        public void WriteValue(ReadOnlySpan<byte> utf8Text)
        {

        }

        public void WriteValue(DateTime value)
        {

        }

        public void WriteValue(DateTimeOffset value)
        {

        }

        public void WriteValue(Guid value)
        {

        }

        public void WriteBytesUnchecked(ReadOnlySpan<byte> utf8Bytes)
        {

        }

        public void WriteComments(string comment)
            => WriteComments(comment.AsSpan());

        public void WriteComments(ReadOnlySpan<char> comment)
        {

        }

        public void WriteComments(ReadOnlySpan<byte> comment)
        {

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
                WriteNewLine(byteBuffer, ref idx);

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
        }
    }
}
