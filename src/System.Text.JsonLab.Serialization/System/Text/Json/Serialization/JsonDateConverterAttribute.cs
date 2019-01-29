// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Converter for ISO 8601 <see cref="DateTime"/> with the format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK".
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    internal class JsonDateConverterAttribute : PropertyValueConverterAttribute
    {
        public JsonDateConverterAttribute()
        {
            PropertyType = typeof(DateTime);
        }

        public override bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out object value)
        {
            return DateConverter.TryGetFromJson(span, type, out value);
        }

        public override bool TrySetToJson(object value, out Span<byte> span)
        {
            return DateConverter.TrySetToJson(value, out span);
        }
    }
}
