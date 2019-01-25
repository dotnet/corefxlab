// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleNumber(ref Utf8JsonReader reader, JsonConverterSettings options, Type returnType, ref JsonObjectState current)
        {
            Debug.Assert(current.PropertyInfo != null);

            Type propType = JsonObjectState.GetElementType(current);

            // More common types are listed first

            if (propType == typeof(int))
            {
                if (!reader.TryGetInt32Value(out int value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(decimal))
            {
                if (!reader.TryGetDecimalValue(out decimal value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(double))
            {
                if (!reader.TryGetDoubleValue(out double value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(long))
            {
                if (!reader.TryGetInt64Value(out long value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(short))
            {
                if (!reader.TryGetInt32Value(out int value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, (short)value);
            }

            if (propType == typeof(float))
            {
                if (!reader.TryGetSingleValue(out float value))
                {
                    throw new FormatException();
                }
                return SetValueAsPrimitive(ref current, value);
            }

            //todo: add support for unsigned types (UInt, etc): https://github.com/dotnet/corefx/issues/33320
            // todo: use boxed object with converter
            throw new InvalidOperationException($"todo: type not known{propType.ToString()}");
        }
    }
}
