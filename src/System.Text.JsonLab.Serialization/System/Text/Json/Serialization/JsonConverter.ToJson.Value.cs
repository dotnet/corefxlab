// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteValue(
            ref Utf8JsonWriter writer,
            JsonConverterSettings settings,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            JsonPropertyInfo propertyInfo = current.PropertyInfo;
            ClassType propertyClassType = propertyInfo.ClassType;
            if (propertyClassType == ClassType.Value)
            {
                propertyInfo.ToJson(ref current, ref writer);
                current.PropertyIndex++;
                current.ResetProperty();
            }
            else if (propertyClassType == ClassType.Object)
            {
                object obj = current.CurrentValue;
                object value = propertyInfo.GetValueAsObject(obj);
                current.PropertyIndex++;
                current.ResetProperty();

                if (value != null)
                {
                    SetPreviousState(ref previous, current, arrayIndex++);
                    current.Reset();
                    current.ClassInfo = settings.GetOrAddClass(propertyInfo.PropertyType);
                    current.PropertyInfo = propertyInfo;
                    current.CurrentValue = value;
                }
                else if (propertyInfo.SerializeNullValues)
                {
                    writer.WriteNull(propertyInfo.Name);
                }
            }
            else
            {
                Debug.Assert(propertyClassType == ClassType.Enumerable);

                JsonClassInfo elementClassInfo = propertyInfo.ElementClassInfo;
                if (elementClassInfo.ClassType == ClassType.Value)
                {
                    // Keep the same stack for primitives.
                    if (current.Enumerator == null)
                    {
                        IEnumerator enumerator = ((IEnumerable)propertyInfo.GetValueAsObject(current.CurrentValue)).GetEnumerator();
                        current.Enumerator = enumerator;
                        writer.WriteStartArray(propertyInfo.Name);
                    }

                    if (current.Enumerator.MoveNext())
                    {
                        propertyInfo.ToJson(ref current, ref writer);
                    }
                    else
                    {
                        writer.WriteEndArray();
                        current.PropertyIndex++;
                        current.ResetProperty();
                    }
                }
                else
                {
                    current.PropertyIndex++;
                    object obj = current.CurrentValue;

                    SetPreviousState(ref previous, current, arrayIndex++);
                    current.Reset();
                    current.ClassInfo = settings.GetOrAddClass(propertyInfo.PropertyType);
                    current.PropertyInfo = propertyInfo;
                    current.CurrentValue = obj;
                }
            }
        }
    }
}
