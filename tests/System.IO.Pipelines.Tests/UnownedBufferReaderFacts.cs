// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class UnownedBufferReaderFacts
    {
        [Fact]
        public async Task CanConsumeData()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello");
                await sw.FlushAsync();
                await sw.WriteAsync("World");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            int calls = 0;

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                calls++;
                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                var segment = buffer.ToArray();

                var data = Encoding.UTF8.GetString(segment);
                if (calls == 1)
                {
                    Assert.Equal("Hello", data);
                }
                else
                {
                    Assert.Equal("World", data);
                }

                reader.Advance(buffer.End);
            }
        }

        [Fact]
        public async Task CanCancelConsumingData()
        {
            var cts = new CancellationTokenSource();
            var stream = new CallbackStream(async (s, token) =>
            {
                var hello = Encoding.UTF8.GetBytes("Hello");
                var world = Encoding.UTF8.GetBytes("World");
                await s.WriteAsync(hello, 0, hello.Length, token);
                cts.Cancel();
                await s.WriteAsync(world, 0, world.Length, token);
            });

            var reader = stream.AsPipelineReader(cts.Token);

            int calls = 0;

            while (true)
            {
                ReadResult result;
                ReadableBuffer buffer;
                try
                {
                    result = await reader.ReadAsync();
                    buffer = result.Buffer;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                finally
                {
                    calls++;
                }
                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                var segment = buffer.ToArray();

                var data = Encoding.UTF8.GetString(segment);
                Assert.Equal("Hello", data);

                reader.Advance(buffer.End);
            }

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task CancellingPendingReadBeforeReadAsync()
        {
            var tcs = new TaskCompletionSource<object>();
            var stream = new CallbackStream(async (s, token) =>
            {
                await tcs.Task;
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                await s.WriteAsync(bytes, 0, bytes.Length);
            });

            var reader = stream.AsPipelineReader();

            reader.CancelPendingRead();

            var result = await reader.ReadAsync();
            var buffer = result.Buffer;
            reader.Advance(buffer.End);

            Assert.False(result.IsCompleted);
            Assert.True(result.IsCancelled);
            Assert.True(buffer.IsEmpty);

            tcs.TrySetResult(null);

            result = await reader.ReadAsync();
            buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
        }

        [Fact]
        public async Task CancellingBeforeAdvance()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                await s.WriteAsync(bytes, 0, bytes.Length);
            });

            var reader = stream.AsPipelineReader();

            var result = await reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.Equal(11, buffer.Length);
            Assert.False(result.IsCancelled);
            Assert.True(buffer.IsSingleSpan);
            var array = new byte[11];
            buffer.First.Span.CopyTo(array);
            Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

            reader.CancelPendingRead();

            reader.Advance(buffer.End);

            var awaitable = reader.ReadAsync();

            Assert.True(awaitable.IsCompleted);

            result = await awaitable;

            Assert.True(result.IsCancelled);
        }

        [Fact]
        public async Task CancellingPendingAfterReadAsync()
        {
            var tcs = new TaskCompletionSource<object>();
            var stream = new CallbackStream(async (s, token) =>
            {
                await tcs.Task;
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                await s.WriteAsync(bytes, 0, bytes.Length);
            });

            var reader = stream.AsPipelineReader();

            var task = Task.Run(async () =>
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                reader.Advance(buffer.End);

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.True(buffer.IsEmpty);

                tcs.TrySetResult(null);

                result = await reader.ReadAsync();
                buffer = result.Buffer;

                Assert.Equal(11, buffer.Length);
                Assert.False(result.IsCancelled);
                Assert.True(buffer.IsSingleSpan);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
            });

            await ((UnownedBufferReader)reader).ReadingStarted;

            reader.CancelPendingRead();

            await task;
        }

        [Fact]
        public async Task CanConsumeLessDataThanProduced()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello ");
                await sw.FlushAsync();
                await sw.WriteAsync("World");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            int index = 0;
            var message = "Hello World";

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                var ch = (char)buffer.First.Span[0];
                Assert.Equal(message[index++], ch);
                reader.Advance(buffer.Start.Seek(1, buffer.End));
            }

            Assert.Equal(message.Length, index);
        }

        [Fact]
        public async Task AccessingUnownedMemoryThrowsIfUsedAfterAdvance()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello ");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            var data = Buffer<byte>.Empty;

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }
                data = buffer.First;
                reader.Advance(buffer.End);
            }

            EnsureSpanDisposed(data);
        }

        [Fact]
        public async Task PreservingUnownedBufferCopies()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello ");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            var preserved = default(PreservedBuffer);

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                preserved = buffer.Preserve();

                // Make sure we can acccess the span
                EnsureSpanValid(buffer.First);

                reader.Advance(buffer.End);
            }

            using (preserved)
            {
                Assert.Equal("Hello ", Encoding.UTF8.GetString(preserved.Buffer.ToArray()));
            }

            EnsureSpanDisposed(preserved.Buffer.First);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        static Span<byte> EnsureSpanValid(Buffer<byte> buffer)
        {
            return buffer.Span;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        static void EnsureSpanDisposed(Buffer<byte> buffer)
        {
            try
            {
                var temp = buffer.Span;
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ObjectDisposedException);
            }
        }

        [Fact]
        public async Task CanConsumeLessDataThanProducedAndPreservingOwnedBuffers()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello ");
                await sw.FlushAsync();
                await sw.WriteAsync("World");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            int index = 0;
            var message = "Hello World";

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                using (buffer.Preserve())
                {
                    var ch = (char)buffer.First.Span[0];
                    Assert.Equal(message[index++], ch);
                    reader.Advance(buffer.Start.Seek(1, buffer.End), buffer.End);
                }
            }

            Assert.Equal(message.Length, index);
        }

        [Fact]
        public async Task CanConsumeLessDataThanProducedAndPreservingUnOwnedBuffers()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello ");
                await sw.FlushAsync();
                await sw.WriteAsync("World");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            int index = 0;
            var message = "Hello World";

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                using (buffer.Preserve())
                {
                    var ch = (char)buffer.First.Span[0];
                    Assert.Equal(message[index++], ch);
                    reader.Advance(buffer.Start.Seek(1, buffer.End));
                }
            }

            Assert.Equal(message.Length, index);
        }

        [Fact]
        public async Task CanConsumeLessDataThanProducedWithBufferReuse()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var data = new byte[4096];
                Encoding.UTF8.GetBytes("Hello ", 0, 6, data, 0);
                await s.WriteAsync(data, 0, 6);
                Encoding.UTF8.GetBytes("World", 0, 5, data, 0);
                await s.WriteAsync(data, 0, 5);
            });

            var reader = stream.AsPipelineReader();

            int index = 0;
            var message = "Hello World";

            while (index <= message.Length)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                var ch = Encoding.UTF8.GetString(buffer.Slice(0, index).ToArray());
                Assert.Equal(message.Substring(0, index), ch);

                // Never consume, to force buffers to be copied
                reader.Advance(buffer.Start, buffer.Start.Seek(index, buffer.End));

                // Yield the task. This will ensure that we don't have any Tasks idling
                // around in UnownedBufferReader.OnCompleted
                await Task.Yield();

                index++;
            }

            Assert.Equal(message.Length + 1, index);
        }

        [Fact]
        public async Task NotCallingAdvanceWillCauseReadToThrow()
        {
            var stream = new CallbackStream(async (s, token) =>
            {
                var sw = new StreamWriter(s);
                await sw.WriteAsync("Hello");
                await sw.FlushAsync();
                await sw.WriteAsync("World");
                await sw.FlushAsync();
            });

            var reader = stream.AsPipelineReader();

            int calls = 0;

            InvalidOperationException thrown = null;
            while (true)
            {
                ReadResult result;
                ReadableBuffer buffer;
                try
                {
                    result = await reader.ReadAsync();
                    buffer = result.Buffer;
                }
                catch (InvalidOperationException ex)
                {
                    thrown = ex;
                    break;
                }

                calls++;
                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                var segment = buffer.ToArray();

                var data = Encoding.UTF8.GetString(segment);
                if (calls == 1)
                {
                    Assert.Equal("Hello", data);
                }
                else
                {
                    Assert.Equal("World", data);
                }
            }
            Assert.Equal(1, calls);
            Assert.NotNull(thrown);
            Assert.Equal("Cannot Read until the previous read has been acknowledged by calling Advance", thrown.Message);
        }

        [Fact]
        public async Task StreamAsPipelineReaderUsesUnderlyingPipelineReaderIfAvailable()
        {
            var stream = new StreamAndPipeReader();
            var sw = new StreamWriter(stream);
            sw.Write("Hello");
            sw.Flush();
            stream.FinishWriting();

            var reader = stream.AsPipelineReader();

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                if (buffer.IsEmpty && result.IsCompleted)
                {
                    // Done
                    break;
                }

                var segment = buffer.ToArray();

                var data = Encoding.UTF8.GetString(segment);
                Assert.Equal("Hello", data);
                reader.Advance(buffer.End);
            }

        }

        [Fact]
        public async Task StreamAsPipelineReaderReadStream()
        {
            var stream = new StreamAndPipeReader();
            var sw = new StreamWriter(stream);
            sw.Write("Hello");
            sw.Flush();

            var reader = stream.AsPipelineReader();
            var result = await reader.ReadAsync();
            var buffer = result.Buffer;
            var segment = buffer.ToArray();
            var data = Encoding.UTF8.GetString(segment);
            Assert.Equal("Hello", data);
            reader.Advance(buffer.End);

            sw.Write("World");
            sw.Flush();
            stream.FinishWriting();

            var readBuf = new byte[512];
            int read = await stream.ReadAsync(readBuf, 0, readBuf.Length);
            Assert.Equal("World", Encoding.UTF8.GetString(readBuf, 0, read));
        }

        private class StreamAndPipeReader : Stream, IPipeReader
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

            public void CancelPendingRead() => _pipe.Reader.CancelPendingRead();

            public void Advance(ReadCursor consumed, ReadCursor examined)
            {
                _pipe.Reader.Advance(consumed, examined);
            }

            public void Complete(Exception exception = null)
            {
                _pipe.Reader.Complete(exception);
            }

            public override void Flush()
            {

            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _pipe.ReadAsync(new ArraySegment<byte>(buffer, offset, count)).GetAwaiter().GetResult();
            }

            public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return await _pipe.ReadAsync(new ArraySegment<byte>(buffer, offset, count));
            }

            public ReadableBufferAwaitable ReadAsync(CancellationToken cancellationToken = default)
            {
                return _pipe.Reader.ReadAsync();
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

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return _pipe.WriteAsync(new ArraySegment<byte>(buffer, offset, count));
            }

            public void FinishWriting() => _pipe.Writer.Complete();

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _pipe.Reader.Complete();
                _pipe.Writer.Complete();
            }

            public bool TryRead(out ReadResult result) => _pipe.Reader.TryRead(out result);
        }

        private class CallbackStream : Stream
        {
            private readonly Func<Stream, CancellationToken, Task> _callback;
            public CallbackStream(Func<Stream, CancellationToken, Task> callback)
            {
                _callback = callback;
            }

            public override bool CanRead
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanSeek
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            {
                return _callback(destination, cancellationToken);
            }
        }
    }
}
