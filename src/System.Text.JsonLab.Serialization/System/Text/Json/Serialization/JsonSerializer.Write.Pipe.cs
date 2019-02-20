// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        [System.CLSCompliantAttribute(false)]
        public static Task WriteAsync<TValue>(TValue value, PipeWriter utf8Json, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return WriteAsyncCore(value, typeof(TValue), utf8Json, options, cancellationToken);
        }

        [System.CLSCompliantAttribute(false)]
        public static Task WriteAsync(object value, Type type, PipeWriter utf8Json, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            VerifyValueAndType(value, type);

            return WriteAsyncCore(value, type, utf8Json, options, cancellationToken);
        }

        private static async Task WriteAsyncCore(object value, Type type, PipeWriter utf8Json, JsonSerializerOptions options, CancellationToken cancellationToken)
        {
            if (options == null)
                options = s_defaultSettings;

            var writerState = new JsonWriterState(options.WriterOptions);

            // Allocate the initial buffer. We don't want to use the existing buffer as there may be very few bytes left
            // and we won't be able to calculate flushThreshold appropriately.
            Memory<byte> memory = utf8Json.GetMemory(options.EffectiveBufferSize);

            if (value == null)
            {
                WriteNull(ref writerState, utf8Json);
                await utf8Json.FlushAsync(cancellationToken).ConfigureAwait(false);
                return;
            }

            if (type == null)
                type = value.GetType();

            JsonClassInfo classInfo = options.GetOrAddClass(type);
            WriteObjectState current = default;
            current.ClassInfo = classInfo;
            current.CurrentValue = value;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }
            List<WriteObjectState> previous = null;
            int arrayIndex = 0;

            bool isFinalBlock;

            // For Pipes there is not a way to get current buffer total size, so we just use the initial memory size.
            int flushThreshold = (int)(memory.Length * .9); //todo: determine best value here (extensible?)

            do
            {
                isFinalBlock = Write(ref writerState, utf8Json, flushThreshold, options, ref current, ref previous, ref arrayIndex);
                await utf8Json.FlushAsync(cancellationToken).ConfigureAwait(false);
            } while (!isFinalBlock);
        }
    }
}
