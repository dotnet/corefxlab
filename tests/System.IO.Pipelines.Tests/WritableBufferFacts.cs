using System;
using System.Collections;
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
                var readerWriter = new PipelineReaderWriter(memoryPool);
                var buffer = readerWriter.Alloc();
                buffer.Advance(0); // doing nothing, the hard way
                await buffer.FlushAsync();
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
                var readerWriter = new PipelineReaderWriter(memoryPool);
                var buffer = readerWriter.Alloc();
                buffer.Append(value, EncodingData.InvariantUtf8);
                await buffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
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
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();
                output.Write(data);
                var foo = output.Memory.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                readerWriter.CompleteWriter();

                int offset = 0;
                while (true)
                {
                    var result = await readerWriter.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;

                    Assert.True(input.Equals(new Span<byte>(data, offset, input.Length)));
                    offset += input.Length;
                    readerWriter.Advance(input.End);
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
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();
                output.Append(data, EncodingData.TextEncoding.Utf8);
                var foo = output.Memory.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                readerWriter.CompleteWriter();

                int offset = 0;
                while (true)
                {
                    var result = await readerWriter.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;

                    string s = ReadableBufferExtensions.GetUtf8String(input);
                    Assert.Equal(data.Substring(offset, input.Length), s);
                    offset += input.Length;
                    readerWriter.Advance(input.End);
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
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();
                output.Append(data, EncodingData.TextEncoding.Utf8);
                var foo = output.Memory.IsEmpty; // trying to see if .Memory breaks
                await output.FlushAsync();
                readerWriter.CompleteWriter();

                int offset = 0;
                while (true)
                {
                    var result = await readerWriter.ReadAsync();
                    var input = result.Buffer;
                    if (input.Length == 0) break;

                    string s = ReadableBufferExtensions.GetAsciiString(input);
                    Assert.Equal(data.Substring(offset, input.Length), s);
                    offset += input.Length;
                    readerWriter.Advance(input.End);
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
                var readerWriter = new PipelineReaderWriter(memoryPool);
                var output = readerWriter.Alloc();

                Assert.True(output.AsReadableBuffer().IsEmpty);
                Assert.Equal(0, output.AsReadableBuffer().Length);


                output.Append("hello world", EncodingData.TextEncoding.Utf8);
                var readable = output.AsReadableBuffer();

                // check that looks about right
                Assert.False(readable.IsEmpty);
                Assert.Equal(11, readable.Length);
                Assert.True(readable.Equals(Encoding.UTF8.GetBytes("hello world")));
                Assert.True(readable.Slice(1, 3).Equals(Encoding.UTF8.GetBytes("ell")));

                // check it all works after we write more
                output.Append("more data", EncodingData.TextEncoding.Utf8);

                // note that the snapshotted readable should not have changed by this
                Assert.False(readable.IsEmpty);
                Assert.Equal(11, readable.Length);
                Assert.True(readable.Equals(Encoding.UTF8.GetBytes("hello world")));
                Assert.True(readable.Slice(1, 3).Equals(Encoding.UTF8.GetBytes("ell")));

                // if we fetch it again, we can see everything
                readable = output.AsReadableBuffer();
                Assert.False(readable.IsEmpty);
                Assert.Equal(20, readable.Length);
                Assert.True(readable.Equals(Encoding.UTF8.GetBytes("hello worldmore data")));
            }
        }

        [Fact]
        public void CanReReadDataThatHasNotBeenCommitted_LargeData()
        {
            using (var memoryPool = new MemoryPool())
            {
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();

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
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();
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
                var readerWriter = new PipelineReaderWriter(memoryPool);

                var output = readerWriter.Alloc();

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
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var buffer = readerWriter.Alloc();
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
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var buffer = readerWriter.Alloc();
                buffer.Append(value, EncodingData.InvariantUtf8, 'x');

                Assert.Equal(hex, buffer.AsReadableBuffer().GetAsciiString());
            }
        }
    }
}
