// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Globalization;
using System.IO.Pipelines.Testing;
using System.IO.Pipelines.Text.Primitives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Formatting;
using System.Threading.Tasks;
using Xunit;

using static System.Buffers.Binary.BinaryPrimitives;

namespace System.IO.Pipelines.Tests
{
    public class ReadableBufferFacts: IDisposable
    {
        const int BlockSize = 4032;

        private IPipe _pipe;
        private MemoryPool<byte> _pool;

        public ReadableBufferFacts()
        {
            _pool = new MemoryPool();
            _pipe = new Pipe(new PipeOptions(_pool));
        }
        public void Dispose()
        {
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        [Fact]
        public void ReadableBufferSequenceWorks()
        {
            var readable = BufferUtilities.CreateBuffer(new byte[] { 1 }, new byte[] { 2, 2 }, new byte[] { 3, 3, 3 });
            Position position = readable.Start;
            int spanCount = 0;
            while (readable.TryGet(ref position, out ReadOnlyMemory<byte> memory))
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
            var buffer = new ReadOnlyBuffer<byte>(data, 3, data.Length - 7);
            Assert.Equal(13, buffer.Length);
            var split = buffer.Split((byte)'|');
            Assert.Equal(3, split.Count());
            using (var iter = split.GetEnumerator())
            {
                Assert.True(iter.MoveNext());
                var current = iter.Current;
                Assert.Equal("abc", current.GetAsciiString());

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("def", current.GetAsciiString());

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("ghijk", current.GetAsciiString());

                Assert.False(iter.MoveNext());
            }
        }

        [Fact]
        public void CanUseOwnedBufferBasedReadableBuffers()
        {
            var data = Encoding.ASCII.GetBytes("***abc|def|ghijk****"); // note sthe padding here - verifying that it is omitted correctly
            var buffer = new ReadOnlyBuffer<byte>(data, 3, data.Length - 7);
            Assert.Equal(13, buffer.Length);
            var split = buffer.Split((byte)'|');
            Assert.Equal(3, split.Count());
            using (var iter = split.GetEnumerator())
            {
                Assert.True(iter.MoveNext());
                var current = iter.Current;
                Assert.Equal("abc", current.GetAsciiString());

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("def", current.GetAsciiString());

                Assert.True(iter.MoveNext());
                current = iter.Current;
                Assert.Equal("ghijk", current.GetAsciiString());

                Assert.False(iter.MoveNext());
            }
        }

        [Fact]
        public async Task CopyToAsyncNativeMemory()
        {
            var output = _pipe.Writer.Alloc();
            output.Append("Hello World", SymbolTable.InvariantUtf8);
            await output.FlushAsync();
            var ms = new MemoryStream();
            var result = await _pipe.Reader.ReadAsync();
            var rb = result.Buffer;
            await rb.CopyToAsync(ms);
            ms.Position = 0;
            Assert.Equal(11, rb.Length);
            Assert.Equal(11, ms.Length);
            Assert.Equal(rb.ToArray(), ms.ToArray());
            Assert.Equal("Hello World", Encoding.ASCII.GetString(ms.ToArray()));
            _pipe.Reader.Advance(rb.End);
        }

        [Fact]
        public async Task TestIndexOfWorksForAllLocations()
        {
            const int Size = 5 * BlockSize; // multiple blocks

            // populate with a pile of dummy data
            byte[] data = new byte[512];
            for (int i = 0; i < data.Length; i++) data[i] = 42;
            int totalBytes = 0;
            var writeBuffer = _pipe.Writer.Alloc();
            for (int i = 0; i < Size / data.Length; i++)
            {
                writeBuffer.Write(data);
                totalBytes += data.Length;
            }
            await writeBuffer.FlushAsync();

            // now read it back
            var result = await _pipe.Reader.ReadAsync();
            var readBuffer = result.Buffer;
            Assert.False(readBuffer.IsSingleSegment);
            Assert.Equal(totalBytes, readBuffer.Length);
            TestIndexOfWorksForAllLocations(ref readBuffer, 42);
            _pipe.Reader.Advance(readBuffer.End);
        }

        [Fact]
        public async Task EqualsDetectsDeltaForAllLocations()
        {
            // populate with dummy data
            const int DataSize = 10000;
            byte[] data = new byte[DataSize];
            var rand = new Random(12345);
            rand.NextBytes(data);

            var writeBuffer = _pipe.Writer.Alloc();
            writeBuffer.Write(data);
            await writeBuffer.FlushAsync();

            // now read it back
            var result = await _pipe.Reader.ReadAsync();
            var readBuffer = result.Buffer;
            Assert.False(readBuffer.IsSingleSegment);
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
            _pipe.Reader.Advance(readBuffer.End);
        }

        private void EqualsDetectsDeltaForAllLocations(ReadOnlyBuffer<byte> slice, byte[] expected, int offset, int length)
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
            var writeBuffer = _pipe.Writer.Alloc();
            writeBuffer.Ensure(50);
            writeBuffer.Advance(50); // not even going to pretend to write data here - we're going to cheat
            await writeBuffer.FlushAsync(); // by overwriting the buffer in-situ

            // now read it back
            var result = await _pipe.Reader.ReadAsync();
            var readBuffer = result.Buffer;

            ReadUInt64GivesExpectedValues(ref readBuffer);

            _pipe.Reader.Advance(readBuffer.End);
        }

        [Theory]
        [InlineData(" hello", "hello")]
        [InlineData("    hello", "hello")]
        [InlineData("\r\n hello", "hello")]
        [InlineData("\rhe  llo", "he  llo")]
        [InlineData("\thell o ", "hell o ")]
        public async Task TrimStartTrimsWhitespaceAtStart(string input, string expected)
        {
            var writeBuffer = _pipe.Writer.Alloc();
            var bytes = Encoding.ASCII.GetBytes(input);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            var trimmed = buffer.TrimStart();
            var outputBytes = trimmed.ToArray();

            Assert.Equal(expected, Encoding.ASCII.GetString(outputBytes));

            _pipe.Reader.Advance(buffer.End);
        }

        [Theory]
        [InlineData("hello ", "hello")]
        [InlineData("hello    ", "hello")]
        [InlineData("hello \r\n", "hello")]
        [InlineData("he  llo\r", "he  llo")]
        [InlineData(" hell o\t", " hell o")]
        public async Task TrimEndTrimsWhitespaceAtEnd(string input, string expected)
        {
            var writeBuffer = _pipe.Writer.Alloc();
            var bytes = Encoding.ASCII.GetBytes(input);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            var trimmed = buffer.TrimEnd();
            var outputBytes = trimmed.ToArray();
            _pipe.Reader.Advance(buffer.End);

            Assert.Equal(expected, Encoding.ASCII.GetString(outputBytes));
        }

        [Theory]
        [InlineData("foo\rbar\r\n", "\r\n", "foo\rbar")]
        [InlineData("foo\rbar\r\n", "\rbar", "foo")]
        [InlineData("/pathpath/", "path/", "/path")]
        [InlineData("hellzhello", "hell", null)]
        public async Task TrySliceToSpan(string input, string sliceTo, string expected)
        {
            var sliceToBytes = Encoding.UTF8.GetBytes(sliceTo);

            var writeBuffer = _pipe.Writer.Alloc();
            var bytes = Encoding.UTF8.GetBytes(input);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Assert.True(buffer.TrySliceTo(sliceToBytes, out ReadOnlyBuffer<byte> slice, out Position cursor));
            Assert.Equal(expected, slice.GetUtf8Span());

            _pipe.Reader.Advance(buffer.End);
        }

        private unsafe void TestIndexOfWorksForAllLocations(ref ReadOnlyBuffer<byte> readBuffer, byte emptyValue)
        {
            byte huntValue = (byte)~emptyValue;

            var handles = new List<MemoryHandle>();
            // we're going to fully index the final locations of the buffer, so that we
            // can mutate etc in constant time
            var addresses = BuildPointerIndex(ref readBuffer, handles);
            var found = readBuffer.TrySliceTo(huntValue, out ReadOnlyBuffer<byte> slice, out Position cursor);
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
                Assert.True(handle.Pointer != null);
                if (i % BlockSize == 0)
                {
                    Assert.True((byte*)handle.Pointer == addresses[i]);
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

        private static unsafe byte*[] BuildPointerIndex(ref ReadOnlyBuffer<byte> readBuffer, List<MemoryHandle> handles)
        {

            byte*[] addresses = new byte*[readBuffer.Length];
            int index = 0;
            foreach (var memory in readBuffer)
            {
                var handle = memory.Retain(pin: true);
                handles.Add(handle);
                var ptr = (byte*)handle.Pointer;
                for (int i = 0; i < memory.Length; i++)
                {
                    addresses[index++] = ptr++;
                }
            }
            return addresses;
        }

        private unsafe void ReadUInt64GivesExpectedValues(ref ReadOnlyBuffer<byte> readBuffer)
        {
            Assert.True(readBuffer.IsSingleSegment);

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

        private unsafe void TestValue(ref ReadOnlyBuffer<byte> readBuffer, ulong value)
        {
            fixed (byte* ptr = &MemoryMarshal.GetReference(readBuffer.First.Span))
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
            var output = _pipe.Writer.Alloc();
            output.Append(input, SymbolTable.InvariantUtf8);

            var readable = BufferUtilities.CreateBuffer(input);

            // via struct API
            var iter = readable.Split((byte)delimiter);
            Assert.Equal(expected.Length, iter.Count());
            int i = 0;
            foreach (var item in iter)
            {
                Assert.Equal(expected[i++], item.GetUtf8Span());
            }
            Assert.Equal(expected.Length, i);

            // via objects/LINQ etc
            IEnumerable<ReadOnlyBuffer<byte>> asObject = iter;
            Assert.Equal(expected.Length, asObject.Count());
            i = 0;
            foreach (var item in asObject)
            {
                Assert.Equal(expected[i++], item.GetUtf8Span());
            }
            Assert.Equal(expected.Length, i);

            await output.FlushAsync();
        }

        [Fact]
        public void ReadTWorksAgainstSimpleBuffers()
        {
            var readable = BufferUtilities.CreateBuffer(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            var span = readable.First.Span;
            Assert.True(readable.IsSingleSegment);
            Assert.Equal(ReadMachineEndian<byte>(span), readable.ReadLittleEndian<byte>());
            Assert.Equal(ReadMachineEndian<sbyte>(span), readable.ReadLittleEndian<sbyte>());
            Assert.Equal(ReadMachineEndian<short>(span), readable.ReadLittleEndian<short>());
            Assert.Equal(ReadMachineEndian<ushort>(span), readable.ReadLittleEndian<ushort>());
            Assert.Equal(ReadMachineEndian<int>(span), readable.ReadLittleEndian<int>());
            Assert.Equal(ReadMachineEndian<uint>(span), readable.ReadLittleEndian<uint>());
            Assert.Equal(ReadMachineEndian<long>(span), readable.ReadLittleEndian<long>());
            Assert.Equal(ReadMachineEndian<ulong>(span), readable.ReadLittleEndian<ulong>());
            Assert.Equal(ReadMachineEndian<float>(span), readable.ReadLittleEndian<float>());
            Assert.Equal(ReadMachineEndian<double>(span), readable.ReadLittleEndian<double>());
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

            Assert.False(readable.IsSingleSegment);
            Span<byte> span = readable.ToArray();

            Assert.Equal(ReadMachineEndian<byte>(span), readable.ReadLittleEndian<byte>());
            Assert.Equal(ReadMachineEndian<sbyte>(span), readable.ReadLittleEndian<sbyte>());
            Assert.Equal(ReadMachineEndian<short>(span), readable.ReadLittleEndian<short>());
            Assert.Equal(ReadMachineEndian<ushort>(span), readable.ReadLittleEndian<ushort>());
            Assert.Equal(ReadMachineEndian<int>(span), readable.ReadLittleEndian<int>());
            Assert.Equal(ReadMachineEndian<uint>(span), readable.ReadLittleEndian<uint>());
            Assert.Equal(ReadMachineEndian<long>(span), readable.ReadLittleEndian<long>());
            Assert.Equal(ReadMachineEndian<ulong>(span), readable.ReadLittleEndian<ulong>());
            Assert.Equal(ReadMachineEndian<float>(span), readable.ReadLittleEndian<float>());
            Assert.Equal(ReadMachineEndian<double>(span), readable.ReadLittleEndian<double>());
        }

        [Fact]
        public async Task CopyToAsync()
        {
            using (var pool = new MemoryPool())
            {
                var readerWriter = new Pipe(new PipeOptions(pool));
                var output = readerWriter.Writer.Alloc();
                output.Append("Hello World", SymbolTable.InvariantUtf8);
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
