// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoDecimal : JsonPropertyInfo<decimal>, IJsonConverterInternal<decimal>
    {
        public JsonPropertyInfoDecimal(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings) :
            base(classType, propertyType, propertyInfo, settings)
        { }

        public decimal FromJson(ref Utf8JsonReader reader)
        {
            return reader.GetDecimal();
        }

        public void ToJson(ref Utf8JsonWriter writer, decimal value)
        {
            writer.WriteNumberValue(value);
        }

        public void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, decimal value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
