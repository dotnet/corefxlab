// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        // todo: use a common instance across project (once corefxlab done)
        internal static readonly UTF8Encoding s_utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private static readonly JsonSerializerOptions s_defaultSettings = new JsonSerializerOptions();

        private static object Read(
            Utf8JsonReader reader,
            Type returnType,
            JsonSerializerOptions options)
        {
            if (options == null)
                options = s_defaultSettings;

            List<ReadObjectState> previous = null;
            int arrayIndex = 0;

            ReadObjectState current = default;
            JsonClassInfo classInfo = options.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            Read(returnType, options, ref reader, ref current, ref previous, ref arrayIndex);

            return current.ReturnValue;
        }

        // todo: refactor this method to split by ClassType(Enumerable, Object, or Value) like Write()
        private static void Read(
            Type returnType, //todo: we shouldn't need to pass this since we can now get it from current.ClassInfo.PropertyPolicy
            JsonSerializerOptions options,
            ref Utf8JsonReader reader,
            ref ReadObjectState current,
            ref List<ReadObjectState> previous,
            ref int arrayIndex)
        {
            while (reader.Read())
            {
                JsonTokenType tokenType = reader.TokenType;
                if (tokenType >= JsonTokenType.String && tokenType <= JsonTokenType.False)
                {
                    Debug.Assert(tokenType == JsonTokenType.String || tokenType == JsonTokenType.Number || tokenType == JsonTokenType.True || tokenType == JsonTokenType.False);
                    if (HandleValue(options, ref reader, ref current))
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
                    current.PropertyInfo = current.ClassInfo.GetProperty(propertyName, current.PropertyIndex);
                    current.PropertyIndex++;
                }
                else if (tokenType == JsonTokenType.StartObject)
                {
                    HandleStartObject(options, returnType, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndObject)
                {
                    if (HandleEndObject(options, ref current, ref previous, ref arrayIndex))
                    {
                        // todo: verify bytes read == bytes processed.
                        return;
                    }
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    HandleStartArray(options, ref current, ref previous, ref arrayIndex);
                }
                else if (tokenType == JsonTokenType.EndArray)
                {
                    if (HandleEndArray(options, ref current, ref previous, ref arrayIndex))
                    {
                        // todo: verify bytes read == bytes processed.
                        return;
                    }
                }
                else if (tokenType == JsonTokenType.Null)
                {
                    if (HandleNull(ref current, options))
                    {
                        return;
                    }
                }
            }

            return;
        }
    }
}
