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
        public static bool StartsWith(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (str[i] != value[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

