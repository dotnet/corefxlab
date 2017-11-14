﻿// Copyright (c) Microsoft. All rights reserved.
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
        public static Date GetDate(this DateTime dateTime)
        {
            return new Date((int) (dateTime.Ticks / TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Gets a <see cref="Time"/> value that represents the time component of the current
        /// <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> instance.</param>
        /// <returns>The <see cref="Time"/> value.</returns>
        public static Time GetTime(this DateTime dateTime)
        {
            return new Time(dateTime.TimeOfDay.Ticks);
        }
    }
}
