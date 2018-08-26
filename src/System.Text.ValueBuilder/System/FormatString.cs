// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    /// <summary>
    /// Simple wrapper for a format specifier string. We could potentially use this to indicate to C# that we have a direct
    /// string formatting method.
    /// </summary>
    /// <remarks>
    /// <see cref="IO.FormattingTextWriter"/> for examples of how this would be used.
    /// </remarks>
    public readonly ref struct FormatString
    {
        public ReadOnlySpan<char> Format { get; }

        public FormatString(ReadOnlySpan<char> format)
        {
            Format = format;
        }

        public int Length => Format.Length;

        public static implicit operator FormatString(string format) => new FormatString(format);
        public static implicit operator FormatString(ReadOnlySpan<char> format) => new FormatString(format);
    }
}
