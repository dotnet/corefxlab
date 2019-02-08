// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
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
        public static Task<T> FromJsonAsync<T>(this PipeReader reader, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return FromJsonAsync<T>(reader, typeof(T), settings, cancellationToken);
        }

#if !BUILDING_INBOX_LIBRARY
        [System.CLSCompliantAttribute(false)]
#endif
        public static Task<object> FromJsonAsync(this PipeReader reader, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            return FromJsonAsync<object>(reader, returnType, settings, cancellationToken);
        }

        private static async Task<T> FromJsonAsync<T>(this PipeReader reader, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (settings == null)
                settings = s_DefaultSettings;

            FromJsonObjectState current = default;
            JsonClassInfo classInfo = settings.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            var readerState = new JsonReaderState(settings.MaxDepth, settings.ReaderOptions);
            List<FromJsonObjectState> previous = null;
            int arrayIndex = 0;

            ReadResult result;
            do
            {
                result = await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
                ReadOnlySequence<byte> buffer = result.Buffer;

                FromJson(
                    ref readerState,
                    returnType,
                    result.IsCompleted,
                    buffer,
                    settings,
                    ref current,
                    ref previous,
                    ref arrayIndex);

                reader.AdvanceTo(buffer.End);
            } while (!result.IsCompleted);

            return (T)current.ReturnValue;
        }
    }
}
