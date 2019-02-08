// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
#if !BUILDING_INBOX_LIBRARY
        [System.CLSCompliantAttribute(false)]
#endif
        public static ValueTask ToJsonAsync(this PipeWriter writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToJsonAsyncInternal(writer, value, settings, cancellationToken);
        }

        private static async ValueTask ToJsonAsyncInternal(this PipeWriter writer, object value, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
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

            // Allocate the initial buffer. We don't want to use the existing buffer as there may be very few bytes left
            // and we won't be able to calculate flushThreshold appropriately.
            Memory<byte> memory = writer.GetMemory(settings.EffectiveBufferSize);

            // For Pipes there is not a way to get current buffer total size, so we just use the initial memory size.
            int flushThreshold = (int)(memory.Length * .9); //todo: determine best value here

            do
            {
                isFinalBlock = ToJson(ref writerState, writer, flushThreshold, settings, ref current, ref previous, ref arrayIndex);
                await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
            } while (!isFinalBlock);
        }
    }
}
