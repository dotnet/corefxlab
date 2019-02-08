// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static string ToJsonString(object value, JsonConverterSettings settings = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Span<byte> jsonBytes = ToJsonInternal(value, settings);
            string stringJson = JsonReaderHelper.TranscodeHelper(jsonBytes);
            return stringJson;
        }

        public static Span<byte> ToJson(object value, JsonConverterSettings settings = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToJsonInternal(value, settings);
        }

        private static Span<byte> ToJsonInternal(object value, JsonConverterSettings settings)
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

            using (var output = new ArrayBufferWriter<byte>(settings.DefaultBufferSize))
            {
                var writer = new Utf8JsonWriter(output, state);

                ToJson(ref writer, -1, settings, ref current, ref previous, ref arrayIndex);

                writer.Flush(isFinalBlock: true);
                result = output.WrittenMemory.ToArray();
            }

            return result;
        }
    }
}
