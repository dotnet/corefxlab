// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Formatting;
using System.Buffers;

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
            buffer.Append("Hello World", TextEncoder.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public async Task StreamAsPipelineWriterTwiceWritesToSameUnderlyingStream()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoder.Utf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            writer.Complete();

            writer = stream.AsPipelineWriter();

            buffer = writer.Alloc();
            buffer.Append("Hello World", TextEncoder.Utf8);
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
            buffer.Append("Hello World", TextEncoder.Utf8);
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
            buffer.Append("Hello World", TextEncoder.Utf8);

            Assert.Equal(0, stream.Length);
            buffer.Commit();
            writer.Complete();
        }

        [Fact]
        public async Task StreamAsPipelineWriterUsesUnderlyingWriter()
        {
            using (var stream = new MyCustomStream())
            {
                var writer = stream.AsPipelineWriter();

                var output = writer.Alloc();
                output.Append("Hello World", TextEncoder.Utf8);
                await output.FlushAsync();
                writer.Complete();

                var sw = new StreamReader(stream);

                Assert.Equal("Hello World", sw.ReadToEnd());
            }
        }

        private class MyCustomStream : Stream, IPipeWriter
        {
            private readonly Pipe _pipe = new Pipe(BufferPool.Default);

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

            public WritableBuffer Alloc(int minimumSize = 0)
            {
                return _pipe.Writer.Alloc(minimumSize);
            }

            public void Complete(Exception exception = null)
            {
                _pipe.Writer.Complete(exception);
            }

            public void CancelPendingFlush()
            {

            }

            public override void Flush()
            {

            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _pipe.ReadAsync(new ArraySegment<byte>(buffer, offset, count)).GetAwaiter().GetResult();
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
                _pipe.WriteAsync(new ArraySegment<byte>(buffer, offset, count)).GetAwaiter().GetResult();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _pipe.Reader.Complete();
                _pipe.Writer.Complete();
            }
        }
    }
}
