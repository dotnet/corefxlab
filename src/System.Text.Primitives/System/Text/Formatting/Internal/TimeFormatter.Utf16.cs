// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class InvariantUtf16TimeFormatter
    {
        #region Constants

        private const char Colon = ':';
        private const char Comma = ',';
        private const char Minus = '-';
        private const char Period = '.';
        private const char Plus = '+';
        private const char Slash = '/';
        private const char Space = ' ';

        private const char TimeMarker = 'T';
        private const char UtcMarker = 'Z';

        private const char GMT1 = 'G';
        private const char GMT2 = 'M';
        private const char GMT3 = 'T';

        private static readonly char[][] DayAbbreviations = new char[][]
        {
                new char[] { 'S', 'u', 'n' },
                new char[] { 'M', 'o', 'n' },
                new char[] { 'T', 'u', 'e' },
                new char[] { 'W', 'e', 'd' },
                new char[] { 'T', 'h', 'u' },
                new char[] { 'F', 'r', 'i' },
                new char[] { 'S', 'a', 't' },
        };

        private static readonly char[][] MonthAbbreviations = new char[][]
        {
                new char[] { 'J', 'a', 'n' },
                new char[] { 'F', 'e', 'b' },
                new char[] { 'M', 'a', 'r' },
                new char[] { 'A', 'p', 'r' },
                new char[] { 'M', 'a', 'y' },
                new char[] { 'J', 'u', 'n' },
                new char[] { 'J', 'u', 'l' },
                new char[] { 'A', 'u', 'g' },
                new char[] { 'S', 'e', 'p' },
                new char[] { 'O', 'c', 't' },
                new char[] { 'N', 'o', 'v' },
                new char[] { 'D', 'e', 'c' },
        };

        #endregion Constants

        public static bool TryFormatG(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int CharsNeeded = 19;

            bytesWritten = CharsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = buffer.NonPortableCast<byte, char>();
            ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

            FormattingHelpers.WriteTwoDigits(value.Month, ref utf16Bytes, 0);
            Unsafe.Add(ref utf16Bytes, 2) = Slash;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf16Bytes, 3);
            Unsafe.Add(ref utf16Bytes, 5) = Slash;

            FormattingHelpers.WriteFourDigits(value.Year, ref utf16Bytes, 6);
            Unsafe.Add(ref utf16Bytes, 10) = Space;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf16Bytes, 11);
            Unsafe.Add(ref utf16Bytes, 13) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf16Bytes, 14);
            Unsafe.Add(ref utf16Bytes, 16) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf16Bytes, 17);

            return true;
        }

        public static bool TryFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
        {
            const int MinimumCharsNeeded = 27;

            int charsNeeded = MinimumCharsNeeded;
            DateTimeKind kind = DateTimeKind.Local;

            if (offset == PrimitiveFormatter.NullOffset)
            {
                kind = value.Kind;
                if (kind == DateTimeKind.Local)
                {
                    offset = TimeZoneInfo.Local.GetUtcOffset(value);
                    charsNeeded += 6;
                }
                else if (kind == DateTimeKind.Utc)
                {
                    charsNeeded += 1;
                }
            }
            else
            {
                charsNeeded += 6;
            }

            bytesWritten = charsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = buffer.NonPortableCast<byte, char>();
            ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

            FormattingHelpers.WriteFourDigits(value.Year, ref utf16Bytes, 0);
            Unsafe.Add(ref utf16Bytes, 4) = Minus;

            FormattingHelpers.WriteTwoDigits(value.Month, ref utf16Bytes, 5);
            Unsafe.Add(ref utf16Bytes, 7) = Minus;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf16Bytes, 8);
            Unsafe.Add(ref utf16Bytes, 10) = TimeMarker;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf16Bytes, 11);
            Unsafe.Add(ref utf16Bytes, 13) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf16Bytes, 14);
            Unsafe.Add(ref utf16Bytes, 16) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf16Bytes, 17);
            Unsafe.Add(ref utf16Bytes, 19) = Period;

            ulong fraction = (ulong)(value.Ticks % TimeSpan.TicksPerSecond);
            FormattingHelpers.WriteFractionDigits(fraction, ref utf16Bytes, 20);

            if (kind == DateTimeKind.Local)
            {
                int hours = offset.Hours;
                char sign = Plus;

                if (offset.Hours < 0)
                {
                    hours = -offset.Hours;
                    sign = Minus;
                }

                Unsafe.Add(ref utf16Bytes, 27) = sign;
                FormattingHelpers.WriteTwoDigits(hours, ref utf16Bytes, 28);
                Unsafe.Add(ref utf16Bytes, 30) = Colon;
                FormattingHelpers.WriteTwoDigits(offset.Minutes, ref utf16Bytes, 31);
            }
            else if (kind == DateTimeKind.Utc)
            {
                Unsafe.Add(ref utf16Bytes, 27) = UtcMarker;
            }

            return true;
        }

        public static bool TryFormatRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int CharsNeeded = 29;

            bytesWritten = CharsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = buffer.NonPortableCast<byte, char>();
            ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

            var dayAbbrev = DayAbbreviations[(int)value.DayOfWeek];
            Unsafe.Add(ref utf16Bytes, 0) = dayAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 1) = dayAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 2) = dayAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 3) = Comma;
            Unsafe.Add(ref utf16Bytes, 4) = Space;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf16Bytes, 5);
            Unsafe.Add(ref utf16Bytes, 7) = ' ';

            var monthAbbrev = MonthAbbreviations[value.Month - 1];
            Unsafe.Add(ref utf16Bytes, 8) = monthAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 9) = monthAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 10) = monthAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 11) = Space;

            FormattingHelpers.WriteFourDigits(value.Year, ref utf16Bytes, 12);
            Unsafe.Add(ref utf16Bytes, 16) = Space;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf16Bytes, 17);
            Unsafe.Add(ref utf16Bytes, 19) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf16Bytes, 20);
            Unsafe.Add(ref utf16Bytes, 22) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf16Bytes, 23);
            Unsafe.Add(ref utf16Bytes, 25) = Space;

            Unsafe.Add(ref utf16Bytes, 26) = GMT1;
            Unsafe.Add(ref utf16Bytes, 27) = GMT2;
            Unsafe.Add(ref utf16Bytes, 28) = GMT3;

            return true;
        }
    }
}
