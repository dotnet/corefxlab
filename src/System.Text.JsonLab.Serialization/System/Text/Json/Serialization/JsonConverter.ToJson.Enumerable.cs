// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool WriteEnumerable(
            ref Utf8JsonWriter writer,
            JsonConverterSettings settings,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            JsonPropertyInfo propertyInfo = current.PropertyInfo;

            if (!current.StartArrayWritten)
            {
                if (propertyInfo.Name == null)
                {
                    writer.WriteStartArray();
                }
                else
                {
                    writer.WriteStartArray(propertyInfo.Name);
                }

                current.StartArrayWritten = true;
                current.Enumerator = ((IEnumerable)propertyInfo.GetValueAsObject(current.CurrentValue)).GetEnumerator();
            }

            if (current.ClassInfo.ElementClassInfo.ClassType == ClassType.Value)
            {
                if (current.Enumerator.MoveNext())
                {
                    propertyInfo.ToJson(ref current, ref writer);
                    return false;
                }
            }
            else if (current.Enumerator.MoveNext())
            {
                object obj = current.Enumerator.Current;
                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();
                current.ClassInfo = propertyInfo.ElementClassInfo;
                current.CurrentValue = obj;

                if (current.ClassInfo.ClassType == ClassType.Enumerable)
                {
                    current.PropertyInfo = current.ClassInfo.GetPolicyProperty();
                }
                
                return false;
            }

            if (arrayIndex > 0)
            {
                ToJsonObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }
            else
            {
                current.ResetProperty();
            }

            writer.WriteEndArray();
            return false;
        }
    }
}
