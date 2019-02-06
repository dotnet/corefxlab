// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoChar : JsonPropertyInfo<char>, IJsonConverterInternal<char>
    {
        public JsonPropertyInfoChar(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings) :
            base(classType, propertyType, propertyInfo, settings)
        { }

        public char FromJson(ref Utf8JsonReader reader)
        {
            return reader.GetString()[0];
        }

        public void ToJson(ref Utf8JsonWriter writer, char value)
        {
            writer.WriteStringValue(value.ToString());
        }

        public void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, char value)
        {
            writer.WriteString(name, value.ToString());
        }
    }
}
