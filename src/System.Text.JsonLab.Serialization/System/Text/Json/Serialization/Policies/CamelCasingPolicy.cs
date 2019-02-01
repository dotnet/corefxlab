// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    internal static class CamelCasingPolicy
    {
        public static string FromJson(string value)
        {
            if (value.Length == 0)
                return value;

            char firstChar = value[0];
            if (char.IsUpper(firstChar))
                return value;

            if (value.Length == 1)
                return value.ToUpperInvariant();

#if BUILDING_INBOX_LIBRARY
            return string.Create(value.Length, (firstChar, value), (chars, args) =>
            {
                chars[0] = char.ToUpperInvariant(args.firstChar);
                args.value.AsSpan(1).CopyTo(chars.Slice(1));
            });
#else
            if (value.Length < 256)
            {
                Span<char> span = stackalloc char[value.Length];
                span[0] = char.ToUpperInvariant(firstChar);
                value.AsSpan(1).CopyTo(span.Slice(1));
                unsafe
                {
                    fixed (char* ptr = span)
                    {
                        return new string(ptr, 0, span.Length);
                    }
                }
            }
            else
            {
                return char.ToUpperInvariant(firstChar) + value.Substring(1);
            }
#endif
        }

        public static string ToJson(string value)
        {
            if (value.Length == 0)
                return value;

            char firstChar = value[0];
            if (char.IsLower(firstChar))
                return value;

            if (value.Length == 1)
                return value.ToLowerInvariant();

#if BUILDING_INBOX_LIBRARY
            return string.Create(value.Length, (firstChar, value), (chars, args) =>
            {
                chars[0] = char.ToLowerInvariant(args.firstChar);
                args.value.AsSpan(1).CopyTo(chars.Slice(1));
            });
#else
            if (value.Length < 256)
            {
                Span<char> span = stackalloc char[value.Length];
                span[0] = char.ToLowerInvariant(firstChar);
                value.AsSpan(1).CopyTo(span.Slice(1));
                unsafe
                {
                    fixed (char* ptr = span)
                    {
                        return new string(ptr, 0, span.Length);
                    }
                }
            }
            else
            {
                return char.ToLowerInvariant(firstChar) + value.Substring(1);
            }
#endif
        }
    }
}
