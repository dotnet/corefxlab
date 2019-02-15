// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoInt16Nullable : JsonPropertyInfo<short?>, IJsonSerializerInternal<short?>
    {
        public JsonPropertyInfoInt16Nullable(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public short? Read(ref Utf8JsonReader reader)
        {
            checked
            {
                return (short?)reader.GetInt32();
            }
        }

        public void Write(ref Utf8JsonWriter writer, short? value)
        {
            writer.WriteNumberValue(value.Value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, short? value)
        {
            writer.WriteNumber(name, value.Value);
        }
    }
}
