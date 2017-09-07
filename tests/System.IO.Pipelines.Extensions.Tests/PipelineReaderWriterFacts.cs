// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelineReaderWriterFacts : IDisposable
    {
        private IPipe _pipe;
        private PipeFactory _pipeFactory;

        public PipelineReaderWriterFacts()
        {
            _pipeFactory = new PipeFactory();
            _pipe = _pipeFactory.Create();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory?.Dispose();
        }


        [Fact]
        public async Task IndexOfNotFoundReturnsEnd()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");

            await _pipe.Writer.WriteAsync(bytes);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            ReadableBuffer slice;
            ReadCursor cursor;

            Assert.False(buffer.TrySliceTo(10, out slice, out cursor));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task FastPathIndexOfAcrossBlocks()
        {
            const int blockSize = 4032;
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var writeBuffer = _pipe.Writer.Alloc();
            writeBuffer.Write(paddingBytes);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            ReadableBuffer slice;
            ReadCursor cursor;
            Assert.False(buffer.TrySliceTo((byte)'R', out slice, out cursor));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task SlowPathIndexOfAcrossBlocks()
        {
            const int blockSize = 4032;
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var writeBuffer = _pipe.Writer.Alloc();
            writeBuffer.Write(paddingBytes);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            ReadableBuffer slice;
            ReadCursor cursor;
            Assert.False(buffer.IsSingleSpan);
            Assert.True(buffer.TrySliceTo((byte)' ', out slice, out cursor));

            slice = buffer.Slice(cursor).Slice(1);
            var array = slice.ToArray();

            Assert.Equal("World", Encoding.ASCII.GetString(array));

            _pipe.Reader.Advance(buffer.Start, buffer.Start);
        }


    }
}
