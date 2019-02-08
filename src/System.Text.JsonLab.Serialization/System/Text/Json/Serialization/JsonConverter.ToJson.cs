// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        private static bool ToJson(
            ref Utf8JsonWriter writer,
            int flushThreshold,
            JsonConverterSettings settings,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            bool continueWriting = true;
            bool checkForFinished = true;
            do
            {
                ClassType classType = current.ClassInfo.ClassType;
                if (classType == ClassType.Enumerable)
                {
                    checkForFinished = WriteEnumerable(ref writer, ref current, ref previous, ref arrayIndex);
                }
                else if (classType == ClassType.Object)
                {
                    checkForFinished = WriteObject(ref writer, settings, ref current, ref previous, ref arrayIndex);
                }
                else
                {
                    checkForFinished = WriteValue(ref writer, ref current);
                }

                if (flushThreshold >= 0 && writer.BytesWritten > flushThreshold)
                {
                    return false;
                }
                else if (checkForFinished && writer.CurrentDepth == 0)
                {
                    continueWriting = false;
                }
            } while (continueWriting);

            return true;
        }        

        private static void AddNewStackFrame(
            JsonClassInfo nextClassInfo,
            object nextValue,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            SetPreviousState(ref previous, current, arrayIndex++);
            current.Reset();
            current.ClassInfo = nextClassInfo;
            current.CurrentValue = nextValue;

            if (nextClassInfo.ClassType == ClassType.Enumerable)
            {
                current.PopStackOnEndArray = true;
                current.PropertyInfo = current.ClassInfo.GetPolicyProperty();
            }
            else
            {
                Debug.Assert(nextClassInfo.ClassType == ClassType.Object);
                current.PopStackOnEndObject = true;
            }
        }
    }
}
