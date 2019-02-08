// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoString : JsonPropertyInfo<string>, IJsonConverterInternal<string>
    {
        public JsonPropertyInfoString(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings) :
            base(classType, propertyType, propertyInfo, settings)
        { }

        public string FromJson(ref Utf8JsonReader reader)
        {
            checked
            {
                return reader.GetString();
            }
        }

        public void ToJson(ref Utf8JsonWriter writer, string value)
        {
            writer.WriteStringValue(value);
        }

        public void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, string value)
        {
            writer.WriteString(name, value);
        }
    }
}
