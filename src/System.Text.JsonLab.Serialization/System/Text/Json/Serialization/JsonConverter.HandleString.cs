// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleString(ref Utf8JsonReader reader, JsonConverterSettings options, Type returnType, ref JsonObjectState current)
        {
            Debug.Assert(current.PropertyInfo != null);

            Type propType = JsonObjectState.GetElementType(current);

            if (propType == typeof(string))
            {
                string value = reader.GetStringValue();
                return SetValueAsPrimitive<string>(ref current, value);
            }

            if (propType == typeof(DateTime))
            {
                IUtf8ValueConverter<DateTime> converter = current.PropertyInfo.GetValueConverter<DateTime>();
                Debug.Assert(converter != null);

                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (converter.TryGetFromJson(span, typeof(DateTime), out DateTime value))
                {
                    return SetValueAsPrimitive<DateTime>(ref current, value);
                }
                else
                {
                    throw new InvalidOperationException("todo: invalid data");
                }
            }

            if (propType.IsEnum)
            {
                IUtf8ValueConverter<Enum> converter = current.PropertyInfo.GetValueConverter<Enum>();
                Debug.Assert(converter != null);

                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (converter.TryGetFromJson(span, current.PropertyInfo.PropertyType, out Enum value))
                {
                    return SetValueAsPrimitive<Enum>(ref current, value);
                }
                else
                {
                    throw new InvalidOperationException("todo: invalid data");
                }
            }
            else
            {
                //todo: add extensibility
                throw new InvalidOperationException();
            }
        }
    }
}
