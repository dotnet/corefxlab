// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoUInt32 : JsonPropertyInfo<uint>, IJsonSerializerInternal<uint>
    {
        public JsonPropertyInfoUInt32(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public uint Read(ref Utf8JsonReader reader)
        {
            return reader.GetUInt32();
        }

        public void Write(ref Utf8JsonWriter writer, uint value)
        {
            writer.WriteNumberValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, uint value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
