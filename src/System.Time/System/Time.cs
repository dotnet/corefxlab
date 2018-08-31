// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System
{
    /// <summary>
    /// Represents a time of day, as would be read from a clock, within the range 00:00:00 to 23:59:59.9999999
    /// Has properties for working with both 12-hour and 24-hour time values.
    /// </summary>
    [Serializable]
    [XmlSchemaProvider("GetSchema")]
    [StructLayout(LayoutKind.Auto)]
    public struct Time : IEquatable<Time>, IComparable<Time>, IComparable, IFormattable, IXmlSerializable
    {
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;   // 10,000,000
        private const long TicksPerMinute = TicksPerSecond * 60;         // 600,000,000
        private const long TicksPerHour = TicksPerMinute * 60;        // 36,000,000,000
        private const long TicksPerDay = TicksPerHour * 24;          // 864,000,000,000

        private const long MinTicks = 0L;
        private const long MaxTicks = 863999999999L;

        private static readonly Regex EscapeCharRegex = new Regex(@"\\.|"".*?""|'.*?'", RegexOptions.Compiled);
        private static readonly Regex InvalidFormatsRegex = new Regex(@"[dKMyz\/]+|%[dDfFgGmMrRuUyY]", RegexOptions.Compiled);
        private static readonly Regex ISOFormatRegex = new Regex(@"%[Oos]", RegexOptions.Compiled);

        /// <summary>
        /// Represents the smallest possible value of <see cref="Time"/>. This field is read-only.
        /// </summary>
        public static readonly Time MinValue = new Time(MinTicks);

        /// <summary>
        /// Represents the largest possible value of <see cref="Time"/>. This field is read-only.
        /// </summary>
        public static readonly Time MaxValue = new Time(MaxTicks);

        // Number of ticks (100ns units) since midnight at the beginning of a standard 24-hour day.
        // NOTE: This is the only field in this structure.
        private readonly long _ticks;

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to a specified number of ticks.
        /// </summary>
        /// <param name="ticks">
        /// A time expressed in the number of 100-nanosecond intervals that have elapsed since midnight (00:00:00),
        /// without regard to daylight saving time transitions.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ticks"/> is out of the range supported by the <see cref="Time"/> object.
        /// </exception>
        public Time(long ticks)
        {
            if (ticks < MinTicks || ticks > MaxTicks)
            {
                throw new ArgumentOutOfRangeException(nameof(ticks), ticks, Strings.ArgumentOutOfRange_TimeBadTicks);
            }

            Contract.EndContractBlock();

            _ticks = ticks;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour and minute.
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 0 or greater than 23.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// </exception>
        public Time(int hour, int minute)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            Contract.EndContractBlock();

            _ticks = hour * TicksPerHour +
                     minute * TicksPerMinute;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour, minute, and meridiem, using the hours of a 12-hour clock.
        /// </summary>
        /// <param name="hour">The hours (1 through 12).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="meridiem">The meridiem, either <see cref="System.Meridiem.AM"/>,
        /// or <see cref="System.Meridiem.PM"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 1 or greater than 12.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// </exception>
        public Time(int hour, int minute, Meridiem meridiem)
        {
            if (hour < 1 || hour > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour12HF);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            if (meridiem < Meridiem.AM || meridiem > Meridiem.PM)
            {
                throw new ArgumentOutOfRangeException(nameof(meridiem), meridiem, Strings.ArgumentOutOfRange_Meridiem);
            }

            Contract.EndContractBlock();

            int hours24 = Hours12To24(hour, meridiem);
            _ticks = hours24 * TicksPerHour +
                     minute * TicksPerMinute;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour, minute, and second, using the hours of a 24-hour clock.
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 0 or greater than 23.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="second"/> is less than 0 or greater than 59.
        /// </exception>
        public Time(int hour, int minute, int second)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            if (second < 0 || second > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(second), second, Strings.ArgumentOutOfRange_Second);
            }

            Contract.EndContractBlock();

            _ticks = hour * TicksPerHour +
                     minute * TicksPerMinute +
                     second * TicksPerSecond;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour, minute, second, and meridiem, using the hours of a 12-hour clock.
        /// </summary>
        /// <param name="hour">The hours (1 through 12).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="meridiem">The meridiem, either <see cref="System.Meridiem.AM"/>,
        /// or <see cref="System.Meridiem.PM"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 1 or greater than 12.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="second"/> is less than 0 or greater than 59.
        /// </exception>
        public Time(int hour, int minute, int second, Meridiem meridiem)
        {
            if (hour < 1 || hour > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour12HF);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            if (second < 0 || second > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(second), second, Strings.ArgumentOutOfRange_Second);
            }

            if (meridiem < Meridiem.AM || meridiem > Meridiem.PM)
            {
                throw new ArgumentOutOfRangeException(nameof(meridiem), meridiem, Strings.ArgumentOutOfRange_Meridiem);
            }

            Contract.EndContractBlock();

            int hours24 = Hours12To24(hour, meridiem);
            _ticks = hours24 * TicksPerHour +
                     minute * TicksPerMinute +
                     second * TicksPerSecond;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour, minute, second, and millisecond, using the hours of a 24-hour clock.
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 0 or greater than 23.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="second"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="millisecond"/> is less than 0 or greater than 999.
        /// </exception>
        public Time(int hour, int minute, int second, int millisecond)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            if (second < 0 || second > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(second), second, Strings.ArgumentOutOfRange_Second);
            }

            if (millisecond < 0 || millisecond > 999)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecond), millisecond, Strings.ArgumentOutOfRange_Millisecond);
            }

            Contract.EndContractBlock();

            _ticks = hour * TicksPerHour +
                     minute * TicksPerMinute +
                     second * TicksPerSecond +
                     millisecond * TicksPerMillisecond;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Time"/> structure to the specified
        /// hour, minute, second, millisecond, and meridiem, using the hours of a 12-hour clock.
        /// </summary>
        /// <param name="hour">The hours (1 through 12).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="meridiem">The meridiem, either <see cref="System.Meridiem.AM"/>,
        /// or <see cref="System.Meridiem.PM"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hour"/> is less than 1 or greater than 12.
        /// <para>-or-</para>
        /// <paramref name="minute"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="second"/> is less than 0 or greater than 59.
        /// <para>-or-</para>
        /// <paramref name="millisecond"/> is less than 0 or greater than 999.
        /// </exception>
        public Time(int hour, int minute, int second, int millisecond, Meridiem meridiem)
        {
            if (hour < 1 || hour > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), hour, Strings.ArgumentOutOfRange_Hour12HF);
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), minute, Strings.ArgumentOutOfRange_Minute);
            }

            if (second < 0 || second > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(second), second, Strings.ArgumentOutOfRange_Second);
            }

            if (millisecond < 0 || millisecond > 999)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecond), millisecond, Strings.ArgumentOutOfRange_Millisecond);
            }

            if (meridiem < Meridiem.AM || meridiem > Meridiem.PM)
            {
                throw new ArgumentOutOfRangeException(nameof(meridiem), meridiem, Strings.ArgumentOutOfRange_Meridiem);
            }

            Contract.EndContractBlock();

            int hours24 = Hours12To24(hour, meridiem);
            _ticks = hours24 * TicksPerHour +
                     minute * TicksPerMinute +
                     second * TicksPerSecond +
                     millisecond * TicksPerMillisecond;
        }

        /// <summary>
        /// Gets the hour component of the time represented by this instance, using the hours of a 24-hour clock.
        /// </summary>
        /// <value>The hour component, expressed as a value between 0 and 23.</value>
        public int Hour
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 23);

                return (int)((_ticks / TicksPerHour) % 24);
            }
        }

        /// <summary>
        /// Gets the hour component of the time represented by this instance, using the hours of a 12-hour clock.
        /// </summary>
        /// <value>The hour component, expressed as a value between 1 and 12.</value>
        public int HourOf12HourClock
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 1);
                Contract.Ensures(Contract.Result<int>() <= 12);

                return (Hour + 11) % 12 + 1;
            }
        }

        /// <summary>
        /// Gets the meridiem (AM or PM) of the time represented by this instance.
        /// The meridiem can be used inconjunction with the <see cref="HourOf12HourClock"/> property
        /// to represent this instance's time on a 12-hour clock.
        /// </summary>
        /// <value>An enumerated constant that indicates the meridiem of this <see cref="Time"/> value.</value>
        /// <remarks>
        /// Though commonly used in English, these abbreviations derive from Latin.
        /// AM is an abbreviation for "Ante Meridiem", meaning "before mid-day".
        /// PM is an abbreviation for "Post Meridiem", meaning "after mid-day".
        /// </remarks>
        public Meridiem Meridiem => Hour < 12 ? Meridiem.AM : Meridiem.PM;

        /// <summary>
        /// Gets the minute component of the time represented by this instance.
        /// </summary>
        /// <value>The minute component, expressed as a value between 0 and 59.</value>
        public int Minute
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 59);

                return (int)((_ticks / TicksPerMinute) % 60);
            }
        }

        /// <summary>
        /// Gets the second component of the time represented by this instance.
        /// </summary>
        /// <value>The second component, expressed as a value between 0 and 59.</value>
        public int Second
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 59);

                return (int)((_ticks / TicksPerSecond) % 60);
            }
        }

        /// <summary>
        /// Gets the millisecond component of the time represented by this instance.
        /// </summary>
        /// <value>The millisecond component, expressed as a value between 0 and 999.</value>
        public int Millisecond
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= 999);

                return (int)((_ticks / TicksPerMillisecond) % 1000);
            }
        }

        /// <summary>
        /// Gets the number of ticks that represent the time of this instance.
        /// </summary>
        /// <value>
        /// The number of ticks that represent the time of this instance.
        /// The value is between <c>Time.MinValue.Ticks</c> and <c>Time.MaxValue.Ticks</c>.
        /// </value>
        /// <remarks>
        /// Each tick is a 100-nanosecond interval.  Collectively, they represent the time that has
        /// elapsed since midnight (00:00:00), without regard to daylight saving time transitions.
        /// </remarks>
        public long Ticks
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= MinTicks);
                Contract.Ensures(Contract.Result<long>() <= MaxTicks);

                return _ticks;
            }
        }

        /// <summary>
        /// Creates a <see cref="DateTime"/> object from the current <see cref="Time"/> and the specified <see cref="Date"/>.
        /// The resulting value has a <see cref="DateTime.Kind"/> of <see cref="DateTimeKind.Unspecified"/>.
        /// </summary>
        /// <remarks>
        /// Since neither <see cref="Date"/> or <see cref="Time"/> keep track of <see cref="DateTimeKind"/>,
        /// recognize that the <see cref="DateTime"/> produced by <c>Time.Now.On(Date.Today)</c> will have
        /// <see cref="DateTimeKind.Unspecified"/>, rather than then <see cref="DateTimeKind.Local"/> that would be
        /// given by <c>DateTime.Now</c>.
        /// <para>The same applies for <see cref="DateTimeKind.Utc"/>.</para>
        /// </remarks>
        public DateTime On(Date date)
        {
            long ticks = date.DayNumber * TicksPerDay + _ticks;
            return new DateTime(ticks);
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object that is set to the current time in the specified time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="Time"/> for the specified time zone.</returns>
        public static Time NowInTimeZone(TimeZoneInfo timeZoneInfo)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTimeOffset localNow = TimeZoneInfo.ConvertTime(utcNow, timeZoneInfo);
            return TimeFromTimeSpan(localNow.TimeOfDay);
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object that is set to the current time,
        /// expressed in this computer's local time zone.
        /// </summary>
        /// <value>An object whose value is the current local time.</value>
        public static Time Now
        {
            get
            {
                DateTime localNow = DateTime.Now;
                return TimeFromTimeSpan(localNow.TimeOfDay);
            }
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object that is set to the current time,
        /// expressed as Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>An object whose value is the current UTC time.</value>
        public static Time UtcNow
        {
            get
            {
                DateTime utcNow = DateTime.UtcNow;
                return TimeFromTimeSpan(utcNow.TimeOfDay);
            }
        }

        /// <summary>
        /// Determines if a time falls within the range provided.
        /// Supports both "normal" ranges such as 10:00-12:00, and ranges that span midnight such as 23:00-01:00.
        /// </summary>
        /// <param name="startTime">The starting time of day, inclusive.</param>
        /// <param name="endTime">The ending time of day, exclusive.</param>
        /// <returns>True, if the time falls within the range, false otherwise.</returns>
        public bool IsBetween(Time startTime, Time endTime)
        {
            long startTicks = startTime._ticks;
            long endTicks = endTime._ticks;

            return startTicks <= endTicks
                ? (startTicks <= _ticks && endTicks > _ticks)
                : (startTicks <= _ticks || endTicks > _ticks);
        }

        /// <summary>
        /// Subtracts another <see cref="Time"/> value from this instance, returning a <see cref="TimeSpan"/>.
        /// Assumes a standard day, with no invalid or ambiguous times due to Daylight Saving Time.
        /// Supports both "normal" ranges such as 10:00-12:00, and ranges that span midnight such as 23:00-01:00.
        /// </summary>
        /// <param name="startTime">The starting time of day, inclusive.</param>
        /// <returns>
        /// A <see cref="TimeSpan"/> representing the duration between the <paramref name="startTime"/>
        /// (inclusive), and this instance (exclusive).
        /// </returns>
        public TimeSpan Subtract(Time startTime)
        {
            return TimeSpan.FromTicks((_ticks - startTime._ticks + TicksPerDay) % TicksPerDay);
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified amount of time. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="timeSpan">The amount of time to adjust by. The value can be negative or positive.</param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="timeSpan"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:00 plus two hours is 01:00.
        /// </remarks>
        public Time Add(TimeSpan timeSpan)
        {
            return AddTicks(timeSpan.Ticks);
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified number of hours. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="hours">The number of hours to adjust by. The value can be negative or positive.</param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="hours"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:00 plus one hour is 00:00.
        /// </remarks>
        public Time AddHours(double hours)
        {
            return AddTicks((long)(hours * TicksPerHour));
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified number of minutes. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="minutes">The number of minutes to adjust by. The value can be negative or positive.</param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="minutes"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:59 plus one minute is 00:00.
        /// </remarks>
        public Time AddMinutes(double minutes)
        {
            return AddTicks((long)(minutes * TicksPerMinute));
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified number of seconds. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="seconds">The number of seconds to adjust by. The value can be negative or positive.</param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="seconds"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:59:59 plus one second is 00:00:00.
        /// </remarks>
        public Time AddSeconds(double seconds)
        {
            return AddTicks((long)(seconds * TicksPerSecond));
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified number of milliseconds. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="milliseconds">
        /// The number of milliseconds to adjust by. The value can be negative or positive.
        /// </param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="milliseconds"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:59:59.9990000 plus one millisecond is 00:00:00.0000000.
        /// </remarks>
        public Time AddMilliseconds(double milliseconds)
        {
            return AddTicks((long)(milliseconds * TicksPerMillisecond));
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified number of ticks. Positive values will move the time forward; negative values will move the
        /// time backwards.
        /// </summary>
        /// <param name="ticks">
        /// The number of ticks to adjust by. The value can be negative or positive.
        /// A tick is a unit of time equal to 100 nanoseconds.
        /// </param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="ticks"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:59:59.9999999 plus one tick is 00:00:00.0000000.
        /// </remarks>
        public Time AddTicks(long ticks)
        {
            long t = (_ticks + TicksPerDay + (ticks % TicksPerDay)) % TicksPerDay;
            return new Time(t);
        }

        /// <summary>
        /// Gets a <see cref="Time"/> object whose value is ahead or behind the value of this instance by the
        /// specified amount of time. Positive values will move the time backwards; negative values will move the
        /// time forward.  This is equivalent to calling <c>Add(timeSpan.Negate())</c>.
        /// </summary>
        /// <param name="timeSpan">The amount of time to adjust by. The value can be negative or positive.</param>
        /// <returns>
        /// A new <see cref="Time"/> object which is the result of adjusting this instance by the
        /// <paramref name="timeSpan"/> specified.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries backwards
        /// into the previous day.  For example, 01:00 minus two hours is 23:00.
        /// </remarks>
        public Time Subtract(TimeSpan timeSpan)
        {
            return AddTicks(-timeSpan.Ticks);
        }


        /// <summary>
        /// Adds a specified time interval to a specified time, yielding a new time.
        /// </summary>
        /// <param name="time">The time of day value to add to.</param>
        /// <param name="timeSpan">The time interval to add.</param>
        /// <returns>
        /// A <see cref="Time"/> object which is the result of adding the <paramref name="timeSpan"/>
        /// specified to the <paramref name="time"/> provided.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries forward
        /// into the next day.  For example, 23:00 plus two hours is 01:00.
        /// </remarks>
        public static Time operator +(Time time, TimeSpan timeSpan)
        {
            return time.Add(timeSpan);
        }

        /// <summary>
        /// Subtracts a specified time interval to a specified time, yielding a new time.
        /// </summary>
        /// <param name="time">The time of day value to subtract from.</param>
        /// <param name="timeSpan">The time interval to subtract.</param>
        /// <returns>
        /// A <see cref="Time"/> object which is the result of subtracting the <paramref name="timeSpan"/>
        /// specified from the <paramref name="time"/> provided.
        /// </returns>
        /// <remarks>
        /// The time is modeled on a circular 24-hour clock.  When a value crosses midnight, it carries backwards
        /// into the previous day.  For example, 01:00 minus two hours is 23:00.
        /// </remarks>
        public static Time operator -(Time time, TimeSpan timeSpan)
        {
            return time.Subtract(timeSpan);
        }

        /// <summary>
        /// Calculates the duration between the <paramref name="startTime"/> and <see cref="endTime"/>.
        /// Assumes a standard day, with no invalid or ambiguous times due to Daylight Saving Time.
        /// Supports both "normal" ranges such as 10:00-12:00, and ranges that span midnight such as 23:00-01:00.
        /// </summary>
        /// <param name="startTime">The starting time of day, inclusive.</param>
        /// <param name="endTime">The ending time of day, exclusive.</param>
        /// <returns>
        /// A <see cref="TimeSpan"/> representing the duration between the two time of day values.
        /// </returns>
        public static TimeSpan operator -(Time endTime, Time startTime)
        {
            return endTime.Subtract(startTime);
        }

        /// <summary>
        /// Compares two instances of <see cref="Time"/> and returns an integer that indicates whether the first
        /// instance is earlier than, the same as, or later than the second instance, within the same day.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of <paramref name="left"/> and <paramref name="right"/>.
        /// <list type="table">
        /// <listheader><term>Value</term><term>Description</term></listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <term><paramref name="left"/> is earlier than <paramref name="right"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <term><paramref name="left"/> is the same as <paramref name="right"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Greater than zero</term>
        ///   <term><paramref name="left"/> is later than <paramref name="right"/>.</term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public static int Compare(Time left, Time right)
        {
            if (left._ticks > right._ticks)
            {
                return 1;
            }

            if (left._ticks < right._ticks)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Compares the value of this instance to a specified <see cref="Time"/> value and returns an integer
        /// that indicates whether this instance is earlier than, the same as, or later than the specified
        /// <see cref="Time"/> value, within the same day.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the <paramref name="value"/> parameter.
        /// <list type="table">
        /// <listheader><term>Value</term><term>Description</term></listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <term>This instance is earlier than <paramref name="value"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <term>This instance is the same as <paramref name="value"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Greater than zero</term>
        ///   <term>This instance is later than <paramref name="value"/>.</term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public int CompareTo(Time value)
        {
            return Compare(this, value);
        }

        /// <summary>
        /// Compares the value of this instance to a specified object that contains a <see cref="Time"/> value and
        /// returns an integer that indicates whether this instance is earlier than, the same as, or later than the
        /// specified <see cref="Time"/> value, within the same day.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the <paramref name="value"/> parameter.
        /// <list type="table">
        /// <listheader><term>Value</term><term>Description</term></listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <term>This instance is earlier than <paramref name="value"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <term>This instance is earlier than <paramref name="value"/>.</term>
        /// </item>
        /// <item>
        ///   <term>Greater than zero</term>
        ///   <term>
        ///     This instance is later than <paramref name="value"/>,
        ///     or <paramref name="value"/> is <c>null</c>.
        ///   </term>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is not a <see cref="Time"/>.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (!(value is Time))
            {
                throw new ArgumentException(Strings.Argument_MustBeTime);
            }

            return Compare(this, (Time)value);
        }

        /// <summary>
        /// Returns a value indicating whether two <see cref="Time"/> instances have the same time value.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the two values are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Time left, Time right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value indicating whether the value of this instance is equal to the value of the specified
        /// <see cref="Time"/> instance.
        /// </summary>
        /// <param name="value">The other <see cref="Time"/> object to compare against this instance.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> parameter equals the value of this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Time value)
        {
            return _ticks == value._ticks;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="value">The object to compare to this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is an instance of <see cref="Time"/>
        /// and equals the value of this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (ReferenceEquals(null, value))
            {
                return false;
            }

            return value is Time time && Equals(time);
        }

        /// <summary>
        /// Returns the hash code of this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <remarks>
        /// The hash code of a <see cref="Time"/> object is the same as the hash code of
        /// its <see cref="Ticks"/> value.
        /// </remarks>
        public override int GetHashCode()
        {
            return _ticks.GetHashCode();
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="Time"/> object.</returns>
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return DateTime.MinValue.AddTicks(_ticks).ToString("T");
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent string representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of the current <see cref="Time"/> object as specified by
        /// <paramref name="provider"/>.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return DateTime.MinValue.AddTicks(_ticks).ToString("T", provider);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent string representation
        /// using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom time-of-day format string.</param>
        /// <returns>
        /// A string representation of value of the current <see cref="Time"/> object as specified by
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="FormatException">
        /// The length of <paramref name="format"/> is 1, and it is not one of the format specifier characters defined
        /// for <see cref="DateTimeFormatInfo"/>.
        /// <para>-or-</para>
        /// <paramref name="format"/> does not contain a valid custom format pattern.
        /// <para>-or-</para>
        /// The standard or custom format specified is not valid for a <see cref="Time"/> object, because it
        /// contains a date component.
        /// </exception>
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            format = NormalizeTimeFormat(format);
            return DateTime.MinValue.AddTicks(_ticks).ToString(format);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent string representation
        /// using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom time-of-day format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of the current <see cref="Time"/> object as specified by
        /// <paramref name="format"/> and <paramref name="provider"/>.
        /// </returns>
        /// <exception cref="FormatException">
        /// The length of <paramref name="format"/> is 1, and it is not one of the format specifier characters defined
        /// for <see cref="DateTimeFormatInfo"/>.
        /// <para>-or-</para>
        /// <paramref name="format"/> does not contain a valid custom format pattern.
        /// <para>-or-</para>
        /// The standard or custom format specified is not valid for a <see cref="Time"/> object, because it
        /// contains a date component.
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            format = NormalizeTimeFormat(format);
            return DateTime.MinValue.AddTicks(_ticks).ToString(format, provider);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent
        /// long time string representation.
        /// </summary>
        /// <returns>A string that contains the long time string representation of the
        /// current <see cref="Time"/> object.</returns>
        /// <remarks>The value of the current <see cref="Time"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.LongTimePattern" />
        /// property associated with the current thread culture.</remarks>
        public string ToLongTimeString()
        {
            return ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent
        /// long time string representation.
        /// </summary>
        /// <returns>A string that contains the long time string representation of the
        /// current <see cref="Time"/> object.</returns>
        /// <remarks>The value of the current <see cref="Time"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.LongTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public string ToLongTimeStringInvariant()
        {
            return ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent
        /// short time string representation.
        /// </summary>
        /// <returns>A string that contains the short time string representation of the
        /// current <see cref="Time"/> object.</returns>
        /// <remarks>The value of the current <see cref="Time"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.ShortTimePattern" />
        /// property associated with the current thread culture.</remarks>
        public string ToShortTimeString()
        {
            return ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="Time"/> object to its equivalent
        /// short time string representation.
        /// </summary>
        /// <returns>A string that contains the short time string representation of the
        /// current <see cref="Time"/> object.</returns>
        /// <remarks>The value of the current <see cref="Time"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.ShortTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public string ToShortTimeStringInvariant()
        {
            return ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortTimePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the string representation of a time-of-day to its <see cref="Time"/> equivalent.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <returns>An object that is equivalent to the time-of-day contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> does not contain a valid string representation of a time-of-day.
        /// </exception>
        public static Time Parse(string s)
        {
            DateTime dt = DateTime.Parse(s, null, DateTimeStyles.NoCurrentDateDefault);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// by using culture-specific format information.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <returns>
        /// An object that is equivalent to the time-of-day contained in <paramref name="s"/>,
        /// as specified by <paramref name="provider"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> does not contain a valid string representation of a time-of-day.
        /// </exception>
        public static Time Parse(string s, IFormatProvider provider)
        {
            DateTime dt = DateTime.Parse(s, provider, DateTimeStyles.NoCurrentDateDefault);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// by using culture-specific format information and formatting style.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <returns>
        /// An object that is equivalent to the time-of-day contained in <paramref name="s"/>,
        /// as specified by <paramref name="provider"/> and <paramref name="styles"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> does not contain a valid string representation of a time-of-day.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static Time Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            DateTime dt = DateTime.Parse(s, provider, styles);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified format and culture-specific format information.
        /// The format of the string representation must match the specified format exactly or an exception is thrown.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <returns>
        /// An object that is equivalent to the time-of-day contained in <paramref name="s"/>,
        /// as specified by <paramref name="format"/> and <paramref name="provider"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> or <paramref name="format"/> is an empty string.
        /// <para>-or-</para>
        /// <paramref name="s"/> does not contain a time-of-day that corresponds to the pattern specified in
        /// <paramref name="format"/>.
        /// <para>-or-</para>
        /// <paramref name="format"/> contains a format pattern that is not applicable to a <see cref="Time"/>.
        /// </exception>
        public static Time ParseExact(string s, string format, IFormatProvider provider)
        {
            format = NormalizeTimeFormat(format);
            DateTime dt = DateTime.ParseExact(s, format, provider, DateTimeStyles.NoCurrentDateDefault);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified format, culture-specific format information, and style.
        /// The format of the string representation must match the specified format exactly or an exception is thrown.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <returns>
        /// An object that is equivalent to the time-of-day contained in <paramref name="s"/>,
        /// as specified by <paramref name="format"/>, <paramref name="provider"/> and <paramref name="styles"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> or <paramref name="format"/> is an empty string.
        /// <para>-or-</para>
        /// <paramref name="s"/> does not contain a time-of-day that corresponds to the pattern specified in
        /// <paramref name="format"/>.
        /// <para>-or-</para>
        /// <paramref name="format"/> contains a format pattern that is not applicable to a <see cref="Time"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static Time ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            format = NormalizeTimeFormat(format);
            DateTime dt = DateTime.ParseExact(s, format, provider, styles);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified array of formats, culture-specific format information, and style.
        /// The format of the string representation must match at least one of the specified formats
        /// exactly or an exception is thrown.
        /// </summary>
        /// <param name="s">A string that contains a time-of-day to convert.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <returns>
        /// An object that is equivalent to the time-of-day contained in <paramref name="s"/>,
        /// as specified by <paramref name="formats"/>, <paramref name="provider"/> and <paramref name="styles"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="s"/> is an empty string.
        /// <para>-or-</para>
        /// An element of <paramref name="formats"/> is an empty string.
        /// <para>-or-</para>
        /// <paramref name="s"/> does not contain a time-of-day that corresponds to any element of
        /// <paramref name="formats"/>.
        /// <para>-or-</para>
        /// An element of <paramref name="formats"/> contains a format pattern that is not applicable to a
        /// <see cref="Time"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static Time ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            for (int i = 0; i < formats.Length; i++)
            {
                formats[i] = NormalizeTimeFormat(formats[i]);
            }

            DateTime dt = DateTime.ParseExact(s, formats, provider, styles);
            return TimeFromTimeSpan(dt.TimeOfDay);
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a time-of-day to convert.</param>
        /// <param name="time">
        /// When this method returns, contains the <see cref="Time"/> value equivalent to the time-of-day
        /// contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="MinValue"/>
        /// if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is
        /// <c>null</c>, is an empty string (""), or does not contain a valid string representation of a time-of-day.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="s"/> parameter was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParse(string s, out Time time)
        {
            if (!DateTime.TryParse(s, null, DateTimeStyles.NoCurrentDateDefault, out DateTime dt))
            {
                time = default;
                return false;
            }

            time = TimeFromTimeSpan(dt.TimeOfDay);
            return true;
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified culture-specific format information and formatting style,
        /// and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a time-of-day to convert.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <param name="time">
        /// When this method returns, contains the <see cref="Time"/> value equivalent to the time-of-day
        /// contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="MinValue"/>
        /// if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is
        /// <c>null</c>, is an empty string (""), or does not contain a valid string representation of a time-of-day.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="s"/> parameter was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out Time time)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            if (!DateTime.TryParse(s, provider, styles, out DateTime dt))
            {
                time = default;
                return false;
            }

            time = TimeFromTimeSpan(dt.TimeOfDay);
            return true;
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified format, culture-specific format information, and style.
        /// The format of the string representation must match the specified format exactly.
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a time-of-day to convert.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <param name="time">
        /// When this method returns, contains the <see cref="Time"/> value equivalent to the time-of-day
        /// contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="MinValue"/>
        /// if the conversion failed. The conversion fails if either the <paramref name="s"/> or
        /// <paramref name="format"/> parameter is <c>null</c>, is an empty string (""), or does not
        /// contain a time-of-day that coresponds to the pattern specified in <paramref name="format"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="s"/> parameter was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles styles, out Time time)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            format = NormalizeTimeFormat(format);

            if (!DateTime.TryParseExact(s, format, provider, styles, out DateTime dt))
            {
                time = default;
                return false;
            }

            time = TimeFromTimeSpan(dt.TimeOfDay);
            return true;
        }

        /// <summary>
        /// Converts the specified string representation of a time-of-day to its <see cref="Time"/> equivalent
        /// using the specified array of formats, culture-specific format information, and style.
        /// The format of the string representation must match at least one of the specified formats exactly.
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a time-of-day to convert.</param>
        /// <param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="s"/>.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of the enumeration values that indicates the style elements that
        /// can be present in <paramref name="s"/> for the parse operation to succeed.
        /// Note that only styles related to whitespace handling are applicable on a <see cref="Time"/>.
        /// A typical value to specify is <see cref="DateTimeStyles.None"/>.
        /// </param>
        /// <param name="time">
        /// When this method returns, contains the <see cref="Time"/> value equivalent to the time-of-day
        /// contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="MinValue"/>
        /// if the conversion failed. The conversion fails if either the <paramref name="s"/> or
        /// <paramref name="formats"/> parameter is <c>null</c>, <paramref name="s"/> or an element of
        /// <paramref name="formats"/> is an empty string (""), or the format of <paramref name="s"/> is not
        /// exactly as specified by at least one of the format patterns in <paramref name="formats"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="s"/> parameter was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="styles"/> contains invalid <see cref="DateTimeStyles"/> values.
        /// The only styles that are valid for a <see cref="Time"/> are:
        /// <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowLeadingWhite"/>,
        /// <see cref="DateTimeStyles.AllowTrailingWhite"/>, <see cref="DateTimeStyles.AllowInnerWhite"/>, and
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.  The other styles are invalid because they only apply
        /// when both a date and time are being parsed together.
        /// </exception>
        public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles, out Time time)
        {
            if (styles < DateTimeStyles.None || styles >= DateTimeStyles.NoCurrentDateDefault)
            {
                throw new ArgumentException(Strings.Argument_InvalidDateTimeStyles, nameof(styles));
            }

            Contract.EndContractBlock();

            for (int i = 0; i < formats.Length; i++)
            {
                formats[i] = NormalizeTimeFormat(formats[i]);
            }

            if (!DateTime.TryParseExact(s, formats, provider, styles, out DateTime dt))
            {
                time = default;
                return false;
            }

            time = TimeFromTimeSpan(dt.TimeOfDay);
            return true;
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="Time"/> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the same time of day;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="Time"/> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/> do not represent the same time of day;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Time left, Time right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether one specified <see cref="Time"/> is later than another specified
        /// <see cref="Time"/>, within the same day.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is later than <paramref name="right"/> within the same day;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public static bool operator >(Time left, Time right)
        {
            return left._ticks > right._ticks;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Time"/> is equal to or later than another specified
        /// <see cref="Time"/>, within the same day.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is equal to or later than <paramref name="right"/> within the same day;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public static bool operator >=(Time left, Time right)
        {
            return left._ticks >= right._ticks;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Time"/> is earlier than another specified
        /// <see cref="Time"/>, within the same day.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is earlier than <paramref name="right"/> within the same day;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public static bool operator <(Time left, Time right)
        {
            return left._ticks < right._ticks;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Time"/> is equal to or earlier than another specified
        /// <see cref="Time"/>, within the same day.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> is equal to or earlier than <paramref name="right"/> within the same day;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This operation considers all time values to be contained within the same day.  If you need to compare
        /// time values that cross midnight into a different day, use the <see cref="IsBetween"/> method instead.
        /// </remarks>
        public static bool operator <=(Time left, Time right)
        {
            return left._ticks <= right._ticks;
        }

        /// <summary>
        /// Casts a <see cref="TimeSpan"/> object to a <see cref="Time"/> by returning a new <see cref="Time"/> object
        /// that has the equivalent hours, minutes, seconds, and fractional seconds components.  This is useful when
        /// using APIs that express a time-of-day as the elapsed time since midnight, such that their values can be
        /// assigned to a variable having a <see cref="Time"/> type.  However, since it's possible for a <see cref="TimeSpan"/>
        /// to not be representable as a <see cref="Time"/>, the cast is required to be applied explicitly.
        /// Such unrepresentable values will throw an <see cref="InvalidCastException"/>.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> value representing the time elapsed since midnight,
        /// without regard to daylight saving time or other time zone transitions.</param>
        /// <returns>A newly constructed <see cref="Time"/> object with an equivalent value.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="timeSpan"/> is either negative, or greater than <c>23:59:59.9999999</c>, and thus cannot be
        /// cast to a <see cref="Time"/>.
        /// </exception>
        /// <remarks>
        /// Fundamentally, a time-of-day and an elapsed-time are two different concepts.  In previous versions
        /// of the .NET framework, the <see cref="Time"/> type did not exist, and thus several time-of-day
        /// values were represented by <see cref="TimeSpan"/> values erroneously.  For example, the
        /// <see cref="DateTime.TimeOfDay"/> property returns a value having a <see cref="TimeSpan"/> type.
        /// This cast operator allows those APIs to be used with <see cref="Time"/>, when explicitly cast.
        /// <para>
        /// Also note that the input <paramref name="timeSpan"/> might actually *not* accurately represent the
        /// "time elapsed since midnight" on days containing a daylight saving time transition or other time zone transition.
        /// </para>
        /// </remarks>
        public static explicit operator Time(TimeSpan timeSpan)
        {
            long ticks = timeSpan.Ticks;
            if (ticks < 0 || ticks >= TicksPerDay)
            {
                throw new InvalidCastException(Strings.InvalidCast_BadTimeSpan);
            }

            Contract.EndContractBlock();

            return new Time(ticks);
        }

        /// <summary>
        /// Implicitly casts a <see cref="Time"/> object to a <see cref="TimeSpan"/> by returning a new
        /// <see cref="TimeSpan"/> object that has the equivalent hours, minutes, seconds, and fractional seconds
        /// components.  This is useful when using APIs that express a time-of-day as the elapsed time since
        /// midnight, such that a <see cref="Time"/> type can be passed to a method expecting a
        /// <see cref="TimeSpan"/> parameter as a time-of-day.
        /// </summary>
        /// <param name="time">A <see cref="Time"/> value.</param>
        /// <returns>
        /// A newly constructed <see cref="TimeSpan"/> object representing the time elapsed since midnight, without
        /// regard to daylight saving time transitions.
        /// </returns>
        public static implicit operator TimeSpan(Time time)
        {
            return new TimeSpan(time.Ticks);
        }

        /// <summary>
        /// Converts the time from a 12-hour-clock representation to a 24-hour-clock representation.
        /// </summary>
        private static int Hours12To24(int hours12, Meridiem meridiem)
        {
            if (hours12 < 1 || hours12 > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(hours12), hours12, Strings.ArgumentOutOfRange_Hour12HF);
            }

            if (meridiem < Meridiem.AM || meridiem > Meridiem.PM)
            {
                throw new ArgumentOutOfRangeException(nameof(meridiem), meridiem, Strings.ArgumentOutOfRange_Meridiem);
            }

            Contract.EndContractBlock();

            return meridiem == Meridiem.AM
                ? (hours12 == 12 ? 0 : hours12)
                : (hours12 == 12 ? 12 : hours12 + 12);
        }

        /// <summary>
        /// Constructs a <see cref="Time"/> from a <see cref="TimeSpan"/> representing the time elapsed since
        /// midnight, without regard to daylight saving time transitions.
        /// </summary>
        private static Time TimeFromTimeSpan(TimeSpan timeSpan)
        {
            return new Time(timeSpan.Ticks);
        }

        /// <summary>
        /// Normalizes a format string that has standard or custom date/time formats,
        /// such that the formatted output can only contain a time-of-day when applied.
        /// </summary>
        /// <exception cref="FormatException">
        /// The format string contained a format specifier that is only applicable
        /// when a date would be part of the formatted output.
        /// </exception>
        private static string NormalizeTimeFormat(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return "T";
            }

            // standard formats
            if (format.Length == 1)
            {
                switch (format[0])
                {
                    // pass-through formats
                    case 'T':
                    case 't':
                        return format;

                    // ISO formats
                    case 'O':
                    case 'o':
                        return "HH:mm:ss.fffffff";
                    case 's':
                        return "HH:mm:ss";

                    default:
                        // All other standard DateTime formats are invalid for Time
                        throw new FormatException(Strings.Format_InvalidString);
                }
            }

            // custom format - test for date components or embedded standard date formats
            // except when escaped by preceding \ or enclosed in "" or '' quotes

            var filtered = EscapeCharRegex.Replace(format, String.Empty);
            if (InvalidFormatsRegex.IsMatch(filtered))
            {
                throw new FormatException(Strings.Format_InvalidString);
            }

            // custom format with embedded standard format(s) - ISO replacement
            format = ISOFormatRegex.Replace(format, m => m.Value == "%s" ? "HH:mm:ss" : "HH:mm:ss.fffffff");

            // pass through
            return format;
        }

        /// <summary>
        /// Gets a <see cref="XmlQualifiedName"/> that represents the <c>xs:time</c> type of the
        /// W3C XML Schema Definition (XSD) specification.
        /// </summary>
        /// <remarks>
        /// This is required to support the <see cref="XmlSchemaProviderAttribute"/> applied to this structure.
        /// </remarks>
        public static XmlQualifiedName GetSchema(object xs)
        {
            return new XmlQualifiedName("time", "http://www.w3.org/2001/XMLSchema");
        }

        /// <summary>
        /// Required by the <see cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <returns><c>null</c></returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates a <see cref="Time"/> object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
        /// <exception cref="FormatException">
        /// String was not recognized as a valid <see cref="Date"/>.
        /// </exception>
        /// <remarks>
        /// An <c>xs:time</c> uses the ISO-8601 extended time format, with up to seven decimal places of fractional
        /// seconds.  The equivalent .NET Framework format string is <c>HH:mm:ss.FFFFFFF</c>.
        /// </remarks>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var s = reader.NodeType == XmlNodeType.Element
                ? reader.ReadElementContentAsString()
                : reader.ReadContentAsString();

            if (!TryParseExact(s, "HH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture, DateTimeStyles.None, out Time t))
            {
                throw new FormatException(Strings.Format_BadTime);
            }

            this = t;
        }

        /// <summary>
        /// Converts a <see cref="Time"/> object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
        /// <remarks>
        /// An <c>xs:time</c> uses the ISO-8601 extended time format, with up to seven decimal places of fractional
        /// seconds.  The equivalent .NET Framework format string is <c>HH:mm:ss.FFFFFFF</c>.
        /// </remarks>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString("HH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture));
        }
    }
}
