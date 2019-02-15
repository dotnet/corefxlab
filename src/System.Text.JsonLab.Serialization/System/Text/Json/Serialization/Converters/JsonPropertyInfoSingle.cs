// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoSingle : JsonPropertyInfo<float>, IJsonSerializerInternal<float>
    {
        public JsonPropertyInfoSingle(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public float Read(ref Utf8JsonReader reader)
        {
            return reader.GetSingle();
        }

        public void Write(ref Utf8JsonWriter writer, float value)
        {
            writer.WriteNumberValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, float value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
