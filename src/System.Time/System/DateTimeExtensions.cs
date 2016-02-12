// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets a <see cref="Date"/> value that represents the date component of the current
        /// <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>The <see cref="Date"/> value.</returns>
        public static Date GetDate(this DateTime dateTime)
        {
            return new Date((int) (dateTime.Ticks / TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Gets a <see cref="TimeOfDay"/> value that represents the time component of the current
        /// <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>The <see cref="TimeOfDay"/> value.</returns>
        public static TimeOfDay GetTimeOfDay(this DateTime dateTime)
        {
            return new TimeOfDay(dateTime.TimeOfDay.Ticks);
        }
        
        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time in the specified time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="DateTime"/> for the specified time zone.</returns>
        public static DateTime NowInTimeZone(TimeZoneInfo timeZoneInfo)
        {
            // TODO: Propose placing this method directly in the System.DateTime struct
            
            DateTime utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTime(utcNow, timeZoneInfo);
        }

        public static DateTimeOffset AddYears(this DateTime dateTime, int years, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTime, dt => dt.AddYears(years), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddYears(this DateTime dateTime, int years, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTime, dt => dt.AddYears(years), timeZone, resolver);
        }

        public static DateTimeOffset AddMonths(this DateTime dateTime, int months, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTime, dt => dt.AddMonths(months), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddMonths(this DateTime dateTime, int months, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTime, dt => dt.AddMonths(months), timeZone, resolver);
        }

        public static DateTimeOffset AddDays(this DateTime dateTime, int days, TimeZoneInfo timeZone)
        {
            return AddByDate(dateTime, dt => dt.AddDays(days), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddDays(this DateTime dateTime, int days, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return AddByDate(dateTime, dt => dt.AddDays(days), timeZone, resolver);
        }

        public static DateTimeOffset AddHours(this DateTime dateTime, double hours, TimeZoneInfo timeZone)
        {
            return dateTime.Add(TimeSpan.FromHours(hours), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddHours(this DateTime dateTime, double hours, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(TimeSpan.FromHours(hours), timeZone, resolver);
        }

        public static DateTimeOffset AddMinutes(this DateTime dateTime, double minutes, TimeZoneInfo timeZone)
        {
            return dateTime.Add(TimeSpan.FromMinutes(minutes), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddMinutes(this DateTime dateTime, double minutes, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(TimeSpan.FromMinutes(minutes), timeZone, resolver);
        }

        public static DateTimeOffset AddSeconds(this DateTime dateTime, double seconds, TimeZoneInfo timeZone)
        {
            return dateTime.Add(TimeSpan.FromSeconds(seconds), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddSeconds(this DateTime dateTime, double seconds, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(TimeSpan.FromSeconds(seconds), timeZone, resolver);
        }

        public static DateTimeOffset AddMilliseconds(this DateTime dateTime, double milliseconds, TimeZoneInfo timeZone)
        {
            return dateTime.Add(TimeSpan.FromMilliseconds(milliseconds), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddMilliseconds(this DateTime dateTime, double milliseconds, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(TimeSpan.FromMilliseconds(milliseconds), timeZone, resolver);
        }

        public static DateTimeOffset AddTicks(this DateTime dateTime, long ticks, TimeZoneInfo timeZone)
        {
            return dateTime.Add(TimeSpan.FromTicks(ticks), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset AddTicks(this DateTime dateTime, long ticks, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(TimeSpan.FromTicks(ticks), timeZone, resolver);
        }

        public static DateTimeOffset Add(this DateTime dateTime, TimeSpan timeSpan, TimeZoneInfo timeZone)
        {
            return dateTime.Add(timeSpan, timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset Subtract(this DateTime dateTime, TimeSpan timeSpan, TimeZoneInfo timeZone)
        {
            return dateTime.Add(timeSpan.Negate(), timeZone, TimeZoneOffsetResolvers.Default);
        }

        public static DateTimeOffset Subtract(this DateTime dateTime, TimeSpan timeSpan, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            return dateTime.Add(timeSpan.Negate(), timeZone, resolver);
        }

        public static DateTimeOffset Add(this DateTime dateTime, TimeSpan timeSpan, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            var dto = resolver.Invoke(dateTime, timeZone);
            var result = dto.Add(timeSpan);
            return TimeZoneInfo.ConvertTime(result, timeZone);
        }
        
        private static DateTimeOffset AddByDate(DateTime dateTime, Func<DateTime, DateTime> operation, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            if (dateTime.Kind != DateTimeKind.Unspecified)
            {
                dateTime = TimeZoneInfo.ConvertTime(dateTime, timeZone);
            }

            var result = operation.Invoke(dateTime);
            return resolver.Invoke(result, timeZone);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent long date string
        /// representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>
        /// A string that contains the long date string representation of the current <see cref="DateTime"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTime"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.LongDatePattern" /> property associated with the invariant culture.
        /// </remarks>
        public static string ToLongDateStringInvariant(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongDatePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent short date string
        /// representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>
        /// A string that contains the short date string representation of the current <see cref="DateTime"/> object.
        /// </returns>
        /// <remarks>
        /// The value of the current <see cref="DateTime"/> object is formatted using the pattern defined by the
        /// <see cref="DateTimeFormatInfo.ShortDatePattern" /> property associated with the invariant culture.
        /// </remarks>
        public static string ToShortDateStringInvariant(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent
        /// long time string representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>A string that contains the long time string representation of the
        /// current <see cref="DateTime"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTime"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.LongTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public static string ToLongTimeStringInvariant(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent
        /// short time string representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>A string that contains the short time string representation of the
        /// current <see cref="DateTime"/> object.</returns>
        /// <remarks>The value of the current <see cref="DateTime"/> object is formatted
        /// using the pattern defined by the <see cref="DateTimeFormatInfo.ShortTimePattern" />
        /// property associated with the invariant culture.</remarks>
        public static string ToShortTimeStringInvariant(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortTimePattern, CultureInfo.InvariantCulture);
        }
    }
}
