// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    /// <remarks>
    /// This is a pattern we can use to create stack based spans of <see cref="Variant"/>.
    /// </remarks>
    public readonly struct Variant2
    {
        public readonly Variant First;
        public readonly Variant Second;

        public Variant2(in Variant first, in Variant second)
        {
            First = first;
            Second = second;
        }
    }
}
