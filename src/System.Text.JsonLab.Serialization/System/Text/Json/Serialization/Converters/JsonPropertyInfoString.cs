// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoString : JsonPropertyInfo<string>, IJsonSerializerInternal<string>
    {
        public JsonPropertyInfoString(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public string Read(ref Utf8JsonReader reader)
        {
            return reader.GetString();
        }

        public void Write(ref Utf8JsonWriter writer, string value)
        {
            writer.WriteStringValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, string value)
        {
            writer.WriteString(name, value);
        }
    }
}
