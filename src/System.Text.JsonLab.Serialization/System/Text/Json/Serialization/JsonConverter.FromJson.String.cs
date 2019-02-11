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
            var state = new JsonReaderState(settings.MaxDepth, settings.ReaderOptions);
            var reader = new Utf8JsonReader(jsonBytes, isFinalBlock: true, state);
            return FromJson(reader, returnType, settings);
        }
    }
}
