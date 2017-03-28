// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        #region Constants

        private static readonly TimeSpan NullOffset = TimeSpan.MinValue;

        #endregion Constants

        #region Time formatting methods

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }

            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, out bytesWritten, encoder);

                case 'O':
                    return TryFormatDateTimeFormatO(value.DateTime, value.Offset, buffer, out bytesWritten, encoder);

                case 'G':
                    return TryFormatDateTimeFormatG(value.DateTime, buffer, out bytesWritten, encoder);

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }

            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');

            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            switch (format.Symbol)
            {
                case 'R':
                    return TryFormatDateTimeRfc1123(value, buffer, out bytesWritten, encoder);

                case 'O':
                    return TryFormatDateTimeFormatO(value, NullOffset, buffer, out bytesWritten, encoder);

                case 'G':
                    return TryFormatDateTimeFormatG(value, buffer, out bytesWritten, encoder);

                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatG(DateTime value, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return Utf8TimeFormatter.TryFormatG(value, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return Utf16TimeFormatter.TryFormatG(value, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return Utf8TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return Utf16TimeFormatter.TryFormatO(value, offset, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten, TextEncoder encoder)
        {
            // for now it only works for invariant culture
            if (encoder.IsInvariantUtf8)
                return Utf8TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else if (encoder.IsInvariantUtf16)
                return Utf16TimeFormatter.TryFormatRfc1123(value, buffer, out bytesWritten);
            else
                throw new NotImplementedException();
        }

        #endregion Time formatting methods

        #region Invariant UTF8 implementation

        static class Utf8TimeFormatter
        {
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

            public static bool TryFormatG(DateTime value, Span<byte> buffer, out int bytesWritten)
            {
                const int BytesNeeded = 19;

                if (buffer.Length < BytesNeeded)
                {
                    bytesWritten = 0;
                    return false;
                }

                ref byte utf8Bytes = ref buffer.DangerousGetPinnableReference();

                WriteTwoDigits(value.Month, ref utf8Bytes, 0);
                Unsafe.Add(ref utf8Bytes, 2) = Slash;

                WriteTwoDigits(value.Day, ref utf8Bytes, 3);
                Unsafe.Add(ref utf8Bytes, 5) = Slash;

                WriteFourDigits(value.Year, ref utf8Bytes, 6);
                Unsafe.Add(ref utf8Bytes, 10) = Space;

                WriteTwoDigits(value.Hour, ref utf8Bytes, 11);
                Unsafe.Add(ref utf8Bytes, 13) = Colon;

                WriteTwoDigits(value.Minute, ref utf8Bytes, 14);
                Unsafe.Add(ref utf8Bytes, 16) = Colon;

                WriteTwoDigits(value.Second, ref utf8Bytes, 17);

                bytesWritten = BytesNeeded;
                return true;
            }

            public static bool TryFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
            {
                const int MinimumBytesNeeded = 27;

                bytesWritten = MinimumBytesNeeded;
                DateTimeKind kind = DateTimeKind.Local;

                if (offset == NullOffset)
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

                WriteFourDigits(value.Year, ref utf8Bytes, 0);
                Unsafe.Add(ref utf8Bytes, 4) = Minus;

                WriteTwoDigits(value.Month, ref utf8Bytes, 5);
                Unsafe.Add(ref utf8Bytes, 7) = Minus;

                WriteTwoDigits(value.Day, ref utf8Bytes, 8);
                Unsafe.Add(ref utf8Bytes, 10) = TimeMarker;

                WriteTwoDigits(value.Hour, ref utf8Bytes, 11);
                Unsafe.Add(ref utf8Bytes, 13) = Colon;

                WriteTwoDigits(value.Minute, ref utf8Bytes, 14);
                Unsafe.Add(ref utf8Bytes, 16) = Colon;

                WriteTwoDigits(value.Second, ref utf8Bytes, 17);
                Unsafe.Add(ref utf8Bytes, 19) = Period;

                ulong fraction = (ulong)(value.Ticks % TimeSpan.TicksPerSecond);
                WriteFractionDigits(fraction, ref utf8Bytes, 20);

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
                    WriteTwoDigits(hours, ref utf8Bytes, 28);
                    Unsafe.Add(ref utf8Bytes, 30) = Colon;
                    WriteTwoDigits(offset.Minutes, ref utf8Bytes, 31);
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

                WriteTwoDigits(value.Day, ref utf8Bytes, 5);
                Unsafe.Add(ref utf8Bytes, 7) = (byte)' ';

                var monthAbbrev = MonthAbbreviations[value.Month - 1];
                Unsafe.Add(ref utf8Bytes, 8) = monthAbbrev[0];
                Unsafe.Add(ref utf8Bytes, 9) = monthAbbrev[1];
                Unsafe.Add(ref utf8Bytes, 10) = monthAbbrev[2];
                Unsafe.Add(ref utf8Bytes, 11) = Space;

                WriteFourDigits(value.Year, ref utf8Bytes, 12);
                Unsafe.Add(ref utf8Bytes, 16) = Space;

                WriteTwoDigits(value.Hour, ref utf8Bytes, 17);
                Unsafe.Add(ref utf8Bytes, 19) = Colon;

                WriteTwoDigits(value.Minute, ref utf8Bytes, 20);
                Unsafe.Add(ref utf8Bytes, 22) = Colon;

                WriteTwoDigits(value.Second, ref utf8Bytes, 23);
                Unsafe.Add(ref utf8Bytes, 25) = Space;

                Unsafe.Add(ref utf8Bytes, 26) = GMT1;
                Unsafe.Add(ref utf8Bytes, 27) = GMT2;
                Unsafe.Add(ref utf8Bytes, 28) = GMT3;

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteTwoDigits(int value, ref byte buffer, int index)
            {
                Precondition.Require(value >= 0 && value <= 99);

                ulong v1 = DivMod10((ulong)value, out ulong v2);
                Unsafe.Add(ref buffer, index) = (byte)('0' + v1);
                Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteFourDigits(int value, ref byte buffer, int index)
            {
                Precondition.Require(value >= 0 && value <= 9999);

                ulong left = DivMod10((ulong)value, out ulong v);
                Unsafe.Add(ref buffer, index + 3) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 2) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index) = (byte)('0' + v);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteFractionDigits(ulong value, ref byte buffer, int index)
            {
                //Precondition.Require(value >= 0 && value <= 9999);

                ulong left = DivMod10(value, out ulong v);
                Unsafe.Add(ref buffer, index + 6) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 5) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 4) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 3) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 2) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 1) = (byte)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index) = (byte)('0' + v);
            }
        }

        #endregion Invariant UTF8 implementation

        #region Invariant UTF16 implementation

        static class Utf16TimeFormatter
        {
            private const char Colon  = ':';
            private const char Comma  = ',';
            private const char Minus  = '-';
            private const char Period = '.';
            private const char Plus   = '+';
            private const char Slash  = '/';
            private const char Space  = ' ';

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

            public static bool TryFormatG(DateTime value, Span<byte> buffer, out int bytesWritten)
            {
                const int CharsNeeded = 19;

                bytesWritten = CharsNeeded * sizeof(char);
                if (buffer.Length < bytesWritten)
                {
                    bytesWritten = 0;
                    return false;
                }

                Span<char> dst;
                unsafe
                {
                    dst = new Span<char>(Unsafe.AsPointer(ref buffer.DangerousGetPinnableReference()), CharsNeeded);
                }
                ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

                WriteTwoDigits(value.Month, ref utf16Bytes, 0);
                Unsafe.Add(ref utf16Bytes, 2) = Slash;

                WriteTwoDigits(value.Day, ref utf16Bytes, 3);
                Unsafe.Add(ref utf16Bytes, 5) = Slash;

                WriteFourDigits(value.Year, ref utf16Bytes, 6);
                Unsafe.Add(ref utf16Bytes, 10) = Space;

                WriteTwoDigits(value.Hour, ref utf16Bytes, 11);
                Unsafe.Add(ref utf16Bytes, 13) = Colon;

                WriteTwoDigits(value.Minute, ref utf16Bytes, 14);
                Unsafe.Add(ref utf16Bytes, 16) = Colon;

                WriteTwoDigits(value.Second, ref utf16Bytes, 17);

                return true;
            }

            public static bool TryFormatO(DateTime value, TimeSpan offset, Span<byte> buffer, out int bytesWritten)
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

                Span<char> dst;
                unsafe
                {
                    dst = new Span<char>(Unsafe.AsPointer(ref buffer.DangerousGetPinnableReference()), charsNeeded);
                }
                ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

                WriteFourDigits(value.Year, ref utf16Bytes, 0);
                Unsafe.Add(ref utf16Bytes, 4) = Minus;

                WriteTwoDigits(value.Month, ref utf16Bytes, 5);
                Unsafe.Add(ref utf16Bytes, 7) = Minus;

                WriteTwoDigits(value.Day, ref utf16Bytes, 8);
                Unsafe.Add(ref utf16Bytes, 10) = TimeMarker;

                WriteTwoDigits(value.Hour, ref utf16Bytes, 11);
                Unsafe.Add(ref utf16Bytes, 13) = Colon;

                WriteTwoDigits(value.Minute, ref utf16Bytes, 14);
                Unsafe.Add(ref utf16Bytes, 16) = Colon;

                WriteTwoDigits(value.Second, ref utf16Bytes, 17);
                Unsafe.Add(ref utf16Bytes, 19) = Period;

                ulong fraction = (ulong)(value.Ticks % TimeSpan.TicksPerSecond);
                WriteFractionDigits(fraction, ref utf16Bytes, 20);

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
                    WriteTwoDigits(hours, ref utf16Bytes, 28);
                    Unsafe.Add(ref utf16Bytes, 30) = Colon;
                    WriteTwoDigits(offset.Minutes, ref utf16Bytes, 31);
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

                Span<char> dst;
                unsafe
                {
                    dst = new Span<char>(Unsafe.AsPointer(ref buffer.DangerousGetPinnableReference()), CharsNeeded);
                }
                ref char utf16Bytes = ref dst.DangerousGetPinnableReference();

                var dayAbbrev = DayAbbreviations[(int)value.DayOfWeek];
                Unsafe.Add(ref utf16Bytes, 0) = dayAbbrev[0];
                Unsafe.Add(ref utf16Bytes, 1) = dayAbbrev[1];
                Unsafe.Add(ref utf16Bytes, 2) = dayAbbrev[2];
                Unsafe.Add(ref utf16Bytes, 3) = Comma;
                Unsafe.Add(ref utf16Bytes, 4) = Space;

                WriteTwoDigits(value.Day, ref utf16Bytes, 5);
                Unsafe.Add(ref utf16Bytes, 7) = ' ';

                var monthAbbrev = MonthAbbreviations[value.Month - 1];
                Unsafe.Add(ref utf16Bytes, 8) = monthAbbrev[0];
                Unsafe.Add(ref utf16Bytes, 9) = monthAbbrev[1];
                Unsafe.Add(ref utf16Bytes, 10) = monthAbbrev[2];
                Unsafe.Add(ref utf16Bytes, 11) = Space;

                WriteFourDigits(value.Year, ref utf16Bytes, 12);
                Unsafe.Add(ref utf16Bytes, 16) = Space;

                WriteTwoDigits(value.Hour, ref utf16Bytes, 17);
                Unsafe.Add(ref utf16Bytes, 19) = Colon;

                WriteTwoDigits(value.Minute, ref utf16Bytes, 20);
                Unsafe.Add(ref utf16Bytes, 22) = Colon;

                WriteTwoDigits(value.Second, ref utf16Bytes, 23);
                Unsafe.Add(ref utf16Bytes, 25) = Space;

                Unsafe.Add(ref utf16Bytes, 26) = GMT1;
                Unsafe.Add(ref utf16Bytes, 27) = GMT2;
                Unsafe.Add(ref utf16Bytes, 28) = GMT3;

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteTwoDigits(int value, ref char buffer, int index)
            {
                Precondition.Require(value >= 0 && value <= 99);

                ulong v1 = DivMod10((ulong)value, out ulong v2);
                Unsafe.Add(ref buffer, index) = (char)('0' + v1);
                Unsafe.Add(ref buffer, index + 1) = (char)('0' + v2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteFourDigits(int value, ref char buffer, int index)
            {
                Precondition.Require(value >= 0 && value <= 9999);

                ulong left = DivMod10((ulong)value, out ulong v);
                Unsafe.Add(ref buffer, index + 3) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 2) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 1) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index) = (char)('0' + v);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteFractionDigits(ulong value, ref char buffer, int index)
            {
                //Precondition.Require(value >= 0 && value <= 9999);

                ulong left = DivMod10(value, out ulong v);
                Unsafe.Add(ref buffer, index + 6) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 5) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 4) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 3) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 2) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index + 1) = (char)('0' + v);

                left = DivMod10(left, out v);
                Unsafe.Add(ref buffer, index) = (char)('0' + v);
            }
        }

        #endregion Invariant UTF16 implementation

        #region Helper methods

        /// <summary>
        /// This method for doing division and modulus for integers by 10 is ~5-6 times faster
        /// than the division and multiplication operators.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong DivMod10(ulong n, out ulong modulo)
        {
            ulong d = ((0x1999999A * n) >> 32);
            modulo = n - (d * 10);
            return d;
        }

        #endregion Helper methods
    }
}
