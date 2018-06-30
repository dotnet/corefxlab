// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static class JsonWriterHelper
    {
        public static readonly byte[] NewLineUtf8 = Encoding.UTF8.GetBytes(Environment.NewLine);

        // TODO: Add any other characters that need escaping, control characters 0x0 to 0x1F
        // https://tools.ietf.org/html/rfc8259
        public static readonly byte[] ValuesToEscape = { (byte)'"', (byte)'\\', (byte)'\b', (byte)'\f', (byte)'\n', (byte)'\r', (byte)'\t' };
        public static readonly byte[] EscapedValues = { (byte)'"', (byte)'\\', (byte)'b', (byte)'f', (byte)'n', (byte)'r', (byte)'t' };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDigitsUInt64D(ulong value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDigitsUInt64D(ulong value, Span<char> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (char)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountDigits(ulong value)
        {
            int digits = 1;
            uint part;
            if (value >= 10000000)
            {
                if (value >= 100000000000000)
                {
                    part = (uint)(value / 100000000000000);
                    digits += 14;
                }
                else
                {
                    part = (uint)(value / 10000000);
                    digits += 7;
                }
            }
            else
            {
                part = (uint)value;
            }

            if (part < 10)
            {
                // no-op
            }
            else if (part < 100)
            {
                digits += 1;
            }
            else if (part < 1000)
            {
                digits += 2;
            }
            else if (part < 10000)
            {
                digits += 3;
            }
            else if (part < 100000)
            {
                digits += 4;
            }
            else if (part < 1000000)
            {
                digits += 5;
            }
            else
            {
                Debug.Assert(part < 10000000);
                digits += 6;
            }

            return digits;
        }

        // Borrowed from https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/Buffers/Text/Utf8Formatter/Utf8Formatter.Integer.Signed.Default.cs#L16
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFormatInt64Default(long value, Span<byte> destination, out int bytesWritten)
        {
            if ((ulong)value < 10)
            {
                return TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
            }

            return TryFormatInt64MultipleDigits(value, destination, out bytesWritten);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatUInt32SingleDigit(uint value, Span<byte> destination, out int bytesWritten)
        {
            if (destination.Length == 0)
            {
                bytesWritten = 0;
                return false;
            }
            destination[0] = (byte)('0' + value);
            bytesWritten = 1;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatInt64MultipleDigits(long value, Span<byte> destination, out int bytesWritten)
        {
            if (value < 0)
            {
                value = -value;
                int digitCount = CountDigits((ulong)value);
                // WriteDigits does not do bounds checks
                if (digitCount >= destination.Length)
                {
                    bytesWritten = 0;
                    return false;
                }
                destination[0] = (byte)'-';
                bytesWritten = digitCount + 1;
                WriteDigits((ulong)value, destination.Slice(1, digitCount));
                return true;
            }
            else
            {
                return TryFormatUInt64MultipleDigits((ulong)value, destination, out bytesWritten);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatUInt64MultipleDigits(ulong value, Span<byte> destination, out int bytesWritten)
        {
            int digitCount = CountDigits(value);
            // WriteDigits does not do bounds checks
            if (digitCount > destination.Length)
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten = digitCount;
            WriteDigits(value, destination.Slice(0, digitCount));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDigits(ulong value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }
    }
}
