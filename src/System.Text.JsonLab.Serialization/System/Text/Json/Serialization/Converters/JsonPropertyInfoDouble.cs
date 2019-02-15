// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.Json.Serialization.Converters
{
    internal class JsonPropertyInfoDouble : JsonPropertyInfo<double>, IJsonSerializerInternal<double>
    {
        public JsonPropertyInfoDouble(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options) :
            base(classType, propertyType, propertyInfo, options)
        { }

        public double Read(ref Utf8JsonReader reader)
        {
            return reader.GetDouble();
        }

        public void Write(ref Utf8JsonWriter writer, double value)
        {
            writer.WriteNumberValue(value);
        }

        public void Write(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, double value)
        {
            writer.WriteNumber(name, value);
        }
    }
}
