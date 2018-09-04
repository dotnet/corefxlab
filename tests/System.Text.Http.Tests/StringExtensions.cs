// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Http.Parser.Tests
{
    public static class StringExtensions
    {
        public static string EscapeNonPrintable(this string s)
        {
            var ellipsis = s.Length > 128
                ? "..."
                : string.Empty;
            return s.Substring(0, Math.Min(128, s.Length))
                .Replace("\r", @"\x0D")
                .Replace("\n", @"\x0A")
                .Replace("\0", @"\x00")
                + ellipsis;
        }
    }
}