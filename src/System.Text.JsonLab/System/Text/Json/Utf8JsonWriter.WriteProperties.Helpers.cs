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
        public void WriteStringSkipEscape(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            JsonWriterHelper.ValidatePropertyAndValue(ref propertyName, ref value);

            WriteStringSuppressTrue(ref propertyName, ref value);

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePropertyNameAndDepth(ref ReadOnlySpan<char> propertyName)
        {
            // TODO: Use throw helper with proper error messages
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize || CurrentDepth >= JsonConstants.MaxWriterDepth)
                JsonThrowHelper.ThrowJsonWriterOrArgumentException(propertyName, _currentDepth);
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

        private void CopyLoop(ReadOnlySpan<byte> span, ref int idx)
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

        private int WritePropertyNameMinimized(ref ReadOnlySpan<byte> escapedPropertyName)
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

            return idx;
        }

        private int WritePropertyNameIndented(ref ReadOnlySpan<byte> escapedPropertyName)
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

            return idx;
        }

        private int WritePropertyNameMinimized(ref ReadOnlySpan<char> escapedPropertyName)
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

            return idx;
        }

        private int WritePropertyNameIndented(ref ReadOnlySpan<char> escapedPropertyName)
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

            return idx;
        }
    }
}
