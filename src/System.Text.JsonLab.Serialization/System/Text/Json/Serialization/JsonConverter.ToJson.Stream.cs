// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static ValueTask ToJsonAsync(this Stream writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToJsonAsyncInternal(writer, value, settings, cancellationToken);
        }

        private static async ValueTask ToJsonAsyncInternal(this Stream writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
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

            using (var bufferWriter = new ArrayBufferWriter<byte>(settings.EffectiveBufferSize))
            {
                int flushThreshold;
                do
                {
                    flushThreshold = (int)(bufferWriter.Capacity * .9); //todo: determine best value here

                    isFinalBlock = ToJson(ref writerState, bufferWriter, flushThreshold, settings, ref current, ref previous, ref arrayIndex);
#if BUILDING_INBOX_LIBRARY
                    await writer.WriteAsync(bufferWriter.WrittenMemory, cancellationToken).ConfigureAwait(false);
#else
                    // todo: stackalloc
                    await writer.WriteAsync(bufferWriter.WrittenMemory.ToArray(), 0, bufferWriter.WrittenMemory.Length, cancellationToken).ConfigureAwait(false);
#endif
                    bufferWriter.Clear();
                } while (!isFinalBlock);
            }

            // todo: do we want to call FlushAsync here (or above)? It seems like leaving it to the caller would be better.
            //await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
