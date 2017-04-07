// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormatSlow(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 'D' || format.Symbol == 'N' || format.Symbol == 'B' || format.Symbol == 'P');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

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
                    if (!TryWriteChar('{', buffer, ref bytesWritten, encoder)) { return false; }
                    tail = '}';
                    break;

                case 'P':
                    if (!TryWriteChar('(', buffer, ref bytesWritten, encoder)) { return false; }
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

                if (!TryWriteByte(bytes[3], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[2], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[1], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[0], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoder)) { return false; }
                }

                if (!TryWriteByte(bytes[5], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[4], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoder)) { return false; }
                }

                if (!TryWriteByte(bytes[7], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[6], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoder)) { return false; }
                }

                if (!TryWriteByte(bytes[8], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[9], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }

                if (dash)
                {
                    if (!TryWriteChar('-', buffer, ref bytesWritten, encoder)) { return false; }
                }

                if (!TryWriteByte(bytes[10], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[11], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[12], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[13], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[14], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
                if (!TryWriteByte(bytes[15], buffer, ref bytesWritten, byteFormat, encoder)) { return false; }
            }

            if (tail != '\0')
            {
                if (!TryWriteChar(tail, buffer, ref bytesWritten, encoder)) { return false; }
            }

            return true;
        }

        // the following two helpers are more compact APIs for formatting.
        // the public APis cannot be like this because we cannot trust them to always do the right thing with bytesWritten
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryWriteByte(byte b, Span<byte> buffer, ref int bytesWritten, TextFormat byteFormat, TextEncoder encoder)
        {
            int written;
            if (!b.TryFormat(buffer.Slice(bytesWritten), out written, byteFormat, encoder))
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;
            return true;
        }
    }
}
