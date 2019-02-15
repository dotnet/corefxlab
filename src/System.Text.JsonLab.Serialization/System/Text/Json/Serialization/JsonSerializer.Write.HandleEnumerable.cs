// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool WriteEnumerable(
            JsonSerializerOptions options,
            ref Utf8JsonWriter writer,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
            ref int arrayIndex)
        {
            return HandleEnumerable(current.ClassInfo.ElementClassInfo, options, ref writer, ref current, ref previous, ref arrayIndex);
        }

        private static bool HandleEnumerable(
            JsonClassInfo elementClassInfo,
            JsonSerializerOptions options,
            ref Utf8JsonWriter writer,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
            ref int arrayIndex)
        {
            Debug.Assert(current.PropertyInfo.ClassType == ClassType.Enumerable);

            JsonPropertyInfo propertyInfo = current.PropertyInfo;

            if (current.Enumerator == null)
            {
                if (propertyInfo.Name == null)
                {
                    writer.WriteStartArray();
                }
                else
                {
                    writer.WriteStartArray(propertyInfo.Name);
                }

                IEnumerable enumerable = (IEnumerable)propertyInfo.GetValueAsObject(current.CurrentValue, options);

                if (enumerable != null)
                {
                    current.Enumerator = enumerable.GetEnumerator();
                }
            }

            if (current.Enumerator != null && current.Enumerator.MoveNext())
            {
                if (elementClassInfo.ClassType == ClassType.Value)
                {
                    propertyInfo.Write(options, ref current, ref writer);
                }
                else
                {
                    // An object or another enumerator requires a new stack frame
                    JsonClassInfo nextClassInfo = propertyInfo.ElementClassInfo;
                    object nextValue = current.Enumerator.Current;
                    AddNewStackFrame(nextClassInfo, nextValue, ref current, ref previous, ref arrayIndex);
                }

                return false;
            }

            // We are done enumerating.
            writer.WriteEndArray();

            if (current.PopStackOnEndArray)
            {
                WriteObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }
            else
            {
                current.EndArray();
            }

            return true;
        }
    }
}
