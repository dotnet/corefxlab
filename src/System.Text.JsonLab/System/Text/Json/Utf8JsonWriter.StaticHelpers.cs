// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static partial class Utf8JsonWriterHelpers
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
                    WriteDigits((uint)scalar, destination.Slice(written, 4));
                    written += 4;
                    break;
            }
            return numBytesConsumed;
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
                    consumed += EscapeNextChars(ref value, val, ref destination, ref written);
                }
                else
                {
                    destination[written] = val;
                    written++;
                    consumed++;
                }
            }
        }

        private static int EscapeNextChars(ref ReadOnlySpan<char> value, int firstChar, ref Span<char> destination, ref int written)
        {
            int consumed = 1;
            if (InRange(firstChar, 0xD800, 0xDFFF))
            {
                if (value.Length <= consumed || firstChar >= 0xDC00)
                {
                    JsonThrowHelper.ThrowJsonWriterException("Invalid UTF-16 string ending in an invalid surrogate pair.");
                }

                int nextChar = value[consumed];
                if (!InRange(nextChar, 0xDC00, 0xDFFF))
                {
                    JsonThrowHelper.ThrowJsonWriterException("Invalid UTF-16 string ending in an invalid surrogate pair.");
                }

                int highSurrogate = (firstChar - 0xD800) * 0x400;
                int lowSurrogate = nextChar - 0xDC00;
                firstChar = highSurrogate + lowSurrogate;
                consumed++;
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
                    WriteDigits((uint)firstChar, destination.Slice(written, 4));
                    written += 4;
                    break;
            }
            return consumed;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigits(uint value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                uint temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigits(uint value, Span<char> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                uint temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (char)('0' + value);
        }
    }
}
