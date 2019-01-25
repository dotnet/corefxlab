// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    internal static class CamelCasingPolicy
    {
        public static string FromJson(string value)
        {
            if (value.Length == 0 || char.IsUpper(value[0]))
                return value;

            if (value.Length == 1)
                return value.ToUpperInvariant();

            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static string ToJson(string value)
        {
            if (value.Length == 0 || char.IsLower(value[0]))
                return value;

            if (value.Length == 1)
                return value.ToLowerInvariant();

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
