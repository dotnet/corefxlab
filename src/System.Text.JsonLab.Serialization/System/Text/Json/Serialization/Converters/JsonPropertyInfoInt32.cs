// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoInt32 : JsonPropertyInfo<int>, IJsonConverterInternal<int>
    {
        public JsonPropertyInfoInt32(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings) :
            base(classType, propertyType, propertyInfo, settings)
        { }

        public int FromJson(ref Utf8JsonReader reader)
        {
            return reader.GetInt32();
        }

        public void ToJson(ref Utf8JsonWriter writer, int value)
        {
            writer.WriteNumberValue(value);
        }

        public void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, int value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
