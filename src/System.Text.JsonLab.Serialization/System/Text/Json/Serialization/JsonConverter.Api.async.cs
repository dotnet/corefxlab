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

        public static Task<T> FromJsonAsync<T>(this Stream source, JsonConverterSettings options = null, CancellationToken cancellationToken = default)
        {
            return FromJsonAsync<T>(source, typeof(T), options, cancellationToken);
        }

        public static Task<object> FromJsonAsync(this Stream source, Type returnType, JsonConverterSettings options = null, CancellationToken cancellationToken = default)
        {
            return FromJsonAsync<object>(source, returnType, options, cancellationToken);
        }

        private static async Task<T> FromJsonAsync<T>(this Stream source, Type returnType, JsonConverterSettings options = null, CancellationToken cancellationToken = default)
        {
            if (options == null)
                options = s_default_options;

            var readerState = new JsonReaderState(options.MaxDepth, options.ReaderOptions);
            JsonObjectState current = default;
            List<JsonObjectState> previous = null;
            int arrayIndex = 0;

            int bytesRemaining = 0;
            int bytesRead;

            byte[] buffer = ArrayPool<byte>.Shared.Rent(options.DefaultBufferSize);
            int bufferSize = buffer.Length;
            try
            {
                do
                {
                    int bytesToRead = bufferSize - bytesRemaining;
                    bytesRead = await source.ReadAsync(buffer, bytesRemaining, bytesToRead, cancellationToken).ConfigureAwait(false);

                    int deserializeBufferSize = bytesRemaining + bytesRead;
                    bool isFinalBlock = (bytesRead == 0);
                    if (ReadData(
                        ref readerState,
                        returnType,
                        isFinalBlock,
                        buffer,
                        deserializeBufferSize,
                        options,
                        ref current,
                        ref previous,
                        ref arrayIndex))
                    {
                        return (T)current.ReturnValue;
                    }

                    // We have to shift or expand the buffer because there wasn't enough data to complete deserialization.
                    Debug.Assert(isFinalBlock == false);
                    int bytesConsumed = (int)readerState.BytesConsumed;
                    bytesRemaining = deserializeBufferSize - bytesConsumed;

                    if (bytesRemaining <= (bufferSize / 2))
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
                        ArrayPool<byte>.Shared.Return(buffer, clearArray:true);
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

        private static bool ReadData(
            ref JsonReaderState readerState,
            Type returnType,
            bool isFinalBlock,
            byte[] buffer,
            int bytesToRead,
            JsonConverterSettings options,
            ref JsonObjectState current,
            ref List<JsonObjectState> previous,
            ref int arrayIndex)
        {
            Utf8JsonReader reader = new Utf8JsonReader(new ReadOnlySequence<byte>(buffer, 0, bytesToRead), isFinalBlock, readerState);
            bool finished = ReadData(
                ref reader,
                options,
                returnType,
                ref current,
                ref previous,
                ref arrayIndex);

            readerState = reader.CurrentState;
            return finished;
        }
    }
}
