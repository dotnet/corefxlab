// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text.Formatting 
{
    public static partial class PrimitiveFormatters
    {
        static readonly string[] s_dayNames = { "Sun, ", "Mon, ", "Tue, ", "Wed, ", "Thu, ", "Fri, ", "Sat, " };
        static readonly string[] s_monthNames = { "Jan ", "Feb ", "Mar ", "Apr ", "May", "Jun ", "Jul ", "Aug ", "Sep ", "Oct ", "Nov ", "Dec " };
        static readonly Format.Parsed d2 = new Format.Parsed() { Precision = 2, Symbol = Format.Symbol.D };
        static readonly Format.Parsed d4 = new Format.Parsed() { Precision = 4, Symbol = Format.Symbol.D };
        static readonly Format.Parsed d7 = new Format.Parsed() { Precision = 7, Symbol = Format.Symbol.D };
        static readonly Format.Parsed g = new Format.Parsed() { Symbol = Format.Symbol.G };
        const int FractionalTimeScale = 10000000;

        public static bool TryFormat(this DateTime value, Span<byte> buffer, ReadOnlySpan<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            Precondition.Require(format.Symbol == Format.Symbol.R || format.Symbol == Format.Symbol.O || format.Symbol == Format.Symbol.G);

            switch (format.Symbol)
            {
                case Format.Symbol.R:
                    var utc = value.ToUniversalTime();
                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case Format.Symbol.O:
                    if (formattingData.IsUtf16)
                    {
                        return TryFormatDateTimeFormatO(value, buffer, FormattingData.InvariantUtf16, out bytesWritten);
                    }
                    else
                    {
                        return TryFormatDateTimeFormatO(value, buffer, FormattingData.InvariantUtf8, out bytesWritten);
                    }
                case Format.Symbol.G:
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
            if (!TryWriteInt32(value.Month, buffer, g, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('/', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, g, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('/', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Year, buffer, g, formattingData, ref bytesWritten)) { return false; }
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

            if (!TryWriteInt32(hour, buffer, g, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, d2, formattingData, ref bytesWritten)) { return false; }
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

        static bool TryFormatDateTimeFormatO(DateTime value, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            if(value.Kind != DateTimeKind.Utc)
            {
                throw new NotImplementedException("Format O supports UTC date time only.");
            }

            bytesWritten = 0;
            if (!TryWriteInt32(value.Year, buffer, d4, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Month, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar('T', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Hour, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, d2, formattingData, ref bytesWritten)) { return false; }

            // add optional fractional second only if needed...
            var rounded = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
            var delta = value - rounded;

            if (delta.Ticks != 0)
            {
                if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
                var timeFrac = delta.Ticks * FractionalTimeScale / System.TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(timeFrac, buffer, d7, formattingData, ref bytesWritten)) { return false; }
            }

            if (!TryWriteChar('Z', buffer, formattingData, ref bytesWritten)) { return false; }
            return true;
        }

        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, FormattingData formattingData, out int bytesWritten)
        {
            bytesWritten = 0;
            if (!TryWriteString(s_dayNames[(int)value.DayOfWeek], buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Day, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteString(s_monthNames[value.Month - 1], buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Year, buffer, d4, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(' ', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Hour, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Minute, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteInt32(value.Second, buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteString(" GMT", buffer, formattingData, ref bytesWritten)) { return false; }
            return true;
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, ReadOnlySpan<char> format, FormattingData formattingData, out int bytesWritten)
        {
            Format.Parsed parsedFormat = Format.Parse(format);
            return TryFormat(value, buffer, parsedFormat, formattingData, out bytesWritten);
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, Format.Parsed format, FormattingData formattingData, out int bytesWritten)
        {
            Precondition.Require(format.Symbol == default(Format.Parsed).Symbol);

            bytesWritten = 0;

            if (value.Ticks < 0)
            {
                if (!TryWriteChar('-', buffer, formattingData, ref bytesWritten)) { return false; }
            }

            if (value.Days != 0)
            {
                if (!TryWriteInt32(Abs(value.Days), buffer, default(Format.Parsed), formattingData, ref bytesWritten)) { return false; }
                if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
            }

            if (!TryWriteInt32(Abs(value.Hours), buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(Abs(value.Minutes), buffer, d2, formattingData, ref bytesWritten)) { return false; }
            if (!TryWriteChar(':', buffer, formattingData, ref bytesWritten)) { return false; }

            if (!TryWriteInt32(Abs(value.Seconds), buffer, d2, formattingData, ref bytesWritten)) { return false; }

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

            if (remainingTicks != 0)
            {
                if (!TryWriteChar('.', buffer, formattingData, ref bytesWritten)) { return false; }
                var fraction = remainingTicks * FractionalTimeScale / TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(fraction, buffer, d7, formattingData, ref bytesWritten)) { return false; }
            }

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
