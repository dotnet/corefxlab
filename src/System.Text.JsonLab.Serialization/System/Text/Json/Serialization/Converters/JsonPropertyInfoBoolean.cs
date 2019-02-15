// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoBoolean : JsonPropertyInfo<bool>, IJsonSerializerInternal<bool>
    {
        public JsonPropertyInfoBoolean(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public bool Read(ref Utf8JsonReader reader)
        {
            return reader.GetBoolean();
        }

        public void Write(ref Utf8JsonWriter writer, bool value)
        {
            writer.WriteBooleanValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, bool value)
        {
            writer.WriteBoolean(name, value);
        }
    }
}
