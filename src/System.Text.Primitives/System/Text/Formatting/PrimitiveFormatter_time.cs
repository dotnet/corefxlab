// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        static readonly string[] s_dayNames = { "Sun, ", "Mon, ", "Tue, ", "Wed, ", "Thu, ", "Fri, ", "Sat, " };
        static readonly string[] s_monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        static readonly byte[][] s_dayNamesUtf8 = {
            new Utf8String("Sun, ").Bytes.ToArray(),
            new Utf8String("Mon, ").Bytes.ToArray(),
            new Utf8String("Tue, ").Bytes.ToArray(),
            new Utf8String("Wed, ").Bytes.ToArray(),
            new Utf8String("Thu, ").Bytes.ToArray(),
            new Utf8String("Fri, ").Bytes.ToArray(),
            new Utf8String("Sat, ").Bytes.ToArray(),
        };
        static readonly byte[][] s_monthNamesUtf8 = {
            new Utf8String("Jan").Bytes.ToArray(),
            new Utf8String("Feb").Bytes.ToArray(),
            new Utf8String("Mar").Bytes.ToArray(),
            new Utf8String("Apr").Bytes.ToArray(),
            new Utf8String("May").Bytes.ToArray(),
            new Utf8String("Jun").Bytes.ToArray(),
            new Utf8String("Jul").Bytes.ToArray(),
            new Utf8String("Aug").Bytes.ToArray(),
            new Utf8String("Sep").Bytes.ToArray(),
            new Utf8String("Oct").Bytes.ToArray(),
            new Utf8String("Nov").Bytes.ToArray(),
            new Utf8String("Dec").Bytes.ToArray(),
        };
        static readonly byte[] s_gmtUtf8Bytes = new Utf8String(" GMT").Bytes.ToArray();

        static readonly TextFormat D2 = new TextFormat('D', 2);
        static readonly TextFormat D4 = new TextFormat('D', 4);
        static readonly TextFormat D7 = new TextFormat('D', 7);
        static readonly TextFormat G = new TextFormat('G'); 
        static readonly TextFormat t = new TextFormat('t'); 
        const int FractionalTimeScale = 10000000;

        public static bool TryFormat(this DateTimeOffset value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'G';
            }
            Precondition.Require(format.Symbol == 'R' || format.Symbol == 'O' || format.Symbol == 'G');
            switch (format.Symbol)
            {
                case 'R':

                    if (encoding.IsInvariantUtf16) // TODO: there are many checks like this one in the code. They need to also verify that the UTF8 branch is invariant.
                    {
                        return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, out bytesWritten, EncodingData.InvariantUtf16);
                    }
                    else
                    {
                        return TryFormatDateTimeRfc1123(value.UtcDateTime, buffer, out bytesWritten, EncodingData.InvariantUtf8);
                    }
                case 'O':
                    if (encoding.IsInvariantUtf16)
                    {
                        return TryFormatDateTimeFormatO(value.UtcDateTime, false, buffer, out bytesWritten, EncodingData.InvariantUtf16);
                    }
                    else
                    {
                        return TryFormatDateTimeFormatO(value.UtcDateTime, false, buffer, out bytesWritten, EncodingData.InvariantUtf8);
                    }
                case 'G':
                    return TryFormatDateTimeFormatG(value.DateTime, buffer, out bytesWritten, encoding);
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryFormat(this DateTime value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
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
                    if (encoding.IsInvariantUtf16)
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, out bytesWritten, EncodingData.InvariantUtf16);
                    }
                    else
                    {
                        return TryFormatDateTimeRfc1123(utc, buffer, out bytesWritten, EncodingData.InvariantUtf8);
                    }
                case 'O':
                    if (encoding.IsInvariantUtf16)
                    {
                        return TryFormatDateTimeFormatO(value, true, buffer, out bytesWritten, EncodingData.InvariantUtf16);
                    }
                    else
                    {
                        return TryFormatDateTimeFormatO(value, true, buffer, out bytesWritten, EncodingData.InvariantUtf8);
                    }
                case 'G':
                    return TryFormatDateTimeFormatG(value, buffer, out bytesWritten, encoding);
                default:
                    throw new NotImplementedException();
            }      
        }

        static bool TryFormatDateTimeFormatG(DateTime value, Span<byte> buffer, out int bytesWritten, EncodingData encoding)
        {
            // for now it only works for invariant culture
            if(!encoding.IsInvariantUtf16 && !encoding.IsInvariantUtf8)
            {
                throw new NotImplementedException();
            }

            bytesWritten = 0;
            if (!TryWriteInt32(value.Month, buffer, ref bytesWritten, G, encoding)) { return false; }
            if (!TryWriteChar('/', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, ref bytesWritten, G, encoding)) { return false; }
            if (!TryWriteChar('/', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Year, buffer, ref bytesWritten, G, encoding)) { return false; }
            if (!TryWriteChar(' ', buffer, ref bytesWritten, encoding)) { return false; }

            var hour = value.Hour;
            if(hour == 0)
            {
                hour = 12; 
            }
            if(hour > 12)
            {
                hour = hour - 12;
            }

            if (!TryWriteInt32(hour, buffer, ref bytesWritten, G, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(' ', buffer, ref bytesWritten, encoding)) { return false; }

            if(value.Hour > 11)
            {
                TryWriteString("PM", buffer, ref bytesWritten, encoding);
            }
            else
            {
                TryWriteString("AM", buffer, ref bytesWritten, encoding);
            }

            return true;
        }

        static bool TryFormatDateTimeFormatO(DateTimeOffset value, bool isDateTime, Span<byte> buffer, out int bytesWritten, EncodingData encoding)
        {
            bytesWritten = 0;
            if (!TryWriteInt32(value.Year, buffer, ref bytesWritten, D4, encoding)) { return false; }
            if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Month, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Day, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar('T', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Hour, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Minute, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(value.Second, buffer, ref bytesWritten, D2, encoding)) { return false; }

            // add optional fractional second only if needed...
            var rounded = new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, TimeSpan.Zero);
            var delta = value - rounded;

            if (delta.Ticks != 0)
            {
                if (!TryWriteChar('.', buffer, ref bytesWritten, encoding)) { return false; }
                var timeFrac = delta.Ticks * FractionalTimeScale / System.TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(timeFrac, buffer, ref bytesWritten, D7, encoding)) { return false; }
            }

            if (isDateTime)
            {
                if (!TryWriteChar('Z', buffer, ref bytesWritten, encoding)) { return false; }
            }
            else
            {
                if (!TryWriteChar('+', buffer, ref bytesWritten, encoding)) { return false; }
                int bytes;
                if (!value.Offset.TryFormat(buffer.Slice(bytesWritten), out bytes, t, encoding)) { return false; }
                bytesWritten += bytes;
            }

            return true;
        }

        static bool TryFormatDateTimeRfc1123(DateTime value, Span<byte> buffer, out int bytesWritten, EncodingData encoding)
        {
            if (encoding.IsInvariantUtf8)
            {
                bytesWritten = 0;
                if (buffer.Length < 29) {
                    return false;
                }

                s_dayNamesUtf8[(int)value.DayOfWeek].CopyTo(buffer);
                TryFormat(value.Day, buffer.Slice(5), out bytesWritten, D2, encoding);
                buffer[7] = (byte)' ';
                var monthBytes = s_monthNamesUtf8[value.Month - 1];
                monthBytes.CopyTo(buffer.Slice(8));
                buffer[11] = (byte)' ';
                TryFormat(value.Year, buffer.Slice(12), out bytesWritten, D4, encoding);
                buffer[16] = (byte)' ';
                TryFormat(value.Hour, buffer.Slice(17), out bytesWritten, D2, encoding);
                buffer[19] = (byte)':';
                TryFormat(value.Minute, buffer.Slice(20), out bytesWritten, D2, encoding);
                buffer[22] = (byte)':';
                TryFormat(value.Second, buffer.Slice(23), out bytesWritten, D2, encoding);
                s_gmtUtf8Bytes.CopyTo(buffer.Slice(25));
                bytesWritten = 29;
                return true;
            }

            bytesWritten = 0;
            if (!TryWriteString(s_dayNames[(int)value.DayOfWeek], buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteInt32(value.Day, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(' ', buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteString(s_monthNames[value.Month - 1], buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteChar(' ', buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteInt32(value.Year, buffer, ref bytesWritten, D4, encoding)) { return false; }
            if (!TryWriteChar(' ', buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteInt32(value.Hour, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteInt32(value.Minute, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }
            if (!TryWriteInt32(value.Second, buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteString(" GMT", buffer, ref bytesWritten, encoding)) { return false; }
            return true;      
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            if (format.IsDefault)
            {
                format.Symbol = 'c';
            }
            Precondition.Require(format.Symbol == 'G' || format.Symbol == 't' || format.Symbol == 'c' || format.Symbol == 'g');

            if (format.Symbol != 't')
            {
                return TryFormatTimeSpanG(value, buffer, out bytesWritten, format, encoding);
            }

            // else it's format 't' (short time used to print time offsets)
            return TryFormatTimeSpanT(value, buffer, out bytesWritten, encoding);
        }

        private static bool TryFormatTimeSpanG(TimeSpan value, Span<byte> buffer, out int bytesWritten, TextFormat format, EncodingData encoding)
        {
            bytesWritten = 0;

            if (value.Ticks < 0)
            {
                if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
            }

            bool daysWritten = false;
            if (value.Days != 0 || format.Symbol == 'G')
            {
                if (!TryWriteInt32(Abs(value.Days), buffer, ref bytesWritten, default(TextFormat), encoding)) { return false; }
                daysWritten = true;
                if (format.Symbol == 'c')
                {
                    if (!TryWriteChar('.', buffer, ref bytesWritten, encoding)) { return false; }
                }
                else
                {
                    if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }
                }
            }

            var hourFormat = default(TextFormat);
            if ((daysWritten || format.Symbol == 'c') && format.Symbol != 'g')
            {
                hourFormat = D2;
            }
            if (!TryWriteInt32(Abs(value.Hours), buffer, ref bytesWritten, hourFormat, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(Abs(value.Minutes), buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(Abs(value.Seconds), buffer, ref bytesWritten, D2, encoding)) { return false; }

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
                if (!TryWriteChar('.', buffer, ref bytesWritten, encoding)) { return false; }
                var fraction = remainingTicks * FractionalTimeScale / TimeSpan.TicksPerSecond;
                if (!TryWriteInt64(fraction, buffer, ref bytesWritten, ticksFormat, encoding)) { return false; }
            }

            return true;
        }

        private static bool TryFormatTimeSpanT(TimeSpan value, Span<byte> buffer, out int bytesWritten, EncodingData encoding)
        {
            bytesWritten = 0;

            if (value.Ticks < 0)
            {
                if (!TryWriteChar('-', buffer, ref bytesWritten, encoding)) { return false; }
            }

            if (!TryWriteInt32(Abs((int)value.TotalHours), buffer, ref bytesWritten, D2, encoding)) { return false; }
            if (!TryWriteChar(':', buffer, ref bytesWritten, encoding)) { return false; }

            if (!TryWriteInt32(Abs(value.Minutes), buffer, ref bytesWritten, D2, encoding)) { return false; }

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
