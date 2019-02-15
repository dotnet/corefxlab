// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        public static byte[] Write<TValue>(TValue value, JsonSerializerOptions options = null)
        {
            return WriteInternal(value, typeof(TValue), options);
        }

        public static byte[] Write(object value, Type type = null, JsonSerializerOptions options = null)
        {
            VerifyValueAndType(value, type);
            return WriteInternal(value, type, options);
        }

        private static byte[] WriteInternal(object value, Type type, JsonSerializerOptions options)
        {
            if (options == null)
                options = s_defaultSettings;

            byte[] result;
            var state = new JsonWriterState(options.WriterOptions);

            using (var output = new ArrayBufferWriter<byte>(options.EffectiveBufferSize))
            {
                var writer = new Utf8JsonWriter(output, state);

                if (value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    if (type == null)
                        type = value.GetType();

                    WriteObjectState current = default;

                    JsonClassInfo classInfo = options.GetOrAddClass(type);
                    current.ClassInfo = classInfo;
                    current.CurrentValue = value;
                    if (classInfo.ClassType != ClassType.Object)
                    {
                        current.PropertyInfo = classInfo.GetPolicyProperty();
                    }

                    List<WriteObjectState> previous = null;
                    int arrayIndex = 0;

                    Write(ref writer, -1, options, ref current, ref previous, ref arrayIndex);
                }

                writer.Flush(isFinalBlock: true);
                result = output.WrittenMemory.ToArray();
            }

            return result;
        }
    }
}
