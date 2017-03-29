// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class InvariantUtf8TimeFormatter
    {
        #region Constants

        private const byte Colon = (byte)':';
        private const byte Comma = (byte)',';
        private const byte Minus = (byte)'-';
        private const byte Period = (byte)'.';
        private const byte Plus = (byte)'+';
        private const byte Slash = (byte)'/';
        private const byte Space = (byte)' ';

        private const byte TimeMarker = (byte)'T';
        private const byte UtcMarker = (byte)'Z';

        private const byte GMT1 = (byte)'G';
        private const byte GMT2 = (byte)'M';
        private const byte GMT3 = (byte)'T';

        private static readonly byte[][] DayAbbreviations = new byte[][]
        {
                new byte[] { (byte)'S', (byte)'u', (byte)'n' },
                new byte[] { (byte)'M', (byte)'o', (byte)'n' },
                new byte[] { (byte)'T', (byte)'u', (byte)'e' },
                new byte[] { (byte)'W', (byte)'e', (byte)'d' },
                new byte[] { (byte)'T', (byte)'h', (byte)'u' },
                new byte[] { (byte)'F', (byte)'r', (byte)'i' },
                new byte[] { (byte)'S', (byte)'a', (byte)'t' },
        };

        private static readonly byte[][] MonthAbbreviations = new byte[][]
        {
                new byte[] { (byte)'J', (byte)'a', (byte)'n' },
                new byte[] { (byte)'F', (byte)'e', (byte)'b' },
                new byte[] { (byte)'M', (byte)'a', (byte)'r' },
                new byte[] { (byte)'A', (byte)'p', (byte)'r' },
                new byte[] { (byte)'M', (byte)'a', (byte)'y' },
                new byte[] { (byte)'J', (byte)'u', (byte)'n' },
                new byte[] { (byte)'J', (byte)'u', (byte)'l' },
                new byte[] { (byte)'A', (byte)'u', (byte)'g' },
                new byte[] { (byte)'S', (byte)'e', (byte)'p' },
                new byte[] { (byte)'O', (byte)'c', (byte)'t' },
                new byte[] { (byte)'N', (byte)'o', (byte)'v' },
                new byte[] { (byte)'D', (byte)'e', (byte)'c' },
        };

        #endregion Constants

        public static bool TryFormatG(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int BytesNeeded = 19;

            if (buffer.Length < BytesNeeded)
            {
                bytesWritten = 0;
                return false;
            }

            ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();

            FormattingHelpers.WriteTwoDigits(value.Month, ref utf8Bytes, 0);
            Unsafe.Add(ref utf8Bytes, 2) = Slash;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf8Bytes, 3);
            Unsafe.Add(ref utf8Bytes, 5) = Slash;

            FormattingHelpers.WriteFourDigits(value.Year, ref utf8Bytes, 6);
            Unsafe.Add(ref utf8Bytes, 10) = Space;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf8Bytes, 11);
            Unsafe.Add(ref utf8Bytes, 13) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf8Bytes, 14);
            Unsafe.Add(ref utf8Bytes, 16) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf8Bytes, 17);

            bytesWritten = BytesNeeded;
            return true;
        }

        public static bool TryFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
        {
            const int MinimumBytesNeeded = 27;

            bytesWritten = MinimumBytesNeeded;
            DateTimeKind kind = DateTimeKind.Local;

            if (offset == PrimitiveFormatter.NullOffset)
            {
                kind = value.Kind;
                if (kind == DateTimeKind.Local)
                {
                    offset = TimeZoneInfo.Local.GetUtcOffset(value);
                    bytesWritten += 6;
                }
                else if (kind == DateTimeKind.Utc)
                {
                    bytesWritten += 1;
                }
            }
            else
            {
                bytesWritten += 6;
            }

            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();

            FormattingHelpers.WriteFourDigits(value.Year, ref utf8Bytes, 0);
            Unsafe.Add(ref utf8Bytes, 4) = Minus;

            FormattingHelpers.WriteTwoDigits(value.Month, ref utf8Bytes, 5);
            Unsafe.Add(ref utf8Bytes, 7) = Minus;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf8Bytes, 8);
            Unsafe.Add(ref utf8Bytes, 10) = TimeMarker;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf8Bytes, 11);
            Unsafe.Add(ref utf8Bytes, 13) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf8Bytes, 14);
            Unsafe.Add(ref utf8Bytes, 16) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf8Bytes, 17);
            Unsafe.Add(ref utf8Bytes, 19) = Period;

            ulong fraction = (ulong)(value.Ticks % TimeSpan.TicksPerSecond);
            FormattingHelpers.WriteFractionDigits(fraction, ref utf8Bytes, 20);

            if (kind == DateTimeKind.Local)
            {
                int hours = offset.Hours;
                byte sign = Plus;

                if (offset.Hours < 0)
                {
                    hours = -offset.Hours;
                    sign = Minus;
                }

                Unsafe.Add(ref utf8Bytes, 27) = sign;
                FormattingHelpers.WriteTwoDigits(hours, ref utf8Bytes, 28);
                Unsafe.Add(ref utf8Bytes, 30) = Colon;
                FormattingHelpers.WriteTwoDigits(offset.Minutes, ref utf8Bytes, 31);
            }
            else if (kind == DateTimeKind.Utc)
            {
                Unsafe.Add(ref utf8Bytes, 27) = UtcMarker;
            }

            return true;
        }

        public static bool TryFormatRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten)
        {
            const int BytesNeeded = 29;

            bytesWritten = BytesNeeded;
            if (buffer.Length < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();

            var dayAbbrev = DayAbbreviations[(int)value.DayOfWeek];
            Unsafe.Add(ref utf8Bytes, 0) = dayAbbrev[0];
            Unsafe.Add(ref utf8Bytes, 1) = dayAbbrev[1];
            Unsafe.Add(ref utf8Bytes, 2) = dayAbbrev[2];
            Unsafe.Add(ref utf8Bytes, 3) = Comma;
            Unsafe.Add(ref utf8Bytes, 4) = Space;

            FormattingHelpers.WriteTwoDigits(value.Day, ref utf8Bytes, 5);
            Unsafe.Add(ref utf8Bytes, 7) = (byte)' ';

            var monthAbbrev = MonthAbbreviations[value.Month - 1];
            Unsafe.Add(ref utf8Bytes, 8) = monthAbbrev[0];
            Unsafe.Add(ref utf8Bytes, 9) = monthAbbrev[1];
            Unsafe.Add(ref utf8Bytes, 10) = monthAbbrev[2];
            Unsafe.Add(ref utf8Bytes, 11) = Space;

            FormattingHelpers.WriteFourDigits(value.Year, ref utf8Bytes, 12);
            Unsafe.Add(ref utf8Bytes, 16) = Space;

            FormattingHelpers.WriteTwoDigits(value.Hour, ref utf8Bytes, 17);
            Unsafe.Add(ref utf8Bytes, 19) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Minute, ref utf8Bytes, 20);
            Unsafe.Add(ref utf8Bytes, 22) = Colon;

            FormattingHelpers.WriteTwoDigits(value.Second, ref utf8Bytes, 23);
            Unsafe.Add(ref utf8Bytes, 25) = Space;

            Unsafe.Add(ref utf8Bytes, 26) = GMT1;
            Unsafe.Add(ref utf8Bytes, 27) = GMT2;
            Unsafe.Add(ref utf8Bytes, 28) = GMT3;

            return true;
        }
    }
}
