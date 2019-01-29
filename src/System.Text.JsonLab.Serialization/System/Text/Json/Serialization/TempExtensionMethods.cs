// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    // Temporary extension methods until we get the API additions from Preview2
    internal static class TempExtensionMethods
    {
        public static int GetInt32(this Utf8JsonReader reader)
        {
            if (!reader.TryGetInt32Value(out int value))
            {
                throw new FormatException();
            }
            return value;
        }

        public static uint GetUInt32(this Utf8JsonReader reader)
        {
            if (!reader.TryGetInt64Value(out long value))
            {
                throw new FormatException();
            }
            return (uint)value;
        }

        public static long GetInt64(this Utf8JsonReader reader)
        {
            if (!reader.TryGetInt64Value(out long value))
            {
                throw new FormatException();
            }
            return value;
        }

        public static ulong GetUInt64(this Utf8JsonReader reader)
        {
            if (!reader.TryGetInt64Value(out long value))
            {
                throw new FormatException();
            }
            unchecked
            {
                return (ulong)value;
            }
        }

        public static string GetString(this Utf8JsonReader reader)
        {
            return reader.GetStringValue();
        }

        public static decimal GetDecimal(this Utf8JsonReader reader)
        {
            if (!reader.TryGetDecimalValue(out decimal value))
            {
                throw new FormatException();
            }
            return value;
        }

        public static double GetDouble(this Utf8JsonReader reader)
        {
            if (!reader.TryGetDoubleValue(out double value))
            {
                throw new FormatException();
            }
            return value;
        }

        public static float GetSingle(this Utf8JsonReader reader)
        {
            if (!reader.TryGetSingleValue(out float value))
            {
                throw new FormatException();
            }
            return value;
        }
    }
}
