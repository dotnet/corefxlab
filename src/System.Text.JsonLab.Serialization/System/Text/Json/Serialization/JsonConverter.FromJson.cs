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

        private static bool FromJson(
            ref Utf8JsonReader reader,
            JsonConverterSettings settings,
            Type returnType,
            ref FromJsonObjectState current,
            ref List<FromJsonObjectState> previous,
            ref int arrayIndex)
        {
            while (reader.Read())
            {
                JsonTokenType tokenType = reader.TokenType;
                if (tokenType == JsonTokenType.String || tokenType == JsonTokenType.Number || tokenType == JsonTokenType.True || tokenType == JsonTokenType.False)
                {
                    if (HandleValue(ref reader, settings, returnType, ref current))
                    {
                        // todo: verify bytes read == bytes processed.
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
                    HandleStartObject(settings, returnType, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndObject)
                {
                    if (HandleEndObject(ref current, ref previous, ref arrayIndex))
                    {
                        // todo: verify bytes read == bytes processed.
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    HandleStartArray(settings, returnType, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndArray)
                {
                    if (HandleEndArray(ref current, ref previous, ref arrayIndex))
                    {
                        // todo: verify bytes read == bytes processed.
                        return true;
                    }
                }
                else if (tokenType == JsonTokenType.Null)
                {
                    if (HandleNull(ref current))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool SetValueAsPrimitive<T>(ref FromJsonObjectState current, in T value)
        {
            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                FromJsonObjectState.SetReturnValue(ref current, value);
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
        private static bool SetValueAsObject(ref FromJsonObjectState current, in object value)
        {
            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                FromJsonObjectState.SetReturnValue(ref current, value);
                return false;
            }

            if (current.ReturnValue == null)
            {
                current.ReturnValue = value;
                return true;
            }

           current.PropertyInfo.SetValueAsObject(current.ReturnValue, value);
           return false;
        }
    }
}
