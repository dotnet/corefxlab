// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoInt64 : JsonPropertyInfo<long>, IJsonSerializerInternal<long>
    {
        public JsonPropertyInfoInt64(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public long Read(ref Utf8JsonReader reader)
        {
            return reader.GetInt64();
        }

        public void Write(ref Utf8JsonWriter writer, long value)
        {
            writer.WriteNumberValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, long value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
