// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Binary;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Common extension methods against readable buffers
    /// </summary>
    public static class DefaultReadableBufferExtensions
    {
        /// <summary>
        /// Copies a <see cref="ReadableBuffer"/> to a <see cref="Stream"/> asynchronously
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to copy</param>
        /// <param name="stream">The target <see cref="Stream"/></param>
        /// <returns></returns>
        public static Task CopyToAsync(this ReadableBuffer buffer, Stream stream)
        {
            if (buffer.IsSingleSpan)
            {
                return WriteToStream(stream, buffer.First);
            }

            return CopyMultipleToStreamAsync(buffer, stream);
        }

        private static async Task CopyMultipleToStreamAsync(this ReadableBuffer buffer, Stream stream)
        {
            foreach (var memory in buffer)
            {
                await WriteToStream(stream, memory);
            }
        }

        private static async Task WriteToStream(Stream stream, Memory<byte> memory)
        {
            ArraySegment<byte> data;
            if (memory.TryGetArray(out data))
            {
                await stream.WriteAsync(data.Array, data.Offset, data.Count).ConfigureAwait(continueOnCapturedContext: false);
            }
            else
            {
                // Copy required 
                var array = memory.Span.ToArray();
                await stream.WriteAsync(array, 0, array.Length).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static async Task<ReadableBuffer> ReadToEndAsync(this IPipeReader input)
        {
            while (true)
            {
                // Wait for more data
                var result = await input.ReadAsync();

                if (result.IsCompleted)
                {
                    // Read all the data, return it
                    return result.Buffer;
                }

                // Don't advance the buffer so remains in buffer
                input.Advance(result.Buffer.Start, result.Buffer.End);
            }
        }

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadBigEndian<[Primitive]T>(this ReadableBuffer buffer) where T : struct
        {
            var memory = buffer.First;
            int len = Unsafe.SizeOf<T>();
            var value = memory.Length >= len ? memory.Span.ReadBigEndian<T>() : ReadMultiBig<T>(buffer, len);
            return value;
        }

        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/> out of a buffer of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadLittleEndian<[Primitive]T>(this ReadableBuffer buffer) where T : struct
        {
            var memory = buffer.First;
            int len = Unsafe.SizeOf<T>();
            var value = memory.Length >= len ? memory.Span.ReadLittleEndian<T>() : ReadMultiLittle<T>(buffer, len);
            return value;
        }

        private static unsafe T ReadMultiBig<[Primitive]T>(ReadableBuffer buffer, int len) where T : struct
        {
            byte* local = stackalloc byte[len];
            var localSpan = new Span<byte>(local, len);
            buffer.Slice(0, len).CopyTo(localSpan);
            return localSpan.ReadBigEndian<T>();
        }

        private static unsafe T ReadMultiLittle<[Primitive]T>(ReadableBuffer buffer, int len) where T : struct
        {
            byte* local = stackalloc byte[len];
            var localSpan = new Span<byte>(local, len);
            buffer.Slice(0, len).CopyTo(localSpan);
            return localSpan.ReadLittleEndian<T>();
        }
    }
}
