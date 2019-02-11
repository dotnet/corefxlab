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

        private static void FromJson(
            ref JsonReaderState readerState,
            Type returnType,
            bool isFinalBlock,
            byte[] buffer,
            int bytesToRead,
            JsonConverterSettings settings,
            ref FromJsonObjectState current,
            ref List<FromJsonObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonReader reader = new Utf8JsonReader(new ReadOnlySpan<byte>(buffer, 0, bytesToRead), isFinalBlock, readerState);

            FromJson(
                ref reader,
                settings,
                returnType,
                ref current,
                ref previous,
                ref arrayIndex);

            readerState = reader.CurrentState;
        }

        private static void FromJson(
            ref JsonReaderState readerState,
            Type returnType,
            bool isFinalBlock,
            ReadOnlySequence<byte> buffer,
            JsonConverterSettings settings,
            ref FromJsonObjectState current,
            ref List<FromJsonObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonReader reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);

            FromJson(
                ref reader,
                settings,
                returnType,
                ref current,
                ref previous,
                ref arrayIndex);

            readerState = reader.CurrentState;
        }

        private static object FromJson(
            Utf8JsonReader reader,
            Type returnType,
            JsonConverterSettings settings)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            List<FromJsonObjectState> previous = null;
            int arrayIndex = 0;

            FromJsonObjectState current = default;
            JsonClassInfo classInfo = settings.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            FromJson(ref reader, settings, returnType, ref current, ref previous, ref arrayIndex);

            return current.ReturnValue;
        }

        // todo: refactor this method to split by ClassType(Enumerable, Object, or Value) like ToJson()
        private static void FromJson(
            ref Utf8JsonReader reader,
            JsonConverterSettings settings,
            Type returnType, //todo: we shouldn't need to pass this since we can now get it from current.ClassInfo.PropertyPolicy
            ref FromJsonObjectState current,
            ref List<FromJsonObjectState> previous,
            ref int arrayIndex)
        {
            while (reader.Read())
            {
                JsonTokenType tokenType = reader.TokenType;
                if (tokenType >= JsonTokenType.String && tokenType <= JsonTokenType.False)
                {
                    Debug.Assert(tokenType == JsonTokenType.String || tokenType == JsonTokenType.Number || tokenType == JsonTokenType.True || tokenType == JsonTokenType.False);
                    if (HandleValue(ref reader, settings, returnType, ref current))
                    {
                        // todo: verify bytes read == bytes processed.
                        return;
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
                        return;
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
                        return;
                    }
                }
                else if (tokenType == JsonTokenType.Null)
                {
                    if (HandleNull(ref current))
                    {
                        return;
                    }
                }
            }

            return;
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
