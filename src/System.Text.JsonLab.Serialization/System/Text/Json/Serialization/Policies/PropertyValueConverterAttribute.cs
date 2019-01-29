// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class PropertyValueConverterAttribute : Attribute
    {
        public Type PropertyType { get; protected set; }

        public abstract bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out object value);

        public abstract bool TrySetToJson(object value, out Span<byte> span);
    }
}
