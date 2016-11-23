using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelineReaderWriterFacts
    {
        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytesWhenOverflowingSegments()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                // Fill the block with stuff leaving 5 bytes at the end
                var buffer = readerWriter.Alloc(1);

                var len = buffer.Memory.Length;
                // Fill the buffer with garbage
                //     block 1       ->    block2
                // [padding..hello]  ->  [  world   ]
                var paddingBytes = Enumerable.Repeat((byte)'a', len - 5).ToArray();
                buffer.Write(paddingBytes);
                await buffer.FlushAsync();

                // Write 10 and flush
                buffer = readerWriter.Alloc();
                buffer.WriteLittleEndian(10);

                // Write 9
                buffer.WriteLittleEndian(9);

                // Write 8
                buffer.WriteLittleEndian(8);

                // Make sure we don't see it yet
                var result = await readerWriter.ReadAsync();
                var reader = result.Buffer;

                Assert.Equal(len - 5, reader.Length);

                // Don't move
                readerWriter.Advance(reader.End);

                // Now flush
                await buffer.FlushAsync();

                reader = (await readerWriter.ReadAsync()).Buffer;

                Assert.Equal(12, reader.Length);
                Assert.Equal(10, reader.ReadLittleEndian<int>());
                Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
                Assert.Equal(8, reader.Slice(8).ReadLittleEndian<int>());
            }
        }

        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytes()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                // Write 10 and flush
                var buffer = readerWriter.Alloc();
                buffer.WriteLittleEndian(10);
                await buffer.FlushAsync();

                // Write 9
                buffer = readerWriter.Alloc();
                buffer.WriteLittleEndian(9);

                // Write 8
                buffer.WriteLittleEndian(8);

                // Make sure we don't see it yet
                var result = await readerWriter.ReadAsync();
                var reader = result.Buffer;

                Assert.Equal(4, reader.Length);
                Assert.Equal(10, reader.ReadLittleEndian<int>());

                // Don't move
                readerWriter.Advance(reader.Start);

                // Now flush
                await buffer.FlushAsync();

                reader = (await readerWriter.ReadAsync()).Buffer;

                Assert.Equal(12, reader.Length);
                Assert.Equal(10, reader.ReadLittleEndian<int>());
                Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
                Assert.Equal(8, reader.Slice(8).ReadLittleEndian<int>());
            }
        }

        [Fact]
        public async Task ReaderShouldNotGetUnflushedBytesWithAppend()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                // Write 10 and flush
                var buffer = readerWriter.Alloc();
                buffer.WriteLittleEndian(10);
                await buffer.FlushAsync();

                // Write Hello to another pipeline and get the buffer
                var bytes = Encoding.ASCII.GetBytes("Hello");

                var c2 = factory.Create();
                await c2.WriteAsync(bytes);
                var result = await c2.ReadAsync();
                var c2Buffer = result.Buffer;

                Assert.Equal(bytes.Length, c2Buffer.Length);

                // Write 9 to the buffer
                buffer = readerWriter.Alloc();
                buffer.WriteLittleEndian(9);

                // Append the data from the other pipeline
                buffer.Append(c2Buffer);

                // Mark it as consumed
                c2.Advance(c2Buffer.End);

                // Now read and make sure we only see the comitted data
                result = await readerWriter.ReadAsync();
                var reader = result.Buffer;

                Assert.Equal(4, reader.Length);
                Assert.Equal(10, reader.ReadLittleEndian<int>());

                // Consume nothing
                readerWriter.Advance(reader.Start);

                // Flush the second set of writes
                await buffer.FlushAsync();

                reader = (await readerWriter.ReadAsync()).Buffer;

                // int, int, "Hello"
                Assert.Equal(13, reader.Length);
                Assert.Equal(10, reader.ReadLittleEndian<int>());
                Assert.Equal(9, reader.Slice(4).ReadLittleEndian<int>());
                Assert.Equal("Hello", reader.Slice(8).GetUtf8String());
            }
        }

        [Fact]
        public async Task WritingDataMakesDataReadableViaPipeline()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var bytes = Encoding.ASCII.GetBytes("Hello World");

                await readerWriter.WriteAsync(bytes);
                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;

                Assert.Equal(11, buffer.Length);
                Assert.True(buffer.IsSingleSpan);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
            }
        }

        [Fact]
        public async Task AdvanceShouldResetStateIfReadCancelled()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                readerWriter.CancelPendingRead();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                readerWriter.Advance(buffer.End);

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.True(buffer.IsEmpty);

                var awaitable = readerWriter.ReadAsync();
                Assert.False(awaitable.IsCompleted);
            }
        }

        [Fact]
        public async Task CancellingPendingReadBeforeReadAsync()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                readerWriter.CancelPendingRead();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                readerWriter.Advance(buffer.End);

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.True(buffer.IsEmpty);

                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var output = readerWriter.Alloc();
                output.Write(bytes);
                await output.FlushAsync();

                result = await readerWriter.ReadAsync();
                buffer = result.Buffer;

                Assert.Equal(11, buffer.Length);
                Assert.False(result.IsCancelled);
                Assert.True(buffer.IsSingleSpan);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));
            }
        }

        [Fact]
        public async Task CancellingPendingAfterReadAsync()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var output = readerWriter.Alloc();
                output.Write(bytes);

                var task = Task.Run(async () =>
                {
                    var result = await readerWriter.ReadAsync();
                    var buffer = result.Buffer;
                    readerWriter.Advance(buffer.End);

                    Assert.False(result.IsCompleted);
                    Assert.True(result.IsCancelled);
                    Assert.True(buffer.IsEmpty);

                    await output.FlushAsync();

                    result = await readerWriter.ReadAsync();
                    buffer = result.Buffer;

                    Assert.Equal(11, buffer.Length);
                    Assert.True(buffer.IsSingleSpan);
                    Assert.False(result.IsCancelled);
                    var array = new byte[11];
                    buffer.First.Span.CopyTo(array);
                    Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

                    readerWriter.CompleteReader();
                });

                // Wait until reading starts to cancel the pending read
                await readerWriter.ReadingStarted;

                readerWriter.CancelPendingRead();

                await task;

                readerWriter.CompleteWriter();
            }
        }

        [Fact]
        public async Task WriteAndCancellingPendingReadBeforeReadAsync()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var output = readerWriter.Alloc();
                output.Write(bytes);
                await output.FlushAsync();

                readerWriter.CancelPendingRead();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;

                Assert.False(result.IsCompleted);
                Assert.True(result.IsCancelled);
                Assert.False(buffer.IsEmpty);
                Assert.Equal(11, buffer.Length);
                Assert.True(buffer.IsSingleSpan);
                var array = new byte[11];
                buffer.First.Span.CopyTo(array);
                Assert.Equal("Hello World", Encoding.ASCII.GetString(array));

                readerWriter.CompleteWriter();
                readerWriter.CompleteReader();
            }
        }

        [Fact]
        public async Task ReadingCanBeCancelled()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var cts = new CancellationTokenSource();
                cts.Token.Register(() =>
                {
                    readerWriter.CompleteWriter(new OperationCanceledException(cts.Token));
                });

                var ignore = Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    cts.Cancel();
                });

                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                {
                    var result = await readerWriter.ReadAsync();
                    var buffer = result.Buffer;
                });
            }
        }

        [Fact]
        public async Task HelloWorldAcrossTwoBlocks()
        {
            const int blockSize = 4032;
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                //     block 1       ->    block2
                // [padding..hello]  ->  [  world   ]
                var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var writeBuffer = readerWriter.Alloc();
                writeBuffer.Write(paddingBytes);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                Assert.False(buffer.IsSingleSpan);
                var helloBuffer = buffer.Slice(blockSize - 5);
                Assert.False(helloBuffer.IsSingleSpan);
                var memory = new List<Memory<byte>>();
                foreach (var m in helloBuffer)
                {
                    memory.Add(m);
                }
                var spans = memory;
                Assert.Equal(2, memory.Count);
                var helloBytes = new byte[spans[0].Length];
                spans[0].Span.CopyTo(helloBytes);
                var worldBytes = new byte[spans[1].Length];
                spans[1].Span.CopyTo(worldBytes);
                Assert.Equal("Hello", Encoding.ASCII.GetString(helloBytes));
                Assert.Equal(" World", Encoding.ASCII.GetString(worldBytes));
            }
        }

        [Fact]
        public async Task IndexOfNotFoundReturnsEnd()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var bytes = Encoding.ASCII.GetBytes("Hello World");

                await readerWriter.WriteAsync(bytes);
                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                ReadableBuffer slice;
                ReadCursor cursor;

                Assert.False(buffer.TrySliceTo(10, out slice, out cursor));
            }
        }

        [Fact]
        public async Task FastPathIndexOfAcrossBlocks()
        {
            var vecUpperR = new Vector<byte>((byte)'R');

            const int blockSize = 4032;
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                //     block 1       ->    block2
                // [padding..hello]  ->  [  world   ]
                var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var writeBuffer = readerWriter.Alloc();
                writeBuffer.Write(paddingBytes);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                ReadableBuffer slice;
                ReadCursor cursor;
                Assert.False(buffer.TrySliceTo((byte)'R', out slice, out cursor));
            }
        }

        [Fact]
        public async Task SlowPathIndexOfAcrossBlocks()
        {
            const int blockSize = 4032;
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                //     block 1       ->    block2
                // [padding..hello]  ->  [  world   ]
                var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
                var bytes = Encoding.ASCII.GetBytes("Hello World");
                var writeBuffer = readerWriter.Alloc();
                writeBuffer.Write(paddingBytes);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
                var buffer = result.Buffer;
                ReadableBuffer slice;
                ReadCursor cursor;
                Assert.False(buffer.IsSingleSpan);
                Assert.True(buffer.TrySliceTo((byte)' ', out slice, out cursor));

                slice = buffer.Slice(cursor).Slice(1);
                var array = slice.ToArray();

                Assert.Equal("World", Encoding.ASCII.GetString(array));
            }
        }

        [Fact]
        public void AllocMoreThanPoolBlockSizeThrows()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                Assert.Throws<ArgumentOutOfRangeException>(() => readerWriter.Alloc(8192));
            }
        }

        [Fact]
        public void ReadingStartedCompletesOnCompleteReader()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                readerWriter.CompleteReader();

                Assert.True(readerWriter.ReadingStarted.IsCompleted);
            }
        }

        [Fact]
        public void ReadingStartedCompletesOnCallToReadAsync()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                readerWriter.ReadAsync();

                Assert.True(readerWriter.ReadingStarted.IsCompleted);
            }
        }
    }
}
