// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

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
                throw Exceptions.NotSupported_BadComparisonType(comparisonType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfStartIndexOutOfRange(int startIndex, int actualLength)
        {
            if ((uint)startIndex > (uint)actualLength)
            {
                throw Exceptions.ArgumentOutOfRange_StartIndex();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfStartIndexOrCountOutOfRange(int startIndex, int count, ParamName countParamName, int actualLength)
        {
            ThrowIfStartIndexOutOfRange(startIndex, actualLength);

            if ((uint)count > (uint)(actualLength - startIndex))
            {
                throw Exceptions.ArgumentOutOfRange_LengthOrCount(countParamName);
            }
        }
    }
}
