// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="TimeZoneInfo"/>.
    /// </summary>
    public static class TimeZoneInfoExtensions
    {
        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date, time,
        /// and offset from Coordinated Universal Time (UTC) in this time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="DateTimeOffset"/> for the instance time zone.</returns>
        public static DateTimeOffset GetCurrentDateTimeOffset(this TimeZoneInfo timeZoneInfo)
        {
            return DateTimeOffsetExtensions.NowInTimeZone(timeZoneInfo);
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time in this time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="DateTime"/> for the instance time zone.</returns>
        public static DateTime GetCurrentDateTime(this TimeZoneInfo timeZoneInfo)
        {
            return DateTimeExtensions.NowInTimeZone(timeZoneInfo);
        }

        /// <summary>
        /// Gets a <see cref="Date"/> object that is set to the current date in this time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="Date"/> for the instance time zone.</returns>
        public static Date GetCurrentDate(this TimeZoneInfo timeZoneInfo)
        {
            return Date.TodayInTimeZone(timeZoneInfo);
        }

        /// <summary>
        /// Gets a <see cref="TimeOfDay"/> object that is set to the current time in this time zone.
        /// </summary>
        /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> instance.</param>
        /// <returns>The current <see cref="TimeOfDay"/> for the instance time zone.</returns>
        public static TimeOfDay GetCurrentTime(this TimeZoneInfo timeZoneInfo)
        {
            return TimeOfDay.NowInTimeZone(timeZoneInfo);
        }
    }
}
