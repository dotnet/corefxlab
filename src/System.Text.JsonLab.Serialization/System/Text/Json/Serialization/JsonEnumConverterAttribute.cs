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
        private static readonly IUtf8ValueConverter<Enum> s_enumConverterFalse = new EnumConverter(false);
        private static readonly IUtf8ValueConverter<Enum> s_enumConverterTrue = new EnumConverter(true);

        /// <summary>
        /// Determines whether the enum should be output as a <see cref="string"/>. By default, enums are <see cref="long"/>.
        /// </summary>
        public bool TreatAsString { get; set; }

        public JsonEnumConverterAttribute()
        {
            PropertyType = typeof(Enum);
        }

        public override IUtf8ValueConverter<Enum> GetConverter<Enum>()
        {
            if (TreatAsString)
            {
                return (IUtf8ValueConverter<Enum>)s_enumConverterTrue;
            }
            return (IUtf8ValueConverter<Enum>)s_enumConverterFalse;
        }
    }
}
