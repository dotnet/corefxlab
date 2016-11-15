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
            var channel = new PipelineReaderWriter(pool);
            channel.CopyToAsync(stream).ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    channel.CompleteReader(task.Exception);
                }
                else
                {
                    channel.CompleteReader();
                }
            });

            return channel;
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
            return streamAdaptor.Channel;
        }

        /// <summary>
        /// Copies the content of a <see cref="Stream"/> into a <see cref="IPipelineWriter"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static Task CopyToAsync(this Stream stream, IPipelineWriter channel)
        {
            return stream.CopyToAsync(new StreamChannel(channel));
        }

        private class UnownedBufferStream : Stream
        {
            private readonly Stream _stream;
            private readonly UnownedBufferReader _channel;

            public IPipelineReader Channel => _channel;

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
                _channel = new UnownedBufferReader();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                WriteAsync(buffer, offset, count).Wait();
            }

            public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                await _channel.WriteAsync(new ArraySegment<byte>(buffer, offset, count), cancellationToken);
            }

            // *gasp* Async Void!? It works here because we still have _channel.Writing to track completion.
            internal async void Produce(CancellationToken cancellationToken)
            {
                // Wait for a reader
                await _channel.ReadingStarted;

                try
                {
                    // We have to provide a buffer size in order to provide a cancellation token. Weird but meh.
                    // 4096 is the "default" value.
                    await _stream.CopyToAsync(this, 4096, cancellationToken);
                    _channel.CompleteWriter();
                }
                catch (Exception ex)
                {
                    _channel.CompleteWriter(ex);
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

        private class StreamChannel : Stream
        {
            private IPipelineWriter _channel;

            public StreamChannel(IPipelineWriter channel)
            {
                _channel = channel;
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
                var channelBuffer = _channel.Alloc();
                channelBuffer.Write(new Span<byte>(buffer, offset, count));
                await channelBuffer.FlushAsync();
            }
        }
    }
}
