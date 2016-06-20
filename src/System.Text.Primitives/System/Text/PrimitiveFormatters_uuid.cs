// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatters
    {
        public static bool TryFormat(this Guid value, Span<byte> buffer, Span<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this Guid value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'D' || format.Symbol == 'N' || format.Symbol == 'B' || format.Symbol == 'P');
            bool dash = true;
            char tail = '\0';
            bytesWritten = 0;

            switch (format.Symbol)
            {
                case 'D':
                case 'G':
                    break;

                case 'N':
                    dash = false;
                    break;

                case 'B':
                    if (!TryWriteChar('{', buffer, formattingData, ref bytesWritten)) { return false; }
                    tail = '}';
                    break;

                case 'P':
                    if (!TryWriteChar('(', buffer, formattingData, ref bytesWritten)) { return false; }
                    tail = ')';
                    break;

                default:
                    Precondition.Require(false); // how did we get here? 
                    break;
            }


            var byteFormat = new Format.Parsed('x', 2);
            unsafe
            {
                byte* bytes = (byte*)&value;

                if (!TryWriteByte(bytes[3], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[2], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[1], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[0], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
                }

                if (!TryWriteByte(bytes[5], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[4], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
                }

                if (!TryWriteByte(bytes[7], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[6], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
                }

                if (!TryWriteByte(bytes[8], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[9], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
                }

                if (!TryWriteByte(bytes[10], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[11], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[12], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[13], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[14], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteByte(bytes[15], buffer, byteFormat, formattingData, ref bytesWritten)) { return false; }
            }

            if (tail != '\0')
            {
                if (!TryWriteChar(tail, buffer, formattingData, ref bytesWritten)) { return false; }
            }

            return true;
        }

        // the following two helpers are more compact APIs for formatting.
        // the public APis cannot be like this because we cannot trust them to always do the right thing with bytesWritten
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteByte(byte b, Span<byte> buffer, Format.Parsed byteFormat, FormattingData formattingData, ref int bytesWritten)
        {
            int written;
            if (!b.TryFormat(buffer.Slice(bytesWritten), byteFormat, formattingData, out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt32(int i, Span<byte> buffer, Format.Parsed byteFormat, FormattingData formattingData, ref int bytesWritten)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), byteFormat, formattingData, out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt64(long i, Span<byte> buffer, Format.Parsed byteFormat, FormattingData formattingData, ref int bytesWritten)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), byteFormat, formattingData, out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteChar(char c, Span<byte> buffer, FormattingData formattingData, ref int bytesWritten)
        {
            int written;
            if (!c.TryFormat(buffer.Slice(bytesWritten), default(Format.Parsed), formattingData, out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteString(string s, Span<byte> buffer, FormattingData formattingData, ref int bytesWritten)
        {
            int written;
            if (!s.TryFormat(buffer.Slice(bytesWritten), default(Format.Parsed), formattingData, out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }
    }
}
