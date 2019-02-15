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
        private static void HandleStartObject(JsonSerializerOptions options, Type returnType, ref ReadObjectState current, ref List<ReadObjectState> previous, ref int arrayIndex)
        {
            Type objType;

            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                // An array of objects either on the current property or on a list
                objType = current.GetElementType();
                JsonPropertyInfo propInfo = current.PropertyInfo;
                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();

                current.ClassInfo = options.GetOrAddClass(objType);
                current.ReturnValue = current.ClassInfo.CreateObject();
            }
            else if (current.PropertyInfo != null)
            {
                // Nested object
                objType = current.PropertyInfo.PropertyType;
                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();

                current.ClassInfo = options.GetOrAddClass(objType);
                current.ReturnValue = current.ClassInfo.CreateObject();
            }
            else
            {
                // Initial object type
                objType = returnType;

                Debug.Assert(current.ClassInfo != null);
                current.ReturnValue = current.ClassInfo.CreateObject();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleEndObject(JsonSerializerOptions options, ref ReadObjectState current, ref List<ReadObjectState> previous, ref int arrayIndex)
        {
            object value = current.ReturnValue;

            if (arrayIndex > 0)
            {
                ReadObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }
            else
            {
                current.Reset();
                current.ReturnValue = value;
                return true;
            }

            ReadObjectState.SetReturnValue(value, options, ref current);
            return false;
        }
    }
}
