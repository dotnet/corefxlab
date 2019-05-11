﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Text.Json;

namespace System.Text.JsonLab.Serialization.Converters
{
    internal class JsonPropertyInfoDoubleNullable : JsonPropertyInfo<double?>, IJsonValueConverter<double?>
    {
        public JsonPropertyInfoDoubleNullable(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public bool TryRead(Type valueType, ref Utf8JsonReader reader, out double? value)
        {
            if (reader.TokenType != JsonTokenType.Number)
            {
                value = default;
                return false;
            }

            value = reader.GetDouble();
            return true;
        }

        public void Write(double? value, ref Utf8JsonWriter writer)
        {
            writer.WriteNumberValue(value.Value);
        }

        public void Write(Span<byte> escapedPropertyName, double? value, ref Utf8JsonWriter writer)
        {
            writer.WriteNumber(escapedPropertyName, value.Value);
        }
    }
}
