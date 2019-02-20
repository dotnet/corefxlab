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
        private static bool Write(
            ref JsonWriterState writerState,
            IBufferWriter<byte> bufferWriter,
            int flushThreshold,
            JsonSerializerOptions options,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonWriter writer = new Utf8JsonWriter(bufferWriter, writerState);

            bool isFinalBlock = Write(
                ref writer,
                flushThreshold,
                options,
                ref current,
                ref previous,
                ref arrayIndex);

            writer.Flush(isFinalBlock: isFinalBlock);
            writerState = writer.GetCurrentState();

            return isFinalBlock;
        }

        private static bool Write(
            ref Utf8JsonWriter writer,
            int flushThreshold,
            JsonSerializerOptions options,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
            ref int arrayIndex)
        {
            bool continueWriting = true;
            bool finishedSerializing;
            do
            {
                switch (current.ClassInfo.ClassType)
                {
                    case ClassType.Enumerable:
                        finishedSerializing = WriteEnumerable(options, ref writer, ref current, ref previous, ref arrayIndex);
                        break;
                    case ClassType.Object:
                        finishedSerializing = WriteObject(options, ref writer, ref current, ref previous, ref arrayIndex);
                        break;
                    default:
                        finishedSerializing = WriteValue(options, ref writer, ref current);
                        break;
                }

                if (flushThreshold >= 0 && writer.BytesWritten > flushThreshold)
                {
                    return false;
                }

                if (finishedSerializing && writer.CurrentDepth == 0)
                {
                    continueWriting = false;
                }
            } while (continueWriting);

            return true;
        }        

        private static void AddNewStackFrame(
            JsonClassInfo nextClassInfo,
            object nextValue,
            ref WriteObjectState current,
            ref List<WriteObjectState> previous,
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
