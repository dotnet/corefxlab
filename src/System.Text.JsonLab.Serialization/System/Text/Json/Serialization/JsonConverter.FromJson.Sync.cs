// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static T FromJson<T>(string json, JsonConverterSettings settings = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            return (T)FromJsonInternal(json, typeof(T), settings);
        }

        public static object FromJson(string json, Type returnType, JsonConverterSettings settings = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            return FromJsonInternal(json, returnType, settings);
        }

        private static object FromJsonInternal(string json, Type returnType, JsonConverterSettings settings = null)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            // todo: use an array pool here for smaller requests to avoid the alloc. Also doc the API that UTF8 is preferred for perf. 
            byte[] jsonBytes = s_utf8Encoding.GetBytes(json);
            var state = new JsonReaderState(settings.DefaultBufferSize, settings.ReaderOptions);
            var reader = new Utf8JsonReader(jsonBytes, isFinalBlock: true, state);
            return FromJsonInternal(reader, returnType, settings);
        }

        public static T FromJson<T>(this ReadOnlySpan<byte> json, JsonConverterSettings settings = null)
        {
            return (T)FromJson(json, typeof(T), settings);
        }

        public static object FromJson(this ReadOnlySpan<byte> json, Type returnType, JsonConverterSettings settings = null)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            var state = new JsonReaderState(settings.DefaultBufferSize, settings.ReaderOptions);
            var reader = new Utf8JsonReader(json, isFinalBlock: true, state);
            return FromJsonInternal(reader, returnType, settings);
        }

        public static T FromJson<T>(this ReadOnlySequence<byte> json, JsonConverterSettings settings = null)
        {
            return (T)FromJson(json, typeof(T), settings);
        }

        public static object FromJson(this ReadOnlySequence<byte> json, Type returnType, JsonConverterSettings settings = null)
        {
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            if (settings == null)
                settings = s_DefaultSettings;

            var state = new JsonReaderState(settings.DefaultBufferSize, settings.ReaderOptions);
            var reader = new Utf8JsonReader(json, isFinalBlock: true, state);
            return FromJsonInternal(reader, returnType, settings);
        }

        private static object FromJsonInternal(this Utf8JsonReader reader, Type returnType, JsonConverterSettings settings = null)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            List<FromJsonObjectState> previous = null;
            int arrayIndex = 0;

            FromJsonObjectState current = default;
            JsonClassInfo classInfo = settings.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            FromJson(ref reader, settings, returnType, ref current, ref previous, ref arrayIndex);

            return current.ReturnValue;
        }
    }
}
