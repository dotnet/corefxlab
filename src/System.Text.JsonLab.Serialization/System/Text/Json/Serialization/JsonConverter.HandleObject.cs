// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartObject(JsonConverterSettings options, Type returnType, ref JsonObjectState current, ref List<JsonObjectState> previous, ref int arrayIndex)
        {
            Type objType;

            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                // An array of objects either on the current property or on a list
                objType = current.PropertyInfo.ElementType;
                JsonPropertyInfo propInfo = current.PropertyInfo;
                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();
            }
            else if (current.PropertyInfo != null)
            {
                // Nested object
                objType = current.PropertyInfo.PropertyType;
                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();
            }
            else
            {
                // Initial object type
                objType = returnType;
            }

            current.ClassInfo = options.GetOrAddClass(objType);
            current.ReturnValue = current.ClassInfo.CreateObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleEndObject(ref JsonObjectState current, ref List<JsonObjectState> previous, ref int arrayIndex)
        {
            object value = current.ReturnValue;

            if (arrayIndex > 0)
            {
                JsonObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }
            else
            {
                current.Reset();
                current.ReturnValue = value;
                return true;
            }

            JsonObjectState.SetReturnValue(ref current, value, false);
            return false;
        }
    }
}
