// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        private const string HexTable = "0123456789abcdef";
        private const int GuidChars = 32;

        public static unsafe bool TryFormat(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            bool dash = format.Symbol != 'N';
            bool bookEnds = (format.Symbol == 'B') || (format.Symbol == 'P');
            int bytesNeeded = GuidChars + (dash ? 4 : 0) + (bookEnds ? 2 : 0);

            byte* chars = stackalloc byte[bytesNeeded];
            byte* bytes = (byte*)&value;
            int idx = 0;

            if (bookEnds && format.Symbol == 'B')
                *(chars + idx++) = (byte)'{';
            else if (bookEnds && format.Symbol == (byte)'P')
                *(chars + idx++) = (byte)'(';

            WriteHexByte(bytes[3], chars, idx++);
            WriteHexByte(bytes[2], chars, idx+2);
            WriteHexByte(bytes[1], chars, idx+4);
            WriteHexByte(bytes[0], chars, idx+6);
            idx += 8;

            if (dash)
                *(chars + idx++) = (byte)'-';

            WriteHexByte(bytes[5], chars, idx);
            WriteHexByte(bytes[4], chars, idx+2);
            idx += 4;

            if (dash)
                *(chars + idx++) = (byte)'-';

            WriteHexByte(bytes[7], chars, idx);
            WriteHexByte(bytes[6], chars, idx+2);
            idx += 4;

            if (dash)
                *(chars + idx++) = (byte)'-';

            WriteHexByte(bytes[8], chars, idx);
            WriteHexByte(bytes[9], chars, idx+2);
            idx += 4;

            if (dash)
                *(chars + idx++) = (byte)'-';

            WriteHexByte(bytes[10], chars, idx);
            WriteHexByte(bytes[11], chars, idx+2);
            WriteHexByte(bytes[12], chars, idx+4);
            WriteHexByte(bytes[13], chars, idx+6);
            WriteHexByte(bytes[14], chars, idx+8);
            WriteHexByte(bytes[15], chars, idx+10);
            idx += 12;

            if (bookEnds && format.Symbol == 'B')
                *(chars + idx++) = (byte)'}';
            else if (bookEnds && format.Symbol == 'P')
                *(chars + idx++) = (byte)')';

            int consumed;
            Span<byte> utf8 = new Span<byte>(chars, bytesNeeded);
            return encoder.TryEncode(utf8, buffer, out consumed, out bytesWritten);

        }

        // This method assumes the buffer passed starting at index has space for at least 2 more chars.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteHexByte(byte value, byte* buffer, int index)
        {
            *(buffer + index) = (byte)HexTable[value >> 4];
            *(buffer + index + 1) = (byte)HexTable[value & 0xF];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt32(int i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, TextEncoder encoder)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoder))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteInt64(long i, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, TextEncoder encoder)
        {
            int written;
            if (!i.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoder))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteChar(char character, Span<byte> buffer, ref int bytesWritten, TextEncoder encoder)
        {
            int consumed;
            int written;

            unsafe
            {
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&character, 1);

                if (!encoder.TryEncode(charSpan, buffer.Slice(bytesWritten), out consumed, out written))
                {
                    bytesWritten = 0;
                    return false;
                }
            }

            bytesWritten += written;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteString(string text, Span<byte> buffer, ref int bytesWritten, TextEncoder encoder)
        {
            int written;
            if (!encoder.TryEncode(text, buffer.Slice(bytesWritten), out written))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }
    }
}
