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
    internal static class Exceptions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ArgumentOutOfRangeException ArgumentOutOfRange_LengthOrCount(ParamName paramName)
        {
            return new ArgumentOutOfRangeException(
                message: Strings.ArgumentOutOfRange_LengthOrCount,
                paramName: GetParamName(paramName));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ArgumentOutOfRangeException ArgumentOutOfRange_StartIndex()
        {
            return new ArgumentOutOfRangeException(
                message: Strings.ArgumentOutOfRange_StartIndex,
                paramName: "startIndex");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InvalidOperationException InvalidOperation_WouldCreateIllFormedUtf8String()
        {
            return new InvalidOperationException(
                message: Strings.InvalidOperation_WouldCreateIllFormedUtf8String);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static NotSupportedException NotSupported_BadComparisonType(StringComparison comparisonType)
        {
            return new NotSupportedException(
                message: String.Format(CultureInfo.InvariantCulture, Strings.NotSupported_BadComparisonType, comparisonType));
        }

        private static string GetParamName(ParamName paramName)
        {
            switch (paramName)
            {
                case ParamName.count:
                    return nameof(ParamName.count);

                case ParamName.length:
                    return nameof(ParamName.length);

                default:
                    return paramName.ToString();
            }
        }
    }
}
