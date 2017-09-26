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
        [Fact]
        public async Task CanWriteNothingToBuffer()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                var buffer = pipe.Writer.Alloc();
                buffer.Advance(0); // doing nothing, the hard way
                await buffer.FlushAsync();
            }
        }

        [Fact]
        public void ThrowsOnAdvanceWithNoMemory()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                var buffer = pipe.Writer.Alloc();
                var exception = Assert.Throws<InvalidOperationException>(() => buffer.Advance(1));
                Assert.Equal("Can't advance without buffer allocated", exception.Message);
            }
        }

        [Fact]
        public void ThrowsOnAdvanceOverMemorySize()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                var buffer = pipe.Writer.Alloc(1);
                var exception = Assert.Throws<InvalidOperationException>(() => buffer.Advance(buffer.Buffer.Length + 1));
                Assert.Equal("Can't advance past buffer size", exception.Message);
            }
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(500)]
        [InlineData(5000)]
        [InlineData(50000)]
        public async Task WriteLargeDataBinary(int length)
        {
            byte[] data = new byte[length];
            new Random(length).NextBytes(data);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));

                var output = pipe.Writer.Alloc();
                output.Write(data);
                var foo = output.Buffer.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                pipe.Writer.Complete();

                long offset = 0;
                while (true)
                {
                    var result = await pipe.Reader.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;
                    // We are able to cast because test arguments are in range of int
                    Assert.Equal(new Span<byte>(data, (int)offset, (int)input.Length).ToArray(), input.ToArray());
                    offset += input.Length;
                    pipe.Advance(input.End);
                }
                Assert.Equal(data.Length, offset);
            }
        }

        [Fact]
        public void EnsureMoreThanPoolBlockSizeThrows()
        {
            using (var pool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(pool));
                var buffer = pipe.Writer.Alloc();
                Assert.Throws<ArgumentOutOfRangeException>(() => buffer.Ensure(8192));
            }
        }

        [Fact]
        public void EmptyWriteDoesNotThrow()
        {
            using (var pool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(pool));
                var buffer = pipe.Writer.Alloc();
                buffer.Write(new byte[0], 0, 0);
            }
        }
    }
}
