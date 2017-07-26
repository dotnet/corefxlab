// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;
using System.Text.Formatting;

namespace System.IO.Pipelines.Tests
{
    public class WritableBufferFacts
    {
        [Fact]
        public async Task CanWriteNothingToBuffer()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);
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
                var pipe = new Pipe(memoryPool);
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
                var pipe = new Pipe(memoryPool);
                var buffer = pipe.Writer.Alloc(1);
                var exception = Assert.Throws<InvalidOperationException>(() => buffer.Advance(buffer.Buffer.Length + 1));
                Assert.Equal("Can't advance past buffer size", exception.Message);
            }
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(20, "20")]
        [InlineData(300, "300")]
        [InlineData(4000, "4000")]
        [InlineData(500000, "500000")]
        [InlineData(60000000000000000, "60000000000000000")]
        public async Task CanWriteUInt64ToBuffer(ulong value, string valueAsString)
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);
                var buffer = pipe.Writer.Alloc();
                buffer.Append(value, SymbolTable.InvariantUtf8);
                await buffer.FlushAsync();

                var result = await pipe.Reader.ReadAsync();
                var inputBuffer = result.Buffer;

                Assert.Equal(valueAsString, inputBuffer.GetUtf8String());
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
                var pipe = new Pipe(memoryPool);

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
                    // Sizes less then max int
                    Assert.True(input.EqualsTo(new Span<byte>(data, (int) offset, (int) input.Length)));
                    offset += input.Length;
                    pipe.Advance(input.End);
                }
                Assert.Equal(data.Length, offset);
            }
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(500)]
        [InlineData(5000)]
        [InlineData(50000)]
        public async Task WriteLargeDataTextUtf8(int length)
        {
            string data = new string('#', length);
            FillRandomStringData(data, length);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);

                var output = pipe.Writer.Alloc();
                output.Append(data, SymbolTable.InvariantUtf8);
                var foo = output.Buffer.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                pipe.Writer.Complete();

                long offset = 0;
                while (true)
                {
                    var result = await pipe.Reader.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;

                    string s = ReadableBufferExtensions.GetUtf8String(input);
                    // Sizes less then max int
                    Assert.Equal(data.Substring((int) offset, (int) input.Length), s);
                    offset += input.Length;
                    pipe.Advance(input.End);
                }
                Assert.Equal(data.Length, offset);
            }
        }
        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(500)]
        [InlineData(5000)]
        [InlineData(50000)]
        public async Task WriteLargeDataTextAscii(int length)
        {
            string data = new string('#', length);
            FillRandomStringData(data, length);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);

                var output = pipe.Writer.Alloc();
                output.Append(data, SymbolTable.InvariantUtf8);
                var foo = output.Buffer.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                pipe.Writer.Complete();

                long offset = 0;
                while (true)
                {
                    var result = await pipe.Reader.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;

                    string s = ReadableBufferExtensions.GetAsciiString(input);
                    Assert.Equal(data.Substring((int) offset, (int) input.Length), s);
                    offset += input.Length;
                    pipe.Advance(input.End);
                }
                Assert.Equal(data.Length, offset);
            }
        }

        private unsafe void FillRandomStringData(string data, int seed)
        {
            Random rand = new Random(seed);
            fixed (char* c = data)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    c[i] = (char)(rand.Next(127) + 1); // want range 1-127
                }
            }
        }


        [Fact]
        public void CanReReadDataThatHasNotBeenCommitted_SmallData()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);
                var output = pipe.Writer.Alloc();

                Assert.True(output.AsReadableBuffer().IsEmpty);
                Assert.Equal(0, output.AsReadableBuffer().Length);


                output.Append("hello world", SymbolTable.InvariantUtf8);
                var readable = output.AsReadableBuffer();

                // check that looks about right
                Assert.False(readable.IsEmpty);
                Assert.Equal(11, readable.Length);
                Assert.True(readable.EqualsTo(Encoding.UTF8.GetBytes("hello world")));
                Assert.True(readable.Slice(1, 3).EqualsTo(Encoding.UTF8.GetBytes("ell")));

                // check it all works after we write more
                output.Append("more data", SymbolTable.InvariantUtf8);

                // note that the snapshotted readable should not have changed by this
                Assert.False(readable.IsEmpty);
                Assert.Equal(11, readable.Length);
                Assert.True(readable.EqualsTo(Encoding.UTF8.GetBytes("hello world")));
                Assert.True(readable.Slice(1, 3).EqualsTo(Encoding.UTF8.GetBytes("ell")));

                // if we fetch it again, we can see everything
                readable = output.AsReadableBuffer();
                Assert.False(readable.IsEmpty);
                Assert.Equal(20, readable.Length);
                Assert.True(readable.EqualsTo(Encoding.UTF8.GetBytes("hello worldmore data")));
            }
        }

        [Fact]
        public void CanReReadDataThatHasNotBeenCommitted_LargeData()
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);

                var output = pipe.Writer.Alloc();

                byte[] predictablyGibberish = new byte[512];
                const int SEED = 1235412;
                Random random = new Random(SEED);
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < predictablyGibberish.Length; j++)
                    {
                        // doing it this way to be 100% sure about repeating the PRNG order
                        predictablyGibberish[j] = (byte)random.Next(0, 256);
                    }
                    output.Write(predictablyGibberish);
                }

                var readable = output.AsReadableBuffer();
                Assert.False(readable.IsSingleSpan);
                Assert.False(readable.IsEmpty);
                Assert.Equal(50 * 512, readable.Length);

                random = new Random(SEED);
                int correctCount = 0;
                foreach (var memory in readable)
                {
                    var span = memory.Span;
                    for (int i = 0; i < span.Length; i++)
                    {
                        if (span[i] == (byte)random.Next(0, 256)) correctCount++;
                    }
                }
                Assert.Equal(50 * 512, correctCount);
            }
        }

        [Fact]
        public async Task CanAppendSelfWhileEmpty()
        { // not really an expectation; just an accepted caveat
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);

                var output = pipe.Writer.Alloc();
                var readable = output.AsReadableBuffer();
                output.Append(readable);
                Assert.Equal(0, output.AsReadableBuffer().Length);

                await output.FlushAsync();
            }
        }

        [Fact]
        public async Task CanAppendSelfWhileNotEmpty()
        {
            byte[] chunk = new byte[512];
            new Random().NextBytes(chunk);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(memoryPool);

                var output = pipe.Writer.Alloc();

                for (int i = 0; i < 20; i++)
                {
                    output.Write(chunk);
                }
                var readable = output.AsReadableBuffer();
                Assert.Equal(512 * 20, readable.Length);

                output.Append(readable);
                Assert.Equal(512 * 20, readable.Length);

                readable = output.AsReadableBuffer();
                Assert.Equal(2 * 512 * 20, readable.Length);

                await output.FlushAsync();
            }
        }

        [Fact]
        public void EnsureMoreThanPoolBlockSizeThrows()
        {
            using (var factory = new PipeFactory())
            {
                var pipe = factory.Create();
                var buffer = pipe.Writer.Alloc();
                Assert.Throws<ArgumentOutOfRangeException>(() => buffer.Ensure(8192));
            }
        }

        public static IEnumerable<object[]> HexNumbers
        {
            get
            {
                yield return new object[] { 0, "0" };
                for (int i = 1; i < 50; i++)
                {
                    yield return new object[] { i, i.ToString("x2").TrimStart('0') };
                }
            }
        }

        [Theory]
        [MemberData(nameof(HexNumbers))]
        public void WriteHex(int value, string hex)
        {
            using (var factory = new PipeFactory())
            {
                var pipe = factory.Create();
                var buffer = pipe.Writer.Alloc();
                buffer.Append(value, SymbolTable.InvariantUtf8, 'x');

                Assert.Equal(hex, buffer.AsReadableBuffer().GetAsciiString());
            }
        }

        [Fact]
        public void EmptyWriteDoesNotThrow()
        {
            using (var factory = new PipeFactory())
            {
                var pipe = factory.Create();
                var buffer = pipe.Writer.Alloc();
                buffer.Write(new byte[0], 0, 0);
            }
        }
    }
}
