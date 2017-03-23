// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A collection of convenient span helpers, exposed as extension methods.
    /// </summary>
    public static partial class SpanExtensionsLabs
    {
        public static void CopyTo<T>(this T[] array, Span<T> span)
        {
            new Span<T>(array).CopyTo(span);
        }
    }
}

