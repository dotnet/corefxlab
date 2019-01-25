// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(JsonConverterSettings options, Type returnType, ref JsonObjectState current, ref List<JsonObjectState> previous, ref int arrayIndex)
        {
            Type arrayType = current.PropertyInfo.PropertyType;
            if (!typeof(IEnumerable).IsAssignableFrom(arrayType) || (arrayType.IsArray && arrayType.GetArrayRank() > 1))
            {
                throw new InvalidOperationException($"todo: type {arrayType.ToString()} is not convertable to array.");
            }

            Debug.Assert(current.PropertyInfo.ElementType != null);

            // If a nested array then push a new stack frame.
            Type propType = current.PropertyInfo.PropertyType;
            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                Type elementType = current.PropertyInfo.ElementType;

                SetPreviousState(ref previous, current, arrayIndex++);
                current.Reset();
                current.ClassInfo = options.GetOrAddClass(elementType);
                current.PropertyInfo = current.ClassInfo.GetPolicyProperty();
                current.PopStackOnEndArray = true;

                object value = JsonObjectState.CreateEnumerableValue(ref current, propType, options, arrayType);
                if (value != null)
                {
                    current.SetReturnValue(value, true);
                }
            }
            else
            {
                // If current property is already set (from a constructor, for example) leave as-is
                if (current.PropertyInfo.GetValueAsObject(current.ReturnValue) == null)
                {
                    // Avoid creating a stack frame for the first array.
                    object value = JsonObjectState.CreateEnumerableValue(ref current, propType, options, arrayType);
                    if (value != null)
                    {
                        if (current.ReturnValue != null)
                        {
                            JsonObjectState.SetReturnValue(ref current, value, true);
                            //current.propertyInfo.SetValueAsObject(current.obj, value);
                        }
                        else
                        {
                            // Primitive arrays being returned without object
                            current.SetReturnValue(value, true);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleEndArray(ref JsonObjectState current, ref List<JsonObjectState> previous, ref int arrayIndex)
        {
            IEnumerable value = JsonObjectState.GetEnumerableValue(current);
            if (value == null)
            {
                // We added the items to the list property already.
                current.ResetProperty();
                return false;
            }

            bool lastFrame = (arrayIndex == 0);

            Type elementType = current.PropertyInfo.ElementType;
            if (current.PopStackOnEndArray)
            {
                JsonObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
            }

            bool ignorePropertyEnumerable;
            if (current.TempEnumerableValues != null)
            {
                EnumerableConverterAttribute converter = current.PropertyInfo.EnumerableConverter;
                value = converter.CreateFromList(elementType, (IList)value);
                ignorePropertyEnumerable = true;
            }
            else
            {
                ignorePropertyEnumerable = false;
            }

            if (lastFrame)
            {
                if (current.ReturnValue == null)
                {
                    // Returning a converted list or object.
                    current.Reset();
                    current.ReturnValue = value;
                    return true;
                }
                else if (current.IsEnumerable())
                {
                    // Returning a non-converted list.
                    return true;
                }
            }

            JsonObjectState.SetReturnValue(ref current, value, true, ignorePropertyEnumerable);
            return false;
        }
    }
}
