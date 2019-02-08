// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        private static bool ToJson(
            ref Utf8JsonWriter writer,
            JsonConverterSettings settings,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            bool writeMore = true;
            do
            {
                ClassType classType = current.ClassInfo.ClassType;
                if (classType == ClassType.Enumerable)
                {
                    if (WriteEnumerable(ref writer, settings, ref current, ref previous, ref arrayIndex))
                    {
                        WriteValue(ref writer, settings, ref current, ref previous, ref arrayIndex);
                    }
                    else
                    {
                        writeMore = (writer.CurrentDepth > 0);
                    }
                }
                else if (classType == ClassType.Object)
                {
                    if (WriteObject(ref writer, ref current, ref previous, ref arrayIndex))
                    {
                        WriteValue(ref writer, settings, ref current, ref previous, ref arrayIndex);
                    }
                    else
                    {
                        writeMore = (writer.CurrentDepth > 0);
                    }
                }
                else
                {
                    WriteValue(ref writer, settings, ref current, ref previous, ref arrayIndex);
                    writeMore = (writer.CurrentDepth > 0);
                }

                // todo: if writeMore==true and we wrote to buffer length, then we need to check for a flush (when async implemented)
            } while (writeMore);

            return false;
        }        
    }
}
