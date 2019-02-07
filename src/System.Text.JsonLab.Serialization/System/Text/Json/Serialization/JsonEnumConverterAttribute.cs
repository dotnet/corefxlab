// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
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

        public JsonEnumConverterAttribute(bool treatAsString = default)
        {
            TreatAsString = treatAsString;
            PropertyType = typeof(Enum);
        }

#if BUILDING_INBOX_LIBRARY
        public override object GetFromJson(ref Utf8JsonReader reader, Type propertyType)
#else
        // todo: ns20
        internal override object GetFromJson(ref Utf8JsonReader reader, Type propertyType)
#endif
        {
            if (TreatAsString)
            {
#if !BUILDING_INBOX_LIBRARY 
                throw new NotImplementedException("TODO: EnumConverter is not yet supported on .NET Standard 2.0."); 
#else
                string enumString = reader.GetString();
                if (Enum.TryParse(propertyType, enumString, out object objValue))
                {
                    return (Enum)objValue;
                }

                throw new InvalidOperationException($"todo:could not convert value to string-based Enum {propertyType}");
#endif
            }

            ulong value = reader.GetUInt64();
            Enum enumObject = (Enum)Enum.ToObject(propertyType, value);
            return enumObject;
        }

#if BUILDING_INBOX_LIBRARY
        public override void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value)
#else
        // todo: ns20
        internal override void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value)
#endif
        {
            Debug.Assert(value is Enum);

            if (TreatAsString)
            {
                string enumString = value.ToString();
                if (name.IsEmpty)
                {
                    writer.WriteStringValue(enumString);
                }
                else
                {
                    writer.WriteString(name, enumString);
                }
            }
            else
            {
                Type underlyingType = Enum.GetUnderlyingType(value.GetType());

                if (underlyingType == typeof(ulong))
                {
                    // Keep +sign
                    ulong ulongValue = Convert.ToUInt64(value);
                    if (name.IsEmpty)
                    {
                        writer.WriteNumberValue(ulongValue);
                    }
                    else
                    {
                        writer.WriteNumber(name, ulongValue);
                    }
                }
                else
                {
                    // long can hold the signed\unsigned values of other integer types
                    long longValue = Convert.ToInt64(value);
                    if (name.IsEmpty)
                    {
                        writer.WriteNumberValue(longValue);
                    }
                    else
                    {
                        writer.WriteNumber(name, longValue);
                    }
                }
            }
        }
    }
}
