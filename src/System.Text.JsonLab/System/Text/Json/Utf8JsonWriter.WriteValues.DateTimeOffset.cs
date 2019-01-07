﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        public void WriteStringValue(DateTimeOffset value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteStringValueIndented(value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteStringValueMinimized(value);
            }

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.String;
        }

        private void WriteStringValueMinimized(DateTimeOffset value)
        {
            // Calculated based on the following: ',"DateTimeOffset value"'
            int bytesNeeded = JsonConstants.MaximumDateTimeOffsetLength + 1;
            if (_buffer.Length < bytesNeeded)
            {
                GrowAndEnsure(bytesNeeded);
            }

            int idx = 0;
            if (_currentDepth < 0)
            {
                _buffer[idx++] = JsonConstants.ListSeperator;
            }

            _buffer[idx++] = JsonConstants.Quote;

            Utf8Formatter.TryFormat(value, _buffer.Slice(idx), out int bytesWritten);
            idx += bytesWritten;

            _buffer[idx++] = JsonConstants.Quote;

            Advance(idx);
        }

        private void WriteStringValueIndented(DateTimeOffset value)
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

            WriteStringValue(value, ref idx);

            Advance(idx);
        }
    }
}
