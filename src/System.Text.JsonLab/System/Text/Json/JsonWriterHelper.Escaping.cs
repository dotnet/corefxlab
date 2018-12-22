// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static partial class JsonWriterHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NeedsEscaping(ReadOnlySpan<byte> value)
        {
            int idx;
            for (idx = 0; idx < value.Length; idx++)
            {
                if (NeedsEscaping(value[idx]))
                {
                    goto Return;
                }
            }

            idx = -1; // all characters allowed

        Return:
            return idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NeedsEscaping(ReadOnlySpan<char> value)
        {
            int idx;
            for (idx = 0; idx < value.Length; idx++)
            {
                if (NeedsEscaping(value[idx]))
                {
                    goto Return;
                }
            }

            idx = -1; // all characters allowed

        Return:
            return idx;
        }

        // TODO: Remmove this
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static OperationStatus EscapeString(ReadOnlySpan<byte> value, Span<byte> destination, out int consumed, out int bytesWritten)
        {
            throw new NotImplementedException();
        }

        public static void EscapeString(ref ReadOnlySpan<byte> value, ref Span<byte> destination, int indexOfFirstByteToEscape, out int written)
        {
            Debug.Assert(indexOfFirstByteToEscape >= 0 && indexOfFirstByteToEscape < value.Length);

            value.Slice(0, indexOfFirstByteToEscape).CopyTo(destination);
            written = indexOfFirstByteToEscape;
            int consumed = indexOfFirstByteToEscape;

            while (consumed < value.Length)
            {
                byte val = value[consumed];
                if (NeedsEscaping(val))
                {
                    consumed += EscapeNextBytes(value.Slice(consumed), ref destination, ref written);
                }
                else
                {
                    destination[written] = val;
                    written++;
                    consumed++;
                }
            }
        }

        private static int EscapeNextBytes(ReadOnlySpan<byte> value, ref Span<byte> destination, ref int written)
        {
            SequenceValidity status = Utf8Utility.PeekFirstSequence(value, out int numBytesConsumed, out UnicodeScalar unicodeScalar);
            if (status != SequenceValidity.WellFormed)
                JsonThrowHelper.ThrowJsonWriterException("Invalid UTF-8 string.");

            destination[written++] = (byte)'\\';
            int scalar = unicodeScalar.Value;
            switch (scalar)
            {
                case '\n':
                    destination[written++] = (byte)'n';
                    break;
                case '\r':
                    destination[written++] = (byte)'r';
                    break;
                case '\t':
                    destination[written++] = (byte)'t';
                    break;
                case '\\':
                    destination[written++] = (byte)'\\';
                    break;
                case '/':
                    destination[written++] = (byte)'/';
                    break;
                case '\b':
                    destination[written++] = (byte)'b';
                    break;
                case '\f':
                    destination[written++] = (byte)'f';
                    break;
                default:
                    destination[written++] = (byte)'u';
                    if (scalar < 0x10000)
                    {
                        WriteHex(scalar, ref destination, ref written);
                    }
                    else
                    {
                        int quotient = DivMod(scalar - 0x10000, 0x400, out int remainder);
                        int firstChar = quotient + 0xD800;
                        int nextChar = remainder + 0xDC00;
                        WriteHex(firstChar, ref destination, ref written);
                        destination[written++] = (byte)'\\';
                        destination[written++] = (byte)'u';
                        WriteHex(nextChar, ref destination, ref written);
                    }
                    break;
            }
            return numBytesConsumed;
        }

        /// <summary>
        /// We don't have access to Math.DivRem, so this is a copy of the implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DivMod(int numerator, int denominator, out int modulo)
        {
            int div = numerator / denominator;
            modulo = numerator - (div * denominator);
            return div;
        }

        public static void EscapeString(ref ReadOnlySpan<char> value, ref Span<char> destination, int indexOfFirstByteToEscape, out int written)
        {
            Debug.Assert(indexOfFirstByteToEscape >= 0 && indexOfFirstByteToEscape < value.Length);

            value.Slice(0, indexOfFirstByteToEscape).CopyTo(destination);
            written = indexOfFirstByteToEscape;
            int consumed = indexOfFirstByteToEscape;

            while (consumed < value.Length)
            {
                char val = value[consumed];
                if (NeedsEscaping(val))
                {
                    EscapeNextChars(ref value, val, ref destination, ref consumed, ref written);
                }
                else
                {
                    destination[written++] = val;
                }
                consumed++;
            }
        }

        private static void EscapeNextChars(ref ReadOnlySpan<char> value, int firstChar, ref Span<char> destination, ref int consumed, ref int written)
        {
            int nextChar = -1;
            if (InRange(firstChar, 0xD800, 0xDFFF))
            {
                consumed++;
                if (value.Length <= consumed || firstChar >= 0xDC00)
                {
                    JsonThrowHelper.ThrowJsonWriterException("Invalid UTF-16 string ending in an invalid surrogate pair.");
                }

                nextChar = value[consumed];
                if (!InRange(nextChar, 0xDC00, 0xDFFF))
                {
                    JsonThrowHelper.ThrowJsonWriterException("Invalid UTF-16 string ending in an invalid surrogate pair.");
                }
            }

            destination[written++] = '\\';
            switch (firstChar)
            {
                case '\n':
                    destination[written++] = 'n';
                    break;
                case '\r':
                    destination[written++] = 'r';
                    break;
                case '\t':
                    destination[written++] = 't';
                    break;
                case '\\':
                    destination[written++] = '\\';
                    break;
                case '/':
                    destination[written++] = '/';
                    break;
                case '\b':
                    destination[written++] = 'b';
                    break;
                case '\f':
                    destination[written++] = 'f';
                    break;
                default:
                    destination[written++] = 'u';
                    WriteHex(firstChar, ref destination, ref written);
                    if (nextChar != -1)
                    {
                        destination[written++] = '\\';
                        destination[written++] = 'u';
                        WriteHex(nextChar, ref destination, ref written);
                    }
                    break;
            }
        }

        // Only allow ASCII characters between ' ' (0x20) and '~' (0x7E), inclusively,
        // but exclude characters that need to be escaped as hex: '"', '\'', '&', '+', '<', '>', '`'
        // and exclude characters that need to be escaped by adding a backslash: '\n', '\r', '\t', '\\', '/', '\b', '\f'
        //
        // non-zero = allowed, 0 = disallowed
        private static ReadOnlySpan<byte> AllowList => new byte[256] {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool NeedsEscaping(byte value) => AllowList[value] == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool NeedsEscaping(char value) => value > 255 || AllowList[value] == 0;

        private static bool InRange(int ch, int start, int end)
        {
            return (uint)(ch - start) <= (uint)(end - start);
        }

        private static void WriteHex(int value, ref Span<byte> destination, ref int written)
        {
            destination[written++] = Int32LsbToHexDigit(value >> 12);
            destination[written++] = Int32LsbToHexDigit((int)((value >> 8) & 0xFU));
            destination[written++] = Int32LsbToHexDigit((int)((value >> 4) & 0xFU));
            destination[written++] = Int32LsbToHexDigit((int)(value & 0xFU));
        }

        private static void WriteHex(int value, ref Span<char> destination, ref int written)
        {
            destination[written++] = (char)Int32LsbToHexDigit(value >> 12);
            destination[written++] = (char)Int32LsbToHexDigit((int)((value >> 8) & 0xFU));
            destination[written++] = (char)Int32LsbToHexDigit((int)((value >> 4) & 0xFU));
            destination[written++] = (char)Int32LsbToHexDigit((int)(value & 0xFU));
        }

        /// <summary>
        /// Converts a number 0 - 15 to its associated hex character '0' - 'f' as byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte Int32LsbToHexDigit(int value)
        {
            Debug.Assert(value < 16);
            return (byte)((value < 10) ? ('0' + value) : ('a' + (value - 10)));
        }

    }
}
