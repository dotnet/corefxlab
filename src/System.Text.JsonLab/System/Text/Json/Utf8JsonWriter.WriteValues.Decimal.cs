﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        public void WriteNumberValue(decimal value)
        {
            if (_writerOptions.Indented)
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteNumberValueIndented(value);
            }
            else
            {
                if (!_writerOptions.SkipValidation)
                {
                    ValidateWritingValue();
                }
                WriteNumberValueMinimized(value);
            }

            _currentDepth |= 1 << 31;
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberValueMinimized(decimal value)
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

            WriteNumberValueFormatLoop(value, ref idx);

            Advance(idx);
        }

        private void WriteNumberValueIndented(decimal value)
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

            WriteNumberValueFormatLoop(value, ref idx);

            Advance(idx);
        }
    }
}
