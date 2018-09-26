// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Gets a <see cref="Time"/> value that represents the time component of the current
        /// <see cref="DateTimeOffset"/> object.
        /// </summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
        /// <returns>The <see cref="Time"/> value.</returns>
        public static Time GetTime(this DateTimeOffset dateTimeOffset)
        {
            return new Time(dateTimeOffset.TimeOfDay.Ticks);
        }
    }
}
