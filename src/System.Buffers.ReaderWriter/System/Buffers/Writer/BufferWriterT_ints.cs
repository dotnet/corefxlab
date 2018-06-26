// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Writer
{
    internal static partial class Utf8Constants
    {
        public const byte Colon = (byte)':';
        public const byte Comma = (byte)',';
        public const byte Minus = (byte)'-';
        public const byte Period = (byte)'.';
        public const byte Plus = (byte)'+';
        public const byte Slash = (byte)'/';
        public const byte Space = (byte)' ';
        public const byte Hyphen = (byte)'-';

        public const byte Separator = (byte)',';

        // Invariant formatting uses groups of 3 for each number group separated by commas.
        //   ex. 1,234,567,890
        public const int GroupSize = 3;

        public static readonly TimeSpan NullUtcOffset = TimeSpan.MinValue;  // Utc offsets must range from -14:00 to 14:00 so this is never a valid offset.

        public const int DateTimeMaxUtcOffsetHours = 14; // The UTC offset portion of a TimeSpan or DateTime can be no more than 14 hours and no less than -14 hours.

        public const int DateTimeNumFractionDigits = 7;  // TimeSpan and DateTime formats allow exactly up to many digits for specifying the fraction after the seconds.
        public const int MaxDateTimeFraction = 9999999;  // ... and hence, the largest fraction expressible is this.

        public const ulong BillionMaxUIntValue = (ulong)uint.MaxValue * Billion; // maximum value that can be split into two uint32 {1-10 digits}{9 digits}
        public const uint Billion = 1000000000; // 10^9, used to split int64/uint64 into three uint32 {1-2 digits}{9 digits}{9 digits}
    }

    public static class TempHelper
    {
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
        public static bool TryFormatInt64Default(long value, Span<byte> destination, out int bytesWritten)
        {
            if ((ulong)value < 10)
            {
                return TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
            }

            if (IntPtr.Size == 8)    // x64
            {
                return TryFormatInt64MultipleDigits(value, destination, out bytesWritten);
            }
            else    // x86
            {
                if (value <= int.MaxValue && value >= int.MinValue)
                {
                    return TryFormatInt32MultipleDigits((int)value, destination, out bytesWritten);
                }
                else
                {
                    if (value <= (long)Utf8Constants.BillionMaxUIntValue && value >= -(long)Utf8Constants.BillionMaxUIntValue)
                    {
                        return value < 0 ?
                        TryFormatInt64MoreThanNegativeBillionMaxUInt(-value, destination, out bytesWritten) :
                        TryFormatUInt64LessThanBillionMaxUInt((ulong)value, destination, out bytesWritten);
                    }
                    else
                    {
                        return value < 0 ?
                        TryFormatInt64LessThanNegativeBillionMaxUInt(-value, destination, out bytesWritten) :
                        TryFormatUInt64MoreThanBillionMaxUInt((ulong)value, destination, out bytesWritten);
                    }
                }
            }
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
            buffer[0] = (byte)('0' + value);
        }

        // Split ulong into two parts that can each fit in a uint - {1-10 digits}{9 digits}
        private static bool TryFormatUInt64LessThanBillionMaxUInt(ulong value, Span<byte> destination, out int bytesWritten)
        {
            uint overNineDigits = (uint)(value / Utf8Constants.Billion);
            uint lastNineDigits = (uint)(value - (overNineDigits * Utf8Constants.Billion));

            int digitCountOverNineDigits = CountDigits(overNineDigits);
            int digitCount = digitCountOverNineDigits + 9;
            // WriteDigits does not do bounds checks
            if (digitCount > destination.Length)
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten = digitCount;
            WriteDigits(overNineDigits, destination.Slice(0, digitCountOverNineDigits));
            WriteDigits(lastNineDigits, destination.Slice(digitCountOverNineDigits, 9));
            return true;
        }

        // Split ulong into three parts that can each fit in a uint - {1-2 digits}{9 digits}{9 digits}
        private static bool TryFormatUInt64MoreThanBillionMaxUInt(ulong value, Span<byte> destination, out int bytesWritten)
        {
            ulong overNineDigits = value / Utf8Constants.Billion;
            uint lastNineDigits = (uint)(value - (overNineDigits * Utf8Constants.Billion));
            uint overEighteenDigits = (uint)(overNineDigits / Utf8Constants.Billion);
            uint middleNineDigits = (uint)(overNineDigits - (overEighteenDigits * Utf8Constants.Billion));

            int digitCountOverEighteenDigits = CountDigits(overEighteenDigits);
            int digitCount = digitCountOverEighteenDigits + 18;
            // WriteDigits does not do bounds checks
            if (digitCount > destination.Length)
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten = digitCount;
            WriteDigits(overEighteenDigits, destination.Slice(0, digitCountOverEighteenDigits));
            WriteDigits(middleNineDigits, destination.Slice(digitCountOverEighteenDigits, 9));
            WriteDigits(lastNineDigits, destination.Slice(digitCountOverEighteenDigits + 9, 9));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDigitsUInt64D(ulong value, Span<byte> buffer)
        {
            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }
            buffer[0] = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDigitsUInt64D(ulong value, Span<char> buffer)
        {

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }
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
                digits += 6;
            }

            return digits;
        }

        // TODO: Use this instead of TryFormatInt64Default to format numbers less than int.MaxValue
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatInt32Default(int value, Span<byte> destination, out int bytesWritten)
        {
            if ((uint)value < 10)
            {
                return TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
            }
            return TryFormatInt32MultipleDigits(value, destination, out bytesWritten);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatInt32MultipleDigits(int value, Span<byte> destination, out int bytesWritten)
        {
            if (value < 0)
            {
                value = -value;
                int digitCount = CountDigits((uint)value);
                // WriteDigits does not do bounds checks
                if (digitCount >= destination.Length)
                {
                    bytesWritten = 0;
                    return false;
                }
                destination[0] = Utf8Constants.Minus;
                bytesWritten = digitCount + 1;
                WriteDigits((uint)value, destination.Slice(1, digitCount));
                return true;
            }
            else
            {
                return TryFormatUInt32MultipleDigits((uint)value, destination, out bytesWritten);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatUInt32MultipleDigits(uint value, Span<byte> destination, out int bytesWritten)
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
                destination[0] = Utf8Constants.Minus;
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

        // Split long into two parts that can each fit in a uint - {1-10 digits}{9 digits}
        private static bool TryFormatInt64MoreThanNegativeBillionMaxUInt(long value, Span<byte> destination, out int bytesWritten)
        {
            uint overNineDigits = (uint)(value / Utf8Constants.Billion);
            uint lastNineDigits = (uint)(value - (overNineDigits * Utf8Constants.Billion));

            int digitCountOverNineDigits = CountDigits(overNineDigits);
            int digitCount = digitCountOverNineDigits + 9;
            // WriteDigits does not do bounds checks
            if (digitCount >= destination.Length)
            {
                bytesWritten = 0;
                return false;
            }
            destination[0] = Utf8Constants.Minus;
            bytesWritten = digitCount + 1;
            WriteDigits(overNineDigits, destination.Slice(1, digitCountOverNineDigits));
            WriteDigits(lastNineDigits, destination.Slice(digitCountOverNineDigits + 1, 9));
            return true;
        }

        // Split long into three parts that can each fit in a uint - {1 digit}{9 digits}{9 digits}
        private static bool TryFormatInt64LessThanNegativeBillionMaxUInt(long value, Span<byte> destination, out int bytesWritten)
        {
            // value can still be negative if value == long.MinValue
            // Therefore, cast to ulong, since (ulong)value actually equals abs(long.MinValue)
            ulong overNineDigits = (ulong)value / Utf8Constants.Billion;
            uint lastNineDigits = (uint)((ulong)value - (overNineDigits * Utf8Constants.Billion));
            uint overEighteenDigits = (uint)(overNineDigits / Utf8Constants.Billion);
            uint middleNineDigits = (uint)(overNineDigits - (overEighteenDigits * Utf8Constants.Billion));

            int digitCountOverEighteenDigits = CountDigits(overEighteenDigits);
            int digitCount = digitCountOverEighteenDigits + 18;
            // WriteDigits does not do bounds checks
            if (digitCount >= destination.Length)
            {
                bytesWritten = 0;
                return false;
            }
            destination[0] = Utf8Constants.Minus;
            bytesWritten = digitCount + 1;
            WriteDigits(overEighteenDigits, destination.Slice(1, digitCountOverEighteenDigits));
            WriteDigits(middleNineDigits, destination.Slice(digitCountOverEighteenDigits + 1, 9));
            WriteDigits(lastNineDigits, destination.Slice(digitCountOverEighteenDigits + 1 + 9, 9));
            return true;
        }
    }

    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        #region Int32
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, default))
            {
                Enlarge();
            }
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long value)
        {
            int written;
            while (!TempHelper.TryFormatInt64Default(value, Buffer, out written))
            {
                Enlarge();
            }
            /*while (!Utf8Formatter.TryFormat(value, Buffer, out written, default))
            {
                Enlarge();
            }*/
            Advance(written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }

        public void Write(int value, TransformationFormat format)
        {
            int written;
            while (true)
            {
                Span<byte> buffer = Buffer;
                while (!Utf8Formatter.TryFormat(value, Buffer, out written, format.Format))
                {
                    Enlarge();
                }
                if (format.TryTransform(buffer, ref written))
                {
                    Advance(written);
                    return;
                }
                Enlarge();
            }
        }
        #endregion

        #region UInt64
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value, StandardFormat format = default)
        {
            int written;
            while (!Utf8Formatter.TryFormat(value, Buffer, out written, format))
            {
                Enlarge();
            }
            Advance(written);
        }
        #endregion
    }
}
