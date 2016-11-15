using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipelineWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IPipelineWriter AsPipelineWriter(this Stream stream)
        {
            return (stream as IPipelineWriter) ?? stream.AsPipelineWriter(ArrayBufferPool.Instance);
        }

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipelineWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        public static IPipelineWriter AsPipelineWriter(this Stream stream, IBufferPool pool)
        {
            var writer = new PipelineReaderWriter(pool);
            writer.CopyToAsync(stream).ContinueWith((task, state) =>
            {
                var innerWriter = (PipelineReaderWriter)state;

                if (task.IsFaulted)
                {
                    innerWriter.CompleteReader(task.Exception.InnerException);
                }
                else
                {
                    innerWriter.CompleteReader();
                }
            }, 
            writer, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return writer;
        }

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipelineReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IPipelineReader AsPipelineReader(this Stream stream) => AsPipelineReader(stream, CancellationToken.None);

        /// <summary>
        /// Adapts a <see cref="Stream"/> into a <see cref="IPipelineReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static IPipelineReader AsPipelineReader(this Stream stream, CancellationToken cancellationToken)
        {
            if (stream is IPipelineReader)
            {
                return (IPipelineReader)stream;
            }

            var streamAdaptor = new UnownedBufferStream(stream);
            streamAdaptor.Produce(cancellationToken);
            return streamAdaptor.Reader;
        }

        /// <summary>
        /// Copies the content of a <see cref="Stream"/> into a <see cref="IPipelineWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this Stream stream, IPipelineWriter writer)
        {
            return stream.CopyToAsync(new PipelineWriterStream(writer));
        }

        private class UnownedBufferStream : Stream
        {
            private readonly Stream _stream;
            private readonly UnownedBufferReader _reader;

            public IPipelineReader Reader => _reader;

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
            private IPipelineWriter _writer;

            public PipelineWriterStream(IPipelineWriter writer)
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
