// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Binary;
using System.Buffers;
using System.Buffers.Pools;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Globalization;
using System.IO.Pipelines.Testing;
using System.IO.Pipelines.Text.Primitives;
using System.Linq;
using System.Text;
using System.Text.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class ReadableBufferFacts
    {
        const int BlockSize = 4032;

        [Fact]
        public void ReadableBufferSequenceWorks()
        {
            var readable = BufferUtilities.CreateBuffer(new byte[] { 1 }, new byte[] { 2, 2 }, new byte[] { 3, 3, 3 }).AsSequence();
            var position = Position.First;
            ReadOnlyBuffer<byte> memory;
            int spanCount = 0;
            while (readable.TryGet(ref position, out memory))
            {
                spanCount++;
                Assert.Equal(spanCount, memory.Length);
            }
            Assert.Equal(3, spanCount);
        }

        [Fact]
        public void CanUseArrayBasedReadableBuffers()
        {
            var data = Encoding.ASCII.GetBytes("***abc|def|ghijk****"); // note sthe padding here - verifying that it is omitted correctly
            var buffer = ReadableBuffer.Create(data, 3, data.Length - 7);
            Assert.Equal(13, buffer.Length);
            var split = buffer.Split((byte)'|');
            Assert.Equal(3, split.Count());
            using (var iter = split.GetEnumerator())
            {
                Assert.True(iter.MoveNext());
                var current = iter.Current;
                Assert.Equal("abc", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("abc", preserved.Buffer.GetAsciiString());
                }

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("def", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("def", preserved.Buffer.GetAsciiString());
                }

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("ghijk", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("ghijk", preserved.Buffer.GetAsciiString());
                }

                Assert.False(iter.MoveNext());
            }
        }

        [Fact]
        public void CanUseOwnedBufferBasedReadableBuffers()
        {
            var data = Encoding.ASCII.GetBytes("***abc|def|ghijk****"); // note sthe padding here - verifying that it is omitted correctly
            OwnedBuffer<byte> owned = data;
            var buffer = ReadableBuffer.Create(owned, 3, data.Length - 7);
            Assert.Equal(13, buffer.Length);
            var split = buffer.Split((byte)'|');
            Assert.Equal(3, split.Count());
            using (var iter = split.GetEnumerator())
            {
                Assert.True(iter.MoveNext());
                var current = iter.Current;
                Assert.Equal("abc", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("abc", preserved.Buffer.GetAsciiString());
                }

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("def", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("def", preserved.Buffer.GetAsciiString());
                }

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("ghijk", current.GetAsciiString());
                using (var preserved = iter.Current.Preserve())
                {
                    Assert.Equal("ghijk", preserved.Buffer.GetAsciiString());
                }

                Assert.False(iter.MoveNext());
            }
        }

        [Fact]
        public async Task CopyToAsyncNativeMemory()
        {
            using (var factory = new PipeFactory(NativeBufferPool.Shared))
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
                output.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
                await output.FlushAsync();
                var ms = new MemoryStream();
                var result = await readerWriter.Reader.ReadAsync();
                var rb = result.Buffer;
                await rb.CopyToAsync(ms);
                ms.Position = 0;
                Assert.Equal(11, rb.Length);
                Assert.Equal(11, ms.Length);
                Assert.Equal(rb.ToArray(), ms.ToArray());
                Assert.Equal("Hello World", Encoding.ASCII.GetString(ms.ToArray()));
            }
        }

        [Fact]
        public async Task TestIndexOfWorksForAllLocations()
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                const int Size = 5 * BlockSize; // multiple blocks

                // populate with a pile of dummy data
                byte[] data = new byte[512];
                for (int i = 0; i < data.Length; i++) data[i] = 42;
                int totalBytes = 0;
                var writeBuffer = readerWriter.Writer.Alloc();
                for (int i = 0; i < Size / data.Length; i++)
                {
                    writeBuffer.Write(data);
                    totalBytes += data.Length;
                }
                await writeBuffer.FlushAsync();

                // now read it back
                var result = await readerWriter.Reader.ReadAsync();
                var readBuffer = result.Buffer;
                Assert.False(readBuffer.IsSingleSpan);
                Assert.Equal(totalBytes, readBuffer.Length);
                TestIndexOfWorksForAllLocations(ref readBuffer, 42);
            }
        }

        [Fact]
        public async Task EqualsDetectsDeltaForAllLocations()
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();

                // populate with dummy data
                const int DataSize = 10000;
                byte[] data = new byte[DataSize];
                var rand = new Random(12345);
                rand.NextBytes(data);

                var writeBuffer = readerWriter.Writer.Alloc();
                writeBuffer.Write(data);
                await writeBuffer.FlushAsync();

                // now read it back
                var result = await readerWriter.Reader.ReadAsync();
                var readBuffer = result.Buffer;
                Assert.False(readBuffer.IsSingleSpan);
                Assert.Equal(data.Length, readBuffer.Length);

                // check the entire buffer
                EqualsDetectsDeltaForAllLocations(readBuffer, data, 0, data.Length);

                // check the first 32 sub-lengths
                for (int i = 0; i <= 32; i++)
                {
                    var slice = readBuffer.Slice(0, i);
                    EqualsDetectsDeltaForAllLocations(slice, data, 0, i);
                }

                // check the last 32 sub-lengths
                for (int i = 0; i <= 32; i++)
                {
                    var slice = readBuffer.Slice(data.Length - i, i);
                    EqualsDetectsDeltaForAllLocations(slice, data, data.Length - i, i);
                }
            }
        }

        private void EqualsDetectsDeltaForAllLocations(ReadableBuffer slice, byte[] expected, int offset, int length)
        {
            Assert.Equal(length, slice.Length);
            Assert.True(slice.EqualsTo(new Span<byte>(expected, offset, length)));
            // change one byte in buffer, for every position
            for (int i = 0; i < length; i++)
            {
                expected[offset + i] ^= 42;
                Assert.False(slice.EqualsTo(new Span<byte>(expected, offset, length)));
                expected[offset + i] ^= 42;
            }
        }

        [Fact]
        public async Task GetUInt64GivesExpectedValues()
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Writer.Alloc();
                writeBuffer.Ensure(50);
                writeBuffer.Advance(50); // not even going to pretend to write data here - we're going to cheat
                await writeBuffer.FlushAsync(); // by overwriting the buffer in-situ

                // now read it back
                var result = await readerWriter.Reader.ReadAsync();
                var readBuffer = result.Buffer;

                ReadUInt64GivesExpectedValues(ref readBuffer);
            }
        }

        [Theory]
        [InlineData(" hello", "hello")]
        [InlineData("    hello", "hello")]
        [InlineData("\r\n hello", "hello")]
        [InlineData("\rhe  llo", "he  llo")]
        [InlineData("\thell o ", "hell o ")]
        public async Task TrimStartTrimsWhitespaceAtStart(string input, string expected)
        {
            using (var readerWriter = new PipeFactory())
            {
                var connection = readerWriter.Create();

                var writeBuffer = connection.Writer.Alloc();
                var bytes = Encoding.ASCII.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await connection.Reader.ReadAsync();
                var buffer = result.Buffer;
                var trimmed = buffer.TrimStart();
                var outputBytes = trimmed.ToArray();

                Assert.Equal(expected, Encoding.ASCII.GetString(outputBytes));
            }
        }

        [Theory]
        [InlineData("hello ", "hello")]
        [InlineData("hello    ", "hello")]
        [InlineData("hello \r\n", "hello")]
        [InlineData("he  llo\r", "he  llo")]
        [InlineData(" hell o\t", " hell o")]
        public async Task TrimEndTrimsWhitespaceAtEnd(string input, string expected)
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Writer.Alloc();
                var bytes = Encoding.ASCII.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.Reader.ReadAsync();
                var buffer = result.Buffer;
                var trimmed = buffer.TrimEnd();
                var outputBytes = trimmed.ToArray();

                Assert.Equal(expected, Encoding.ASCII.GetString(outputBytes));
            }
        }

        [Theory]
        [InlineData("foo\rbar\r\n", "\r\n", "foo\rbar")]
        [InlineData("foo\rbar\r\n", "\rbar", "foo")]
        [InlineData("/pathpath/", "path/", "/path")]
        [InlineData("hellzhello", "hell", null)]
        public async Task TrySliceToSpan(string input, string sliceTo, string expected)
        {
            var sliceToBytes = Encoding.UTF8.GetBytes(sliceTo);

            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Writer.Alloc();
                var bytes = Encoding.UTF8.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.Reader.ReadAsync();
                var buffer = result.Buffer;
                ReadableBuffer slice;
                ReadCursor cursor;
                Assert.True(buffer.TrySliceTo(sliceToBytes, out slice, out cursor));
                Assert.Equal(expected, slice.GetUtf8String());
            }
        }

        private unsafe void TestIndexOfWorksForAllLocations(ref ReadableBuffer readBuffer, byte emptyValue)
        {
            byte huntValue = (byte)~emptyValue;

            var handles = new List<BufferHandle>();
            // we're going to fully index the final locations of the buffer, so that we
            // can mutate etc in constant time
            var addresses = BuildPointerIndex(ref readBuffer, handles);

            // check it isn't there to start with
            ReadableBuffer slice;
            ReadCursor cursor;
            var found = readBuffer.TrySliceTo(huntValue, out slice, out cursor);
            Assert.False(found);

            // correctness test all values
            for (int i = 0; i < readBuffer.Length; i++)
            {
                *addresses[i] = huntValue;
                found = readBuffer.TrySliceTo(huntValue, out slice, out cursor);
                *addresses[i] = emptyValue;

                Assert.True(found);
                var remaining = readBuffer.Slice(cursor);
                var handle = remaining.First.Retain(pin: true);
                Assert.True(handle.PinnedPointer != null);
                if (i % BlockSize == 0)
                {
                    Assert.True((byte*)handle.PinnedPointer == addresses[i]);
                }
                handle.Dispose();
            }

            // free up memory handles
            foreach (var handle in handles)
            {
                handle.Dispose();
            }
            handles.Clear();
        }

        private static unsafe byte*[] BuildPointerIndex(ref ReadableBuffer readBuffer, List<BufferHandle> handles)
        {

            byte*[] addresses = new byte*[readBuffer.Length];
            int index = 0;
            foreach (var memory in readBuffer)
            {
                var handle = memory.Retain(pin: true);
                handles.Add(handle);
                var ptr = (byte*)handle.PinnedPointer;
                for (int i = 0; i < memory.Length; i++)
                {
                    addresses[index++] = ptr++;
                }
            }
            return addresses;
        }

        private unsafe void ReadUInt64GivesExpectedValues(ref ReadableBuffer readBuffer)
        {
            Assert.True(readBuffer.IsSingleSpan);

            for (ulong i = 0; i < 1024; i++)
            {
                TestValue(ref readBuffer, i);
            }
            TestValue(ref readBuffer, ulong.MinValue);
            TestValue(ref readBuffer, ulong.MaxValue);

            var rand = new Random(41234);
            // low numbers
            for (int i = 0; i < 10000; i++)
            {
                TestValue(ref readBuffer, (ulong)rand.Next());
            }
            // wider range of numbers
            for (int i = 0; i < 10000; i++)
            {
                ulong x = (ulong)rand.Next(), y = (ulong)rand.Next();
                TestValue(ref readBuffer, (x << 32) | y);
                TestValue(ref readBuffer, (y << 32) | x);
            }
        }

        private unsafe void TestValue(ref ReadableBuffer readBuffer, ulong value)
        {
            fixed (byte* ptr = &readBuffer.First.Span.DangerousGetPinnableReference())
            {
                string s = value.ToString(CultureInfo.InvariantCulture);
                int written;
                fixed (char* c = s)
                {
                    // We are able to cast because test arguments are in range of int
                    written = Encoding.ASCII.GetBytes(c, s.Length, ptr, (int)readBuffer.Length);
                }
                var slice = readBuffer.Slice(0, written);
                Assert.Equal(value, slice.GetUInt64());
            }
        }

        [Theory]
        [InlineData("abc,def,ghi", ',')]
        [InlineData("a;b;c;d", ';')]
        [InlineData("a;b;c;d", ',')]
        [InlineData("", ',')]
        public async Task Split(string input, char delimiter)
        {
            // note: different expectation to string.Split; empty has 0 outputs
            var expected = input == "" ? new string[0] : input.Split(delimiter);

            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
                output.AsOutput().Append(input, SymbolTable.InvariantUtf8);

                var readable = BufferUtilities.CreateBuffer(input);

                // via struct API
                var iter = readable.Split((byte)delimiter);
                Assert.Equal(expected.Length, iter.Count());
                int i = 0;
                foreach (var item in iter)
                {
                    Assert.Equal(expected[i++], item.GetUtf8String());
                }
                Assert.Equal(expected.Length, i);

                // via objects/LINQ etc
                IEnumerable<ReadableBuffer> asObject = iter;
                Assert.Equal(expected.Length, asObject.Count());
                i = 0;
                foreach (var item in asObject)
                {
                    Assert.Equal(expected[i++], item.GetUtf8String());
                }
                Assert.Equal(expected.Length, i);

                await output.FlushAsync();
            }
        }

        [Fact]
        public void ReadTWorksAgainstSimpleBuffers()
        {
            var readable = BufferUtilities.CreateBuffer(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            var span = readable.First.Span;
            Assert.True(readable.IsSingleSpan);
            Assert.Equal(span.Read<byte>(), readable.ReadLittleEndian<byte>());
            Assert.Equal(span.Read<sbyte>(), readable.ReadLittleEndian<sbyte>());
            Assert.Equal(span.Read<short>(), readable.ReadLittleEndian<short>());
            Assert.Equal(span.Read<ushort>(), readable.ReadLittleEndian<ushort>());
            Assert.Equal(span.Read<int>(), readable.ReadLittleEndian<int>());
            Assert.Equal(span.Read<uint>(), readable.ReadLittleEndian<uint>());
            Assert.Equal(span.Read<long>(), readable.ReadLittleEndian<long>());
            Assert.Equal(span.Read<ulong>(), readable.ReadLittleEndian<ulong>());
            Assert.Equal(span.Read<float>(), readable.ReadLittleEndian<float>());
            Assert.Equal(span.Read<double>(), readable.ReadLittleEndian<double>());
        }

        [Fact]
        public void ReadTWorksAgainstMultipleBuffers()
        {
            var readable = BufferUtilities.CreateBuffer(new byte[] { 0, 1, 2 }, new byte[] { 3, 4, 5 }, new byte[] { 6, 7, 9 });
            Assert.Equal(9, readable.Length);

            int spanCount = 0;
            foreach (var _ in readable)
            {
                spanCount++;
            }
            Assert.Equal(3, spanCount);

            Assert.False(readable.IsSingleSpan);
            Span<byte> span = readable.ToArray();

            Assert.Equal(span.Read<byte>(), readable.ReadLittleEndian<byte>());
            Assert.Equal(span.Read<sbyte>(), readable.ReadLittleEndian<sbyte>());
            Assert.Equal(span.Read<short>(), readable.ReadLittleEndian<short>());
            Assert.Equal(span.Read<ushort>(), readable.ReadLittleEndian<ushort>());
            Assert.Equal(span.Read<int>(), readable.ReadLittleEndian<int>());
            Assert.Equal(span.Read<uint>(), readable.ReadLittleEndian<uint>());
            Assert.Equal(span.Read<long>(), readable.ReadLittleEndian<long>());
            Assert.Equal(span.Read<ulong>(), readable.ReadLittleEndian<ulong>());
            Assert.Equal(span.Read<float>(), readable.ReadLittleEndian<float>());
            Assert.Equal(span.Read<double>(), readable.ReadLittleEndian<double>());
        }

        [Fact]
        public async Task CopyToAsync()
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
                output.AsOutput().Append("Hello World", SymbolTable.InvariantUtf8);
                await output.FlushAsync();
                var ms = new MemoryStream();
                var result = await readerWriter.Reader.ReadAsync();
                var rb = result.Buffer;
                await rb.CopyToAsync(ms);
                ms.Position = 0;
                Assert.Equal(11, rb.Length);
                Assert.Equal(11, ms.Length);
                Assert.Equal(rb.ToArray(), ms.ToArray());
                Assert.Equal("Hello World", Encoding.ASCII.GetString(ms.ToArray()));
            }
        }


    }
}
