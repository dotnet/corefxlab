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

            return string.Create(value.Length, (firstChar, value), (chars, args) =>
            {
                chars[0] = char.ToUpperInvariant(args.firstChar);
                args.value.AsSpan(1).CopyTo(chars.Slice(1));
            });
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

            return string.Create(value.Length, (firstChar, value), (chars, args) =>
            {
                chars[0] = char.ToLowerInvariant(args.firstChar);
                args.value.AsSpan(1).CopyTo(chars.Slice(1));
            });
        }
    }
}
