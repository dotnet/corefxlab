// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        public static Date Date(this DateTime dateTime)
        {
            return new Date((int) (dateTime.Ticks / TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Gets a <see cref="TimeOfDay"/> value that represents the time component of the current
        /// <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>The <see cref="TimeOfDay"/> value.</returns>
        public static TimeOfDay TimeOfDay(this DateTime dateTime)
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
            var dto = dateTime.Kind == DateTimeKind.Unspecified
                ? resolver.Invoke(dateTime, timeZone)
                : new DateTimeOffset(dateTime);

            var result = dto.Add(timeSpan);
            return TimeZoneInfo.ConvertTime(result, timeZone);
        }
        
        private static DateTimeOffset AddByDate(DateTime dateTime, Func<DateTime, DateTime> operation, TimeZoneInfo timeZone, TimeZoneOffsetResolver resolver)
        {
            if (dateTime.Kind != DateTimeKind.Unspecified)
            {
                dateTime = TimeZoneInfo.ConvertTime(dateTime, timeZone);
            }

            var dt = operation.Invoke(dateTime);
            return resolver.Invoke(dt, timeZone);
        }
    }
}
