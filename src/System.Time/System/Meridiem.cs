// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System
{
    /// <summary>
    /// Provides an enumeration of AM or PM to support 12-hour clock values in the <see cref="TimeOfDay"/> type.
    /// </summary>
    /// <remarks>
    /// Though commonly used in English, these abbreviations derive from Latin.
    /// AM is an abbreviation for "Ante Meridiem", meaning "before mid-day".
    /// PM is an abbreviation for "Post Meridiem", meaning "after mid-day".
    /// </remarks>
    public enum Meridiem
    {
        AM,
        PM
    }
}