// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="DateTimeOffset"/>.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Gets a <see cref="Date"/> value that represents the date component of the current
        /// <see cref="DateTimeOffset"/> object.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>The <see cref="Date"/> value.</returns>
        public static Date GetDate(this DateTimeOffset dateTimeOffset)
        {
            return new Date((int)(dateTimeOffset.DateTime.Ticks / TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Gets a <see cref="TimeOfDay"/> value that represents the time component of the current
        /// <see cref="DateTimeOffset"/> object.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>The <see cref="TimeOfDay"/> value.</returns>
        public static TimeOfDay GetTimeOfDay(this DateTimeOffset dateTimeOffset)
        {
            return new TimeOfDay(dateTimeOffset.TimeOfDay.Ticks);
        }

        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date, time,
        /// and offset from Coordinated Universal Time (UTC) in the specified time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="DateTimeOffset"/> for the specified time zone.</returns>
        public static DateTimeOffset NowInTimeZone(TimeZoneInfo timeZoneInfo)
        {
            // TODO: Propose placing this method directly in the System.DateTimeOffset struct

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            return TimeZoneInfo.ConvertTime(utcNow, timeZoneInfo);
        }

        public static DateTimeOffset AddYears(this DateTimeOffset dateTimeOffset, int years, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddYears(years), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddYears(this DateTimeOffset dateTimeOffset, int years, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddYears(years), timeZone, resolver);
        }

        public static DateTimeOffset AddMonths(this DateTimeOffset dateTimeOffset, int months, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddMonths(months), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddMonths(this DateTimeOffset dateTimeOffset, int months, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddMonths(months), timeZone, resolver);
        }

        public static DateTimeOffset AddDays(this DateTimeOffset dateTimeOffset, int days, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddDays(days), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddDays(this DateTimeOffset dateTimeOffset, int days, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTimeOffset, dt => dt.AddDays(days), timeZone, resolver);
        }

        public static DateTimeOffset AddHours(this DateTimeOffset dateTimeOffset, double hours, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(TimeSpan.FromHours(hours), timeZone);
        }

        public static DateTimeOffset AddMinutes(this DateTimeOffset dateTimeOffset, double minutes, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(TimeSpan.FromMinutes(minutes), timeZone);
        }

        public static DateTimeOffset AddSeconds(this DateTimeOffset dateTimeOffset, double seconds, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(TimeSpan.FromSeconds(seconds), timeZone);
        }

        public static DateTimeOffset AddMilliseconds(this DateTimeOffset dateTimeOffset, double milliseconds, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(TimeSpan.FromMilliseconds(milliseconds), timeZone);
        }

        public static DateTimeOffset AddTicks(this DateTimeOffset dateTimeOffset, long ticks, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(TimeSpan.FromTicks(ticks), timeZone);
        }

        public static DateTimeOffset Subtract(this DateTimeOffset dateTimeOffset, TimeSpan timeSpan, TimeZoneInfo timeZone)
        {
            return dateTimeOffset.Add(timeSpan.Negate(), timeZone);
        }

        public static DateTimeOffset Add(this DateTimeOffset dateTimeOffset, TimeSpan timeSpan, TimeZoneInfo timeZone)
        {
            var t = dateTimeOffset.Add(timeSpan);
            return TimeZoneInfo.ConvertTime(t, timeZone);
        }
        
        private static DateTimeOffset AddByDate(DateTimeOffset dateTimeOffset, Func<DateTime, DateTime> operation, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            var dto = TimeZoneInfo.ConvertTime(dateTimeOffset, timeZone);
            var dt = operation.Invoke(dto.DateTime);
            return resolver.Invoke(dt, timeZone);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent long date string
        /// representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>
        /// A string that contains the long date string representation of the current <see cref="DateTimeOffset"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTimeOffset"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.LongDatePattern" /> property associated with the current thread culture.
        /// </remarks>
        public static string ToLongDateString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent long date string
        /// representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>
        /// A string that contains the long date string representation of the current <see cref="DateTimeOffset"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTimeOffset"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.LongDatePattern" /> property associated with the invariant culture.
        /// </remarks>
        public static string ToLongDateStringInvariant(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongDatePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent short date string
        /// representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>
        /// A string that contains the short date string representation of the current <see cref="DateTimeOffset"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTimeOffset"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.ShortDatePattern" /> property associated with the current thread culture.
        /// </remarks>
        public static string ToShortDateString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent short date string
        /// representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>
        /// A string that contains the short date string representation of the current <see cref="DateTimeOffset"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTimeOffset"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.ShortDatePattern" /> property associated with the invariant culture.
        /// </remarks>
        public static string ToShortDateStringInvariant(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent
        /// long time string representation.
        /// </summary>
        /// <returns>A string that contains the long time string representation of the
        /// current <see cref="DateTimeOffset"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTimeOffset"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.LongTimePattern" />
        /// property associated with the current thread culture.</remarks>
        public static string ToLongTimeString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent
        /// long time string representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>A string that contains the long time string representation of the
        /// current <see cref="DateTimeOffset"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTimeOffset"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.LongTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public static string ToLongTimeStringInvariant(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent
        /// short time string representation.
        /// </summary>
        /// <returns>A string that contains the short time string representation of the
        /// current <see cref="DateTimeOffset"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTimeOffset"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.ShortTimePattern" />
        /// property associated with the current thread culture.</remarks>
        public static string ToShortTimeString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeOffset"/> object to its equivalent
        /// short time string representation.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>A string that contains the short time string representation of the
        /// current <see cref="DateTimeOffset"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTimeOffset"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.ShortTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public static string ToShortTimeStringInvariant(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortTimePattern, CultureInfo.InvariantCulture);
        }
    }
}
