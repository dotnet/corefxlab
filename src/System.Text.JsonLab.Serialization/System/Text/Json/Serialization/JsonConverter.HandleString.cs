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
                string value = reader.GetString();
                return SetValueAsPrimitive<string>(ref current, value);
            }

            if (propType == typeof(DateTime))
            {
                PropertyValueConverterAttribute converter = current.PropertyInfo.GetValueConverter();
                Debug.Assert(converter != null);

                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (converter.TryGetFromJson(span, typeof(DateTime), out object value))
                {
                    DateTime temp = (DateTime)value;
                    return SetValueAsPrimitive<DateTime>(ref current, temp);
                }
                else
                {
                    throw new InvalidOperationException("todo: invalid data");
                }
            }

            if (propType.IsEnum)
            {
                PropertyValueConverterAttribute converter = current.PropertyInfo.GetValueConverter();
                Debug.Assert(converter != null);

                JsonEnumConverterAttribute enumConverter = converter as JsonEnumConverterAttribute;

                if (!enumConverter.TreatAsString)
                {
                    throw new InvalidOperationException($"todo: expected property {current.PropertyInfo.PropertyInfo.Name} to have JsonEnumConverterAttribute.TreatAsString=true");
                }

                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                object value;
                if (converter.TryGetFromJson(span, current.PropertyInfo.PropertyType, out value))
                {
                    return SetValueAsPrimitive<Enum>(ref current, (Enum)value);
                }
                else
                {
                    throw new InvalidOperationException("todo: invalid data");
                }
            }

            if (propType == typeof(char))
            {
                string value = reader.GetString();
                if (value.Length != 1)
                {
                    throw new InvalidOperationException("todo: invalid data");
                }

                return SetValueAsPrimitive<char>(ref current, value[0]);
            }

            //todo: add extensibility
            throw new InvalidOperationException();
        }
    }
}
