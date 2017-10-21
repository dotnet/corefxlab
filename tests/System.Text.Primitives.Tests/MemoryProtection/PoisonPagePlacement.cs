// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.MemoryProtection
{
    /// <summary>
    /// Dictates where the poison page should be placed.
    /// </summary>
    public enum PoisonPagePlacement
    {
        /// <summary>
        /// The poison page should be placed before the span.
        /// Attempting to access the memory page immediately before the
        /// span will result in an AV.
        /// </summary>
        BeforeSpan,

        /// <summary>
        /// The poison page should be placed after the span.
        /// Attempting to access the memory page immediately following the
        /// span will result in an AV.
        /// </summary>
        AfterSpan
    }
}
