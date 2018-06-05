// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    public static partial class Int32Polyfill
    {
        public static bool TryParse(ReadOnlySpan<char> buffer, out int value)
        {
#if NETCOREAPP2_1
            return int.TryParse(buffer, out value);
#else
            return int.TryParse(buffer.ToString(), out value);
#endif
        }
    }
}
