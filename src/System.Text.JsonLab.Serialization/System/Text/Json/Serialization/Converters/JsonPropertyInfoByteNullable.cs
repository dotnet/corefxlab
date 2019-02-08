// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoByteNullable : JsonPropertyInfo<byte?>, IJsonConverterInternal<byte?>
    {
        public JsonPropertyInfoByteNullable(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings) :
            base(classType, propertyType, propertyInfo, settings)
        { }

        public byte? FromJson(ref Utf8JsonReader reader)
        {
            checked
            {
                return (byte?)reader.GetInt32();
            }
        }

        public void ToJson(ref Utf8JsonWriter writer, byte? value)
        {
            writer.WriteNumberValue(value.Value);
        }

        public void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, byte? value)
        {
            writer.WriteNumber(name, value.Value);
        }
    }
}
