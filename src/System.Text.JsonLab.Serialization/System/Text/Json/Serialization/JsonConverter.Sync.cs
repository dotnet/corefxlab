// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;

// Todo: this is a copy of class in the test project. We need to detemine best implementation for sync.

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
            var reader = new Utf8JsonReader(jsonBytes, true, state);
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
            var reader = new Utf8JsonReader(json, true, state);
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
            var reader = new Utf8JsonReader(json, true, state);
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

        public static string ToJsonString(object value, JsonConverterSettings settings = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Span<byte> jsonBytes = ToJsonInternal(value, settings);
#if BUILDING_INBOX_LIBRARY
            string stringJson = s_utf8Encoding.GetString(jsonBytes);
#else
            string stringJson;
            if (jsonBytes.IsEmpty)
            {
                stringJson = string.Empty;
            }
            unsafe
            {
                fixed (byte* bytePtr = jsonBytes)
                {
                    stringJson = s_utf8Encoding.GetString(bytePtr, jsonBytes.Length);
                }
            }
#endif
            return stringJson;
        }

        public static Span<byte> ToJson(object value, JsonConverterSettings settings = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToJsonInternal(value, settings);
        }

        private static Span<byte> ToJsonInternal(object value, JsonConverterSettings settings = null)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            ToJsonObjectState current = default;
            Type initialType = value.GetType();

            JsonClassInfo classInfo = settings.GetOrAddClass(initialType);
            current.ClassInfo = classInfo;
            current.CurrentValue = value;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            List<ToJsonObjectState> previous = null;
            int arrayIndex = 0;

            var state = new JsonWriterState(settings.WriterOptions);

            byte[] result;

            using (var output = new ArrayBufferWriter(settings.DefaultBufferSize))
            {
                var writer = new Utf8JsonWriter(output, state);

                ToJson(ref writer, settings, ref current, ref previous, ref arrayIndex);

                int byteCount;
                checked
                {
                    byteCount = (int)writer.BytesWritten;
                }

                Span<byte> json = output.GetSpan().Slice(0, byteCount);
                result = json.ToArray();
            }

            return result;
        }

        // Coming soon:
        
        //public static ValueTask ToJsonAsync<T>(this PipeWriter writer, T value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default);
        //public static ValueTask ToJsonAsync(this PipeWriter writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default);

        //public static ValueTask ToJsonAsync<T>(this Stream writer, T value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default);
        //public static ValueTask ToJsonAsync(this Stream writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default);
    }
}
