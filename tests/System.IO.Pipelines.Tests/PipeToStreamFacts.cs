using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeToStreamFacts
    {
        [Fact]
        public async Task CopyFromPipeToEnd()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new MemoryStream())
            {
                var pipe = pipeFactory.Create();
                await pipe.Writer.WriteAsync(buffer);
                pipe.Writer.Complete();
                await pipe.Reader.CopyToEndAsync(stream);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var reader = pipe.Reader.ReadAsync();
                });
                Assert.Equal<byte>(buffer, stream.ToArray());
            }
        }

        [Fact]
        public async Task CopyFromPipeToEndThrow()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new FakeExceptionStream())
            {
                var pipe = pipeFactory.Create();
                await pipe.Writer.WriteAsync(buffer);
                pipe.Writer.Complete();
                await pipe.Reader.CopyToEndAsync(stream);
                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    var reader = await pipe.Reader.ReadAsync();
                });
            }
        }

        [Fact]
        public async Task CopyFromPipeNoEndThrow()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new FakeExceptionStream())
            {
                var pipe = pipeFactory.Create();
                await pipe.Writer.WriteAsync(buffer);
                pipe.Writer.Complete();

                await Assert.ThrowsAsync<IOException>(async () =>
                {
                    await pipe.Reader.CopyToAsync(stream);
                });
            }
        }

        [Fact]
        public async Task CopyFromPipeNoEnd()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new MemoryStream())
            {
                var pipe = pipeFactory.Create();
                await pipe.Writer.WriteAsync(buffer);
                await pipe.Reader.CopyToAsync(stream);
                var reader = pipe.Reader.ReadAsync();

                Assert.False(reader.IsCompleted);
                Assert.Equal<byte>(buffer, stream.ToArray());
            }
        }

        [Fact]
        public async Task CopyFromStreamToEnd()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new MemoryStream(buffer))
            {
                var pipe = pipeFactory.Create();
                await stream.CopyToEndAsync(pipe.Writer);
                var reader = await pipe.Reader.ReadAsync();

                Assert.True(reader.IsCompleted);
                Assert.Equal<byte>(buffer, reader.Buffer.ToArray());
                pipe.Reader.Advance(reader.Buffer.End);
            }
        }

        [Fact]
        public async Task CopyFromStreamToEndThrow()
        {
            using (var pipeFactory = new PipeFactory())
            using (var stream = new FakeExceptionStream())
            {
                var pipe = pipeFactory.Create();
                await stream.CopyToEndAsync(pipe.Writer);

                await Assert.ThrowsAsync<IOException>(async () =>
                {
                    var reader = await pipe.Reader.ReadAsync();
                });
            }
        }

        [Fact]
        public async Task CopyFromStreamNoEndThrow()
        {
            using (var pipeFactory = new PipeFactory())
            using (var stream = new FakeExceptionStream())
            {
                var pipe = pipeFactory.Create();
                await Assert.ThrowsAsync<IOException>(async () =>
                {
                    await stream.CopyToAsync(pipe.Writer);
                });
            }
        }

        [Fact]
        public async Task CopyFromStreamNoEnd()
        {
            var buffer = new byte[1000];
            (new Random()).NextBytes(buffer);
            using (var pipeFactory = new PipeFactory())
            using (var stream = new MemoryStream(buffer))
            {
                var pipe = pipeFactory.Create();
                await stream.CopyToAsync(pipe.Writer);
                var reader = await pipe.Reader.ReadAsync();

                Assert.False(reader.IsCompleted);
                Assert.Equal<byte>(buffer, reader.Buffer.ToArray());
                pipe.Reader.Advance(reader.Buffer.End);
            }
        }

        private class FakeExceptionStream : MemoryStream
        {
            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return Task.FromException<int>(new IOException());
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return Task.FromException<int>(new IOException());
            }
        }
    }
}
