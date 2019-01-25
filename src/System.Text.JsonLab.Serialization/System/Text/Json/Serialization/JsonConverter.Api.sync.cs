// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static T FromJson<T>(string json, JsonConverterSettings options = null)
        {
            return (T)FromJson(json, typeof(T), options);
        }

        public static object FromJson(string json, Type returnType, JsonConverterSettings options = null)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            return FromJson((ReadOnlySpan<byte>)jsonBytes, returnType, options);
        }

        public static T FromJson<T>(this ReadOnlySpan<byte> json, JsonConverterSettings options = null)
        {
            return (T)FromJson(json, typeof(T), options);
        }

        public static object FromJson(this ReadOnlySpan<byte> json, Type returnType, JsonConverterSettings options = null)
        {
            if (options == null)
                options = s_default_options;

            JsonReaderState state = new JsonReaderState(options.DefaultBufferSize, options.ReaderOptions);
            Utf8JsonReader reader = new Utf8JsonReader(json, true, state);
            return FromJson(reader, returnType, options);
        }

        public static T FromJson<T>(this ReadOnlySequence<byte> json, JsonConverterSettings options = null)
        {
            return (T)FromJson(json, typeof(T), options);
        }

        public static object FromJson(this ReadOnlySequence<byte> json, Type returnType, JsonConverterSettings options = null)
        {
            if (options == null)
                options = s_default_options;

            JsonReaderState state = new JsonReaderState(options.DefaultBufferSize, options.ReaderOptions);
            Utf8JsonReader reader = new Utf8JsonReader(json, true, state);
            return FromJson(reader, returnType, options);
        }

        private static object FromJson(this Utf8JsonReader reader, Type returnType, JsonConverterSettings options = null)
        {
            if (options == null)
                options = s_default_options;

            JsonObjectState current = default;
            List<JsonObjectState> previous = null;
            int arrayIndex = 0;

            ReadData(ref reader, options, returnType, ref current, ref previous, ref arrayIndex);

            return current.ReturnValue;
        }
    }
}
