// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Text;
using System.Text.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
            buffer.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));
        }

        [Fact]
        public async Task StreamAsPipelineWriterTwiceWritesToSameUnderlyingStream()
        {
            var stream = new MemoryStream();

            var writer = stream.AsPipelineWriter();

            var buffer = writer.Alloc();
            buffer.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
            await buffer.FlushAsync();

            Assert.Equal("Hello World", Encoding.UTF8.GetString(stream.ToArray()));

            writer.Complete();

            writer = stream.AsPipelineWriter();

            buffer = writer.Alloc();
            buffer.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
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
            buffer.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
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
            buffer.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);

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
                output.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
                await output.FlushAsync();
                writer.Complete();

                var sw = new StreamReader(stream);

                Assert.Equal("Hello World", sw.ReadToEnd());
            }
        }

        [Fact]
        public async Task CanCancelCopyToAsync()
        {
            var bytes = new byte[100];
            using (var stream = new MemoryStream(bytes))
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool, new PipeOptions
                {
                    // We want to block FlushAsync so we can test the CancellationToken
                    MaximumSizeHigh = 10
                });

                var source = new CancellationTokenSource();
                var copyTask = stream.CopyToAsync(pipe.Writer, source.Token);

                Assert.False(copyTask.Wait(100));

                source.Cancel();

                await Assert.ThrowsAsync<OperationCanceledException>(() => copyTask);
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

            public void Allocate(int minimumSize = 0)
            {
                _pipe.Writer.Allocate(minimumSize);
            }

            public void Ensure(int count = 1)
            {
                _pipe.Writer.Ensure(count);
            }

            public void Advance(int bytesWritten)
            {
                _pipe.Writer.Advance(bytesWritten);
            }

            public void Append(ReadableBuffer buffer)
            {
                _pipe.Writer.Append(buffer);
            }

            public void Commit()
            {
                _pipe.Writer.Commit();
            }

            public new WritableBufferAwaitable FlushAsync(CancellationToken cancellationToken = default)
            {
               return  _pipe.Writer.FlushAsync(cancellationToken);
            }

            public Buffer<byte> Buffer { get; }

            public void Complete(Exception exception = null)
            {
                _pipe.Writer.Complete(exception);
            }

            public void CancelPendingFlush()
            {

            }

            public void OnReaderCompleted(Action<Exception, object> callback, object state)
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
