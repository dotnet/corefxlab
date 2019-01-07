// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
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
