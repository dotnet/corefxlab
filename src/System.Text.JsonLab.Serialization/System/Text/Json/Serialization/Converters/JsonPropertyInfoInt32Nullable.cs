// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoInt32Nullable : JsonPropertyInfo<int?>, IJsonSerializerInternal<int?>
    {
        public JsonPropertyInfoInt32Nullable(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public int? Read(ref Utf8JsonReader reader)
        {
            return reader.GetInt32();
        }

        public void Write(ref Utf8JsonWriter writer, int? value)
        {
            writer.WriteNumberValue(value.Value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, int? value)
        {
            writer.WriteNumber(name, value.Value);
        }
    }
}
