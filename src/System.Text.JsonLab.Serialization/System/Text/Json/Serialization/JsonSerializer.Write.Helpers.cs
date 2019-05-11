// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace System.Text.JsonLab.Serialization
{
    public static partial class JsonSerializer
    {
        private static void VerifyValueAndType(object value, Type type)
        {
            if (type == null)
            {
                if (value != null)
                {
                    throw new ArgumentNullException(nameof(type));
                }
            }
            else if (value != null)
            {
                if (!type.IsAssignableFrom(value.GetType()))
                {
                    throw new ArgumentException("todo - type must derive from value", nameof(type));
                }
            }
        }

        private static void WriteNull(
            ref JsonWriterState writerState,
            IBufferWriter<byte> bufferWriter)
        {
            Utf8JsonWriter writer = new Utf8JsonWriter(bufferWriter, writerState);
            writer.WriteNullValue();
            writer.Flush(true);
        }

        private static byte[] WriteCore(object value, Type type, JsonSerializerOptions options)
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
