// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Utf8
{
    internal static class Utf16LittleEndianStringExtensions
    {
        // TODO: Naming it Equals causes picking up wrong overload when compiling (Equals(object))
        public static bool EqualsUtf8String(this string left, Utf8String right)
        {
            return right.Equals(left);
        }
    }
}
