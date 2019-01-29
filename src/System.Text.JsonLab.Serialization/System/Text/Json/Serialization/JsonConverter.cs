// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        // todo: use a common instance across project (once corefxlab done)
        internal static readonly UTF8Encoding s_utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private static readonly JsonConverterSettings s_DefaultSettings = new JsonConverterSettings();

        private static bool ReadData(
            ref Utf8JsonReader reader,
            JsonConverterSettings options,
            Type returnType,
            ref JsonObjectState current,
            ref List<JsonObjectState> previous,
            ref int arrayIndex)
        {
            while (reader.Read())
            {
                JsonTokenType tokenType = reader.TokenType;
                if (tokenType == JsonTokenType.String)
                {
                    CheckForNonObjectReturn(ref current, options, returnType);
                    if (HandleString(ref reader, options, returnType, ref current))
                    {
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.Number)
                {
                    CheckForNonObjectReturn(ref current, options, returnType);
                    if (HandleNumber(ref reader, options, returnType, ref current))
                    {
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.PropertyName)
                {
                    Debug.Assert(current.ReturnValue != default);
                    Debug.Assert(current.ClassInfo != default);

                    ReadOnlySpan<byte> propertyName = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                    current.PropertyInfo = current.ClassInfo.GetProperty(current.ReturnValue.GetType(), propertyName, current.PropertyIndex);
                    current.PropertyIndex++;
                }
                else if (tokenType == JsonTokenType.StartObject)
                {
                    HandleStartObject(options, returnType, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndObject)
                {
                    if (HandleEndObject(ref current, ref previous, ref arrayIndex))
                    {
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    CheckForNonObjectReturn(ref current, options, returnType);
                    HandleStartArray(options, returnType, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndArray)
                {
                    if (HandleEndArray(ref current, ref previous, ref arrayIndex))
                    {
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.True)
                {
                    CheckForNonObjectReturn(ref current, options, returnType);
                    if (SetValueAsPrimitive(ref current, true))
                    {
                        return true;
                    }

                }
                else if (tokenType == JsonTokenType.False)
                {
                    CheckForNonObjectReturn(ref current, options, returnType);
                    if (SetValueAsPrimitive(ref current, false))
                    {
                        return true;
                    }
                }
                else
                {
                    // todo: use boxed object with converter
                    throw new InvalidOperationException();
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SetValueAsPrimitive<T>(ref JsonObjectState current, in T value)
        {
            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                JsonObjectState.SetReturnValue(ref current, value, current.IsPropertyEnumerable());
                return false;
            }

            if (current.ReturnValue == null)
            {
                current.ReturnValue = value;
                return true;
            }

            ((JsonPropertyInfo<T>)current.PropertyInfo).Set(current.ReturnValue, value);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckForNonObjectReturn(ref JsonObjectState current, JsonConverterSettings options, Type returnType)
        {
            if (current.ClassInfo == null)
            {
                Debug.Assert(current.PropertyInfo == null);
                JsonClassInfo returnClassInfo = options.GetOrAddClass(returnType);
                current.ClassInfo = returnClassInfo;
                current.PropertyInfo = returnClassInfo.GetPolicyProperty();
            }
        }

        private static void GetPreviousState(ref List<JsonObjectState> previous, ref JsonObjectState state, int index)
        {
            if (previous == null)
            {
                previous = new List<JsonObjectState>();
            }

            if (index >= previous.Count)
            {
                Debug.Assert(index == previous.Count);
                state = new JsonObjectState();
                previous.Add(state);
            }
            else
            {
                state = previous[index];
            }
        }

        private static void SetPreviousState(ref List<JsonObjectState> previous, in JsonObjectState state, int index)
        {
            if (previous == null)
            {
                previous = new List<JsonObjectState>();
            }

            if (index >= previous.Count)
            {
                Debug.Assert(index == previous.Count);
                previous.Add(state);
            }
            else
            {
                previous[index] = state;
            }
        }
    }
}
