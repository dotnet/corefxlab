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
  
        // Helper methods similar to System.ArrayExtension:
        // String helper methods, offering methods like String on Slice<char>:
        // TODO(joe): culture-sensitive comparisons.
        // TODO: should these move to string/text related assembly
        public static bool Contains(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }
            return str.IndexOf(value) >= 0;
        }

        public static bool EndsWith(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            int valueLength = value.Length;
            if (valueLength > str.Length)
            {
                return false;
            }

            int j = str.Length - valueLength;
            for (int i = 0; i < valueLength; i++)
            {
                char c = value[i];
                if (str[j] != c)
                {
                    return false;
                }
                j++;
            }
            return true;
        }

        public static int IndexOf(this ReadOnlySpan<char> str, string value)
        {
            return SpanExtensions.IndexOf(str, value.Slice());
        }

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

