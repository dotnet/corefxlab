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

        public static Task<T> FromJsonAsync<T>(this Stream reader, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return FromJsonAsync<T>(reader, typeof(T), settings, cancellationToken);
        }

        public static Task<object> FromJsonAsync(this Stream reader, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            return FromJsonAsync<object>(reader, returnType, settings, cancellationToken);
        }

        private static async Task<T> FromJsonAsync<T>(this Stream reader, Type returnType, JsonConverterSettings settings = null, CancellationToken cancellationToken = default)
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

            // todo: switch to IBufferWriter implementation to handle the allocs?
            byte[] buffer = ArrayPool<byte>.Shared.Rent(settings.EffectiveBufferSize);
            int bufferSize = buffer.Length;
            bool isFinalBlock;
            try
            {
                do
                {
                    int bytesToRead = bufferSize - bytesRemaining;
                    bytesRead = await reader.ReadAsync(buffer, bytesRemaining, bytesToRead, cancellationToken).ConfigureAwait(false);

                    int deserializeBufferSize = bytesRemaining + bytesRead;
                    isFinalBlock = (bytesRead == 0);

                    FromJson(
                        ref readerState,
                        returnType,
                        isFinalBlock,
                        buffer,
                        deserializeBufferSize,
                        settings,
                        ref current,
                        ref previous,
                        ref arrayIndex);

                    if (isFinalBlock)
                    {
                        return (T)current.ReturnValue;
                    }

                    // We have to shift or expand the buffer because there wasn't enough data to complete deserialization.
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
                } while (!isFinalBlock);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
            }

            throw new InvalidOperationException("todo");
        }
    }
}
