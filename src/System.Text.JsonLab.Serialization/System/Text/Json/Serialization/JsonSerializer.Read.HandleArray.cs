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
    public static partial class JsonSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(
            JsonSerializerOptions options,
            ref ReadObjectState current,
            ref List<ReadObjectState> previous,
            ref int arrayIndex)
        {
            Type arrayType = current.PropertyInfo.PropertyType;
            if (!typeof(IEnumerable).IsAssignableFrom(arrayType) || (arrayType.IsArray && arrayType.GetArrayRank() > 1))
            {
                throw new InvalidOperationException($"todo: type {arrayType.ToString()} is not convertable to array.");
            }

            Debug.Assert(current.IsPropertyEnumerable());
            if (current.IsPropertyEnumerable())
            {
                if (current.EnumerableCreated)
                {
                    // A nested json array so push a new stack frame.
                    Type elementType = current.ClassInfo.ElementClassInfo.GetPolicyProperty().PropertyType;

                    SetPreviousState(ref previous, current, arrayIndex++);
                    current.Reset();
                    current.ClassInfo = options.GetOrAddClass(elementType);
                    current.PropertyInfo = current.ClassInfo.GetPolicyProperty();
                    current.PopStackOnEndArray = true;
                }
                else
                {
                    current.EnumerableCreated = true;
                }

                // If current property is already set (from a constructor, for example) leave as-is
                if (current.PropertyInfo.GetValueAsObject(current.ReturnValue, options) == null)
                {
                    // Create the enumerable.
                    object value = ReadObjectState.CreateEnumerableValue(ref current, options);
                    if (value != null)
                    {
                        if (current.ReturnValue != null)
                        {
                            current.PropertyInfo.SetValueAsObject(current.ReturnValue, value, options);
                        }
                        else
                        {
                            // Primitive arrays being returned without object
                            current.SetReturnValue(value, options);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleEndArray(
            JsonSerializerOptions options,
            ref ReadObjectState current,
            ref List<ReadObjectState> previous,
            ref int arrayIndex)
        {
            IEnumerable value = ReadObjectState.GetEnumerableValue(current);
            if (value == null)
            {
                // We added the items to the list property already.
                current.ResetProperty();
                return false;
            }

            bool lastFrame = (arrayIndex == 0);

            bool setPropertyDirectly;
            if (current.TempEnumerableValues != null)
            {
                EnumerableConverterAttribute converter = current.PropertyInfo.EnumerableConverter;
                if (converter == null)
                {
                    converter = current.ClassInfo.EnumerableConverter;
                }

                Type elementType = current.GetElementType();
                value = converter.CreateFromList(elementType, (IList)value);
                setPropertyDirectly = true;
            }
            else
            {
                setPropertyDirectly = false;
            }

            if (current.PopStackOnEndArray)
            {
                ReadObjectState previousFrame = default;
                GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                current = previousFrame;
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
                // else there must be an outer object, so we'll return false here.
            }

            ReadObjectState.SetReturnValue(value, options, ref current, setPropertyDirectly : setPropertyDirectly);
            return false;
        }
    }
}
