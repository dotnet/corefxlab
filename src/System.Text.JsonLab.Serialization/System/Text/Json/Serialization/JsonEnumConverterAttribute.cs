// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Converter for <see cref="Enum"/> types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class JsonEnumConverterAttribute : PropertyValueConverterAttribute
    {
        /// <summary>
        /// Determines how an enum should be converted to\from a <see cref="string"/>. By default, enums are <see cref="long"/>.
        /// </summary>
        public bool TreatAsString { get; set; }

        public JsonEnumConverterAttribute(bool treatAsString) : this()
        {
            TreatAsString = treatAsString;
        }

        public JsonEnumConverterAttribute()
        {
            PropertyType = typeof(Enum);
        }

        public override bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out object value)
        {
            bool success = EnumConverter.TryGetFromJson(span, type, out Enum temp);
            value = temp;
            return success;
        }

        public override bool TrySetToJson(object value, out Span<byte> span)
        {
            return EnumConverter.TrySetToJson((Enum)value, out span);
        }
    }
}
