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
        private const int HalfMaxValue = int.MaxValue / 2;

        public static Task<T> FromJsonAsync<T>(this Stream stream, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            return FromJsonAsync<T>(stream, typeof(T), settings, cancellationToken);
        }

        public static Task<object> FromJsonAsync(this Stream stream, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            return FromJsonAsync<object>(stream, returnType, settings, cancellationToken);
        }

        private static async Task<T> FromJsonAsync<T>(this Stream stream, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
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

            int bytesRemaining = 0;
            int bytesRead;

            // todo: switch to IBufferWriter implementation.
            byte[] buffer = ArrayPool<byte>.Shared.Rent(settings.DefaultBufferSize);
            int bufferSize = buffer.Length;
            try
            {
                do
                {
                    int bytesToRead = bufferSize - bytesRemaining;
                    bytesRead = await stream.ReadAsync(buffer, bytesRemaining, bytesToRead, cancellationToken).ConfigureAwait(false);

                    int deserializeBufferSize = bytesRemaining + bytesRead;
                    bool isFinalBlock = (bytesRead == 0);
                    bool finished = FromJson(
                        ref readerState,
                        returnType,
                        isFinalBlock,
                        buffer,
                        deserializeBufferSize,
                        settings,
                        ref current,
                        ref previous,
                        ref arrayIndex);

                    if (finished)
                    {
                        return (T)current.ReturnValue;
                    }

                    // We have to shift or expand the buffer because there wasn't enough data to complete deserialization.
                    Debug.Assert(isFinalBlock == false);
                    int bytesConsumed = (int)readerState.BytesConsumed;
                    bytesRemaining = deserializeBufferSize - bytesConsumed;

                    if (bytesConsumed <= (bufferSize / 2))
                    {
                        // We have less than half the buffer available, double the buffer size.
                        bufferSize = (bufferSize < HalfMaxValue) ? bufferSize * 2 : int.MaxValue;

                        byte[] dest = ArrayPool<byte>.Shared.Rent(bufferSize);
                        bufferSize = dest.Length;
                        if (bytesRemaining > 0)
                        {
                            // Copy the unprocessed data to the new buffer while shifting the processed bytes.
                            Buffer.BlockCopy(buffer, bytesConsumed, dest, 0, bytesRemaining);
                        }
                        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
                        buffer = dest;
                    }
                    else if (bytesRemaining > 0)
                    {
                        // Shift the processed bytes to the beginning of buffer to make more room.
                        Buffer.BlockCopy(buffer, bytesConsumed, buffer, 0, bytesRemaining);
                    }
                } while (bytesRead > 0);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
            }

            throw new InvalidOperationException("todo");
        }

        private static bool FromJson(
            ref JsonReaderState readerState,
            Type returnType,
            bool isFinalBlock,
            byte[] buffer,
            int bytesToRead,
            JsonConverterSettings settings,
            ref FromJsonObjectState current,
            ref List<FromJsonObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonReader reader = new Utf8JsonReader(buffer.AsSpan(0, bytesToRead), isFinalBlock, readerState);

            bool finished = FromJson(
                ref reader,
                settings,
                returnType,
                ref current,
                ref previous,
                ref arrayIndex);

            readerState = reader.CurrentState;
            return finished;
        }
    }
}
