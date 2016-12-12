using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;
using System.Text.Formatting;

namespace System.IO.Pipelines.Tests
{
    public class PipelineWriterFacts
    {
        [Fact]
        public async Task StreamAsPipelineWriter()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoding.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public async Task StreamAsPipelineWriterTwiceWritesToSameUnderlyingStream()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoding.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            writer.Complete();

            writer = stream.AsPipelineWriter();

            buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoding.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello WorldHello World", Encoding.UTF8.GetString(stream.ToArray()));

            writer.Complete();
        }

        [Fact]
        public async Task StreamAsPipelineWriterWriteToWriterThenWriteToStream()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoding.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            writer.Complete();

            var sw = new StreamWriter(stream);
            sw.Write("Hello World");
            sw.Flush();

            Assert.Equal("Hello WorldHello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public void StreamAsPipelineWriterNothingWrittenIfNotFlushed()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoding.Utf8);

            Assert.Equal(0, stream.Length);

            writer.Complete();
        }

        [Fact]
        public async Task StreamAsPipelineWriterUsesUnderlyingWriter()
        {
            using (var stream = new MyCustomStream())
            {
                var writer = stream.AsPipelineWriter();

                var output = writer.Alloc();
                output.Append("Hello World", TextEncoding.Utf8);
                await output.FlushAsync();
                writer.Complete();

                var sw = new StreamReader(stream);

                Assert.Equal("Hello World", sw.ReadToEnd());
            }
        }

        private class MyCustomStream : Stream, IPipelineWriter
        {
            private readonly Pipe _pipe = new Pipe(ArrayBufferPool.Instance);

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

            public Task Writing => _pipe.Writing;

            public WritableBuffer Alloc(int minimumSize = 0)
            {
                return _pipe.Alloc(minimumSize);
            }

            public void Complete(Exception exception = null)
            {
                _pipe.CompleteWriter(exception);
            }

            public override void Flush()
            {

            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _pipe.ReadAsync(new Span<byte>(buffer, offset, count)).GetAwaiter().GetResult();
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
                _pipe.WriteAsync(new Span<byte>(buffer, offset, count)).GetAwaiter().GetResult();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _pipe.CompleteReader();
                _pipe.CompleteWriter();
            }
        }
    }
}
