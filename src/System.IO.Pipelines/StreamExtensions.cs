// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
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
            return (stream as IPipeWriter) ?? stream.AsPipelineWriter(ArrayBufferPool.Instance);
        }

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipeWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        public static IPipeWriter AsPipelineWriter(this Stream stream, IBufferPool pool)
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
        public static Task CopyToAsync(this Stream stream, IPipeWriter writer)
        {
            return stream.CopyToAsync(new PipelineWriterStream(writer));
        }

        private class UnownedBufferStream : Stream
        {
            private readonly Stream _stream;
            private readonly UnownedBufferReader _reader;

            public IPipeReader Reader => _reader;

            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => true;

            public override long Length
            {
                get
                {
                    ThrowHelper.ThrowNotSupportedException();
                    return 0;
                }
            }

            public override long Position
            {
                get
                {
                    ThrowHelper.ThrowNotSupportedException();
                    return 0;
                }

                set
                {
                    ThrowHelper.ThrowNotSupportedException();
                }
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
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }

            public override void SetLength(long value)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }
        }

        private class PipelineWriterStream : Stream
        {
            private IPipeWriter _writer;

            public PipelineWriterStream(IPipeWriter writer)
            {
                _writer = writer;
            }

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length
            {
                get
                {
                    ThrowHelper.ThrowNotSupportedException();
                    return 0;
                }
            }

            public override long Position
            {
                get
                {
                    ThrowHelper.ThrowNotSupportedException();
                    return 0;
                }

                set
                {
                    ThrowHelper.ThrowNotSupportedException();
                }
            }

            public override void Flush()
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                ThrowHelper.ThrowNotSupportedException();
                return 0;
            }

            public override void SetLength(long value)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                var output = _writer.Alloc();
                output.Write(new Span<byte>(buffer, offset, count));
                await output.FlushAsync();
            }
        }
    }
}
