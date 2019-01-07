﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        public void WriteCommentValue(string utf16Text, bool suppressEscaping = false)
            => WriteCommentValue(utf16Text.AsSpan(), suppressEscaping);

        public void WriteCommentValue(ReadOnlySpan<char> utf16Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf16Text);

            if (!suppressEscaping)
                WriteCommentSuppressFalse(ref utf16Text);
            else
                WriteCommentByOptions(ref utf16Text);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteCommentSuppressFalse(ref ReadOnlySpan<char> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);

            Debug.Assert(valueIdx >= -1 && valueIdx < int.MaxValue / 2);

            if (valueIdx != -1)
            {
                WriteCommentEscapeValue(ref value, valueIdx);
            }
            else
            {
                WriteCommentByOptions(ref value);
            }
        }

        private void WriteCommentByOptions(ref ReadOnlySpan<char> value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteCommentIndented(ref value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteCommentMinimized(ref value);
            }
        }

        private void WriteCommentMinimized(ref ReadOnlySpan<char> escapedValue)
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

            WriteCommentValue(ref escapedValue, ref idx);

            Advance(idx);
        }

        private void WriteCommentIndented(ref ReadOnlySpan<char> escapedValue)
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

            WriteCommentValue(ref escapedValue, ref idx);

            Advance(idx);
        }

        private void WriteCommentEscapeValue(ref ReadOnlySpan<char> value, int firstEscapeIndexVal)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);

            char[] valueArray = null;

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
                JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndexVal, out int written);
                value = span.Slice(0, written);
            }

            WriteCommentByOptions(ref value);

            if (valueArray != null)
                ArrayPool<char>.Shared.Return(valueArray);
        }

        public void WriteCommentValue(ReadOnlySpan<byte> utf8Text, bool suppressEscaping = false)
        {
            JsonWriterHelper.ValidateValue(ref utf8Text);

            if (!suppressEscaping)
                WriteCommentSuppressFalse(ref utf8Text);
            else
                WriteCommentByOptions(ref utf8Text);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteCommentSuppressFalse(ref ReadOnlySpan<byte> value)
        {
            int valueIdx = JsonWriterHelper.NeedsEscaping(value);

            Debug.Assert(valueIdx >= -1 && valueIdx < int.MaxValue / 2);

            if (valueIdx != -1)
            {
                WriteCommentEscapeValue(ref value, valueIdx);
            }
            else
            {
                WriteCommentByOptions(ref value);
            }
        }

        private void WriteCommentByOptions(ref ReadOnlySpan<byte> value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteCommentIndented(ref value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteCommentMinimized(ref value);
            }
        }

        private void WriteCommentMinimized(ref ReadOnlySpan<byte> escapedValue)
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

            WriteCommentValue(ref escapedValue, ref idx);

            Advance(idx);
        }

        private void WriteCommentIndented(ref ReadOnlySpan<byte> escapedValue)
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

            WriteCommentValue(ref escapedValue, ref idx);

            Advance(idx);
        }

        private void WriteCommentEscapeValue(ref ReadOnlySpan<byte> value, int firstEscapeIndexVal)
        {
            Debug.Assert(int.MaxValue / 6 >= value.Length);

            byte[] valueArray = null;

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
                JsonWriterHelper.EscapeString(ref value, ref span, firstEscapeIndexVal, out int written);
                value = span.Slice(0, written);
            }

            WriteCommentByOptions(ref value);

            if (valueArray != null)
                ArrayPool<byte>.Shared.Return(valueArray);
        }

        private void WriteCommentValue(ref ReadOnlySpan<char> escapedValue, ref int idx)
        {
            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Solidus;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            ReadOnlySpan<byte> byteSpan = MemoryMarshal.AsBytes(escapedValue);
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
            _buffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Solidus;
        }

        private void WriteCommentValue(ref ReadOnlySpan<byte> escapedValue, ref int idx)
        {
            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Solidus;

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            CopyLoop(ref escapedValue, ref idx);

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = (byte)'*'; // TODO: Replace with JsonConstants.Asterisk

            while (_buffer.Length <= idx)
            {
                AdvanceAndGrow(idx);
                idx = 0;
            }
            _buffer[idx++] = JsonConstants.Solidus;
        }
    }
}
