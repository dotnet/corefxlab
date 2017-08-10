// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipeWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IPipeWriter AsPipelineWriter(this Stream stream)
        {
            return (stream as IPipeWriter) ?? stream.AsPipelineWriter(BufferPool.Default);
        }

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipeWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        public static IPipeWriter AsPipelineWriter(this Stream stream, BufferPool pool)
        {
            var pipe = new Pipe(pool);
            pipe.CopyToAsync(stream);
            return pipe;
        }

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipeReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IPipeReader AsPipelineReader(this Stream stream) => AsPipelineReader(stream, CancellationToken.None);

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipeReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static IPipeReader AsPipelineReader(this Stream stream, CancellationToken cancellationToken)
        {
            if (stream is IPipeReader)
            {
                return (IPipeReader)stream;
            }

            var streamAdaptor = new UnownedBufferStream(stream);
            streamAdaptor.Produce(cancellationToken);
            return streamAdaptor.Reader;
        }

        /// <summary>
        /// Copies the content of a <see cref="Stream"/> into a <see cref="IPipeWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this Stream stream, IPipeWriter writer, CancellationToken cancellationToken = default(CancellationToken))
        {
            // 81920 is the default bufferSize, there is not stream.CopyToAsync overload that takes only a cancellationToken
            return stream.CopyToAsync(new PipelineWriterStream(writer, cancellationToken), bufferSize: 81920, cancellationToken: cancellationToken);
        }

        public static async Task CopyToEndAsync(this Stream stream, IPipeWriter writer, CancellationToken cancellationToken = default(CancellationToken))
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

        private static async Task WriteToStream(Stream stream, Buffer<byte> memory)
        {
            ArraySegment<byte> data;
            if (memory.TryGetArray(out data))
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

        private class UnownedBufferStream : Stream
        {
            private readonly Stream _stream;
            private readonly UnownedBufferReader _reader;

            public IPipeReader Reader => _reader;

            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => true;

            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public UnownedBufferStream(Stream stream)
            {
                _stream = stream;
                _reader = new UnownedBufferReader();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                WriteAsync(buffer, offset, count).Wait();
            }

            public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                await _reader.WriteAsync(new ArraySegment<byte>(buffer, offset, count), cancellationToken);
            }

            // *gasp* Async Void!? It works here because we still have _reader.Writing to track completion.
            internal async void Produce(CancellationToken cancellationToken)
            {
                // Wait for a reader
                await _reader.ReadingStarted;

                try
                {
                    // We have to provide a buffer size in order to provide a cancellation token. Weird but meh.
                    // 4096 is the "default" value.
                    await _stream.CopyToAsync(this, 4096, cancellationToken);
                    _reader.CompleteWriter();
                }
                catch (Exception ex)
                {
                    _reader.CompleteWriter(ex);
                }
            }

            public override void Flush()
            {
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }

        private class PipelineWriterStream : Stream
        {
            private readonly IPipeWriter _writer;
            private readonly CancellationToken _cancellationToken;

            public PipelineWriterStream(IPipeWriter writer, CancellationToken cancellationToken)
            {
                _writer = writer;
                _cancellationToken = cancellationToken;
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
                _cancellationToken.ThrowIfCancellationRequested();

                if (_cancellationToken.CanBeCanceled)
                {
                    if (cancellationToken.CanBeCanceled)
                    {
                        cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationToken).Token;
                    }
                    else
                    {
                        cancellationToken = _cancellationToken;
                    }
                }

                var output = _writer.Alloc();
                output.Write(new Span<byte>(buffer, offset, count));
                await output.FlushAsync(cancellationToken);
            }
        }
    }
}
