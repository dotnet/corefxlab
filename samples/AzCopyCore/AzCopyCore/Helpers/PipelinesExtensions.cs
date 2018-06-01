// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    // TODO (pri 3): Would be nice to add to the platform (but NetStandard does not support the stream APIs)
    static class PipelinesExtensions
    {
        /// <summary>
        /// Copies bytes from ReadOnlySequence to a Stream
        /// </summary>
        public static async Task WriteAsync(this Stream stream, ReadOnlySequence<byte> buffer)
        {
            for (SequencePosition position = buffer.Start; buffer.TryGet(ref position, out var memory);)
            {
                await stream.WriteAsync(memory).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Copies bytes from PipeReader to a Stream
        /// </summary>
        public static async Task WriteAsync(this Stream stream, PipeReader reader, ulong bytes)
        {
            while (bytes > 0)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> bodyBuffer = result.Buffer;
                if (bytes < (ulong)bodyBuffer.Length)
                {
                    throw new NotImplementedException();
                }
                bytes -= (ulong)bodyBuffer.Length;
                await stream.WriteAsync(bodyBuffer).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                reader.AdvanceTo(bodyBuffer.End);
            }
        }

        /// <summary>
        /// Copies bytes from Stream to PipeWriter 
        /// </summary>
        public static async Task WriteAsync(this PipeWriter writer, Stream stream, long bytesToWrite)
        {
            if (!stream.CanRead) throw new ArgumentException("Stream.CanRead returned false", nameof(stream));
            while (bytesToWrite > 0)
            {
                Memory<byte> buffer = writer.GetMemory();
                if (buffer.Length > bytesToWrite)
                {
                    buffer = buffer.Slice(0, (int)bytesToWrite);
                }
                if (buffer.Length == 0) throw new NotSupportedException("PipeWriter.GetMemory returned an empty buffer.");
                int read = await stream.ReadAsync(buffer).ConfigureAwait(false);
                if (read == 0) return;
                writer.Advance(read);
                bytesToWrite -= read;
                await writer.FlushAsync();
            }
        }
    }
}


