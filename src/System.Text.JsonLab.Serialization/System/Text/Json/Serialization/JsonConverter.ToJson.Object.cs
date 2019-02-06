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
        private static bool WriteObject(
            ref Utf8JsonWriter writer,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            if (!current.StartObjectWritten)
            {
                if (current.PropertyInfo?.Name == null)
                {
                    writer.WriteStartObject();
                }
                else
                {
                    writer.WriteStartObject(current.PropertyInfo.Name);
                }
                current.StartObjectWritten = true;
            }

            if (current.PropertyIndex == current.ClassInfo.PropertyCount)
            {
                writer.WriteEndObject();

                if (arrayIndex > 0)
                {
                    ToJsonObjectState previousFrame = default;
                    GetPreviousState(ref previous, ref previousFrame, --arrayIndex);
                    current = previousFrame;
                }

                return false;
            }

            JsonPropertyInfo propertyInfo = current.ClassInfo.GetProperty(current.PropertyIndex);
            current.PropertyInfo = propertyInfo;

            return true;
        }
    }
}
