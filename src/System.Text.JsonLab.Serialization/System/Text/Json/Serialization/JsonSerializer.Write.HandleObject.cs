// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool WriteObject(
            JsonSerializerOptions options,
            ref Utf8JsonWriter writer,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
            ref int arrayIndex)
        {
            // Write the start.
            if (!current.StartObjectWritten)
            {
                if (current.PropertyInfo?.Name == null)
                {
                    writer.WriteStartObject();
                }
                else
                {
                    writer.WriteStartObject(current.PropertyInfo.Name);
                }
                current.StartObjectWritten = true;
            }

            // Determine if we are done enumerating properties.
            if (current.PropertyIndex != current.ClassInfo.PropertyCount)
            {
                HandleObject(options, ref writer, ref current, ref previous, ref arrayIndex);
                return false;
            }

            // We are done enumerating properties.
            writer.WriteEndObject();

            if (current.PopStackOnEndObject)
            {
                WriteObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }
            else
            {
                current.EndObject();
            }

            return true;
        }

        private static bool HandleObject(
                JsonSerializerOptions options,
                ref Utf8JsonWriter writer,
                ref WriteObjectState current,
                ref List<WriteObjectState> previous,
                ref int arrayIndex)
        {
            Debug.Assert(current.ClassInfo.ClassType == ClassType.Object);

            JsonPropertyInfo propertyInfo = current.ClassInfo.GetProperty(current.PropertyIndex);
            current.PropertyInfo = propertyInfo;

            ClassType propertyClassType = propertyInfo.ClassType;
            if (propertyClassType == ClassType.Value)
            {
                propertyInfo.Write(options, ref current, ref writer);
                current.NextProperty();
                return true;
            }

            // A property that returns an enumerator keeps the same stack frame.
            if (propertyClassType == ClassType.Enumerable)
            {
                bool endOfEnumerable = HandleEnumerable(propertyInfo.ElementClassInfo, options, ref writer, ref current, ref previous, ref arrayIndex);
                if (endOfEnumerable)
                {
                    current.NextProperty();
                }

                return endOfEnumerable;
            }

            // A property that returns an object requires a new stack frame.
            object value = propertyInfo.GetValueAsObject(current.CurrentValue, options);
            if (value != null)
            {
                JsonPropertyInfo previousPropertyInfo = current.PropertyInfo;

                current.NextProperty();

                JsonClassInfo nextClassInfo = options.GetOrAddClass(propertyInfo.PropertyType);
                AddNewStackFrame(nextClassInfo, value, ref current, ref previous, ref arrayIndex);

                // Set the PropertyInfo so we can obtain the property name in order to write it.
                current.PropertyInfo = previousPropertyInfo;
            }
            else if (!propertyInfo.IgnoreNullPropertyValueOnWrite(options))
            {
                writer.WriteNull(propertyInfo.Name);
            }

            return true;
        }
    }
}
