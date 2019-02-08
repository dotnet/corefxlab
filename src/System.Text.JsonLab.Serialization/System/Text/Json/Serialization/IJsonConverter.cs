// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Internal converter interface for well-known types. This may be used instead of Attributes in the near future.
    /// </summary>
    internal interface IJsonConverterInternal<TValue>
    {
        TValue FromJson(ref Utf8JsonReader reader);
        void ToJson(ref Utf8JsonWriter writer, TValue value);
        void ToJson(ref Utf8JsonWriter writer, ReadOnlySpan<byte> name, TValue value);
    }
}
