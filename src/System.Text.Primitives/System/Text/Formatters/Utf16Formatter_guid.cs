// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class Utf16Formatter
    {
        #region Constants

        private const int GuidChars = 32;

        private const char OpenBrace = '{';
        private const char CloseBrace = '}';

        private const char OpenParen = '(';
        private const char CloseParen = ')';

        private const char Dash = '-';

        #endregion Constants

        public static unsafe bool TryFormat(Guid value, Span<byte> buffer, out int bytesWritten, StandardFormat format = default)
        {
            bool dash = format.Symbol != 'N';
            bool bookEnds = (format.Symbol == 'B') || (format.Symbol == 'P');

            bytesWritten = (GuidChars + (dash ? 4 : 0) + (bookEnds ? 2 : 0)) * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);
            byte* bytes = (byte*)&value;
            int idx = 0;

            if (bookEnds && format.Symbol == 'B')
                Unsafe.Add(ref utf16Bytes, idx++) = OpenBrace;
            else if (bookEnds && format.Symbol == (byte)'P')
                Unsafe.Add(ref utf16Bytes, idx++) = OpenParen;

            FormattingHelpers.WriteHexByte(bytes[3], ref utf16Bytes, idx);
            FormattingHelpers.WriteHexByte(bytes[2], ref utf16Bytes, idx + 2);
            FormattingHelpers.WriteHexByte(bytes[1], ref utf16Bytes, idx + 4);
            FormattingHelpers.WriteHexByte(bytes[0], ref utf16Bytes, idx + 6);
            idx += 8;

            if (dash)
                Unsafe.Add(ref utf16Bytes, idx++) = Dash;

            FormattingHelpers.WriteHexByte(bytes[5], ref utf16Bytes, idx);
            FormattingHelpers.WriteHexByte(bytes[4], ref utf16Bytes, idx + 2);
            idx += 4;

            if (dash)
                Unsafe.Add(ref utf16Bytes, idx++) = Dash;

            FormattingHelpers.WriteHexByte(bytes[7], ref utf16Bytes, idx);
            FormattingHelpers.WriteHexByte(bytes[6], ref utf16Bytes, idx + 2);
            idx += 4;

            if (dash)
                Unsafe.Add(ref utf16Bytes, idx++) = Dash;

            FormattingHelpers.WriteHexByte(bytes[8], ref utf16Bytes, idx);
            FormattingHelpers.WriteHexByte(bytes[9], ref utf16Bytes, idx + 2);
            idx += 4;

            if (dash)
                Unsafe.Add(ref utf16Bytes, idx++) = Dash;

            FormattingHelpers.WriteHexByte(bytes[10], ref utf16Bytes, idx);
            FormattingHelpers.WriteHexByte(bytes[11], ref utf16Bytes, idx + 2);
            FormattingHelpers.WriteHexByte(bytes[12], ref utf16Bytes, idx + 4);
            FormattingHelpers.WriteHexByte(bytes[13], ref utf16Bytes, idx + 6);
            FormattingHelpers.WriteHexByte(bytes[14], ref utf16Bytes, idx + 8);
            FormattingHelpers.WriteHexByte(bytes[15], ref utf16Bytes, idx + 10);
            idx += 12;

            if (bookEnds && format.Symbol == 'B')
                Unsafe.Add(ref utf16Bytes, idx++) = CloseBrace;
            else if (bookEnds && format.Symbol == 'P')
                Unsafe.Add(ref utf16Bytes, idx++) = CloseParen;

            return true;
        }
    }
}
