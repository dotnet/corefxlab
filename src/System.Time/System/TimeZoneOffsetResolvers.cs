// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System
{
    public delegate DateTimeOffset TimeZoneOffsetResolver(DateTime dateTime, TimeZoneInfo timeZone);

    public static class TimeZoneOffsetResolvers
    {
        public static DateTimeOffset Default(DateTime dt, TimeZoneInfo timeZone)
        {
            if (dt.Kind != DateTimeKind.Unspecified)
            {
                var dto = new DateTimeOffset(dt);
                return TimeZoneInfo.ConvertTime(dto, timeZone);
            }

            if (timeZone.IsAmbiguousTime(dt))
            {
                var earlierOffset = timeZone.GetUtcOffset(dt.AddDays(-1));
                return new DateTimeOffset(dt, earlierOffset);
            }

            if (timeZone.IsInvalidTime(dt))
            {
                var earlierOffset = timeZone.GetUtcOffset(dt.AddDays(-1));
                var laterOffset = timeZone.GetUtcOffset(dt.AddDays(1));
                var transitionGap = laterOffset - earlierOffset;
                return new DateTimeOffset(dt.Add(transitionGap), laterOffset);
            }

            return new DateTimeOffset(dt, timeZone.GetUtcOffset(dt));
        }

        // TODO: include other kinds of resolvers
    }
}
