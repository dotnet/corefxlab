// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormat(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), EncodingData encoding = default(EncodingData))
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
                    if (!TryWriteChar('{', buffer, ref bytesWritten, encoding)) { return false; }
                    tail = '}';
                    break;

                case 'P':
                    if (!TryWriteChar('(', buffer, ref bytesWritten, encoding)) { return false; }
                    tail = ')';
                    break;

                default:
                    Precondition.Require(false); // how did we get here? 
                    break;
            }


            var byteFormat = new TextFormat('x', 2);
            unsafe
            {
                byte* bytes = (byte*)&value;

                if (!TryWriteByte(bytes[3], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[2], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[1], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[0], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
                }

                if (!TryWriteByte(bytes[5], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[4], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
                }

                if (!TryWriteByte(bytes[7], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[6], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
                }

                if (!TryWriteByte(bytes[8], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[9], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
                }

                if (!TryWriteByte(bytes[10], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[11], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[12], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[13], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[14], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
                if (!TryWriteByte(bytes[15], buffer, ref bytesWritten, byteFormat, encoding)) { return false; }
            }

            if (tail != '\0')
            {
                if (!TryWriteChar(tail, buffer, ref bytesWritten, encoding)) { return false; }
            }

            return true;
        }

        // the following two helpers are more compact APIs for formatting.
        // the public APis cannot be like this because we cannot trust them to always do the right thing with bytesWritten
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteByte(byte b, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, EncodingData encoding)
        {
            int written;
            if (!b.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoding))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt32(int i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, EncodingData encoding)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoding))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt64(long i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, EncodingData encoding)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoding))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteChar(char character, Span<byte> buffer, ref int bytesWritten, EncodingData encoding)
        {
            int consumed;
            int written;

            unsafe
            {
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&character, 1);

                if (!encoding.TextEncoder.TryEncode(charSpan, buffer.Slice(bytesWritten), out consumed, out written))
                {
                    bytesWritten = 0;
                    return false;
                }
            }

            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteString(string text, Span<byte> buffer, ref int bytesWritten, EncodingData encoding)
        {
            int written;
            if (!encoding.TextEncoder.TryEncode(text, buffer.Slice(bytesWritten), out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }
    }
}
