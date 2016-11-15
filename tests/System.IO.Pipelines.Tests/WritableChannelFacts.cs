using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class WritableChannelFacts
    {
        [Fact]
        public async Task StreamAsWritableChannel()
        {
            var stream = new MemoryStream();

            var channel = stream.AsPipelineWriter();

            var buffer = channel.Alloc();
            buffer.WriteUtf8String("Hello World");
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public async Task StreamAsWritableChannelTwiceWritesToSameUnderlyingStream()
        {
            var stream = new MemoryStream();

            var channel = stream.AsPipelineWriter();

            var buffer = channel.Alloc();
            buffer.WriteUtf8String("Hello World");
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            channel.Complete();

            channel = stream.AsPipelineWriter();

            buffer = channel.Alloc();
            buffer.WriteUtf8String("Hello World");
            await buffer.FlushAsync();

            Assert.Equal("Hello WorldHello World", Encoding.UTF8.GetString(stream.ToArray()));

            channel.Complete();
        }

        [Fact]
        public async Task StreamAsWritableChannelWriteToChannelThenWriteToStream()
        {
            var stream = new MemoryStream();

            var channel = stream.AsPipelineWriter();

            var buffer = channel.Alloc();
            buffer.WriteUtf8String("Hello World");
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            channel.Complete();

            var sw = new StreamWriter(stream);
            sw.Write("Hello World");
            sw.Flush();

            Assert.Equal("Hello WorldHello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public void StreamAsWritableChannelNothingWrittenIfNotFlushed()
        {
            var stream = new MemoryStream();

            var channel = stream.AsPipelineWriter();

            var buffer = channel.Alloc();
            buffer.WriteUtf8String("Hello World");

            Assert.Equal(0, stream.Length);

            channel.Complete();
        }

        [Fact]
        public async Task StreamAsWritableChannelUsesUnderlyingChannel()
        {
            using (var stream = new MyCustomStream())
            {
                var outputChannel = stream.AsPipelineWriter();

                var output = outputChannel.Alloc();
                output.WriteUtf8String("Hello World");
                await output.FlushAsync();
                outputChannel.Complete();

                var sw = new StreamReader(stream);

                Assert.Equal("Hello World", sw.ReadToEnd());
            }
        }

        private class MyCustomStream : Stream, IPipelineWriter
        {
            private readonly PipelineReaderWriter _channel = new PipelineReaderWriter(ArrayBufferPool.Instance);

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotSupportedException();
                }

                set
                {
                    throw new NotSupportedException();
                }
            }

            public Task Writing => _channel.Writing;

            public WritableBuffer Alloc(int minimumSize = 0)
            {
                return _channel.Alloc(minimumSize);
            }

            public void Complete(Exception exception = null)
            {
                _channel.CompleteWriter(exception);
            }

            public override void Flush()
            {

            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _channel.ReadAsync(new Span<byte>(buffer, offset, count)).GetAwaiter().GetResult();
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
                _channel.WriteAsync(new Span<byte>(buffer, offset, count)).GetAwaiter().GetResult();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _channel.CompleteReader();
                _channel.CompleteWriter();
            }
        }
    }
}
