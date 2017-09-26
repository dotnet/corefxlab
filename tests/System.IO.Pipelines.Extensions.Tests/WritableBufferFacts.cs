// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;

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
        public async Task WriteLargeDataTextUtf8(int length)
        {
            string data = new string('#', length);
            FillRandomStringData(data, length);
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));

                var output = pipe.Writer.Alloc();
                output.AsOutput().Append(data, SymbolTable.InvariantUtf8);
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
                    // We are able to cast because test arguments are in range of int
                    Assert.Equal(data.Substring((int)offset, (int)input.Length), s);
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
                var pipe = new Pipe(new PipeOptions(memoryPool));

                var output = pipe.Writer.Alloc();
                output.AsOutput().Append(data, SymbolTable.InvariantUtf8);
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
                    // We are able to cast because test arguments are in range of int
                    Assert.Equal(data.Substring((int)offset, (int)input.Length), s);
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
                var pipe = new Pipe(new PipeOptions(memoryPool));
                var buffer = pipe.Writer.Alloc();
                buffer.AsOutput().Append(value, SymbolTable.InvariantUtf8);
                await buffer.FlushAsync();

                var result = await pipe.Reader.ReadAsync();
                var inputBuffer = result.Buffer;

                Assert.Equal(valueAsString, inputBuffer.GetUtf8String());
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
        public async Task WriteHex(int value, string hex)
        {
            using (var memoryPool = new MemoryPool())
            {
                var pipe = new Pipe(new PipeOptions(memoryPool));
                var buffer = pipe.Writer.Alloc();
                buffer.AsOutput().Append(value, SymbolTable.InvariantUtf8, 'x');
                await buffer.FlushAsync();
                var result = await pipe.Reader.ReadAsync();

                Assert.Equal(hex, result.Buffer.GetAsciiString());
                pipe.Reader.Advance(result.Buffer.End);
            }
        }

    }
}
