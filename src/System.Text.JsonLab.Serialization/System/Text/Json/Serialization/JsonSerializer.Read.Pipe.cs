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
    public static partial class JsonSerializer
    {
        [System.CLSCompliantAttribute(false)]
        public static ValueTask<TValue> ReadAsync<TValue>(PipeReader utf8Json, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            return ReadAsync<TValue>(utf8Json, typeof(TValue), options, cancellationToken);
        }

        [System.CLSCompliantAttribute(false)]
        public static ValueTask<object> ReadAsync(PipeReader utf8Json, Type returnType, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            return ReadAsync<object>(utf8Json, returnType, options, cancellationToken);
        }

        private static async ValueTask<TValue> ReadAsync<TValue>(PipeReader utf8Json, Type returnType, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (options == null)
                options = s_defaultSettings;

            ReadObjectState current = default;
            JsonClassInfo classInfo = options.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            var readerState = new JsonReaderState(options: options.ReaderOptions);
            List<ReadObjectState> previous = null;
            int arrayIndex = 0;

            ReadResult result;
            do
            {
                result = await utf8Json.ReadAsync(cancellationToken).ConfigureAwait(false);
                ReadOnlySequence<byte> buffer = result.Buffer;

                ReadCore(
                    ref readerState,
                    returnType,
                    result.IsCompleted,
                    buffer,
                    options,
                    ref current,
                    ref previous,
                    ref arrayIndex);

                utf8Json.AdvanceTo(buffer.GetPosition(readerState.BytesConsumed), buffer.End);
            } while (!result.IsCompleted);

            return (TValue)current.ReturnValue;
        }

        private static void ReadCore(
            ref JsonReaderState readerState,
            Type returnType,
            bool isFinalBlock,
            ReadOnlySequence<byte> buffer,
            JsonSerializerOptions options,
            ref ReadObjectState current,
            ref List<ReadObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonReader reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);

            ReadCore(
                options,
                ref reader,
                ref current,
                ref previous,
                ref arrayIndex);

            readerState = reader.CurrentState;
        }
    }
}
