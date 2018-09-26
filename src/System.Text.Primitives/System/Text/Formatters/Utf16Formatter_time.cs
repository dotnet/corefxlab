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

        private const int DefaultFractionDigits = 7;

        private const char TimeMarker = 'T';
        private const char UtcMarker = 'Z';

        private const char GMT1 = 'G';
        private const char GMT2 = 'M';
        private const char GMT3 = 'T';

        private const char GMT1Lowercase = 'g';
        private const char GMT2Lowercase = 'm';
        private const char GMT3Lowercase = 't';

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

        private static readonly char[][] DayAbbreviationsLowercase = new char[][]
        {
            new char[] { 's', 'u', 'n' },
            new char[] { 'm', 'o', 'n' },
            new char[] { 't', 'u', 'e' },
            new char[] { 'w', 'e', 'd' },
            new char[] { 't', 'h', 'u' },
            new char[] { 'f', 'r', 'i' },
            new char[] { 's', 'a', 't' },
        };

        private static readonly char[][] MonthAbbreviationsLowercase = new char[][]
        {
            new char[] { 'j', 'a', 'n' },
            new char[] { 'f', 'e', 'b' },
            new char[] { 'm', 'a', 'r' },
            new char[] { 'a', 'p', 'r' },
            new char[] { 'm', 'a', 'y' },
            new char[] { 'j', 'u', 'n' },
            new char[] { 'j', 'u', 'l' },
            new char[] { 'a', 'u', 'g' },
            new char[] { 's', 'e', 'p' },
            new char[] { 'o', 'c', 't' },
            new char[] { 'n', 'o', 'v' },
            new char[] { 'd', 'e', 'c' },
        };

        private static readonly TimeSpan NullOffset = TimeSpan.MinValue;

        #endregion Constants

        private static bool TryFormatG(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
        {
            const int MinimumCharsNeeded = 19;

            int charsNeeded = MinimumCharsNeeded;
            if (offset != NullOffset)
            {
                charsNeeded += 7; // Space['+'|'-']hh:ss
            }

            bytesWritten = charsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);

            FormattingHelpers.WriteDigits(value.Month, 2, ref utf16Bytes, 0);
            Unsafe.Add(ref utf16Bytes, 2) = Slash;

            FormattingHelpers.WriteDigits(value.Day, 2, ref utf16Bytes, 3);
            Unsafe.Add(ref utf16Bytes, 5) = Slash;

            FormattingHelpers.WriteDigits(value.Year, 4, ref utf16Bytes, 6);
            Unsafe.Add(ref utf16Bytes, 10) = Space;

            FormattingHelpers.WriteDigits(value.Hour, 2, ref utf16Bytes, 11);
            Unsafe.Add(ref utf16Bytes, 13) = Colon;

            FormattingHelpers.WriteDigits(value.Minute, 2, ref utf16Bytes, 14);
            Unsafe.Add(ref utf16Bytes, 16) = Colon;

            FormattingHelpers.WriteDigits(value.Second, 2, ref utf16Bytes, 17);

            if (offset != NullOffset)
            {
                Unsafe.Add(ref utf16Bytes, 19) = Space;

                long ticks = value.Ticks;
                if (ticks < 0)
                {
                    Unsafe.Add(ref utf16Bytes, 20) = Minus;
                    ticks = -ticks;
                }
                else
                {
                    Unsafe.Add(ref utf16Bytes, 20) = Plus;
                }

                FormattingHelpers.WriteDigits(offset.Hours, 2, ref utf16Bytes, 21);
                Unsafe.Add(ref utf16Bytes, 23) = Colon;
                FormattingHelpers.WriteDigits(offset.Minutes, 2, ref utf16Bytes, 24);
            }

            return true;
        }

        private static bool TryFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
        {
            const int MinimumCharsNeeded = 27;

            int charsNeeded = MinimumCharsNeeded;
            DateTimeKind kind = DateTimeKind.Local;

            if (offset == NullOffset)
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

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);

            FormattingHelpers.WriteDigits(value.Year, 4, ref utf16Bytes, 0);
            Unsafe.Add(ref utf16Bytes, 4) = Minus;

            FormattingHelpers.WriteDigits(value.Month, 2, ref utf16Bytes, 5);
            Unsafe.Add(ref utf16Bytes, 7) = Minus;

            FormattingHelpers.WriteDigits(value.Day, 2, ref utf16Bytes, 8);
            Unsafe.Add(ref utf16Bytes, 10) = TimeMarker;

            FormattingHelpers.WriteDigits(value.Hour, 2, ref utf16Bytes, 11);
            Unsafe.Add(ref utf16Bytes, 13) = Colon;

            FormattingHelpers.WriteDigits(value.Minute, 2, ref utf16Bytes, 14);
            Unsafe.Add(ref utf16Bytes, 16) = Colon;

            FormattingHelpers.WriteDigits(value.Second, 2, ref utf16Bytes, 17);
            Unsafe.Add(ref utf16Bytes, 19) = Period;

            FormattingHelpers.DivMod(value.Ticks, TimeSpan.TicksPerSecond, out long fraction);
            FormattingHelpers.WriteFractionDigits(fraction, DefaultFractionDigits, ref utf16Bytes, 20);

            if (kind == DateTimeKind.Local)
            {
                char sign = Plus;

                if (offset < default(TimeSpan) /* a "const" version of TimeSpan.Zero */)
                {
                    offset = TimeSpan.FromTicks(-offset.Ticks);
                    sign = Minus;
                }

                Unsafe.Add(ref utf16Bytes, 27) = sign;
                FormattingHelpers.WriteDigits(offset.Hours, 2, ref utf16Bytes, 28);
                Unsafe.Add(ref utf16Bytes, 30) = Colon;
                FormattingHelpers.WriteDigits(offset.Minutes, 2, ref utf16Bytes, 31);
            }
            else if (kind == DateTimeKind.Utc)
            {
                Unsafe.Add(ref utf16Bytes, 27) = UtcMarker;
            }

            return true;
        }

        private static bool TryFormatRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int CharsNeeded = 29;

            bytesWritten = CharsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);

            var dayAbbrev = DayAbbreviations[(int)value.DayOfWeek];
            Unsafe.Add(ref utf16Bytes, 0) = dayAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 1) = dayAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 2) = dayAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 3) = Comma;
            Unsafe.Add(ref utf16Bytes, 4) = Space;

            FormattingHelpers.WriteDigits(value.Day, 2, ref utf16Bytes, 5);
            Unsafe.Add(ref utf16Bytes, 7) = ' ';

            var monthAbbrev = MonthAbbreviations[value.Month - 1];
            Unsafe.Add(ref utf16Bytes, 8) = monthAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 9) = monthAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 10) = monthAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 11) = Space;

            FormattingHelpers.WriteDigits(value.Year, 4, ref utf16Bytes, 12);
            Unsafe.Add(ref utf16Bytes, 16) = Space;

            FormattingHelpers.WriteDigits(value.Hour, 2, ref utf16Bytes, 17);
            Unsafe.Add(ref utf16Bytes, 19) = Colon;

            FormattingHelpers.WriteDigits(value.Minute, 2, ref utf16Bytes, 20);
            Unsafe.Add(ref utf16Bytes, 22) = Colon;

            FormattingHelpers.WriteDigits(value.Second, 2, ref utf16Bytes, 23);
            Unsafe.Add(ref utf16Bytes, 25) = Space;

            Unsafe.Add(ref utf16Bytes, 26) = GMT1;
            Unsafe.Add(ref utf16Bytes, 27) = GMT2;
            Unsafe.Add(ref utf16Bytes, 28) = GMT3;

            return true;
        }

        private static bool TryFormatRfc1123Lowercase(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int CharsNeeded = 29;

            bytesWritten = CharsNeeded * sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);

            var dayAbbrev = DayAbbreviationsLowercase[(int)value.DayOfWeek];
            Unsafe.Add(ref utf16Bytes, 0) = dayAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 1) = dayAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 2) = dayAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 3) = Comma;
            Unsafe.Add(ref utf16Bytes, 4) = Space;

            FormattingHelpers.WriteDigits(value.Day, 2, ref utf16Bytes, 5);
            Unsafe.Add(ref utf16Bytes, 7) = ' ';

            var monthAbbrev = MonthAbbreviationsLowercase[value.Month - 1];
            Unsafe.Add(ref utf16Bytes, 8) = monthAbbrev[0];
            Unsafe.Add(ref utf16Bytes, 9) = monthAbbrev[1];
            Unsafe.Add(ref utf16Bytes, 10) = monthAbbrev[2];
            Unsafe.Add(ref utf16Bytes, 11) = Space;

            FormattingHelpers.WriteDigits(value.Year, 4, ref utf16Bytes, 12);
            Unsafe.Add(ref utf16Bytes, 16) = Space;

            FormattingHelpers.WriteDigits(value.Hour, 2, ref utf16Bytes, 17);
            Unsafe.Add(ref utf16Bytes, 19) = Colon;

            FormattingHelpers.WriteDigits(value.Minute, 2, ref utf16Bytes, 20);
            Unsafe.Add(ref utf16Bytes, 22) = Colon;

            FormattingHelpers.WriteDigits(value.Second, 2, ref utf16Bytes, 23);
            Unsafe.Add(ref utf16Bytes, 25) = Space;

            Unsafe.Add(ref utf16Bytes, 26) = GMT1Lowercase;
            Unsafe.Add(ref utf16Bytes, 27) = GMT2Lowercase;
            Unsafe.Add(ref utf16Bytes, 28) = GMT3Lowercase;

            return true;
        }

        private static bool TryFormatTimeSpan(TimeSpan value, char format, Span<byte> buffer, out int bytesWritten)
        {
            bool longForm = (format == 'G');
            bool constant = (format == 't' || format == 'T' || format == 'c');

            long ticks = value.Ticks;
            int days = (int)FormattingHelpers.DivMod(ticks, TimeSpan.TicksPerDay, out long timeLeft);

            bool showSign = false;
            if (ticks < 0)
            {
                showSign = true;
                days = -days;
                timeLeft = -timeLeft;
            }

            int hours = (int)FormattingHelpers.DivMod(timeLeft, TimeSpan.TicksPerHour, out timeLeft);
            int minutes = (int)FormattingHelpers.DivMod(timeLeft, TimeSpan.TicksPerMinute, out timeLeft);
            int seconds = (int)FormattingHelpers.DivMod(timeLeft, TimeSpan.TicksPerSecond, out long fraction);

            int dayDigits = 0;
            int hourDigits = (constant || longForm || hours > 9) ? 2 : 1;
            int fractionDigits = 0;

            bytesWritten = hourDigits + 6; // [h]h:mm:ss
            if (showSign)
                bytesWritten += 1;  // [-]
            if (longForm || days > 0)
            {
                dayDigits = FormattingHelpers.CountDigits(days);
                bytesWritten += dayDigits + 1; // [d'.']
            }
            if (longForm || fraction > 0)
            {
                fractionDigits = (longForm || constant) ? DefaultFractionDigits : FormattingHelpers.CountFractionDigits(fraction);
                bytesWritten += fractionDigits + 1; // ['.'fffffff] or ['.'FFFFFFF] for short-form
            }

            bytesWritten *= sizeof(char);
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            Span<char> dst = MemoryMarshal.Cast<byte, char>(buffer);
            ref char utf16Bytes = ref MemoryMarshal.GetReference(dst);
            int idx = 0;

            if (showSign)
                Unsafe.Add(ref utf16Bytes, idx++) = Minus;

            if (dayDigits > 0)
            {
                idx += FormattingHelpers.WriteDigits(days, dayDigits, ref utf16Bytes, idx);
                Unsafe.Add(ref utf16Bytes, idx++) = constant ? Period : Colon;
            }

            idx += FormattingHelpers.WriteDigits(hours, hourDigits, ref utf16Bytes, idx);
            Unsafe.Add(ref utf16Bytes, idx++) = Colon;

            idx += FormattingHelpers.WriteDigits(minutes, 2, ref utf16Bytes, idx);
            Unsafe.Add(ref utf16Bytes, idx++) = Colon;

            idx += FormattingHelpers.WriteDigits(seconds, 2, ref utf16Bytes, idx);

            if (fractionDigits > 0)
            {
                Unsafe.Add(ref utf16Bytes, idx++) = Period;
                idx += FormattingHelpers.WriteFractionDigits(fraction, fractionDigits, ref utf16Bytes, idx);
            }

            return true;
        }
    }
}
