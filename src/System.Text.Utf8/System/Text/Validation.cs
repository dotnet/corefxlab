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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfStartIndexOrCountOutOfRange(int startIndex, int count, int actualLength)
        {
            if ((uint)startIndex > (uint)actualLength)
            {
                ThrowIfStartIndexOrCountOutOfRangeInternal_StartIndex();
            }

            if ((uint)count > (uint)(actualLength - startIndex))
            {
                ThrowIfStartIndexOrCountOutOfRangeInternal_Count();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowIfStartIndexOrCountOutOfRangeInternal_Count()
        {
            throw new ArgumentOutOfRangeException(paramName: "count");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowIfStartIndexOrCountOutOfRangeInternal_StartIndex()
        {
            throw new ArgumentOutOfRangeException(paramName: "startIndex");
        }
    }
}
