// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoUInt16 : JsonPropertyInfo<ushort>, IJsonSerializerInternal<ushort>
    {
        public JsonPropertyInfoUInt16(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public ushort Read(ref Utf8JsonReader reader)
        {
            checked
            {
                return (ushort)reader.GetUInt32();
            }
        }

        public void Write(ref Utf8JsonWriter writer, ushort value)
        {
            writer.WriteNumberValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, ushort value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
