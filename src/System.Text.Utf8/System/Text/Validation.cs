// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Utf8.Resources;

namespace System.Text
{
    [StackTraceHidden]
    internal static class Validation
    {
        public static void ThrowIfNotOrdinal(StringComparison comparisonType)
        {
            if (comparisonType != StringComparison.Ordinal)
            {
                ThrowIfNotOrdinalInternal(comparisonType);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowIfNotOrdinalInternal(StringComparison comparisonType)
        {
            throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, Strings.NotSupported_BadComparisonType, comparisonType));
        }
    }
}
