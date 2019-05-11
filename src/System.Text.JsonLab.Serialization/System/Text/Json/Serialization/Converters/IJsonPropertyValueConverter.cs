// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

namespace System.Text.JsonLab.Serialization.Converters
{
    /// <summary>
    /// Internal converter interface for well-known types that allows the converter instance to also be a JsonPropertyInfo<typeparamref name="TValue"/>.
    /// </summary>
    internal interface IJsonValueConverter<TValue>
    {
        bool TryRead(Type valueType, ref Utf8JsonReader reader, out TValue value);
        void Write(TValue value, ref Utf8JsonWriter writer);
        void Write(Span<byte> escapedPropertyName, TValue value, ref Utf8JsonWriter writer);
    }
}
