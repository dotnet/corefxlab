// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

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