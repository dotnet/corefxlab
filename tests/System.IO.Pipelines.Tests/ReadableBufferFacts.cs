// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Pools;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Globalization;
using System.IO;
using System.IO.Pipelines.Testing;
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
        public void EmptyIsCorrect()
        {
            var buffer = BufferUtilities.CreateBuffer(0, 0);
            Assert.Equal(0, buffer.Length);
            Assert.True(buffer.IsEmpty);
        }

        [Fact]
        public async Task TestIndexOfWorksForAllLocations()
        {
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                const int Size = 5 * 4032; // multiple blocks

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

            var handles = new List<MemoryHandle>();

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
                var handle = remaining.First.Pin();
                Assert.True(handle.PinnedPointer != null);
                Assert.True((byte*)handle.PinnedPointer == addresses[i]);
                handle.Free();
            }

            // free up memory handles
            foreach (var handle in handles)
            {
                handle.Free();
            }
            handles.Clear();
        }

        private static unsafe byte*[] BuildPointerIndex(ref ReadableBuffer readBuffer, List<MemoryHandle> handles)
        {

            byte*[] addresses = new byte*[readBuffer.Length];
            int index = 0;
            foreach (var memory in readBuffer)
            {
                var handle = memory.Pin();
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
                Assert.True(ptr != null);
                string s = value.ToString(CultureInfo.InvariantCulture);
                int written;
                fixed (char* c = s)
                {
                    written = Encoding.ASCII.GetBytes(c, s.Length, ptr, readBuffer.Length);
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
                output.Append(input, TextEncoder.Utf8);

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

                await output.FlushAsync();
            }
        }

        [Fact]
        public async Task ReadTWorksAgainstSimpleBuffers()
        {
            byte[] chunk = { 0, 1, 2, 3, 4, 5, 6, 7 };
            var span = new Span<byte>(chunk);

            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
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
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();

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
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
                output.Append("Hello World", TextEncoder.Utf8);
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
        public async Task CopyToAsyncNativeMemory()
        {
            using (var factory = new PipeFactory(NativeBufferPool.Shared))
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();
                output.Append("Hello World", TextEncoder.Utf8);
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
            using (var factory = new PipeFactory())
            {
                var readerWriter = factory.Create();
                var output = readerWriter.Writer.Alloc();

                {
                    // empty buffer
                    var readable = output.AsReadableBuffer() as ISequence<ReadOnlyMemory<byte>>;
                    var position = Position.First;
                    ReadOnlyMemory<byte> memory;
                    int spanCount = 0;
                    while (readable.TryGet(ref position, out memory))
                    {
                        spanCount++;
                        Assert.Equal(0, memory.Length);
                    }
                    Assert.Equal(1, spanCount);
                }

                {
                    var readable = BufferUtilities.CreateBuffer(new byte[] { 1 }, new byte[] { 2, 2 }, new byte[] { 3, 3, 3 }) as ISequence<ReadOnlyMemory<byte>>;
                    var position = Position.First;
                    ReadOnlyMemory<byte> memory;
                    int spanCount = 0;
                    while (readable.TryGet(ref position, out memory))
                    {
                        spanCount++;
                        Assert.Equal(spanCount, memory.Length);
                    }
                    Assert.Equal(3, spanCount);
                }
            }
        }

        [Theory]
        [MemberData(nameof(OutOfRangeSliceCases))]
        public void ReadableBufferDoesNotAllowSlicingOutOfRange(Action<ReadableBuffer> fail)
        {
            foreach (var p in Size100ReadableBuffers)
            {
                var buffer = (ReadableBuffer) p[0];
                var ex = Assert.Throws<InvalidOperationException>(() => fail(buffer));
            }
        }

        [Theory]
        [MemberData(nameof(Size100ReadableBuffers))]
        public void ReadableBufferMove_MovesReadCursor(ReadableBuffer buffer)
        {
            var cursor = buffer.Move(buffer.Start, 65);
            Assert.Equal(buffer.Slice(65).Start, cursor);
        }

        [Theory]
        [MemberData(nameof(Size100ReadableBuffers))]
        public void ReadableBufferMove_ChecksBounds(ReadableBuffer buffer)
        {
            Assert.Throws<InvalidOperationException>(() => buffer.Move(buffer.Start, 101));
        }

        [Fact]
        public void ReadableBufferMove_DoesNotAlowNegative()
        {
            var data = new byte[20];
            var buffer = ReadableBuffer.Create(data);
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.Move(buffer.Start, -1));
        }

        [Fact]
        public void ReadCursorSeekChecksEndIfNotTrustingEnd()
        {
            var buffer = BufferUtilities.CreateBuffer(1, 1, 1);
            var buffer2 = BufferUtilities.CreateBuffer(1, 1, 1);
            Assert.Throws<InvalidOperationException>(() => buffer.Start.Seek(2, buffer2.End, true));
        }

        [Fact]
        public void ReadCursorSeekDoesNotCheckEndIfTrustingEnd()
        {
            var buffer = BufferUtilities.CreateBuffer(1, 1, 1);
            var buffer2 = BufferUtilities.CreateBuffer(1, 1, 1);
            buffer.Start.Seek(2, buffer2.End, false);
        }

        public static TheoryData<ReadableBuffer> Size100ReadableBuffers
        {
            get
            {
                return new TheoryData<ReadableBuffer>()
                {
                    BufferUtilities.CreateBuffer(100),
                    BufferUtilities.CreateBuffer(50, 50),
                    BufferUtilities.CreateBuffer(33, 33, 34)
                };
            }
        }

        public static TheoryData<Action<ReadableBuffer>> OutOfRangeSliceCases
        {
            get
            {
                return new TheoryData<Action<ReadableBuffer>>()
                {
                    b => b.Slice(101),
                    b => b.Slice(0, 101),
                    b => b.Slice(b.Start, 101),
                    b => b.Slice(0, 70).Slice(b.End, b.End),
                    b => b.Slice(0, 70).Slice(b.Start, b.End),
                    b => b.Slice(0, 70).Slice(0, b.End),
                    b => b.Slice(70, b.Start)
                };
            }
        }
    }
}
