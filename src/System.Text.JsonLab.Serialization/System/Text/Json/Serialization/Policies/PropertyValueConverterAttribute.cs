// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class PropertyValueConverterAttribute : Attribute
    {
        public Type PropertyType { get; protected set; }

#if BUILDING_INBOX_LIBRARY
        // todo: these must not be boxed to object (performance); we need to refactor to add converter interfaces with <T>
        public abstract object GetRead(ref Utf8JsonReader reader, Type propertyType);
        public abstract void SetWrite(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value);
#else
        // Temporary for corefxlab
        internal abstract object GetRead(ref Utf8JsonReader reader, Type propertyType);
        internal abstract void SetWrite(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, object value);
#endif
    }
}
