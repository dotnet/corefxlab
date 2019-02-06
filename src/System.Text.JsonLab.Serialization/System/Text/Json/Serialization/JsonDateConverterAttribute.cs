// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
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

#if BUILDING_INBOX_LIBRARY
        public override object GetFromJson(ref Utf8JsonReader reader, Type propertyType)
#else
        // todo: ns20
        internal override object GetFromJson(ref Utf8JsonReader reader, Type propertyType)
#endif
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            DateTime value;
            bool success = Utf8Parser.TryParse(span, out value, out int bytesConsumed, 'O') && span.Length == bytesConsumed;
            if (success)
            {
                return value;
            }

            throw new FormatException("todo: invalid DateTime");
        }

#if BUILDING_INBOX_LIBRARY
        public override void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value)
#else
        internal override void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value)
#endif
        {
            Debug.Assert(value is DateTime);

            byte[] stringValue = JsonConverter.s_utf8Encoding.GetBytes(((DateTime)value).ToString("O"));

            if (name.IsEmpty)
            {
                writer.WriteStringValue(stringValue);
            }
            else
            {
                writer.WriteString(name, stringValue);
            }
        }
    }
}
