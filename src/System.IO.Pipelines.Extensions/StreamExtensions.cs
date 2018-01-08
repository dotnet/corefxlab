﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Copies the content of a <see cref="Stream"/> into a <see cref="IPipeWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="writer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this Stream stream, IPipeWriter writer, CancellationToken cancellationToken = default)
        {
            // 81920 is the default bufferSize, there is not stream.CopyToAsync overload that takes only a cancellationToken
            return stream.CopyToAsync(new PipelineWriterStream(writer), bufferSize: 81920, cancellationToken: cancellationToken);
        }

        public static async Task CopyToEndAsync(this Stream stream, IPipeWriter writer, CancellationToken cancellationToken = default)
        {
            try
            {
                await stream.CopyToAsync(writer, cancellationToken);
            }
            catch (Exception ex)
            {
                writer.Complete(ex);
                return;
            }
            writer.Complete();
        }

        /// <summary>
        /// Copies a <see cref="ReadOnlyBuffer"/> to a <see cref="Stream"/> asynchronously
        /// </summary>
        /// <param name="buffer">The <see cref="ReadOnlyBuffer"/> to copy</param>
        /// <param name="stream">The target <see cref="Stream"/></param>
        /// <returns></returns>
        public static Task CopyToAsync(this ReadOnlyBuffer buffer, Stream stream)
        {
            if (buffer.IsSingleSegment)
            {
                return WriteToStream(stream, buffer.First);
            }

            return CopyMultipleToStreamAsync(buffer, stream);
        }

        private static async Task CopyMultipleToStreamAsync(this ReadOnlyBuffer buffer, Stream stream)
        {
            foreach (var memory in buffer)
            {
                await WriteToStream(stream, memory);
            }
        }

        private static async Task WriteToStream(Stream stream, ReadOnlyMemory<byte> readOnlyMemory)
        {
            var memory = MemoryMarshal.AsMemory(readOnlyMemory);
            if (memory.TryGetArray(out ArraySegment<byte> data))
            {
                await stream.WriteAsync(data.Array, data.Offset, data.Count)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            else
            {
                // Copy required
                var array = memory.Span.ToArray();
                await stream.WriteAsync(array, 0, array.Length).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static Task CopyToEndAsync(this IPipeReader input, Stream stream)
        {
            return input.CopyToEndAsync(stream, 4096, CancellationToken.None);
        }

        public static async Task CopyToEndAsync(this IPipeReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            try
            {
                await input.CopyToAsync(stream, bufferSize, cancellationToken);
            }
            catch (Exception ex)
            {
                input.Complete(ex);
                return;
            }
            return;
        }

        private class PipelineWriterStream : Stream
        {
            private readonly IPipeWriter _writer;

            public PipelineWriterStream(IPipeWriter writer)
            {
                _writer = writer;
            }

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override void Flush()
            {
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var output = _writer.Alloc();
                output.Write(new ReadOnlySpan<byte>(buffer, offset, count));
                await output.FlushAsync(cancellationToken);
            }
        }
    }
}
