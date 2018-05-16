// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text
{
    /// <summary>
    /// Specifies how data is to be trimmed from a string.
    /// </summary>
    [Flags]
    internal enum TrimType
    {
        None = 0,

        /// <summary>
        /// Trim from the start of the string.
        /// </summary>
        Start = 1,

        /// <summary>
        /// Trim from the end of the string.
        /// </summary>
        End = 2,

        /// <summary>
        /// Trim from both the start and the end of the string.
        /// </summary>
        Both = 3
    }
}
