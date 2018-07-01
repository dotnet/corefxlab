// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace System
{
    public static partial class Int32Polyfill
    {
        public static bool TryParse(ReadOnlySpan<char> s, out int value)
        {
#if NETCOREAPP2_1
            return int.TryParse(s, out value);
#else
            return int.TryParse(s.ToString(), out value);
#endif
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out int value)
        {
#if NETCOREAPP2_1
            return int.TryParse(s, style, provider, out value);
#else
            return int.TryParse(s.ToString(), style, provider, out value);
#endif
        } 

        public static int Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider = null)
        {
#if NETCOREAPP2_1
            return int.Parse(s, style, provider);
#else
            return int.Parse(s.ToString(), style, provider);
#endif
        }

        public static bool TryFormat(this int value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
#if NETCOREAPP2_1
            return value.TryFormat(destination, out charsWritten, format, provider);
#else
            var str = value.ToString(format.ToString(), provider);
            if (str.AsSpan().TryCopyTo(destination))
            {
                charsWritten = str.Length;
                return true;
            }
            charsWritten = 0;
            return false;
#endif
        }
    }
}
