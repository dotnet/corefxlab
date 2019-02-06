// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class PropertyValueConverterAttribute : Attribute
    {
        public Type PropertyType { get; protected set; }

        // todo: these must to be boxed to object (performance); we need to refactor to add converter interfaces with <T>

#if BUILDING_INBOX_LIBRARY
        public abstract object GetFromJson(ref Utf8JsonReader reader, Type propertyType);
        public abstract void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value);
#else
        //todo: ns20
        internal abstract object GetFromJson(ref Utf8JsonReader reader, Type propertyType);
        internal abstract void SetToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value);
#endif
    }
}
