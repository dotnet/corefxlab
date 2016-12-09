﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Xunit;
using System.Text.Formatting;

namespace System.IO.Pipelines.Tests
{
    public class ReadableBufferFacts
    {
        [Fact]
        public async Task TestIndexOfWorksForAllLocations()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                const int Size = 5 * 4032; // multiple blocks

                // populate with a pile of dummy data
                byte[] data = new byte[512];
                for (int i = 0; i < data.Length; i++) data[i] = 42;
                int totalBytes = 0;
                var writeBuffer = readerWriter.Alloc();
                for (int i = 0; i < Size / data.Length; i++)
                {
                    writeBuffer.Write(data);
                    totalBytes += data.Length;
                }
                await writeBuffer.FlushAsync();

                // now read it back
                var result = await readerWriter.ReadAsync();
                var readBuffer = result.Buffer;
                Assert.False(readBuffer.IsSingleSpan);
                Assert.Equal(totalBytes, readBuffer.Length);
                TestIndexOfWorksForAllLocations(ref readBuffer, 42);
            }
        }

        [Fact]
        public async Task EqualsDetectsDeltaForAllLocations()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                // populate with dummy data
                const int DataSize = 10000;
                byte[] data = new byte[DataSize];
                var rand = new Random(12345);
                rand.NextBytes(data);

                var writeBuffer = readerWriter.Alloc();
                writeBuffer.Write(data);
                await writeBuffer.FlushAsync();

                // now read it back
                var result = await readerWriter.ReadAsync();
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
            Assert.True(slice.Equals(new Span<byte>(expected, offset, length)));
            // change one byte in buffer, for every position
            for (int i = 0; i < length; i++)
            {
                expected[offset + i] ^= 42;
                Assert.False(slice.Equals(new Span<byte>(expected, offset, length)));
                expected[offset + i] ^= 42;
            }
        }

        [Fact]
        public async Task GetUInt64GivesExpectedValues()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Alloc();
                writeBuffer.Ensure(50);
                writeBuffer.Advance(50); // not even going to pretend to write data here - we're going to cheat
                await writeBuffer.FlushAsync(); // by overwriting the buffer in-situ

                // now read it back
                var result = await readerWriter.ReadAsync();
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
            using (var readerWriter = new PipelineFactory())
            {
                var connection = readerWriter.Create();

                var writeBuffer = connection.Alloc();
                var bytes = Encoding.ASCII.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await connection.ReadAsync();
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
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Alloc();
                var bytes = Encoding.ASCII.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
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

            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();

                var writeBuffer = readerWriter.Alloc();
                var bytes = Encoding.UTF8.GetBytes(input);
                writeBuffer.Write(bytes);
                await writeBuffer.FlushAsync();

                var result = await readerWriter.ReadAsync();
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

            // we're going to fully index the final locations of the buffer, so that we
            // can mutate etc in constant time
            var addresses = BuildPointerIndex(ref readBuffer);

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
                void* pointer;
                Assert.True(remaining.First.TryGetPointer(out pointer));
                Assert.True((byte*)pointer == addresses[i]);
            }
        }

        private static unsafe byte*[] BuildPointerIndex(ref ReadableBuffer readBuffer)
        {

            byte*[] addresses = new byte*[readBuffer.Length];
            int index = 0;
            foreach (var memory in readBuffer)
            {
                void* pointer;
                memory.TryGetPointer(out pointer);
                var ptr = (byte*)pointer;
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
            void* pointer;
            Assert.True(readBuffer.First.TryGetPointer(out pointer));
            var ptr = (byte*)pointer;
            string s = value.ToString(CultureInfo.InvariantCulture);
            int written;
            fixed (char* c = s)
            {
                written = Encoding.ASCII.GetBytes(c, s.Length, ptr, readBuffer.Length);
            }
            var slice = readBuffer.Slice(0, written);
            Assert.Equal(value, slice.GetUInt64());
        }

        [Theory]
        [InlineData("abc,def,ghi", ',')]
        [InlineData("a;b;c;d", ';')]
        [InlineData("a;b;c;d", ',')]
        [InlineData("", ',')]
        public Task Split(string input, char delimiter)
        {
            // note: different expectation to string.Split; empty has 0 outputs
            var expected = input == "" ? new string[0] : input.Split(delimiter);

            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();
                output.Append(input, TextEncoding.Utf8);

                var readable = output.AsReadableBuffer();

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

                return output.FlushAsync();
            }
        }

        [Fact]
        public async Task ReadTWorksAgainstSimpleBuffers()
        {
            byte[] chunk = { 0, 1, 2, 3, 4, 5, 6, 7 };
            var span = new Span<byte>(chunk);

            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();
                output.Write(span);
                var readable = output.AsReadableBuffer();
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
                await output.FlushAsync();
            }
        }

        [Fact]
        public async Task ReadTWorksAgainstMultipleBuffers()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();

                // we're going to try to force 3 buffers for 8 bytes
                output.Write(new byte[] { 0, 1, 2 });
                output.Ensure(4031);
                output.Write(new byte[] { 3, 4, 5 });
                output.Ensure(4031);
                output.Write(new byte[] { 6, 7, 9 });

                var readable = output.AsReadableBuffer();
                Assert.Equal(9, readable.Length);

                int spanCount = 0;
                foreach (var _ in readable)
                {
                    spanCount++;
                }
                Assert.Equal(3, spanCount);

                byte[] local = new byte[9];
                readable.CopyTo(local);
                var span = new Span<byte>(local);

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
                await output.FlushAsync();
            }
        }

        [Fact]
        public async Task CopyToAsync()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();
                output.Append("Hello World", TextEncoding.Utf8);
                await output.FlushAsync();
                var ms = new MemoryStream();
                var result = await readerWriter.ReadAsync();
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
        public async Task CopyToAsyncNativeMemory()
        {
            using (var pool = new NativePool())
            using (var factory = new PipelineFactory(pool))
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();
                output.Append("Hello World", TextEncoding.Utf8);
                await output.FlushAsync();
                var ms = new MemoryStream();
                var result = await readerWriter.ReadAsync();
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
        public void ReadableBufferSequenceWorks()
        {
            using (var factory = new PipelineFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Alloc();

                {
                    // empty buffer
                    var readable = output.AsReadableBuffer() as ISequence<ReadOnlyMemory<byte>>;
                    var position = Position.First;
                    ReadOnlyMemory<byte> memory;
                    int spanCount = 0;
                    while (readable.TryGet(ref position, out memory, advance: true))
                    {
                        spanCount++;
                        Assert.Equal(0, memory.Length);
                    }
                    Assert.Equal(1, spanCount);
                }

                { // 3 segment buffer
                    output.Write(new byte[] { 1 });
                    output.Ensure(4032);
                    output.Write(new byte[] { 2, 2 });
                    output.Ensure(4031);
                    output.Write(new byte[] { 3, 3, 3 });

                    var readable = output.AsReadableBuffer() as ISequence<ReadOnlyMemory<byte>>;
                    var position = Position.First;
                    ReadOnlyMemory<byte> memory;
                    int spanCount = 0;
                    while (readable.TryGet(ref position, out memory, advance: true))
                    {
                        spanCount++;
                        Assert.Equal(spanCount, memory.Length);
                    }
                    Assert.Equal(3, spanCount);
                }
            }
        }

        private class NativePool : IBufferPool
        {
            public void Dispose()
            {

            }

            public OwnedMemory<byte> Lease(int size)
            {
                return NativeBufferPool.Shared.Rent(size);
            }
        }
    }
}
