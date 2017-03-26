﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class WritableBufferWriterFacts : IDisposable
    {
        private PipeFactory _pipeFactory;
        private IPipe _pipe;
        private WritableBuffer _buffer;

        public WritableBufferWriterFacts()
        {
            _pipeFactory = new PipeFactory();
            _pipe = _pipeFactory.Create();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory.Dispose();
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
            var writer = new WritableBufferWriter(_buffer);
            Assert.Equal(_buffer.Buffer.Length, writer.Span.Length);
            Assert.Equal(new byte[] { }, Read());
        }

        [Fact]
        public void SlicesSpanAndAdvancesAfterWrite()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var initialLength = _buffer.Buffer.Length;

            var writer = new WritableBufferWriter(_buffer);

            writer.Write(new byte[] { 1, 2, 3 });

            Assert.Equal(initialLength - 3, writer.Span.Length);
            Assert.Equal(_buffer.Buffer.Length, writer.Span.Length);
            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Fact]
        public void ThrowsForInvalidParameters()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var initialLength = _buffer.Buffer.Length;

            var writer = new WritableBufferWriter(_buffer);
            var array = new byte[] { 1, 2, 3 };

            writer.Write(array, 0, 0);
            writer.Write(array, array.Length, 0);

            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, 0, array.Length + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, array.Length + 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, -1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(array, array.Length + 1, array.Length + 1));

            writer.Write(array, 0, array.Length);

            Assert.Equal(array, Read());
        }

        [Fact]
        public void CanWriteIntoHeadlessBuffer()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new WritableBufferWriter(_buffer);

            writer.Write(new byte[] { 1, 2, 3 });
            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Fact]
        public void CanWriteMultipleTimes()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new WritableBufferWriter(_buffer);

            writer.Write(new byte[] { 1 });
            writer.Write(new byte[] { 2 });
            writer.Write(new byte[] { 3 });

            Assert.Equal(new byte[] { 1, 2, 3 }, Read());
        }

        [Fact]
        public void CanWriteEmpty()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new WritableBufferWriter(_buffer);
            var array = new byte[] { };

            writer.Write(array);
            writer.Write(array, 0, array.Length);

            Assert.Equal(array, Read());
        }

        [Fact]
        public void CanWriteOverTheBlockLength()
        {
            _buffer = _pipe.Writer.Alloc(1);
            var writer = new WritableBufferWriter(_buffer);

            var source = Enumerable.Range(0, _buffer.Buffer.Length).Select(i => (byte)i);
            var expectedBytes = source.Concat(source).Concat(source).ToArray();

            writer.Write(expectedBytes);

            Assert.Equal(expectedBytes, Read());
        }

        [Fact]
        public void EnsureAllocatesSpan()
        {
            _buffer = _pipe.Writer.Alloc();
            var writer = new WritableBufferWriter(_buffer);
            writer.Ensure(10);

            Assert.True(writer.Span.Length > 10);
            Assert.Equal(new byte[] {}, Read());
        }
    }
}
