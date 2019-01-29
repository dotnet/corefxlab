// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Policies;

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
                int value = reader.GetInt32();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(decimal))
            {
                decimal value = reader.GetDecimal();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(double))
            {
                double value = reader.GetDouble();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(long))
            {
                long value = reader.GetInt64();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(short))
            {
                int value = reader.GetInt32();
                return SetValueAsPrimitive(ref current, (short)value);
            }

            if (propType == typeof(float))
            {
                float value = reader.GetSingle();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(byte))
            {
                uint value = reader.GetUInt32();
                return SetValueAsPrimitive(ref current, (byte)value);
            }

            if (propType == typeof(uint))
            {
                uint value = reader.GetUInt32();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(ulong))
            {
                ulong value = reader.GetUInt64();
                return SetValueAsPrimitive(ref current, value);
            }

            if (propType == typeof(ushort))
            {
                uint value = reader.GetUInt32();
                return SetValueAsPrimitive(ref current, (ushort)value);
            }

            if (propType.IsEnum)
            {
                PropertyValueConverterAttribute converter = current.PropertyInfo.GetValueConverter();
                Debug.Assert(converter != null);

                JsonEnumConverterAttribute enumConverter = converter as JsonEnumConverterAttribute;

                if (enumConverter.TreatAsString)
                {
                    throw new InvalidOperationException($"todo: expected property {current.PropertyInfo.PropertyInfo.Name} to have JsonEnumConverterAttribute.TreatAsString=false");
                }

                long value = reader.GetInt64();

                Enum enumObject = (Enum)Enum.ToObject(current.PropertyInfo.PropertyInfo.PropertyType, value);

                return SetValueAsPrimitive<Enum>(ref current, enumObject);
            }

            //todo: add support for unsigned types (UInt, etc): https://github.com/dotnet/corefx/issues/33320
            // todo: use boxed object with converter
            throw new InvalidOperationException($"todo: type not known{propType.ToString()}");
        }
    }
}
