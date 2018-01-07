// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Buffers;
using System.Linq;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class WritableBufferWriterFacts : IDisposable
    {
        private WritableBuffer _buffer;
        private MemoryPool _pool;
        private Pipe _pipe;

        public WritableBufferWriterFacts()
        {
            _pool = new MemoryPool();
            _pipe = new Pipe(new PipeOptions(_pool));
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        private byte[] Read()
        {
             _buffer.FlushAsync().GetAwaiter().GetResult();
            var readResult = _pipe.Reader.ReadAsync().GetAwaiter().GetResult();
            var data = readResult.Buffer.ToArray();
            _pipe.Reader.Advance(readResult.Buffer.End);
            return data;
        }

        [Fact]
        public void ExposesSpan()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var writer = new OutputWriter<>(_buffer);
            Assert.Equal(_buffer.Buffer.Length, writer.Span.Length);
            Assert.Equal(new byte[] { }, Read());
        }

        [Fact]
        public void SlicesSpanAndAdvancesAfterWrite()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var initialLength = _buffer.Buffer.Length;

            var writer = new OutputWriter<>(_buffer);

            writer.Write(new byte[] { 1, 2, 3 });

            Assert.Equal(initialLength - 3, writer.Span.Length);
            Assert.Equal(_buffer.Buffer.Length, writer.Span.Length);
            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Theory]
        [InlineData(3, -1, 0)]
        [InlineData(3, 0, -1)]
        [InlineData(3, 0, 4)]
        [InlineData(3, 4, 0)]
        [InlineData(3, -1, -1)]
        [InlineData(3, 4, 4)]
        public void ThrowsForInvalidParameters(int arrayLength, int offset, int length)
        {
            _buffer = _pipe.Writer.Alloc(1);
            var initialLength = _buffer.Buffer.Length;

            var writer = new OutputWriter<>(_buffer);
            var array = new byte[arrayLength];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(i + 1);
            }

            writer.Write(array, 0, 0);
            writer.Write(array, array.Length, 0);

            try
            {
                writer.Write(array, offset, length);
                Assert.True(false);
            }
            catch(Exception ex)
            {
                Assert.True(ex is ArgumentOutOfRangeException);
            }

            writer.Write(array, 0, array.Length);
            Assert.Equal(array, Read());
        }

        [Theory]
        [InlineData(0, 0, 3)]
        [InlineData(0, 1, 2)]
        [InlineData(0, 2, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 3)]
        [InlineData(1, 1, 2)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 1, 1)]
        public void CanWriteWithOffsetAndLenght(int alloc, int offset, int length)
        {
            _buffer = _pipe.Writer.Alloc(alloc);

            var writer = new OutputWriter<>(_buffer);
            var array = new byte[] { 1, 2, 3 };

            writer.Write(array, offset, length);

            Assert.Equal(array.Skip(offset).Take(length).ToArray(), Read());
        }

        [Fact]
        public void CanWriteIntoHeadlessBuffer()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new OutputWriter<>(_buffer);

            writer.Write(new byte[] { 1, 2, 3 });
            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Fact]
        public void CanWriteMultipleTimes()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new OutputWriter<>(_buffer);

            writer.Write(new byte[] { 1 });
            writer.Write(new byte[] { 2 });
            writer.Write(new byte[] { 3 });

            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Fact]
        public void CanWriteEmpty()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new OutputWriter<>(_buffer);
            var array = new byte[] { };

            writer.Write(array);
            writer.Write(array, 0, array.Length);

            Assert.Equal(array, Read());
        }

        [Fact]
        public void CanWriteOverTheBlockLength()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var writer = new OutputWriter<>(_buffer);

            var source = Enumerable.Range(0, _buffer.Buffer.Length).Select(i => (byte)i);
            var expectedBytes = source.Concat(source).Concat(source).ToArray();

            writer.Write(expectedBytes);

            Assert.Equal(expectedBytes, Read());
        }

        [Fact]
        public void EnsureAllocatesSpan()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new OutputWriter<>(_buffer);
            writer.Ensure(10);

            Assert.True(writer.Span.Length > 10);
            Assert.Equal(new byte[] {}, Read());
        }
    }
}
