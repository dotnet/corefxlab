// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatters
    {
        static readonly string[] s_dayNames = { "Sun, ", "Mon, ", "Tue, ", "Wed, ", "Thu, ", "Fri, ", "Sat, " };
        static readonly string[] s_monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        static readonly Format.Parsed D2 = new Format.Parsed('D', 2);
        static readonly Format.Parsed D4 = new Format.Parsed('D', 4);
        static readonly Format.Parsed D7 = new Format.Parsed('D', 7);
        static readonly Format.Parsed G = new Format.Parsed('G'); 
        static readonly Format.Parsed t = new Format.Parsed('t'); 
        const int FractionalTimeScale = 10000000;

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, Span<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');
            switch (format.Symbol)
            {
                case 'R':

                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case 'O':
                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeFormatO(value.UtcDateTime, false, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeFormatO(value.UtcDateTime, false, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case 'G':
                    return TryFormatDateTimeFormagG(value.DateTime, buffer, formattingData, out bytesWritten);
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, Span<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');

            switch (format.Symbol)
            {
                case 'R':
                    var utc = value.ToUniversalTime();
                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case 'O':
                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeFormatO(value, true, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeFormatO(value, true, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case 'G':
                    return TryFormatDateTimeFormagG(value, buffer, formattingData, out bytesWritten);
                default:
                    throw new NotImplementedException();
            }      
        }

        static bool TryFormatDateTimeFormagG(DateTime value, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            // for now it only works for invariant culture
            if(!formattingData.IsInvariantUtf16 && !formattingData.IsInvariantUtf8)
            {
                throw new NotImplementedException();
            }

            bytesWritten = 0;
            if (!TryWriteInt32(value.Month, buffer, G, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('/', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, G, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('/', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Year, buffer, G, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }

            var hour = value.Hour;
            if(hour == 0)
            {
                hour = 12; 
            }
            if(hour > 12)
            {
                hour = hour - 12;
            }

            if (!TryWriteInt32(hour, buffer, G, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }

            if(value.Hour > 11)
            {
                TryWriteString("PM", buffer, formattingData, ref bytesWritten);
            }
            else
            {
                TryWriteString("AM", buffer, formattingData, ref bytesWritten);
            }

            return true;
        }

        static bool TryFormatDateTimeFormatO(DateTimeOffset value, bool isDateTime, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            bytesWritten = 0;
            if (!TryWriteInt32(value.Year, buffer, D4, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Month, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('T', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Hour, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, D2, formattingData, ref bytesWritten)) { return false; }

            // add optional fractional second only if needed...
            var rounded = new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, TimeSpan.Zero);
            var delta = value - rounded;

            if (delta.Ticks != 0)
            {
                if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
                var timeFrac = delta.Ticks * FractionalTimeScale / System.TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(timeFrac, buffer, D7, formattingData, ref bytesWritten)) { return false; }
            }

            if (isDateTime)
            {
                if (!TryWriteChar('Z', buffer, formattingData, ref bytesWritten)) { return false; }
            }
            else
            {
                if (!TryWriteChar('+', buffer, formattingData, ref bytesWritten)) { return false; }
                int bytes;
                if (!value.Offset.TryFormat(buffer.Slice(bytesWritten), t, formattingData, out bytes)) { return false; }
                bytesWritten += bytes;
            }

            return true;
        }

        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            bytesWritten = 0;
            if (!TryWriteString(s_dayNames[(int)value.DayOfWeek], buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Day, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteString(s_monthNames[value.Month - 1], buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Year, buffer, D4, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Hour, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Minute, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Second, buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteString(" GMT", buffer, formattingData, ref bytesWritten)) { return false; }
            return true;
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, Span<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'c';
            }
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 't' || format.Symbol == 'c' || format.Symbol == 'g');

            if (format.Symbol != 't')
            {
                return TryFormatTimeSpanG(value, buffer, format, formattingData, out bytesWritten);
            }

            // else it's format 't' (short time used to print time offsets)
            return TryFormatTimeSpanT(value, buffer, formattingData, out bytesWritten);
        }

        private static bool TryFormatTimeSpanG(TimeSpan value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            bytesWritten = 0;

            if (value.Ticks < 0)
            {
                if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
            }

            bool daysWritten = false;
            if (value.Days != 0 || format.Symbol == 'G')
            {
                if (!TryWriteInt32(Abs(value.Days), buffer, default(Format.Parsed), formattingData, ref bytesWritten)) { return false; }
                daysWritten = true;
                if (format.Symbol == 'c')
                {
                    if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
                }
                else
                {
                    if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }
                }
            }

            var hourFormat = default(Format.Parsed);
            if ((daysWritten || format.Symbol == 'c') && format.Symbol != 'g')
            {
                hourFormat = D2;
            }
            if (!TryWriteInt32(Abs(value.Hours), buffer, hourFormat, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(Abs(value.Minutes), buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(Abs(value.Seconds), buffer, D2, formattingData, ref bytesWritten)) { return false; }

            long remainingTicks;
            if (value.Ticks != long.MinValue)
            {
                remainingTicks = Abs(value.Ticks) % TimeSpan.TicksPerSecond;
            }
            else
            {
                remainingTicks = long.MaxValue % TimeSpan.TicksPerSecond;
                remainingTicks = (remainingTicks + 1) % TimeSpan.TicksPerSecond;
            }

            var ticksFormat = D7;
            if (remainingTicks != 0)
            {
                if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
                var fraction = remainingTicks * FractionalTimeScale / TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(fraction, buffer, ticksFormat, formattingData, ref bytesWritten)) { return false; }
            }

            return true;
        }

        private static bool TryFormatTimeSpanT(TimeSpan value, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            bytesWritten = 0;

            if (value.Ticks < 0)
            {
                if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
            }

            if (!TryWriteInt32(Abs((int)value.TotalHours), buffer, D2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(Abs(value.Minutes), buffer, D2, formattingData, ref bytesWritten)) { return false; }

            return true;
        }

        // Math.Abs is in System.Runtime.Extensions. I don't want to add a dependency just for Abs.
        // Also, this is not a general purpose Abs. It works only for days, months, etc.
        static int Abs(int value)
        {
            if (value < 0) value = -value;
            return value;
        }
        static long Abs(long value)
        {
            if (value < 0) value = -value;
            return value;
        }
    }
}
