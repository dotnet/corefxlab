// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static Task ToJsonAsync(this Stream stream, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToJsonAsyncInternal(stream, value, settings, cancellationToken);
        }

        private static async Task ToJsonAsyncInternal(this Stream stream, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            ToJsonObjectState current = default;
            Type initialType = value.GetType();

            JsonClassInfo classInfo = settings.GetOrAddClass(initialType);
            current.ClassInfo = classInfo;
            current.CurrentValue = value;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            List<ToJsonObjectState> previous = null;
            int arrayIndex = 0;
            var writerState = new JsonWriterState(settings.WriterOptions);
            bool isFinalBlock;

            using (var bufferWriter = new ArrayBufferWriter<byte>(settings.DefaultBufferSize))
            {
                int flushThreshold;
                do
                {
                    flushThreshold = (int)(bufferWriter.Capacity * .9); //todo: determine best value here

                    isFinalBlock = ToJson(ref writerState, bufferWriter, flushThreshold, settings, ref current, ref previous, ref arrayIndex);
#if BUILDING_INBOX_LIBRARY
                    await stream.WriteAsync(bufferWriter.WrittenMemory, cancellationToken);
#else
                    await stream.WriteAsync(bufferWriter.WrittenMemory.ToArray(), 0, bufferWriter.WrittenMemory.Length, cancellationToken);
#endif
                    bufferWriter.Clear();
                } while (!isFinalBlock);
            }

            // todo: do we want to call FlushAsync here? It seems like leaving it to the caller would be better.
            //await stream.FlushAsync(cancellationToken);
        }

        private static bool ToJson(
            ref JsonWriterState writerState,
            IBufferWriter<byte> bufferWriter,
            int flushThreshold,
            JsonConverterSettings settings,
            ref ToJsonObjectState current,
            ref List<ToJsonObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonWriter writer = new Utf8JsonWriter(bufferWriter, writerState);

            bool isFinalBlock = ToJson(
                ref writer,
                flushThreshold,
                settings,
                ref current,
                ref previous,
                ref arrayIndex);

            writer.Flush(isFinalBlock: isFinalBlock);
            writerState = writer.GetCurrentState();

            return isFinalBlock;
        }
    }
}
