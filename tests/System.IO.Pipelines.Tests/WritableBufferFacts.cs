// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class WritableBufferFacts
    {
        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(500)]
        [InlineData(5000)]
        [InlineData(50000)]
        public async Task WriteLargeDataBinary(int length)
        {
            var data = new byte[length];
            new Random(length).NextBytes(data);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));

                PipeWriter output = pipe.Writer;
                output.Write(data);
                await output.FlushAsync();
                pipe.Writer.Complete();

                long offset = 0;
                while (true)
                {
                    ReadResult result = await pipe.Reader.ReadAsync();
                    ReadOnlyBuffer<byte> input = result.Buffer;
                    if (input.Length == 0) break;
                    // We are able to cast because test arguments are in range of int
                    Assert.Equal(new Span<byte>(data, (int)offset, (int)input.Length).ToArray(), input.ToArray());
                    offset += input.Length;
                    pipe.Reader.AdvanceTo(input.End);
                }

                Assert.Equal(data.Length, offset);
            }
        }

        [Fact]
        public async Task CanWriteNothingToBuffer()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                PipeWriter buffer = pipe.Writer;
                buffer.GetMemory(0);
                buffer.Advance(0); // doing nothing, the hard way
                await buffer.FlushAsync();
                pipe.Reader.Complete();
                pipe.Writer.Complete();
            }
        }

        [Fact]
        public void EmptyWriteDoesNotThrow()
        {
            using (var pool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(pool));
                PipeWriter buffer = pipe.Writer;
                buffer.Write(new byte[0]);
                pipe.Reader.Complete();
                pipe.Writer.Complete();
            }
        }

        [Fact]
        public void EnsureMoreThanPoolBlockSizeThrows()
        {
            using (var pool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(pool));
                PipeWriter buffer = pipe.Writer;
                Assert.Throws<ArgumentOutOfRangeException>(() => buffer.GetMemory(8192));
            }
        }

        [Fact]
        public void ThrowsOnAdvanceOverMemorySize()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                Memory<byte> buffer = pipe.Writer.GetMemory(1);
                var exception = Assert.Throws<InvalidOperationException>(() => pipe.Writer.Advance(buffer.Length + 1));
                Assert.Equal("Can't advance past buffer size", exception.Message);
                pipe.Reader.Complete();
                pipe.Writer.Complete();
            }
        }

        [Fact]
        public void ThrowsOnAdvanceWithNoMemory()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                PipeWriter buffer = pipe.Writer;
                var exception = Assert.Throws<InvalidOperationException>(() => buffer.Advance(1));
                Assert.Equal("No writing operation. Make sure GetMemory() was called.", exception.Message);
            }
        }
    }
}
